using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository repo;

        public HouseService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<HousesQueryServiceModel> All(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1)
        {
            var result = new HousesQueryServiceModel();
            var housesQuery = repo.AllReadonly<House>();
                //.Where(h => h.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                housesQuery = housesQuery.Where(h => h.Category.Name == category);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                housesQuery = housesQuery
                    .Where(h => h.Title.Contains(searchTerm)
                    || h.Address.Contains(searchTerm)
                    || h.Description.Contains(searchTerm));
            }

            housesQuery = sorting switch
            {
                HouseSorting.Price => housesQuery
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => housesQuery
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.Id),
                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            var houses = await housesQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();

            var totalHousesCount = await housesQuery.CountAsync();

            return new()
            {
                TotalHousesCount = totalHousesCount,
                Houses = houses,
            };
        }

        public async Task<IEnumerable<HouseCategoryModel>> AllCategories()
        {
            return await repo.AllReadonly<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new HouseCategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repo.AllReadonly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int agentId)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.AgentId == agentId)
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.RenterId == userId)
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repo.AllReadonly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(HouseModel model, int agentId)
        {
            var house = new House()
            {
                Address = model.Address,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                Title = model.Title,
                AgentId = agentId
            };

            await repo.AddAsync(house);
            await repo.SaveChangesAsync();

            return house.Id;
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            bool result = false;
            var house = await repo.AllReadonly<House>()
                //.Where(h => h.IsActive)
                .Where(h => h.Id == houseId)
                .Include(h => h.Agent)
                .FirstOrDefaultAsync();

            if (house?.Agent != null && house.Agent.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task Edit(int houseId, HouseModel houseModel)
        {
            var house = await repo.GetByIdAsync<House>(houseId);

            house.Description = houseModel.Description;
            house.ImageUrl = houseModel.ImageUrl;
            house.PricePerMonth = houseModel.PricePerMonth;
            house.Title = houseModel.Title;
            house.Address = houseModel.Address;
            house.CategoryId = houseModel.CategoryId;

            await repo.SaveChangesAsync();

        }

        public async Task<bool> Exists(int Id)
            => await repo.AllReadonly<House>()
            .AnyAsync(h => h.Id == Id);

        public async Task<HouseDetailsServiceModel> HouseDetailsById(int id)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.Id == id)
                .Select(h => new HouseDetailsServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    Description = h.Description,
                    Category = h.Category.Name,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Agent = new AgentServiceModel()
                    {
                        Email = h.Agent.User.Email,
                        PhoneNumber = h.Agent.PhoneNumber
                    }
                })
                .FirstAsync();
        }

        //само ще ги четем
        public async Task<IEnumerable<HouseHomeModel>> LastThreeHouses()
        {
            return await repo.AllReadonly<House>()
                .OrderByDescending(h => h.Id)
                .Select(h => new HouseHomeModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    ImageUrl = h.ImageUrl,
                    Address = h.Address
                })
                .Take(3)
                .ToListAsync();
        }

        public async Task<int> GetHouseCategoryId(int houseId)
        {
            return (await repo.GetByIdAsync<House>(houseId)).CategoryId;
        }

        public async Task Delete(int houseId)
        {
            await repo.DeleteAsync<House>(houseId);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> IsRented(int Id)
        {
            var house = await repo.GetByIdAsync<House>(Id);
            return house.RenterId != null;
        }

        public async Task<bool> IsRentedByUser(int Id, string userId)
        {
            bool result = false;
            var house = await repo.GetByIdAsync<House>(Id);

            if (house != null && house.RenterId == userId)
            {
                result = true;
            }

            return result;
               
        }

        public async Task Rent(int Id, string userId)
        {
            var house = await repo.GetByIdAsync<House>(Id);

            if (house != null && house.RenterId != null)
            {
                throw new ArgumentException("House is already rented");
            }

            house.RenterId = userId;
            await repo.SaveChangesAsync();
        }

        public async Task Leave(int Id)
        {
            var house = await repo.GetByIdAsync<House>(Id);

            house.RenterId = null;
            await repo.SaveChangesAsync();
        }
    }
}
