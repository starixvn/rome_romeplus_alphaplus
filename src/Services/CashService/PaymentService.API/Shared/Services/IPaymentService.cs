namespace AlphaPlus.PaymentService.API.Shared;

public interface IPaymentService
{
    string Deposit();

    string Withdraw();
}