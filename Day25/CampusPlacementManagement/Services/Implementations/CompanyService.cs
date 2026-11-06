
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.DTOs.CompanyHRs;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyService(
            ApplicationDbContext context,
            ICompanyRepository companyRepository,
            ICompanyHRRepository companyHRRepository,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _companyRepository = companyRepository;
            _companyHRRepository = companyHRRepository;
            _userManager = userManager;
        }

        // Register a company and its initial HR account.
        // Both are created in one database transaction.
        public async Task<CompanyResponseDto> RegisterAsync(
            CompanyRegistrationDto dto)
        {
            var companyCode = dto.CompanyCode
                .Trim()
                .ToUpperInvariant();

            var officialEmail = dto.OfficialEmail.Trim();

            var hrEmail = dto.HR.Email.Trim();

            // Check duplicate company code.
            var existingCompany = await _companyRepository
                .GetByCodeAsync(companyCode);

            if (existingCompany != null)
            {
                throw new InvalidOperationException(
                    "A company with this company code already exists.");
            }

            // Check duplicate official email.
            existingCompany = await _companyRepository
                .GetByOfficialEmailAsync(officialEmail);

            if (existingCompany != null)
            {
                throw new InvalidOperationException(
                    "A company with this official email already exists.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Create company.
                var company = new Company
                {
                    CompanyName = dto.CompanyName.Trim(),
                    CompanyCode = companyCode,
                    Industry = dto.Industry.Trim(),
                    OfficialEmail = officialEmail,
                    PhoneNumber = dto.PhoneNumber.Trim(),
                    Address = dto.Address.Trim(),
                    Website = dto.Website?.Trim(),
                    Description = dto.Description?.Trim(),
                    Status = CompanyStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _companyRepository.AddAsync(company);

                await _companyRepository.SaveChangesAsync();

                // Create initial Company HR Identity account.
                var hrUser = new ApplicationUser
                {
                    UserName = hrEmail,
                    Email = hrEmail,
                    PhoneNumber = dto.HR.PhoneNumber.Trim(),
                    FullName = dto.HR.FullName.Trim(),
                    CompanyId = company.CompanyId,
                    AccountStatus = AccountStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(
                    hrUser,
                    dto.HR.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Company HR account creation failed: {errors}");
                }

                // Assign CompanyHR role.
                var roleResult = await _userManager.AddToRoleAsync(
                    hrUser,
                    AppRoles.CompanyHR);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Assigning the CompanyHR role failed: {errors}");
                }

                // Create CompanyHR profile.
                var companyHR = new CompanyHR
                {
                    UserId = hrUser.Id,
                    CompanyId = company.CompanyId,
                    Designation = dto.HR.Designation.Trim(),
                    CreatedAt = DateTime.UtcNow,

                    User = hrUser,
                    Company = company
                };

                await _companyHRRepository.AddAsync(companyHR);

                await _companyHRRepository.SaveChangesAsync();

                // Keep the company navigation collection updated.
                company.HRAccounts.Add(hrUser);

                await transaction.CommitAsync();

                return MapCompany(company);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Retrieve all companies.
        public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();

            return companies.Select(MapCompany);
        }

        // Retrieve a company by ID.
        public async Task<CompanyResponseDto?> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            return company == null
                ? null
                : MapCompany(company);
        }

        // Retrieve pending companies.
        public async Task<IEnumerable<CompanyResponseDto>> GetPendingAsync()
        {
            var companies = await _companyRepository.GetPendingAsync();

            return companies.Select(MapCompany);
        }

        // Approve a pending company.
        public async Task<bool> ApproveAsync(int id)
        {
            return await UpdateCompanyStatusAsync(
                id,
                CompanyStatus.Pending,
                CompanyStatus.Approved,
                AccountStatus.Approved);
        }

        // Reject a pending company.
        public async Task<bool> RejectAsync(int id)
        {
            return await UpdateCompanyStatusAsync(
                id,
                CompanyStatus.Pending,
                CompanyStatus.Rejected,
                AccountStatus.Rejected);
        }

        // Suspend an approved company.
        public async Task<bool> SuspendAsync(int id)
        {
            return await UpdateCompanyStatusAsync(
                id,
                CompanyStatus.Approved,
                CompanyStatus.Suspended,
                AccountStatus.Suspended);
        }

        // Update company status and all associated HR accounts.
        private async Task<bool> UpdateCompanyStatusAsync(
            int id,
            CompanyStatus expectedStatus,
            CompanyStatus newStatus,
            AccountStatus accountStatus)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if (company == null)
            {
                return false;
            }

            if (company.Status != expectedStatus)
            {
                throw new InvalidOperationException(
                    $"Cannot change company status from " +
                    $"{company.Status} to {newStatus}.");
            }

            company.Status = newStatus;
            company.UpdatedAt = DateTime.UtcNow;

            var hrUsers = await _context.Users
                .Where(u => u.CompanyId == id)
                .ToListAsync();

            foreach (var user in hrUsers)
            {
                user.AccountStatus = accountStatus;
                user.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        // Register another HR account for the current company.
        // The company ID is derived from the authenticated HR profile.
        public async Task<CompanyHRResponseDto> RegisterHRAsync(
            string currentUserId,
            CompanyHRRegistrationDto dto)
        {
            var currentHR = await _companyHRRepository
                .GetByUserIdAsync(currentUserId);

            if (currentHR == null)
            {
                throw new InvalidOperationException(
                    "The current user is not associated with a company HR profile.");
            }

            // Only approved HR accounts can register additional HR accounts.
            if (currentHR.User.AccountStatus != AccountStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Only an approved Company HR account can register additional HR accounts.");
            }

            var company = await _companyRepository
                .GetByIdAsync(currentHR.CompanyId);

            if (company == null ||
                company.Status != CompanyStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Only an approved company can register additional HR accounts.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Create new HR Identity account.
                var newUser = new ApplicationUser
                {
                    UserName = dto.Email.Trim(),
                    Email = dto.Email.Trim(),
                    PhoneNumber = dto.PhoneNumber.Trim(),
                    FullName = dto.FullName.Trim(),
                    CompanyId = currentHR.CompanyId,
                    AccountStatus = AccountStatus.Approved,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(
                    newUser,
                    dto.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Company HR account creation failed: {errors}");
                }

                // Assign CompanyHR role.
                var roleResult = await _userManager.AddToRoleAsync(
                    newUser,
                    AppRoles.CompanyHR);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Assigning the CompanyHR role failed: {errors}");
                }

                // Create HR profile.
                var newHR = new CompanyHR
                {
                    UserId = newUser.Id,
                    CompanyId = currentHR.CompanyId,
                    Designation = dto.Designation.Trim(),
                    CreatedAt = DateTime.UtcNow,

                    User = newUser,
                    Company = company
                };

                await _companyHRRepository.AddAsync(newHR);

                await _companyHRRepository.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapCompanyHR(newHR);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Map Company entity to response DTO.
        private static CompanyResponseDto MapCompany(
            Company company)
        {
            return new CompanyResponseDto
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                CompanyCode = company.CompanyCode,
                Industry = company.Industry,
                OfficialEmail = company.OfficialEmail,
                PhoneNumber = company.PhoneNumber,
                Address = company.Address,
                Website = company.Website,
                Description = company.Description,
                Status = company.Status.ToString(),
                CreatedAt = company.CreatedAt,
                HRCount = company.HRAccounts.Count
            };
        }

        // Map CompanyHR entity to response DTO.
        private static CompanyHRResponseDto MapCompanyHR(
            CompanyHR companyHR)
        {
            return new CompanyHRResponseDto
            {
                CompanyHRId = companyHR.CompanyHRId,
                UserId = companyHR.UserId,
                FullName = companyHR.User.FullName,
                Email = companyHR.User.Email ?? string.Empty,
                PhoneNumber = companyHR.User.PhoneNumber ?? string.Empty,
                CompanyId = companyHR.CompanyId,
                CompanyName = companyHR.Company.CompanyName,
                Designation = companyHR.Designation,
                CreatedAt = companyHR.CreatedAt
            };
        }
    }
}
