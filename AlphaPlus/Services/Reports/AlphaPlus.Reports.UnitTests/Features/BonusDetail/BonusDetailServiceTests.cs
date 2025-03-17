//using AlphaPlus.Reports.Features.BonusDetail;
//using Fanex.Data.Repository;
//using NSubstitute;

//namespace AlphaPlus.Reports.UnitTests.Features.BonusDetail;

//[Trait("Category", "BonusDetail")]
//public class BonusDetailServiceTests
//{
//    private readonly IDynamicRepository repository;
//    private readonly BonusDetailService service;

//    public BonusDetailServiceTests()
//    {
//        repository = Substitute.For<IDynamicRepository>();
//        service = new BonusDetailService(repository);
//    }

//    [Fact]
//    public async Task GetBonusDetail_ShouldReturnCorrectData_ForSuperLevel()
//    {
//        // Arrange
//        var param = new BonusDetailParam
//        (
//            Username: "testuser",
//            FromDate: new DateTime(2025, 1, 1),
//            ToDate: new DateTime(2025, 1, 31),
//            StatusId: 1,
//            IsComplained: true,
//            PageIndex: 0,
//            PageSize: 10,
//            ViewerLevelId: 4,
//            ViewerId: 123
//        );
//        var criteria = new GetSuperBonusCriteria(
//            param.Username,
//            param.FromDate,
//            param.ToDate,
//            param.StatusId,
//            param.IsComplained,
//            param.PageIndex,
//            param.PageSize,
//            param.ViewerId);
//        var rawData = new List<BonusEntity>
//        {
//            new BonusEntity
//            {
//                CreatedDate = new DateTime(2025, 1, 1),
//                TransId = 123456789L,
//                MasterName = "Master1",
//                AgentName = "Agent1",
//                CustId = 1,
//                Username = "user1",
//                Amount = 100.50m,
//                PaymentDate = new DateTime(2025, 1, 2),
//                StatusId = 1,
//                IsComplained = true
//            }
//        };

//        repository.FetchAsync<BonusEntity>(criteria).Returns(rawData);

//        // Act
//        var result = await service.GetBonusDetail(param);

//        // Assert
//        Assert.Equal(1, result.TotalRecords);
//        Assert.Single(result.DataSource);
//        Assert.Equal("01/01/2025", result.DataSource.First().GroupBy);
//        Assert.Equal(100.50m, result.DataSource.First().TotalAmount);
//    }

//    [Fact]
//    public async Task ConfirmPayment_ShouldReturnCorrectResult()
//    {
//        // Arrange
//        var param = new ConfirmPaymentParam
//        (
//            CustId: 1,
//            SubAccId: 2,
//            TransId: 123456789L,
//            PaymentDate: new DateTime(2025, 3, 17),
//            ViewerId: 3,
//            LoginIP: "127.0.0.1",
//            Language: "en"
//        );
//        var criteria = new ConfirmPaymentCriteria(
//            param.CustId,
//            param.SubAccId,
//            param.TransId,
//            param.PaymentDate,
//            param.ViewerId,
//            param.LoginIP,
//            param.Language);

//        repository.ExecuteAsync(criteria).Returns(Task.CompletedTask);

//        // Act
//        var result = await service.ConfirmPayment(param);

//        // Assert
//        Assert.Equal(criteria.ErrCode, result.Code);
//        Assert.Equal(criteria.ErrMsg, result.Message);
//    }
//}
