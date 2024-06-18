using WealthTrack.Application.DTO;

namespace WealthTrack.Application.Services.CardS;

public interface ICardService
{
    Task<IEnumerable<CardDto>> GetAllAsync(int userId);
    Task CreateCardAsync(CreateCardDto dto, int userId);
    Task UpdateCardAsync(int id, UpdateCardDto dto, int userId);
    Task DeleteCardAsync(int id, int userId);
}