namespace WealthTrack.Domain.Exceptions;

public class WalletLimitExceededException : CustomException
{
    public WalletLimitExceededException() : base("The transaction amount exceeds the wallet limit for the current month.") { }
}