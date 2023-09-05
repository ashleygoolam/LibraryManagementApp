﻿// <auto-generated />
using System;
using LibraryManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryManagementApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230411162632_editTablesId")]
    partial class editTablesId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibraryManagementApp.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Author_Book", b =>
                {
                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.HasKey("AuthorId", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("Author_Book");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AvailableAmount")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<int>("bookCategory")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.BookInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("IsbnNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("bookAvailability")
                        .HasColumnType("int");

                    b.Property<int>("bookStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("BookInstance");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.BookIssue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookInstanceId")
                        .HasColumnType("int");

                    b.Property<int>("BorrowersId")
                        .HasColumnType("int");

                    b.Property<int>("BorrowingLibrariansId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReturningLibrariansId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeBorrowed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeReturned")
                        .HasColumnType("datetime2");

                    b.Property<int>("bookReturnStatus")
                        .HasColumnType("int");

                    b.Property<int>("bookStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookInstanceId");

                    b.ToTable("BookIssue");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PublisherDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublisherLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublisherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Publisher");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Author_Book", b =>
                {
                    b.HasOne("LibraryManagementApp.Models.Author", "Author")
                        .WithMany("Author_Book")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryManagementApp.Models.Book", "Book")
                        .WithMany("Author_Book")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Book", b =>
                {
                    b.HasOne("LibraryManagementApp.Models.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.BookInstance", b =>
                {
                    b.HasOne("LibraryManagementApp.Models.Book", "Book")
                        .WithMany("BookInstance")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.BookIssue", b =>
                {
                    b.HasOne("LibraryManagementApp.Models.BookInstance", "BookInstance")
                        .WithMany("BookIssue")
                        .HasForeignKey("BookInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookInstance");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Author", b =>
                {
                    b.Navigation("Author_Book");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Book", b =>
                {
                    b.Navigation("Author_Book");

                    b.Navigation("BookInstance");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.BookInstance", b =>
                {
                    b.Navigation("BookIssue");
                });

            modelBuilder.Entity("LibraryManagementApp.Models.Publisher", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
