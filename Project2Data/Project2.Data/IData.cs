using Project2.Models.Actor;

namespace Project2.Data {
    public interface IData {
        public GameActor? GetEnemy(GameActor pEnemy);
        public Dictionary<string, GameActor> GetAllEnemies();
        
        public GameActor? CreateEnemy(GameActor pEnemy);
        public Dictionary<string, GameActor> CreateAllEnemies(Dictionary<string, GameActor> pEnemies);
    }
}