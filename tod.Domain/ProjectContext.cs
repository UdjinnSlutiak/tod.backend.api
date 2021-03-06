using System;
using Microsoft.EntityFrameworkCore;
using Tod.Domain.Models;

namespace Tod.Domain
{
	public class ProjectContext : DbContext
	{
		public ProjectContext(DbContextOptions<ProjectContext> options)
			: base(options)
		{
			
		}

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<InterestTag> InterestTags { get; set; }
        public DbSet<FavoriteTopic> FavoriteTopics { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
        public DbSet<TopicTag> TopicTags { get; set; }
        public DbSet<TopicReport> TopicReports { get; set; }
        public DbSet<UserTopicReaction> UserTopicReactions { get; set; }
        public DbSet<CommentaryReport> CommentaryReports { get; set; }
        public DbSet<UserCommentaryReaction> UserCommentaryReactions { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }
        public DbSet<UserCommentary> UserCommentaries { get; set; }
        public DbSet<TopicCommentary> TopicCommentaries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
             builder.Entity<UserTopic>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId);
            builder.Entity<UserTopic>()
                .HasOne<Topic>()
                .WithOne()
                .HasForeignKey<UserTopic>(t => t.TopicId);

            builder.Entity<TopicCommentary>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(c => c.TopicId);
            builder.Entity<TopicCommentary>()
                .HasOne<Commentary>()
                .WithOne()
                .HasForeignKey<TopicCommentary>(c => c.CommentaryId);

            builder.Entity<UserCommentary>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId);
            builder.Entity<UserCommentary>()
                .HasOne<Commentary>()
                .WithOne()
                .HasForeignKey<UserCommentary>(c => c.CommentaryId);

            builder.Entity<InterestTag>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId);
            builder.Entity<InterestTag>()
                .HasOne<Tag>()
                .WithMany()
                .HasForeignKey(t => t.TagId);

            builder.Entity<TopicTag>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(t => t.TopicId);
            builder.Entity<TopicTag>()
                .HasOne<Tag>()
                .WithMany()
                .HasForeignKey(t => t.TagId);

            builder.Entity<FavoriteTopic>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(u => u.UserId);
            builder.Entity<FavoriteTopic>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(u => u.TopicId);
            
            builder.Entity<UserReport>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(u => u.UserId);
            builder.Entity<UserReport>()
                .HasOne<Report>()
                .WithOne()
                .HasForeignKey<UserReport>(u => u.ReportId);

            builder.Entity<TopicReport>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(u => u.TopicId);
            builder.Entity<TopicReport>()
                .HasOne<Report>()
                .WithOne()
                .HasForeignKey<TopicReport>(_ => _.ReportId);

            builder.Entity<CommentaryReport>()
                .HasOne<Commentary>()
                .WithMany()
                .HasForeignKey(u => u.CommentaryId);
            builder.Entity<CommentaryReport>()
                .HasOne<Report>()
                .WithOne()
                .HasForeignKey<CommentaryReport>(_ => _.ReportId);

            builder.Entity<UserTopicReaction>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(t => t.TopicId);
            builder.Entity<UserTopicReaction>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(_ => _.UserId);
            builder.Entity<UserTopicReaction>()
                .HasOne<Reaction>()
                .WithOne()
                .HasForeignKey<UserTopicReaction>(_ => _.ReactionId);

            builder.Entity<UserCommentaryReaction>()
                .HasOne<Commentary>()
                .WithMany()
                .HasForeignKey(c => c.CommentaryId);
            builder.Entity<UserCommentaryReaction>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(_ => _.UserId);
            builder.Entity<UserCommentaryReaction>()
                .HasOne<Reaction>()
                .WithOne()
                .HasForeignKey<UserCommentaryReaction>(_ => _.ReactionId);

            builder.Entity<UserTopic>()
                .HasKey(ut => new { ut.UserId, ut.TopicId });

            builder.Entity<UserCommentary>()
                .HasKey(uc => new { uc.UserId, uc.CommentaryId });

            builder.Entity<TopicCommentary>()
                .HasKey(tc => new { tc.TopicId, tc.CommentaryId });

            builder.Entity<UserTopicReaction>()
                .HasKey(tr => new { tr.UserId, tr.TopicId, tr.ReactionId });

            builder.Entity<TopicReport>()
                .HasKey(tr => new { tr.TopicId, tr.ReportId });

            builder.Entity<TopicTag>()
                .HasKey(tt => new { tt.TopicId, tt.TagId });

            builder.Entity<UserCommentaryReaction>()
                .HasKey(cr => new { cr.UserId, cr.CommentaryId, cr.ReactionId });

            builder.Entity<CommentaryReport>()
                .HasKey(cr => new { cr.CommentaryId, cr.ReportId });

            builder.Entity<FavoriteTopic>()
                .HasKey(ft => new { ft.UserId, ft.TopicId });

            builder.Entity<UserReport>()
                .HasKey(ur => new { ur.UserId, ur.ReportId });

            builder.Entity<InterestTag>()
                .HasKey(ut => new { ut.UserId, ut.TagId });
        }
    }
}
