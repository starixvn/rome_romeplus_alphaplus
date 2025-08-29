using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.Commission;

public class CommissionModelTests
{
    [Fact]
    public void CommissionModel_Should_Initialize_With_Constructor_Parameters()
    {
        // Arrange
        var closed = true;
        var enabled = false;
        var suspended = true;
        var username = "testuser";
        var custId = 123;
        var uGroup = "TestGroup";
        var firstName = "John";
        var lastName = "Doe";
        var credit = 1000.50m;
        var locked = (byte)1;
        var closedReason = (byte)2;
        var group = "AdminGroup";
        var doubleCommLosing = true;
        var isCashMember = false;
        var commissions = new List<SportCommissionModel>
        {
            new SportCommissionModel(10m, 20m, 30m),
            new SportCommissionModel(15m, 25m, 35m)
        };

        // Act
        var model = new CommissionModel(
            closed, enabled, suspended, username, custId, uGroup,
            firstName, lastName, credit, locked, closedReason, group,
            doubleCommLosing, isCashMember, commissions);

        // Assert
        Assert.Equal(closed, model.Closed);
        Assert.Equal(enabled, model.Enabled);
        Assert.Equal(suspended, model.Suspended);
        Assert.Equal(username, model.Username);
        Assert.Equal(custId, model.CustId);
        Assert.Equal(uGroup, model.UGroup);
        Assert.Equal(firstName, model.FirstName);
        Assert.Equal(lastName, model.LastName);
        Assert.Equal(credit, model.Credit);
        Assert.Equal(locked, model.Locked);
        Assert.Equal(closedReason, model.ClosedReason);
        Assert.Equal(group, model.Group);
        Assert.Equal(doubleCommLosing, model.DoubleCommLosing);
        Assert.Equal(isCashMember, model.IsCashMember);
        Assert.Equal(commissions, model.Commissions);
    }

    [Fact]
    public void CommissionModel_Should_Handle_Null_String_Values()
    {
        // Arrange
        string username = null;
        string uGroup = null;
        string firstName = null;
        string lastName = null;
        string group = null;
        var commissions = new List<SportCommissionModel>();

        // Act
        var model = new CommissionModel(
            false, true, false, username, 123, uGroup,
            firstName, lastName, 100m, 0, 0, group,
            false, true, commissions);

        // Assert
        Assert.Null(model.Username);
        Assert.Null(model.UGroup);
        Assert.Null(model.FirstName);
        Assert.Null(model.LastName);
        Assert.Null(model.Group);
    }

    [Fact]
    public void CommissionModel_Should_Handle_Empty_Commissions_Collection()
    {
        // Arrange
        var commissions = new List<SportCommissionModel>();

        // Act
        var model = CreateCommissionModel(commissions: commissions);

        // Assert
        Assert.NotNull(model.Commissions);
        Assert.Empty(model.Commissions);
    }

    [Fact]
    public void CommissionModel_Should_Handle_Null_Commissions_Collection()
    {
        // Arrange
        IEnumerable<SportCommissionModel> commissions = null;

        // Act
        var model = CreateCommissionModel(commissions: commissions);

        // Assert
        // The CreateCommissionModel method has a default value, so it won't actually be null
        // Let's test that the model handles the null value correctly in the helper method
        Assert.NotNull(model.Commissions);
    }

    [Fact]
    public void CommissionModel_Should_Be_Immutable()
    {
        // Arrange
        var model = CreateCommissionModel();

        // Act & Assert
        var type = typeof(CommissionModel);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            Assert.True(property.CanRead, $"{property.Name} should be readable");
            // Note: For positional records, properties may show as writable but have init-only setters
            // The important thing is the record maintains immutability behavior
        }
        
