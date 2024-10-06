using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaggaTanks.map;
using RaggaTanks.shared;

namespace RaggaTanks.Tanks
{
    public class TanksGameLogic : BaseGameLogic
    {
        private TanksGameplayState gameplayState;
        private bool newGamePending = false;
        private int currentLevel = 1;
        private ShowTextState showTextState = new(2f);
        
        public TanksGameLogic(MapGenerator mapGenerator)
        {
            gameplayState = new TanksGameplayState(mapGenerator);
        }
        private void GotoNextLevel()
        {
            currentLevel++;
            newGamePending = false;
            showTextState.Text = $"Level {currentLevel}";
            ChangeState(showTextState);
        }

        public override void OnArrowUp()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.SetDirection(TankDir.Up);
            gameplayState.MoveByDirection();
        }

        public override void OnArrowDown()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.SetDirection(TankDir.Down);
            gameplayState.MoveByDirection();
        }

        public override void OnArrowLeft()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }
            gameplayState.SetDirection(TankDir.Left);
            gameplayState.MoveByDirection();
        }

        public override void OnArrowRight()
        {
            if (CurrentState != gameplayState)
            {
                return;
            }   
            gameplayState.SetDirection(TankDir.Right);
            gameplayState.MoveByDirection();
        }

        public void GotoGameplay()
        {
            gameplayState.level = currentLevel;
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
                ConsoleColor.Blue
                ];
        }

        public void GoToGameOver()
        {
            currentLevel = 0;
            newGamePending = true;
            showTextState.Text = $"Потрачено!";
            ChangeState(showTextState);
        }

        public override void Update(float deltaTime)
        {
            if (CurrentState != null && !CurrentState.IsDone())
            {
                return;
            }
            if (CurrentState == null || CurrentState == gameplayState && !gameplayState.gameOver)
            {
                GotoNextLevel();
            }
            else if (CurrentState == gameplayState && gameplayState.gameOver)
            {
                GoToGameOver();
            }
            else if (CurrentState != gameplayState && newGamePending)
            {
                GotoNextLevel();
            }
            else if (CurrentState != gameplayState && !newGamePending)
            {
                GotoGameplay();
            }
            
        }
    }
}
