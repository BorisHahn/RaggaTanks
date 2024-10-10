using RaggaTanks.map;
using static RaggaTanks.Tanks.TanksGameplayState;

namespace RaggaTanks.Tanks
{
    public class TankShell
    {
        const char circleSymbol = 'o';
        private Cell _body;
        private TankDir _currentShellDir;
        private TanksGameplayState _state;
        private int _index;
        private Tank _tankOwner;
        public int Index {  get { return _index; } }
        public char CircleSymbol => circleSymbol;
        public Cell Body => _body;
        public int Damage { get; private set; } = 50;

        public TankShell(TankDir currentTankDirection, Cell currentTankPosition, TanksGameplayState state, int index, Tank tankOwner)
        {
            _currentShellDir = currentTankDirection;
            switch (currentTankDirection)
            {
                case TankDir.Left:
                    _body = new(currentTankPosition.X - 4, currentTankPosition.Y);
                    break;
                case TankDir.Right:
                    _body = new(currentTankPosition.X + 4, currentTankPosition.Y);
                    break;
                case TankDir.Up:
                    _body = new(currentTankPosition.X, currentTankPosition.Y - 2);
                    break;
                case TankDir.Down:
                    _body = new(currentTankPosition.X, currentTankPosition.Y + 2);
                    break;
            }
            _state = state;
            _index = index;
            _tankOwner = tankOwner;
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

        public void Update(float dt)
        {
            var nextCell = ShiftTo(_body, _currentShellDir);
            MapGenerator mapGenerator = new MapGenerator();

            var mapValueByNextCell = _state.currentMap[nextCell.Y][nextCell.X];
            var test = mapValueByNextCell;
            if (mapValueByNextCell == ' ' || mapValueByNextCell == '█')
            {
                var filteredCoords = _state.Enemies.Where((el) => el.TankPosition.X == nextCell.X && el.TankPosition.Y == nextCell.Y).ToList();
                if (filteredCoords.Count > 0)
                {
                    CauseDamageToTank(filteredCoords);
                    _state.PlayerTank.RemoveTankShellFromList(this);
                }
                else
                {
                    _body = nextCell;
                }
            }
            else if (mapValueByNextCell == '▓')
            {
                if (_currentShellDir == TankDir.Right || _currentShellDir == TankDir.Down || _currentShellDir == TankDir.Up)
                {
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X, 1).Insert(nextCell.X, "░");
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X + 1, 1).Insert(nextCell.X + 1, "░");
                } else if (_currentShellDir == TankDir.Left)
                {
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X, 1).Insert(nextCell.X, "░");
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X - 1, 1).Insert(nextCell.X - 1, "░");
                } 
                _state.PlayerTank.RemoveTankShellFromList(this);
            }
            else if (mapValueByNextCell == '░')
            {
                if (_currentShellDir == TankDir.Right || _currentShellDir == TankDir.Down || _currentShellDir == TankDir.Up)
                {
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X, 1).Insert(nextCell.X, " ");
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X + 1, 1).Insert(nextCell.X + 1, " ");
                }
                else if (_currentShellDir == TankDir.Left)
                {
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X, 1).Insert(nextCell.X, " ");
                    _state.currentMap[nextCell.Y] = _state.currentMap[nextCell.Y].Remove(nextCell.X - 1, 1).Insert(nextCell.X - 1, " ");
                }               
                _state.PlayerTank.RemoveTankShellFromList(this);
            }
            else
            {
                _state.PlayerTank.RemoveTankShellFromList(this);
            }
        }

        private void CauseDamageToTank(List<Tank> filteredCoords)
        {
            if (_tankOwner.IsPlayer)
            {
               filteredCoords[0].GetDamage(this);
            }
        }
    }
}
