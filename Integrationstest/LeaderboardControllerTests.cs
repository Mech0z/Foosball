using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Models.Old;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationsTests
{
    [TestFixture]
    public class LeaderboardControllerTests
    {
        //TODO Move to app settings file
        private const string Basestring = "https://foosballapi-integrationtest.azurewebsites.net/api/Leaderboard";

        [Test]
        public async Task Index_GetIndex_ShouldReturnLeaderboard()
        {
            // Cleanup
            var testHelper = new TestHelper();
            testHelper.DropAllCollections();

            // Arrange
            var httpClient = new HttpClient();
            var uri = $"{Basestring}/Index";

            var result1 = await httpClient.GetAsync(uri);
            testHelper.CreateSeason(DateTime.Today.AddDays(-1));
            testHelper.Create4Users(true);
            var players = await testHelper.GetPlayers();
            var userLoginInfo = await testHelper.GetUserLoginInfo(players.First().Email);
            await testHelper.AddMatch(httpClient, players.Select(x => x.Email).ToList(), userLoginInfo);


            // Act
            //TODO Why dont the first one return something
            var result2 = await httpClient.GetAsync(uri);

            // Assert
            var resultAsString = await result2.Content.ReadAsStringAsync();
            var parsed = JsonConvert.DeserializeObject<List<LeaderboardView>>(resultAsString);
            result2.Should().NotBeNull();
            parsed.Count.Should().BeGreaterThan(0);
        }
    }

    [TestFixture]
    public class LiveMatchControllerTests
    {
        private const string Basestring = "http://localhost:5000/api/LiveMatch";
    }
}
