using System.IO;
using Project2.Models.Actor;

namespace Project2.App.Main {
    public class ManagerActor_CC {
        //  ~Reference Variables
        public ManagerActor RefMActor { get; private set; }
        private ManagerGame refMGame => RefMActor.RefMGame;
        private Random refRand => refMGame.Rand;

        //  Player Variables
        private ActorPlayer player;

        //  Constructor
        public ManagerActor_CC(ManagerActor pRef) {
            //  Setup ~Reference
            RefMActor = pRef;

            // Setup Player
            player = new ActorPlayer();
        }

        //  MainMethod - Character Creation
        public ActorPlayer CharacterCreation() {
            CC_Name();
            CC_Attribute();
            CC_Class();
            CC_Combat();

            return player;
        }

        //  SubMethod of CharacterCreation - Character Creation Name
        private void CC_Name() {
            bool nameValid = false;
            string name = "";

            while(nameValid == false) {
                refMGame.WriteText("Please enter your name: ", 25);
                name = Console.ReadLine() ?? "";

                refMGame.WriteLine($"{name} is your name? (Y/N)", 25);
                if ((Console.ReadLine() ?? "").ToLower() == "y") {
                    Console.WriteLine("");
                    nameValid = true;
                }
            }

            player.Name = name;
            player.Proper = true;
            player.Article = "";
        }

        //  SubMethod of CharacterCreation - Character Creation Attribute
        private void CC_Attribute() {
            //  Roll attributes
            List<int> attributePool = CCAttr_Roll();
            List<int> attributeNum = CCAttr_Assign(attributePool);

            player.D_AttrScr = new Dictionary<string, int>() {
                ["STR"] = attributeNum[0],
                ["DEX"] = attributeNum[1],
                ["CON"] = attributeNum[2],
                ["INT"] = attributeNum[3],
                ["WIS"] = attributeNum[4],
                ["CHA"] = attributeNum[5],
            };

            player.D_AttrMod = new Dictionary<string, int>() {
                ["STR"] = (attributeNum[0] / 2) - 5,
                ["DEX"] = (attributeNum[1] / 2) - 5,
                ["CON"] = (attributeNum[2] / 2) - 5,
                ["INT"] = (attributeNum[3] / 2) - 5,
                ["WIS"] = (attributeNum[4] / 2) - 5,
                ["CHA"] = (attributeNum[5] / 2) - 5,
            };

            //  Setup Defense
            player.Def_UnarmoredAC = 10 + player.D_AttrMod["DEX"];
        }

        private List<int> CCAttr_Roll() {
            bool methodValid = false;
            string rollMethod = "";

            //  Choosing rolling method
            while (methodValid == false) {
                refMGame.WriteLine("Please choose a rolling method: ", 25);
                refMGame.WriteLine("(1) 4d6 drop lowest", 25);
                refMGame.WriteLine("(2) 3d6", 25);
                refMGame.WriteLine("(3) 2d6+6", 25);

                rollMethod = Console.ReadLine() ?? "";

                switch(rollMethod) {
                    case "1":
                        rollMethod = "4d6d1";
                        refMGame.WriteLine("Rolling using 4d6 drop lowest", 25);
                        Console.WriteLine("");

                        methodValid = true;
                        break;

                    case "2":
                        rollMethod = "3d6";
                        refMGame.WriteLine("Rolling using 3d6", 25);
                        Console.WriteLine("");

                        methodValid = true;
                        break;

                    case "3":
                        rollMethod = "2d6+6";
                        refMGame.WriteLine("Rolling using 2d6+6", 25);
                        Console.WriteLine("");

                        methodValid = true;
                        break;

                    default:
                        refMGame.WriteLine("Please choose one of the options", 25);
                        Console.WriteLine("");
                        break;
                }
            }

            refMGame.WriteText("Rolling Attributes", 75);
            refMGame.WriteLine("...", 400);
            Thread.Sleep(500);

            //  Rolling attributes
            List<int> attributePool = new List<int>();
            List<int> rolls = new List<int>();
            int total = 0;

            for(int i = 0; i < 6; i++) {
                refMGame.WriteText($"Rolling {(i+1)}: ", 75);
                rolls.Clear();

                switch(rollMethod) {
                    //  Roll 4d6, drop lowest
                    case "4d6d1":
                        //  Roll 4d6
                        for(int x = 0; x < 4; x++) {
                            int roll = refRand.Next(0, 6)+1;
                            rolls.Add(roll);

                            refMGame.WriteText((roll + ((x < 3) ? ", " : "")), 75);
                        }

                        //  Drop lowest
                        rolls.Sort();
                        rolls.RemoveAt(0);
                        total = rolls.Sum();
                        break;

                    //  Roll 3d6
                    case "3d6":
                        //  Roll 3d6
                        for(int x = 0; x < 3; x++) {
                            int roll = refRand.Next(0, 6)+1;
                            rolls.Add(roll);

                            refMGame.WriteText((roll + ((x < 2) ? ", " : "")), 75);
                        }

                        total = rolls.Sum();
                        break;

                    // Roll 2d6, add 6
                    case "2d6+6":
                        //  Roll 2d6
                        for(int x = 0; x < 2; x++) {
                            int roll = refRand.Next(0, 6)+1;
                            rolls.Add(roll);

                            refMGame.WriteText((roll + ((x < 2) ? ", " : "")), 75);
                        }
                        refMGame.WriteText("6", 75);

                        //  Add 6
                        rolls.Add(6);
                        total = rolls.Sum();
                        break;
                }

                //  Add to attribute pool
                attributePool.Add(total);
                refMGame.WriteLine($", Total {total}{CCAttr_DisplayMod(total)}", 75);
                
                Console.WriteLine("");
                Thread.Sleep(1000);
            }

            attributePool.Sort();
            attributePool.Reverse();

            return attributePool;
        }

