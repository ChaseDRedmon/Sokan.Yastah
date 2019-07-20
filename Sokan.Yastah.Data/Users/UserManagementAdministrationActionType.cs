﻿using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Sokan.Yastah.Data.Administration;

namespace Sokan.Yastah.Data.Users
{
    public enum UserManagementAdministrationActionType
    {
        UserInitialization = 20
    }

    public static class UserManagementAdministrationActionTypeModelBuilder
    {
        [OnModelCreating]
        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<AdministrationActionTypeEntity>(entityBuilder =>
            {
                var types = Enum.GetValues(typeof(UserManagementAdministrationActionType))
                    .Cast<UserManagementAdministrationActionType>();

                foreach (var type in types)
                    entityBuilder.HasData(new AdministrationActionTypeEntity()
                    {
                        Id = (int)type,
                        CategoryId = (int)AdministrationActionCategory.UserManagement,
                        Name = type.ToString()
                    });
            });
    }
}
