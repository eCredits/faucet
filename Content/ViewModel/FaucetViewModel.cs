using System.ComponentModel.DataAnnotations;

namespace NethereumFaucet.ViewModel
{
	public class FaucetViewModel
	{
		[Required]
		[StringLength(42, ErrorMessage = "Addresses should be 42 characters in length", MinimumLength = 40)]
		public string Address { get; set; }

		public string TransactionHash { get; set; }

		public int Amount { get; set; }
	}
}
