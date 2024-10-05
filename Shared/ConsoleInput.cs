using RaggaTanks.interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_sSnake.shared
{
    public class ConsoleInput
    {
        public HashSet<IArrowListener> arrowListeners = [];

        public void Subscribe(IArrowListener iArrowListener)
        {
            arrowListeners.Add(iArrowListener);
        }

        public void Update()
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow or ConsoleKey.W:
                        foreach (var aL in arrowListeners) aL.OnArrowUp();
                        break;
                    case ConsoleKey.DownArrow or ConsoleKey.S:
                        foreach (var aL in arrowListeners) aL.OnArrowDown();
                        break;
                    case ConsoleKey.LeftArrow or ConsoleKey.A:
                        foreach (var aL in arrowListeners) aL.OnArrowLeft();
                        break;
                    case ConsoleKey.RightArrow or ConsoleKey.D:
                        foreach (var aL in arrowListeners) aL.OnArrowRight();
                        break;
                }
            }
        }
    }
}
