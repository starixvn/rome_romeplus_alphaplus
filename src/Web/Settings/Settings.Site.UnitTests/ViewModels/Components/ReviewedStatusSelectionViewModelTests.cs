using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.UnitTests.ViewModels.Components;

public class ReviewedStatusSelectionViewModelTests
{
    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Initialize_With_Correct_Values()
    {
        // Arrange
        var statuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Pending" },
            new SelectListItem { Value = "2", Text = "Reviewed" },
            new SelectListItem { Value = "3", Text = "Rejected" }
        };
        var userSelectedId = 2;

        // Act
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, userSelectedId);

        // Assert
        viewModel.Statuses.Should().BeEquivalentTo(statuses);
        viewModel.UserSelectedReviewedStatusId.Should().Be(userSelectedId);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Have_ReadOnly_Properties()
    {
        // Arrange
        var statuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "All" }
        };
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, 0);

        // Act & Assert
        viewModel.Statuses.Should().BeEquivalentTo(statuses);
        viewModel.UserSelectedReviewedStatusId.Should().Be(0);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_Empty_Statuses()
    {
        // Arrange
        var statuses = new List<SelectListItem>();
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, 0);

        // Act & Assert
        viewModel.Statuses.Should().BeEmpty();
        viewModel.UserSelectedReviewedStatusId.Should().Be(0);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_Null_Statuses()
    {
        // Arrange
        IEnumerable<SelectListItem> statuses = null;
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, -1);

        // Act & Assert
        viewModel.Statuses.Should().BeNull();
        viewModel.UserSelectedReviewedStatusId.Should().Be(-1);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_Zero_UserSelectedId()
    {
        // Arrange
        var statuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "None" }
        };
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, 0);

        // Act & Assert
        viewModel.UserSelectedReviewedStatusId.Should().Be(0);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_Negative_UserSelectedId()
    {
        // Arrange
        var statuses = new List<SelectListItem>();
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, -5);

        // Act & Assert
        viewModel.UserSelectedReviewedStatusId.Should().Be(-5);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_Large_UserSelectedId()
    {
        // Arrange
        var statuses = new List<SelectListItem>();
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, 9999);

        // Act & Assert
        viewModel.UserSelectedReviewedStatusId.Should().Be(9999);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_SelectListItem_With_Selected_Property()
    {
        // Arrange
        var statuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Pending", Selected = false },
            new SelectListItem { Value = "2", Text = "Reviewed", Selected = true },
            new SelectListItem { Value = "3", Text = "Rejected", Selected = false }
        };
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, 2);

        // Act & Assert
        viewModel.Statuses.Should().HaveCount(3);
        viewModel.Statuses.ElementAt(1).Selected.Should().BeTrue();
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_SelectListItem_With_Disabled_Property()
    {
        // Arrange
        var statuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Pending", Disabled = false },
            new SelectListItem { Value = "2", Text = "Reviewed", Disabled = true }
        };
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, 1);

        // Act & Assert
        viewModel.Statuses.Should().HaveCount(2);
        viewModel.Statuses.ElementAt(1).Disabled.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(-1)]
    public void ReviewedStatusSelectionViewModel_Should_Handle_Various_UserSelectedIds(int userSelectedId)
    {
        // Arrange
        var statuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "All" },
            new SelectListItem { Value = "1", Text = "Pending" },
            new SelectListItem { Value = "2", Text = "Reviewed" },
            new SelectListItem { Value = "-1", Text = "None" }
        };

        // Act
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, userSelectedId);

        // Assert
        viewModel.UserSelectedReviewedStatusId.Should().Be(userSelectedId);
        viewModel.Statuses.Should().HaveCount(4);
    }

    [Fact]
    public void ReviewedStatusSelectionViewModel_Should_Handle_Large_Statuses_Collection()
    {
        // Arrange
        var statuses = new List<SelectListItem>();
        for (int i = 0; i < 100; i++)
        {
            statuses.Add(new SelectListItem { Value = i.ToString(), Text = $"Status {i}" });
        }
        var viewModel = new ReviewedStatusSelectionViewModel(statuses, 50);

        // Act & Assert
        viewModel.Statuses.Should().HaveCount(100);
        viewModel.UserSelectedReviewedStatusId.Should().Be(50);
    }
}
