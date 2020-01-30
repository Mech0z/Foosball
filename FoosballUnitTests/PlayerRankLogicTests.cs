using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Foosball.Logic;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using NUnit.Framework;
using Repository;

namespace FoosballUnitTests
{
    public class PlayerRankLogicTests
    {
        [Test]
        public void FillOutPlayerRankBlanks_WhenCalled_ShouldGiveEveryPlayerPlotsForSameDays()
        {
            // Arrange
            var optionsWrapper = GetOptionsWrapper();
            var cut = new PlayerRankLogic(
                new PlayerRankHistoryRepository(
                    optionsWrapper));
            var list = new List<PlayerRankSeasonEntry>();
            var entry1 = CreateEntry1day("player1", 1, 1);
            var entry2 = CreateEntry1day("player2", 1, 2);
            list.Add(entry1);
            list.Add(entry2);

            // Act
            var result = cut.FillOutPlayerRankBlanks(list);

            // Assert
            var player1 = result.SingleOrDefault(x => x.Email == entry1.Email);
            player1.RankPlots.Count.Should().Be(2);
            var player2 = result.SingleOrDefault(x => x.Email == entry2.Email);
            player2.RankPlots.Count.Should().Be(2);
        }

        private static OptionsWrapper<ConnectionStringsSettings> GetOptionsWrapper()
        {
            return new OptionsWrapper<ConnectionStringsSettings>(new ConnectionStringsSettings{DefaultConnectionMongoDB = "mongodb+srv://superuser:superuser@cluster0-sxvkc.mongodb.net/test?retryWrites=true&w=majority", MongoDBDatabaseName = "sdf"});
        }

        [Test]
        public void GetLastEntryOfEachDay_WhenCalled_ShouldOnlyReturnLastPlotOfEachDay()
        {
            // Arrange
            var cut = new PlayerRankLogic(
                new PlayerRankHistoryRepository(GetOptionsWrapper()));
            var list = new List<PlayerRankSeasonEntry>();
            var entry1 = CreateEntries("player1", 1, (1, 1520), (1, 1540), (1, 1560), (2, 1580), (2, 1560), (3, 1540),
                (3, 1520));
            var entry2 = CreateEntries("player2", 1, (1, 1480), (2, 1460), (2, 1440), (4, 1420), (4, 1400), (5, 1380),
                (7, 1400));
            list.Add(entry1);
            list.Add(entry2);

            // Act
            var result = cut.GetLastEntryOfEachDay(list);

            // Assert
            var player1 = result.SingleOrDefault(x => x.Email == entry1.Email);
            player1.RankPlots.Count.Should().Be(3);
            player1.RankPlots.Single(x => x.Date.Day == 1).EloRating.Should().Be(1560);
            player1.RankPlots.Single(x => x.Date.Day == 2).EloRating.Should().Be(1560);
            player1.RankPlots.Single(x => x.Date.Day == 3).EloRating.Should().Be(1520);
            var player2 = result.SingleOrDefault(x => x.Email == entry2.Email);
            player2.RankPlots.Count.Should().Be(5);
            player2.RankPlots.Single(x => x.Date.Day == 1).EloRating.Should().Be(1480);
            player2.RankPlots.Single(x => x.Date.Day == 2).EloRating.Should().Be(1440);
            player2.RankPlots.Single(x => x.Date.Day == 4).EloRating.Should().Be(1400);
            player2.RankPlots.Single(x => x.Date.Day == 5).EloRating.Should().Be(1380);
            player2.RankPlots.Single(x => x.Date.Day == 7).EloRating.Should().Be(1400);
        }

        private PlayerRankSeasonEntry CreateEntry1day(string email, int seasonNumber, int date1)
        {
            var entry = new PlayerRankSeasonEntry(email, $"Season {seasonNumber}");
            entry.RankPlots.Add(new PlayerRankPlot(new DateTime(2000, 1, date1).AddHours(1), 1, 1520));
            return entry;
        }

        private PlayerRankSeasonEntry CreateEntries(string email, int seasonNumber, params (int, int)[] dates)
        {
            var entry = new PlayerRankSeasonEntry(email, $"Season {seasonNumber}");
            var count = 0;
            foreach ((int, int) date in dates)
            {
                count++;
                entry.RankPlots.Add(
                    new PlayerRankPlot(new DateTime(2000, 1, date.Item1).AddHours(count), 1, date.Item2));
            }
            
            return entry;
        }

        private PlayerRankSeasonEntry CreateEntry(string email, int seasonNumber, int date1, int date2, int date3)
        {
            var entry = new PlayerRankSeasonEntry(email, $"Season {seasonNumber}");
            entry.RankPlots.Add(new PlayerRankPlot(new DateTime(2000, 1, date1).AddHours(1), 1, 1520));
            entry.RankPlots.Add(new PlayerRankPlot(new DateTime(2000, 1, date2).AddHours(2), 1, 1540));
            entry.RankPlots.Add(new PlayerRankPlot(new DateTime(2000, 1, date3).AddHours(3), 1, 1560));
            return entry;
        }
    }
}