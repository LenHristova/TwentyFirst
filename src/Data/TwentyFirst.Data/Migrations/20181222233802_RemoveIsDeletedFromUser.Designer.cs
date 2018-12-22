﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwentyFirst.Data;

namespace TwentyFirst.Data.Migrations
{
    [DbContext(typeof(TwentyFirstDbContext))]
    [Migration("20181222233802_RemoveIsDeletedFromUser")]
    partial class RemoveIsDeletedFromUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Article", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .HasMaxLength(200);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("CreatorId")
                        .IsRequired();

                    b.Property<string>("ImageId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsImportant");

                    b.Property<bool>("IsTop");

                    b.Property<string>("Lead")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<DateTime>("PublishedOn");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ImageId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.ArticleCategory", b =>
                {
                    b.Property<string>("ArticleId");

                    b.Property<string>("CategoryId");

                    b.HasKey("ArticleId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ArticlesCategories");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.ArticleEdit", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArticleId")
                        .IsRequired();

                    b.Property<DateTime>("EditDateTime");

                    b.Property<string>("EditorId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("EditorId");

                    b.ToTable("ArticlesEdits");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.ArticleToArticle", b =>
                {
                    b.Property<string>("ConnectedToId");

                    b.Property<string>("ConnectedFromId");

                    b.HasKey("ConnectedToId", "ConnectedFromId");

                    b.HasIndex("ConnectedFromId");

                    b.ToTable("ArticlesToArticles");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("Order");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Image", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("CreatorId")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ThumbUrl")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Interview", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .HasMaxLength(200);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("CreatorId")
                        .IsRequired();

                    b.Property<string>("ImageId")
                        .IsRequired();

                    b.Property<string>("Interviewed")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("PublishedOn");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ImageId");

                    b.ToTable("Interviews");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.InterviewEdit", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EditDateTime");

                    b.Property<string>("EditorId")
                        .IsRequired();

                    b.Property<string>("InterviewId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EditorId");

                    b.HasIndex("InterviewId");

                    b.ToTable("InterviewsEdits");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Log", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("EventId");

                    b.Property<string>("LogLevel")
                        .IsRequired();

                    b.Property<string>("Message")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Poll", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatorId")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Polls");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.PollAnswer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("PollId")
                        .IsRequired();

                    b.Property<int>("Votes");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.ToTable("PollAnswers");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Subscriber", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConfirmationCode")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<bool>("IsConfirmed");

                    b.HasKey("Id");

                    b.ToTable("Subscribers");
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TwentyFirst.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Article", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User", "Creator")
                        .WithMany("CreatedArticles")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TwentyFirst.Data.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.ArticleCategory", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.Article", "Article")
                        .WithMany("Categories")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TwentyFirst.Data.Models.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.ArticleEdit", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.Article", "Article")
                        .WithMany("Edits")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TwentyFirst.Data.Models.User", "Editor")
                        .WithMany("EditedArticles")
                        .HasForeignKey("EditorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.ArticleToArticle", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.Article", "ConnectedFrom")
                        .WithMany("ConnectedTo")
                        .HasForeignKey("ConnectedFromId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TwentyFirst.Data.Models.Article", "ConnectedTo")
                        .WithMany()
                        .HasForeignKey("ConnectedToId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Image", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User", "Creator")
                        .WithMany("CreatedImages")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Interview", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User", "Creator")
                        .WithMany("CreatedInterviews")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TwentyFirst.Data.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.InterviewEdit", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User", "Editor")
                        .WithMany("EditedInterviews")
                        .HasForeignKey("EditorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TwentyFirst.Data.Models.Interview", "Interview")
                        .WithMany("Edits")
                        .HasForeignKey("InterviewId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.Poll", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.User", "Creator")
                        .WithMany("CreatedPolls")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TwentyFirst.Data.Models.PollAnswer", b =>
                {
                    b.HasOne("TwentyFirst.Data.Models.Poll", "Poll")
                        .WithMany("Answers")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
