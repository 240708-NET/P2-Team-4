using Project2.Models.Actor;

namespace Project2.App.Main {
    public class ManagerCave {
        //  ~Reference Variables
        public ManagerGame RefMGame { get; private set; }

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
                switch(RefMGame.Client.GetStringAsync($"/getCaveArea/{player.Id}").Result) {
                    case "Nothing":
                        Console.WriteLine("You encountered nothing and had a rest. Recover 2 hp");
                        break;

                    case "Combat":
                        refMCombat.CombatActive = true;
                        refMCombat.CombatId = -1;

                        refMCombat.CombatSetup();
                        while(refMCombat.CombatId == -1) {
                            Thread.Sleep(400);
                        }
                        refMCombat.CombatLoop();
                        break;

                    case "Experience":
                        Console.WriteLine("You enter the room and see a treasure chest full of gold. Gain 100 exp.");
                        break;
                }

                string action = Console.ReadLine() ?? "";
                if (action == "fquit") {
                    GameActive = false;
                    RefMGame.Force_Quit = true;
                }
            }
        }

        //  MainMethod - Introduction
        public void Introduction() {
            RefMGame.WriteLine("Welcome to The Cave!", 75);
            RefMGame.WriteLine("", 0);

            RefMGame.WriteLine("You will take the role of an adventurer exploring", 25);
            RefMGame.WriteLine("the depths of this cave system, facing fearsome", 25);
            RefMGame.WriteLine("foes and finding glorious treasure.", 25);
            RefMGame.WriteLine("", 0);

            RefMGame.WriteLine("Do you have what it takes?", 25);
            Console.ReadLine();

            refMActor.CharacterCreation();
            
            while(player == null) {
                Thread.Sleep(400);
            }
        }
    }
}