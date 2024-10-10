
namespace RaggaTanks.Tanks
{
    internal class TankRenderView
    {
        public static readonly char[,] Up = new char[,]
        {
            { ' ', '╔', '╩', '╗'},
            { ' ', '╚', '═', '╝'}
        };

        public static readonly char[,] Right = new char[,]
       {
            { '╔', '═', '╗', '_'},
            { '╚', '═', '╝', 'Т'}
       };

        public static readonly char[,] Down = new char[,]
       {
            { ' ', '╔', '═', '╗'},
            { ' ', '╚', '╦', '╝'}
       };

        public static readonly char[,] Left = new char[,]
       { 
            { '_', '╔', '═', '╗'},
            { 'Т', '╚', '═', '╝'}
       };

        public static char[,] GetRenderViewByDirection(TankDir direction)
        {
            switch (direction)
            {
                case TankDir.Up:
                    return Up;
                case TankDir.Down:
                    return Down;
                case TankDir.Left:
                    return Left;
                case TankDir.Right:
                    return Right;
                default:
                    return Left;
            }
        }
    }
}