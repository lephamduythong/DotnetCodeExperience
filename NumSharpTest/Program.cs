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
            
        }
    }
}
