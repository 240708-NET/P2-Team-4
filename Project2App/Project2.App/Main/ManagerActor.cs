using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Project2.Models.Actor;

namespace Project2.App.Main {
    public class ManagerActor {
        //  ~Reference Variables
        public ManagerGame RefMGame { get; private set; }
        private Random refRand => RefMGame.Rand;

        //  Enemy Variables
        public Dictionary<string, ActorEnemy> D_Enemies { get; private set; }
        private List<string> enemyKeys;

        //  Level Varaibles
        public Dictionary<string, int> D_LevelReqs { get; private set; }

        //  Player Variables
        public ActorPlayer Player { get; private set; }

        //  Constructor
        /// <summary>
        /// Manager that handles all actors in memory
        /// </summary>
        /// <param name="pRef">Reference to Game Manager</param>
        public ManagerActor(ManagerGame pRef) {
            //  Setup ~Reference
            RefMGame = pRef;

            //  Setup Level
            D_LevelReqs = new Dictionary<string, int>() {
                ["1"] = 300,
                ["2"] = 900,
                ["3"] = 2700,
                ["4"] = 6500,
                ["5"] = 14000,

                ["6"] = 23000,
                ["7"] = 34000,
                ["8"] = 48000,
                ["9"] = 64000,
                ["10"] = 85000,

                ["11"] = 100000,
                ["12"] = 120000,
                ["13"] = 140000,
                ["14"] = 165000,
                ["15"] = 195000,

                ["16"] = 225000,
                ["17"] = 265000,
                ["18"] = 305000,
                ["19"] = 355000,
            };

            //  Setup Enemy
            D_Enemies = new Dictionary<string, ActorEnemy>();
            enemyKeys = new List<string>();
            AddEnemies();

            //  Setup Player
            Player = new ActorPlayer();
        }

        //  SubMethod of Constructor - Add Enemies
        /// <summary>
        /// Adds enemies to the d_Enemies dictionary and the key to enemyKeys
        /// </summary>
        private async void AddEnemies() {
            HttpClient client = new HttpClient();
            string str = client.GetStringAsync("http://localhost:5201/getAllEnemies").Result;
            Dictionary<string, ActorEnemy> tempDict = JsonConvert.DeserializeObject<Dictionary<string, ActorEnemy>>(str) ?? new Dictionary<string, ActorEnemy>();

            D_Enemies = new Dictionary<string, ActorEnemy>();
            enemyKeys = new List<string>();

            if (tempDict.Count == 0) {
                CreateEnemies();

                var enemies = JsonContent.Create<Dictionary<string, ActorEnemy>>(D_Enemies);
                var postResponse = await client.PostAsync("http://localhost:5201/createAllEnemies", enemies);
                //Console.WriteLine(JsonConvert.DeserializeObject<Dictionary<string, GameActor>>(await postResponse.Content.ReadAsStringAsync()));
            }

            else {
                D_Enemies = new Dictionary<string, ActorEnemy>();
                enemyKeys = new List<string>();
                foreach(var enemy in tempDict) {
                    D_Enemies.Add(enemy.Key, new ActorEnemy(enemy.Value));
                    enemyKeys.Add(enemy.Key);
                }
            }
        }

        //  SubMethod of Add Enemies - Create Enemies
        private void CreateEnemies() {
            enemyKeys.Add("Goblin");
            D_Enemies.Add("Goblin", new ActorEnemy { 
                Name = "Goblin_False", 
                Proficiency = 2,
                Attributes = "8,14,10,10,8,8",
                HealthDice = "2d6",
                AttackUnarmed = "fists_strikes with their_Melee_0/0_1_bludgeoning",
                AttackList = "scimitar_swings with their_Melee_0/1d6_0_slashing",
                DefenseArmor = "Leather Armor_11+DEX"
            });

            enemyKeys.Add("Orc");
            D_Enemies.Add("Orc", new ActorEnemy { 
                Name = "Orc_False", 
                Proficiency = 2,
                Attributes = "16,12,16,7,11,10",
                HealthDice = "2d8",
                AttackUnarmed = "fists_strikes with their_Melee_0/0_1_bludgeoning",
                AttackList = "greataxe_swings with their_Melee_0/1d12_0_slashing",
                DefenseArmor = "Leather Armor_11+DEX"
            });

            enemyKeys.Add("Giant Spider");
            D_Enemies.Add("Giant Spider", new ActorEnemy { 
                Name = "Giant Spider_False", 
                Proficiency = 2,
                Attributes = "14,16,12,2,11,4",
                HealthDice = "4d10",
                AttackUnarmed = "fangs_bites with their_Melee_0/0_1_bludgeoning",
                AttackList = "",
                DefenseArmor = ""
            });
        }

        //  MainMethod - Character Creation
        public void CharacterCreation() {
            ManagerActor_CC creation = new ManagerActor_CC(this);
            Player = creation.CharacterCreation();
        }

        //  MainMethod - Get Enemy
        /// <summary>
        /// Returns a random enemy from d_Enemies
        /// </summary>
        /// <returns></returns>
        public ActorEnemy GetEnemy() {
            return D_Enemies[enemyKeys[refRand.Next(0, enemyKeys.Count)]];
        }
    
        //  MainMethod - Actor Level Up
        public void ActorLevelUp(ActorPlayer pActor) {
            pActor.Level++;
            pActor.ExpReq = D_LevelReqs[pActor.Level.ToString()];

            //  Increase health
            pActor.HealthDice = $"{pActor.Level}d10";
            RefMGame.WriteLine($"Setting health dice to {pActor.HealthDice}", 25);

            int incr = refRand.Next(1, 10)+1;
            pActor.HealthBase += incr;
            pActor.HealthCurr = 0 + pActor.HealthBase;
            RefMGame.WriteLine($"Increasing max health by {incr}", 25);
        }
    }
}