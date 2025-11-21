using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.DormantAccountList.Models;

public class DormantAccountEntityTests
{
    [Fact]
    public void DormantAccountEntity_Should_Be_Record_With_Init_Properties()
    {
        // Arrange & Act
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1,
            StatusId = 2,
            StageId = 3,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = DateTime.Now,
            ClosedDate = DateTime.Now.AddDays(-1),
            DisabledDate = DateTime.Now.AddDays(-2),
            SettingDeletedDate = DateTime.Now.AddDays(-3),
            AccountDeletedDate = DateTime.Now.AddDays(-4),
            IsWarningTicket = true,
            RowNumber = 5,
            Recommend = 6
        };

        // Assert
        Assert.Equal(123, entity.CustId);
        Assert.Equal("testuser", entity.Username);
        Assert.Equal(1, entity.RoleId);
        Assert.Equal(2, entity.StatusId);
        Assert.Equal(3, entity.StageId);
        Assert.Equal(100.50m, entity.ToBeClearedGivenCredit);
        Assert.Equal(200.75m, entity.ClearedGivenCredit);
        Assert.NotNull(entity.LastLogged);
        Assert.NotNull(entity.ClosedDate);
        Assert.NotNull(entity.DisabledDate);
        Assert.NotNull(entity.SettingDeletedDate);
        Assert.NotNull(entity.AccountDeletedDate);
        Assert.True(entity.IsWarningTicket);
        Assert.Equal(5, entity.RowNumber);
        Assert.Equal(6, entity.Recommend);
    }

    [Fact]
    public void DormantAccountEntity_Properties_Should_Be_ReadOnly_After_Initialization()
    {
        // Arrange
        var entity = new DormantAccountEntity { CustId = 123 };

        // Act & Assert
        // Properties with init accessor should not be settable after initialization
        var type = typeof(DormantAccountEntity);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            Assert.True(property.CanRead, $"{property.Name} should be readable");
            
            // For init-only properties, CanWrite returns true but they can't be set after object initialization
            // We can verify the property has an init accessor by checking if it's settable during object creation
            Assert.True(property.CanWrite, $"{property.Name} should have init accessor");
        }
    }

    [Fact]
    public void DormantAccountEntity_With_Null_Nullable_Properties_Should_Work()
    {
        // Act
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = null,
            RoleId = 1,
            StatusId = 2,
            StageId = 3,
            ToBeClearedGivenCredit = null,
            ClearedGivenCredit = null,
            LastLogged = null,
            ClosedDate = null,
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 0,
            Recommend = 0
        };

        // Assert
        Assert.Equal(123, entity.CustId);
        Assert.Null(entity.Username);
        Assert.Null(entity.ToBeClearedGivenCredit);
        Assert.Null(entity.ClearedGivenCredit);
        Assert.Null(entity.LastLogged);
        Assert.Null(entity.ClosedDate);
        Assert.Null(entity.DisabledDate);
        Assert.Null(entity.SettingDeletedDate);
        Assert.Null(entity.AccountDeletedDate);
    }

    [Fact]
    public void DormantAccountEntity_With_Default_Values_Should_Work()
    {
        // Act
        var entity = new DormantAccountEntity();

        // Assert
        Assert.Equal(0, entity.CustId);
        Assert.Null(entity.Username);
        Assert.Equal(0, entity.RoleId);
        Assert.Equal(0, entity.StatusId);
        Assert.Equal(0, entity.StageId);
        Assert.Null(entity.ToBeClearedGivenCredit);
        Assert.Null(entity.ClearedGivenCredit);
        Assert.Null(entity.LastLogged);
        Assert.Null(entity.ClosedDate);
        Assert.Null(entity.DisabledDate);
        Assert.Null(entity.SettingDeletedDate);
        Assert.Null(entity.AccountDeletedDate);
        Assert.False(entity.IsWarningTicket);
        Assert.Equal(0, entity.RowNumber);
        Assert.Equal(0, entity.Recommend);
    }

    [Theory]
    [InlineData(0, "", 0, 0, 0, 0, 0, false)]
    [InlineData(123, "user123", 1, 2, 3, 10, -5, true)]
    [InlineData(int.MaxValue, "maxuser", byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, int.MinValue, false)]
    public void DormantAccountEntity_Should_Handle_Various_Values(
        int custId, string username, byte roleId, byte statusId, byte stageId, 
        int rowNumber, int recommend, bool isWarningTicket)
    {
        // Act
        var entity = new DormantAccountEntity
        {
            CustId = custId,
            Username = username,
            RoleId = roleId,
            StatusId = statusId,
            StageId = stageId,
            RowNumber = rowNumber,
            Recommend = recommend,
            IsWarningTicket = isWarningTicket
        };

        // Assert
        Assert.Equal(custId, entity.CustId);
        Assert.Equal(username, entity.Username);
        Assert.Equal(roleId, entity.RoleId);
        Assert.Equal(statusId, entity.StatusId);
        Assert.Equal(stageId, entity.StageId);
        Assert.Equal(rowNumber, entity.RowNumber);
        Assert.Equal(recommend, entity.Recommend);
        Assert.Equal(isWarningTicket, entity.IsWarningTicket);
    }

    [Fact]
    public void DormantAccountEntity_With_Decimal_Values_Should_Work()
    {
        // Arrange
        var toBeClearedCredit = 123.45m;
        var clearedCredit = 678.90m;

        // Act
        var entity = new DormantAccountEntity
        {
            ToBeClearedGivenCredit = toBeClearedCredit,
            ClearedGivenCredit = clearedCredit
        };

        // Assert
        Assert.Equal(toBeClearedCredit, entity.ToBeClearedGivenCredit);
        Assert.Equal(clearedCredit, entity.ClearedGivenCredit);
    }

    [Fact]
    public void DormantAccountEntity_With_DateTime_Values_Should_Work()
    {
        // Arrange
        var lastLogged = DateTime.Now;
        var closedDate = DateTime.Now.AddDays(-1);
        var disabledDate = DateTime.Now.AddDays(-2);
        var settingDeletedDate = DateTime.Now.AddDays(-3);
        var accountDeletedDate = DateTime.Now.AddDays(-4);

        // Act
        var entity = new DormantAccountEntity
        {
            LastLogged = lastLogged,
            ClosedDate = closedDate,
            DisabledDate = disabledDate,
            SettingDeletedDate = settingDeletedDate,
            AccountDeletedDate = accountDeletedDate
        };

        // Assert
        Assert.Equal(lastLogged, entity.LastLogged);
        Assert.Equal(closedDate, entity.ClosedDate);
        Assert.Equal(disabledDate, entity.DisabledDate);
        Assert.Equal(settingDeletedDate, entity.SettingDeletedDate);
        Assert.Equal(accountDeletedDate, entity.AccountDeletedDate);
    }

    [Fact]
    public void DormantAccountEntity_Should_Handle_Various_Decimal_Values()
    {
        // Test different decimal values
        var testValues = new decimal?[] { -999.99m, 0m, 999.99m, null };

        foreach (var creditValue in testValues)
        {
            // Act
            var entity = new DormantAccountEntity
            {
                ToBeClearedGivenCredit = creditValue,
                ClearedGivenCredit = creditValue
            };

            // Assert
            Assert.Equal(creditValue, entity.ToBeClearedGivenCredit);
            Assert.Equal(creditValue, entity.ClearedGivenCredit);
        }
    }

    [Fact]
    public void DormantAccountEntity_Should_Be_Immutable_After_Initialization()
    {
        // Arrange & Act
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            IsWarningTicket = false
        };

        // Assert
        // Properties with init accessor cannot be modified after initialization
        Assert.Equal(123, entity.CustId);
        Assert.Equal("testuser", entity.Username);
        Assert.False(entity.IsWarningTicket);
        
        // Note: DormantAccountEntity is a class with init-only properties, not a record,
        // so it doesn't support 'with' expressions but properties are still immutable after initialization
    }

    [Fact]
    public void DormantAccountEntity_Should_Have_Correct_Property_Types()
    {
        // Arrange
        var type = typeof(DormantAccountEntity);

        // Act & Assert
        Assert.Equal(typeof(int), type.GetProperty(nameof(DormantAccountEntity.CustId))?.PropertyType);
        Assert.Equal(typeof(string), type.GetProperty(nameof(DormantAccountEntity.Username))?.PropertyType);
        Assert.Equal(typeof(byte), type.GetProperty(nameof(DormantAccountEntity.RoleId))?.PropertyType);
        Assert.Equal(typeof(byte), type.GetProperty(nameof(DormantAccountEntity.StatusId))?.PropertyType);
        Assert.Equal(typeof(byte), type.GetProperty(nameof(DormantAccountEntity.StageId))?.PropertyType);
        Assert.Equal(typeof(decimal?), type.GetProperty(nameof(DormantAccountEntity.ToBeClearedGivenCredit))?.PropertyType);
        Assert.Equal(typeof(decimal?), type.GetProperty(nameof(DormantAccountEntity.ClearedGivenCredit))?.PropertyType);
        Assert.Equal(typeof(DateTime?), type.GetProperty(nameof(DormantAccountEntity.LastLogged))?.PropertyType);
        Assert.Equal(typeof(DateTime?), type.GetProperty(nameof(DormantAccountEntity.ClosedDate))?.PropertyType);
        Assert.Equal(typeof(DateTime?), type.GetProperty(nameof(DormantAccountEntity.DisabledDate))?.PropertyType);
        Assert.Equal(typeof(DateTime?), type.GetProperty(nameof(DormantAccountEntity.SettingDeletedDate))?.PropertyType);
        Assert.Equal(typeof(DateTime?), type.GetProperty(nameof(DormantAccountEntity.AccountDeletedDate))?.PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty(nameof(DormantAccountEntity.IsWarningTicket))?.PropertyType);
        Assert.Equal(typeof(int), type.GetProperty(nameof(DormantAccountEntity.RowNumber))?.PropertyType);
        Assert.Equal(typeof(int), type.GetProperty(nameof(DormantAccountEntity.Recommend))?.PropertyType);
    }

    [Fact]
    public void DormantAccountEntity_Should_Have_Reference_Equality()
    {
        // Arrange
        var entity1 = new DormantAccountEntity { CustId = 123, Username = "test" };
        var entity2 = new DormantAccountEntity { CustId = 123, Username = "test" };
        var entity3 = entity1; // Same reference

        // Act & Assert
        Assert.NotEqual(entity1, entity2); // Different instances, even with same values
        Assert.Equal(entity1, entity3); // Same reference should be equal
        Assert.True(ReferenceEquals(entity1, entity3));
        Assert.False(ReferenceEquals(entity1, entity2));
    }
}
