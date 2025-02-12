﻿// <auto-generated />
using System;
using ChatApp.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatApp.Backend.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    partial class ChatDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChatApp.Backend.Domain.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ChatApp.Backend.Domain.GroupUsers", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("ChatApp.Backend.Domain.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReceiverGroupId")
                        .HasColumnType("int");

                    b.Property<string>("ReceiverUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverGroupId");

                    b.HasIndex("ReceiverUserId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages", t =>
                        {
                            t.HasCheckConstraint("CK_MESSAGE_RECEIVER", "(ReceiverUserId IS NOT NULL AND ReceiverGroupId IS NULL) OR (ReceiverUserId IS NULL AND ReceiverGroupId IS NOT NULL)");
                        });
                });

            modelBuilder.Entity("ChatApp.Backend.Domain.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DisplayName")
                        .IsUnique()
                        .HasDatabaseName("IX_UNIQUE_NAME");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("IX_UNIQUE_EMAIL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatApp.Backend.Domain.GroupUsers", b =>
                {
                    b.HasOne("ChatApp.Backend.Domain.Group", "Group")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Backend.Domain.User", "User")
                        .WithMany("GroupUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Backend.Domain.Message", b =>
                {
                    b.HasOne("ChatApp.Backend.Domain.Group", "ReceiverGroup")
                        .WithMany("Messages")
                        .HasForeignKey("ReceiverGroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ChatApp.Backend.Domain.User", "ReceiverUser")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ChatApp.Backend.Domain.User", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ReceiverGroup");

                    b.Navigation("ReceiverUser");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("ChatApp.Backend.Domain.Group", b =>
                {
                    b.Navigation("GroupUsers");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ChatApp.Backend.Domain.User", b =>
                {
                    b.Navigation("GroupUsers");

                    b.Navigation("ReceivedMessages");

                    b.Navigation("SentMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
