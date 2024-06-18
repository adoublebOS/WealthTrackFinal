namespace WealthTrack.Domain.Exceptions;

public class TransactionNotFoundException : NotFoundException
{
    public TransactionNotFoundException() : base("Transaction not found.") { }
}