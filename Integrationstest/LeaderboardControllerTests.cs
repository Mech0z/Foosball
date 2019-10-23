using System.Collections.Generic;
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
        private const string Basestring = "https://foosballapi-integrationtest.azurewebsites.net/api/Leaderboard";

        [Test]
        public async Task Index_GetIndex_ShouldReturnLeaderboard()
        {
            // Arrange
            var httpClient = new HttpClient();

            // Act
            var uri = $"{Basestring}/Index";
            var result = await httpClient.GetAsync(uri);

            // Assert
            var resultAsString = await result.Content.ReadAsStringAsync();
            var parsed = JsonConvert.DeserializeObject<List<LeaderboardView>>(resultAsString);
            result.Should().NotBeNull();
            parsed.Count.Should().BeGreaterThan(0);
        }
    }
}
