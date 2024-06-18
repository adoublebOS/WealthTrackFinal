namespace WealthTrack.Domain.Exceptions;

public class WalletLimitNotFoundException : NotFoundException
{
    public WalletLimitNotFoundException() : base("Limit for the wallet for the selected month not found.") { }
}