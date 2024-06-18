using WealthTrack.Application.DTO;

namespace WealthTrack.Application.Services.SavingsPlanS;

public interface ISavingsPlanService
{
    Task<IEnumerable<SavingsPlanDto>> GetAllAsync(int userId);
    Task CreateSavingsPlanAsync(CreateSavingsPlanDto dto, int userId);
    Task UpdateSavingsPlanAsync(int id, UpdateSavingsPlanDto dto, int userId);
    Task DeleteSavingsPlanAsync(int id, int userId);
}