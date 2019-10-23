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
            await testHelper.CreateSeasonLeaderBoardAndAddMatch(httpClient);

            // Act
            var result = await httpClient.GetAsync(uri);

            // Assert
            Console.WriteLine(result.ReasonPhrase);
            var resultAsString = await result.Content.ReadAsStringAsync();
            var parsed = JsonConvert.DeserializeObject<List<LeaderboardView>>(resultAsString);
            result.Should().NotBeNull();
            parsed.Count.Should().Be(1);
            parsed.First().Entries.Count.Should().Be(4);
        }
    }
}
