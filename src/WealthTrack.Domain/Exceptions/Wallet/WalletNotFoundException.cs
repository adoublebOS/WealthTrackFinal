namespace WealthTrack.Domain.Exceptions;

public class WalletNotFoundException : NotFoundException
{
    public WalletNotFoundException() : base("Wallet not found.") { }
}