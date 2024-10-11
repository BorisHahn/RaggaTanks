
namespace RaggaTanks.Tanks
{
    internal class TankShellRenderView
    {
        public static readonly char[,] Up = new char[,]
        {
            { ' ', 'o'},
            { ' ', ' '}
        };

        public static readonly char[,] Right = new char[,]
       {
            { 'o' , ' '},
            { ' ' , ' '}
       };

        public static readonly char[,] Down = new char[,]
       {
            { ' ' , 'o'},
            { ' ' , ' '}
       };

        public static readonly char[,] Left = new char[,]
       {
            { 'o' , ' '},
            { ' ' , ' '}
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