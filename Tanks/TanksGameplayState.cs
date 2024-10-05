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
        //const char snakeSymbol = '■';
        //const char circleSymbol = '0';

        private static MapGenerator _mapGenerator;
        
        private TankDir currentDir = TankDir.Left;
        private float _timeToMove = 0f;
        private List<Cell> _body = new();

        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }

        public bool gameOver;
        public bool hasWon = false;
        public int level;
        
        public TanksGameplayState(MapGenerator mapGenerator)
        {
            _mapGenerator = mapGenerator;
        }
        public void SetDirection(TankDir dir)
        {
            currentDir = dir;
        }

        private Cell ShiftTo(Cell curCell, TankDir dir)
        {
            switch (dir)
            {
                case TankDir.Up:
                    return new Cell(curCell.X, curCell.Y - 1);
                case TankDir.Down:
                    return new Cell(curCell.X, curCell.Y + 1);
                case TankDir.Left:
                    return new Cell(curCell.X - 1, curCell.Y);
                case TankDir.Right:
                    return new Cell(curCell.X + 1, curCell.Y);
            }
            return curCell;
        }
        public override void Reset()
        {
            _body.Clear();
            var middleY = fieldHeight / 2;
            var middleX = fieldWidth / 2;
            currentDir = TankDir.Left;
            _body.Add(new(middleX + 2, middleY));
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
                    } else
                    {
                        renderer.SetPixel(x, y, currentMap[y][x], 3);
                    }
                }
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