        private void CCAttr_DisplayPool(List<int> pAttr) {
            refMGame.WriteText("Attribute Pool: ", 25);
            for(int i = 0; i < pAttr.Count; i++) {
                refMGame.WriteText((pAttr[i] + CCAttr_DisplayMod(pAttr[i]) + ((i < pAttr.Count-1) ? ", " : "")), 25);
            }
            Console.WriteLine("");
        }

        private string CCAttr_DisplayMod(int pAttr) {
            int mod = (pAttr / 2) - 5;
            return $"({((mod > 0) ? "+" : "")}{mod})";
        }

        private List<int> CCAttr_Assign(List<int> pPool) {
            List<int> attributes = new List<int>();
            int actionCount = 0;
            int attrNum = -1;

            while(pPool.Count > 0) {
                CCAttr_DisplayPool(pPool);
                actionCount = 0;

                while(actionCount < 5) {
                    attrNum = -1;
                    switch(pPool.Count) {
                        //  Assign strength
                        case 6:
                            refMGame.WriteText("Enter strength score from pool: ", 25);
                            break;
                            //  Assign strength
                        case 5:
                            refMGame.WriteText("Enter dexterity score from pool: ", 25);
                            break;
                            //  Assign strength
                        case 4:
                            refMGame.WriteText("Enter constitution score from pool: ", 25);
                            break;
                            //  Assign strength
                        case 3:
                            refMGame.WriteText("Enter intelligence score from pool: ", 25);
                            break;
                            //  Assign strength
                        case 2:
                            refMGame.WriteText("Enter wisdom score from pool: ", 25);
                            break;
                            //  Assign strength
                        case 1:
                            refMGame.WriteText($"Assigning last value of {pPool[0]} to charisma", 25);
                            attributes.Add(pPool[0]);
                            pPool.Clear();

                            actionCount = 6;
                            Console.WriteLine("");
                            break;
                    }

                    if (pPool.Count > 0 && int.TryParse(Console.ReadLine(), out attrNum) == true && pPool.Contains(attrNum)) {
                        attributes.Add(attrNum);
                        pPool.Remove(attrNum);
                        actionCount = 6;
                    }
                    else if (pPool.Count > 0) {
                        actionCount++;
                    }

                    Console.WriteLine("");
                }
            }

            //  Final check for attributes
            refMGame.WriteLine("Attributes are:", 25);
            refMGame.WriteLine($" - Strength     : {((attributes[0] < 10 && attributes[0] > 0) ? " " : "") + attributes[0]} {CCAttr_DisplayMod(attributes[0])}", 25);
            refMGame.WriteLine($" - Dexterity    : {((attributes[1] < 10 && attributes[0] > 0) ? " " : "") + attributes[1]} {CCAttr_DisplayMod(attributes[1])}", 25);
            refMGame.WriteLine($" - Constitution : {((attributes[2] < 10 && attributes[0] > 0) ? " " : "") + attributes[2]} {CCAttr_DisplayMod(attributes[2])}", 25);
            refMGame.WriteLine($" - Intelligence : {((attributes[3] < 10 && attributes[0] > 0) ? " " : "") + attributes[3]} {CCAttr_DisplayMod(attributes[3])}", 25);
            refMGame.WriteLine($" - Wisdom       : {((attributes[4] < 10 && attributes[0] > 0) ? " " : "") + attributes[4]} {CCAttr_DisplayMod(attributes[4])}", 25);
            refMGame.WriteLine($" - Charisma     : {((attributes[5] < 10 && attributes[0] > 0) ? " " : "") + attributes[5]} {CCAttr_DisplayMod(attributes[5])}", 25);
            
            actionCount = 0;
            string action = "";

            while(actionCount >= 0) {
                refMGame.WriteLine("Do you wish to continue? (Y/N)", 25);
                action = (Console.ReadLine() ?? "").ToLower();
                Console.WriteLine("");

                if (action == "n") {
                    return CCAttr_Assign(attributes);
                }
                else if (action == "y") {
                    actionCount = -1;
                }
            }

            return attributes;
        }

