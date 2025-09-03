using AlphaPlus.Features.Handbook.API.Models;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Models
{
    public class ProfilePictureModelTests
    {
        [Fact]
        public void ImageBase64String_WhenImageIsNull_ReturnsEmptyString()
        {
            // Arrange
            var model = new ProfilePictureModel
            {
                Image = null
            };

            // Act
            var result = model.ImageBase64String;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ImageBase64String_WhenImageIsEmpty_ReturnsDataUriWithEmptyBase64()
        {
            // Arrange
            var model = new ProfilePictureModel
            {
                Image = new byte[0]
            };

            // Act
            var result = model.ImageBase64String;

            // Assert
            Assert.Equal("data:image/jpeg;base64,", result);
        }

        [Fact]
        public void ImageBase64String_WhenImageHasData_ReturnsCorrectDataUri()
        {
            // Arrange
            var imageBytes = new byte[] { 255, 216, 255, 224 }; // Sample JPEG header bytes
            var expectedBase64 = Convert.ToBase64String(imageBytes);
            var expectedResult = $"data:image/jpeg;base64,{expectedBase64}";
            
            var model = new ProfilePictureModel
            {
                Image = imageBytes
            };

            // Act
            var result = model.ImageBase64String;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ImageBase64String_WithLargeImage_ReturnsCorrectDataUri()
        {
            // Arrange
            var imageBytes = new byte[1024];
            for (int i = 0; i < imageBytes.Length; i++)
            {
                imageBytes[i] = (byte)(i % 256);
            }
            
            var expectedBase64 = Convert.ToBase64String(imageBytes);
            var expectedResult = $"data:image/jpeg;base64,{expectedBase64}";
            
            var model = new ProfilePictureModel
            {
                Image = imageBytes
            };

            // Act
            var result = model.ImageBase64String;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ImageId_CanBeSetAndRetrieved()
        {
            // Arrange
            var model = new ProfilePictureModel();
            var expectedImageId = 12345;

            // Act
            model.ImageId = expectedImageId;

            // Assert
            Assert.Equal(expectedImageId, model.ImageId);
        }

        [Fact]
        public void Modified_CanBeSetAndRetrieved()
        {
            // Arrange
            var model = new ProfilePictureModel();
            var expectedModified = DateTime.Now;

            // Act
            model.Modified = expectedModified;

            // Assert
            Assert.Equal(expectedModified, model.Modified);
        }

        [Fact]
        public void Modified_CanBeNull()
        {
            // Arrange & Act
            var model = new ProfilePictureModel
            {
                Modified = null
            };

            // Assert
            Assert.Null(model.Modified);
        }

        [Fact]
        public void Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var model = new ProfilePictureModel();

            // Assert
            Assert.Null(model.Image);
            Assert.Equal(0, model.ImageId);
            Assert.Null(model.Modified);
            Assert.Equal(string.Empty, model.ImageBase64String);
        }
    }
}
