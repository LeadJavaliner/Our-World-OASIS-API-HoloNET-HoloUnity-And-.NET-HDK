﻿using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using EOSNewYork.EOSCore.Response.API;
using Nethereum.Contracts;
using Console = System.Console;
//using Spectre.Console;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Interfaces.STAR;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Events;
using NextGenSoftware.OASIS.API.Core.Holons;
using NextGenSoftware.OASIS.API.Core.Managers;
using NextGenSoftware.OASIS.API.Core.Objects;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Providers.SEEDSOASIS.Membranes;
using NextGenSoftware.OASIS.STAR.ErrorEventArgs;
using NextGenSoftware.OASIS.STAR.CelestialBodies;
using NextGenSoftware.OASIS.STAR.Zomes;

namespace NextGenSoftware.OASIS.STAR.TestHarness
{
    class Program
    {
        private static Planet _ourWorld;
        private static Spinner _spinner = new Spinner();
        private static string _privateKey = ""; //Set to privatekey when testing BUT remember to remove again before checking in code! Better to use avatar methods so private key is retreived from avatar and then no need to pass them in.

        static async Task Main(string[] args)
        {
            try
            {
                string dnaFolder = "C:\\CODE\\Our-World-OASIS-API-HoloNET-HoloUnity-And-.NET-HDK\\NextGenSoftware.OASIS.STAR.TestHarness\\CelestialBodyDNA";
                string cSharpGeneisFolder = "C:\\CODE\\Our-World-OASIS-API-HoloNET-HoloUnity-And-.NET-HDK\\NextGenSoftware.OASIS.STAR.TestHarness\\bin\\Release\\net5.0\\Genesis\\CSharp";
                string rustGenesisFolder = "C:\\CODE\\Our-World-OASIS-API-HoloNET-HoloUnity-And-.NET-HDK\\NextGenSoftware.OASIS.STAR.TestHarness\\bin\\Release\\net5.0\\Genesis\\Rust";

                ShowHeader();
                ShowMessage("", false);

                // TODO: Not sure what events should expose on Star, StarCore and HoloNETClient?
                // I feel the events should at least be on the Star object, but then they need to be on the others to bubble them up (maybe could be hidden somehow?)
                SuperStar.OnZomeError += Star_OnZomeError;
                SuperStar.OnHolonLoaded += Star_OnHolonLoaded;
                SuperStar.OnHolonsLoaded += Star_OnHolonsLoaded;
                SuperStar.OnHolonSaved += Star_OnHolonSaved;
                SuperStar.OnSuperStarIgnited += SuperStar_OnSuperStarIgnited;
                SuperStar.OnSuperStarError += SuperStar_OnSuperStarError;
                SuperStar.OnSuperStarStatusChanged += SuperStar_OnSuperStarStatusChanged;
                SuperStar.OnOASISBooted += SuperStar_OnOASISBooted;
                SuperStar.OnOASISBootError += SuperStar_OnOASISBootError;

                OASISResult<ICelestialBody> result = SuperStar.IgniteSuperStar();

                if (result.IsError)
                    ShowErrorMessage(string.Concat("Error Igniting SuperStar. Error Message: ", result.Message));
                else
                {
                    //Console.ForegroundColor = ConsoleColor.Yellow;

                    if (!GetConfirmation("Do you have an existing avatar? "))
                        CreateAvatar();
                    else
                        ShowMessage("", false);

                    LoginAvatar();

                    ShowMessage("", false);
                    Colorful.Console.WriteAscii(" READY PLAYER ONE?", Color.Green);
                    ShowMessage("", false);

                    await Test(dnaFolder, cSharpGeneisFolder, rustGenesisFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                ShowErrorMessage(string.Concat("An unknown error has occured. Error Details: ", ex.ToString()));
                //AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            }
        }

        private static void SuperStar_OnSuperStarStatusChanged(object sender, EventArgs.SuperStarStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case Enums.SuperStarStatus.BootingOASIS:
                    ShowWorkingMessage("BOOTING OASIS...");
                    break;

                case Enums.SuperStarStatus.OASISBooted:
                    ShowSuccessMessage("OASIS BOOTED");
                    break;

                case Enums.SuperStarStatus.Igniting:
                    ShowWorkingMessage("IGNITING SUPERSTAR..."); 
                    break;

                case Enums.SuperStarStatus.Ingited:
                    ShowSuccessMessage("SUPERSTAR IGNITED");
                    break;

                    //case Enums.SuperStarStatus.Error:
                    //  ShowErrorMessage("SuperStar Error");
            }
        }

        private static void SuperStar_OnOASISBootError(object sender, OASISBootErrorEventArgs e)
        {
            //ShowErrorMessage(string.Concat("OASIS Boot Error. Reason: ", e.ErrorReason));
            ShowErrorMessage(e.ErrorReason);
        }

        private static void SuperStar_OnOASISBooted(object sender, EventArgs.OASISBootedEventArgs e)
        {
            //ShowSuccessMessage(string.Concat("OASIS BOOTED.", e.Message));
        }

        private static void SuperStar_OnSuperStarError(object sender, EventArgs.SuperStarErrorEventArgs e)
        {
           // ShowErrorMessage(string.Concat("Error Igniting SuperStar. Reason: ", e.Reason));
        }

        private static void SuperStar_OnSuperStarIgnited(object sender, System.EventArgs e)
        {
            //ShowSuccessMessage("SUPERSTAR IGNITED");
        }

        private static async Task Test(string dnaFolder, string cSharpGeneisFolder, string rustGenesisFolder)
        {
            // Create Planet (OAPP) by generating dynamic template/scaffolding code.
            ShowWorkingMessage("Generating Planet Our World...");
            CoronalEjection result = SuperStar.LightAsync(GenesisType.Planet, "Our World", dnaFolder, cSharpGeneisFolder, rustGenesisFolder, "NextGenSoftware.Holochain.HoloNET.HDK.Core.TestHarness.Genesis").Result;

            if (result.ErrorOccured)
                ShowErrorMessage(string.Concat(" ERROR OCCURED. Error Message: ", result.Message));
            else
            {
                ShowSuccessMessage(" Planet Our World Generated.");
                _ourWorld = result.CelestialBody as Planet;

                Console.WriteLine("");
                Console.WriteLine(string.Concat(" Id: ", _ourWorld.Id));
                Console.WriteLine(string.Concat(" CreatedByAvatarId: ", _ourWorld.CreatedByAvatarId));
                Console.WriteLine(string.Concat(" CreatedDate: ", _ourWorld.CreatedDate));
                Console.WriteLine("");
                Console.WriteLine(string.Concat(" Planet contains ", _ourWorld.CelestialBodyCore.Zomes.Count(), " Zomes: "));

                foreach (Zome zome in _ourWorld.CelestialBodyCore.Zomes)
                {
                    Console.WriteLine(string.Concat("  Zome Name: ", zome.Name, " Zome Id: ", zome.Id, " containing ", zome.Holons.Count(), " holons:"));

                    foreach (Holon holon in zome.Holons)
                    {
                        Console.WriteLine("");
                        Console.WriteLine(string.Concat("   Holon Name: ", holon.Name, " Holon Id: ", holon.Id, " containing ", holon.Nodes.Count(), " nodes: "));

                        foreach (Node node in holon.Nodes)
                        {
                            Console.WriteLine("");
                            Console.WriteLine(string.Concat("    Node Name: ", node.NodeName, " Node Id: ", node.Id, " Node Type: ", Enum.GetName(node.NodeType)));
                        }
                    }
                }

                _ourWorld.OnHolonLoaded += OurWorld_OnHolonLoaded;
                _ourWorld.OnHolonSaved += OurWorld_OnHolonSaved;
                _ourWorld.OnZomeError += OurWorld_OnZomeError;

                ShowWorkingMessage("Loading Zomes & Holons...");
                //_ourWorld.LoadAllAsync();
                await _ourWorld.LoadZomesAsync();
                _spinner.Stop();

                Holon newHolon = new Holon();
                newHolon.Name = "Test Data";
                newHolon.Description = "Test Desc";
                newHolon.HolonType = HolonType.Park;

                ShowWorkingMessage("Saving Holon...");

                // If you are using the generated code from Light above (highly recommended) you do not need to pass the HolonTypeName in, you only need to pass the holon in.
                //ourWorld.CelestialBodyCore.SaveHolonAsync("Test", newHolon);
                await _ourWorld.CelestialBodyCore.SaveHolonAsync(newHolon);


                // BEGIN OASIS API DEMO ***********************************************************************************

                //Set auto-replicate for all providers except IPFS and Neo4j.
                ProviderManager.SetAutoReplicateForAllProviders(true);
                ProviderManager.SetAutoReplicationForProviders(false, new List<ProviderType>() { ProviderType.IPFSOASIS, ProviderType.Neo4jOASIS });

                //Set auto-failover for all providers except Holochain.
                ProviderManager.SetAutoFailOverForAllProviders(true);
                ProviderManager.SetAutoFailOverForProviders(false, new List<ProviderType>() { ProviderType.HoloOASIS });

                //Set auto-load balance for all providers except Ethereum.
                ProviderManager.SetAutoLoadBalanceForAllProviders(true);
                ProviderManager.SetAutoLoadBalanceForProviders(false, new List<ProviderType>() { ProviderType.EthereumOASIS });

                //  Set the default provider to MongoDB.
                ProviderManager.SetAndActivateCurrentStorageProvider(ProviderType.MongoDBOASIS, true); // Set last param to false if you wish only the next call to use this provider.



                //  Give HoloOASIS Store permission for the Name field(the field will only be stored on Holochain).
                SuperStar.OASISAPI.Avatar.Config.FieldToProviderMappings.Name.Add(new ProviderManagerConfig.FieldToProviderMappingAccess { Access = ProviderManagerConfig.ProviderAccess.Store, Provider = ProviderType.HoloOASIS });

                // Give all providers read/write access to the Karma field (will allow them to read and write to the field but it will only be stored on Holochain).
                // You could choose to store it on more than one provider if you wanted the extra redundancy (but not normally needed since Holochain has a lot of redundancy built in).
                SuperStar.OASISAPI.Avatar.Config.FieldToProviderMappings.Karma.Add(new ProviderManagerConfig.FieldToProviderMappingAccess { Access = ProviderManagerConfig.ProviderAccess.ReadWrite, Provider = ProviderType.All });

                //Give Ethereum read-only access to the DOB field.
                SuperStar.OASISAPI.Avatar.Config.FieldToProviderMappings.DOB.Add(new ProviderManagerConfig.FieldToProviderMappingAccess { Access = ProviderManagerConfig.ProviderAccess.ReadOnly, Provider = ProviderType.EthereumOASIS });


                // All calls are load-balanced and have multiple redudancy/fail over for all supported OASIS Providers.
                SuperStar.OASISAPI.Avatar.LoadAllAvatars(); // Load-balanced across all providers.
                SuperStar.OASISAPI.Avatar.LoadAllAvatars(ProviderType.MongoDBOASIS); // Only loads from MongoDB.
                SuperStar.OASISAPI.Avatar.LoadAvatar(SuperStar.LoggedInUser.Id, ProviderType.HoloOASIS); // Only loads from Holochain.
                SuperStar.OASISAPI.Map.CreateAndDrawRouteOnMapBetweenHolons(newHolon, newHolon); // Load-balanced across all providers.

                SuperStar.OASISAPI.Data.LoadHolon(newHolon.Id); // Load-balanced across all providers.
                SuperStar.OASISAPI.Data.LoadHolon(newHolon.Id, ProviderType.IPFSOASIS); // Only loads from IPFS.
                SuperStar.OASISAPI.Data.LoadAllHolons(HolonType.Moon, ProviderType.HoloOASIS); // Loads all moon (OAPPs) from Holochain.
                SuperStar.OASISAPI.Data.SaveHolon(newHolon); // Load-balanced across all providers.
                SuperStar.OASISAPI.Data.SaveHolon(newHolon, ProviderType.EthereumOASIS); //  Only saves to Etherum.

                SuperStar.OASISAPI.Data.LoadAllHolons(HolonType.All, ProviderType.Default); // Loads all parks from current default provider.
                SuperStar.OASISAPI.Data.LoadAllHolons(HolonType.Park, ProviderType.All); // Loads all parks from all providers (load-balanced/fail over).
                SuperStar.OASISAPI.Data.LoadAllHolons(HolonType.Park); // shorthand for above.
                SuperStar.OASISAPI.Data.LoadAllHolons(HolonType.Quest); //  Loads all quests from all providers.
                SuperStar.OASISAPI.Data.LoadAllHolons(HolonType.Restaurant); //  Loads all resaurants from all providers.

                // Holochain Support
                await SuperStar.OASISAPI.Providers.Holochain.HoloNETClient.CallZomeFunctionAsync(SuperStar.OASISAPI.Providers.Holochain.HoloNETClient.AgentPubKey, "our_world_core", "load_holons", null);

                // IPFS Support
                await SuperStar.OASISAPI.Providers.IPFS.IPFSEngine.FileSystem.ReadFileAsync("");
                await SuperStar.OASISAPI.Providers.IPFS.IPFSEngine.FileSystem.AddFileAsync("");
                await SuperStar.OASISAPI.Providers.IPFS.IPFSEngine.Swarm.PeersAsync();
                await SuperStar.OASISAPI.Providers.IPFS.IPFSEngine.KeyChainAsync();
                await SuperStar.OASISAPI.Providers.IPFS.IPFSEngine.Dns.ResolveAsync("test");
                await SuperStar.OASISAPI.Providers.IPFS.IPFSEngine.Dag.GetAsync(new Ipfs.Cid() { Hash = "" });
                await SuperStar.OASISAPI.Providers.IPFS.IPFSEngine.Dag.PutAsync(new Ipfs.Cid() { Hash = "" });

                // Ethereum Support
                await SuperStar.OASISAPI.Providers.Ethereum.Web3.Client.SendRequestAsync(new Nethereum.JsonRpc.Client.RpcRequest("id", "test"));
                await SuperStar.OASISAPI.Providers.Ethereum.Web3.Eth.Blocks.GetBlockNumber.SendRequestAsync("");
                Contract contract = SuperStar.OASISAPI.Providers.Ethereum.Web3.Eth.GetContract("abi", "contractAddress");

                // EOSIO Support
                SuperStar.OASISAPI.Providers.EOSIO.ChainAPI.GetTableRows("accounts", "accounts", "users", "true", 0, 0, 1, 3);
                SuperStar.OASISAPI.Providers.EOSIO.ChainAPI.GetBlock("block");
                SuperStar.OASISAPI.Providers.EOSIO.ChainAPI.GetAccount("test.account");
                SuperStar.OASISAPI.Providers.EOSIO.ChainAPI.GetCurrencyBalance("test.account", "", "");

                // Graph DB Support
                await SuperStar.OASISAPI.Providers.Neo4j.GraphClient.Cypher.Merge("(a:Avatar { Id: avatar.Id })").OnCreate().Set("a = avatar").ExecuteWithoutResultsAsync(); //Insert/Update Avatar.
                Avatar newAvatar = SuperStar.OASISAPI.Providers.Neo4j.GraphClient.Cypher.Match("(p:Avatar {Username: {nameParam}})").WithParam("nameParam", "davidellams@hotmail.com").Return(p => p.As<Avatar>()).ResultsAsync.Result.Single(); //Load Avatar.

                // Document/Object DB Support
                SuperStar.OASISAPI.Providers.MongoDB.Database.MongoDB.ListCollectionNames();
                SuperStar.OASISAPI.Providers.MongoDB.Database.MongoDB.GetCollection<Avatar>("testCollection");

                // SEEDS Support
                Console.WriteLine(" Getting Balance for account davidsellams...");
                string balance = SuperStar.OASISAPI.Providers.SEEDS.GetBalanceForTelosAccount("davidsellams");
                Console.WriteLine(string.Concat(" Balance: ", balance));

                Console.WriteLine(" Getting Balance for account nextgenworld...");
                balance = SuperStar.OASISAPI.Providers.SEEDS.GetBalanceForTelosAccount("nextgenworld");
                Console.WriteLine(string.Concat(" Balance: ", balance));

                Console.WriteLine(" Getting Account for account davidsellams...");
                Account account = SuperStar.OASISAPI.Providers.SEEDS.TelosOASIS.GetTelosAccount("davidsellams");
                Console.WriteLine(string.Concat(" Account.account_name: ", account.account_name));
                Console.WriteLine(string.Concat(" Account.created: ", account.created_datetime.ToString()));

                Console.WriteLine(" Getting Account for account nextgenworld...");
                account = SuperStar.OASISAPI.Providers.SEEDS.TelosOASIS.GetTelosAccount("nextgenworld");
                Console.WriteLine(string.Concat(" Account.account_name: ", account.account_name));
                Console.WriteLine(string.Concat(" Account.created: ", account.created_datetime.ToString()));

                // Check that the Telos account name is linked to the avatar and link it if it is not (PayWithSeeds will fail if it is not linked when it tries to add the karma points).
                if (!SuperStar.LoggedInUser.ProviderKey.ContainsKey(ProviderType.TelosOASIS))
                    SuperStar.OASISAPI.Avatar.LinkProviderKeyToAvatar(SuperStar.LoggedInUser.Id, ProviderType.TelosOASIS, "davidsellams");

                Console.WriteLine(" Sending SEEDS from nextgenworld to davidsellams...");
                OASISResult<string> payWithSeedsResult = SuperStar.OASISAPI.Providers.SEEDS.PayWithSeedsUsingTelosAccount("davidsellams", _privateKey, "nextgenworld", 1, KarmaSourceType.API, "test", "test", "test", "test memo");
                Console.WriteLine(string.Concat(" Success: ", payWithSeedsResult.IsError ? "false" : "true"));

                if (payWithSeedsResult.IsError)
                    Console.WriteLine(string.Concat(" Error Message: ", payWithSeedsResult.Message));

                Console.WriteLine(string.Concat(" Result: ", payWithSeedsResult.Result));

                Console.WriteLine(" Getting Balance for account davidsellams...");
                balance = SuperStar.OASISAPI.Providers.SEEDS.GetBalanceForTelosAccount("davidsellams");
                Console.WriteLine(string.Concat(" Balance: ", balance));

                Console.WriteLine(" Getting Balance for account nextgenworld...");
                balance = SuperStar.OASISAPI.Providers.SEEDS.GetBalanceForTelosAccount("nextgenworld");
                Console.WriteLine(string.Concat(" Balance: ", balance));

                Console.WriteLine(" Getting Organsiations...");
                string orgs = SuperStar.OASISAPI.Providers.SEEDS.GetAllOrganisationsAsJSON();
                Console.WriteLine(string.Concat(" Organisations: ", orgs));

                //Console.WriteLine("Getting nextgenworld organsiation...");
                //string org = OASISAPI.Providers.SEEDS.GetOrganisation("nextgenworld");
                //Console.WriteLine(string.Concat("nextgenworld org: ", org));

                Console.WriteLine(" Generating QR Code for davidsellams...");
                string qrCode = SuperStar.OASISAPI.Providers.SEEDS.GenerateSignInQRCode("davidsellams");
                Console.WriteLine(string.Concat(" SEEDS Sign-In QRCode: ", qrCode));

                Console.WriteLine(" Sending invite to davidsellams...");
                OASISResult<SendInviteResult> sendInviteResult = SuperStar.OASISAPI.Providers.SEEDS.SendInviteToJoinSeedsUsingTelosAccount("davidsellams", _privateKey, "davidsellams", 1, 1, KarmaSourceType.API, "test", "test", "test");
                Console.WriteLine(string.Concat(" Success: ", sendInviteResult.IsError ? "false" : "true"));

                if (sendInviteResult.IsError)
                    Console.WriteLine(string.Concat(" Error Message: ", sendInviteResult.Message));
                else
                {
                    Console.WriteLine(string.Concat(" Invite Sent To Join SEEDS. Invite Secret: ", sendInviteResult.Result.InviteSecret, ". Transction ID: ", sendInviteResult.Result.TransactionId));

                    Console.WriteLine(" Accepting invite to davidsellams...");
                    OASISResult<string> acceptInviteResult = SuperStar.OASISAPI.Providers.SEEDS.AcceptInviteToJoinSeedsUsingTelosAccount("davidsellams", sendInviteResult.Result.InviteSecret, KarmaSourceType.API, "test", "test", "test");
                    Console.WriteLine(string.Concat("Success: ", acceptInviteResult.IsError ? "false" : "true"));

                    if (acceptInviteResult.IsError)
                        Console.WriteLine(string.Concat(" Error Message: ", acceptInviteResult.Message));
                    else
                        Console.WriteLine(string.Concat(" Invite Accepted To Join SEEDS. Transction ID: ", acceptInviteResult.Result));
                }
                // ThreeFold, AcivityPub, SOLID, Cross/Off Chain, Smart Contract Interoperability & lots more coming soon! :)

                // END OASIS API DEMO ***********************************************************************************



                // Build
                CoronalEjection ejection = _ourWorld.Flare();
                //OR
                //CoronalEjection ejection = Star.Flare(ourWorld);

                // Activate & Launch - Launch & activate the planet (OAPP) by shining the star's light upon it...
                SuperStar.Shine(_ourWorld);
                _ourWorld.Shine();

                // Deactivate the planet (OAPP)
                SuperStar.Dim(_ourWorld);

                // Deploy the planet (OAPP)
                SuperStar.Seed(_ourWorld);

                // Run Tests
                SuperStar.Twinkle(_ourWorld);

                // Highlight the Planet (OAPP) in the OAPP Store (StarNET). *Admin Only*
                SuperStar.Radiate(_ourWorld);

                // Show how much light the planet (OAPP) is emitting into the solar system (StarNET/HoloNET)
                SuperStar.Emit(_ourWorld);

                // Show stats of the Planet (OAPP).
                SuperStar.Reflect(_ourWorld);

                // Upgrade/update a Planet (OAPP).
                SuperStar.Evolve(_ourWorld);

                // Import/Export hApp, dApp & others.
                SuperStar.Mutate(_ourWorld);

                // Send/Receive Love
                SuperStar.Love(_ourWorld);

                // Show network stats/management/settings
                SuperStar.Burst(_ourWorld);

                // Reserved For Future Use...
                SuperStar.Super(_ourWorld);

                // Delete a planet (OAPP).
                SuperStar.Dust(_ourWorld);
            }
        }

        //private static void Star_OnStarError(object sender, StarErrorEventArgs e)
        //{
        //    ShowErrorMessage(string.Concat(" Star Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails, "EndPoint: ", e.EndPoint));
        //}

        //private static void HoloNETClient_OnError(object sender, Client.Core.HoloNETErrorEventArgs e)
        //{
        //    Console.WriteLine(string.Concat("HoloNET Client Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails, "EndPoint: ", e.EndPoint));
        //}

        private static void StarCore_OnZomeError(object sender, ZomeErrorEventArgs e)
        {
            //Console.WriteLine(string.Concat("Star Core Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails, "HoloNETErrorDetails.Reason: ", e.HoloNETErrorDetails.Reason, "HoloNETErrorDetails.ErrorDetails: ", e.HoloNETErrorDetails.ErrorDetails));
            ShowErrorMessage(string.Concat(" Star Core Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails));
        }

        private static void Star_OnInitialized(object sender, System.EventArgs e)
        {
            ShowSuccessMessage(" Star Initialized.");
        }

        private static void Star_OnHolonSaved(object sender, HolonSavedEventArgs e)
        {
            if (e.Result.IsError)
                ShowErrorMessage(e.Result.Message);
            else
                ShowSuccessMessage(string.Concat(" Star Holons Saved. Holon Saved: ", e.Result.Result.Name));
        }

        private static void Star_OnHolonsLoaded(object sender, HolonsLoadedEventArgs e)
        {
            ShowSuccessMessage(string.Concat(" Star Holons Loaded. Holons Loaded: ", e.Result.Result.Count()));
        }

        private static void Star_OnHolonLoaded(object sender, HolonLoadedEventArgs e)
        {
            ShowSuccessMessage(string.Concat(" Star Holons Loaded. Holon Name: ", e.Result.Result.Name));
        }

        private static void Star_OnZomeError(object sender, ZomeErrorEventArgs e)
        {
            //Console.WriteLine(string.Concat("Star Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails, "HoloNETErrorDetails.Reason: ", e.HoloNETErrorDetails.Reason, "HoloNETErrorDetails.ErrorDetails: ", e.HoloNETErrorDetails.ErrorDetails));
            ShowErrorMessage(string.Concat(" Star Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails));
        }

        private static void OurWorld_OnZomeError(object sender, ZomeErrorEventArgs e)
        {
            //Console.WriteLine(string.Concat("Our World Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails, "HoloNETErrorDetails.Reason: ", e.HoloNETErrorDetails.Reason, "HoloNETErrorDetails.ErrorDetails: ", e.HoloNETErrorDetails.ErrorDetails));
            ShowErrorMessage(string.Concat(" Our World Error Occured. EndPoint: ", e.EndPoint, ". Reason: ", e.Reason, ". Error Details: ", e.ErrorDetails));
        }

        private static void OurWorld_OnHolonSaved(object sender, HolonSavedEventArgs e)
        {
            if (e.Result.IsError)
                ShowErrorMessage(e.Result.Message);
            else
            {
                Console.WriteLine(" Holon Saved");
                Console.WriteLine(string.Concat(" Holon Id: ", e.Result.Result.Id));
                Console.WriteLine(string.Concat(" Holon ProviderKey: ", e.Result.Result.ProviderKey));
                Console.WriteLine(string.Concat(" Holon Name: ", e.Result.Result.Name));
                Console.WriteLine(string.Concat("Holon Type: ", e.Result.Result.HolonType));
                Console.WriteLine(string.Concat(" Holon Description: ", e.Result.Result.Description));

                Console.WriteLine(" Loading Holon...");
                //ourWorld.CelestialBodyCore.LoadHolonAsync(e.Holon.Name, e.Holon.ProviderKey);
                _ourWorld.CelestialBodyCore.LoadHolonAsync(e.Result.Result.Id);
            }
        }

        private static void OurWorld_OnHolonLoaded(object sender, HolonLoadedEventArgs e)
        {
            Console.WriteLine(" Holon Loaded");
            Console.WriteLine(string.Concat(" Holon Id: ", e.Result.Result.Id));
            Console.WriteLine(string.Concat(" Holon ProviderKey: ", e.Result.Result.ProviderKey));
            Console.WriteLine(string.Concat(" Holon Name: ", e.Result.Result.Name));
            Console.WriteLine(string.Concat(" Holon Type: ", e.Result.Result.HolonType));
            Console.WriteLine(string.Concat(" Holon Description: ", e.Result.Result.Description));

            //Console.WriteLine(string.Concat("ourWorld.Zomes[0].Holons[0].ProviderKey: ", ourWorld.Zomes[0].Holons[0].ProviderKey));
            Console.WriteLine(string.Concat(" ourWorld.Zomes[0].Holons[0].ProviderKey: ", _ourWorld.CelestialBodyCore.Zomes[0].Holons[0].ProviderKey));
        }

        private static void ShowColoursAvailable()
        {
            ShowMessage("", false);
            ConsoleColor oldColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" Sorry, that colour is not valid. Please try again. The colour needs to be one of the following: ");

            string[] values = Enum.GetNames(typeof(ConsoleColor));

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "Black")
                {
                    PrintColour(values[i]);

                    if (i < values.Length - 2)
                        Console.Write(", ");

                    else if (i == values.Length - 2)
                        Console.Write(" or ");
                }
            }

            ShowMessage("", false);
            Console.ForegroundColor = oldColour;
        }

        private static void PrintColour(string colour)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colour);
            Console.Write(colour);
        }

        private static void ShowSuccessMessage(string message, bool lineSpace = true)
        {
            if (_spinner.IsActive)
            {
                _spinner.Stop();
                Console.WriteLine("");
            }

            ConsoleColor existingColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            if (lineSpace)
                Console.WriteLine("");

            Console.WriteLine(string.Concat(" ", message));
            Console.ForegroundColor = existingColour;

            //if (SuperStar.LoggedInUser != null)
            //    Console.ForegroundColor = SuperStar.LoggedInUser.STARCLIColour;
            //else
            //    Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void ShowMessage(string message, bool lineSpace = true, bool noLineBreaks = false)
        {
            if (lineSpace)
                Console.WriteLine(" ");
            
            if (noLineBreaks)
                Console.Write(string.Concat(" ", message));
            else
                Console.WriteLine(string.Concat(" ", message));
        }

        private static void ShowWorkingMessage(string message, bool lineSpace = true)
        {
            if (_spinner.IsActive)
            {
                _spinner.Stop();
                Console.WriteLine("");
            }

            if (lineSpace)
                Console.WriteLine(" ");

            Console.Write(string.Concat(" ", message));
            _spinner.Start();
        }

        private static void ShowErrorMessage(string message, bool lineSpace = true)
        {
            if (_spinner.IsActive)
            {
                _spinner.Stop();
                Console.WriteLine("");
            }

            ConsoleColor existingColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            
            if (lineSpace)
                Console.WriteLine("");

            Console.WriteLine(string.Concat(" ", message));
            Console.ForegroundColor = existingColour;

            //if (SuperStar.LoggedInUser != null)
            //    Console.ForegroundColor = SuperStar.LoggedInUser.STARCLIColour;
            //else
            //    Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static string GetValidTitle(string message)
        {
            string title = GetValidInput(message).ToUpper();
            //string[] validTitles = new string[5] { "Mr", "Mrs", "Ms", "Miss", "Dr" };
            string validTitles = "MR,MRS,MS,MISS,DR";

            bool titleValid = false;
            while (!titleValid)
            {
                if (!validTitles.Contains(title))
                {
                    ShowErrorMessage("Title invalid. Please try again.");
                    title = GetValidInput(message).ToUpper();
                }
                else
                    titleValid = true;
            }

            return ExtensionMethods.ExtensionMethods.ToPascalCase(title);
        }

        private static string GetValidInput(string message)
        {
            string input = "";
            while (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                ShowMessage(string.Concat("", message), true, true);
                input = Console.ReadLine();
            }

            return input;
        }

        private static bool GetConfirmation(string message)
        {
            bool validKey = false;
            bool confirm = false;

            while (!validKey)
            {
                //ShowMessage("", false);
                ShowMessage(message, true, true);
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.Y)
                {
                    confirm = true;
                    validKey = true;
                }

                if (key == ConsoleKey.N)
                {
                    confirm = false;
                    validKey = true;
                }
            }

            return confirm;
        }

        private static string GetValidEmail(string message, bool checkIfEmailAlreadyInUse)
        {
            bool emailValid = false;
            string email = "";

            while (!emailValid)
            {
                ShowMessage(string.Concat("", message), true, true);
                email = Console.ReadLine();

                if (!ValidationHelper.IsValidEmail(email))
                    ShowErrorMessage("That email is not valid. Please try again.");

                else if (checkIfEmailAlreadyInUse)
                {
                    ShowWorkingMessage("Checking if email already in use...");

                    if (SuperStar.OASISAPI.Avatar.CheckIfEmailIsAlreadyInUse(email))
                        ShowErrorMessage("Sorry, that email is already in use, please use another one.");
                    else
                    {
                        emailValid = true;
                        _spinner.Stop();
                        ShowMessage("", false);
                    }
                }
                else
                    emailValid = true;
            }

            return email;
        }

        private static string GetValidPassword()
        {
            string password = "";
            string password2 = "";
            ShowMessage("", false);

            while ((string.IsNullOrEmpty(password) && string.IsNullOrEmpty(password2)) || password != password2)
            {
                password = ReadPassword("What is the password you wish to use? ");
                password2 = ReadPassword("Please confirm password: ");

                if (password != password2)
                    ShowErrorMessage("The passwords do not match. Please try again.");
            }

            return password;
        }

        private static string ReadPassword(string message)
        {
            string password = "";
            ConsoleKey key;

            while (string.IsNullOrEmpty(password) && string.IsNullOrWhiteSpace(password))
            {
                ShowMessage(string.Concat("", message), true, true);

                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        Console.Write("\b \b");
                        password = password[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        password += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                ShowMessage("", false);
            }

            return password;
        }

        private static void GetValidColour(ref ConsoleColor favColour, ref ConsoleColor cliColour)
        {
            bool colourSet = false;
            while (!colourSet)
            {
                ShowMessage("What is your favourite colour? ", true, true);
                string colour = Console.ReadLine();
                colour = ExtensionMethods.ExtensionMethods.ToPascalCase(colour);
                object colourObj = null;

                if (Enum.TryParse(typeof(ConsoleColor), colour, out colourObj))
                {
                    favColour = (ConsoleColor)colourObj;
                    Console.ForegroundColor = favColour;
                    ShowMessage("Do you prefer to use your favourite colour? :) ", true, true);

                    if (Console.ReadKey().Key != ConsoleKey.Y)
                    {
                        Console.WriteLine("");

                        while (!colourSet)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            ShowMessage("Which colour would you prefer? ", true, true);
                            colour = Console.ReadLine();
                            colour = ExtensionMethods.ExtensionMethods.ToPascalCase(colour);
                            colourObj = null;

                            if (Enum.TryParse(typeof(ConsoleColor), colour, out colourObj))
                            {
                                cliColour = (ConsoleColor)colourObj;
                                Console.ForegroundColor = cliColour;

                               // ShowMessage("", false);
                                ShowMessage("This colour ok? ", true, true);

                                if (Console.ReadKey().Key == ConsoleKey.Y)
                                    colourSet = true;
                                else
                                    ShowMessage("", false);
                            }
                            else
                                ShowColoursAvailable();
                        }
                    }
                    else
                        colourSet = true;
                }
                else
                    ShowColoursAvailable();
            }
        }

        private static void CreateAvatar()
        {
            ConsoleColor favColour = ConsoleColor.Green;
            ConsoleColor cliColour = ConsoleColor.Green;

            ShowMessage("");
            ShowMessage("Please create an avatar below:", false);

            string title = GetValidTitle("What is your title? ");
            string firstName = GetValidInput("What is your first name? ");
            ShowMessage(string.Concat("Nice to meet you ", firstName, ". :)"));
            string lastName = GetValidInput(string.Concat("What is your last name ", firstName, "? "));
            string email = GetValidEmail("What is your email address? ", true);
            GetValidColour(ref favColour, ref cliColour);
            string password = GetValidPassword();
            ShowWorkingMessage("Creating Avatar...");

            OASISResult<IAvatar> createAvatarResult = SuperStar.CreateAvatar(title, firstName, lastName, email, password, cliColour, favColour);
            ShowMessage("");

            if (createAvatarResult.IsError)
                ShowErrorMessage(string.Concat("Error creating avatar. Error message: ", createAvatarResult.Message));
            else
                ShowSuccessMessage("Successfully Created Avatar. Please Check Your Email To Verify Your Account Before Logging In.");
        }

        private static void ShowHeader()
        {
            var versionString = Assembly.GetEntryAssembly()
                                           .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                           .InformationalVersion
                                           .ToString();

            // Console.SetWindowSize(300, Console.WindowHeight);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("************************************************************************************************");
            Console.Write(" NextGen Software");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" STAR");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" (Synergiser Transformer Aggregator Resolver) HDK/ODK TEST HARNESS v{versionString} ");
            Console.WriteLine("");
            Console.WriteLine("************************************************************************************************");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("                  ,O,");
            Console.WriteLine("                 ,OOO,");
            Console.WriteLine("           'oooooOOOOOooooo'");
            Console.WriteLine("             `OOOOOOOOOOO`");
            Console.WriteLine("               `OOOOOOO`");
            Console.WriteLine("               OOOO'OOOO");
            Console.WriteLine("              OOO'   'OOO");
            Console.WriteLine("             O'         'O");

            /*
            Image Picture = Image.FromFile("images/star6b.jpg");
            Console.SetBufferSize((Picture.Width * 0x2), (Picture.Height * 0x2));
            //Console.SetBufferSize((Picture.Width), (Picture.Height));
            Console.WindowWidth = 100; //180
            //Console.WindowHeight = 61;

            FrameDimension Dimension = new FrameDimension(Picture.FrameDimensionsList[0x0]);
            int FrameCount = Picture.GetFrameCount(Dimension);
            int Left = Console.WindowLeft, Top = Console.WindowTop;
            char[] Chars = { '#', '#', '@', '%', '=', '+', '*', ':', '-', '.', ' ' };
            Picture.SelectActiveFrame(Dimension, 0x0);
            for (int i = 0x0; i < Picture.Height; i++)
            {
                for (int x = 0x0; x < Picture.Width; x++)
                {
                    Color Color = ((Bitmap)Picture).GetPixel(x, i);
                    int Gray = (Color.R + Color.G + Color.B) / 0x3;
                    int Index = (Gray * (Chars.Length - 0x1)) / 0xFF;
                    Console.Write(Chars[Index]);
                }
                Console.Write('\n');
                Thread.Sleep(50);
            }
            //Console.SetCursorPosition(Left, Top);
            */

            // Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            Colorful.Console.WriteAscii(" STAR", Color.Yellow);

            // var font = FigletFont.Load("fonts/wow.flf");
            // Figlet figlet = new Figlet(font);
            //Colorful.Console.WriteLine(figlet.ToAscii("STAR"), Color.FromArgb(67, 144, 198));
            // Colorful.Console.WriteLine(figlet.ToAscii("STAR"), Color.Yellow);

            ShowCommands();

            Console.WriteLine("");
            Console.Write(" Welcome to");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" STAR");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" (The");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" ♥");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" Of The OASIS)");
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void ShowCommands()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n Usage:");
            Console.WriteLine("   star ignite = Ignite SuperStar & Boot The OASIS");
            Console.WriteLine("   star extinguish = Extinguish SuperStar & Shutdown The OASIS");
            Console.WriteLine("   star beamin = Log in");
            Console.WriteLine("   star beamout = Log out");
            Console.WriteLine("   star light -dnaFolder -cSharpGeneisFolder -rustGenesisFolder = Creates a new Planet (OAPP) at the given folder genesis locations, from the given OAPP DNA.");
            Console.WriteLine("   star light -transmute -hAppDNA -cSharpGeneisFolder -rustGenesisFolder = Creates a new Planet (OAPP) at the given folder genesis locations, from the given hApp DNA.");
            Console.WriteLine("   star flare -planetName = Build a planet (OAPP).");
            Console.WriteLine("   star shine -planetName = Launch & activate a planet (OAPP) by shining the star's light upon it...");
            Console.WriteLine("   star dim -planetName = Deactivate a planet (OAPP).");
            Console.WriteLine("   star seed -planetName = Deploy a planet (OAPP).");
            Console.WriteLine("   star twinkle -planetName = Deactivate a planet (OAPP).");
            Console.WriteLine("   star dust -planetName = Delete a planet (OAPP).");
            Console.WriteLine("   star radiate -planetName = Highlight the Planet (OAPP) in the OAPP Store (StarNET). *Admin Only*");
            Console.WriteLine("   star emit -planetName = Show how much light the planet (OAPP) is emitting into the solar system (StarNET/HoloNET)");
            Console.WriteLine("   star reflect -planetName = Show stats of the Planet (OAPP).");
            Console.WriteLine("   star evolve -planetName = Upgrade/update a Planet (OAPP).");
            Console.WriteLine("   star mutate -planetName = Import/Export hApp, dApp & others.");
            Console.WriteLine("   star love -planetName = Send/Receive Love.");
            Console.WriteLine("   star burst = View network stats/management/settings.");
            Console.WriteLine("   star super - Reserved For Future Use...");
            Console.WriteLine("************************************************************************************************");
        }

        private static void LoginAvatar()
        {
            OASISResult<IAvatar> beamInResult = null;

            while (beamInResult == null || (beamInResult != null && beamInResult.IsError))
            {
                //TODO: TEMP - PUT BACK IN WHEN GOING LIVE!
                /*
                ShowMessage("Please login below:");
                string username = GetValidEmail("Username/Email? ", false);
                string password = ReadPassword("Password? ");
                ShowWorkingMessage("Beaming In...");
                beamInResult = SuperStar.BeamIn(username, password);
                */

                ShowWorkingMessage("Beaming In...");
                //beamInResult = SuperStar.BeamIn("davidellams@hotmail.com", "my-super-secret-password");
                beamInResult = SuperStar.BeamIn("davidellams@hotmail.com", "test!");
                ShowMessage("");

                if (beamInResult.IsError)
                {
                    ShowErrorMessage(string.Concat("Error logging in. Error Message: ", beamInResult.Message));

                    if (beamInResult.Message == "Avatar has not been verified. Please check your email.")
                    {
                        ShowErrorMessage("Then either click the link in the email to activate your avatar or enter the validation token contained in the email below:", false);

                        bool validToken = false;
                        while (!validToken)
                        {
                            string token = GetValidInput("Enter validation token: ");
                            ShowWorkingMessage("Verifying Token...");
                            OASISResult<bool> verifyEmailResult = SuperStar.OASISAPI.Avatar.VerifyEmail(token);

                            if (verifyEmailResult.IsError)
                                ShowErrorMessage(verifyEmailResult.Message);
                            else
                            {
                                ShowSuccessMessage("Verification successful, you can now login");
                                validToken = true;
                            }
                        }
                    }
                }

                else if (SuperStar.LoggedInUser == null)
                    ShowErrorMessage("Error Beaming In. Username/Password may be incorrect.");
            }

            ShowSuccessMessage(string.Concat("Successfully Beamed In! Welcome back ", SuperStar.LoggedInUser.FullName, ". Have a nice day! :)"));
            ShowAvatarStats();
        }

        private static void ShowAvatarStats()
        {
            ShowMessage("", false);
            Console.WriteLine(string.Concat(" Karma: ", SuperStar.LoggedInUser.Karma));
            Console.WriteLine(string.Concat(" Level: ", SuperStar.LoggedInUser.Level));
            Console.WriteLine(string.Concat(" XP: ", SuperStar.LoggedInUser.XP));

            Console.WriteLine("");
            Console.WriteLine(" Chakras:");
            Console.WriteLine(string.Concat(" Crown XP: ", SuperStar.LoggedInUser.Chakras.Crown.XP));
            Console.WriteLine(string.Concat(" Crown Level: ", SuperStar.LoggedInUser.Chakras.Crown.Level));
            Console.WriteLine(string.Concat(" ThirdEye XP: ", SuperStar.LoggedInUser.Chakras.ThirdEye.XP));
            Console.WriteLine(string.Concat(" ThirdEye Level: ", SuperStar.LoggedInUser.Chakras.ThirdEye.Level));
            Console.WriteLine(string.Concat(" Throat XP: ", SuperStar.LoggedInUser.Chakras.Throat.XP));
            Console.WriteLine(string.Concat(" Throat Level: ", SuperStar.LoggedInUser.Chakras.Throat.Level));
            Console.WriteLine(string.Concat(" Heart XP: ", SuperStar.LoggedInUser.Chakras.Heart.XP));
            Console.WriteLine(string.Concat(" Heart Level: ", SuperStar.LoggedInUser.Chakras.Heart.Level));
            Console.WriteLine(string.Concat(" SoloarPlexus XP: ", SuperStar.LoggedInUser.Chakras.SoloarPlexus.XP));
            Console.WriteLine(string.Concat(" SoloarPlexus Level: ", SuperStar.LoggedInUser.Chakras.SoloarPlexus.Level));
            Console.WriteLine(string.Concat(" Sacral XP: ", SuperStar.LoggedInUser.Chakras.Sacral.XP));
            Console.WriteLine(string.Concat(" Sacral Level: ", SuperStar.LoggedInUser.Chakras.Sacral.Level));

            Console.WriteLine(string.Concat(" Root SanskritName: ", SuperStar.LoggedInUser.Chakras.Root.SanskritName));
            Console.WriteLine(string.Concat(" Root XP: ", SuperStar.LoggedInUser.Chakras.Root.XP));
            Console.WriteLine(string.Concat(" Root Level: ", SuperStar.LoggedInUser.Chakras.Root.Level));
            Console.WriteLine(string.Concat(" Root Progress: ", SuperStar.LoggedInUser.Chakras.Root.Progress));
           // Console.WriteLine(string.Concat(" Root Color: ", SuperStar.LoggedInUser.Chakras.Root.Color.Name));
            Console.WriteLine(string.Concat(" Root Element: ", SuperStar.LoggedInUser.Chakras.Root.Element.Name));
            Console.WriteLine(string.Concat(" Root YogaPose: ", SuperStar.LoggedInUser.Chakras.Root.YogaPose.Name));
            Console.WriteLine(string.Concat(" Root WhatItControls: ", SuperStar.LoggedInUser.Chakras.Root.WhatItControls));
            Console.WriteLine(string.Concat(" Root WhenItDevelops: ", SuperStar.LoggedInUser.Chakras.Root.WhenItDevelops));
            Console.WriteLine(string.Concat(" Root Crystal Name: ", SuperStar.LoggedInUser.Chakras.Root.Crystal.Name.Name));
            Console.WriteLine(string.Concat(" Root Crystal AmplifyicationLevel: ", SuperStar.LoggedInUser.Chakras.Root.Crystal.AmplifyicationLevel));
            Console.WriteLine(string.Concat(" Root Crystal CleansingLevel: ", SuperStar.LoggedInUser.Chakras.Root.Crystal.CleansingLevel));
            Console.WriteLine(string.Concat(" Root Crystal EnergisingLevel: ", SuperStar.LoggedInUser.Chakras.Root.Crystal.EnergisingLevel));
            Console.WriteLine(string.Concat(" Root Crystal GroundingLevel: ", SuperStar.LoggedInUser.Chakras.Root.Crystal.GroundingLevel));
            Console.WriteLine(string.Concat(" Root Crystal ProtectionLevel: ", SuperStar.LoggedInUser.Chakras.Root.Crystal.ProtectionLevel));

            Console.WriteLine("");
            Console.WriteLine(" Aurua:");
            Console.WriteLine(string.Concat(" Brightness: ", SuperStar.LoggedInUser.Aura.Brightness));
            Console.WriteLine(string.Concat(" Size: ", SuperStar.LoggedInUser.Aura.Size));
            Console.WriteLine(string.Concat(" Level: ", SuperStar.LoggedInUser.Aura.Level));
            Console.WriteLine(string.Concat(" Value: ", SuperStar.LoggedInUser.Aura.Value));
            Console.WriteLine(string.Concat(" Progress: ", SuperStar.LoggedInUser.Aura.Progress));
            Console.WriteLine(string.Concat(" ColourRed: ", SuperStar.LoggedInUser.Aura.ColourRed));
            Console.WriteLine(string.Concat(" ColourGreen: ", SuperStar.LoggedInUser.Aura.ColourGreen));
            Console.WriteLine(string.Concat(" ColourBlue: ", SuperStar.LoggedInUser.Aura.ColourBlue));

            Console.WriteLine("");
            Console.WriteLine(" Attributes:");
            Console.WriteLine(string.Concat(" Strength: ", SuperStar.LoggedInUser.Attributes.Strength));
            Console.WriteLine(string.Concat(" Speed: ", SuperStar.LoggedInUser.Attributes.Speed));
            Console.WriteLine(string.Concat(" Dexterity: ", SuperStar.LoggedInUser.Attributes.Dexterity));
            Console.WriteLine(string.Concat(" Intelligence: ", SuperStar.LoggedInUser.Attributes.Intelligence));
            Console.WriteLine(string.Concat(" Magic: ", SuperStar.LoggedInUser.Attributes.Magic));
            Console.WriteLine(string.Concat(" Wisdom: ", SuperStar.LoggedInUser.Attributes.Wisdom));
            Console.WriteLine(string.Concat(" Toughness: ", SuperStar.LoggedInUser.Attributes.Toughness));
            Console.WriteLine(string.Concat(" Vitality: ", SuperStar.LoggedInUser.Attributes.Vitality));
            Console.WriteLine(string.Concat(" Endurance: ", SuperStar.LoggedInUser.Attributes.Endurance));

            Console.WriteLine("");
            Console.WriteLine(" Stats:");
            Console.WriteLine(string.Concat(" HP: ", SuperStar.LoggedInUser.Stats.HP.Current, "/", SuperStar.LoggedInUser.Stats.HP.Max));
            Console.WriteLine(string.Concat(" Mana: ", SuperStar.LoggedInUser.Stats.Mana.Current, "/", SuperStar.LoggedInUser.Stats.Mana.Max));
            Console.WriteLine(string.Concat(" Energy: ", SuperStar.LoggedInUser.Stats.Energy.Current, "/", SuperStar.LoggedInUser.Stats.Energy.Max));
            Console.WriteLine(string.Concat(" Staminia: ", SuperStar.LoggedInUser.Stats.Staminia.Current, "/", SuperStar.LoggedInUser.Stats.Staminia.Max));

            Console.WriteLine("");
            Console.WriteLine(" Super Powers:");
            Console.WriteLine(string.Concat(" Flight: ", SuperStar.LoggedInUser.SuperPowers.Flight));
            Console.WriteLine(string.Concat(" Astral Projection: ", SuperStar.LoggedInUser.SuperPowers.AstralProjection));
            Console.WriteLine(string.Concat(" Bio-Locatation: ", SuperStar.LoggedInUser.SuperPowers.BioLocatation));
            Console.WriteLine(string.Concat(" Heat Vision: ", SuperStar.LoggedInUser.SuperPowers.HeatVision));
            Console.WriteLine(string.Concat(" Invulerability: ", SuperStar.LoggedInUser.SuperPowers.Invulerability));
            Console.WriteLine(string.Concat(" Remote Viewing: ", SuperStar.LoggedInUser.SuperPowers.RemoteViewing));
            Console.WriteLine(string.Concat(" Super Speed: ", SuperStar.LoggedInUser.SuperPowers.SuperSpeed));
            Console.WriteLine(string.Concat(" Super Strength: ", SuperStar.LoggedInUser.SuperPowers.SuperStrength));
            Console.WriteLine(string.Concat(" Telekineseis: ", SuperStar.LoggedInUser.SuperPowers.Telekineseis));
            Console.WriteLine(string.Concat(" XRay Vision: ", SuperStar.LoggedInUser.SuperPowers.XRayVision));

            Console.WriteLine("");
            Console.WriteLine(" Skills:");
            Console.WriteLine(string.Concat(" Computers: ", SuperStar.LoggedInUser.Skills.Computers));
            Console.WriteLine(string.Concat(" Engineering: ", SuperStar.LoggedInUser.Skills.Engineering));
            Console.WriteLine(string.Concat(" Farming: ", SuperStar.LoggedInUser.Skills.Farming));
            Console.WriteLine(string.Concat(" FireStarting: ", SuperStar.LoggedInUser.Skills.FireStarting));
            Console.WriteLine(string.Concat(" Fishing: ", SuperStar.LoggedInUser.Skills.Fishing));
            Console.WriteLine(string.Concat(" Languages: ", SuperStar.LoggedInUser.Skills.Languages));
            Console.WriteLine(string.Concat(" Meditation: ", SuperStar.LoggedInUser.Skills.Meditation));
            Console.WriteLine(string.Concat(" MelleeCombat: ", SuperStar.LoggedInUser.Skills.MelleeCombat));
            Console.WriteLine(string.Concat(" Mindfulness: ", SuperStar.LoggedInUser.Skills.Mindfulness));
            Console.WriteLine(string.Concat(" Negotiating: ", SuperStar.LoggedInUser.Skills.Negotiating));
            Console.WriteLine(string.Concat(" RangeCombat: ", SuperStar.LoggedInUser.Skills.RangeCombat));
            Console.WriteLine(string.Concat(" Research: ", SuperStar.LoggedInUser.Skills.Research));
            Console.WriteLine(string.Concat(" Science: ", SuperStar.LoggedInUser.Skills.Science));
            Console.WriteLine(string.Concat(" SpellCasting: ", SuperStar.LoggedInUser.Skills.SpellCasting));
            Console.WriteLine(string.Concat(" Translating: ", SuperStar.LoggedInUser.Skills.Translating));
            Console.WriteLine(string.Concat(" Yoga: ", SuperStar.LoggedInUser.Skills.Yoga));

            Console.WriteLine("");
            Console.WriteLine(" Gifts:");

            foreach (AvatarGift gift in SuperStar.LoggedInUser.Gifts)
                Console.WriteLine(string.Concat(" ", Enum.GetName(gift.GiftType), " earnt on ", gift.GiftEarnt.ToString()));

            Console.WriteLine("");
            Console.WriteLine(" Spells:");

            foreach (Spell spell in SuperStar.LoggedInUser.Spells)
                Console.WriteLine(string.Concat(" ", spell.Name));

            Console.WriteLine("");
            Console.WriteLine(" Inventory:");

            foreach (InventoryItem inventoryItem in SuperStar.LoggedInUser.Inventory)
                Console.WriteLine(string.Concat(" ", inventoryItem.Name));

            Console.WriteLine("");
            Console.WriteLine(" Achievements:");

            foreach (Achievement achievement in SuperStar.LoggedInUser.Achievements)
                Console.WriteLine(string.Concat(" ", achievement.Name));

            Console.WriteLine("");
            Console.WriteLine(" Gene Keys:");

            foreach (GeneKey geneKey in SuperStar.LoggedInUser.GeneKeys)
                Console.WriteLine(string.Concat(" ", geneKey.Name));

            Console.WriteLine("");
            Console.WriteLine(" Human Design:");
            Console.WriteLine(string.Concat(" Type: ", SuperStar.LoggedInUser.HumanDesign.Type));
        }
    }
}
