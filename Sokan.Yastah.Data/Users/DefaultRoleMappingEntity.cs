﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Sokan.Yastah.Data.Administration;
using Sokan.Yastah.Data.Roles;

namespace Sokan.Yastah.Data.Users
{
    [Table("DefaultRoleMappings", Schema = "Users")]
    internal class DefaultRoleMappingEntity
    {
        public DefaultRoleMappingEntity(
            long id,
            long roleId,
            long creationId,
            long? deletionId)
        {
            Id = id;
            RoleId = roleId;
            CreationId = creationId;
            DeletionId = deletionId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; internal set; }

        [ForeignKey(nameof(Role))]
        public long RoleId { get; }

        public RoleEntity Role { get; internal set; }
            = null!;

        [ForeignKey(nameof(Creation))]
        public long CreationId { get; }

        public AdministrationActionEntity Creation { get; internal set; }
            = null!;

        [ForeignKey(nameof(Deletion))]
        public long? DeletionId { get; set; }

        public AdministrationActionEntity? Deletion { get; set; }

        [OnModelCreating]
        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<DefaultRoleMappingEntity>();
    }
}
