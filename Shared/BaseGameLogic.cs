using RaggaTanks.interfaces;
using RaggaTanks.shared;

namespace RaggaTanks.shared
{
    public abstract class BaseGameLogic: IArrowListener
    {
        protected BaseGameState? CurrentState { get; private set; }
        protected float Time { get; private set; }
        protected int ScreenWidth { get; private set; }
        protected int ScreenHeight { get; private set; }
        public abstract void OnArrowDown();

        public abstract void OnArrowLeft();

        public abstract void OnArrowRight();

        public abstract void OnArrowUp();
        public abstract ConsoleColor[] CreatePallet();

        public void InitializeInput(ConsoleInput consoleInput)
        {
            consoleInput.Subscribe(this);
        }

        public void ChangeState(BaseGameState? baseGameState)
        {
            CurrentState?.Reset();      
            CurrentState = baseGameState;
        }

        public void DrawNewState(float deltaTime, ConsoleRenderer renderer)
        {
            Time += deltaTime;
            ScreenHeight = renderer.Height;
            ScreenWidth = renderer.Width;

            CurrentState?.Update(deltaTime);

            CurrentState?.Draw(renderer);

            Update(deltaTime);
        }

        public abstract void Update(float deltaTime);
    }
}
