
namespace AlphaPlus.Reports.Features.Customer;

public record UsernameSuggestionParam(
    int ViewerId,
    string Username,
    bool IsInternal,
    bool DirectDownline,
    bool IsAccInfo);