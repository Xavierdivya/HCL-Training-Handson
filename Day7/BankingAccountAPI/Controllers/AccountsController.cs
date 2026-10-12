
using BankingAccountAPI.Data;
using BankingAccountAPI.DTOs;
using BankingAccountAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingAccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/accounts?accountType=Savings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetAccounts(
            [FromQuery] string? accountType)
        {
            var query = _context.BankAccounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(accountType))
            {
                if (accountType != "Savings" && accountType != "Current")
                    return BadRequest("Account type must be Savings or Current.");

                query = query.Where(a => a.AccountType == accountType);
            }

            return await query.OrderBy(a => a.Id).ToListAsync();
        }

        // GET: api/accounts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetAccount(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
                return NotFound();

            return account;
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<ActionResult<BankAccount>> CreateAccount(
            BankAccount account)
        {
            bool duplicate = await _context.BankAccounts.AnyAsync(
                a => a.AccountNumber == account.AccountNumber);

            if (duplicate)
                return Conflict("Account number already exists.");

            _context.BankAccounts.Add(account);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Also handles a duplicate created by a concurrent request.
                return Conflict("Account number already exists.");
            }

            return CreatedAtAction(
                nameof(GetAccount),
                new { id = account.Id },
                account);
        }

        // PUT: api/accounts/1
        // Balance cannot be changed through this endpoint.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(
            int id, UpdateAccountRequest request)
        {
            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
                return NotFound();

            account.AccountHolderName = request.AccountHolderName;
            account.AccountType = request.AccountType;

            await _context.SaveChangesAsync();

            return Ok(account);
        }

        // DELETE: api/accounts/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
                return NotFound();

            _context.BankAccounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/accounts/1/deposit
        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> Deposit(
            int id, TransactionRequest request)
        {
            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
                return NotFound();

            account.Balance += request.Amount;
            await _context.SaveChangesAsync();

            return Ok(account);
        }

        // POST: api/accounts/1/withdraw
        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(
            int id, TransactionRequest request)
        {
            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
                return NotFound();

            if (account.Balance < request.Amount)
                return BadRequest("Insufficient balance.");

            account.Balance -= request.Amount;
            await _context.SaveChangesAsync();

            return Ok(account);
        }
    }
}
