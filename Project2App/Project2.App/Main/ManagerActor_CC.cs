using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Project2.Models.Actor;
using Project2.Models.User;

namespace Project2.App.Main {
    public class ManagerActor_CC {
        //  ~Reference Variables
        public ManagerActor RefMActor { get; private set; }
        private ManagerGame refMGame => RefMActor.RefMGame;
        //  Constructor
        public ManagerActor_CC(ManagerActor pRef) {
            //  Setup ~Reference
            RefMActor = pRef;
        }

        //  MainMethod - Character Creation
        public async void CharacterCreation() {
            //CC_Initial();

            //  Get or create user
            string userStr = refMGame.Client.GetStringAsync("getUserByName/BobUser").Result;
            UserPlayer? user = JsonConvert.DeserializeObject<UserPlayer>(userStr);

            if (user == null) {
                var userResponse = await refMGame.Client.PostAsync("createUser/BobUser", null);
                userStr = refMGame.Client.GetStringAsync("getUserByName/BobUser").Result;
                user = JsonConvert.DeserializeObject<UserPlayer>(userStr);
            }

            //  Get or create player
            string playerStr = refMGame.Client.GetStringAsync($"getPlayerByName/{user.Id}/Bob").Result ?? "";
            ActorPlayer? player = JsonConvert.DeserializeObject<ActorPlayer>(playerStr);

            if (player == null) {
                var playerResponse = await refMGame.Client.PostAsync($"createEmptyPlayer/{user.Id}/Bob", null);
                playerStr = refMGame.Client.GetStringAsync($"getPlayerByName/{user.Id}/Bob").Result;
                player = JsonConvert.DeserializeObject<ActorPlayer>(playerStr);
            }

            //  Rolling attributes
            string attrStr = refMGame.Client.GetStringAsync($"createAttributePool/2d6+6").Result;
            List<int> attributePool = JsonConvert.DeserializeObject<List<int>>(attrStr) ?? new List<int>();
            attributePool.Sort();
            attributePool.Reverse();

            //  Assigning attributes
            string attributes = string.Join(",", attributePool);
            var attrResponse = await refMGame.Client.PostAsync($"createPlayerAttributes/{player.Id}/{attributes}", null);

            //  Create class
            var classJson = JsonContent.Create<string>("Fighter");
            var classResponse = await refMGame.Client.PostAsync($"createPlayerClass/{player.Id}", classJson);

            //  Create level and experience
            var expJson = JsonContent.Create<string>("5_3000/6000");
            var expResponse = await refMGame.Client.PostAsync($"createPlayerLevel/{player.Id}", expJson);

            //  Create skill
            var skillJson = JsonContent.Create<int>(3);
            var skillResponse = await refMGame.Client.PostAsync($"createPlayerSkill/{player.Id}", skillJson);

            //  Create health
            var healthJson = JsonContent.Create<string>("5d10");
            var healthResponse = await refMGame.Client.PostAsync($"createPlayerHealth/{player.Id}", healthJson);

            //  Create unarmed attack
            var unarmedJson = JsonContent.Create<string>("fists_punches with their_Melee_0/0_1_bludgeoning");
            var unarmedResponse = await refMGame.Client.PostAsync($"createPlayerUnarmed/{player.Id}", unarmedJson);

            //  Create weapon attack
            var longswordJson = JsonContent.Create<string>("longsword_swings with their_Melee_0/1d8_0_slashing");
            var attackResponse = await refMGame.Client.PostAsync($"createPlayerAttack/{player.Id}", longswordJson);

            //  Create defense
            var armorJson = JsonContent.Create<string>("Breastplate_14+DEX/M2");
            var defenseResponse = await refMGame.Client.PostAsync($"createPlayerDefense/{player.Id}", armorJson);

            //  Get Player
            var lastResponse = refMGame.Client.GetStringAsync($"getPlayerById/{player.Id}").Result;
            RefMActor.Player = new ActorPlayer(JsonConvert.DeserializeObject<ActorPlayer>(lastResponse));
        }

