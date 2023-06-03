using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Interfaces.STAR;
using NextGenSoftware.OASIS.API.Core.Objects;

namespace NextGenSoftware.OASIS.API.Providers.RedisOASIS.Entities
{
    public class Holon : HolonBase
    {
        [JsonProperty("parentOmniverseId")]
        public Guid ParentOmniverseId { get; set; }

        [JsonProperty("parentOmniverse")]
        public IOmiverse ParentOmniverse { get; set; }

        [JsonProperty("parentMultiverseId")]
        public Guid ParentMultiverseId { get; set; }

        [JsonProperty("parentMultiverse")]
        public IMultiverse ParentMultiverse { get; set; }

        [JsonProperty("parentUniverseId")]
        public Guid ParentUniverseId { get; set; }

        [JsonProperty("parentUniverse")]
        public IUniverse ParentUniverse { get; set; }

        [JsonProperty("parentDimensionId")]
        public Guid ParentDimensionId { get; set; }

        [JsonProperty("parentDimension")]
        public IDimension ParentDimension { get; set; }

        // ... The rest of the properties would follow this pattern ...

        [JsonProperty("parentZomeId")]
        public Guid ParentZomeId { get; set; }

        [JsonProperty("parentZome")]
        public IZome ParentZome { get; set; }

        [JsonProperty("parentHolonId")]
        public Guid ParentHolonId { get; set; }

        [JsonProperty("parentHolon")]
        public IHolon ParentHolon { get; set; }

        [JsonProperty("children")]
        public IEnumerable<IHolon> Children { get; set; }

        [JsonProperty("nodes")]
        public IEnumerable<Node> Nodes { get; set; }
    }
}
