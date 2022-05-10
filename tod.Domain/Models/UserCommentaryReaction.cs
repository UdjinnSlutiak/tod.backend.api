namespace Tod.Domain.Models
{
	public class UserCommentaryReaction
	{
        public int UserId { get; set; }
        public int CommentaryId { get; set; }
        public int ReactionId { get; set; }
    }
}
