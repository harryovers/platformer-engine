using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Platformer();

            game.Run(1 / 120, 1 / 60);
        }
    }
}
