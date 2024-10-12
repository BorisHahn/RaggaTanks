using RaggaTanks.shared;
using static RaggaTanks.Tanks.TanksGameplayState;

namespace RaggaTanks.Tanks
{
    public class Tank
    {
        private TanksGameplayState _gameplayState;
        private string _tankName;
        private Cell _tankPosition;
        public Cell TankPosition => _tankPosition;
        private List<TankShell> _tankShell = new();
        public List<TankShell> ListTankShell { get { return _tankShell; } }
        private TankDir _currentDir = TankDir.Left;
        public TankDir CurrentDir => _currentDir;
        private float _timeToMove = 0f;
        public bool IsPlayer { get; private set; }
        public int Health { get; private set; } = 250;

        public Tank(TanksGameplayState gameplayState, Cell startPosition, bool isPlayer, string tankName)
        {
            _gameplayState = gameplayState;
            IsPlayer = isPlayer;
            _tankPosition = startPosition;
            _tankName = tankName;
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
                if (IsPlayer)
                {
                    var filteredCoords = _gameplayState.Enemies.Where((el) => el.TankPosition.X == newPosition.X && el.TankPosition.Y == newPosition.Y).ToList();
                    if (filteredCoords.Count == 0)
                    {
                        _tankPosition = newPosition;
                    }
                }
                else
                {
                    var newPotentialBotPosition = newPosition.X == _gameplayState.PlayerTank.TankPosition.X && newPosition.Y == _gameplayState.PlayerTank.TankPosition.Y;
                    if (newPotentialBotPosition)
                    {
                        ChangeTankDirection();
                    }
                    else
                    {
                        _tankPosition = newPosition;
                    }
                }
            }
            else if (!IsPlayer)
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

            _timeToMove = IsPlayer ? (1f / 10): 1f;
            
            if (!IsPlayer)
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
                    var tankColor = (byte)(IsPlayer ? 4 : 2);
                    renderer.SetPixel(tx + _tankPosition.X, ty + _tankPosition.Y, tankModel[ty, tx], tankColor);
                }
            }
            if (_tankShell.Count > 0)
            {
                var tankShellModel = TankShellRenderView.GetRenderViewByDirection(_currentDir);
                foreach (var tankShell in _tankShell)
                {
                    renderer.SetPixel(tankShell.Body.X, tankShell.Body.Y, tankShell.CircleSymbol, 4);
                }
                /*foreach (var tankShell in _tankShell)
                {
                    for (int ty = 0; ty < 2; ty++)
                    {
                        for (int tx = 0; tx < 2; tx++)
                        {
                            renderer.SetPixel(tx + tankShell.Body.X, ty + tankShell.Body.Y, tankShellModel[ty, tx], 4);
                        }
                    }
                }*/
            }
        }

        public void GetDamage(TankShell shell)
        {
            if (!_gameplayState.gameOver && Health > 0)
            {
                Health -= shell.Damage;
                if (Health <= 0)
                {
                    _gameplayState.RemoveEnemyFromGameState(this);
                }
            }
        }
    }
}
