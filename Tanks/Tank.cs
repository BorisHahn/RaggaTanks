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
        public int Health { get; private set; } = 100;
        public DateTime LastShoot { get; private set; }
        private int _cdShoot = 2;


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
            var mapValueByNextCell = _gameplayState.GetMapValueByNextCell(newPosition);

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

        public bool CheckEnemyByCurrentDirection()
        {
            Cell step;
            switch (_currentDir)
            {
                case TankDir.Up:
                    step = new(0, -1);
                    break;
                case TankDir.Right:
                    step = new(1, 0);
                    break;
                case TankDir.Down:
                    step = new(0, 1);
                    break;
                case TankDir.Left:
                    step = new(-1, 0);
                    break;
                default:
                    return false;
            }
            var pos = _tankPosition;
            do
            {
                pos.X += step.X;
                pos.Y += step.Y;
                var element = _gameplayState.GetMapValueByNextCell(pos);

                if (element == null || element == '▓' || element == '░')
                    return false;
                if (_gameplayState.PlayerTank.TankPosition.X == pos.X &&
                   _gameplayState.PlayerTank.TankPosition.Y == pos.Y)
                    return true;
            }
            while (true);
        }

        public void Shoot()
        {
            if (DateTime.Now.Ticks - LastShoot.Ticks < TimeSpan.FromSeconds(_cdShoot).Ticks) return;
            Random rnd = new Random();
            int index = rnd.Next(100);
            var guid = System.Guid.NewGuid();            
            TankShell tankShell = new(_currentDir, _tankPosition, _gameplayState, guid, this);
            AddTankShellToList(tankShell);
            LastShoot = DateTime.Now;
            Console.Beep();
        }

        private void ChangeTankDirection()
        {            
            var tankDirMemberCount = Enum.GetNames(typeof(TankDir)).Length;
            Random random = new Random();
            var newIndex = random.Next(0, tankDirMemberCount);
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

            if (_gameplayState.gameOver)
                return;
            foreach (var tankShell in _tankShell)
            {
                tankShell.Update(deltaTime);
            }
            if (!IsPlayer)
            {
                _timeToMove -= deltaTime;
                if (_timeToMove > 0f)
                    return;

                _timeToMove = 1f;

                if (CheckEnemyByCurrentDirection())                
                    Shoot();                
                else
                    MoveByDirection();                
            }                       
        }

        public void DrawTank(ConsoleRenderer renderer)
        {
            var tankColor = (byte)(IsPlayer ? 4 : 2);
            var tankModel = TankRenderView.GetRenderViewByDirection(_currentDir);
            for (int ty = 0; ty < 2; ty++)
            {
                for (int tx = 0; tx < 4; tx++)
                {                    
                    renderer.SetPixel(tx + _tankPosition.X, ty + _tankPosition.Y, tankModel[ty, tx], tankColor);
                }
            }
            if (_tankShell.Count > 0)
            {                
                foreach (var tankShell in _tankShell)
                {
                    renderer.SetPixel(tankShell.Body.X, tankShell.Body.Y, tankShell.CircleSymbol, tankColor);
                }               
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
