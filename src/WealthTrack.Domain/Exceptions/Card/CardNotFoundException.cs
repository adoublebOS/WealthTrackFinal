namespace WealthTrack.Domain.Exceptions.Card;

public class CardNotFoundException : NotFoundException
{
    public CardNotFoundException() : base("Card not found.") { }
}