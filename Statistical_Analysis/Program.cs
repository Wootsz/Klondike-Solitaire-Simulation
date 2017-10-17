using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistical_Analysis
{
    class Program
    {
        static void Main(string[] args)
        {
            RNG random = new RNG(); 
            while (true)
            {
                long n = random.GetNumber();
                string binary_n = Convert.ToString(n, 2);
                Console.WriteLine(binary_n.Length + "  " + n + "  " + binary_n);
            }
            Console.ReadLine();
        }
    }
}
