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

        public int CombatId;
        public bool CombatActive;

        //  Combat Variables
        private string combatUI_Solid => $"+{new string('-', 29)}+";
        private string combatUI_Empty => $"+{new string(' ', 29)}+";

        //  Player Variables
        private ActorPlayer player => RefMGame.M_Actor.Player;

        //  Constructor
        public ManagerCombat(ManagerGame pRef) {
            //  Setup ~Reference
            RefMGame = pRef;

            //  Setup Combat
            CombatId = -1;
            CombatActive = false;
        }

        //  MainMethod - Combat Setup
        public async void CombatSetup() {
            var combatResponse = await RefMGame.Client.PostAsync($"createCombat/{player.Id}", null);
            string combatStr = await combatResponse.Content.ReadAsStringAsync();
            CombatId = JsonConvert.DeserializeObject<int>(combatStr);
        }

        //  MainMethod - Combat Loop
        public void CombatLoop() {
            //  Encounter initiated
            int enemyId = int.Parse(RefMGame.Client.GetStringAsync($"/getCombatEnemyId/{CombatId}").Result);
            string enemyName = RefMGame.Client.GetStringAsync($"/getCombatEnemyName/{CombatId}").Result;
            enemyName = enemyName.Split("_")[0];

            string enemyArt = RefMGame.Client.GetStringAsync($"/getEnemyArticle/{enemyId}").Result;
            Console.WriteLine($"Player has encountered {enemyArt} {enemyName}! Combat initiated!");
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
                        enemyId = int.Parse(RefMGame.Client.GetStringAsync($"/getCombatEnemyId/{CombatId}").Result);
                        RefMGame.Client.PutAsync($"/resetEnemyHealth/{enemyId}", null);

                        var expResponse = RefMGame.Client.PutAsync($"/playerGainExp/{player.Id}/{200}", null);

                        //RefMGame.WriteLine($"Player exp: {player.ExpStr}", 25);
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

                            string attackResponse = RefMGame.Client.GetStringAsync($"/playerAttacks/{CombatId}").Result;

                            if (attackResponse != "-999") {
                                string[] attackArr = attackResponse.Split("/n");

                                string[] hitArr = attackArr[0].Split("_");
                                Console.WriteLine(hitArr[0]);

                                for (int i = 1; i < attackArr.Length; i++) {
                                    Console.WriteLine(attackArr[i]);
                                }
                            }
                            break;

                        //  Invalid input
                        default:
                            actionCount++;
                            break;
                    }
                }

                if (actionValid == false) {
                    RefMGame.WriteLine("Reset Display", 25);
                    RefMGame.WriteLine("", 0);

                    DisplayCombat_Status();
                }
            }
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

                            string actionResponse = RefMGame.Client.GetStringAsync($"/combatEnds/{CombatId}/{2}").Result;
                            break;

                        //  Invalid input
                        default:
                            actionCount++;
                            break;
                    }
                }

                if (actionValid == false) {
                    RefMGame.WriteLine("Reset Display", 25);
                    RefMGame.WriteLine("", 0);

                    DisplayCombat_Status();
                }
            }
        }
    }
}