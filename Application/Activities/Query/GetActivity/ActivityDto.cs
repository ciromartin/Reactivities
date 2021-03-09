using System;
using Application.Common.Mappings;
using Domain;

namespace Application.Activities.Query.GetActivity
{
    public class ActivityDto : IMapFrom<Activity>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
    }
}