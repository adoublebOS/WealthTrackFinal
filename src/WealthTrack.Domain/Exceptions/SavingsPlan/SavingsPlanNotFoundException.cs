namespace WealthTrack.Domain.Exceptions;

public class SavingsPlanNotFoundException : NotFoundException
{
    public SavingsPlanNotFoundException() : base("Savings Plan not found.") { }
}