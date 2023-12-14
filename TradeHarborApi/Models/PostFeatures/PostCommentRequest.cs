using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.PostFeatures
{
    public class PostCommentRequest
    {
        [Required(ErrorMessage = "The PostId field is required.")]
        public string PostId { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Comment field is required.")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "The PostOwnerUserId field is required.")]
        public string PostOwnerUserId { get; set; } = string.Empty;
    }
}
