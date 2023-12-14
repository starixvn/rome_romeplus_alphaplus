namespace Alpha.Main.Services
{
    using Params;

    public interface KickoutCustomerService
    {
        Task Kickout(KickoutCustomerParam param);
    }
}
