namespace AlphaPlus.Identity.API.Features.Auth
{
    public record ProductSetting
    {
        public int ProductId { get; init; }

        public int? GroupId { get; init; }

        public int SportType { get; init; }

        public int BetType { get; init; }

        public bool IsHidden { get; init; }

        public bool IsDisabled { get; init; }
    }
}