using Project2.Models.Actor;

namespace Project2.App.Main {
    public class ManagerCombat {
        //  ~Reference Variables
        public ManagerGame RefMGame { get; private set; }
        private Random RefRand => RefMGame.Rand;

        private ManagerActor refMActor => RefMGame.M_Actor;
        private ManagerCave refMCave => RefMGame.M_Cave;

        //  Combat Variables
        private bool combatActive;
        private string combatUI_Solid => $"+{new string('-', 29)}+";
        private string combatUI_Empty => $"+{new string(' ', 29)}+";

        //  Enemy Variables
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

        //  Player Variables
        private ActorPlayer player => RefMGame.M_Actor.Player;

        private string player_Name => $"+ {player.Name}{new string(' ', (27 - player.Name.Length))} +";
        private string player_Health => $"+ HP: {player.HealthStr}{new string(' ', (23 - player.HealthStr.Length))} +";
        private string player_AC => $"+ AC: {player.Def_AC}{new string(' ', (23 - player.Def_AC.ToString().Length))} +";

        //  Constructor
        /// <summary>
        /// Manager that handles all combat encounters
        /// </summary>
        /// <param name="pRef">Reference to Game Manager</param>
        public ManagerCombat(ManagerGame pRef) {
            //  Setup ~Reference
            RefMGame = pRef;

            //  Setup Combat
            combatActive = true;

            //  Setup Enemy
            enemy = new ActorEnemy(RefMGame.M_Actor.GetEnemy());

            enemy_ACLow = -999;
            enemy_ACHigh = 999;
            enemy_ACRange = "???";
        }

        //  MainMethod - Combat Setup
        public void CombatSetup() {
            combatActive = true;

            //  Setup Enemy
            enemy = new ActorEnemy(RefMGame.M_Actor.GetEnemy());

            enemy_ACLow = -999;
            enemy_ACHigh = 999;
            enemy_ACRange = "???";
        }

        //  MainMethod - Combat Loop
        public void CombatLoop() {
            string enemyName = ((enemy.Proper == false) ? enemy.Name.ToLower() : enemy.Name).Split("_")[0];
            string enemyStr = ((enemy.Article != "") ? (enemy.Article + " ") : "") + enemyName;

            //  Encounter initiated
            Console.WriteLine($"Player has encountered {enemyStr}! Combat initiated!");
            string action = Console.ReadLine() ?? "";

            //  Initial action check
            switch(action) {
                //  Force Quit action
                case "fquit":
                    combatActive = false;
                    refMCave.GameActive = false;
                    RefMGame.Force_Quit = true;
                    break;
            }

            while (combatActive == true) {
                DisplayCombat_Status();

                //  If player is still active, handle their input
                if (player.HealthCurr > 0) {
                    PlayerAction_Combat();
                }

                //  If player is dead, end combat and exit
                else {
                    RefMGame.WriteLine($"{player.Name} has died to {enemyStr}", 75);
                    combatActive = false;
                    refMCave.GameActive = false;
                    RefMGame.Force_Quit = true;
                }

                if (combatActive == true) {
                    //  If enemy is still active, handle their action
                    if (enemy.HealthCurr > 0) {
                        EnemyAction_Combat();
                    }

                    //  If enemy is dead, end combat and exit
                    else {
                        RefMGame.WriteLine($"{enemy.Name} has died! Player gains 200 exp.", 25);
                        player.GainExperience(200);
                        RefMGame.WriteLine($"Player exp: {player.ExpStr}", 25);

                        if (player.ExpCurr >= player.ExpReq) {
                            RefMGame.WriteLine($"Player levels up from {player.Level} to {(player.Level+1)}!", 25);
                            refMActor.ActorLevelUp(player);
                        }
                        Console.WriteLine("");

                        DisplayCombat_Ending();
                        PlayerAction_Ending();
                    }
                }
            }
        }
        
        //  SubMethod of Combat Loop - Display Combat Status
        private void DisplayCombat_Status() {
            Console.WriteLine(combatUI_Solid);

            Console.WriteLine(enemy_Name);
            Console.WriteLine(enemy_Health);
            Console.WriteLine(enemy_AC);

            Console.WriteLine(combatUI_Empty);

            Console.WriteLine(player_Name);
            Console.WriteLine(player_Health);
            Console.WriteLine(player_AC);

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
            Console.WriteLine($"You've defeated {((enemy.Proper == false) ? "the " + enemy.Name.ToLower() : enemy.Name)}. Do you wish to press on or rest awhile?");
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
                            combatActive = false;
                            refMCave.GameActive = false;
                            RefMGame.Force_Quit = true;
                            break;
                        
                        //  Player attack
                        case "1":
                            actionCount = 5;
                            actionValid = true;

                            playerToHit = player.Attack(RefRand, enemy);
                            PAC_UpdateEnemyAC(playerToHit);
                            break;

                        //  Invalid input
                        default:
                            actionCount++;
                            break;
                    }
                }

                if (actionValid == false) {
                    Console.WriteLine("Reset Display");
                    Console.WriteLine("");

                    DisplayCombat_Status();
                }
            }
        }

        //  SubMethod of PlayerAction_Combat - Update Enemy AC
        private void PAC_UpdateEnemyAC(int pToHit) {
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
        }

        //  SubMethod of CombatLoop - Player Action Ending
        private void PlayerAction_Ending() {
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
                            combatActive = false;

                            refMCave.GameActive = false;
                            RefMGame.Force_Quit = true;
                            break;
                        
                        //  Player presses on
                        case "1":
                            actionCount = 5;
                            actionValid = true;
                            combatActive = false;
                            break;

                        //  Player rests
                        case "2":
                            actionCount = 5;
                            actionValid = true;
                            combatActive = false;

                            player.RestoreHealth();
                            break;

                        //  Invalid input
                        default:
                            actionCount++;
                            break;
                    }
                }

                if (actionValid == false) {
                    Console.WriteLine("Reset Display");
                    Console.WriteLine("");

                    DisplayCombat_Status();
                }
            }
        }
        
        //  SubMethod of CombatLoop - Enemy Action Combat
        private void EnemyAction_Combat() {
            enemy.Attack(RefRand, player);
        }
    }
}