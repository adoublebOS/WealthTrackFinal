namespace WealthTrack.Domain.Exceptions;

public class AccessDeniedException : CustomException
{
    public AccessDeniedException() : base("Access denied.") { }
}