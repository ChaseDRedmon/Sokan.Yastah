﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sokan.Yastah.Data;

namespace Sokan.Yastah.Data.Migrations
{
    [DbContext(typeof(YastahDbContext))]
    [Migration("20190715195219_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Sokan.Yastah.Data.Authorization.PermissionCategoryEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PermissionCategories");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Authorization.PermissionEntity", b =>
                {
                    b.Property<long>("PermissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CategoryId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PermissionId");

                    b.HasIndex("CategoryId", "Name")
                        .IsUnique();

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Roles.RoleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreatedById");

                    b.Property<long?>("DeletedById");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Roles.RolePermissionMappingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<long>("CreatedById");

                    b.Property<long?>("DeletedById");

                    b.Property<long>("PermissionId");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissionMappings");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarHash");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<DateTimeOffset>("FirstSeen");

                    b.Property<DateTimeOffset>("LastSeen");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserPermissionDefaultMappingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<long>("CreatedById");

                    b.Property<long?>("DeletedById");

                    b.Property<long>("PermissionId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("PermissionId");

                    b.ToTable("UserPermissionDefaultMappings");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserPermissionMappingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<long>("CreatedById");

                    b.Property<long?>("DeletedById");

                    b.Property<bool>("IsDenied");

                    b.Property<long>("PermissionId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermissionMappings");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserRoleDefaultMappingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<long>("CreatedById");

                    b.Property<long?>("DeletedById");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoleDefaultMappings");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserRoleMappingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<long>("CreatedById");

                    b.Property<long?>("DeletedById");

                    b.Property<long>("RoleId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoleMappings");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Authorization.PermissionEntity", b =>
                {
                    b.HasOne("Sokan.Yastah.Data.Authorization.PermissionCategoryEntity", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Roles.RoleEntity", b =>
                {
                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Roles.RolePermissionMappingEntity", b =>
                {
                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("Sokan.Yastah.Data.Authorization.PermissionEntity", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Roles.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserPermissionDefaultMappingEntity", b =>
                {
                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("Sokan.Yastah.Data.Authorization.PermissionEntity", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserPermissionMappingEntity", b =>
                {
                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("Sokan.Yastah.Data.Authorization.PermissionEntity", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserRoleDefaultMappingEntity", b =>
                {
                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("Sokan.Yastah.Data.Roles.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sokan.Yastah.Data.Users.UserRoleMappingEntity", b =>
                {
                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("Sokan.Yastah.Data.Roles.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sokan.Yastah.Data.Users.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
