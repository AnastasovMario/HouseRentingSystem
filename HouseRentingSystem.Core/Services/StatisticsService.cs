using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Statistics;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository repo;
        public StatisticsService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<StatisticsServiceModel> Total()
        {
            var totalHouses = await repo.AllReadonly<House>()
                .CountAsync();
            var rentedHouses = await repo.AllReadonly<House>()
                .CountAsync(h => h.RenterId != null);

            return new()
            {
                TotalHouses = totalHouses,
                TotalRent = rentedHouses
            };
        }
    }
}
