using Microsoft.AspNetCore.Mvc;
using Project2.Data;
using Project2.Models.Actor;

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
        /// <summary>
        /// Gets all enemies from database, builds objects from the server information
        /// </summary>
        /// <returns>Dictionary of local enemy objects with name as key</returns>
        [HttpGet("/getAllEnemies")]
        public Dictionary<string, ActorEnemy> GetAllEnemies() {
            return _Data.GetAllEnemies();
        }

        //  PostMethod - Create All Enemies
        [HttpPost("/createAllEnemies")]
        public Dictionary<string, ActorEnemy> CreateAllEnemies([FromBody] Dictionary<string, ActorEnemy> pEnemies) {
            return _Data.CreateAllEnemies(pEnemies);
        }
    }
}