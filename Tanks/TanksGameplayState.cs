using RaggaTanks.map;
using RaggaTanks.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace RaggaTanks.Tanks
{
    public enum TankDir
    {
        Up, Down, Left, Right
    }
    public class TanksGameplayState : BaseGameState
    {
        private TankDir currentDir = TankDir.Left;
        private Cell tankPosition = new(4, 4);
        
        private string[] _currentMap;

        private static MapGenerator _mapGenerator;
        
        private float _timeToMove = 0f;

        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }

        public bool gameOver;
        public bool hasWon = false;
        public int level;
        
        public TanksGameplayState(MapGenerator mapGenerator)
        {
            _mapGenerator = mapGenerator;
            _currentMap = mapGenerator.GetCurrentLevelMap("level1");
        }
        public void SetDirection(TankDir dir)
        {
            currentDir = dir;
        }

        public void MoveByDirection()
        {
            var newPosition = ShiftTo(tankPosition, currentDir);
            tankPosition = newPosition;
        }

        private Cell ShiftTo(Cell curCell, TankDir dir)
        {
            switch (dir)
            {
                case TankDir.Up:
                    return new Cell(curCell.X, curCell.Y - 2);
                case TankDir.Down:
                    return new Cell(curCell.X, curCell.Y + 2);
                case TankDir.Left:
                    return new Cell(curCell.X - 4, curCell.Y);
                case TankDir.Right:
                    return new Cell(curCell.X + 4, curCell.Y);
            }
            return curCell;
        }
        public override void Reset()
        {
            var middleY = fieldHeight / 2;
            var middleX = fieldWidth / 2;
            currentDir = TankDir.Left;
            _timeToMove = 0f;
            
            gameOver = false;
            hasWon = false;
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;
            if (_timeToMove > 0f || gameOver)
                return;

            _timeToMove = 1f / 4;
        }

        public override void Draw(ConsoleRenderer renderer)
        {
            string[] currentMap = _mapGenerator.GetCurrentLevelMap($"level{level - 1}");

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
                        renderer.SetPixel(x, y, currentMap[y][x], 2);
                    }
                    else
                    {
                        renderer.SetPixel(x, y, currentMap[y][x], 3);
                    }
                }
            }

            var tankModel = TankRenderView.GetRenderViewByDirection(currentDir);
            for (int ty = 0; ty < 2; ty++)
            {
                for (int tx = 0; tx < 4; tx++)
                {
                    renderer.SetPixel(tx + tankPosition.X, ty + tankPosition.Y, tankModel[ty,tx], 2);
                }
            }
            //renderer.SetPixel();
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