        private async void CC_Initial() {
            string userStr = refMGame.Client.GetStringAsync("getUserByName/BobUser").Result;
            UserPlayer? user = JsonConvert.DeserializeObject<UserPlayer>(userStr);

            if (user == null) {
                var userResponse = await refMGame.Client.PostAsync("createUser/BobUser", null);

                while((userStr = refMGame.Client.GetStringAsync("getUserByName/BobUser").Result) == "") {
                    Thread.Sleep(400);
                }
                user = JsonConvert.DeserializeObject<UserPlayer>(userStr);
            }

            CC_Name(user);
        }

        //  SubMethod of CharacterCreation - Character Creation Name
        private async void CC_Name(UserPlayer pUser) {
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

            string playerStr = refMGame.Client.GetStringAsync($"getPlayerByName/{pUser.Id}/{name}").Result;
            ActorPlayer? player = JsonConvert.DeserializeObject<ActorPlayer>(playerStr);

            if (player == null) {
                var playerResponse = await refMGame.Client.PostAsync($"createEmptyPlayer/{pUser.Id}/{name}", null);

                while((playerStr = refMGame.Client.GetStringAsync($"getPlayerByName/{pUser.Id}/{name}").Result) == "") {
                    Thread.Sleep(400);
                }
                player = JsonConvert.DeserializeObject<ActorPlayer>(playerStr);
            }

            CC_Attribute(pUser.Id, player.Id);
        }

