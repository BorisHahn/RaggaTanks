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
        private Tank? _playerTank;
        public Tank PlayerTank {  get => _playerTank; }
       
        public string[] currentMap;

        public static MapGenerator _mapGenerator;
        
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

        public void AddPlayerTankToGameState(Tank playerTank)
        {
            _playerTank = playerTank;
        }
        
        public override void Reset()
        {
            var middleY = fieldHeight / 2;
            var middleX = fieldWidth / 2;
            gameOver = false;
            hasWon = false;
        }

        public override void Update(float deltaTime)
        {
            _playerTank.Update(deltaTime);
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
            _playerTank.DrawTank(renderer);
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
