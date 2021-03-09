using FluentValidation;

namespace Application.Activities.Command.CreateActivity
{
    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator()
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