using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Project2.Models.Actor;
using Project2.Models.Combats;

namespace Project2.App.Main {
    public class ManagerCombat {
        //  ~Reference Variables
        public ManagerGame RefMGame { get; private set; }

        private ManagerActor refMActor => RefMGame.M_Actor;
        private ManagerCave refMCave => RefMGame.M_Cave;

        private int CombatId;
        public bool CombatActive;

        //  Combat Variables
        private string combatUI_Solid => $"+{new string('-', 29)}+";
        private string combatUI_Empty => $"+{new string(' ', 29)}+";

        //  Enemy Variables
        /*
        private ActorEnemy enemy;
        public ActorEnemy Enemy {
            get { return enemy; }
            set {
                enemy = value;

                enemy_ACLow = -999;
                enemy_ACHigh = 999;
                enemy_ACRange = "???";
            }
        }
        private string enemy_Name => $"+ {new string(' ', (27 - enemy.Name.Length))}{enemy.Name} +";
        private string enemy_Health => $"+ {new string(' ', (23 - enemy.HealthStr.Length))}HP: {enemy.HealthStr} +";

        private int enemy_ACLow;
        private int enemy_ACHigh;
        private string enemy_ACRange;
        private string enemy_AC => $"+ {new string(' ', (23 - enemy_ACRange.Length))}AC: {enemy_ACRange} +";
        */

        //  Player Variables
        private ActorPlayer player => RefMGame.M_Actor.Player;

        //private string player_Name => $"+ {player.Name}{new string(' ', (27 - player.Name.Length))} +";
        //private string player_Health => $"+ HP: {player.Health}{new string(' ', (23 - player.Health.Length))} +";
        //private string player_AC => $"+ AC: {player.Def_AC}{new string(' ', (23 - player.Def_AC.ToString().Length))} +";

        //  Constructor
        /// <summary>
        /// Manager that handles all combat encounters
        /// </summary>
        /// <param name="pRef">Reference to Game Manager</param>
        public ManagerCombat(ManagerGame pRef) {
            //  Setup ~Reference
            RefMGame = pRef;

            //  Setup Combat
            CombatActive = true;

            //  Setup Enemy
            //enemy = new ActorEnemy();

            //enemy_ACLow = -999;
            //enemy_ACHigh = 999;
            //enemy_ACRange = "???";
        }

        //  MainMethod - Combat Setup
        public async void CombatSetup() {
            //  Setup Enemy
            //enemy = new ActorEnemy(RefMGame.M_Actor.GetEnemy());
            //string enemyStr = RefMGame.Client.GetStringAsync("getRandomEnemy").Result;
            //enemy = new ActorEnemy(JsonConvert.DeserializeObject<ActorEnemy>(enemyStr));

            //Console.WriteLine(player.Id);

            /*
            //  Create class
            var classJson = JsonContent.Create<string>("Fighter");
            var classResponse = await refMGame.Client.PostAsync($"createPlayerClass/{player.Id}", classJson);
            */

            var combatResponse = await RefMGame.Client.PostAsync($"createCombat/{player.Id}", null);
            string combatStr = await combatResponse.Content.ReadAsStringAsync();
            CombatId = JsonConvert.DeserializeObject<int>(combatStr);
        }

