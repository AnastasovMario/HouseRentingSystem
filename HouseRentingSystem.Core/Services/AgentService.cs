using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Services
{
    public class AgentService : IAgentService
    {
        private readonly IRepository repo;

        public AgentService(IRepository _repo)
        {
            repo = _repo;
        }
        public async Task<bool> ExistsByIdAsync(string userId)
        {
            return await repo.All<Agent>()
                .AnyAsync(agent => agent.UserId == userId);
        }
    }
}
