using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Helpers;
using Avatar = NextGenSoftware.OASIS.API.Providers.RedisOASIS.Entities.Avatar;
using AvatarDetail = NextGenSoftware.OASIS.API.Providers.RedisOASIS.Entities.AvatarDetail;
using RedisOASISProvider = NextGenSoftware.OASIS.API.Providers.RedisOASIS;


// Purpose: Demonstrate that the RedisOASIS database provider can connect, save, and load data to a free online db instance of Redis.
// RedisURL: https://redis.com/
// Temp Requirements:  You'll need to have two enviornment variables, one called REDIS_CONNECTION_STRING and the other called REDIS. 
// TODO: Make this into just one env variable while keeping all tests and helper methods if possible.
// TODO: Fix SaveAvatarAsync_ValidAvatar_ReturnsSavedAvatar() and  LoadAvatarAsync_WithValidId_ReturnsAvatar()

namespace NextGenSoftware.OASIS.API.Providers.RedisOASISTestHarness
{
    [TestClass]
    public class RedisOASISTestHarness
    {

        private Guid _avatarID;
        private RedisOASISProvider.RedisOASIS _RedisOASIS;

        [TestInitialize]
        public void TestInit()
        {
            
            string connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
            Assert.IsNotNull(connectionString, "The connectionString for your Redis database is null, have you set your env REDIS_CONNECTION_STRING ariable? ");
        
            _RedisOASIS = new RedisOASISProvider.RedisOASIS(connectionString);
            Assert.IsNotNull(_RedisOASIS, "RedisOASIS instance should not be null.");

            _avatarID = Guid.NewGuid();
        }

        //  Begin test helper methods
        public async Task<RedisOASISProvider.RedisOASIS> CreateRedisProvider()
        {
            string connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
            Assert.IsNotNull(connectionString, "The connectionString for your Redis database is null, have you set your env REDIS_CONNECTION_STRING ariable? ");
            RedisOASISProvider.RedisOASIS aRedisOASISProviderInstance = new RedisOASISProvider.RedisOASIS(connectionString);
            Assert.IsNotNull(aRedisOASISProviderInstance, "RedisOASIS instance should not be null.");

            return aRedisOASISProviderInstance;
        }
       
        public async Task<IDatabase> DirectConnectToRedisDB()
        {
            string redisPassword = Environment.GetEnvironmentVariable("REDIS");
            string connectionString = "redis-15805.c90.us-east-1-3.ec2.cloud.redislabs.com:15805,password=" + redisPassword + ",abortConnect=false";
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
            IDatabase db = redis.GetDatabase();
            return db;
        }

        public async Task<bool> LocalSimpleSave(Guid id, string simpleValue)
        {
            var serializedString = Newtonsoft.Json.JsonConvert.SerializeObject(simpleValue);
            var redisDb = await this.DirectConnectToRedisDB();
            var saved = await redisDb.StringSetAsync(id.ToString(), serializedString);

            return saved;
        }
        // End test helper methods


        [TestMethod]
        [Priority(1)]
        public async Task TestTaskDirectConnectToRedisDB() 
        {
            var db = this.DirectConnectToRedisDB();
            Assert.IsNotNull(db, "The db variable is unexpectedly Null.");
        }

        [TestMethod]
        [Priority(1)]
        public async Task TestCreateRedisProvider()
        {
            var redis = this.CreateRedisProvider();
            Assert.IsNotNull(redis, "The RedisOASIS provider instane is unexpextedly Null.");
        }


        [TestMethod]
        [Priority(1)]
        public async Task TestRedisOASISProviderSimpleSave()
        {
            Guid id = new Guid();
            bool save = _RedisOASIS.SimpleSave(id, "This is a test of SimpleSave.");
            Assert.IsTrue(save);
        }

        [TestMethod]
        [Priority(1)]
        public async Task TestLocalSimpleSave()
        {
            Guid id = new Guid();
            bool save = await this.LocalSimpleSave(id, "This is a test of SimpleSave.");
            Assert.IsTrue(save);
        }


        [TestMethod]
        [Priority(1)]
        public async Task TestRedisOASISProviderSimpleLoad()
        {
            Guid avatarID = Guid.NewGuid();
            string originalValue = "This is a test of SimpleLoad.";

            bool save = _RedisOASIS.SimpleSave(avatarID, originalValue);
            Assert.IsTrue(save);

            string loadedValue = _RedisOASIS.SimpleLoad(avatarID);
            Assert.IsNotNull(loadedValue);

            string deserializedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(loadedValue);
            Assert.IsTrue(deserializedValue == originalValue);
        }



    // TODO: Fix SaveAvatarAsync_ValidAvatar_ReturnsSavedAvatar() and  LoadAvatarAsync_WithValidId_ReturnsAvatar()
    //     [TestMethod]
    //     [Priority(2)]
    //     public async Task SaveAvatarAsync_ValidAvatar_ReturnsSavedAvatar()
    //     {
    //         // Arrange
    //         Avatar testAvatar = new Avatar { Id = _avatarID.ToString(), Username = "TestAvatarUsername", Password = "TestAvatarPassword" };


    //         // Act
    //         //  Expecting to be able to pass an instance of Avatar to a method that accepts IAvatar
    //         OASISResult<IAvatar> result = await _RedisOASIS.SaveAvatarAsync((IAvatar)testAvatar);

    //         // Assert
    //         Assert.IsNotNull(result.Result, "Avatar should not be null.");
    //         Assert.AreEqual(testAvatar.Id, result.Result.Id, "Avatar ID should match the input ID.");

    //         // Cleanup
    //         // After testing, you may want to remove the test data from your database.
    //         // You'll need to implement this method on your own, or it may already exist in your project.
    //         // await this._RedisOASIS.DeleteAvatarAsync(testAvatar.Id);
    //     }

    //     [TestMethod]
    //     [Priority(2)]
    //     public async Task LoadAvatarAsync_WithValidId_ReturnsAvatar()
    //     {
    //         // Arrange
    //         Guid testAvatarId = _avatarID;

    //         // Act
    //         OASISResult<IAvatar> result = await this._RedisOASIS.LoadAvatarAsync(testAvatarId);

    //         // Assert
    //         Assert.IsNotNull(result.Result, "Avatar should not be null.");
    //         Assert.AreEqual(testAvatarId, result.Result.Id, "Avatar ID should match the input ID.");
    //     }
      }
}