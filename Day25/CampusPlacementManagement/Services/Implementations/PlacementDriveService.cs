
using CampusPlacementManagement.DTOs.PlacementDrives;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class PlacementDriveService : IPlacementDriveService
    {
        private readonly IPlacementDriveRepository _driveRepository;
        private readonly IPlacementOfficerRepository _officerRepository;
        private readonly ICompanyRepository _companyRepository;

        public PlacementDriveService(
            IPlacementDriveRepository driveRepository,
            IPlacementOfficerRepository officerRepository,
            ICompanyRepository companyRepository)
        {
            _driveRepository = driveRepository;
            _officerRepository = officerRepository;
            _companyRepository = companyRepository;
        }

        public async Task<PlacementDriveResponseDto> CreateAsync(
            string placementOfficerUserId,
            PlacementDriveCreateDto dto)
        {
            var officer = await _officerRepository.GetByUserIdAsync(
                placementOfficerUserId);

            if (officer == null)
                throw new InvalidOperationException(
                    "Placement Officer profile not found.");

            if (officer.User.AccountStatus != AccountStatus.Approved)
                throw new InvalidOperationException(
                    "Placement Officer account is not approved.");

            if (officer.College.Status != CollegeStatus.Approved)
                throw new InvalidOperationException(
                    "College is not approved.");

            var company = await _companyRepository.GetByIdAsync(dto.CompanyId);

            if (company == null)
                throw new InvalidOperationException(
                    "Company not found.");

            if (company.Status != CompanyStatus.Approved)
                throw new InvalidOperationException(
                    "Only approved companies can participate in placement drives.");

            ValidateDriveDetails(
                dto.StartDate,
                dto.EndDate,
                dto.RegistrationDeadline,
                dto.MinimumCGPA,
                dto.MaximumActiveBacklogs,
                dto.GraduationYearFrom,
                dto.GraduationYearTo);

            var drive = new PlacementDrive
            {
                CollegeId = officer.CollegeId,
                CompanyId = company.CompanyId,
                PlacementOfficerId = officer.PlacementOfficerId,

                DriveName = dto.DriveName.Trim(),
                Description = dto.Description.Trim(),
                Venue = dto.Venue.Trim(),

                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                RegistrationDeadline = dto.RegistrationDeadline,

                MinimumCGPA = dto.MinimumCGPA,
                MaximumActiveBacklogs = dto.MaximumActiveBacklogs,

                EligibleDepartments = dto.EligibleDepartments.Trim(),
                EligibleDegrees = dto.EligibleDegrees.Trim(),

                GraduationYearFrom = dto.GraduationYearFrom,
                GraduationYearTo = dto.GraduationYearTo,

                Status = PlacementDriveStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            await _driveRepository.AddAsync(drive);
            await _driveRepository.SaveChangesAsync();

            var savedDrive = await _driveRepository.GetByIdAsync(
                drive.PlacementDriveId);

            return MapToDto(savedDrive!);
        }

        public async Task<PlacementDriveResponseDto> UpdateAsync(
            int driveId,
            string placementOfficerUserId,
            PlacementDriveUpdateDto dto)
        {
            var drive = await _driveRepository.GetByIdAsync(driveId);

            if (drive == null)
                throw new KeyNotFoundException(
                    "Placement drive not found.");

            var officer = await _officerRepository.GetByUserIdAsync(
                placementOfficerUserId);

            if (officer == null)
                throw new InvalidOperationException(
                    "Placement Officer profile not found.");

            if (drive.CollegeId != officer.CollegeId ||
                drive.PlacementOfficerId != officer.PlacementOfficerId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot update another Placement Officer's drive.");
            }

            if (!Enum.IsDefined(typeof(PlacementDriveStatus), dto.Status))
                throw new InvalidOperationException(
                    "Invalid placement drive status.");

            ValidateDriveDetails(
                dto.StartDate,
                dto.EndDate,
                dto.RegistrationDeadline,
                dto.MinimumCGPA,
                dto.MaximumActiveBacklogs,
                dto.GraduationYearFrom,
                dto.GraduationYearTo);

            drive.DriveName = dto.DriveName.Trim();
            drive.Description = dto.Description.Trim();
            drive.Venue = dto.Venue.Trim();

            drive.StartDate = dto.StartDate;
            drive.EndDate = dto.EndDate;
            drive.RegistrationDeadline = dto.RegistrationDeadline;

            drive.MinimumCGPA = dto.MinimumCGPA;
            drive.MaximumActiveBacklogs = dto.MaximumActiveBacklogs;

            drive.EligibleDepartments = dto.EligibleDepartments.Trim();
            drive.EligibleDegrees = dto.EligibleDegrees.Trim();

            drive.GraduationYearFrom = dto.GraduationYearFrom;
            drive.GraduationYearTo = dto.GraduationYearTo;

            drive.Status = dto.Status;
            drive.UpdatedAt = DateTime.UtcNow;

            await _driveRepository.SaveChangesAsync();

            return MapToDto(drive);
        }

        public async Task<IEnumerable<PlacementDriveResponseDto>> GetAllAsync()
        {
            var drives = await _driveRepository.GetAllAsync();

            return drives.Select(MapToDto);
        }

        public async Task<IEnumerable<PlacementDriveResponseDto>>
            GetByCollegeIdAsync(int collegeId)
        {
            var drives = await _driveRepository.GetByCollegeIdAsync(collegeId);

            return drives.Select(MapToDto);
        }

        public async Task<IEnumerable<PlacementDriveResponseDto>>
            GetMyDrivesAsync(string placementOfficerUserId)
        {
            var officer = await _officerRepository.GetByUserIdAsync(
                placementOfficerUserId);

            if (officer == null)
                throw new InvalidOperationException(
                    "Placement Officer profile not found.");

            var drives = await _driveRepository.GetByPlacementOfficerIdAsync(
                officer.PlacementOfficerId);

            return drives.Select(MapToDto);
        }

        public async Task<IEnumerable<PlacementDriveResponseDto>>
            GetOpenDrivesAsync()
        {
            var drives = await _driveRepository.GetAllAsync();

            var now = DateTime.UtcNow;

            return drives
                .Where(d =>
                    d.Status == PlacementDriveStatus.RegistrationOpen &&
                    d.RegistrationDeadline > now &&
                    d.StartDate > now &&
                    d.Company.Status == CompanyStatus.Approved &&
                    d.College.Status == CollegeStatus.Approved)
                .Select(MapToDto);
        }

        public async Task<PlacementDriveResponseDto?> GetByIdAsync(int id)
        {
            var drive = await _driveRepository.GetByIdAsync(id);

            return drive == null ? null : MapToDto(drive);
        }

        private static void ValidateDriveDetails(
            DateTime startDate,
            DateTime endDate,
            DateTime registrationDeadline,
            decimal minimumCGPA,
            int maximumActiveBacklogs,
            int graduationYearFrom,
            int graduationYearTo)
        {
            var now = DateTime.UtcNow;

            if (startDate <= now)
                throw new InvalidOperationException(
                    "Drive start date must be in the future.");

            if (endDate <= startDate)
                throw new InvalidOperationException(
                    "Drive end date must be after its start date.");

            if (registrationDeadline >= startDate)
                throw new InvalidOperationException(
                    "Registration deadline must be before the drive start date.");

            if (registrationDeadline <= now)
                throw new InvalidOperationException(
                    "Registration deadline must be in the future.");

            if (minimumCGPA < 0 || minimumCGPA > 10)
                throw new InvalidOperationException(
                    "Minimum CGPA must be between 0 and 10.");

            if (maximumActiveBacklogs < 0)
                throw new InvalidOperationException(
                    "Maximum active backlogs cannot be negative.");

            if (graduationYearFrom > graduationYearTo)
                throw new InvalidOperationException(
                    "Graduation year range is invalid.");
        }

        private static PlacementDriveResponseDto MapToDto(
            PlacementDrive drive)
        {
            return new PlacementDriveResponseDto
            {
                PlacementDriveId = drive.PlacementDriveId,

                CollegeId = drive.CollegeId,
                CollegeName = drive.College.CollegeName,

                CompanyId = drive.CompanyId,
                CompanyName = drive.Company.CompanyName,

                PlacementOfficerId = drive.PlacementOfficerId,
                PlacementOfficerName = drive.PlacementOfficer.User.FullName,

                DriveName = drive.DriveName,
                Description = drive.Description,
                Venue = drive.Venue,

                StartDate = drive.StartDate,
                EndDate = drive.EndDate,
                RegistrationDeadline = drive.RegistrationDeadline,

                MinimumCGPA = drive.MinimumCGPA,
                MaximumActiveBacklogs = drive.MaximumActiveBacklogs,

                EligibleDepartments = drive.EligibleDepartments,
                EligibleDegrees = drive.EligibleDegrees,

                GraduationYearFrom = drive.GraduationYearFrom,
                GraduationYearTo = drive.GraduationYearTo,

                Status = drive.Status,
                CreatedAt = drive.CreatedAt,
                UpdatedAt = drive.UpdatedAt
            };
        }
    }
}
