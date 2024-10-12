using RaggaTanks.map;
using RaggaTanks.shared;
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
                    _body = new(currentTankPosition.X, currentTankPosition.Y);
                    break;
                case TankDir.Right:
                    _body = new(currentTankPosition.X + 3, currentTankPosition.Y);
                    break;
                case TankDir.Up:
                    _body = new(currentTankPosition.X, currentTankPosition.Y);
                    break;
                case TankDir.Down:
                    _body = new(currentTankPosition.X, currentTankPosition.Y + 1);
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
                EditWall("░", nextCell);
            }
            else if (mapValueByNextCell == '░')
            {
                EditWall(" ", nextCell);                
            }
            else
            {
                _state.PlayerTank.RemoveTankShellFromList(this);
            }
        }

        public void EditWall(string value, Cell nextCell)
        {            
            for (var y = 0; y < 2; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    var nextY = _currentShellDir == TankDir.Up ? nextCell.Y - y : nextCell.Y + y;
                    var nextX = _currentShellDir == TankDir.Left ? nextCell.X - x : nextCell.X + x;
                    _state.currentMap[nextY] = _state.currentMap[nextY].Remove(nextX, 1).Insert(nextX, value);
                }
            }                                
            _state.PlayerTank.RemoveTankShellFromList(this);
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
