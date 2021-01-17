using System;
using System.IO;
using NumSharp;

namespace NumSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            float[,] arr = new float[2,2] {{1, 2}, {3, 4}};
            var a = np.array(new NDArray(arr));
            System.Console.WriteLine(a.ToString());
            var methodsList = typeof(NDArray).GetMethods();
            File.Delete("methods.txt");
            var outputMethods = File.AppendText("methods.txt");
            foreach (var item in methodsList)
            {
                outputMethods.WriteLine(item.ToString());
            }
            outputMethods.Close();
        }
    }
}
