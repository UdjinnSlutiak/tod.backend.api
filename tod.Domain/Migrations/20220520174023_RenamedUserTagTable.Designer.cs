﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tod.Domain;

#nullable disable

namespace Tod.Domain.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20220520174023_RenamedUserTagTable")]
    partial class RenamedUserTagTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Tod.Domain.Models.Commentary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Commentaries");
                });

            modelBuilder.Entity("Tod.Domain.Models.CommentaryReport", b =>
                {
                    b.Property<int>("CommentaryId")
                        .HasColumnType("int");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.HasKey("CommentaryId", "ReportId");

                    b.HasIndex("ReportId")
                        .IsUnique();

                    b.ToTable("CommentaryReports");
                });

            modelBuilder.Entity("Tod.Domain.Models.FavoriteTopic", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("FavoriteTopics");
                });

            modelBuilder.Entity("Tod.Domain.Models.InterestTag", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("InterestTags");
                });

            modelBuilder.Entity("Tod.Domain.Models.Reaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReactionValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("Tod.Domain.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Tod.Domain.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsedCount")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Tod.Domain.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Tod.Domain.Models.TopicCommentary", b =>
                {
                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("CommentaryId")
                        .HasColumnType("int");

                    b.HasKey("TopicId", "CommentaryId");

                    b.HasIndex("CommentaryId")
                        .IsUnique();

                    b.ToTable("TopicCommentaries");
                });

            modelBuilder.Entity("Tod.Domain.Models.TopicReport", b =>
                {
                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.HasKey("TopicId", "ReportId");

                    b.HasIndex("ReportId")
                        .IsUnique();

                    b.ToTable("TopicReports");
                });

            modelBuilder.Entity("Tod.Domain.Models.TopicTag", b =>
                {
                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("TopicId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("TopicTags");
                });

            modelBuilder.Entity("Tod.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Tod.Domain.Models.UserCommentary", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CommentaryId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CommentaryId");

                    b.HasIndex("CommentaryId")
                        .IsUnique();

                    b.ToTable("UserCommentaries");
                });

            modelBuilder.Entity("Tod.Domain.Models.UserCommentaryReaction", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CommentaryId")
                        .HasColumnType("int");

                    b.Property<int>("ReactionId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CommentaryId", "ReactionId");

                    b.HasIndex("CommentaryId");

                    b.HasIndex("ReactionId")
                        .IsUnique();

                    b.ToTable("UserCommentaryReactions");
                });

            modelBuilder.Entity("Tod.Domain.Models.UserReport", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ReportId");

                    b.HasIndex("ReportId")
                        .IsUnique();

                    b.ToTable("UserReports");
                });

            modelBuilder.Entity("Tod.Domain.Models.UserTopic", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TopicId");

                    b.HasIndex("TopicId")
                        .IsUnique();

                    b.ToTable("UserTopics");
                });

            modelBuilder.Entity("Tod.Domain.Models.UserTopicReaction", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("ReactionId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TopicId", "ReactionId");

                    b.HasIndex("ReactionId")
                        .IsUnique();

                    b.HasIndex("TopicId");

                    b.ToTable("UserTopicReactions");
                });

            modelBuilder.Entity("Tod.Domain.Models.CommentaryReport", b =>
                {
                    b.HasOne("Tod.Domain.Models.Commentary", null)
                        .WithMany()
                        .HasForeignKey("CommentaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.Report", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.CommentaryReport", "ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.FavoriteTopic", b =>
                {
                    b.HasOne("Tod.Domain.Models.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.InterestTag", b =>
                {
                    b.HasOne("Tod.Domain.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.TopicCommentary", b =>
                {
                    b.HasOne("Tod.Domain.Models.Commentary", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.TopicCommentary", "CommentaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.TopicReport", b =>
                {
                    b.HasOne("Tod.Domain.Models.Report", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.TopicReport", "ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.TopicTag", b =>
                {
                    b.HasOne("Tod.Domain.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.UserCommentary", b =>
                {
                    b.HasOne("Tod.Domain.Models.Commentary", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.UserCommentary", "CommentaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.UserCommentaryReaction", b =>
                {
                    b.HasOne("Tod.Domain.Models.Commentary", null)
                        .WithMany()
                        .HasForeignKey("CommentaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.Reaction", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.UserCommentaryReaction", "ReactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.UserReport", b =>
                {
                    b.HasOne("Tod.Domain.Models.Report", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.UserReport", "ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.UserTopic", b =>
                {
                    b.HasOne("Tod.Domain.Models.Topic", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.UserTopic", "TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tod.Domain.Models.UserTopicReaction", b =>
                {
                    b.HasOne("Tod.Domain.Models.Reaction", null)
                        .WithOne()
                        .HasForeignKey("Tod.Domain.Models.UserTopicReaction", "ReactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tod.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
