﻿using System;
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
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<FavoriteTopic> FavoriteTopics { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
        public DbSet<TopicTag> TopicTags { get; set; }
        public DbSet<TopicReport> TopicReports { get; set; }
        public DbSet<TopicReaction> TopicReactions { get; set; }
        public DbSet<CommentaryReport> CommentaryReports { get; set; }
        public DbSet<CommentaryReaction> CommentaryReactions { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }
        public DbSet<UserCommentary> UserCommentaries { get; set; }
        public DbSet<TopicCommentary> TopicCommentaries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
             builder.Entity<UserTopic>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId);

            builder.Entity<TopicCommentary>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(c => c.TopicId);

            builder.Entity<UserCommentary>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId);

            builder.Entity<UserTag>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId);
            builder.Entity<UserTag>()
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

            builder.Entity<TopicReport>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(u => u.TopicId);

            builder.Entity<CommentaryReport>()
                .HasOne<Commentary>()
                .WithMany()
                .HasForeignKey(u => u.CommentaryId);

            builder.Entity<TopicReaction>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(t => t.TopicId);

            builder.Entity<CommentaryReaction>()
                .HasOne<Commentary>()
                .WithMany()
                .HasForeignKey(c => c.CommentaryId);

            builder.Entity<UserTopic>()
                .HasKey(ut => new { ut.UserId, ut.TopicId });

            builder.Entity<UserCommentary>()
                .HasKey(uc => new { uc.UserId, uc.CommentaryId });

            builder.Entity<TopicCommentary>()
                .HasKey(tc => new { tc.TopicId, tc.CommentaryId });

            builder.Entity<TopicReaction>()
                .HasKey(tr => new { tr.TopicId, tr.ReactionId });

            builder.Entity<TopicReport>()
                .HasKey(tr => new { tr.TopicId, tr.ReportId });

            builder.Entity<TopicTag>()
                .HasKey(tt => new { tt.TopicId, tt.TagId });

            builder.Entity<CommentaryReaction>()
                .HasKey(cr => new { cr.CommentaryId, cr.ReactionId });

            builder.Entity<CommentaryReport>()
                .HasKey(cr => new { cr.CommentaryId, cr.ReportId });

            builder.Entity<FavoriteTopic>()
                .HasKey(ft => new { ft.UserId, ft.TopicId });

            builder.Entity<UserReport>()
                .HasKey(ur => new { ur.UserId, ur.ReportId });

            builder.Entity<UserTag>()
                .HasKey(ut => new { ut.UserId, ut.TagId });
        }
    }
}
