using RaggaTanks.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RaggaTanks.Tanks.TanksGameplayState;

namespace RaggaTanks.Tanks
{
    public class Tank
    {
        private TanksGameplayState _gameplayState;
        private Cell _tankPosition;
        public Cell TankPosition => _tankPosition;
        private List<TankShell> _tankShell = new();
        public List<TankShell> ListTankShell { get { return _tankShell; } }
        private TankDir _currentDir = TankDir.Left;
        public TankDir CurrentDir => _currentDir;
        private float _timeToMove = 0f;
        private bool isPlayer;

        public Tank(TanksGameplayState gameplayState, Cell startPosition, bool isPlayer)
        {
            _gameplayState = gameplayState;
            this.isPlayer = isPlayer;
            _tankPosition = startPosition;
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
            var mapValueByNextCell = _mapGenerator.GetCurrentCharByCoords(newPosition.X, newPosition.Y, $"level{_gameplayState.Level}");
            if (mapValueByNextCell == ' ')
            {
                _tankPosition = newPosition;
            }
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

        public void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;
            if (_timeToMove > 0f || _gameplayState.gameOver)
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

        public void DrawTank(ConsoleRenderer renderer)
        {
            var tankModel = TankRenderView.GetRenderViewByDirection(_currentDir);
            for (int ty = 0; ty < 2; ty++)
            {
                for (int tx = 0; tx < 4; tx++)
                {
                    renderer.SetPixel(tx + _tankPosition.X, ty + _tankPosition.Y, tankModel[ty, tx], 2);
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
    }
}
