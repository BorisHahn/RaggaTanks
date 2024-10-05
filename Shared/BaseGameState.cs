using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_sSnake.shared
{
    public abstract class BaseGameState
    {
        public abstract void Update(float deltaTime);
        public abstract void Reset();
        public abstract void Draw(ConsoleRenderer renderer);
        public abstract bool IsDone();
    }
}
