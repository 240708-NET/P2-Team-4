using Microsoft.AspNetCore.Mvc;
using Project2.Data;
using Project2.Models.Actor;
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
        //  Enemy Methods
        //--------------------------------------------------
        //  GetMethod - Get Enemy
        [HttpGet("/getEnemy")]
        public ActorEnemy? GetEnemy([FromBody] ActorEnemy pEnemy) {
            Console.WriteLine("HttpGet : Get Enemy");
            return _Data.GetEnemy(pEnemy);
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

       // Update Enemy
        [HttpPut("/updateEnemy/{pId}")]
        public ActionResult<ActorEnemy> UpdateEnemy(int pId, [FromBody] ActorEnemy updatedEnemy) {
            var enemy = _Data.UpdateEnemy(pId, updatedEnemy);
            if (enemy == null) {
                return NotFound();
            }
            return enemy;
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
        public string GetPlayerAttributes(int pUserId, int pId){
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
    }
}