//using System;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Text.Json;
//using Newtonsoft.Json;
using Project2.Models.Actor;
using Project2.Models.User;

namespace Project2.App.Main {
    public class ManagerGame {
        //  ~Server Variables
        public HttpClient Client;
        public string Port = "5201";

        //  End Variables
        public bool Force_Quit;

        //  Manager Variables
        public ManagerActor M_Actor { get; private set; }
        public ManagerCave M_Cave { get; private set; }
        public ManagerCombat M_Combat { get; private set; }

        //  Constructor
        public ManagerGame() {
            //  Setup ~Server
            Client = new HttpClient() { BaseAddress = new Uri($"http://localhost:{Port}/") };

            //  Setup Managers
            M_Actor = new ManagerActor(this);
            M_Cave = new ManagerCave(this);
            M_Combat = new ManagerCombat(this);
        }

        //  MainMethod - Play Game
        public void PlayGame() {
            //  Game logic will loop until player force quits
            while(Force_Quit == false) {
                M_Cave.GameLoop();
            }
        }

        //  MainMethod - Write Text
        public void WriteText(string pText, int pSleep) {
            for(int i = 0; i < pText.Length; i++) {
                Console.Write(pText.Substring(i, 1));
                //Thread.Sleep(pSleep);
            }
        }

        //  MainMethod - Write Line
        public void WriteLine(string pText, int pSleep) {
            WriteText(pText, pSleep);
            Console.WriteLine("");
        }
    }
}