using System;
using AutoMapper;

namespace AutoMapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // config with "null" condition exclude
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ViewModel, Model>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            var mapper = configuration.CreateMapper();

            // map ViewModel object to Model object
            var viewModel = new ViewModel() 
            {
                Id = Guid.NewGuid(),
                Name = "Thong",
                Description = null,
                AdvancedInfo = "CU LAC GION TAN"
            };
            var model = new Model()
            {
                Description = "ABCXYZ"
            };
            mapper.Map(viewModel, model);

            // print mapped model's information, regard the 'Description' value
            Console.WriteLine(model.Id.ToString());
            Console.WriteLine(model.Name);
            Console.WriteLine(model.Description);
        }
    }
}
