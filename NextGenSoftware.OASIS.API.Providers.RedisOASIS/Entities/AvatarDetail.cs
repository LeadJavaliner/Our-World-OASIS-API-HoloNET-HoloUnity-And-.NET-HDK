using System;
using System.Collections.Generic;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Core.Objects;

namespace NextGenSoftware.OASIS.API.Providers.RedisOASIS.Entities
{
    public class AvatarDetail : Holon
    {
        public string UmaJson { get; set; }
        public string Portrait { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Landline { get; set; }
        public string Mobile { get; set; }
        public DateTime DOB { get; set; }
        public int Karma { get; set; }
        public ConsoleColor FavouriteColour { get; set; }
        public ConsoleColor STARCLIColour { get; set; }
        public int XP { get; set; }
        public List<AvatarGift> Gifts { get; set; } = new List<AvatarGift>();
        public AvatarChakras Chakras { get; set; } = new AvatarChakras();
        public AvatarAura Aura { get; set; } = new AvatarAura();
        public AvatarStats Stats { get; set; } = new AvatarStats();
        public List<GeneKey> GeneKeys { get; set; } = new List<GeneKey>();
        public HumanDesign HumanDesign { get; set; } = new HumanDesign();
        public AvatarSkills Skills { get; set; } = new AvatarSkills();
        public AvatarAttributes Attributes { get; set; } = new AvatarAttributes();
        public AvatarSuperPowers SuperPowers { get; set; } = new AvatarSuperPowers();
        public List<Spell> Spells { get; set; } = new List<Spell>();
        public List<Achievement> Achievements { get; set; } = new List<Achievement>();
        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
        public List<KarmaAkashicRecord> KarmaAkashicRecords { get; set; }
        public int Level { get; set; }
    }
}
