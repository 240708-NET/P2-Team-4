using Microsoft.AspNetCore.Mvc;
using Project2.Data;
using Project2.Models.Actor;
using Project2.Models.Combats;
using Project2.Models.User;

namespace Project2.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase {
        private readonly ILogger<DataController> _Logger;
        private readonly IData _Data;

        //  Constructor
        public DataController(ILogger<DataController> pLogger, IData pData) {
            _Logger = pLogger;
            _Data = pData;
        }

        //--------------------------------------------------
        //  Cave Methods
        //--------------------------------------------------
        //  GetMethod - Get Cave Area
        [HttpGet("/getCaveArea/{pId}")]
        public string GetCaveArea(int pId) {
            Console.WriteLine("HttpGet : Get Cave Area");
            return _Data.GetCaveArea(pId);
        }

        //--------------------------------------------------
        //  Enemy Methods
        //--------------------------------------------------
        //  GetMethod - Get Enemy
        [HttpGet("/getEnemy")]
        public ActorEnemy? GetEnemy([FromBody] ActorEnemy pEnemy) {
            Console.WriteLine("HttpGet : Get Enemy");
            return _Data.GetEnemy(pEnemy);
        }

        //  GetMethod - Get Enemy By Id
        [HttpGet("/getEnemyById/{pId}")]
        public ActorEnemy? GetEnemyById(int pId) {
            Console.WriteLine($"HttpGet : Get Enemy By Id ({pId})");
            return _Data.GetEnemyById(pId);
        }

        //  GetMethod - Get Enemy By Name
        [HttpGet("/getEnemyByName/{pName}")]
        public ActorEnemy? GetEnemyByName(string pName) {
            Console.WriteLine($"HttpGet : Get Enemy By Name ({pName})");
            return _Data.GetEnemyByName(pName);
        }

        //  GetMethod - Get All Enemies
        [HttpGet("/getAllEnemies")]
        public Dictionary<string, ActorEnemy> GetAllEnemies() {
            Console.WriteLine("HttpGet : Get All Enemies");
            return _Data.GetAllEnemies();
        }

        //  GetMethod - Get Random Enemy
        [HttpGet("/getRandomEnemy")]
        public ActorEnemy? GetRandomEnemy() {
            Console.WriteLine("HttpGet : Get Random Enemy");
            return _Data.GetRandomEnemy();
        }

        //  PostMethod - Create Enemy
        [HttpPost("/createEnemy")]
        public ActorEnemy? CreateEnemy([FromBody] ActorEnemy pEnemy) {
            Console.WriteLine("HttpPost : Create Enemy");
            return _Data.CreateEnemy(pEnemy);
        }

        //  PostMethod - Create All Enemies
        [HttpPost("/createAllEnemies")]
        public Dictionary<string, ActorEnemy> CreateAllEnemies([FromBody] Dictionary<string, ActorEnemy> pEnemies) {
            Console.WriteLine("HttpPost : Create All Enemies");
            return _Data.CreateAllEnemies(pEnemies);
        }

       // PutMethod - Update Enemy
        [HttpPut("/updateEnemy")]
        public ActionResult<ActorEnemy> UpdateEnemy([FromBody] ActorEnemy updatedEnemy) {
            Console.WriteLine("HttpPut : Update Enemy");
            var enemy = _Data.UpdateEnemy(updatedEnemy);
            if (enemy == null) {
                return NotFound();
            }
            return enemy;
        }

        //  PutMethod - Reset Enemy Health
        [HttpPut("/resetEnemyHealth/{pId}")]
        public void ResetEnemyHealth(int pId) {
            Console.WriteLine("HttpPut : Reset Enemy Health");
            _Data.ResetEnemyHealth(pId);
        }

        // Delete Enemy
        [HttpDelete("/deleteEnemy/{pId}")]
        public IActionResult DeleteEnemy(int pId) {
            var success = _Data.DeleteEnemy(pId);
            if (!success) {
                return NotFound();
            }
            return NoContent();
        }

        //--------------------------------------------------
        //  Player Methods
        //--------------------------------------------------
        //  GetMethod - Get Player
        [HttpGet("/getPlayer")]
        public ActorPlayer? GetPlayer([FromBody] ActorPlayer pPlayer) {
            Console.WriteLine($"HttpGet : Get Player");
            return _Data.GetPlayer(pPlayer);
        }

        // Get Player By Id
        [HttpGet("/getPlayerById/{pId}")]
        public ActorPlayer? GetPlayerById(int pId) {
            Console.WriteLine($"HttpGet : Get Player By Id ({pId})");
            return _Data.GetPlayerById(pId);
        }

        // Get Player By Name
        [HttpGet("/getPlayerByName/{pUserId}/{pName}")]
        public ActorPlayer? GetPlayerByName(int pUserId, string pName) {
            Console.WriteLine($"HttpGet : Get Player Of User ({pUserId}) By Name ({pName})");
            return _Data.GetPlayerByName(pUserId, pName);
        }

