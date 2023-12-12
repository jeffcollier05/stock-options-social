using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.Post
{
    public class DeleteTradePostRequest
    {
        [Required(ErrorMessage = "The TradeId field is required.")]
        public string TradeId { get; set; } = string.Empty;
    }
}