        //  MainMethod - Combat Loop
        public void CombatLoop() {
            //string enemyName = ((enemy.Proper == false) ? enemy.Name.ToLower() : enemy.Name).Split("_")[0];
            //string enemyStr = ((enemy.Article != "") ? (enemy.Article + " ") : "") + enemyName;

            //  Encounter initiated
            //Console.WriteLine($"Player has encountered {enemyStr}! Combat initiated!");
            string action = Console.ReadLine() ?? "";

            //  Initial action check
            switch(action) {
                //  Force Quit action
                case "fquit":
                    CombatActive = false;
                    refMCave.GameActive = false;
                    RefMGame.Force_Quit = true;
                    break;
            }

            while (CombatActive == true) {
                DisplayCombat_Status();

                //  If player is still active, handle their input
                //bool playerAction = false;
                string playerStr = RefMGame.Client.GetStringAsync($"/getCombatPlayerHealth/{CombatId}").Result;
                string[] playerHealthArr = playerStr.Split("/");
                int playerHealthCurr = int.Parse(playerHealthArr[0]);

                if (playerHealthCurr > 0) {
                    PlayerAction_Combat();
                }

                //  If player is dead, end combat and exit
                else {
                    //RefMGame.WriteLine($"{player.Name} has died to {enemyStr}", 75);
                    CombatActive = false;
                    refMCave.GameActive = false;
                    RefMGame.Force_Quit = true;
                }

                if (CombatActive == true) {
                    //  If enemy is still active, handle their action
                    string enemyStr = RefMGame.Client.GetStringAsync($"/getCombatEnemyHealth/{CombatId}").Result;
                    string[] enemyHealthArr = enemyStr.Split("/");
                    int enemyHealthCurr = int.Parse(enemyHealthArr[0]);
                    if (enemyHealthCurr > 0) {
                        //EnemyAction_Combat();

                        string attackResponse = RefMGame.Client.GetStringAsync($"/enemyAttacks/{CombatId}").Result;
                        if (attackResponse != "-999") {
                            string[] attackArr = attackResponse.Split("/n");
                            
                            string[] hitArr = attackArr[0].Split("_");
                            Console.WriteLine(hitArr[0]);
                            
                            for (int i = 1; i < attackArr.Length; i++) {
                                Console.WriteLine(attackArr[i]);
                            }
                        }
                    }

                    //  If enemy is dead, end combat and exit
                    else {
                        //RefMGame.WriteLine($"{enemy.Name} has died! Player gains 200 exp.", 25);
                        string enemyId = RefMGame.Client.GetStringAsync($"/getCombatEnemyId/{CombatId}").Result;
                        RefMGame.Client.PutAsync($"/resetEnemyHealth/{int.Parse(enemyId)}", null);

                        var expResponse = RefMGame.Client.PutAsync($"/playerGainExp/{player.Id}/{200}", null);

                        //player.GainExperience(200);
                        //RefMGame.WriteLine($"Player exp: {player.ExpStr}", 25);

                        /*
                        if (player.ExpCurr >= player.ExpReq) {
                            RefMGame.WriteLine($"Player levels up from {player.Level} to {(player.Level+1)}!", 25);
                            refMActor.ActorLevelUp(player);
                        }
                        */
                        //Console.WriteLine("");

                        DisplayCombat_Ending();
                        PlayerAction_Ending();
                    }
                }
            }
        }
        
        //  SubMethod of Combat Loop - Display Combat Status
        private void DisplayCombat_Status() {
            Console.WriteLine(combatUI_Solid);

            //  Enemy Name
            string enemyName = RefMGame.Client.GetStringAsync($"/getCombatEnemyName/{CombatId}").Result;
            enemyName = enemyName.Split("_")[0];
            Console.WriteLine($"+ {new string(' ', 27 - enemyName.Length)}{enemyName} +");

            //  Enemy Health
            string enemyHealth = RefMGame.Client.GetStringAsync($"/getCombatEnemyHealth/{CombatId}").Result;
            Console.WriteLine($"+ {new string(' ', 23 - enemyHealth.Length)}HP: {enemyHealth} +");

            //  Enemy PAC
            string enemyPAC = RefMGame.Client.GetStringAsync($"/getCombatEnemyPAC/{CombatId}").Result;
            Console.WriteLine($"+ {new string(' ', (23 - enemyPAC.Length))}AC: {enemyPAC} +");

            Console.WriteLine(combatUI_Empty);

            //  Player Name
            string playerName = RefMGame.Client.GetStringAsync($"/getCombatPlayerName/{CombatId}").Result;
            Console.WriteLine($"+ {playerName}{new string(' ', (27 - playerName.Length))} +");

            //  Player Health
            string playerHealth = RefMGame.Client.GetStringAsync($"/getCombatPlayerHealth/{CombatId}").Result;
            Console.WriteLine($"+ HP: {playerHealth}{new string(' ', (23 - playerHealth.Length))} +");

            //  Player AC
            string playerAC = RefMGame.Client.GetStringAsync($"/getCombatPlayerAC/{CombatId}").Result;
            Console.WriteLine($"+ AC: {playerAC}{new string(' ', (23 - playerAC.Length))} +");

            Console.WriteLine(combatUI_Solid);
            DisplayCombat_Options();
        }

        //  SubMethod of CombatLoop - Display Combat Options
        private void DisplayCombat_Options() {
            Console.WriteLine($"+  (1) Attack                 +");
            Console.WriteLine(combatUI_Solid);
        }

        //  SubMethod of CombatLoop - Display Combat Ending
        private void DisplayCombat_Ending() {
            string combatStr = RefMGame.Client.GetStringAsync($"/getCombatById/{CombatId}").Result;
            Combat combat = JsonConvert.DeserializeObject<Combat>(combatStr);

            int enemyId = combat.ActorEnemyId;
            string enemyStr = RefMGame.Client.GetStringAsync($"/GetEnemyById/{enemyId}").Result;
            ActorEnemy enemy = JsonConvert.DeserializeObject<ActorEnemy>(enemyStr);

            string[] enemyNameArr = enemy.Name.Split("_");

            Console.WriteLine($"You've defeated {((enemyNameArr[1] == "False") ? "the " + enemyNameArr[0].ToLower() : enemyNameArr[0])}. Do you wish to press on or rest awhile?");
            Console.WriteLine("(1) Press on  (2) Rest");
        }

