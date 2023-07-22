using HouseRentingSystem.Core.Models.House;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IHouseService
    {
        Task<IEnumerable<HouseHomeModel>> LastThreeHouses();

        Task<IEnumerable<HouseCategoryModel>> AllCategories();

        Task<bool> CategoryExists(int categoryId);

        Task<int> Create(HouseModel model, int agentId);

        Task<HousesQueryServiceModel> All(
                    string? category = null,
                    string? searchTerm = null,
                    HouseSorting sorting = HouseSorting.Newest,
                    int currentPage = 1,
                    int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNames();

        Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int agentId);

        Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId);

        Task<bool> Exists(int Id);

        Task<HouseDetailsServiceModel> HouseDetailsById(int id);

        Task Edit(int houseId, HouseModel houseModel);

        Task<bool> HasAgentWithId(int houseId, string currentUserId);

        Task<int> GetHouseCategoryId(int houseId);

        Task Delete(int houseId);

        Task<bool> IsRented(int Id);

        Task<bool> IsRentedByUser(int Id, string userId);

        Task Rent(int Id, string userId);
    }
}
