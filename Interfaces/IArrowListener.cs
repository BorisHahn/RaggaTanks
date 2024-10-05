using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaggaTanks.interfaces
{
    public interface IArrowListener
    {
        void OnArrowUp();
        void OnArrowDown();
        void OnArrowLeft();
        void OnArrowRight();
    }
}
