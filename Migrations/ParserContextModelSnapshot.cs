﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parser.Models;

namespace Parser.Migrations
{
    [DbContext(typeof(ParserContext))]
    partial class ParserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("Parser.Models.Domain", b =>
                {
                    b.Property<string>("DomainName")
                        .HasColumnType("TEXT");

                    b.HasKey("DomainName");

                    b.ToTable("Domains");
                });

            modelBuilder.Entity("Parser.Models.Record", b =>
                {
                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<string>("Body")
                        .HasColumnType("TEXT");

                    b.Property<string>("DomainName")
                        .HasColumnType("TEXT");

                    b.HasKey("Url");

                    b.HasIndex("DomainName");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("Parser.Models.Record", b =>
                {
                    b.HasOne("Parser.Models.Domain", "Domain")
                        .WithMany("Records")
                        .HasForeignKey("DomainName");
                });
#pragma warning restore 612, 618
        }
    }
}
