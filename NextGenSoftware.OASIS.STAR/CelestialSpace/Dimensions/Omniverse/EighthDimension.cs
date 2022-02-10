﻿using System;
using System.Collections.Generic;
using NextGenSoftware.OASIS.API.Core.Interfaces.STAR;
using NextGenSoftware.OASIS.API.Core.Enums;

namespace NextGenSoftware.OASIS.STAR.CelestialSpace
{
    public class EighthDimension : OmniverseDimension, IEighthDimension
    {
        public EighthDimension(IOmiverse omniverse = null) : base(omniverse)   
        {
            Init(omniverse);
        }

        public EighthDimension(Guid id, IOmiverse omniverse = null) : base(id, omniverse)
        {
            Init(omniverse);
        }

        public EighthDimension(Dictionary<ProviderType, string> providerKey, IOmiverse omniverse = null) : base(providerKey, omniverse)
        {
            Init(omniverse);
        }

        private void Init(IOmiverse omniverse = null)
        {
            this.Name = "The Eighth Dimension";
            this.Description = "Coming Soon...";
            this.DimensionLevel = DimensionLevel.Eighth;
            this.SuperVerse.Name = $"{this.Name} SuperVerse";
        }
    }
}