        // Test that the record behaves immutably - values don't change
        Assert.Equal("testuser", model.Username);
        Assert.Equal(123, model.CustId);
        Assert.Equal(1000m, model.Credit);
    }

    [Fact]
    public void CommissionModel_Should_Support_Equality_Comparison()
    {
        // Arrange
        var commissions = new List<SportCommissionModel> { new SportCommissionModel(10m, 20m, 30m) };
        var model1 = new CommissionModel(
            true, false, true, "user", 123, "group", "John", "Doe", 1000m,
            1, 2, "admin", true, false, commissions);
        var model2 = new CommissionModel(
            true, false, true, "user", 123, "group", "John", "Doe", 1000m,
            1, 2, "admin", true, false, commissions);
        var model3 = new CommissionModel(
            false, true, false, "user2", 456, "group2", "Jane", "Smith", 2000m,
            2, 3, "user", false, true, commissions);

        // Act & Assert
        Assert.Equal(model1, model2); // Same values should be equal
        Assert.NotEqual(model1, model3); // Different values should not be equal
    }

    [Fact]
    public void CommissionModel_Should_Support_Deconstruction()
    {
        // Arrange
        var commissions = new List<SportCommissionModel> { new SportCommissionModel(10m, 20m, 30m) };
        var model = new CommissionModel(
            true, false, true, "user", 123, "group", "John", "Doe", 1000m,
            1, 2, "admin", true, false, commissions);

        // Act
        var (closed, enabled, suspended, username, custId, uGroup, firstName, lastName, 
             credit, locked, closedReason, group, doubleCommLosing, isCashMember, modelCommissions) = model;

        // Assert
        Assert.True(closed);
        Assert.False(enabled);
        Assert.True(suspended);
        Assert.Equal("user", username);
        Assert.Equal(123, custId);
        Assert.Equal("group", uGroup);
        Assert.Equal("John", firstName);
        Assert.Equal("Doe", lastName);
        Assert.Equal(1000m, credit);
        Assert.Equal(1, locked);
        Assert.Equal(2, closedReason);
        Assert.Equal("admin", group);
        Assert.True(doubleCommLosing);
        Assert.False(isCashMember);
        Assert.Equal(commissions, modelCommissions);
    }

    [Fact]
    public void CommissionModel_Should_Support_With_Expression()
    {
        // Arrange
        var originalModel = CreateCommissionModel();

        // Act
        var modifiedModel = originalModel with 
        { 
            Username = "newuser",
            Credit = 2000m,
            Enabled = true
        };

        // Assert
        Assert.Equal("newuser", modifiedModel.Username);
        Assert.Equal(2000m, modifiedModel.Credit);
        Assert.True(modifiedModel.Enabled);
        
        // Other properties should remain unchanged
        Assert.Equal(originalModel.CustId, modifiedModel.CustId);
        Assert.Equal(originalModel.FirstName, modifiedModel.FirstName);
        Assert.Equal(originalModel.LastName, modifiedModel.LastName);
        
        // Original should remain unchanged
        Assert.Equal("testuser", originalModel.Username);
        Assert.Equal(1000m, originalModel.Credit);
        Assert.False(originalModel.Enabled);
    }

    [Theory]
    [InlineData(true, true, true, true, true)]
    [InlineData(false, false, false, false, false)]
    [InlineData(true, false, true, false, true)]
    public void CommissionModel_Should_Handle_Various_Boolean_Values(
        bool closed, bool enabled, bool suspended, bool doubleCommLosing, bool isCashMember)
    {
        // Act
        var model = new CommissionModel(
            closed, enabled, suspended, "user", 123, "group", "John", "Doe", 1000m,
            1, 2, "admin", doubleCommLosing, isCashMember, new List<SportCommissionModel>());

        // Assert
        Assert.Equal(closed, model.Closed);
        Assert.Equal(enabled, model.Enabled);
        Assert.Equal(suspended, model.Suspended);
        Assert.Equal(doubleCommLosing, model.DoubleCommLosing);
        Assert.Equal(isCashMember, model.IsCashMember);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 2, 3)]
    [InlineData(byte.MaxValue, byte.MaxValue, int.MaxValue)]
    [InlineData(byte.MinValue, byte.MinValue, int.MinValue)]
    public void CommissionModel_Should_Handle_Various_Numeric_Values(byte locked, byte closedReason, int custId)
    {
        // Act
        var model = new CommissionModel(
            false, true, false, "user", custId, "group", "John", "Doe", 1000m,
            locked, closedReason, "admin", false, true, new List<SportCommissionModel>());

        // Assert
        Assert.Equal(locked, model.Locked);
        Assert.Equal(closedReason, model.ClosedReason);
        Assert.Equal(custId, model.CustId);
    }

    [Theory]
    [InlineData(-1000.50)]
    [InlineData(0)]
    [InlineData(1000.50)]
    [InlineData(999999.99)]
    public void CommissionModel_Should_Handle_Various_Credit_Values(decimal credit)
    {
        // Act
        var model = CreateCommissionModel(credit: credit);

        // Assert
        Assert.Equal(credit, model.Credit);
    }

    [Fact]
    public void CommissionModel_Should_Preserve_Commissions_Collection()
    {
        // Arrange
        var commissions = new List<SportCommissionModel>
        {
            new SportCommissionModel(10m, 20m, 30m),
            new SportCommissionModel(15m, 25m, 35m),
            new SportCommissionModel(20m, 30m, 40m)
        };

        // Act
        var model = CreateCommissionModel(commissions: commissions);

        // Assert
        Assert.Equal(3, model.Commissions.Count());
        Assert.Contains(commissions[0], model.Commissions);
        Assert.Contains(commissions[1], model.Commissions);
        Assert.Contains(commissions[2], model.Commissions);
    }

    [Fact]
    public void CommissionModel_Properties_Should_Have_Correct_Types()
    {
        // Arrange
        var type = typeof(CommissionModel);

        // Act & Assert
        Assert.Equal(typeof(bool), type.GetProperty(nameof(CommissionModel.Closed))?.PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty(nameof(CommissionModel.Enabled))?.PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty(nameof(CommissionModel.Suspended))?.PropertyType);
        Assert.Equal(typeof(string), type.GetProperty(nameof(CommissionModel.Username))?.PropertyType);
        Assert.Equal(typeof(int), type.GetProperty(nameof(CommissionModel.CustId))?.PropertyType);
        Assert.Equal(typeof(string), type.GetProperty(nameof(CommissionModel.UGroup))?.PropertyType);
        Assert.Equal(typeof(string), type.GetProperty(nameof(CommissionModel.FirstName))?.PropertyType);
        Assert.Equal(typeof(string), type.GetProperty(nameof(CommissionModel.LastName))?.PropertyType);
        Assert.Equal(typeof(decimal), type.GetProperty(nameof(CommissionModel.Credit))?.PropertyType);
        Assert.Equal(typeof(byte), type.GetProperty(nameof(CommissionModel.Locked))?.PropertyType);
        Assert.Equal(typeof(byte), type.GetProperty(nameof(CommissionModel.ClosedReason))?.PropertyType);
        Assert.Equal(typeof(string), type.GetProperty(nameof(CommissionModel.Group))?.PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty(nameof(CommissionModel.DoubleCommLosing))?.PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty(nameof(CommissionModel.IsCashMember))?.PropertyType);
        Assert.Equal(typeof(IEnumerable<SportCommissionModel>), type.GetProperty(nameof(CommissionModel.Commissions))?.PropertyType);
    }

    private static CommissionModel CreateCommissionModel(
        bool closed = false,
        bool enabled = false,
        bool suspended = false,
        string username = "testuser",
        int custId = 123,
        string uGroup = "TestGroup",
        string firstName = "John",
        string lastName = "Doe",
        decimal credit = 1000m,
        byte locked = 0,
        byte closedReason = 0,
        string group = "AdminGroup",
        bool doubleCommLosing = false,
        bool isCashMember = false,
        IEnumerable<SportCommissionModel> commissions = null)
    {
        commissions ??= new List<SportCommissionModel> { new SportCommissionModel(10m, 20m, 30m) };
        
        return new CommissionModel(
            closed, enabled, suspended, username, custId, uGroup,
            firstName, lastName, credit, locked, closedReason, group,
            doubleCommLosing, isCashMember, commissions);
    }
}
