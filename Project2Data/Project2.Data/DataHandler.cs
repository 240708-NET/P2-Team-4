using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using Project2.Models.Actor;
using Project2.Models.Combats;
using Project2.Models.User;

namespace Project2.Data {
    public class DataHandler : IData {
        //  ~Reference Variables
        private readonly Random rand;
        //  ~Server Variables
        private DataContext context;

        //--------------------------------------------------
        //  Constructors
        //--------------------------------------------------
        //  Constructor
        public DataHandler(string pConnect) {
            //  Setup ~Reference
            rand = new Random();

            //  Setup ~Server
            context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(pConnect).Options);
        }

        //--------------------------------------------------
        //  Cave Methods
        //--------------------------------------------------
        //  GetMethod - Get Cave Area
        public string GetCaveArea(int pId) {
            int chance = rand.Next(0, 100) + 1;

            //  20% chance for nothing (Player restores 2 health)
            if (chance <= 20) {
                PlayerRestoresHealth(pId, 2);
                return "Nothing";
            }

            //  60% chance for combat encounter
            else if (chance <= 80) {
                return "Combat";
            }

            //  20% chance for treasure room (Player gains 100 exp)
            else {
                PlayerGainExp(pId, 100);
                return "Experience";
            }
        }

        //--------------------------------------------------
        //  Enemy Methods
        //--------------------------------------------------
        //  GetMethod - Get Enemy
        public ActorEnemy? GetEnemy(ActorEnemy pEnemy) {
            return context.Enemies.FirstOrDefault(p => p.Id == pEnemy.Id);
        }

        //  GetMethod - Get Enemy By Id
        public ActorEnemy? GetEnemyById(int pId) {
            return context.Enemies.FirstOrDefault(p => p.Id == pId);
        }

        //  GetMethod - Get Enemy By Name
        public ActorEnemy? GetEnemyByName(string pName) {
            return context.Enemies.FirstOrDefault(p => p.Name == pName);
        }

        //  GetMethod - Get Enemy Article
        public string GetEnemyArticle(int pId) {
            ActorEnemy? enemy = GetEnemyById(pId);
            
            if (enemy != null) {
                if (enemy.Name != null && enemy.Name != "") {
                    string first = enemy.Name.Substring(0, 1);
                    return (first == "a" || first == "e" || first == "i" || first == "o" || first == "u") ? "an" : "a";
                }
                return "ERROR (GetEnemyArticle): Enemy Name Not Valid";
            }

            return "";
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
            ActorEnemy? existingEnemy = GetEnemy(pEnemy);

            //  Enemy already exists
                if (existingEnemy != null) {
                    Console.WriteLine("-Updating enemy: " + pEnemy.Name);
                    UpdateEnemy(pEnemy);
                }

                //  Enemy doesn't exist
                else {
                    Console.WriteLine("-Adding enemy: " + pEnemy.Name);
                    context.Add(pEnemy);
                }

            return GetEnemy(pEnemy);
        }

        //  PostMethod - Create All Enemies
        public Dictionary<string, ActorEnemy> CreateAllEnemies(Dictionary<string, ActorEnemy> pEnemies) {
            foreach(var enemy in pEnemies) {
                ActorEnemy? existingEnemy = GetEnemyByName(enemy.Value.Name);

                //  Enemy already exists
                if (existingEnemy != null) {
                    Console.WriteLine("-Updating enemy: " + enemy.Value.Name);
                    UpdateEnemy(enemy.Value);
                }

                //  Enemy doesn't exist
                else {
                    Console.WriteLine("-Adding enemy: " + enemy.Value.Name);
                    context.Add(enemy.Value);
                }
            }
            context.SaveChanges();

            return GetAllEnemies();
        }

        // PutMethod - Update Enemy
        public ActorEnemy? UpdateEnemy(ActorEnemy pEnemy) {
            ActorEnemy? enemy = GetEnemyByName(pEnemy.Name.Contains('_') ? pEnemy.Name : pEnemy.Name + "_False");

            if (enemy != null) {
                enemy.Attributes = "" + pEnemy.Attributes; 

                enemy.AttackUnarmed = "" + pEnemy.AttackUnarmed;
                enemy.AttackList = "" + pEnemy.AttackList;

                enemy.DefenseArmor = "" + pEnemy.DefenseArmor;

                enemy.HealthDice = "" + pEnemy.HealthDice;

                if (pEnemy.HealthBase != 0) {
                    enemy.Health = "" + pEnemy.HealthCurr + "/" + pEnemy.HealthBase;
                }
                else {
                    enemy.Health = "" + pEnemy.Health;
                }

                enemy.Name = "" + pEnemy.Name + (pEnemy.Name.Contains('_') ? "" : "_False");

                context.Entry(enemy).State = EntityState.Modified;
                context.SaveChanges();
            }

            return enemy;
        }

