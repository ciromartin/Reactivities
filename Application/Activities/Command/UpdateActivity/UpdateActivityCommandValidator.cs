using FluentValidation;

namespace Application.Activities.Command.UpdateActivity
{
    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        public UpdateActivityCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();                
            RuleFor(x => x.Description).NotEmpty();                
            RuleFor(x => x.Date).NotEmpty();                
            RuleFor(x => x.Category).NotEmpty();                
            RuleFor(x => x.City).NotEmpty();                
            RuleFor(x => x.Venue).NotEmpty();                
        }    
    }
}