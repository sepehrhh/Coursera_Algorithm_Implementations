using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A12
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var problem = new Q2AddExitToMaze("");
            problem.Solve(4, new long[][] { new long[] { 1, 2 },
                new long[] { 3, 2 } });
        }
    }
}
