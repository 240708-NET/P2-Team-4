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
            return context.Enemies.FirstOrDefault(p => p.Id == pEnemy.Id);
        }

        //  GetMethod - Get All Enemies
        public Dictionary<string, ActorEnemy> GetAllEnemies() {
            Dictionary<string, ActorEnemy> result = new Dictionary<string, ActorEnemy>();
            List<ActorEnemy> enemies = context.Enemies.ToList();
            
            foreach(ActorEnemy enemy in enemies) {
                result.Add($"{enemy.Name.Split("_")[0]}", new ActorEnemy(enemy));
            }

            return result;
        }

        //  GetMethod - Get Random Enemy
        public ActorEnemy? GetRandomEnemy() {
            List<ActorEnemy> enemies = context.Enemies.ToList();

            if (enemies != null && enemies.Count > 0) {
                Random rand = new Random();

                return enemies[rand.Next(0, enemies.Count)];
            }

            return null;
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

         // Update Enemy
        public ActorEnemy? UpdateEnemy(int pId, ActorEnemy pEnemy) {
            var existingEnemy = context.Enemies.FirstOrDefault(e => e.Id == pId);
            if (existingEnemy != null) {
                existingEnemy.Name = pEnemy.Name;
                existingEnemy.HealthDice = pEnemy.HealthDice;
                existingEnemy.HealthBase = pEnemy.HealthBase;
                existingEnemy.HealthCurr = pEnemy.HealthCurr;

                // Update other properties as necessary
                context.SaveChanges();
                return existingEnemy;
            }
            return null;
        }

        // Delete Enemy
        public bool DeleteEnemy(int pId) {
            var enemy = context.Enemies.FirstOrDefault(e => e.Id == pId);
            if (enemy != null) {
                context.Enemies.Remove(enemy);
                context.SaveChanges();
                return true;
            }
            return false;
        }
         
        //--------------------------------------------------
        //  Player Methods
        //--------------------------------------------------
        //  GetMethod - Get Player
        public ActorPlayer? GetPlayer(ActorPlayer pPlayer) {
            return context.Players.FirstOrDefault(p => p.Id == pPlayer.Id);
        }
        
        //  GetMethod - Get Player By Id
        public ActorPlayer? GetPlayerById(int pId) {
            return context.Players.FirstOrDefault(p => p.Id == pId);
        }

        //  GetMethod - Get Player By Name
        public ActorPlayer? GetPlayerByName(int pUserId, string pName) {
            return context.Players.FirstOrDefault(p => p.UserId == pUserId && p.Name == pName);
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

        //  GetMethod - Get Player Attributes
        public string GetPlayerAttributes(int pUserId, int pId) {
            ActorPlayer? player = context.Players.FirstOrDefault(p => p.UserId == pUserId && p.Id == pId);
            if (player != null) {
                return player.Attributes;
            }

            return null;
        }

        //  PostMethod -  Create Player
        public ActorPlayer? CreatePlayer(ActorPlayer pPlayer) {
            Console.WriteLine("-Adding player: " + pPlayer.Name);

            context.Add(pPlayer);
            context.SaveChanges();

            return GetPlayer(pPlayer);
        }

        //  PostMethod - Create Empty Player
        public ActorPlayer? CreateEmptyPlayer(int pUserId, string pName) {
            ActorPlayer player = new ActorPlayer { UserId = pUserId, Name = pName };

            context.Add(player);
            context.SaveChanges();

            return player;
        }

        //  PostMethod - Create Player Name
        public ActorPlayer? CreatePlayerName(int pId, string pName) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerName): Player not found");
            }

            //  Player isn't null
            else {
                player.Name = pName;
                context.SaveChanges();
            }

            return player;
        }

        //  GetMethod - Create Attribute Pool
        public List<int> CreateAttributePool(string pType) {
            List<int> attributePool = new List<int>();
            List<int> rolls = new List<int>();
            Random rand = new Random();
            int total = 0;

            for(int i = 0; i < 6; i++) {
                rolls.Clear();

                switch(pType) {
                    //  Roll 4d6, drop lowest
                    case "4d6d1":
                        //  Roll 4d6
                        for(int x = 0; x < 4; x++) {
                            rolls.Add(rand.Next(0, 6)+1);
                        }

                        //  Drop lowest
                        rolls.Sort();
                        rolls.RemoveAt(0);
                        total = rolls.Sum();
                        break;

                    //  Roll 3d6
                    case "3d6":
                        //  Roll 3d6
                        for(int x = 0; x < 3; x++) {
                            rolls.Add(rand.Next(0, 6)+1);
                        }

                        total = rolls.Sum();
                        break;

                    // Roll 2d6, add 6
                    case "2d6+6":
                        //  Roll 2d6
                        for(int x = 0; x < 2; x++) {
                            rolls.Add(rand.Next(0, 6)+1);
                        }

                        //  Add 6
                        rolls.Add(6);
                        total = rolls.Sum();
                        break;

                    //  Catch All
                    default:
                        if (i == 0) {
                            Console.WriteLine("ERROR (CreateAttributePool): Invalid pool type, adding 10 to pool");
                        }
                        else {
                            Console.WriteLine("-adding 10 to pool");
                        }

                        attributePool.Add(10);
                        break;
                }

                //  Add to attribute pool
                attributePool.Add(total);
            }

            return attributePool;
        }

        //  PostMethod - Create Player Attributes
        public ActorPlayer? CreatePlayerAttributes(int pId, string pAttr) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);
            string[] attrArr = pAttr.Split(",");

            //  Player is null
            if (player == null) {
                Console.WriteLine("ERROR (CreatePlayerAttributes): Player not found");
            }

            //  Attribute doesn't have 6 values
            else if (attrArr.Length != 6) {
                Console.WriteLine("ERROR (CreatePlayerAttributes): Attribute doesn't have 6 values, setting all to 10");
                player.Attributes = "10,10,10,10,10,10";
            }

            //  Player isn't null and Attribute has 6 values
            else {
                string attribute = "";
                bool attrValid = false;
                int attrNum = 0;

                //  Loop through attribute array
                for (int i = 0; i < attrArr.Length; i++) {
                    attrValid = Int32.TryParse(attrArr[i], out attrNum);

                    //  Attribute wasn't a number
                    if (attrValid == false) {
                        Console.WriteLine($"ERROR (CreatePlayerAttributes): Attribute {(i+1)} wasn't a number, setting it to 10");
                        attribute += 10 + ((i < attrArr.Length-1) ? "," : "");
                    }

                    //  Attribute was a number
                    else {
                        attribute += attrNum + ((i < attrArr.Length-1) ? "," : "");
                    }
                }

                player.Attributes = "" + pAttr;
                context.SaveChanges();
            }

            return player;
        }

        //  PostMethod - Create Player Class
        public ActorPlayer? CreatePlayerClass(int pId, string pClass) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerClass): Player not found");
            }

            //  Player isn't null
            else {
                player.Class = pClass;
                context.SaveChanges();
            }

            return player;
        }

        //  PostMethod - Create Player Level
        public ActorPlayer? CreatePlayerLevel(int pId, int pLevel, string pExp) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerLevel): Player not found");
            }

            //  Player isn't null
            else {
                player.Level = pLevel;
                player.Experience = pExp;
                context.SaveChanges();
            }

            return player;
        }

        //  PostMethod - Create Player Skill
        public ActorPlayer? CreatePlayerSkill(int pId, int pProf) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerSkill): Player not found");
            }

            //  Player isn't null
            else {
                player.Proficiency = pProf;
                context.SaveChanges();
            }

            return player;
        }

        //  PostMethod - Create Player Health
        public ActorPlayer? CreatePlayerHealth(int pId, string pDice) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerClass): Player not found");
            }

            //  Player isn't null
            else {
                Random rand = new Random();

                string[] diceArr = pDice.Split("d");
                int diceNum = int.Parse(diceArr[1]);
                int conNum = int.Parse(player.Attributes.Split(",")[2]);
                int conMod = (conNum / 2) - 5;

                int health = diceNum + conMod;

                for(int i = 1; i < player.Level; i++) {
                    int amt = (rand.Next(0, diceNum)+1);
                    health += amt + conMod;
                }
                
                player.HealthDice = "" + pDice;
                player.HealthBase = 0 + health;
                player.HealthCurr = 0 + player.HealthBase;
                context.SaveChanges();
            }

            return player;
        }

        //  PostMethod - Create Player Unarmed
        public ActorPlayer? CreatePlayerUnarmed(int pId, string pAttack) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerUnarmed): Player not found");
            }

            //  Player isn't null
            else {
                player.AttackUnarmed = pAttack;
                context.SaveChanges();
            }

            return player;
        }

        //  PostMethod - Create Player Attack
        public ActorPlayer? CreatePlayerAttack(int pId, string pAttack) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerAttack): Player not found");
            }

            //  Player isn't null
            else {
                player.AttackList += ((player.AttackList != null) ? "," : "") + pAttack;
                context.SaveChanges();
            }

            return player;
        }

        //  PostMethod - Create Player Defense
        public ActorPlayer? CreatePlayerDefense(int pId, string pDefense) {
            var player = context.Players.FirstOrDefault(p => p.Id == pId);

            //  Player is null
            if (player == null) {
                Console.WriteLine($"ERROR (CreatePlayerDefense): Player not found");
            }

            //  Player isn't null
            else {
                player.DefenseArmor = pDefense;
                context.SaveChanges();
            }

            return player;
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
            return context.Users.FirstOrDefault(u => u.Id == pUser.Id);
        }

        //  GetMethod - Get User By Id
        public UserPlayer? GetUserById(int pId) {
            return context.Users.FirstOrDefault(u => u.Id == pId);
        }

        //  GetMethod - Get User By Name
        public UserPlayer? GetUserByName(string pName) {
            return context.Users.FirstOrDefault(p => p.Name == pName);
        }

        //  PostMethod - Create User
        public UserPlayer? CreateUser(string pName) {
            var existingUser = GetUserByName(pName);

            if (existingUser != null) {
                Console.WriteLine($"-User {pName} already exists");
                return existingUser;
            }

            Console.WriteLine("-Adding new user: " + pName);
            context.Add(new UserPlayer() { Name = pName, });
            context.SaveChanges();

            return GetUserByName(pName);
        }
    }
}