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

    public DateTime EventDate { get; init; }

    public int SportType { get; init; }

    public int Entered { get; init; }

    public int NonEntered { get; init; }
}
