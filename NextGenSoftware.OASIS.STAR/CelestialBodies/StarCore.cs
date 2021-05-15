﻿using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Core.Interfaces.STAR;
using System;

namespace NextGenSoftware.OASIS.STAR.CelestialBodies
{
    public class StarCore : CelestialBodyCore, IStarCore
    {
        /*
      //  private string _providerKey = "";
        private const string STAR_CORE_ZOME = "star_core_zome"; //Name of the core zome in rust hc.
        //private const string STAR_HOLON_TYPE = "star_holon";
        private const string STAR_HOLON_TYPE = "star";
        //private const string STAR_HOLONS_TYPE = "star_holons";

        private const string STAR_ADD_STAR = "star_add_star";
        private const string STAR_ADD_PLANET = "star_add_planet";
        private const string STAR_GET_STARS = "star_get_stars";
        private const string STAR_GET_PLANETS = "star_get_planets";
        // private const string STAR_ADD_MOON = "star_add_moon";
        // private const string STAR_GET_MOONS = "star_get_moons";

        public string ProviderKey { get; set; }

        public StarCore(HoloNETClientBase holoNETClient, string providerKey) : base(holoNETClient, STAR_CORE_ZOME, STAR_HOLON_TYPE, providerKey)
        {
            ProviderKey = providerKey;
        }

        public StarCore(string holochainConductorURI, HoloNETClientType type, string providerKey) : base(holochainConductorURI, type, STAR_CORE_ZOME, STAR_HOLON_TYPE, providerKey)
        {
            ProviderKey = providerKey;
        }

        public StarCore(HoloNETClientBase holoNETClient) : base(holoNETClient, STAR_CORE_ZOME, STAR_HOLON_TYPE)
        {

        }

        public StarCore(string holochainConductorURI, HoloNETClientType type) : base(holochainConductorURI, type, STAR_CORE_ZOME, STAR_HOLON_TYPE)
        {

        }
        */

        public IStar Star { get; set; }

        public StarCore(IStar star) : base()
        {
            this.Star = star;
        }

        public StarCore(Dictionary<ProviderType, string> providerKey, IStar star) : base(providerKey)
        {
            this.Star = star;
        }

        public StarCore(Guid id, IStar star) : base(id)
        {
            this.Star = star;
        }

        //ONLY SUPERSTAR CAN HAVE A COLLECTION OF OTHER STARS.

        //public async Task<IStar> AddStarAsync(IStar star)
        //{
        //    //TODO: Do we want to add the new star to the main star? Can a Star have a collection of Stars?
        //    // Yes, I think we do, but that means if we can create Stars, then the first main star needs to be either SuperStar, BlueStar or GreatCentralSun! ;-)
        //    // Then SuperStar/BlueStar/GreatCentralSun is the only object that can contain a collection of other stars. Normal Stars only contain collections of planets.
        //    // I feel GreatCentralSun would be best because it then accurately models the Galaxy/Universe! ;-)

        //    //TODO: SO.... tomorrow need to rename the existing Star to GreatCentralSun and then create a normal Star...
        //    // Think StarBody can be renamed to Star and Star renamed to GreatCentralSun...

        //    return (IStar)await base.SaveHolonAsync((IHolon)star);
        //    //return (IPlanet)await base.CallZomeFunctionAsync(STAR_ADD_STAR, planet);
        //}

        public async Task<OASISResult<IPlanet>> AddPlanetAsync(IPlanet planet)
        {
            OASISResult<ICelestialBody> result = new OASISResult<ICelestialBody>();

            if (this.Star.Planets == null)
                this.Star.Planets = new List<IPlanet>();

            this.Star.Planets.Add(planet);
            result = await this.Star.SaveAsync();

            // TODO: This will only work if the planet names are unique (which we want to enforce anyway!) - need to add this soon!
            IPlanet savedPlanet = this.Star.Planets.FirstOrDefault(x => x.Name == planet.Name);
            return new OASISResult<IPlanet>() { Result = savedPlanet, Message = result.Message, IsError = result.IsError };

            // Alternative way is to save the planet first and then then the star (as code below does):

            /*
            // If the planet has now been saved yet then save it now.
            if (planet.Id == Guid.Empty)
                result = await planet.Save();

            if (result.IsError)
                return new OASISResult<IPlanet>() { Result = (IPlanet)result.Result, ErrorMessage = result.ErrorMessage, IsError = result.IsError };
            else
            {
                planet = (IPlanet)result.Result;

                if (this.Star.Planets == null)
                    this.Star.Planets = new List<IPlanet>();

                this.Star.Planets.Add(planet);
                result = await this.Star.Save(); //TODO: Even though this will save the planet and all its child zomes and holons, we want to save the planet as its own indiviudal holon (and not just be embedded as a child object). This also applies to saving each child zome/holon indivudally so we can get a unique id for each. But this depends on how each OASIS Provider implements this. This is where graphDB (neo4j) would be better?
                return new OASISResult<IPlanet>() { Result = planet, ErrorMessage = result.ErrorMessage, IsError = result.IsError };
            }*/
        }


        //ONLY SUPERSTAR CAN HAVE A COLLECTION OF OTHER STARS.

        //public async Task<List<IStar>> GetStars()
        //{
        //    if (string.IsNullOrEmpty(ProviderKey))
        //        throw new System.ArgumentException("ERROR: ProviderKey is null, please set this before calling this method.", "ProviderKey");

        //    return (List<IStar>)await base.LoadHolonsAsync(ProviderKey, API.Core.HolonType.Star);
        //    //return (List<IMoon>)await base.CallZomeFunctionAsync(STAR_GET_STARS, ProviderKey);
        //}

        public async Task<List<IPlanet>> GetPlanets()
        {
            return (List<IPlanet>)await base.LoadHolonsAsync(ProviderKey, HolonType.Planet);
        }

        //TODO: I think we need to also add back in these Moon functions because Star can also create Moons...
        // BUT I THINK ONLY A SUPERSTAR CAN CREATE MOONS?
        // THINK A NORMAL STAR CAN GET COLLECTION OF MOONS AND PLANETS THAT BELONG TO ITS SOLAR SYSTEM?

        //public async Task<IMoon> AddMoonAsync(IMoon moon)
        //{
        //    return (IMoon)await base.CallZomeFunctionAsync(STAR_ADD_MOON, moon);
        //}

        //public async Task<List<IMoon>> GetMoons()
        //{
        //    if (string.IsNullOrEmpty(ProviderKey))
        //        throw new System.ArgumentException("ERROR: ProviderKey is null, please set this before calling this method.", "ProviderKey");

        //    return (List<IMoon>)await base.CallZomeFunctionAsync(STAR_GET_MOONS, ProviderKey);
        //}     
    }
}