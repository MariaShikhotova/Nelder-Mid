using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nelder_Mid;
using System;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void F()
        {
            Function f = new Function("x[0]+x[1]*4+1");
            double[] x = { 0, 0 };
            Assert.AreEqual(f.F(x), 1);
        }
        [TestMethod]
        public void Rosenbrock()
        {
            Function f = new Function("(1-x[0])^2+100*(x[1]-x[0]^2)^2");
            Algorithm algorithm = new Algorithm(f);
            double[] res = algorithm.algorithm_result();
            for (int i = 0; i < res.Length; i++)
            {
                Math.Round(res[i]);
            }
            double[] result = { 1, 1 };
            bool flag = true;
            for (int i = 0;i < res.Length;i++)
            {
                if (res[i] != result[i])
                    flag = false;
            }
        }
        [TestMethod]
        public void Himmelblau()
        {
            Function f = new Function("(x[0]^2+x[1]-11)^2+(x[0]+x[1]^2-7)^2");
            Algorithm algorithm = new Algorithm(f);
            double[] res = algorithm.algorithm_result();
            for (int i = 0; i < res.Length; i++)
            {
                Math.Round(res[i]);
            }
            double[] result = { 3, 2 };
            bool flag = true;
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] != result[i])
                    flag = false;
            }
        }
    }
}
