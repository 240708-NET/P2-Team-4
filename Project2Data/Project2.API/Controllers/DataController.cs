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

        //  GetMethod - Get All Enemies
        [HttpGet("/getAllEnemies")]
        public Dictionary<string, ActorEnemy> GetAllEnemies() {
            Console.WriteLine("HttpGet : Get All Enemies");
            return _Data.GetAllEnemies();
        }

        //  PostMethod - Create All Enemies
        [HttpPost("/createAllEnemies")]
        public Dictionary<string, ActorEnemy> CreateAllEnemies([FromBody] Dictionary<string, ActorEnemy> pEnemies) {
            Console.WriteLine("HttpPost : Create All Enemies");
            return _Data.CreateAllEnemies(pEnemies);
        }

        //  GetMethod - Get User By Name
        [HttpGet("/getUserByName/{pName}")]
        public UserPlayer? GetUserByName(string pName) {
            Console.WriteLine($"HttpGet : Get User By Name ({pName})");
            return _Data.GetUserByName(pName);
        }

        //  PostMethod - Create User
        [HttpPost("/createUser")]
        public UserPlayer? CreateUser([FromBody] UserPlayer pUser){
            Console.WriteLine("HttpPost : Create User");
            return _Data.CreateUser(pUser);
        }

        //  PostMethod - Create Player
        [HttpPost("/createPlayer")]
        public ActorPlayer? CreatePlayer([FromBody] ActorPlayer pPlayer) {
            Console.WriteLine("HttpPost : Create Player");
            return _Data.CreatePlayer(pPlayer);
        }

        //  PutMethod - Update Player
        [HttpPut("/updatePlayer")]
        public void UpdatePlayer([FromBody] ActorPlayer pPlayer) {
            Console.WriteLine("HttpPut : Update Player");
            _Data.UpdatePlayer(pPlayer);
        }
    }
}