using Project2.Models.Actor;

namespace Project2.Data {
    public interface IData {
        public ActorEnemy? GetEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> GetAllEnemies();
        
        public ActorEnemy? CreateEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> CreateAllEnemies(Dictionary<string, ActorEnemy> pEnemies);
        public ActorPlayer? GetPlayer(ActorPlayer Player);
        public ActorPlayer? CreatePlayer(ActorPlayer Player);
        public Dictionary<string, ActorPlayer> GetAllPlayers();
    }
}