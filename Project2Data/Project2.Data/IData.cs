using Project2.Models.Actor;
using Project2.Models.Combats;
using Project2.Models.User;

namespace Project2.Data {
    public interface IData {
        //  Cave Methods
        public string GetCaveArea(int pId);

        //  Enemy Methods
        public ActorEnemy? GetEnemy(ActorEnemy pEnemy);
        public ActorEnemy? GetEnemyById(int pId);
        public ActorEnemy? GetEnemyByName(string pName);
        public Dictionary<string, ActorEnemy> GetAllEnemies();
        public ActorEnemy? GetRandomEnemy();
        public ActorEnemy? CreateEnemy(ActorEnemy pEnemy);
        public Dictionary<string, ActorEnemy> CreateAllEnemies(Dictionary<string, ActorEnemy> pEnemies);
        public ActorEnemy? UpdateEnemy(ActorEnemy pEnemy); 
        public void ResetEnemyHealth(int pId);
        public bool DeleteEnemy(int pId); 

        //  Player Variables
        public ActorPlayer? GetPlayer(ActorPlayer pPlayer);
        public ActorPlayer? GetPlayerById(int pId);
        public ActorPlayer? GetPlayerByName(int pUserId, string pName);
        public Dictionary<string, ActorPlayer> GetAllPlayers();
        public List<string> GetAllPlayersName();
        public string GetPlayerName(int pId);
        public string? GetPlayerAttributes(int pUserId, int pId);
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
        public ActorPlayer? PlayerGainExp(int pId, int pAmt);
        public void UpdatePlayer(ActorPlayer pPlayer);

        //  User Variables
        public UserPlayer? GetUser(UserPlayer pUser);
        public UserPlayer? GetUserById(int pId);
        public UserPlayer? GetUserByName(string pName);
        public UserPlayer? CreateUser(string pName);  

        //  Combat Variables
        public Combat? GetCombat();
        public int GetCombatEnemyId(int pCombatId);
        public string GetCombatEnemyName(int pCombatId);
        public string GetCombatEnemyHealth(int pCombatId);
        public string GetCombatEnemyPAC(int pCombatId);
        public string GetCombatPlayerName(int pCombatId);
        public string GetCombatPlayerHealth(int pCombatId);
        public string GetCombatPlayerAC(int pCombatId);
        public Combat? GetCombatById(int pId);
        public int? CreateCombat(int pId);
        public string PlayerAttacks(int pCombatId);
        public void UpdateEnemyPAC(Combat pCombat, int pToHit);
        public string EnemyAttacks(int pCombatId);
        public string CombatEnding(int pCombatId, int pActionId);
    }
}