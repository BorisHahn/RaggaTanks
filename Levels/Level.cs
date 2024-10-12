using RaggaTanks.Tanks;

namespace RaggaTanks.Levels
{
    public class Level
    {
        public string name;
        public TanksGameplayState.Cell playerSpawn;
        public List<TanksGameplayState.Cell> enemySpawn;

        public Level(string name, TanksGameplayState.Cell playerSpawn, List<TanksGameplayState.Cell> enemySpawn)
        {
            this.name = name;            
            this.playerSpawn = playerSpawn;
            this.enemySpawn = enemySpawn;
        }
    }
}
