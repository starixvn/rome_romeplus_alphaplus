using AlphaPlus.Features.Messages.API.Models;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Models
{
    public class PagedReportDataTests
    {
        [Fact]
        public void Constructor_WithValidData_SetsPropertiesCorrectly()
        {
            // Arrange
            var dataSource = new List<string> { "item1", "item2", "item3" };
            var pageIndex = 1;
            var pageSize = 10;
            var totalRecords = 25;

            // Act
            var result = new PagedReportData<string>(dataSource, pageIndex, pageSize, totalRecords);

            // Assert
            Assert.Equal(dataSource, result.DataSource);
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(totalRecords, result.TotalRecords);
        }

        [Fact]
        public void Constructor_WithEmptyDataSource_SetsPropertiesCorrectly()
        {
            // Arrange
            var dataSource = new List<string>();
            var pageIndex = 1;
            var pageSize = 10;
            var totalRecords = 0;

            // Act
            var result = new PagedReportData<string>(dataSource, pageIndex, pageSize, totalRecords);

            // Assert
            Assert.Equal(dataSource, result.DataSource);
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(totalRecords, result.TotalRecords);
        }

        [Fact]
        public void Any_WhenTotalRecordsIsZero_ReturnsFalse()
        {
            // Arrange
            var result = new PagedReportData<string>(new List<string>(), 1, 10, 0);

            // Act & Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void Any_WhenTotalRecordsIsGreaterThanZero_ReturnsTrue()
        {
            // Arrange
            var result = new PagedReportData<string>(new List<string> { "item1" }, 1, 10, 1);

            // Act & Assert
            Assert.True(result.Any());
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(10, true)]
        [InlineData(100, true)]
        public void MoreThanOne_WithDifferentTotalRecords_ReturnsExpectedResult(int totalRecords, bool expected)
        {
            // Arrange
            var result = new PagedReportData<string>(new List<string>(), 1, 10, totalRecords);

            // Act & Assert
            Assert.Equal(expected, result.MoreThanOne());
        }

        [Fact]
        public void MoreThanOne_WhenTotalRecordsIsZero_ReturnsFalse()
        {
            // Arrange
            var result = new PagedReportData<string>(new List<string>(), 1, 10, 0);

            // Act & Assert
            Assert.False(result.MoreThanOne());
        }

        [Fact]
        public void MoreThanOne_WhenTotalRecordsIsOne_ReturnsFalse()
        {
            // Arrange
            var result = new PagedReportData<string>(new List<string> { "item1" }, 1, 10, 1);

            // Act & Assert
            Assert.False(result.MoreThanOne());
        }

        [Fact]
        public void Properties_AreReadOnly()
        {
            // Arrange
            var dataSource = new List<string> { "item1" };
            var result = new PagedReportData<string>(dataSource, 1, 10, 1);

            // Act & Assert
            Assert.Equal(dataSource, result.DataSource);
            
            // Verify properties don't have setters (are read-only)
            var type = typeof(PagedReportData<string>);
            Assert.False(type.GetProperty(nameof(result.DataSource)).CanWrite);
            Assert.False(type.GetProperty(nameof(result.PageIndex)).CanWrite);
            Assert.False(type.GetProperty(nameof(result.PageSize)).CanWrite);
            Assert.False(type.GetProperty(nameof(result.TotalRecords)).CanWrite);
        }

        [Theory]
        [InlineData(-1, 10, 0)]
        [InlineData(0, 10, 0)]
        [InlineData(1, -1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 10, -1)]
        public void Constructor_WithEdgeCaseValues_DoesNotThrow(int pageIndex, int pageSize, int totalRecords)
        {
            // Arrange
            var dataSource = new List<string>();

            // Act & Assert - Should not throw
            var result = new PagedReportData<string>(dataSource, pageIndex, pageSize, totalRecords);
            Assert.NotNull(result);
        }

        [Fact]
        public void Constructor_WithNullDataSource_DoesNotThrow()
        {
            // Act & Assert - Should not throw
            var result = new PagedReportData<string>(null, 1, 10, 0);
            Assert.NotNull(result);
            Assert.Null(result.DataSource);
        }

        [Fact]
        public void WithDifferentTypes_WorksCorrectly()
        {
            // Arrange
            var personData = new List<Person> { new Person { Name = "John" }, new Person { Name = "Jane" } };
            var stringData = new List<string> { "a", "b", "c" };

            // Act
            var personResult = new PagedReportData<Person>(personData, 1, 10, 2);
            var stringResult = new PagedReportData<string>(stringData, 1, 10, 3);

            // Assert
            Assert.Equal(personData, personResult.DataSource);
            Assert.Equal(stringData, stringResult.DataSource);
            Assert.True(personResult.Any());
            Assert.True(stringResult.Any());
            Assert.True(personResult.MoreThanOne());
            Assert.True(stringResult.MoreThanOne());
        }

        private class Person
        {
            public string Name { get; set; }
        }
    }
}
