using System;

namespace Nelder_Mid
{
    internal class Program
    {
        static void Main()
        {
            Function f = new Function("(x[0]^2+x[1]-11)^2+(x[0]+x[1]^2-7)^2");
            Algorithm algorithm = new Algorithm(f);
            algorithm.algorithm_result();
        }
    }
}
