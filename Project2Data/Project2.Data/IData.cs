using Project2.Models.Actor;
using Project2.Models.User;

namespace Project2.Data {
    public interface IData {
        //  Enemy Methods
        public ActorEnemy? GetEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> GetAllEnemies();
        public ActorEnemy? CreateEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> CreateAllEnemies(Dictionary<string, ActorEnemy> pEnemies);

        //  Player Variables
        public ActorPlayer? GetPlayer(ActorPlayer pPlayer);
        public Dictionary<string, ActorPlayer> GetAllPlayers();
        public ActorPlayer? CreatePlayer(ActorPlayer pPlayer);
        public void UpdatePlayer(ActorPlayer pPlayer);

        //  User Variables
        public UserPlayer? GetUser(UserPlayer pUser);
        public UserPlayer? GetUserByName(string pName);
        public UserPlayer? CreateUser(UserPlayer pUser);
    }
}