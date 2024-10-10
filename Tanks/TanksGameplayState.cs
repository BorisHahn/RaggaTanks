using RaggaTanks.map;
using RaggaTanks.shared;

namespace RaggaTanks.Tanks
{
    public enum TankDir
    {
        Up, Right, Down, Left
    }
    public class TanksGameplayState : BaseGameState
    {
        public Tank PlayerTank { get; private set; }
        public List<Tank> Enemies { get; set; } = [];
        public int Level { get; set; } = 1;
       
        public string[] currentMap;

        public static MapGenerator _mapGenerator;
        
        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }

        public bool gameOver = false;
        public bool hasWon = false;

        
        public TanksGameplayState(MapGenerator mapGenerator)
        {
            _mapGenerator = mapGenerator;
            currentMap = mapGenerator.GetCurrentLevelMap($"level{Level}");
        }

        public void AddPlayerToGameState(Tank playerTank)
        {
            PlayerTank = playerTank;
        }

        public void AddEnemyToGameState(Tank enemy)
        {
            Enemies.Add(enemy);
        }
        
        public void RemoveEnemyFromGameState(Tank enemy)
        {
            Enemies.Remove(enemy);
        }
        public override void Reset()
        {
            var middleY = fieldHeight / 2;
            var middleX = fieldWidth / 2;
            gameOver = false;
            hasWon = false;
        }

        public override void Update(float deltaTime)
        {
            PlayerTank.Update(deltaTime);
            foreach (var enemy in Enemies)
            {
                enemy.Update(deltaTime);
            }

        }

        public override void Draw(ConsoleRenderer renderer)
        {        
            for (int y = 0; y < Math.Min(currentMap.Length, renderer.Height); y++)
            {
                for (int x = 0; x < Math.Min(currentMap[0].Length, renderer.Width); x++)
                {
                    if (currentMap[y][x] == '▓')
                    {
                        renderer.SetPixel(x, y, currentMap[y][x], 0);
                    }
                    else if (currentMap[y][x] == '░')
                    {
                        renderer.SetPixel(x, y, currentMap[y][x], 0);
                    }
                    else
                    {
                        renderer.SetPixel(x, y, currentMap[y][x], 3);
                    }
                }
            }
            PlayerTank.DrawTank(renderer);

            foreach (var enemy in Enemies)
            {
                enemy.DrawTank(renderer);
            }
        }

        public override bool IsDone()
        {
            return gameOver || hasWon;
        }

        public struct Cell
        {
            public int X;
            public int Y;

            public Cell(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