        //  Get All Players
        [HttpGet("/getAllPlayers")]
        public Dictionary<string, ActorPlayer> GetAllPlayers() {
            Console.WriteLine($"HttpGet : Get All Players");
            return _Data.GetAllPlayers();
        }
        
        //  Get All Players Name
        [HttpGet("/getAllPlayersName")]
        public List<string> GetAllPlayersName(int pId) {
            Console.WriteLine($"HttpGet : Get All Players Name");
            return _Data.GetAllPlayersName();
        }

        //  Get Player Attributes
        [HttpGet("/getPlayerAttributes/{pUserId}/{pId}")]
        public string? GetPlayerAttributes(int pUserId, int pId){
            Console.WriteLine($"HttpGet : Get Player {pId} Attributes");
            return _Data.GetPlayerAttributes(pUserId, pId);
        }

        //  PostMethod - Create Player
        [HttpPost("/createPlayer")]
        public ActorPlayer? CreatePlayer([FromBody] ActorPlayer pPlayer) {
            Console.WriteLine("HttpPost : Create Player");
            return _Data.CreatePlayer(pPlayer);
        }

        //  PostMethod - Create Empty Player
        [HttpPost("/createEmptyPlayer/{pUserId}/{pName}")]
        public ActorPlayer? CreateEmptyPlayer(int pUserId, string pName) {
            Console.WriteLine("HttpPost : Create Empty Player");
            return _Data.CreateEmptyPlayer(pUserId, pName);
        }

        //  PostMethod - Create Player Name
        [HttpPost("/createPlayerName/{pId}/{pName}")]
        public ActorPlayer? CreatePlayerName(int pId, string pName) {
            Console.WriteLine("HttpPost : Create Player Name");
            return _Data.CreatePlayerName(pId, pName);
        }

        //  GetMethod - Create Attribute Pool
        [HttpGet("/createAttributePool/{pType}")]
        public List<int> CreateAttributePool(string pType) {
            Console.WriteLine("HttpGet : Create Attribute Pool");
            return _Data.CreateAttributePool(pType);
        }

        //  PostMethod - Create Player Attributes
        [HttpPost("/createPlayerAttributes/{pId}/{pAttr}")]
        public ActorPlayer? CreatePlayerAttributes(int pId, string pAttr) {
            Console.WriteLine("HttpPost : Create Player Attributes");
            return _Data.CreatePlayerAttributes(pId, pAttr);
        }

        //  PostMethod - Create Player Class
        [HttpPost("/createPlayerClass/{pId}")]
        public ActorPlayer? CreatePlayerClass(int pId, [FromBody] string pClass) {
            Console.WriteLine("HttpPost : Create Player Class");
            return _Data.CreatePlayerClass(pId, pClass);
        }

        //  PostMethod - Create Player Level
        [HttpPost("/createPlayerLevel/{pId}")]
        public ActorPlayer? CreatePlayerLevel(int pId, [FromBody] string pBody) {
            Console.WriteLine("HttpPost : Create Player Level");
            string[] bodyArr = pBody.Split("_");
            return _Data.CreatePlayerLevel(pId, int.Parse(bodyArr[0]), bodyArr[1]);
        }

        //  PostMethod - Create Player Skill
        [HttpPost("/createPlayerSkill/{pId}")]
        public ActorPlayer? CreatePlayerSkill(int pId, [FromBody] int pProf) {
            Console.WriteLine("HttpPost : Create Player Skill");
            return _Data.CreatePlayerSkill(pId, pProf);
        }

        //  PostMethod - Create Player Health
        [HttpPost("/createPlayerHealth/{pId}")]
        public ActorPlayer? CreatePlayerHealth(int pId, [FromBody] string pDice) {
            Console.WriteLine("HttpPost : Create Player Health");
            return _Data.CreatePlayerHealth(pId, pDice);
        }

        //  PostMethod - Create Player Unarmed
        [HttpPost("/createPlayerUnarmed/{pId}")]
        public ActorPlayer? CreatePlayerUnarmed(int pId, [FromBody] string pAttack) {
            Console.WriteLine("HttpPost : Create Player Unarmed");
            return _Data.CreatePlayerUnarmed(pId, pAttack);
        }

        //  PostMethod - Create Player Attack
        [HttpPost("/createPlayerAttack/{pId}")]
        public ActorPlayer? CreatePlayerAttack(int pId, [FromBody] string pAttack) {
            Console.WriteLine("HttpPost : Create Player Attack");
            return _Data.CreatePlayerAttack(pId, pAttack);
        }

        //  PostMethod - Create Player Defense
        [HttpPost("/createPlayerDefense/{pId}")]
        public ActorPlayer? CreatePlayerDefense(int pId, [FromBody] string pDefense) {
            Console.WriteLine("HttpPost : Create Player Defense");
            return _Data.CreatePlayerDefense(pId, pDefense);
        }

        //  PutMethod - Player Gain Exp
        [HttpPut("/playerGainExp/{pId}/{pAmt}")]
        public ActorPlayer? PlayerGainExp(int pId, int pAmt) {
            Console.WriteLine("HttpPut : Player Gain Experience");
            return _Data.PlayerGainExp(pId, pAmt);
        }

