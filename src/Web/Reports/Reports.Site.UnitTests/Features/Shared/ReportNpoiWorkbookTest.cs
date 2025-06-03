using AlphaPlus.Reports.Site.Features.Shared.Tests;
using AlphaPlus.Reports.Site.Features.Shared;
using Microsoft.AspNetCore.Hosting;

namespace AlphaPlus.Reports.Site.Features.Shared.Tests
{

    [Trait("Category", "Shared")]
    public class ReportNpoiWorkbookTest
    {
        private readonly Mock<IWebHostEnvironment> webHostEnvironmentMock;
        private readonly TestReportNpoiWorkbook workbook;

        public ReportNpoiWorkbookTest()
        {
            webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("");
            workbook = new TestReportNpoiWorkbook(webHostEnvironmentMock.Object);
        }

        [Fact]
        public void GetExcelTemplatePath_Should_Throw_ArgumentNullException_When_TemplateName_Is_Null()
        {
            // Arrange
            string templateName = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => workbook.GetExcelTemplatePathPublic(templateName));
            Assert.Equal("templateName", exception.ParamName);
        }

        [Fact]
        public void GetExcelTemplatePath_Should_Throw_ArgumentNullException_When_TemplateName_Is_Empty()
        {
            // Arrange
            var templateName = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => workbook.GetExcelTemplatePathPublic(templateName));
            Assert.Equal("templateName", exception.ParamName);
        }

        [Fact]
        public void GetExcelTemplatePath_Should_Throw_FileNotFoundException_When_File_Does_Not_Exist()
        {
            // Arrange
            var templateName = "NonExistentTemplate";

            // Mock file does not exist
            File.Exists_Result = false;

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => workbook.GetExcelTemplatePathPublic(templateName));
            Assert.Contains("does not exist", exception.Message);
        }

        [Fact]
        public void GetExcelTemplateFolder_Should_Return_Correct_Namespace_Part()
        {
            // Act
            var result = workbook.GetExcelTemplateFolderPublic();

            // Assert
            Assert.Equal("Tests", result);
        }

        [Fact]
        public void GetExcelTemplateFolder_Should_Throw_ArgumentException_When_No_Dot_In_Namespace()
        {
            // Arrange
            var invalidWorkbook = new InvalidNamespaceReportNpoiWorkbook(webHostEnvironmentMock.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => invalidWorkbook.GetExcelTemplateFolderPublic());
            Assert.Contains("Namespace does not contain any dot character", exception.Message);
        }

        // Test helper classes
        public class TestReportNpoiWorkbook : ReportNpoiWorkbook<object>
        {
            public TestReportNpoiWorkbook(IWebHostEnvironment webHostEnvironment)
                : base(webHostEnvironment)
            {
            }

            public string GetExcelTemplatePathPublic(string templateName, string fileExtension = "xls")
            {
                return GetExcelTemplatePath(templateName, fileExtension);
            }

            public string GetExcelTemplateFolderPublic()
            {
                return GetExcelTemplateFolder();
            }

            // Override to ensure we get a predictable namespace for testing
            public new Type GetType()
            {
                return typeof(AlphaPlus.Reports.Site.Features.Shared.TestNamespace.DummyClass);
            }
        }

        public static class File
        {
            public static bool Exists_Result { get; set; }

            public static bool Exists(string path)
            {
                return Exists_Result;
            }
        }
    }

    public class NoNamespaceDummyClass { }
}

// Classes used for namespace testing
namespace AlphaPlus.Reports.Site.Features.Shared.TestNamespace
{
    public class DummyClass { }
}

namespace AlphaPlus
{
    public class InvalidNamespaceReportNpoiWorkbook : ReportNpoiWorkbook<object>
    {
        public InvalidNamespaceReportNpoiWorkbook(IWebHostEnvironment webHostEnvironment)
            : base(webHostEnvironment)
        {
        }

        public string GetExcelTemplateFolderPublic()
        {
            return GetExcelTemplateFolder();
        }

        // Override to return a type with no dots in namespace
        public new Type GetType()
        {
            return typeof(NoNamespaceDummyClass);
        }
    }
}
