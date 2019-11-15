﻿using System;
using System.ComponentModel;

using Microsoft.EntityFrameworkCore;

using Sokan.Yastah.Data.Permissions;

namespace Sokan.Yastah.Data.Administration
{
    public enum AdministrationPermission
    {
        [Description("Allows management of application permissions")]
        ManagePermissions = 1,

        [Description("Allows management of application roles")]
        ManageRoles = 2,

        [Description("Allows management of application users")]
        ManageUsers = 3
    }

    public static class AdministrationPermissionModelBuilder
    {
        [OnModelCreating]
        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<PermissionEntity>(entityBuilder =>
            {
                foreach (var category in EnumEx.EnumerateValuesWithDescriptions<AdministrationPermission>())
                    entityBuilder.HasData(new PermissionEntity(
                        categoryId:     (int)PermissionCategory.Administration,
                        permissionId:   (int)category.value,
                        name:           category.value.ToString(),
                        description:    category.description));
            });
    }
}
