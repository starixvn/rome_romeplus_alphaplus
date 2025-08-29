using System.Reflection;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Settings.Site.UnitTests;

public class TestFixture : IDisposable
{
    public TestFixture()
    {
        SetupAppSettings();
        SetupMicroSites();
        SetupAlphaContext();
    }

    // Global method to configure AutoFixture for all tests
    public static void ConfigureGlobalAutoFixture(Fixture fixture)
    {
        // Fix UserLevel generation - only allow valid values (1,2,3,4)
        fixture.Customize<byte>(c => c.FromFactory(() => 
        {
            var validLevelIds = new byte[] { 1, 2, 3, 4 }; // Member, Agent, Master, Super
            var random = new Random();
            return validLevelIds[random.Next(validLevelIds.Length)];
        }));

        // Fix DateTime to avoid overflow issues  
        fixture.Customize<DateTime>(c => c.FromFactory(() => 
        {
            var random = new Random();
            var minYear = 2000;
            var maxYear = 2030;
            return new DateTime(
                random.Next(minYear, maxYear),
                random.Next(1, 13),
                random.Next(1, 28),
                random.Next(0, 24),
                random.Next(0, 60),
                random.Next(0, 60)
            );
        }));

        // Fix IDateTime to provide safe DateTime values for ViewLogs
        var mockDateTime = new Mock<AlphaPlus.Settings.ViewLogs.API.Helpers.IDateTime>();
        mockDateTime.SetupGet(d => d.Now).Returns(new DateTime(2023, 6, 15, 10, 30, 0)); // Safe middle value
        fixture.Customize<AlphaPlus.Settings.ViewLogs.API.Helpers.IDateTime>(c => 
            c.FromFactory(() => mockDateTime.Object));

        // Create mock User with valid UserLevel properties to avoid UserLevel validation errors
        var mockUser = new Mock<AlphaPlus.Settings.Site.Features.Shared.Models.IUser>();
        mockUser.SetupGet(u => u.Id).Returns(123);
        mockUser.SetupGet(u => u.Name).Returns("testuser");
        mockUser.SetupGet(u => u.LevelId).Returns((byte)2); // Agent
        mockUser.SetupGet(u => u.SubAccId).Returns(456);
        mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
        mockUser.SetupGet(u => u.AGIP).Returns("127.0.0.1");
        mockUser.SetupGet(u => u.CanViewReports).Returns(true);
        mockUser.SetupGet(u => u.CanTransfer).Returns(true);
        mockUser.SetupGet(u => u.CanUpdateAccount).Returns(true);
        mockUser.SetupGet(u => u.Site).Returns("testsite");
        mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
        mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
        
        // Create real PageAccess since it can't be mocked (non-virtual properties)
        var pageAccess = new AlphaPlus.Core.Models.PageAccess();
        mockUser.SetupGet(u => u.SubPageAccess).Returns(pageAccess);

        fixture.Customize<AlphaPlus.Settings.Site.Features.Shared.Models.IUser>(c => 
            c.FromFactory(() => mockUser.Object));

        // Create custom factories for Transfer Request objects to avoid UserLevel constructor issues
        fixture.Customize<AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.HintTransferQuery>(c =>
            c.FromFactory(() => {
                var obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.HintTransferQuery));
                var query = (AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.HintTransferQuery)obj;
                
                // Set required properties for IsValid() to work
                SetTransferQueryProperties(query);
                return query;
            }));

        fixture.Customize<AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.CashBalanceTransferQuery>(c =>
            c.FromFactory(() => {
                var obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.CashBalanceTransferQuery));
                var query = (AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.CashBalanceTransferQuery)obj;
                
                SetTransferQueryProperties(query);
                return query;
            }));

        fixture.Customize<AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.CashBalanceTransferCommand>(c =>
            c.FromFactory(() => {
                var obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.CashBalanceTransferCommand));
                var command = (AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer.CashBalanceTransferCommand)obj;
                
                SetTransferQueryProperties(command);
                return command;
            }));

        // Custom factory for ProblemAccountReportViewModel to provide valid IndexRequest 
        fixture.Customize<AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount.ProblemAccountReportViewModel>(c =>
            c.FromFactory(() => {
                var mockUser = new Mock<AlphaPlus.Settings.MemberInfo.Domain.Models.IUser>();
                mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
                mockUser.SetupGet(u => u.Id).Returns(456);
                mockUser.SetupGet(u => u.Name).Returns("testuser");
                mockUser.SetupGet(u => u.Site).Returns("testsite");
                mockUser.SetupGet(u => u.LevelId).Returns((byte)2);
                mockUser.SetupGet(u => u.SubAccId).Returns(789);
                mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
                mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
                
                var request = new AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount.IndexRequest(mockUser.Object);
                var models = new List<AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount.ProblemAccountModel>();
                return new AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount.ProblemAccountReportViewModel(request, models, false);
            }));

        // Custom factory for CreditBalanceViewModel to provide valid CreditBalanceQuery
        fixture.Customize<AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.CreditBalance.CreditBalanceViewModel>(c =>
            c.FromFactory(() => {
                var query = new AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance.CreditBalanceQuery();
                SetMemberInfoQueryProperties(query);
                var enteredSportResult = new AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss.EnteredSportResult();
                return new AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.CreditBalance.CreditBalanceViewModel(query, enteredSportResult, false);
            }));

        // Custom factory for CreditBalanceQuery to provide valid UserLevel fields
        fixture.Customize<AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance.CreditBalanceQuery>(c =>
            c.FromFactory(() => {
                var query = new AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance.CreditBalanceQuery();
                SetMemberInfoQueryProperties(query);
                return query;
            }));
    }

    // Helper method to set required properties for MemberInfo objects
    private static void SetMemberInfoQueryProperties(object memberInfoQuery)
    {
        var type = memberInfoQuery.GetType();
        
        // Set CustId field
        var custIdField = type.GetField("custId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (custIdField != null)
        {
            custIdField.SetValue(memberInfoQuery, 123);
        }
        
        // Set ActorLevel field
        var actorLevelField = type.GetField("actorLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (actorLevelField != null)
        {
            actorLevelField.SetValue(memberInfoQuery, (byte)2); // Agent level
        }
        
        // Set CustLevel field  
        var custLevelField = type.GetField("custLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (custLevelField != null)
        {
            custLevelField.SetValue(memberInfoQuery, (byte)3); // Master level
        }
    }

    // Helper method to set required properties for Transfer objects
    private static void SetTransferQueryProperties(object transferQuery)
    {
        var type = transferQuery.GetType();
        
        // Set CustId property
        var custIdProp = type.GetProperty("CustId");
        if (custIdProp != null && custIdProp.CanWrite)
        {
            custIdProp.SetValue(transferQuery, 123);
        }
        
        // Set ActorLevel and CustLevel using reflection
        var actorLevelField = type.GetField("ActorLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var actorLevelProp = type.GetProperty("ActorLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        var custLevelField = type.GetField("CustLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var custLevelProp = type.GetProperty("CustLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Get UserLevel.Agent (value 2)
        var userLevelType = typeof(AlphaPlus.Core.Models.UserLevel);
        var agentLevel = userLevelType.GetProperty("Agent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetValue(null);
        
        // Set ActorLevel
        if (agentLevel != null)
        {
            if (actorLevelProp != null && actorLevelProp.CanWrite)
                actorLevelProp.SetValue(transferQuery, agentLevel);
            else if (actorLevelField != null)
                actorLevelField.SetValue(transferQuery, agentLevel);
        }
        
        // Set CustLevel
        if (agentLevel != null)
        {
            if (custLevelProp != null && custLevelProp.CanWrite)
                custLevelProp.SetValue(transferQuery, agentLevel);
            else if (custLevelField != null)
                custLevelField.SetValue(transferQuery, agentLevel);
        }
    }

    private static void SetupAppSettings()
    {
        // Create a mock configuration
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

        // Setup AppSettings.IsSiteLabel789y to return false by default
        var appSettingsType = typeof(AppSettings);
        var isSiteLabel789yProperty = appSettingsType.GetProperty("IsSiteLabel789y", BindingFlags.Public | BindingFlags.Static);

        if (isSiteLabel789yProperty != null)
        {
            // Use reflection to set the property
            var backingField = appSettingsType.GetField("_isSiteLabel789y", BindingFlags.NonPublic | BindingFlags.Static);
            if (backingField != null)
            {
                backingField.SetValue(null, false);
            }
        }
    }

    private static void SetupMicroSites()
    {
        // Setup MicroSites methods if needed
        // This is a simplified setup - in a real scenario you might need more complex mocking
    }

    private static void SetupAlphaContext()
    {
        // Create a mock HttpContextAccessor
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new Mock<HttpContext>();
        
        // Create a mock HttpRequest
        var mockRequest = new Mock<HttpRequest>();
        var mockCookieCollection = new Mock<IRequestCookieCollection>();
        mockRequest.Setup(r => r.Cookies).Returns(mockCookieCollection.Object);
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
        
        // Create a mock Session
        var mockSession = new Mock<ISession>();
        byte[] outValue;
        mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out outValue)).Returns(false);
        mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
        
        // Setup the HttpContext to return the mock context
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);
        
        // Configure AlphaContext with the mock HttpContextAccessor
        AlphaContext.Configure(mockHttpContextAccessor.Object);
    }

    public TabViewModel CreateTabViewModel(string tabName = "index")
    {
        // Create TabViewModel with mocked dependencies
        return new TabViewModel(tabName);
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}
