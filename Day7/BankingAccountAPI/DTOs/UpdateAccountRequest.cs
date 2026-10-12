
using System.ComponentModel.DataAnnotations;

namespace BankingAccountAPI.DTOs
{
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = "Account holder name is required.")]
        [StringLength(100)]
        public string AccountHolderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(30)]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type is required.")]
        [RegularExpression("^(Savings|Current)$",
            ErrorMessage = "Account type must be Savings or Current.")]
        public string AccountType { get; set; } = "Savings";
    }
}
