using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Project2.Models.Actor;
using Project2.Models.User;

namespace Project2.Data {
    public class DataHandler : IData {
        private DataContext context;

        //--------------------------------------------------
        //  Constructors
        //--------------------------------------------------
        //  Constructor
        public DataHandler(string pConnect) {
            context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(pConnect).Options);
        }

        //--------------------------------------------------
        //  Enemy Methods
        //--------------------------------------------------
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
                Console.WriteLine("-Adding enemy: " + enemy.Value.Name);
                context.Add(enemy.Value);
            }
            context.SaveChanges();

            return GetAllEnemies();
        }
         
        //--------------------------------------------------
        //  Player Methods
        //--------------------------------------------------
        //  GetMethod - Get Player
        public ActorPlayer? GetPlayer(ActorPlayer pPlayer) {
            var found = from e in context.Players.ToList()
                where e.Id == pPlayer.Id
                select e;

            return found.FirstOrDefault();
        }
        
        //  GetMethod - Get All Players
        public Dictionary<string, ActorPlayer> GetAllPlayers() {
            Dictionary<string, ActorPlayer> result = new Dictionary<string, ActorPlayer>();

            List<ActorPlayer> Players = context.Players.ToList();
            //Console.WriteLine(enemies[0].ActorStr);
            foreach(ActorPlayer player in Players) {
                result.Add($"{player.Name.Split("_")[0]}", new ActorPlayer(player));
            }

            return result;
        }

        //  PostMethod -  Create Player
        public ActorPlayer? CreatePlayer(ActorPlayer pPlayer) {
            Console.WriteLine("-Adding player: " + pPlayer.Name);
            context.Add(pPlayer);
            context.SaveChanges();

            return GetPlayer(pPlayer);
        }

        //  PutMethod - Update Player
        public void UpdatePlayer(ActorPlayer pPlayer) {
            ActorPlayer? player = GetPlayer(pPlayer);

            if (player != null) {
                player.Attributes = $"{pPlayer.D_AttrScr["STR"]},{pPlayer.D_AttrScr["DEX"]},{pPlayer.D_AttrScr["CON"]},{pPlayer.D_AttrScr["INT"]},{pPlayer.D_AttrScr["WIS"]},{pPlayer.D_AttrScr["CHA"]}";

                player.AttackUnarmed = "" + pPlayer.Atk_Unarmed.ToString();

                string attacks = "";
                for(int i = 0; i < pPlayer.Atk_List.Count; i++) {
                    attacks += player.Atk_List[i] + ((i < pPlayer.Atk_List.Count-1) ? "," : "");
                }
                player.AttackList = "" + attacks;

                player.DefenseArmor = "" + pPlayer.DefenseArmor;

                player.HealthDice = "" + pPlayer.HealthDice;

                player.Name = "" + pPlayer.Name;

                player.Proficiency = 0 + pPlayer.Proficiency;

                context.Entry(player).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    
        //--------------------------------------------------
        //  User Methods
        //--------------------------------------------------
        //  GetMethod - Get User
        public UserPlayer? GetUser(UserPlayer pUser) {
            var found = from e in context.Users.ToList()
                where e.Id == pUser.Id
                select e;

            return found.FirstOrDefault();
        }

        //  GetMethod - Get User By Name
        public UserPlayer? GetUserByName(string pName) {
            var found = from e in context.Users.ToList()
                where e.Name == pName
                select e;

            if (found.FirstOrDefault() != null) {
                Console.WriteLine($"-User {pName} found");
            }
            else {
                Console.WriteLine("-User not found");
            }
            return found.FirstOrDefault();
        }

        //  PostMethod -  Create User
        public UserPlayer? CreateUser(UserPlayer pUser) {
            Console.WriteLine("-Adding user: " + pUser.Name);
            context.Add(pUser);
            context.SaveChanges();

            return GetUser(pUser);
        }
    }
}