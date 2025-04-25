using AlphaPlus.Reports.Features.BonusDetail;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.BonusDetail
{
    [Trait("Category", "BonusDetail")]
    public class BonusDetailServiceTest
    {
        private readonly Mock<IDynamicRepository> repositoryMock;
        private readonly BonusDetailService service;

        public BonusDetailServiceTest()
        {
            repositoryMock = new Mock<IDynamicRepository>();
            service = new BonusDetailService(repositoryMock.Object);
        }

        [Fact]
        public async Task GetBonusDetail_WithSuperLevel_ShouldReturnExpectedData()
        {
            // Arrange
            var param = new BonusDetailParam(
                ViewerId: 123,
                ViewerLevelId: RoleIdConstants.Super,
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                StatusId: 1,
                IsComplained: false,
                PageIndex: 1,
                PageSize: 10
            );

            var bonusEntities = new List<BonusEntity>
            {
                new() { Username = "Super1", MasterName = "Master1", AgentName = "Agent1", Amount = 100, StatusId = 0, CreatedDate = DateTime.Now },
                new() { Username = "Super1", MasterName = "Master1", AgentName = "Agent1", Amount = 50, StatusId = 2, CreatedDate = DateTime.Now }
            };

            repositoryMock.Setup(r => r.FetchAsync<BonusEntity>(It.IsAny<ICriteria>(), string.Empty))
                .Callback<ICriteria, string>((c, ip) =>
                {
                    if (c is GetSuperBonusCriteria criteria)
                    {
                        criteria.TotalRecords = 2;
                    }
                })
                .ReturnsAsync(bonusEntities);

            // Act
            var result = await service.GetBonusDetail(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRecords);
            Assert.Single(result.DataSource);
            Assert.Equal(150, result.DataSource.First().TotalAmount);
            Assert.Equal(150, result.Total.TotalAmount);

            repositoryMock.Verify(
                r => r.FetchAsync<BonusEntity>(It.Is<GetSuperBonusCriteria>(c =>
                    c.Username == param.Username &&
                    c.FromDate == param.FromDate &&
                    c.ToDate == param.ToDate &&
                    c.StatusId == param.StatusId &&
                    c.IsComplained == param.IsComplained &&
                    c.PageIndex == param.PageIndex &&
                    c.PageSize == param.PageSize &&
                    c.SuperId == param.ViewerId), string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task GetBonusDetail_WithMasterLevel_ShouldReturnExpectedData()
        {
            // Arrange
            var param = new BonusDetailParam(
                ViewerId: 123,
                ViewerLevelId: RoleIdConstants.Master,
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                StatusId: 1,
                IsComplained: false,
                PageIndex: 1,
                PageSize: 10
            );

            var bonusEntities = new List<BonusEntity>
            {
                new() { MasterName = "Master1", AgentName = "Agent1", Amount = 100, StatusId = 0, CreatedDate = DateTime.Now },
                new() { MasterName = "Master1", AgentName = "Agent1", Amount = 50, StatusId = 2, CreatedDate = DateTime.Now }
            };

            repositoryMock.Setup(r => r.FetchAsync<BonusEntity>(It.IsAny<ICriteria>(), string.Empty))
                .Callback<ICriteria, string>((c, ip) =>
                {
                    if (c is GetMasterBonusCriteria criteria)
                    {
                        criteria.TotalRecords = 2;
                    }
                })
                .ReturnsAsync(bonusEntities);

            // Act
            var result = await service.GetBonusDetail(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRecords);
            Assert.Single(result.DataSource);
            Assert.Equal(150, result.DataSource.First().TotalAmount);
            Assert.Equal(150, result.Total.TotalAmount);

            repositoryMock.Verify(
                r => r.FetchAsync<BonusEntity>(It.Is<GetMasterBonusCriteria>(c =>
                    c.Username == param.Username &&
                    c.FromDate == param.FromDate &&
                    c.ToDate == param.ToDate &&
                    c.StatusId == param.StatusId &&
                    c.IsComplained == param.IsComplained &&
                    c.PageIndex == param.PageIndex &&
                    c.PageSize == param.PageSize &&
                    c.MasterId == param.ViewerId), string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task GetBonusDetail_WithAgentLevel_ShouldReturnExpectedData()
        {
            // Arrange
            var param = new BonusDetailParam(
                ViewerId: 123,
                ViewerLevelId: RoleIdConstants.Agent,
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                StatusId: 1,
                IsComplained: false,
                PageIndex: 1,
                PageSize: 10
            );

            var bonusEntities = new List<BonusEntity>
            {
                new() { AgentName = "Agent1", Amount = 100, StatusId = 0, CreatedDate = DateTime.Now },
                new() { AgentName = "Agent1", Amount = 50, StatusId = 2, CreatedDate = DateTime.Now }
            };

            repositoryMock.Setup(r => r.FetchAsync<BonusEntity>(It.IsAny<ICriteria>(), string.Empty))
                .Callback<ICriteria, string>((c, ip) =>
                {
                    if (c is GetAgentBonusCriteria criteria)
                    {
                        criteria.TotalRecords = 2;
                    }
                })
                .ReturnsAsync(bonusEntities);

            // Act
            var result = await service.GetBonusDetail(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRecords);
            Assert.Single(result.DataSource);
            Assert.Equal(150, result.DataSource.First().TotalAmount);
            Assert.Equal(150, result.Total.TotalAmount);

            repositoryMock.Verify(
                r => r.FetchAsync<BonusEntity>(It.Is<GetAgentBonusCriteria>(c =>
                    c.Username == param.Username &&
                    c.FromDate == param.FromDate &&
                    c.ToDate == param.ToDate &&
                    c.StatusId == param.StatusId &&
                    c.IsComplained == param.IsComplained &&
                    c.PageIndex == param.PageIndex &&
                    c.PageSize == param.PageSize &&
                    c.AgentId == param.ViewerId), string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task GetBonusDetail_WithCancelledBonus_ShouldNotCountCancelledAmounts()
        {
            // Arrange
            var param = new BonusDetailParam(
                ViewerId: 123,
                ViewerLevelId: RoleIdConstants.Agent,
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                StatusId: 1,
                IsComplained: false,
                PageIndex: 1,
                PageSize: 10
            );

            var bonusEntities = new List<BonusEntity>
            {
                new() { AgentName = "Agent1", Amount = 100, StatusId = 0, CreatedDate = DateTime.Now },
                new() { AgentName = "Agent1", Amount = 50, StatusId = 1, CreatedDate = DateTime.Now }
            };

            repositoryMock.Setup(r => r.FetchAsync<BonusEntity>(It.IsAny<ICriteria>(), string.Empty))
                .Callback<ICriteria, string>((c, ip) =>
                {
                    if (c is GetAgentBonusCriteria criteria)
                    {
                        criteria.TotalRecords = 2;
                    }
                })
                .ReturnsAsync(bonusEntities);

            // Act
            var result = await service.GetBonusDetail(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRecords);
            Assert.Single(result.DataSource);
            Assert.Equal(100, result.DataSource.First().TotalAmount);
            Assert.Equal(100, result.Total.TotalAmount);
        }

        [Fact]
        public async Task GetBonusDetail_WithEmptyData_ShouldReturnEmptyList()
        {
            // Arrange
            var param = new BonusDetailParam(
                ViewerId: 123,
                ViewerLevelId: RoleIdConstants.Agent,
                Username: "testUser",
                FromDate: DateTime.Now.AddDays(-7),
                ToDate: DateTime.Now,
                StatusId: 1,
                IsComplained: false,
                PageIndex: 1,
                PageSize: 10
            );

            var bonusEntities = new List<BonusEntity>();

            repositoryMock.Setup(r => r.FetchAsync<BonusEntity>(It.IsAny<ICriteria>(), string.Empty))
                .Callback<ICriteria, string>((c, ip) =>
                {
                    if (c is GetAgentBonusCriteria criteria)
                    {
                        criteria.TotalRecords = 0;
                    }
                })
                .ReturnsAsync(bonusEntities);

            // Act
            var result = await service.GetBonusDetail(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalRecords);
            Assert.Empty(result.DataSource);
            Assert.Equal(0, result.Total.TotalAmount);
        }

        [Fact]
        public async Task ConfirmPayment_ShouldExecuteWithCorrectParameters()
        {
            // Arrange
            var param = new ConfirmPaymentParam(
                ViewerId: 321,
                SubAccId: 456,
                CustId: 123,
                TransId: 789,
                PaymentDate: DateTime.Now,
                LoginIP: "127.0.0.1",
                Language: "en-US"
            );

            repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty))
                .Callback<INonQueryCommand, string>((c, ip) =>
                {
                    if (c is ConfirmPaymentCriteria criteria)
                    {
                        criteria.ErrCode = 0;
                        criteria.ErrMsg = "Success";
                    }
                })
                .Returns(Task.FromResult(0));

            // Act
            var result = await service.ConfirmPayment(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.Equal("Success", result.Message);

            repositoryMock.Verify(
                r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task ConfirmPayment_WithError_ShouldReturnErrorResult()
        {
            // Arrange
            var param = new ConfirmPaymentParam(
                ViewerId: 321,
                SubAccId: 456,
                CustId: 123,
                TransId: 789,
                PaymentDate: DateTime.Now,
                LoginIP: "127.0.0.1",
                Language: "en-US"
            );

            repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty))
                .Callback<INonQueryCommand, string>((c, ip) =>
                {
                    if (c is ConfirmPaymentCriteria criteria)
                    {
                        criteria.ErrCode = 1;
                        criteria.ErrMsg = "Error occurred";
                    }
                })
                .Returns(Task.FromResult(0));

            // Act
            var result = await service.ConfirmPayment(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Code);
            Assert.Equal("Error occurred", result.Message);
        }
    }
}