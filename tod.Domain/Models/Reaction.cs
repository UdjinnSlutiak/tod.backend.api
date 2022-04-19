using System;
using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models
{
	public class Reaction
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public ReactionValue ReactionValue { get; set; }
        public ReactionType ReactionType { get; set; }
        public DateTime Created { get; set; }
    }
}