        //  PutMethod - Update Player
        [HttpPut("/updatePlayer")]
        public void UpdatePlayer([FromBody] ActorPlayer pPlayer) {
            Console.WriteLine("HttpPut : Update Player");
            _Data.UpdatePlayer(pPlayer);
        }

        //--------------------------------------------------
        //  User Methods
        //--------------------------------------------------
        // GetMethod - Get User By Id
        [HttpGet("/getUserById/{id}")]
        public UserPlayer? GetUserById(int id) {
            Console.WriteLine($"HttpGet : Get User By Id ({id})");
            return _Data.GetUserById(id);
        }

        //  GetMethod - Get User By Name
        [HttpGet("/getUserByName/{pName}")]
        public UserPlayer? GetUserByName(string pName) {
            Console.WriteLine($"HttpGet : Get User By Name ({pName})");
            return _Data.GetUserByName(pName);
        }

        //  PostMethod - Create User
        [HttpPost("/createUser/{pName}")]
        public UserPlayer? CreateUser(string pName) {
            Console.WriteLine("HttpPost : Create User");
            return _Data.CreateUser(pName);
        }

        //--------------------------------------------------
        //  Combat Methods
        //--------------------------------------------------
        //  GetMethod - Get Combat
        [HttpGet("/getCombat")]
        public Combat? GetCombat() {
            Console.WriteLine($"HttpGet : Get Combat");
            return _Data.GetCombat();
        }

        //  GetMethod - Get Combat Enemy Id
        [HttpGet("/getCombatEnemyId/{pId}")]
        public int GetCombatEnemyId(int pId) {
            Console.WriteLine($"HttpGet : Get Combat Enemy Id");
            return _Data.GetCombatEnemyId(pId);
        }

        //  GetMethod - Get Combat Enemy Name
        [HttpGet("/getCombatEnemyName/{pId}")]
        public string GetCombatEnemyName(string pId) {
            Console.WriteLine($"HttpGet : Get Combat Enemy Name");
            return _Data.GetCombatEnemyName(pId);
        }

        //  GetMethod - Get Combat Enemy Health
        [HttpGet("/getCombatEnemyHealth/{pId}")]
        public string GetCombatEnemyHealth(int pId) {
            Console.WriteLine($"HttpGet : Get Combat Enemy Health");
            return _Data.GetCombatEnemyHealth(pId);
        }

        //  GetMethod - Get Combat Enemy PAC
        [HttpGet("/getCombatEnemyPAC/{pId}")]
        public string GetCombatEnemyPAC(int pId) {
            Console.WriteLine($"HttpGet : Get Combat Enemy PAC");
            return _Data.GetCombatEnemyPAC(pId);
        }

        //  GetMethod - Get Combat Player Name
        [HttpGet("/getCombatPlayerName/{pId}")]
        public string GetCombatPlayerName(int pId) {
            Console.WriteLine($"HttpGet : Get Combat Player Name");
            return _Data.GetCombatPlayerName(pId);
        }

        //  GetMethod - Get Combat Player Health
        [HttpGet("/getCombatPlayerHealth/{pId}")]
        public string GetCombatPlayerHealth(int pId) {
            Console.WriteLine($"HttpGet : Get Combat Player Health");
            return _Data.GetCombatPlayerHealth(pId);
        }

        //  GetMethod - Get Combat Player AC
        [HttpGet("/getCombatPlayerAC/{pId}")]
        public string GetCombatPlayerAC(int pId) {
            Console.WriteLine($"HttpGet : Get Combat Player AC");
            return _Data.GetCombatPlayerAC(pId);
        }

        //  GetMethod - Get Combat By Id
        [HttpGet("/getCombatById/{pId}")]
        public Combat? GetCombatById(int pId) {
            Console.WriteLine($"HttpGet : Get Combat By Id ({pId})");
            return _Data.GetCombatById(pId);
        }

        //  PostMethod - Create Combat
        [HttpPost("/createCombat/{pPlayerId}")]
        public int? CreateCombat(int pPlayerId) {
            Console.WriteLine("HttpPost : Create Combat");
            return _Data.CreateCombat(pPlayerId);
        }

        //  GetMethod - Player Attacks
        [HttpGet("/playerAttacks/{pCombatId}")]
        public string PlayerAttacks(int pCombatId) {
            Console.WriteLine("HttpPut : Player Attacks");
            return _Data.PlayerAttacks(pCombatId);
        }

        //  GetMethod - Enemy Attacks
        [HttpGet("/enemyAttacks/{pCombatId}")]
        public string EnemyAttacks(int pCombatId) {
            Console.WriteLine("HttpPut : Enemy Attacks");
            return _Data.EnemyAttacks(pCombatId);
        }

        //  GetMethod - Combat Ending
        [HttpGet("/combatEnds/{pCombatId}/{pActionId}")]
        public string CombatEnding(int pCombatId, int pActionId) {
            Console.WriteLine("HttpGet : Combat End");
            return _Data.CombatEnding(pCombatId, pActionId);
        }
    }
}