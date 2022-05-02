using Facilis.Core.Abstractions;
using Facilis.Core.Enums;
using Facilis.Core.Tests.Models;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.Json;

namespace Facilis.Core.Tests
{
    public class AutoBindProfile
    {
        private Instances instances { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.instances = new Instances();
        }

        [Test]
        public void TestAddUserWithProfile_AttributesAutoAdded()
        {
            // Arrange
            var users = this.instances.GetEntities<User>();
            var user = new User();

            user.SetProfile(new UserProfile() { Copy = new() });
            users.Add(user);

            // Act
            var profile = user.Profile;
            var attributes = this.instances
                .GetEntities<ExtendedAttribute>()
                .WhereEnabled(entity => entity.ScopedId == user.Id)
                .ToDictionary(entity => entity.Key);

            // Assert
            Assert.AreEqual(profile.FirstName, attributes["FirstName"].Value);
            Assert.AreEqual(profile.LastName, attributes["LastName"].Value);
            Assert.AreEqual(profile.Phone, attributes["Phone"].Value);

            Assert.AreEqual(profile.Age, JsonSerializer.Deserialize<int>(attributes["Age"].Value));
            Assert.AreEqual(profile.HeightInCm, JsonSerializer.Deserialize<decimal>(attributes["HeightInCm"].Value));
            Assert.AreEqual(profile.LastSignInAtUtc, JsonSerializer.Deserialize<DateTime>(attributes["LastSignInAtUtc"].Value));

            Assert.AreEqual(profile.Avatar, attributes["Avatar"].Value);
            Assert.NotNull(JsonSerializer.Deserialize<UserProfile>(attributes["Copy"].Value).FirstName);

            Assert.Pass();
        }

        [Test]
        public void Test_UpdateUserWithProfile_AttributesAreUpdated()
        {
            // Arrange
            var users = this.instances.GetEntities<User>();
            var user = new User();
            var profile = new UserProfile() { Copy = new() };

            var notExpected = profile.FirstName;
            var expected = "Iron";

            user.SetProfile(profile);
            users.Add(user);

            // Act
            profile.FirstName = expected;
            user.SetProfile(profile);
            users.Update(user);

            var attribute = this.instances
                .GetEntities<ExtendedAttribute>()
                .WhereEnabled(entity => entity.ScopedId == user.Id)
                .FirstOrDefault(entity => entity.Key == nameof(UserProfile.FirstName));

            // Assert
            Assert.AreEqual(expected, attribute.Value);
            Assert.AreNotEqual(notExpected, attribute.Value);

            Assert.Pass();
        }

        [Test]
        public void Test_UpdateUserStatus_AttributesStatusAreUpdated()
        {
            // Arrange
            var users = this.instances.GetEntities<User>();
            var user = new User();

            user.SetProfile(new UserProfile() { Copy = new() });
            users.Add(user);

            // Act
            user.Status = StatusTypes.Disabled;
            users.Update(user);

            var attributes = this.instances
                .GetEntities<ExtendedAttribute>()
                .WhereAll(entity => entity.ScopedId == user.Id);

            // Assert
            Assert.AreEqual(StatusTypes.Disabled, user.Status);
            Assert.IsTrue(attributes.All(entity => entity.Status == user.Status));
            Assert.Pass();
        }

        [Test]
        public void Test_UpdateImmutable_AttributeIsOnTopAdded()
        {
            // Arrange
            var users = this.instances.GetEntities<User>();
            var user = new User();
            var profile = new UserProfile() { Copy = new() };

            user.SetProfile(profile);
            users.Add(user);

            // Act
            profile.LastSignInAtUtc = DateTime.UtcNow.AddHours(1);
            user.SetProfile(profile);
            users.Update(user);

            var attributes = this.instances
                .GetEntities<ExtendedAttribute>()
                .WhereEnabled(entity => entity.ScopedId == user.Id &&
                    entity.Key == nameof(UserProfile.LastSignInAtUtc)
                )
                .ToArray();

            // Assert
            Assert.AreNotEqual(attributes[0].Value, attributes[1].Value);
            Assert.Pass();
        }
    }
}