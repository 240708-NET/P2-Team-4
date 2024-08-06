using Project2.Models.Actor;
using Project2.Models.User;

namespace Project2.Data {
    public interface IData {
        //  Enemy Methods
        public ActorEnemy? GetEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> GetAllEnemies();
        public ActorEnemy? CreateEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> CreateAllEnemies(Dictionary<string, ActorEnemy> pEnemies);

        ActorEnemy? UpdateEnemy(int id, ActorEnemy updatedEnemy); 
        bool DeleteEnemy(int id); 

        //  Player Variables
        public ActorPlayer? GetPlayer(ActorPlayer pPlayer);
        public Dictionary<string, ActorPlayer> GetAllPlayers();
        public ActorPlayer? CreatePlayer(ActorPlayer pPlayer);
        public void UpdatePlayer(ActorPlayer pPlayer);

        //  User Variables
        public UserPlayer? GetUser(UserPlayer pUser);
        public UserPlayer? GetUserByName(string pName);
        public UserPlayer? CreateUser(UserPlayer user);
        

        // New Methods for Users
        public UserPlayer? GetUserById(int id);
        public ActorPlayer? GetPlayerByName(string name);
        public ActorPlayer? GetPlayerById(int id);

         // New Methods 
        public ActorPlayer? CreateEmptyPlayer(int userId);
        public ActorPlayer? CreatePlayerName(int playerId, string name);
        public ActorPlayer? CreatePlayerAttributes(int playerId, Dictionary<string, int> attributes);
        public ActorPlayer? CreatePlayerClass(int playerId, string className);

        
    }
}