        //  SubMethod of CombatLoop - Player Action Combat
        private void PlayerAction_Combat() {
            string action = "";
            int actionCount = 0;
            bool actionValid = false;
            int playerToHit = 0;

            while(actionValid == false) {
                while(actionCount < 5) {
                    //  Get player action
                    action = Console.ReadLine() ?? "";
                    Console.WriteLine("");

                    switch(action) {
                        //  Force Quit action
                        case "fquit":
                            actionCount = 5;
                            actionValid = true;
                            CombatActive = false;
                            refMCave.GameActive = false;
                            RefMGame.Force_Quit = true;
                            break;
                        
                        //  Player attack
                        case "1":
                            actionCount = 5;
                            actionValid = true;

                            //playerToHit = player.Attack(RefRand, enemy);
                            string attackResponse = RefMGame.Client.GetStringAsync($"/playerAttacks/{CombatId}").Result;
                            if (attackResponse != "-999") {
                                string[] attackArr = attackResponse.Split("/n");

                                string[] hitArr = attackArr[0].Split("_");
                                Console.WriteLine(hitArr[0]);

                                for (int i = 1; i < attackArr.Length; i++) {
                                    Console.WriteLine(attackArr[i]);
                                }
                            }
                            //PAC_UpdateEnemyAC(playerToHit);
                            break;

                        //  Invalid input
                        default:
                            actionCount++;
                            break;
                    }
                }

                if (actionValid == false) {
                    //Console.WriteLine("Reset Display");
                    //Console.WriteLine("");

                    //DisplayCombat_Status();
                }
            }
        }

        //  SubMethod of PlayerAction_Combat - Update Enemy AC
        private void PAC_UpdateEnemyAC(int pToHit) {
            /*
            if (pToHit != -999) {
                if (pToHit >= enemy.Def_AC) {
                    //  Player hasn't hit before and has missed, increment enemy_ACLow to switch to range
                    if (enemy_ACHigh == 999 && enemy_ACLow != -999) {
                        enemy_ACLow++;
                    }
                    enemy_ACHigh = (pToHit < enemy_ACHigh) ? pToHit : enemy_ACHigh;
                }

                else {
                    enemy_ACLow = (pToHit > enemy_ACLow) ? pToHit : enemy_ACLow;
                }
            }

            //  Actual AC is known
            if (enemy_ACHigh == enemy_ACLow) {
                enemy_ACRange = "" + enemy_ACHigh;
            }

            //  AC range is known
            else if (enemy_ACHigh != 999 && enemy_ACLow != -999) {
                enemy_ACRange = enemy_ACLow + "-" + enemy_ACHigh;
            }

            //  AC low is known
            else if (enemy_ACLow != -999) {
                enemy_ACRange = ">" + enemy_ACLow;
            }

            //  AC high is known
            else if (enemy_ACHigh != 999) {
                enemy_ACRange = "<" + enemy_ACHigh;
            }
            */
        }

        //  SubMethod of CombatLoop - Player Action Ending
        private async void PlayerAction_Ending() {
            string action = "";
            int actionCount = 0;
            bool actionValid = false;

            while(actionValid == false) {
                while(actionCount < 5) {
                    //  Get player action
                    action = Console.ReadLine() ?? "";
                    Console.WriteLine("");

                    switch(action) {
                        //  Force Quit action
                        case "fquit":
                            actionCount = 5;
                            actionValid = true;
                            CombatActive = false;

                            refMCave.GameActive = false;
                            RefMGame.Force_Quit = true;
                            break;
                        
                        //  Player presses on
                        case "1":
                            actionCount = 5;
                            actionValid = true;
                            CombatActive = false;
                            break;

                        //  Player rests
                        case "2":
                            actionCount = 5;
                            actionValid = true;
                            CombatActive = false;

                            //player.RestoreHealth();
                            string actionResponse = RefMGame.Client.GetStringAsync($"/combatEnds/{CombatId}/{2}").Result;
                            break;

                        //  Invalid input
                        default:
                            actionCount++;
                            break;
                    }
                }

                if (actionValid == false) {
                    //Console.WriteLine("Reset Display");
                    //Console.WriteLine("");

                    //DisplayCombat_Status();
                }
            }
        }
        
        //  SubMethod of CombatLoop - Enemy Action Combat
        private void EnemyAction_Combat() {
            //enemy.Attack(RefRand, player);
        }
    }
}