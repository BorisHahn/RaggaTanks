
using RaggaTanks.map;
using RaggaTanks.Tanks;

namespace RaggaTanks.Levels
{
    public class LevelManager
    {
        public int currentLevel = 0;
        public List<Level> levels;
        public TanksGameplayState gameplayState;
        public MapGenerator mapGenerator;
        
        public LevelManager(List<Level> levels, TanksGameplayState gameplayState, MapGenerator mapGenerator)
        {
            this.levels = levels;
            this.gameplayState = gameplayState;
            this.mapGenerator = mapGenerator;
        }

        public void NextLevel()
        {
            if (currentLevel < levels.Count - 1)
            {
                currentLevel++;
            }
            else
            {
                ResetLevel();
            }
        }

        public void ResetLevel()
        {
            currentLevel = 0;
        }

        public void LoadLevel()
        {
            Level curLevel = levels[currentLevel];
            gameplayState.Level = currentLevel + 1;
            gameplayState.currentMap = mapGenerator.GetCurrentLevelMap(curLevel.name);
            gameplayState.AddPlayerToGameState(new Tank(gameplayState, curLevel.playerSpawn, true, "Boris"));
            gameplayState.Enemies.Clear();
            for (int i = 0; i < curLevel.enemySpawn.Count; i++)
            {
                gameplayState.AddEnemyToGameState(new Tank(gameplayState, curLevel.enemySpawn[i], false, $"Bot{i + 1}"));
            }
        }
    }
}
