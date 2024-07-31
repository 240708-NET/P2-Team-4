using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Models.Actor {
    public class ActorEnemy : GameActor {
        //  Default Constructor
        public ActorEnemy() : base() {

        }

        //  Copy Constructor
        public ActorEnemy(ActorEnemy pEnemy) : base(pEnemy) {
            
        }
    }
}