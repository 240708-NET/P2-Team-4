using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project2.Models.User;
using Project2.Models.Items;

namespace Project2.Models.Actor {
    public class ActorPlayer : GameActor {
        //Server Variable
        public int UserId { get; set;}
        public UserPlayer user {get;set;}
        public int Score { get; set; }
        public Inventory inventories {get; set;}

        //  Class Variables
        public string Class { get; set; }

        //  Level Variables
        public int Level { get; set; }
        [NotMapped]
        public int ExpCurr { get; set; }
        [NotMapped]
        public int ExpReq { get; set; }
        [NotMapped]
        public string ExpStr => $"{ExpCurr}/{ExpReq}";
        public string Experience { get; set; }

        //  Score Variables
        //public int Score { get; set; }

        //  Default Constructor
        public ActorPlayer() : base() {
            //  Setup Class
            Class = "";

            //  Setup Experience
            Level = 1;
            Experience = "0/300";
            ExpCurr = 0;
            ExpReq = 300;

            //  Setup Score
            Score = 0;
        }

        //  Copy Constructor
        public ActorPlayer(ActorPlayer pPlayer) : base(pPlayer) {
            //  Setup _Server
            UserId = 0 + pPlayer.UserId;

            //  Setup Class
            Class = "" + pPlayer.Class;

            //  Setup Experience
            Level = pPlayer.Level;
            Experience = "" + pPlayer.Experience;

            int[] expArr = Array.ConvertAll(Experience.Split("/"), int.Parse);
            ExpCurr = 0 + expArr[0];
            ExpReq = 0 + expArr[1];

            //  Setup Score
            Score = 0 + pPlayer.Score;
        }

        //  MainMethod - Gain Experience
        public void GainExperience(int pAmt) {
            ExpCurr += pAmt;

            if (ExpCurr > ExpReq) {
                Level++;
                ExpReq = 2 * ExpCurr;

                Random rand = new Random();
                string[] dice = HealthDice.Split("d");

                HealthBase += rand.Next(0, int.Parse(dice[1]))+1 + D_AttrMod["CON"];
                HealthCurr = 0 + HealthBase;
            }
        }
    }
}