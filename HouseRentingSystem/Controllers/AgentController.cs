using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentService agentService;
        public AgentController(IAgentService _agentService)
        {
            agentService = _agentService;
        }
        [HttpGet]
        public async Task<IActionResult> Become()
        {
            //we are getting the Users Id from the method we wrote - User.Id
            //Then from the service we check if he is already an agent;
            if (await agentService.ExistsById(User.Id()))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeAgentModel model)
        {
            //връща към първата страница
            return RedirectToAction("All", "House");
        }
    }
}