        //  PutMethod - Reset Enemy Health
        public void ResetEnemyHealth(int pId) {
            ActorEnemy? tempEnemy = GetEnemyById(pId);

            if (tempEnemy != null) {
                Console.WriteLine("-" + tempEnemy.Health);
                string[] healthArr = tempEnemy.Health.Split("/");
                tempEnemy.Health = healthArr[1] + "/" + healthArr[1];
                
                context.Entry(tempEnemy).State = EntityState.Modified;
                context.SaveChanges();
            }
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

        // Just a test method to see if we can get all players from the database
        // just to demonastrate that we can get data from the database
        public Dictionary<string, ActorPlayer> GetAllPlayers() {
        Dictionary<string, ActorPlayer> result = new Dictionary<string, ActorPlayer>();
        List<ActorPlayer> Players = context.Players.ToList();

        Console.WriteLine($"Fetched {Players.Count} players from the database.");

        foreach (ActorPlayer player in Players) {
            try {
                Console.WriteLine($"Processing player: {player.Name}, ID: {player.Id}");

                // Check if the properties are null or empty and assign default values if necessary
                player.Class = string.IsNullOrEmpty(player.Class) ? "fighter" : player.Class;
                player.Experience = string.IsNullOrEmpty(player.Experience) ? "0/300" : player.Experience;
                player.Attributes = string.IsNullOrEmpty(player.Attributes) ? "10,10,10,10,10,10" : player.Attributes;
                player.AttackUnarmed = string.IsNullOrEmpty(player.AttackUnarmed) ? "0/1_bludgeoning" : player.AttackUnarmed;
                player.AttackList = string.IsNullOrEmpty(player.AttackList) ? "0/1_slashing" : player.AttackList;
                player.DefenseArmor = string.IsNullOrEmpty(player.DefenseArmor) ? "10" : player.DefenseArmor;
                player.HealthDice = string.IsNullOrEmpty(player.HealthDice) ? "1d6" : player.HealthDice;
                player.Name = string.IsNullOrEmpty(player.Name) ? "Unnamed" : player.Name;
                player.Health = string.IsNullOrEmpty(player.Health) ? "10" : player.Health;
                player.Level = player.Level == 0 ? 1 : player.Level;
                player.Score = player.Score == 0 ? 0 : player.Score;
                player.Proficiency = player.Proficiency == 0 ? 2 : player.Proficiency;

                // Log the properties
                Console.WriteLine($"Class: {player.Class}");
                Console.WriteLine($"Experience: {player.Experience}");
                Console.WriteLine($"Attributes: {player.Attributes}");
                Console.WriteLine($"AttackUnarmed: {player.AttackUnarmed}");
                Console.WriteLine($"AttackList: {player.AttackList}");
                Console.WriteLine($"DefenseArmor: {player.DefenseArmor}");
                Console.WriteLine($"Health: {player.Health}");
                Console.WriteLine($"HealthDice: {player.HealthDice}");
                Console.WriteLine($"Name: {player.Name}");

                result.Add($"{player.Name.Split("_")[0]}", new ActorPlayer(player));
            }
            catch (Exception ex) {
                Console.WriteLine($"Error processing player with ID {player.Id}: {ex.Message}");
            }
        }

        return result;
    }

        //  GetMethod - Get Player Name
        public string GetPlayerName(int pId) {
            ActorPlayer? player = GetPlayerById(pId);
            
            if (player != null) {
                return player.Name;
            }

            return "";
        }

        //  GetMethod - Get Player Article
        public string GetPlayerArticle(int pId) {
            ActorPlayer? player = GetPlayerById(pId);
            
            if (player != null) {
                if (player.Name != null && player.Name != "") {
                    string first = player.Name.Substring(0, 1);
                    return (first == "a" || first == "e" || first == "i" || first == "o" || first == "u") ? "an" : "a";
                }
                return "ERROR (GetPlayerArticle): Player Name Not Valid";
            }

            return "";
        }

        //  GetMethod - Get Player Defense
        public string GetPlayerDefense(int pId) {
            ActorPlayer? player = GetPlayerById(pId);
            
            if (player != null) {
                return player.DefenseArmor;
            }

            return "";
        }

        //  GetMethod - Get All Players Name
        public List<string> GetAllPlayersName() {
            List<ActorPlayer> players = context.Players.ToList() ?? new List<ActorPlayer>();
            List<string> playerNames = new List<string>();
        
            foreach(ActorPlayer player in players) {
                playerNames.Add(player.Name);
            }
        
            return playerNames;
        }

        //  GetMethod - Get Player Attributes
        public string? GetPlayerAttributes(int pUserId, int pId) {
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
                player.Health = $"{player.HealthCurr}/{player.HealthBase}";
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
                player.AttackList = pAttack;
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

        //  PutMethod - Player Gain Experience
        public ActorPlayer? PlayerGainExp(int pId, int pAmt) {
            ActorPlayer? tempPlayer = GetPlayerById(pId);

            if (tempPlayer != null) {
                ActorPlayer player = new ActorPlayer(tempPlayer);

                //  Increase Experience
                player.ExpCurr += pAmt;

                //  Level up
                if (player.ExpCurr >= player.ExpReq) {
                    //  Increase Level and Exp Req
                    player.Level++;
                    player.ExpReq *= 2;

                    //  Increase Health
                    string[] diceArr = player.HealthDice.Split("d");
                    int diceNum = int.Parse(diceArr[0]);
                    int diceAmt = int.Parse(diceArr[1]);
                    player.HealthDice = (diceNum+1) + "d" + diceAmt;

                    Random rand = new Random();
                    player.HealthBase += rand.Next(0, diceAmt) + 1;
                    player.HealthCurr = 0 + player.HealthBase;
                }

                UpdatePlayer(player);
            }

            return tempPlayer;
        }

        //  PutMethod - Player Restores Health
        public ActorPlayer? PlayerRestoresHealth(int pId, int pAmt) {
            ActorPlayer? tempPlayer = GetPlayerById(pId);

            if (tempPlayer != null) {
                ActorPlayer player = new ActorPlayer(tempPlayer);

                //  Increase Health
                player.HealthCurr = Math.Min(player.HealthCurr + pAmt, player.HealthBase);

                UpdatePlayer(player);
            }

            return tempPlayer;
        }

        //  PutMethod - Update Player
        public void UpdatePlayer(ActorPlayer pPlayer) {
            ActorPlayer? player = GetPlayer(pPlayer);

            if (player != null) {
                player.Attributes = "" + pPlayer.Attributes;

                player.AttackUnarmed = "" + pPlayer.AttackUnarmed;
                player.AttackList = "" + pPlayer.AttackList;

                player.DefenseArmor = "" + pPlayer.DefenseArmor;

                player.HealthDice = "" + pPlayer.HealthDice;

                if (pPlayer.HealthBase != 0) {
                    player.Health = "" + pPlayer.HealthCurr + "/" + pPlayer.HealthBase;
                }
                else {
                    player.Health = "" + pPlayer.Health;
                }

                player.Name = "" + pPlayer.Name;

                player.Proficiency = 0 + pPlayer.Proficiency;
                player.Experience = $"{pPlayer.ExpCurr}/{pPlayer.ExpReq}";

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
    
        //--------------------------------------------------
        //  Combat Methods
        //--------------------------------------------------
        //  GetMethod - Get Combat
        public Combat? GetCombat() {
            List<Combat> combatList = context.Combats.ToList() ?? new List<Combat>();

            if (combatList.Count > 0) { 
                return combatList[0];
            }

            return null;
        }

        //  GetMethod - Get Combat Enemy Id
        public int GetCombatEnemyId(int pCombatId){
            Combat? combat = GetCombatById(pCombatId);

            if (combat == null) {
                return -1;
            }

            else {
                ActorEnemy? enemy = GetEnemyById(combat.ActorEnemyId);

                if (enemy == null) {
                    return -1; 
                }
                
                else {
                    return enemy.Id;
                }
            }
        }

        //  GetMethod - Get Combat Enemy Name
        public string GetCombatEnemyName(string pCombatId) {
            Combat? combat = GetCombatById(int.Parse(pCombatId));

            if (combat == null) {
                return "ERROR (GetCombatEnemyName): Combat doesn't exist";
            }

            else {
                ActorEnemy? enemy = GetEnemyById(combat.ActorEnemyId);

                if (enemy == null) {
                    return "ERROR (GetCombatEnemyName): Enemy doesn't exist"; 
                }
                
                else {
                    return enemy.Name;
                }
            }
        }

        //  GetMethod - Get Combat Enemy Health
        public string GetCombatEnemyHealth(int pCombatId) {
            Combat? combat = GetCombatById(pCombatId);

            if (combat == null) {
                return "ERROR (GetCombatEnemyHealth): Combat doesn't exist";
            }

            else {
                ActorEnemy? enemy = GetEnemyById(combat.ActorEnemyId);

                if (enemy == null) {
                    return "ERROR (GetCombatEnemyHealth): Enemy doesn't exist"; 
                }
                
                else {
                    return enemy.Health;
                }
            }
        }

        //  GetMethod - Get Combat Enemy PAC
        public string GetCombatEnemyPAC(int pCombatId) {
            Combat? combat = GetCombatById(pCombatId);

            if (combat == null) {
                return "ERROR (GetCombatEnemyPAC): Combat doesn't exist";
            }

            else {
                return combat.EnemyACRange;
            }
        }

        //  GetMethod - Get Combat Player Name
        public string GetCombatPlayerName(int pCombatId) {
            Combat? combat = GetCombatById(pCombatId);

            if (combat == null) {
                return "ERROR (GetCombatPlayerName): Combat doesn't exist";
            }

            else {
                ActorPlayer? player = GetPlayerById(combat.ActorPlayerId);

                if (player == null) {
                    return "ERROR (GetCombatPlayerName): Enemy doesn't exist"; 
                }
                
                else {
                    return player.Name;
                }
            }
        }

        //  GetMethod - Get Combat Player Health
        public string GetCombatPlayerHealth(int pCombatId) {
            Combat? combat = GetCombatById(pCombatId);

            if (combat == null) {
                return "ERROR (GetCombatPlayerHealth): Combat doesn't exist";
            }

            else {
                ActorPlayer? player = GetPlayerById(combat.ActorPlayerId);

                if (player == null) {
                    return "ERROR (GetCombatPlayerHealth): Player doesn't exist"; 
                }
                
                else {
                    return player.Health;
                }
            }
        }

        //  GetMethod - Get Combat Player AC
        public string GetCombatPlayerAC(int pCombatId) {
            Combat? combat = GetCombatById(pCombatId);

            if (combat == null) {
                return "ERROR (GetCombatPlayerAC): Combat doesn't exist";
            }

            else {
                ActorPlayer? tempPlayer = GetPlayerById(combat.ActorPlayerId);

                if (tempPlayer == null) {
                    return "ERROR (GetCombatPlayerHealth): Player doesn't exist"; 
                }
                
                else {
                    ActorPlayer player = new ActorPlayer(tempPlayer);
                    return "" + player.Def_AC;
                }
            }
        }

        //  GetMethod - Get Combat By Id
        public Combat? GetCombatById(int pId) {
            return context.Combats.FirstOrDefault(c => c.Id == pId);
        }

        //  GetMethod - Create Combat
        public int? CreateCombat(int pPlayerId) {
            var existingPlayer = GetPlayerById(pPlayerId);
            var enemyRandom = GetRandomEnemy();

            if (existingPlayer != null && enemyRandom != null) {
                Combat? existingCombat = GetCombat();

                if (existingCombat != null) {
                    existingCombat.ActorPlayerId = pPlayerId;
                    existingCombat.ActorPlayerId = pPlayerId;
                    existingCombat.ActorEnemyId = enemyRandom.Id;
                    existingCombat.EnemyACLow = -999;
                    existingCombat.EnemyACHigh = 999;
                    existingCombat.EnemyACRange = "???";

                    context.Entry(existingCombat).State = EntityState.Modified;
                    context.SaveChanges();
                }

                else {
                    existingCombat = new Combat() {
                        ActorPlayerId = pPlayerId,
                        ActorEnemyId = enemyRandom.Id,
                        EnemyACLow = -999,
                        EnemyACHigh = 999,
                        EnemyACRange = "???"
                    };
                    context.Combats.Add(existingCombat);
                    context.SaveChanges();
                }

                return context.Combats.ToList()[0].Id;
            }

            return null;
        }

        //  PutMethod - Player Attacks
        public string PlayerAttacks(int pCombatId) {
            Combat? combat = GetCombatById(pCombatId);

            //  Combat is null
            if (combat == null) {
                return "ERROR (PlayerAttacks): Combat Not Found";
            }

            //  Combat isn't null
            else {
                //  Pull Combat Player and Enemy
                ActorPlayer? tempPlayer = GetPlayerById(combat.ActorPlayerId);
                ActorEnemy? tempEnemy = GetEnemyById(combat.ActorEnemyId);

                //  If either player or enemy is null
                if (tempPlayer == null || tempEnemy == null) {
                    return "ERROR (PlayerAttacks): Player or Enemy is null";
                }

                //  Neither player nor enemy is null
                else {
                    //  Build player and enemy from database info
                    ActorPlayer player = new ActorPlayer(tempPlayer);
                    ActorEnemy enemy = new ActorEnemy(tempEnemy);

                    //  Player attacks, also deals damage to enemy
                    string attackResult = player.Attack(new Random(), enemy);
                    string[] attackArr = attackResult.Split("/n");
                    if (attackArr[0].Contains('_')) {

                    }

                    int toHit = attackArr[0].Contains('_') ? int.Parse(attackArr[0].Split('_')[1]) : -999;


                    UpdateEnemy(enemy);

                    //  Update enemy ac range
                    UpdateEnemyPAC(combat, toHit);

                    return attackResult;
                }
            }
        }

        //  SubMethod of PlayerAttacks - Update Enemy PAC
        public void UpdateEnemyPAC(Combat pCombat, int pToHit) {
            ActorEnemy? tempEnemy = GetEnemyById(pCombat.ActorEnemyId);

            if (tempEnemy != null) {
                ActorEnemy enemy = new ActorEnemy(tempEnemy);

                if (pToHit != -999) {
                    if (pToHit >= enemy.Def_AC) {
                        //  Player hasn't hit before and has missed, increment enemy_ACLow to switch to range
                        if (pCombat.EnemyACHigh == 999 && pCombat.EnemyACLow != -999) {
                            pCombat.EnemyACLow++;
                        }
                        pCombat.EnemyACHigh = (pToHit < pCombat.EnemyACHigh) ? pToHit : pCombat.EnemyACHigh;
                    }

                    else {
                        pCombat.EnemyACLow = (pToHit > pCombat.EnemyACLow) ? pToHit : pCombat.EnemyACLow;
                    }
                }

                //  Actual AC is known
                if (pCombat.EnemyACHigh == pCombat.EnemyACLow) {
                    pCombat.EnemyACRange = "" + pCombat.EnemyACHigh;
                }

                //  AC range is known
                else if (pCombat.EnemyACHigh != 999 && pCombat.EnemyACLow != -999) {
                    pCombat.EnemyACRange = pCombat.EnemyACLow + "-" + pCombat.EnemyACHigh;
                }

                //  AC low is known
                else if (pCombat.EnemyACLow != -999) {
                    pCombat.EnemyACRange = ">" + pCombat.EnemyACLow;
                }

                //  AC high is known
                else if (pCombat.EnemyACHigh != 999) {
                    pCombat.EnemyACRange = "<" + pCombat.EnemyACHigh;
                }

                context.SaveChanges();
            }
        }

        //  PutMethod - Enemy Attacks
        public string EnemyAttacks(int pCombatId) {
            Combat? combat = GetCombatById(pCombatId);

            //  Combat is null
            if (combat == null) {
                return "ERROR (PlayerAttacks): Combat Not Found";
            }

            //  Combat isn't null
            else {
                //  Pull Combat Player and Enemy
                ActorPlayer? tempPlayer = GetPlayerById(combat.ActorPlayerId);
                ActorEnemy? tempEnemy = GetEnemyById(combat.ActorEnemyId);

                //  If either player or enemy is null
                if (tempPlayer == null || tempEnemy == null) {
                    return "ERROR (PlayerAttacks): Player or Enemy is null";
                }

                //  Neither player nor enemy is null
                else {
                    //  Build player and enemy from database info
                    ActorPlayer player = new ActorPlayer(tempPlayer);
                    ActorEnemy enemy = new ActorEnemy(tempEnemy);

                    //  Player attacks, also deals damage to enemy
                    string attackResult = enemy.Attack(new Random(), player);
                    UpdatePlayer(player);
                    //Console.WriteLine("Player: " + player.HealthCurr + "/" + player.HealthBase);

                    return attackResult;
                }
            }
        }

        //  PutMethod - Combat Ending
        public string CombatEnding(int pCombatId, int pActionId) {
            Combat? combat = GetCombatById(pCombatId);

            //  Combat is null
            if (combat == null) {
                return "ERROR (PlayerAttacks): Combat Not Found";
            }

            //  Combat isn't null
            else {
                if (pActionId == 2) {
                    ActorPlayer? tempPlayer = GetPlayerById(combat.ActorPlayerId);

                    //  Player isn't null
                    if (tempPlayer != null) {
                        ActorPlayer player = new ActorPlayer(tempPlayer);
                        player.RestoreHealth();
                        UpdatePlayer(player);
                    }
                }

                return "Combat ended";
            }
        }
    }
}