using System;
using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models
{
	public class Reaction
	{
        public int Id { get; set; }
        public ReactionValue ReactionValue { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
