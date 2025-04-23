using AlphaPlus.Reports.Features.BetChoiceMatching;

namespace AlphaPlus.Reports.UnitTests.Features.BetChoiceMatching
{
    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingTicketModelTest
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var leagueId = 1;
            var leagueGroupId = 2;
            var globalShowTime = DateTime.Now;
            var sportType = 3;
            var matchId = 123456789;
            var custId = 4;
            var username = "user1";
            var betType = 5;
            var betTeam = "TeamA";
            var hdp1 = 1.5m;
            var hdp2 = 2.5m;
            var ip = "127.0.0.1";
            var odds = 1.8m;
            var displayOdds = "1.8";
            byte oddsType = 1;
            var displayOddsType = "Decimal";
            var transDate = DateTime.Now;
            var transId = 987654321;
            var stake = 100m;
            var memberWinLoss = 50m;
            var superWinLoss = 25m;
            var company = 10m;
            var homeId = 10;
            var awayId = 20;
            var isLive = true;
            var liveHomeScore = 1;
            var liveAwayScore = 2;
            var isRunning = true;

            // Act
            var ticket = new BetChoiceMatchingTicketModel
            {
                LeagueId = leagueId,
                LeagueGroupId = leagueGroupId,
                GlobalShowTime = globalShowTime,
                SportType = sportType,
                MatchId = matchId,
                CustId = custId,
                Username = username,
                BetType = betType,
                BetTeam = betTeam,
                Hdp1 = hdp1,
                Hdp2 = hdp2,
                IP = ip,
                Odds = odds,
                DisplayOdds = displayOdds,
                OddsType = oddsType,
                DisplayOddsType = displayOddsType,
                TransDate = transDate,
                TransId = transId,
                Stake = stake,
                MemberWinLoss = memberWinLoss,
                SuperWinLoss = superWinLoss,
                Company = company,
                HomeId = homeId,
                AwayId = awayId,
                IsLive = isLive,
                LiveHomeScore = liveHomeScore,
                LiveAwayScore = liveAwayScore,
                IsRunning = isRunning
            };

            // Assert
            Assert.Equal(leagueId, ticket.LeagueId);
            Assert.Equal(leagueGroupId, ticket.LeagueGroupId);
            Assert.Equal(globalShowTime, ticket.GlobalShowTime);
            Assert.Equal(sportType, ticket.SportType);
            Assert.Equal(matchId, ticket.MatchId);
            Assert.Equal(custId, ticket.CustId);
            Assert.Equal(username, ticket.Username);
            Assert.Equal(betType, ticket.BetType);
            Assert.Equal(betTeam, ticket.BetTeam);
            Assert.Equal(hdp1, ticket.Hdp1);
            Assert.Equal(hdp2, ticket.Hdp2);
            Assert.Equal(ip, ticket.IP);
            Assert.Equal(odds, ticket.Odds);
            Assert.Equal(displayOdds, ticket.DisplayOdds);
            Assert.Equal(oddsType, ticket.OddsType);
            Assert.Equal(displayOddsType, ticket.DisplayOddsType);
            Assert.Equal(transDate, ticket.TransDate);
            Assert.Equal(transId, ticket.TransId);
            Assert.Equal(stake, ticket.Stake);
            Assert.Equal(memberWinLoss, ticket.MemberWinLoss);
            Assert.Equal(superWinLoss, ticket.SuperWinLoss);
            Assert.Equal(company, ticket.Company);
            Assert.Equal(homeId, ticket.HomeId);
            Assert.Equal(awayId, ticket.AwayId);
            Assert.Equal(isLive, ticket.IsLive);
            Assert.Equal(liveHomeScore, ticket.LiveHomeScore);
            Assert.Equal(liveAwayScore, ticket.LiveAwayScore);
            Assert.Equal(isRunning, ticket.IsRunning);
            Assert.Equal($"{matchId}_{betType}_{hdp1}_{hdp2}_{betTeam}_{isLive}_{liveHomeScore}_{liveAwayScore}", ticket.EventKey);
        }
    }
}
