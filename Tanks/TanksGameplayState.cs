using RaggaTanks.Levels;
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
        public int Level { get; set; }
       
        public string[] currentMap;

        private ShowTextState _showTextState;
        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }

        public bool gameOver = false;
        public bool hasWon = false;

        
        public TanksGameplayState(ShowTextState showTextState)
        {
            _showTextState = showTextState;
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

        public char? GetMapValueByNextCell(Cell position)
        {
            if (position.X >= currentMap[0].Length || position.Y >= currentMap.Length || position.X < 0 || position.Y < 0) return null;
            return currentMap[position.Y][position.X];
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
            ShowGameStateInfo(renderer);
        }

        public void ShowGameStateInfo(ConsoleRenderer renderer)
        {
            _showTextState.DrawText(renderer, 0, 60, ConsoleColor.Green, $"Player: {PlayerTank.Health}HP ");
            _showTextState.DrawText(renderer, 1, 60, ConsoleColor.DarkYellow, $"Level: {Level}");
            _showTextState.DrawText(renderer, 2, 60, ConsoleColor.Magenta, $"Enemies: {Enemies.Count}");
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
