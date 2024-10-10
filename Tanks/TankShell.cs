using RaggaTanks.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public int Index {  get { return _index; } }
        public char CircleSymbol => circleSymbol;
        public Cell Body => _body;

        public TankShell(TankDir currentTankDirection, Cell currentTankPosition, TanksGameplayState state, int index)
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
                _body = nextCell;
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
    }
}
