using Microsoft.EntityFrameworkCore;
using Project2.Models.Actor;

namespace Project2.Data {
    public class DataHandler : IData {
        private DataContext context;

        //  Constructor
        public DataHandler(string pConnect) {
            context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(pConnect).Options);
        }

        //  GetMethod - Get Enemy
        public ActorEnemy? GetEnemy(ActorEnemy pEnemy) {
            var found = from e in context.Enemies.ToList()
                where e.Id == pEnemy.Id
                select e;

            return found.FirstOrDefault();
        }

        //  GetMethod - Get All Enemies
        /// <summary>
        /// Gets all enemies from database, builds objects from the server information
        /// </summary>
        /// <returns>Dictionary of local enemy objects with name as key</returns>
        public Dictionary<string, ActorEnemy> GetAllEnemies() {
            Dictionary<string, ActorEnemy> result = new Dictionary<string, ActorEnemy>();

            List<ActorEnemy> enemies = context.Enemies.ToList();
            //Console.WriteLine(enemies[0].ActorStr);
            foreach(ActorEnemy enemy in enemies) {
                result.Add($"{enemy.Name.Split("_")[0]}", new ActorEnemy(enemy));
            }

            return result;
        }

        //  PostMethod - Create Enemy
        public ActorEnemy? CreateEnemy(ActorEnemy pEnemy) {
            context.Add(pEnemy);
            context.SaveChanges();

            return GetEnemy(pEnemy);
        }

        //  PostMethod - Create All Enemies
        public Dictionary<string, ActorEnemy> CreateAllEnemies(Dictionary<string, ActorEnemy> pEnemies) {
            foreach(var enemy in pEnemies) {
                context.Add(enemy.Value);
            }
            context.SaveChanges();

            return GetAllEnemies();
        }
    }
}