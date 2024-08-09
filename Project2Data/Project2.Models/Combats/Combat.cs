using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project2.Models.Actor;

namespace Project2.Models.Combats {
    public class Combat {
        [Key]
        public int Id { get; set; }

        //  Enemy Variables
        public int ActorEnemyId{ get; set; }
        public ActorEnemy enemy { get; set; } = null!;
        public int EnemyACLow { get; set; }
        public int EnemyACHigh { get; set; }
        public string EnemyACRange { get; set; }

        //  Player Variables
        public int ActorPlayerId { get; set; }
        public ActorPlayer player { get; set; } = null!;

        public Combat() {
            EnemyACLow = -999;
            EnemyACHigh = 999;
            EnemyACRange = "???";
        }
    }
} 