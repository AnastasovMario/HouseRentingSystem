using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Extensions;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Extensions;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;

        public HouseController(
            IHouseService _houseService,
            IAgentService _agentService)
        {
            houseService = _houseService;
            agentService = _agentService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel model)
        {
            var housesResult = await houseService
                .All(model.Category, model.SearchTerm, model.Sorting, model.CurrentPage, AllHousesQueryModel.HousesPerPage);

            model.TotalHousesCount = housesResult.TotalHousesCount;
            model.Houses = housesResult.Houses;

            var housesCategories = await houseService.AllCategoriesNames();
            model.Categories = housesCategories;


            return View(model);
        }


        public async Task<IActionResult> Mine()
        {
            IEnumerable<HouseServiceModel> myHouses;

            var userId = this.User.Id();

            if (await agentService.ExistsById(userId))
            {
                var agentId = await agentService.GetAgentId(userId);
                myHouses = await houseService.AllHousesByAgentId(agentId);
            }
            else
            {
                myHouses = await houseService.AllHousesByUserId(userId);
            }


            return View(myHouses);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, string information)
        {

            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }
            var model = await houseService.HouseDetailsById(id);

            if (information != model.GetInformation())
            {
                TempData["ErrorMessage"] = "Don't touch my slug!";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //If not an agent, Return to redirect
            if (!(await agentService.ExistsById(User.Id())))
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }
            var model = new HouseModel()
            {
                HouseCategories = await houseService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseModel model)
        {
            if (!await agentService.ExistsById(User.Id()))
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            if (!await houseService.CategoryExists(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var agentId = await agentService.GetAgentId(User.Id());

            int id = await houseService.Create(model, agentId);

            //Редиректваме към детайлите, само ако има добавено Id

            return RedirectToAction(nameof(Details), new { id = id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, this.User.Id())) == false)
            {

                return RedirectToPage("/Account/AccessDenied");
            }

            var house = await houseService.HouseDetailsById(id);
            var houseCategory = await houseService.GetHouseCategoryId(id);

            var model = new HouseModel()
            {
                Id = id,
                Address = house.Address,
                CategoryId = houseCategory,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                Title = house.Title,
                HouseCategories = await houseService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseModel model)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, this.User.Id())) == false)
            {

                return RedirectToPage("/Account/AccessDenied");
            }

            if ((await houseService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                model.HouseCategories = await houseService.AllCategories();

                return View(model);
            }
            await houseService.Edit(model.Id, model);

            return RedirectToAction(nameof(Details), new { id = model.Id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if ((await houseService.Exists(Id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(Id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied");
                //return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var house = await houseService.HouseDetailsById(Id);
            var model = new HouseDetailsViewModel()
            {
                Id = house.Id,
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            if ((await houseService.Exists(model.Id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(model.Id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied");
                //return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await houseService.Delete(model.Id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int Id)
        {
            if ((await houseService.Exists(Id)) == false)
            {
                return RedirectToAction(nameof(All));

            }

            if (await agentService.ExistsById(User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            if (await houseService.IsRented(Id))
            {
                return RedirectToAction(nameof(All));
            }

            await houseService.Rent(Id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if ((await houseService.IsRented(id)) == false 
                || (await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.IsRentedByUser(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            await houseService.Leave(id);
            return RedirectToAction(nameof(Mine));
        }
    }
}
