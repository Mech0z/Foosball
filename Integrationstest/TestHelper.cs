using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Models.Old;
using Models.RequestResponses;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace IntegrationsTests
{
    public class TestHelper
    {
        //TODO Move to app settings file
        private const string Basestring = "https://foosballapi-integrationtest.azurewebsites.net/api/";

        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Season> _seasonsCollection;
        private readonly IMongoCollection<UserLoginInfo> _userLoginInfosCollection;
        private readonly IMongoDatabase _database;
        private const string emailDomain = "@foosballapi.com";

        public TestHelper()
        {
            //TODO Move to app settings file
            var connectionString =
                "mongodb://superuser:superuser1!@ds018839.mlab.com:18839/foosball-integrationtest?retryWrites=false";
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("foosball-integrationtest");

            _usersCollection = _database.GetCollection<User>("Users");
            _seasonsCollection = _database.GetCollection<Season>("Seasons");
            _userLoginInfosCollection = _database.GetCollection<UserLoginInfo>("UserLoginInfos");
        }

        public void DropAllCollections()
        {
            List<string> collectionNames = _database.ListCollectionNames().ToList();
            foreach (string collectionName in collectionNames)
            {
                if (collectionName.StartsWith("system"))
                {
                    continue;
                }
                _database.DropCollection(collectionName);
            }
        }

        public void CreateUser(string username)
        {
            var user = new User($"{username}{emailDomain}", username);
            _usersCollection.InsertOne(user);
        }

        public void Create4Users(bool withPlayerRole)
        {
            var players = new List<string> {"user1", "user2", "user3", "user4"};
            foreach (string player in players)
            {
                CreateUser(player);
                if (withPlayerRole)
                {
                    AddPlayerRole($"{player}{emailDomain}");
                }
            }
        }

        public void CreateSeason(DateTime startDate, string seasonName = "Season 1")
        {
            var season = new Season{Name = seasonName, StartDate = startDate};
            _seasonsCollection.InsertOne(season);
        }

        public void AddPlayerRole(string email)
        {
            var userLoginInfo = new UserLoginInfo{Email = email, Roles = new List<string>{"Player"}};
            _userLoginInfosCollection.InsertOne(userLoginInfo);
        }

        public async Task<List<User>> GetPlayers()
        {
            return await _usersCollection.AsQueryable().ToListAsync();
        }

        public async Task AddMatch(HttpClient httpClient, List<string> players)
        {
            var uri = Basestring + "/Match/SaveMatch";
            var request = new SaveMatchesRequest
            {
                Email = players.First(),
                Matches = new List<Match>
                {
                    new Match(DateTime.Now, players, new MatchResult{Team1Score = 8, Team2Score = 2}, 0, players.First())
                }
            };
            await httpClient.PostAsync(uri, GetHttpContent(request));
        }

        private StringContent GetHttpContent(object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            var content = new StringContent(str);
            return content;
        }
    }
}