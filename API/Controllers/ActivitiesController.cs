using System;
using System.Threading.Tasks;
using Application.Activities.Command.CreateActivity;
using Application.Activities.Command.DeleteActivity;
using Application.Activities.Command.UpdateActivity;
using Application.Activities.Query.GetActivities;
using Application.Activities.Query.GetActivity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            return HandleResult(await Mediator.Send(new GetActivitiesQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetActivityQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(CreateActivityCommand command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(Guid id, UpdateActivityCommand command)
        {
            command.Id = id;
            return HandleResult(await Mediator.Send(command));            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteActivityCommand { Id = id }));
        }

    }
}