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
            var mapValueByNextCell = _gameplayState.currentMap[newPosition.Y][newPosition.X];
            if (mapValueByNextCell == ' ')
            {
                _tankPosition = newPosition;
            } else if (!isPlayer)
            {
                ChangeTankDirection();
            }
        }

        private void ChangeTankDirection()
        {
            int currentIndexDirection = (int)CurrentDir;

            var tankDirMemberCount = Enum.GetNames(typeof(TankDir)).Length - 1;
            Random random = new Random();
            var newIndex = random.Next(0, 3);
            var tankDirValues = Enum.GetValues<TankDir>();
            for (int i = 0; i < tankDirValues.Length; i++)
            {
                if (i == newIndex)
                {
                    var value = (TankDir)i;
                    SetDirection(value);
                    return;
                }
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

            _timeToMove = isPlayer ? (1f / 10): 1f;

            if (!isPlayer)
            {
                MoveByDirection();
            }
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
                    var tankColor = (byte)(isPlayer ? 4 : 2);
                    renderer.SetPixel(tx + _tankPosition.X, ty + _tankPosition.Y, tankModel[ty, tx], tankColor);
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
