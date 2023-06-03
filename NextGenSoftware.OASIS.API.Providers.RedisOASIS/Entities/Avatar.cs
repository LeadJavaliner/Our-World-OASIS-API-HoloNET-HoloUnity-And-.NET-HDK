using System;
using System.Collections.Generic;
using StackExchange.Redis;
using Newtonsoft.Json;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Objects;

namespace NextGenSoftware.OASIS.API.Providers.RedisOASIS.Entities
{
    public class Avatar : HolonBase
    {
        public Dictionary<ProviderType, List<ProviderWallet>> ProviderWallets { get; set; } = new Dictionary<ProviderType, List<ProviderWallet>>();

        public Dictionary<ProviderType, string> ProviderUsername { get; set; } = new Dictionary<ProviderType, string>();  

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return string.Concat(Title, " ", FirstName, " ", LastName);
            }
        }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public EnumValue<AvatarType> AvatarType { get; set; }
        public bool AcceptTerms { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string ResetToken { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime? LastBeamedIn { get; set; }
        public DateTime? LastBeamedOut { get; set; }
        public bool IsBeamedIn { get; set; }
    }
}
