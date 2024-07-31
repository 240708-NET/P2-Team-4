using Project2.Models.Actor;

namespace Project2.App.Main {
    public class ManagerCave {
        //  ~Reference Variables
        public ManagerGame RefMGame { get; private set; }
        private Random refRand => RefMGame.Rand;

        private ManagerActor refMActor => RefMGame.M_Actor;
        private ActorPlayer player => refMActor.Player;

        private ManagerCombat refMCombat => RefMGame.M_Combat;

        //  Game Variables
        public bool GameActive;

        //  Constructor
        public ManagerCave(ManagerGame pRef) {
            //  Setup ~Reference
            RefMGame = pRef;

            //  Setup Game
            GameActive = true;
        }

        //  MainMethod - Game Loop
        public void GameLoop() {
            Introduction();

            while (GameActive == true) {
                ExploreArea();
                Console.ReadLine();
            }
        }

        //  MainMethod - Introduction
        public void Introduction() {
            RefMGame.WriteLine("Welcome to The Cave!", 75);
            Console.WriteLine("");

            RefMGame.WriteLine("You will take the role of an adventurer exploring", 25);
            RefMGame.WriteLine("the depths of this cave system, facing fearsome", 25);
            RefMGame.WriteLine("foes and finding glorious treasure.", 25);
            Console.WriteLine("");

            RefMGame.WriteLine("Do you have what it takes?", 25);
            Console.ReadLine();

            refMActor.CharacterCreation();
        }

        //  MainMethod - Explore Area
        public void ExploreArea() {
            int chance = refRand.Next(0, 100)+1;

            //  20% chance for nothing
            if (chance <= 20) {
                RefMGame.WriteLine("You encountered nothing and had a rest. Recover 2 hp.", 25);
                player.RestoreHealth(2);
                
            }

            //  60% chance for combat encounter
            else if (chance <= 80) {
                refMCombat.CombatSetup();
                refMCombat.CombatLoop();
            }

            //  20% chance for treasure room
            else {
                RefMGame.WriteLine("You enter the room and see a treasure chest full of gold. Gain 100 exp.", 25);
                player.GainExperience(100);
                RefMGame.WriteLine($"Player exp: {player.ExpStr}", 25);

                if (player.ExpCurr >= player.ExpReq) {
                    RefMGame.WriteLine($"Player levels up from {player.Level} to {(player.Level+1)}!", 25);
                    refMActor.ActorLevelUp(player);
                }
            }
        }
    }
}