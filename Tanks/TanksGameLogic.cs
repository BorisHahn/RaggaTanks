﻿using System;
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
        /*private void GotoNextLevel()
        {

            newGamePending = false;
            ChangeState(gameplayState);
            showTextState.Text = $"Level {currentLevel}";
        }*/

        public void ChangeLiveGameСycle(TanksGameplayState gameplayState)
        {
            if (gameplayState.Enemies.Count == 0)
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

        public override void OnPressN()
        {
            _levelManager.NextLevel();
            _levelManager.LoadLevel();
        }

        public override void OnPressSpace()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
           gameplayState.PlayerTank.Shoot();
        }

        public void GotoGameplay()
        {
            gameplayState.Level = _levelManager.currentLevel;
            gameplayState.fieldWidth = ScreenWidth;
            gameplayState.fieldHeight = ScreenHeight;
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
            _levelManager.ResetLevel();
            _levelManager.LoadLevel();
            gameplayState.gameOver = false;           
        }

        public override void Update(float deltaTime)
        {
            if (CurrentState != null && !gameplayState.gameOver)
            {
                ChangeLiveGameСycle(gameplayState);
                CheckGameOver();
            }
            if (CurrentState == null || CurrentState == gameplayState && !gameplayState.gameOver)
            {
                ChangeState(gameplayState);
            }
            else if (CurrentState == gameplayState && gameplayState.gameOver)
            {
                GoToGameOver();
            }           
        }
    }
}
