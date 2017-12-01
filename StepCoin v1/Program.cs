using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCoin_v1
{
    class Program
    {
        static void Main(string[] args)
        {
            Transaction N = new Transaction("Sender", "Beneficiar", 25.00m);
            Console.WriteLine(N.ToString());

        }
    }
}
