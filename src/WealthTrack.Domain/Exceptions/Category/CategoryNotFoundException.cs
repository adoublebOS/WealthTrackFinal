namespace WealthTrack.Domain.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException() : base("Category not found.") { }
}