using AliHussainPortfolio.Application.DTOs;

namespace AliHussainPortfolio.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken cancellationToken = default);
}
