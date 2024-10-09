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
        private TankDir _currentDir = TankDir.Left;
        private Cell _tankPosition = new(4, 4);
        private List<TankShell> _tankShell = new();
        public List<TankShell> ListTankShell {  get { return _tankShell; } }
        public string[] currentMap;

        public static MapGenerator _mapGenerator;
        
        private float _timeToMove = 0f;
        
        public TankDir CurrentDir => _currentDir;
        public Cell TankPosition => _tankPosition;
        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }

        public bool gameOver;
        public bool hasWon = false;
        
        public int Level { get => _level; set { _level = value; } }
        private int _level = 1;
        
        public TanksGameplayState(MapGenerator mapGenerator)
        {
            _mapGenerator = mapGenerator;
            currentMap = mapGenerator.GetCurrentLevelMap($"level{_level}");
        }
        public void SetDirection(TankDir dir)
        {
            _currentDir = dir;
        }

        public void AddTankShellToList(TankShell tankShell)
        {
            _tankShell.Add(tankShell);
        }

        public void RemoveTankShellFromList(TankShell tankShell)
        {
            var newList = _tankShell.Where(t => t.Index != tankShell.Index).ToList();
            _tankShell = newList;
        }
        public void MoveByDirection()
        {
            var newPosition = ShiftTo(_tankPosition, _currentDir);
            _tankPosition = newPosition;
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
            _currentDir = TankDir.Left;
            _timeToMove = 0f;
            
            gameOver = false;
            hasWon = false;
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;
            if (_timeToMove > 0f || gameOver)
                return;

            _timeToMove = 1f / 10;

            if (_tankShell.Count == 0) return;
            if (_tankShell.Count > 0)
            {
                foreach (var tankShell in _tankShell)
                {
                    tankShell.Update(deltaTime);
                }
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

            var tankModel = TankRenderView.GetRenderViewByDirection(_currentDir);
            for (int ty = 0; ty < 2; ty++)
            {
                for (int tx = 0; tx < 4; tx++)
                {
                    renderer.SetPixel(tx + _tankPosition.X, ty + _tankPosition.Y, tankModel[ty,tx], 2);
                }
            }
            if (_tankShell.Count > 0)
            {
                foreach (var tankShell in _tankShell)
                {
                    renderer.SetPixel(tankShell.Body.X, tankShell.Body.Y, tankShell.CircleSymbol, 2);
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
