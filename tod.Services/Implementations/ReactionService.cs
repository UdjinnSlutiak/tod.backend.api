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
     
		public ReactionService(ITopicReactionRepository topicReactionRepository,
            IReactionRepository reactionRepository)
		{
            this.reactionRepository = reactionRepository;
            this.topicReactionRepository = topicReactionRepository;
		}

        public async Task<List<Reaction>> GetReactionsByTopicIdAsync(int topicId)
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
    }
}

