using Project2.Models.Actor;
using Project2.Models.User;

namespace Project2.Data {
    public interface IData {
        //  Enemy Methods
        public ActorEnemy? GetEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> GetAllEnemies();
        public ActorEnemy? GetRandomEnemy();
        public ActorEnemy? CreateEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> CreateAllEnemies(Dictionary<string, ActorEnemy> pEnemies);
        public ActorEnemy? UpdateEnemy(int pId, ActorEnemy pEnemy); 
        public bool DeleteEnemy(int pId); 

        //  Player Variables
        public ActorPlayer? GetPlayer(ActorPlayer pPlayer);
        public ActorPlayer? GetPlayerById(int pId);
        public List<string> GetAllPlayersName();
        public ActorPlayer? GetPlayerByName(int pUserId, string pName);
        public Dictionary<string, ActorPlayer> GetAllPlayers();
        public string GetPlayerAttributes(int pUserId, int pId);
        public ActorPlayer? CreatePlayer(ActorPlayer pPlayer);
        public ActorPlayer? CreateEmptyPlayer(int pUserId, string pName);
        public ActorPlayer? CreatePlayerName(int pId, string pName);
        public List<int> CreateAttributePool(string pType);
        public ActorPlayer? CreatePlayerAttributes(int pId, string pAttr);
        public ActorPlayer? CreatePlayerClass(int pId, string pClass);  
        public ActorPlayer? CreatePlayerLevel(int pId, int pLevel, string pExp);
        public ActorPlayer? CreatePlayerSkill(int pId, int pProf); 
        public ActorPlayer? CreatePlayerHealth(int pId, string pDice);
        public ActorPlayer? CreatePlayerUnarmed(int pId, string pAttack);
        public ActorPlayer? CreatePlayerAttack(int pId, string pAttack);
        public ActorPlayer? CreatePlayerDefense(int pId, string pDefense);
        public void UpdatePlayer(ActorPlayer pPlayer);

        //  User Variables
        public UserPlayer? GetUser(UserPlayer pUser);
        public UserPlayer? GetUserById(int pId);
        public UserPlayer? GetUserByName(string pName);
        public UserPlayer? CreateUser(string pName);  
    }
}