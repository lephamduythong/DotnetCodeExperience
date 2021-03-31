using System;
using FluentValidation;
using FluentValidation.Results;

namespace FluentValidationTest
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public decimal Discount { get; set; }
        public string Address { get; set; }
    }

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Surname).NotNull();
            RuleFor(customer => customer.Address).MinimumLength(10);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer()
            {
                Id = Guid.NewGuid(),
                Surname = "Le",
                Forename = "Thong",
                Discount = 12,
                Address = "wtf"
            };

            CustomerValidator validator = new CustomerValidator();
            ValidationResult result = validator.Validate(customer);

            if (result.IsValid)
            {
                Console.WriteLine("OK");
            }
            else
            {
                System.Console.WriteLine("Error cmnr!");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(nameof(error.ErrorCode) + " : " + error.ErrorCode);
                    Console.WriteLine(nameof(error.ErrorMessage) + " : " + error.ErrorMessage);
                    Console.WriteLine(nameof(error.AttemptedValue) + " : " + error.AttemptedValue);
                    Console.WriteLine(nameof(error.CustomState) + " : " + error.CustomState);
                    Console.WriteLine(nameof(error.PropertyName) + " : " + error.PropertyName);
                    Console.WriteLine(nameof(error.Severity) + " : " + error.Severity);
                }
            }
        }
    }
}
