using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.Post
{
    public class CreateTradePostRequest
    {
        [Required(ErrorMessage = "The Ticker field is required.")]
        public string Ticker { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Position field is required.")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Option field is required.")]
        public string Option { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Strikeprice field is required.")]
        public string Strikeprice { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Comment field is required.")]
        public string Comment { get; set; } = string.Empty;
    }
}
