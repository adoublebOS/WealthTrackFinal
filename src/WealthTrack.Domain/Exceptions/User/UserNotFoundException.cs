namespace WealthTrack.Domain.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException() : base("User not found.") { }
}