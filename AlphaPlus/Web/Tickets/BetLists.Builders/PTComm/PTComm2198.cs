using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Utils;

namespace AlphaPlus.BetLists.Builders;

public class PTComm2198(ITicket ticket, UserLevel userLevel) : PTComm(ticket, userLevel)
{
    public override string PositionTaking => FormatPositionTaking(PtByUserLevel[UserLevel]);

    private static string FormatPositionTaking(object value)
    {
        var ptValue = Cast.AsDecimal(value);

        return ptValue < 0
            ? string.Format(ConvertByBetType.Minus, string.Format(Formatter.DECIMALFORMAT40, ptValue))
            : string.Format(Formatter.DECIMALFORMAT40, ptValue);
    }
}
