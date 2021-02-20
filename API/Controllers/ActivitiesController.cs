using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<List<ActivityDto>>> GetActivities()
        {
            return await Mediator.Send(new GetActivitiesQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivity(Guid id)
        {
            return await Mediator.Send(new GetActivityQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult> CreateActivity(CreateActivityCommand command)
        {
            return Created(nameof(GetActivity), await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActivity(Guid id, UpdateActivityCommand command)
        {
            command.Id = id;
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity(Guid id)
        {
            await Mediator.Send(new DeleteActivityCommand { Id = id });
            return NoContent();
        }

    }
}