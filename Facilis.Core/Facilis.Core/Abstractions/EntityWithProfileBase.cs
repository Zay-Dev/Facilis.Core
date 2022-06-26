using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Facilis.Core.Abstractions
{
    public class EntityWithProfileBase<T>
    {
        private T profile;
        private string serializedProfile;

        [NotMapped]
        public object UncastedProfile => this.Profile;

        [NotMapped]
        public T Profile
        {
            get
            {
                this.profile ??= this.GetProfile();
                return this.profile;
            }
        }

        public string SerializedProfile
        {
            get => this.serializedProfile;
            set
            {
                this.serializedProfile = value;
                this.profile = this.GetProfile();
            }
        }

        public T GetProfile()
        {
            return this.serializedProfile == null ? default :
                JsonSerializer.Deserialize<T>(this.serializedProfile);
        }

        public void SetProfile(object profile)
        {
            this.serializedProfile = JsonSerializer.Serialize(profile);
        }

        public void SetProfile(T profile)
        {
            this.SetProfile((object)profile);
        }
    }
}