using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Project2.Models.Actor;

namespace Project2.App.Main {
    public class ManagerActor {
        //  ~Reference Variables
        public ManagerGame RefMGame { get; private set; }

        //  Player Variables
        public ActorPlayer Player;

        //  Constructor
        public ManagerActor(ManagerGame pRef) {
            //  Setup ~Reference
            RefMGame = pRef;
        }

        //  MainMethod - Character Creation
        public void CharacterCreation() {
            ManagerActor_CC creation = new ManagerActor_CC(this);
            creation.CharacterCreation();
        }
    }
}