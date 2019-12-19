﻿using System.Collections.Generic;

using NUnit.Framework;
using Shouldly;

using Sokan.Yastah.Data.Characters;
using Sokan.Yastah.Data.Users;

namespace Sokan.Yastah.Data.Test.Characters
{
    [TestFixture]
    public class CharacterIdentityViewModelTests
    {
        #region Constructor() Tests

        public static readonly IReadOnlyList<TestCaseData> Constructor_TestCaseData
            = new[]
            {
                /*                  id,             name            */
                new TestCaseData(   default(long),  string.Empty    ).SetName("{m}(Default Values"),
                new TestCaseData(   long.MinValue,  string.Empty    ).SetName("{m}(Min Values)"),
                new TestCaseData(   1L,             "name 2"        ).SetName("{m}(Unique Value Set 1)"),
                new TestCaseData(   3L,             "name 4"        ).SetName("{m}(Unique Value Set 2)"),
                new TestCaseData(   5L,             "name 6"        ).SetName("{m}(Unique Value Set 3)"),
                new TestCaseData(   long.MaxValue,  "Max Value"     ).SetName("{m}(Max Values)")
            };

        [TestCaseSource(nameof(Constructor_TestCaseData))]
        public void Constructor_Always_ReturnsIdentity(
            long id,
            string name)
        {
            var owner = new UserIdentityViewModel(
                id:             default,
                username:       string.Empty,
                discriminator:  string.Empty);

            var result = new CharacterIdentityViewModel(
                id,
                name,
                owner);

            result.Id.ShouldBe(id);
            result.Name.ShouldBe(name);
            result.Owner.ShouldBeSameAs(owner);
        }

        #endregion Constructor() Tests
    }
}
