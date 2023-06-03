using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Helpers;

namespace NextGenSoftware.OASIS.API.Providers.RedisOASIS.Entities
{
    public class HolonBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isChanged")]
        public bool IsChanged { get; set; }

        [JsonProperty("providerUniqueStorageKey")]
        public Dictionary<ProviderType, string> ProviderUniqueStorageKey { get; set; } = new Dictionary<ProviderType, string>();

        [JsonProperty("providerMetaData")]
        public Dictionary<ProviderType, Dictionary<string, string>> ProviderMetaData { get; set; } = new Dictionary<ProviderType, Dictionary<string, string>>();

        [JsonProperty("metaData")]
        public Dictionary<string, string> MetaData { get; set; } = new Dictionary<string, string>();

        [JsonProperty("holonId")]
        public Guid HolonId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("holonType")]
        public HolonType HolonType { get; set; }

        [JsonProperty("createdProviderType")]
        public EnumValue<ProviderType> CreatedProviderType { get; set; }

        [JsonProperty("createdOASISType")]
        public EnumValue<OASISType> CreatedOASISType { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("deletedDate")]
        public DateTime DeletedDate { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("previousVersionId")]
        public Guid PreviousVersionId { get; set; }

        [JsonProperty("previousVersionProviderUniqueStorageKey")]
        public Dictionary<ProviderType, string> PreviousVersionProviderUniqueStorageKey { get; set; } = new Dictionary<ProviderType, string>();

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdByAvatarId")]
        public string CreatedByAvatarId { get; set; }

        [JsonProperty("modifiedByAvatarId")]
        public string ModifiedByAvatarId { get; set; }

        [JsonProperty("deletedByAvatarId")]
        public string DeletedByAvatarId { get; set; }
    }
}
