using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.PostFeatures
{
    public class PostReactionRequest
    {
        [Required(ErrorMessage = "The PostId field is required.")]
        public string PostId { get; set; } = string.Empty;

        [Required(ErrorMessage = "The ReactionType field is required.")]
        public string ReactionType { get; set; } = string.Empty;
    }
}
