using RaggaTanks.map;
using RaggaTanks.shared;
using RaggaTanks.Tanks;

namespace RaggaTanks
{
    internal class Program
    {
        const float targetFrameTime = 1f / 60f;

        static void Main()
        {
            MapGenerator mapGenerator = new MapGenerator();
            TanksGameLogic gameLogic = new TanksGameLogic(mapGenerator);

            var pallete = gameLogic.CreatePallet();

            ConsoleRenderer renderer0 = new ConsoleRenderer(pallete);
            ConsoleRenderer renderer1 = new ConsoleRenderer(pallete);

            ConsoleInput input = new ConsoleInput();
            gameLogic.InitializeInput(input);

            ConsoleRenderer prevRenderer = renderer0;
            ConsoleRenderer currRenderer = renderer1;
            DateTime lastFrameTime = DateTime.Now;

            while (true)
            {
                DateTime frameStartTime = DateTime.Now;
                var deltaTime = (float)(frameStartTime - lastFrameTime).TotalSeconds;

                input.Update();

                gameLogic.DrawNewState(deltaTime, currRenderer);
                lastFrameTime = frameStartTime;

                if (!currRenderer.Equals(prevRenderer)) currRenderer.Render();

                ConsoleRenderer tmp = prevRenderer;
                prevRenderer = currRenderer;
                currRenderer = tmp;
                currRenderer.Clear();

                var nextFrameTime = frameStartTime + TimeSpan.FromSeconds(targetFrameTime);
                var endFrameTime = DateTime.Now;
                if (nextFrameTime > endFrameTime)
                {
                    Thread.Sleep((int)(nextFrameTime - endFrameTime).TotalMilliseconds);
                }
            }
        }
        }
    }
