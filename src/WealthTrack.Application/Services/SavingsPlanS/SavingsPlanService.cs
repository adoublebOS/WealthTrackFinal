using WealthTrack.Application.DTO;
using WealthTrack.Domain.Entities;
using WealthTrack.Domain.Exceptions;
using WealthTrack.Infrastructure.Repository;

namespace WealthTrack.Application.Services.SavingsPlanS;

public class SavingsPlanService : ISavingsPlanService
{
    private readonly IRepository<SavingsPlan> _savingsPlanRepository;
    private readonly IRepository<User> _userRepository;

    public SavingsPlanService(IRepository<SavingsPlan> savingsPlanRepository, 
        IRepository<User> userRepository)
    {
        _savingsPlanRepository = savingsPlanRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<SavingsPlanDto>> GetAllAsync(int userId)
    {
        var savingsPlans = await _savingsPlanRepository
            .GetAllByFilterAsync(
                sp => sp.User!.Id == userId,
                sp => sp.User!);

        return savingsPlans
            .OrderByDescending(sp => sp.CreatedAt)
            .Select(sp => sp.AsDto())
            .ToList();
    }

    public async Task CreateSavingsPlanAsync(CreateSavingsPlanDto dto, int userId)
    {
        var user = await _userRepository.GetOneAsync(u => u.Id == userId) ??
                   throw new UserNotFoundException();
        var savingsPlan = dto.AsEntity(user);
        
        _userRepository.Attach(user);
        
        await _savingsPlanRepository.CreateAsync(savingsPlan);
        await _savingsPlanRepository.SaveChangesAsync();
    }

    public async Task UpdateSavingsPlanAsync(int id, UpdateSavingsPlanDto dto, int userId)
    {
        var savingsPlan = await _savingsPlanRepository.GetOneAsync(
            sp => sp.Id == id,
            sp => sp.User!);

        if (savingsPlan is null) throw new SavingsPlanNotFoundException();
        if (savingsPlan.User!.Id != userId) throw new AccessDeniedException();

        dto.UpdateEntity(savingsPlan);

        _userRepository.Attach(savingsPlan.User);
        
        await _savingsPlanRepository.UpdateAsync(id, savingsPlan);
        await _savingsPlanRepository.SaveChangesAsync();
    }

    public async Task DeleteSavingsPlanAsync(int id, int userId)
    {
        var savingsPlan = await _savingsPlanRepository.GetOneAsync(
            sp => sp.Id == id,
            sp => sp.User!);

        if (savingsPlan is null) throw new SavingsPlanNotFoundException();
        if (savingsPlan.User!.Id != userId) throw new AccessDeniedException();

        await _savingsPlanRepository.SoftDeleteAsync(id);
        await _savingsPlanRepository.SaveChangesAsync();
    }
}