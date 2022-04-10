#nullable disable

namespace ServiceLocatorTest
{
    public interface IOperatingSystem
    {
        string Name { get; set; }
        void Run();
    }

    public class Windows : IOperatingSystem
    {
        public Windows()
        {
            Name = "Windows 10";
        }

        public string Name { get; set; }

        public void Run()
        {
            Console.WriteLine(Name + " is running.");
        }
    }

    public class Computer
    {
        // Dependency Injection
        private readonly IOperatingSystem _operatingSystem;

        public Computer()
        {
            _operatingSystem = ServiceLocator.Instance.GetService<IOperatingSystem>();
        }

        public void Start()
        {
            _operatingSystem.Run();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine(typeof(ServiceLocator));
            
            ServiceLocator.Instance.Register<IOperatingSystem>(new Windows());

            var computer = new Computer();
            computer.Start();

            Console.Read();
        }
    }
}