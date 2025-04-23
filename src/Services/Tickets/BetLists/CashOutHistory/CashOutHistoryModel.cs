using System.Xml.Linq;

namespace AlphaPlus.BetLists.CashOutHistory;

public class CashOutHistoryModel
{
    public long TransId { get; set; }

    public string CashoutExtraInfo { get; set; }

    public IEnumerable<CashoutHistoryDetail> DetailHistory
    {
        get
        {
            if (!string.IsNullOrEmpty(CashoutExtraInfo))
            {
                var root = XElement.Parse(CashoutExtraInfo);

                foreach (XElement item in root.Elements())
                {
                    var time = item.Attribute("ct").Value;
                    var amount = item.Attribute("w").Value;
                    var action = item.Attribute("s").Value;

                    yield return new CashoutHistoryDetail(DateTime.Parse(time), decimal.Parse(amount), action);
                }
            }
        }
    }
}
