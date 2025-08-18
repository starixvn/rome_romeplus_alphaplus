namespace AlphaPlus.SecurityCode.UnitTests.Web;

public class SecurityCodeSessionManagerTests
{
    [Fact]
    public void SecurityCodeSessionManager_StaticConstructor_ShouldInitializeSession()
    {
        // Arrange & Act
        var session = SecurityCodeSessionManager.SecurityCodeSession;

        // Assert
        Assert.NotNull(session);
        Assert.IsAssignableFrom<ISecurityCodeSession>(session);
    }

    [Fact]
    public void SecurityCodeSessionManager_SecurityCodeSession_ShouldBeSettable()
    {
        // Arrange
        var mockSession = new Mock<ISecurityCodeSession>();
        var originalSession = SecurityCodeSessionManager.SecurityCodeSession;

        try
        {
            // Act
            SecurityCodeSessionManager.SecurityCodeSession = mockSession.Object;

            // Assert
            Assert.Equal(mockSession.Object, SecurityCodeSessionManager.SecurityCodeSession);
        }
        finally
        {
            // Cleanup - restore original session
            SecurityCodeSessionManager.SecurityCodeSession = originalSession;
        }
    }

    [Fact]
    public void SecurityCodeSessionManager_SecurityCodeSession_ShouldBeReusable()
    {
        // Arrange
        var session1 = SecurityCodeSessionManager.SecurityCodeSession;
        var session2 = SecurityCodeSessionManager.SecurityCodeSession;

        // Assert
        Assert.Equal(session1, session2);
    }
}
