
using System.ComponentModel.DataAnnotations;

namespace BankingAccountAPI.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account holder name is required.")]
        [StringLength(100)]
        public string AccountHolderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(30)]
        public string AccountNumber { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "999999999999.99",
            ErrorMessage = "Balance cannot be negative.")]
        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Account type is required.")]
        [RegularExpression("^(Savings|Current)$",
            ErrorMessage = "Account type must be Savings or Current.")]
        public string AccountType { get; set; } = "Savings";
    }
}
