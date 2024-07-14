﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TraffiLearn.Infrastructure.Database;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuestionTopic", b =>
                {
                    b.Property<Guid>("QuestionsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TopicsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionsId", "TopicsId");

                    b.HasIndex("TopicsId");

                    b.ToTable("QuestionTopic");
                });

            modelBuilder.Entity("TraffiLearn.Domain.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("DislikesCount")
                        .HasColumnType("int");

                    b.Property<string>("Explanation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LikesCount")
                        .HasColumnType("int");

                    b.ComplexProperty<Dictionary<string, object>>("TitleDetails", "TraffiLearn.Domain.Entities.Question.TitleDetails#QuestionTitleDetails", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int?>("QuestionNumber")
                                .HasColumnType("int");

                            b1.Property<int?>("TicketNumber")
                                .HasColumnType("int");
                        });

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TraffiLearn.Domain.Entities.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("QuestionTopic", b =>
                {
                    b.HasOne("TraffiLearn.Domain.Entities.Question", null)
                        .WithMany()
                        .HasForeignKey("QuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TraffiLearn.Domain.Entities.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TraffiLearn.Domain.Entities.Question", b =>
                {
                    b.OwnsMany("TraffiLearn.Domain.ValueObjects.Answer", "Answers", b1 =>
                        {
                            b1.Property<Guid>("QuestionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<bool>("IsCorrect")
                                .HasColumnType("bit");

                            b1.Property<string>("Text")
                                .IsRequired()
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)");

                            b1.HasKey("QuestionId", "Id");

                            b1.ToTable("Questions");

                            b1.ToJson("Answers");

                            b1.WithOwner()
                                .HasForeignKey("QuestionId");
                        });

                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
