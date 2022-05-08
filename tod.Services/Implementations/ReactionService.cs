using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;

namespace Tod.Services.Implementations
{
	public class ReactionService : IReactionService
	{
        private readonly IReactionRepository reactionRepository;
        private readonly ITopicReactionRepository topicReactionRepository;
        private readonly ICommentaryReactionRepository commentaryReactionRepository;
     
		public ReactionService(ITopicReactionRepository topicReactionRepository,
            IReactionRepository reactionRepository,
            ICommentaryReactionRepository commentaryReactionRepository)
		{
            this.reactionRepository = reactionRepository;
            this.topicReactionRepository = topicReactionRepository;
            this.commentaryReactionRepository = commentaryReactionRepository;
		}

        public async Task<List<Reaction>> GetByTopicIdAsync(int topicId)
        {
            var reactionsIds = this.topicReactionRepository.GetByTopicId(topicId);

            var reactions = new List<Reaction>();
            foreach (var reactionId in reactionsIds)
            {
                var reaction = await this.reactionRepository.GetAsync(reactionId);
                reactions.Add(reaction);
            }

            return reactions;
        }

        public async Task<List<Reaction>> GetByCommentaryIdAsync(int commentaryId)
        {
            var reactionsIds = this.commentaryReactionRepository.GetByCommentaryId(commentaryId);

            var reactions = new List<Reaction>();
            foreach (var reactionId in reactionsIds)
            {
                var reaction = await this.reactionRepository.GetAsync(reactionId);
                reactions.Add(reaction);
            }

            return reactions;
        }
    }
}

