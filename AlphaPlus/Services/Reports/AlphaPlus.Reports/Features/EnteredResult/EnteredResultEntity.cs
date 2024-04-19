using System.Text.Json.Serialization;
using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Features.EnteredResult;

public class EnteredResultEntity
{
    public EnteredResultEntity()
    {
    }

    public EnteredResultEntity(DateTime eventDate, int entered, int nonEntered)
    {
        EventDate = eventDate;
        Entered = entered;
        NonEntered = nonEntered;
    }

    public bool IsToday { get; init; }

    [JsonIgnore]
    public DateTime EventDate { get; init; }

    public string EventDateText => EventDate.ToMMDDWithSlash();

    public int SportType { get; init; }

    public int Entered { get; init; }

    public int NonEntered { get; init; }
}
