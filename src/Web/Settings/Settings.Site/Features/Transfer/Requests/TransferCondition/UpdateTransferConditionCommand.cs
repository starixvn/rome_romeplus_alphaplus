using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferCondition;

public class UpdateTransferConditionCommand : TransferBaseQuery, IRequest<Result>
{
    public string TransferOption { get; set; }

    public bool AutoTransferOption { get; set; }

    public int SubAccId => viewer.SubAccId;

    public string IP => viewer.IP;

    public string LangKey => viewer.LanguageKey;

    public override bool IsValid()
        => !viewer.IsInternal && IsValidTransferOption() && base.IsValid();

    private bool IsValidTransferOption()
        => !string.IsNullOrWhiteSpace(TransferOption)
        && TransferOption.Length == 8
        && TransferOption != "11111111";
}
