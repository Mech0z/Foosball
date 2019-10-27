using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Models.RequestResponses;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationsTests
{
    [TestFixture]
    public class LiveMatchControllerTests
    {
        private const string Basestring = "https://foosballapi-integrationtest.azurewebsites.net/api/LiveMatch";

        [Test]
        public async Task GetLiveMatch_WhenGetIsCalled_ShouldReturnCurrentLiveMatch()
        {
            // Cleanup
            var testHelper = new TestHelper();
            testHelper.DropAllCollections();

            // Arrange
            var players = testHelper.Create4Users(true);
            var user = testHelper.CreateTableUser();
            var userLoginInfo = await testHelper.GetUserLoginInfo(user.Email);
            
            var httpClient = new HttpClient();
            var loginToken = userLoginInfo.Tokens.First();

            var now = DateTime.Now;

            var request = new LiveMatchUpdateRequest
            {
                Team1Players = new List<string> { players[0].Email, players[1].Email},
                Team2Players = new List<string> { players[2].Email, players[3].Email},
                StartTime = now.AddMinutes(-2),
                LiveMatchUpdates = new List<LiveMatchUpdate>
                {
                    new LiveMatchUpdate{EventType = EventType.Goal, Team = 1, Timestamp = now.AddMinutes(-1)},
                    new LiveMatchUpdate{EventType = EventType.Goal, Team = 1, Timestamp = now},
                }
            };
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{Basestring}/UpdateActivityStatus"),
                Headers =
                {
                    { "Token", loginToken.Token },
                    { "Email", userLoginInfo.Email },
                    { "DeviceName", loginToken.DeviceName }
                },
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };
            await httpClient.SendAsync(httpRequestMessage);

            // Act
            var result = await httpClient.GetAsync(Basestring+ "/GetUpdateActivityStatus");

            //Assert
            var resultAsString = await result.Content.ReadAsStringAsync();
            var parsed = JsonConvert.DeserializeObject<LiveMatchUpdateRequest>(resultAsString);
            parsed.Team1Players.Count.Should().Be(2);
            parsed.Team2Players.Count.Should().Be(2);
            parsed.StartTime.Should().BeCloseTo(now.AddMinutes(-2));
            parsed.LiveMatchUpdates.Count.Should().Be(2);
        }
    }
}