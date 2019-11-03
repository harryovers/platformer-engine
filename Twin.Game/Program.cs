using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twin.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new TwinSS();

            game.Run(1 / 60, 1 / 60);
        }
    }
}
