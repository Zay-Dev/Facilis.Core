using Facilis.Core.Abstractions;
using Facilis.Core.Attributes;
using Facilis.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text.Json;

namespace Facilis.Core.Tests.Models
{
    public class User :
        IEntityWithId,
        IEntityWithStatus,
        IEntityWithProfile<UserProfile>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public StatusTypes Status { get; set; }

        [NotMapped]
        public UserProfile Profile => this.GetProfile();

        [NotMapped]
        public object UncastedProfile => this.Profile;

        public string SerializedProfile { get; set; }

        public UserProfile GetProfile()
        {
            return this.SerializedProfile == null ? default :
                JsonSerializer.Deserialize<UserProfile>(this.SerializedProfile);
        }

        public void SetProfile(object profile)
        {
            this.SerializedProfile = JsonSerializer.Serialize(profile);
        }

        public void SetProfile(UserProfile profile)
        {
            this.SetProfile((object)profile);
        }
    }

    public class UserProfile
    {
        public string FirstName { get; set; } = "Tony";
        public string LastName { get; set; } = "Stark";
        public string Phone { get; set; }
        public int Age { get; set; } = 48;
        public string[] AddressLines { get; set; } = { "890 Fifth Avenue", "Manhattan", "New York City" };
        public decimal HeightInCm { get; set; } = 185.42m;

        [Immutable]
        public DateTime LastSignInAtUtc { get; set; } = DateTime.UtcNow;

        public FileInfo Avatar { get; set; }
        public UserProfile Copy { get; set; }
    }
}