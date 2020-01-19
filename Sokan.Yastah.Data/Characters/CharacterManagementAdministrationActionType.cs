﻿using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Sokan.Yastah.Data.Administration;

namespace Sokan.Yastah.Data.Characters
{
    public enum CharacterManagementAdministrationActionType
    {
        GuildCreated                = 400,
        GuildModified               = 401,
        GuildDeleted                = 402,
        GuildRestored               = 403,
        DivisionCreated             = 420,
        DivisionModified            = 421,
        DivisionDeleted             = 422,
        DivisionRestored            = 423,
        LevelDefinitionsInitialized = 440,
        LevelDefinitionsUpdated     = 441,
        CharacterCreated            = 460,
        CharacterModified           = 461,
        CharacterDeleted            = 462,
        CharacterRestored           = 463,
    }

    public static class RoleManagementAdministrationActionTypeModelBuilder
    {
        [OnModelCreating]
        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<AdministrationActionTypeEntity>(entityBuilder =>
            {
                var types = Enum.GetValues(typeof(CharacterManagementAdministrationActionType))
                    .Cast<CharacterManagementAdministrationActionType>();

                foreach (var type in types)
                    entityBuilder.HasData(new AdministrationActionTypeEntity(
                        id:         (int)type,
                        categoryId: (int)AdministrationActionCategory.CharacterManagement,
                        name:       type.ToString()));
            });
    }
}
