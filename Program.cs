using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using WLP.Models;
namespace WLP
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToLogfile = null;
            if(args.Length > 0)
            {
                pathToLogfile = args[0];
            }

            Monitor = new IntrusionMonitor(pathToLogfile);




            Console.ReadKey();



        }


        public static IntrusionMonitor Monitor;


       



    }
}