        //  SubMethod of CharacterCreation - Character Creation Attribute
        private async void CC_Attribute(int pUserId, int pId) {
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
            Console.WriteLine($"createAttributePool/{rollMethod}");
            string attrStr = refMGame.Client.GetStringAsync($"createAttributePool/{rollMethod}").Result;
            List<int> attributePool = JsonConvert.DeserializeObject<List<int>>(attrStr);
            attributePool.Sort();
            attributePool.Reverse();

            List<int> attributeNum = CCAttr_Assign(attributePool);

            string attributes = $"{attributeNum[0]},{attributeNum[1]},{attributeNum[2]},{attributeNum[3]},{attributeNum[4]},{attributeNum[5]}";
            Console.WriteLine(attributes);
            var attrResponse = await refMGame.Client.PostAsync($"createPlayerAttributes/{pId}/{attributes}", null);

            attrStr = "";
            while((attrStr = refMGame.Client.GetStringAsync($"getPlayerAttributes/{pUserId}/{pId}").Result) == "") {
                Thread.Sleep(400);
            }

            CC_Class(pId);
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

        private async void CC_Class(int pId) {
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

            var classJson = JsonContent.Create<string>(classType);
            var classResponse = await refMGame.Client.PostAsync($"createPlayerClass/{pId}", classJson);
            while(classResponse == null) {
                Console.WriteLine("-Pause 1");
                Thread.Sleep(200);
            }

            var expJson = JsonContent.Create<string>("5_3000/6000");
            var expResponse = await refMGame.Client.PostAsync($"createPlayerLevel/{pId}", expJson);
            while(expResponse == null) {
                Console.WriteLine("-Pause 2");
                Thread.Sleep(200);
            }

            var skillJson = JsonContent.Create<int>(3);
            var skillResponse = await refMGame.Client.PostAsync($"createPlayerSkill/{pId}", skillJson);
            while(skillResponse == null) {
                Console.WriteLine("-Pause 3");
                Thread.Sleep(200);
            }

            var healthJson = JsonContent.Create<string>("5d10");
            var healthResponse = await refMGame.Client.PostAsync($"createPlayerHealth/{pId}", healthJson);
            while(healthResponse == null) {
                Console.WriteLine("-Pause 4");
                Thread.Sleep(200);
            }

            //  Setup Health
            /*
            switch(player.Class) {
                case "Fighter":
                    player.HealthDice = $"{player.Level}d10";
                    refMGame.WriteLine($"Assigning {player.Level}d10 to health dice", 25);

                    string conMod = $"{((player.D_AttrMod["CON"] > 0) ? "+" : "")}{((player.D_AttrMod["CON"]!=0) ? "" + player.D_AttrMod["CON"] : "")}";

                    player.HealthBase = 10 + player.D_AttrMod["CON"];
                    refMGame.WriteLine($"- Adding 10{conMod} to health for level 1", 25);

                    /*
                    for(int i = 1; i < player.Level; i++) {
                        int amt = (refRand.Next(0, 10)+1);
                        player.HealthBase += amt + player.D_AttrMod["CON"];
                        refMGame.WriteLine($"- Adding {amt}{conMod} to health for level {i+1}", 25);
                    }
                    refMGame.WriteLine($"Setting max health to {player.HealthBase}", 25);
                    
                    player.HealthCurr = 0 + player.HealthBase;
                    break;
            }
            */
            Console.WriteLine("");

            CC_Combat(pId);
        }
    
        private async void CC_Combat(int pId) {
            //  Setup Unarmed
            string unarmed = "fists_punches with their_Melee_0/0_1_bludgeoning";
            var unarmedJson = JsonContent.Create<string>(unarmed);
            var unarmedResponse = await refMGame.Client.PostAsync($"createPlayerUnarmed/{pId}", unarmedJson);
            while(unarmedResponse == null) {
                Console.WriteLine("-Pause 5");
                Thread.Sleep(200);
            }
            /*
            player.Atk_Unarmed = new GameAttack("fists", "punches with their", "Melee", 0, "0_1_bludgeoning");
            player.AttackUnarmed += "fists_punches with their_Melee_0/0_1_bludgeoning";
            refMGame.WriteLine("Giving player an unarmed melee attack called 'fists' with 1 bludgeoning damage", 25);
            */

            //  Setup Combat
            string longsword = "longsword_swings with their_Melee_0/1d8_0_slashing";
            var longswordJson = JsonContent.Create<string>(longsword);
            var attackResponse = await refMGame.Client.PostAsync($"createPlayerAttack/{pId}", longswordJson);
            while(attackResponse == null) {
                Console.WriteLine("-Pause 6");
                Thread.Sleep(200);
            }
            /*
            player.Atk_List = new List<GameAttack>();
            player.Atk_List.Add(new GameAttack("longsword", "swings with their", "Melee", 0, "1d8_0_slashing"));
            player.AttackList += "longsword_swings with their_Melee_0/1d8_0_slashing";
            refMGame.WriteLine("Giving player a melee attack called 'longsword' with 1d8 slashing damage", 25);
            Console.WriteLine("");
            */

            //  Setup Defense
            string armor = "Breastplate_14+DEX/M2";
            var armorJson = JsonContent.Create<string>(armor);
            var defenseResponse = await refMGame.Client.PostAsync($"createPlayerDefense/{pId}", armorJson);
            while(defenseResponse == null) {
                Console.WriteLine("-Pause 6");
                Thread.Sleep(200);
            }
            /*
            player.DefenseArmor = "Breastplate_14+DEX/M2";
            player.Def_ArmoredAC = 14 + ((player.D_AttrMod["DEX"] > 2) ? 2 : player.D_AttrMod["DEX"]);
            refMGame.WriteLine($"Giving player a breastplate with 14{((player.D_AttrMod["DEX"] > 0) ? "+" : "")}{player.D_AttrMod["DEX"]}({player.Def_ArmoredAC}) AC", 25);
            Console.WriteLine("");
            */

            /*
            string userStr = refMGame.Client.GetStringAsync("getUserByName/BobUser").Result;
            UserPlayer user = JsonConvert.DeserializeObject<UserPlayer>(userStr);
            Console.WriteLine(user == null);

            if (user == null) {
                var userJson = JsonContent.Create<UserPlayer>(new UserPlayer() { Name = "BobUser", });
                var userResponse = await refMGame.Client.PostAsync("createUser", userJson);
            }

            userStr = refMGame.Client.GetStringAsync("getUserByName/BobUser").Result;
            player.UserId = JsonConvert.DeserializeObject<UserPlayer>(userStr).Id;
            //player.User = user;

            var playerJson = JsonContent.Create<ActorPlayer>(player);
            Console.WriteLine(refMGame.Client.BaseAddress + "createPlayer");
            var postResponse = await refMGame.Client.PostAsync("createPlayer", playerJson);
            //Console.WriteLine(postResponse.Content.ReadAsStringAsync().Result);
            */

            var playerResponse = refMGame.Client.GetStringAsync($"getPlayerById/{pId}").Result;
            while(playerResponse == null) {
                Console.WriteLine("-Pause 7");
                Thread.Sleep(200);
            }
            RefMActor.Player = new ActorPlayer(JsonConvert.DeserializeObject<ActorPlayer>(playerResponse));
        }
    }
}