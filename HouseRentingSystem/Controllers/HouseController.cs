using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Extensions;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Extensions;
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

            if (await agentService.ExistsByIdAsync(userId))
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

            //if (information != model.GetInformation())
            //{
            //    TempData["ErrorMessage"] = "Don't touch my slug!";

            //    return RedirectToAction("Index", "Home");
            //}

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //If not an agent, Return to redirect
            if (!(await agentService.ExistsByIdAsync(User.Id())))
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
            if (!await agentService.ExistsByIdAsync(User.Id()))
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

            int newHouseId = await houseService.Create(model, agentId);

            //Редиректваме към детайлите, само ако има добавено Id

            return RedirectToAction(nameof(Details), new { newHouseId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new HouseModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseModel model)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            return RedirectToAction(nameof(Mine));
        }
    }
}