        private void CC_Class() {
            bool classValid = false;
            string classType = "";

            //  Setup Class
            while(classValid == false) {
                refMGame.WriteLine("Available classes: ", 25);
                refMGame.WriteLine("(1) Fighter", 25);
                Console.WriteLine("");

                refMGame.WriteText("Please choose your class: ", 25);
                classType = Console.ReadLine() ?? "";

                switch(classType) {
                    case "1":
                        classType = "Fighter";
                        break;
                    
                    default:
                        classType = "Fighter";
                        break;
                }

                int actionCount = 0;
                while(actionCount < 5) {
                    refMGame.WriteLine($"You are a {classType}? (Y/N)", 25);
                    if ((Console.ReadLine() ?? "").ToLower() == "y") {
                        Console.WriteLine("");
                        classValid = true;
                        actionCount = 5;
                    }
                    
                    actionCount++;
                }
            }

            player.Class = classType;

            //  Setup Level
            player.Level = 5;
            player.ExpCurr = RefMActor.D_LevelReqs[((player.Level > 1) ? (player.Level-1) : player.Level).ToString()];
            player.ExpReq = RefMActor.D_LevelReqs[player.Level.ToString()];

            player.Proficiency = 3;

            //  Setup Health
            switch(player.Class) {
                case "Fighter":
                    player.HealthDice = $"{player.Level}d10";
                    refMGame.WriteLine($"Assigning {player.Level}d10 to health dice", 25);

                    string conMod = $"{((player.D_AttrMod["CON"] > 0) ? "+" : "")}{((player.D_AttrMod["CON"]!=0) ? "" + player.D_AttrMod["CON"] : "")}";

                    player.HealthBase = 10 + player.D_AttrMod["CON"];
                    refMGame.WriteLine($"- Adding 10{conMod} to health for level 1", 25);

                    for(int i = 1; i < player.Level; i++) {
                        int amt = (refRand.Next(0, 10)+1);
                        player.HealthBase += amt + player.D_AttrMod["CON"];
                        refMGame.WriteLine($"- Adding {amt}{conMod} to health for level {i+1}", 25);
                    }
                    refMGame.WriteLine($"Setting max health to {player.HealthBase}", 25);
                    
                    player.HealthCurr = 0 + player.HealthBase;
                    break;
            }
            Console.WriteLine("");
        }
    
        private void CC_Combat() {
            //  Setup Unarmed
            player.Atk_Unarmed = new GameAttack("fists", "punches with", "Melee", 0, "0_1_bludgeoning");
            refMGame.WriteLine("Giving player an unarmed melee attack called 'fists' with 1 bludgeoning damage", 25);

            //  Setup Combat
            player.Atk_List = new List<GameAttack>();
            player.Atk_List.Add(new GameAttack("longsword", "swings with", "Melee", 0, "1d8_0_slashing"));
            refMGame.WriteLine("Giving player a melee attack called 'longsword' with 1d8 slashing damage", 25);
            Console.WriteLine("");

            //  Setup Defense
            player.DefenseArmor = "Breastplate_14+DEX/M2";
            player.Def_ArmoredAC = 14 + ((player.D_AttrMod["DEX"] > 2) ? 2 : player.D_AttrMod["DEX"]);
            refMGame.WriteLine($"Giving player a breastplate with 14{((player.D_AttrMod["DEX"] > 0) ? "+" : "")}{player.D_AttrMod["DEX"]}({player.Def_ArmoredAC}) AC", 25);
            Console.WriteLine("");
        }
    }
}