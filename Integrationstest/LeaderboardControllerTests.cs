using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Models.Old;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Integrationstest
{
    [TestFixture]
    public class LeaderboardControllerTests
    {
        private const string Basestring = "http://localhost:5000/api/Leaderboard";

        [Test]
        public async Task Index_GetIndex_ShouldReturnLeaderboard()
        {
            // Arrange
            var httpClient = new HttpClient();

            // Act
            var result = await httpClient.GetAsync($"{Basestring}/Index");

            // Assert
            var resultAsString = await result.Content.ReadAsStringAsync();
            var parsed = JsonConvert.DeserializeObject<List<LeaderboardView>>(resultAsString);
            result.Should().NotBeNull();
            parsed.Count.Should().BeGreaterThan(0);
        }
    }
}
