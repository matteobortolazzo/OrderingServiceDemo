using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Ordering.App.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.UserId).Must(BePositive);         
            RuleFor(command => command.ProductId).Must(BePositive); 
            RuleFor(command => command.Quantity).Must(BePositive);

            RuleFor(command => command.City).NotEmpty();
            RuleFor(command => command.Street).NotEmpty();
            RuleFor(command => command.State).NotEmpty();
            RuleFor(command => command.Country).NotEmpty();
            RuleFor(command => command.ZipCode).NotEmpty();
        }

        private bool BePositive(int n)
        {
            return n > 0;
        }
    }
}
