namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class UpdateMacauGamesStatusCriteria : UpdateDCSProductStatusCriteria
    {
        private const short MacauGamesStatusSettingTypeId = 21;

        public short SettingType => MacauGamesStatusSettingTypeId;

        public UpdateMacauGamesStatusCriteria(
            int custId,
            int subAccId,
            int userId,
            string ip,
            string productStatusXml) : base(custId, subAccId, userId, ip, productStatusXml)
        {
        }
    }
}