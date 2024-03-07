namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;

    public class AddMultiplePersonalPresetCommand : MemberInfoBaseQuery, IRequest<IEnumerable<NamedResult>>
    {
        public AddMultiplePersonalPresetCommand() : this(null){}
        
        public AddMultiplePersonalPresetCommand(IUser viewer) : base(viewer)
        {
        }

        public PresetInfo[] Presets { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}