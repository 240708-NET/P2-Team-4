using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Models.Actor {
    public class ActorEnemy : GameActor {
        public string ItemId { get; set; }

        //  Default Constructor
        public ActorEnemy() : base() {
            ItemId = "";
        }

        //  Copy Constructor
        public ActorEnemy(ActorEnemy pEnemy) : base(pEnemy) {
            ItemId = "" + pEnemy.ItemId;
        }
    }
}