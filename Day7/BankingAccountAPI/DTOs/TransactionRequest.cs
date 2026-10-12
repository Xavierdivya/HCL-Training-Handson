
using System.ComponentModel.DataAnnotations;

namespace BankingAccountAPI.DTOs
{
    public class TransactionRequest
    {
        [Range(typeof(decimal), "0.01", "999999999999.99",
            ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
