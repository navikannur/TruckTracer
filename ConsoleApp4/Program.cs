using System;

namespace NavigateSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool debug = false;
            int delay = 1000;

            //Delay
            if (args.Length > 1)
            {
                delay = Convert.ToInt32(args[1]);
            }

            //Debug mode
            if (args.Length > 2)
            {
                if (args[2].ToLower() == "y")
                    debug = true;
            }

            var navigate = new Navigation(args[0], delay, debug);

            
            navigate.Run();
        }
    }
}
