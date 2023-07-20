using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Core.Services;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseDetailsServiceModel : HouseServiceModel
    {
        public string Description { get; init; } = null!;

        public string Category { get; init; } = null!;

        public AgentServiceModel Agent { get; init; } = null!;
    }
}
