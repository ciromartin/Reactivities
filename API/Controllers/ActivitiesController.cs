using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Application.Activities.Command.CreateActivity;
using Application.Activities.Query.GetAcitivities;
using Application.Activities.Query.GetActivity;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _context;

        public ActivitiesController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await Mediator.Send(new GetActivitiesQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await Mediator.Send(new GetActivityQuery{ Id = id});
        }

        [HttpPost]
        public async Task<ActionResult> CreateActivity(Activity activity)
        {
            return Ok(await Mediator.Send(new CreateActivityCommand{ Activity = activity}));
        }

    }
}