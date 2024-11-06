using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class UpdatePersonalPresetCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public int ID { get; set; }

        public string PresetTitle { get; set; }

        public decimal WinningThreshold { get; set; }

        public decimal LosingThreshold { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}