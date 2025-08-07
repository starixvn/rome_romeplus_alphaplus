namespace AlphaPlus.CashService.API.Features.Shared.Constants;

public static class ErrorCodeConstants
{
    public const int Success = 0;
    public const int AddPaymentMethodWarning = 200;

    public static bool IsSuccess(int errorCode) {
        return errorCode == Success;
    }

    public static bool IsWarning(int errorCode) {
        return errorCode == AddPaymentMethodWarning;
    }
}