namespace WealthTrack.Domain.Exceptions;

public class TransactionInvalidAmount : CustomException
{
    public TransactionInvalidAmount() : base("Transaction must have positive amount")
    {
        
    }
}