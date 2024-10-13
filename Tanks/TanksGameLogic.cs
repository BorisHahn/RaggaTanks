using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaggaTanks.Levels;
using RaggaTanks.map;
using RaggaTanks.shared;
using static RaggaTanks.Tanks.TanksGameplayState;

namespace RaggaTanks.Tanks
{
    public class TanksGameLogic : BaseGameLogic
    {
        private TanksGameplayState gameplayState;
        private bool newGamePending = false;
        private ShowTextState showTextState = new(2f);
        private LevelManager _levelManager;
        public TanksGameLogic(MapGenerator mapGenerator)
        {
            gameplayState = new TanksGameplayState(showTextState);

            List<Level> levels = [
                new("level1", new Cell(4, 4), [new (44, 4)]),
                new("level2", new Cell(4, 4), [new (44, 4), new (4, 20)]),
                new("level3", new Cell(4, 4), [new (44, 4), new (4, 20), new (44, 20)]),
                new("level4", new Cell(4, 4), [new (44, 4), new (4, 20), new (44, 20), new(16, 6)]),                
            ];
            _levelManager = new(levels, gameplayState, mapGenerator);
            _levelManager.LoadLevel();
        }

        public void ChangeLiveGameСycle(TanksGameplayState gameplayState)
        {
            if (gameplayState.Enemies.Count == 0 && !gameplayState.gameOver && !gameplayState.hasWon)
            {
                _levelManager.NextLevel();
                _levelManager.LoadLevel();
            }
        }

        public void CheckGameOver()
        {
            if (gameplayState.PlayerTank.Health <= 0)
            {
                gameplayState.gameOver = true;
            }
            else if (gameplayState.Level == 4 && gameplayState.Enemies.Count == 0)
            {
                gameplayState.hasWon = true;
            }
        }

        public override void OnArrowUp()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.PlayerTank.SetDirection(TankDir.Up);
            gameplayState.PlayerTank.MoveByDirection();
        }

        public override void OnArrowDown()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.PlayerTank.SetDirection(TankDir.Down);
            gameplayState.PlayerTank.MoveByDirection();
        }

        public override void OnArrowLeft()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.PlayerTank.SetDirection(TankDir.Left);
            gameplayState.PlayerTank.MoveByDirection();
        }

        public override void OnArrowRight()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.PlayerTank.SetDirection(TankDir.Right);
            gameplayState.PlayerTank.MoveByDirection();
        }

        public override void OnPressR()
        {
            RestartGame();
        }

        public override void OnPressSpace()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.PlayerTank.Shoot();
        }

        public void RestartGame()
        {
            Console.Clear();
            ChangeState(gameplayState);
            _levelManager.ResetLevel();
            _levelManager.LoadLevel();
            gameplayState.Reset();
        }
        public void GotoGameplay()
        {
            ChangeState(gameplayState);
            gameplayState.Reset();
        }

        public override ConsoleColor[] CreatePallet()
        {
            return [
                ConsoleColor.Red,
                ConsoleColor.Black,
                ConsoleColor.Gray,
                ConsoleColor.Blue,
                ConsoleColor.Green,
                ConsoleColor.DarkYellow,
                ConsoleColor.Magenta,
                ];
        }

        public void GoToGameOver()
        {
            if (CurrentState != showTextState)
            {
                Console.Clear();
                ChangeState(showTextState);
                var message = gameplayState.hasWon ? "прошли игру" : "проиграли";
                var color = gameplayState.hasWon ? ConsoleColor.Green : ConsoleColor.Red;
                 showTextState.Color = color;
                 showTextState.Text = $"Вы {message}! Нажмите R - для рестарта";
            }
        }

        public override void Update(float deltaTime)
        {
            if (CurrentState != null && !gameplayState.gameOver || !gameplayState.hasWon)
            {
                CheckGameOver();
                ChangeLiveGameСycle(gameplayState);
            }      
            if (CurrentState == gameplayState && gameplayState.gameOver || gameplayState.hasWon)
            {
                GoToGameOver();
            }           
            if (CurrentState == null && !gameplayState.gameOver && !gameplayState.hasWon)
            {
                ChangeState(gameplayState);
            }
        }
    }
}
