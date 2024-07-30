using Project2.Models.Actor;

namespace Project2.App.Main {
    public class ManagerGame {
        //  ~Reference Variables
        public Random Rand;

        //  End Variables
        public bool Force_Quit;

        //  Manager Variables
        public ManagerActor M_Actor { get; private set; }
        public ManagerCave M_Cave { get; private set; }
        public ManagerCombat M_Combat { get; private set; }

        //  Constructor
        /// <summary>
        /// Manager that connects all parts of game logic
        /// </summary>
        public ManagerGame() {
            //  Setup ~Reference
            Rand = new Random();

            //  Setup Managers
            M_Actor = new ManagerActor(this);
            M_Cave = new ManagerCave(this);
            M_Combat = new ManagerCombat(this);
        }

        //  MainMethod - Play Game
        /// <summary>
        /// Main Game Method
        /// </summary>
        public void PlayGame() {
            //  Game logic will loop until player force quits
            while(Force_Quit == false) {
                M_Cave.GameLoop();
            }
        }

        //  MainMethod - Write Text
        /// <summary>
        /// Slowly writes text to the console [REMAINS IN LINE]
        /// </summary>
        /// <param name="pText">Text to print</param>
        /// <param name="pSleep">Time delay between characters</param>
        public void WriteText(string pText, int pSleep) {
            for(int i = 0; i < pText.Length; i++) {
                Console.Write(pText.Substring(i, 1));
                Thread.Sleep(pSleep);
            }
        }

        //  MainMethod - Write Line
        /// <summary>
        /// Slowly writes text to the console [SEND TO NEW LINE]
        /// </summary>
        /// <param name="pText">Text to print</param>
        /// <param name="pSleep">Time delay between characters</param>
        public void WriteLine(string pText, int pSleep) {
            WriteText(pText, pSleep);
            Console.WriteLine("");
        }
    }
}