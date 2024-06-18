using WealthTrack.Application.DTO;
using WealthTrack.Domain.Entities;
using WealthTrack.Domain.Exceptions;
using WealthTrack.Domain.Exceptions.Card;
using WealthTrack.Infrastructure.Repository;

namespace WealthTrack.Application.Services.CardS;

public class CardService : ICardService
{
    private readonly IRepository<Card> _cardRepository;
    private readonly IRepository<User> _userRepository;

    public CardService(IRepository<Card> cardRepository, 
        IRepository<User> userRepository)
    {
        _cardRepository = cardRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<CardDto>> GetAllAsync(int userId)
    {
        var cards = await _cardRepository
            .GetAllByFilterAsync(
                c => c.User!.Id == userId,
                c => c.User!
            );

        return cards
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => c.AsDto())
            .ToList();
    }

    public async Task CreateCardAsync(CreateCardDto dto, int userId)
    {
        var user = await _userRepository.GetOneAsync(u => u.Id == userId) ??
                   throw new UserNotFoundException();
        var card = dto.AsEntity(user);

        _userRepository.Attach(user);
        
        await _cardRepository.CreateAsync(card);
        await _cardRepository.SaveChangesAsync();
    }

    public async Task UpdateCardAsync(int id, UpdateCardDto dto, int userId)
    {
        var card = await _cardRepository.GetOneAsync(
            c => c.Id == id,
            c => c.User!
        );

        if (card is null) throw new CardNotFoundException();
        if (card.User!.Id != userId) throw new AccessDeniedException();
        
        dto.UpdateEntity(card);
        
        _userRepository.Attach(card.User);
        
        await _cardRepository.UpdateAsync(id, card);
        await _cardRepository.SaveChangesAsync();
    }

    public async Task DeleteCardAsync(int id, int userId)
    {
        var card = await _cardRepository.GetOneAsync(
            c => c.Id == id,
            c => c.User!
        );

        if (card is null) throw new CardNotFoundException();
        if (card.User!.Id != userId) throw new AccessDeniedException();
        
        await _cardRepository.SoftDeleteAsync(id);
        await _cardRepository.SaveChangesAsync();
    }
}