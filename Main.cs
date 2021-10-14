using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Type[] personnagesArray = new Type[] {new Druid().GetType(), new Damager().GetType(), new Tank().GetType(), new Healer().GetType(), new Robot().GetType()};

    static void Main(string[] args)
    { 
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        // affichage debut jeu
        Screen screen = new Screen(80, 20);

        screen.Add(new Coordinates(30, 5), "jeu Tour par Tour RPG", 1);
        screen.Add(new Coordinates(35, 6), "crée par :", 1);
        screen.Add(new Coordinates(34, 8), "PELE Camille", 1);
        screen.Add(new Coordinates(33, 9), "DIDIER Mathias", 1);
        screen.Add(new Coordinates(34, 10), "PINEDA Joris", 1);

        screen.Display();
        screen.DeleteLayer(1);
        System.Threading.Thread.Sleep(3000);


        //Démarrage boucle de jeu
        while (true) {
            Console.Clear();
            Console.SetCursorPosition(0, 0);


            // choix personnage joueur
            screen.Add(new Coordinates(30, 5), "Choix du Personnage :", 2);
            int space = screen.width / (personnagesArray.Length+1); 
            for(int i = 0; i < personnagesArray.Length; i++)
            {
                screen.Add(new Coordinates(space * (i+1) - personnagesArray[i].ToString().Length/2, 10), personnagesArray[i].ToString(), 2);
            }
            screen.Display();

            Coordinates selectorCoordinates = new Coordinates(screen.width / (personnagesArray.Length+1) - personnagesArray[0].ToString().Length/2, 11);
            string selectorIndicator = new String('^', personnagesArray[0].ToString().Length);
            screen.Add(selectorCoordinates, selectorIndicator, 1);
            screen.Display();

            int selected = 0;
            while(true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.RightArrow) {
                    selected++;
                } else if (key == ConsoleKey.LeftArrow) {
                    selected--;
                } else if (key == ConsoleKey.Enter) {
                    break;
                }

                if (selected < 0) selected = personnagesArray.Length-1;
                if (selected >= personnagesArray.Count()) selected = 0;

                selectorCoordinates.x = (screen.width / (personnagesArray.Length+1))*(selected+1) - personnagesArray[selected].ToString().Length/2;
                selectorIndicator = new String('^', personnagesArray[selected].ToString().Length);
                screen.DeleteLayer(1); // y a moyen de faire mieux, je sais juste pas
                screen.Add(selectorCoordinates, selectorIndicator, 1);
                screen.Display();
            }
            IPersonnage persoJ = (IPersonnage)Activator.CreateInstance(personnagesArray[selected]); // initialisation peronnage du Joueur
            screen.Clear();
            // TOCHANGE
            screen.Add(new Coordinates(screen.width/2 - 16, screen.height/2 - 3), $"Vous avez choisi la classe {persoJ.GetType().ToString()}");
            screen.Display();
            System.Threading.Thread.Sleep(1000);
            screen.Clear();

            // choix personnage Ordinateur
            screen.Add(new Coordinates(30, 5), "L'ordinateur choisi :", 1);
            for (int i = 0; i < 8; i++) // attente Fake
            {
                screen.Add(new Coordinates(screen.width/2-1, screen.height/2), new String('.', (i%4)), 2);
                screen.Display();
                screen.DeleteLayer(2);
                System.Threading.Thread.Sleep(500);
            }

            Random rand = new Random();
            int numPersoO = rand.Next(0, personnagesArray.Length); // on choisit le personnage de l'ordinateur aléatoirement
            IPersonnage persoO = (IPersonnage)Activator.CreateInstance(personnagesArray[numPersoO]); // initialisation personnage Ordinateur
            
            screen.Add(new Coordinates(screen.width/2 - personnagesArray[numPersoO].ToString().Length/2, screen.height/2), personnagesArray[numPersoO].ToString());
            screen.Display();
            System.Threading.Thread.Sleep(1000);
            screen.Clear();

            //ROUND

            //boucle fin de partie
            bool partie = true; 
            while (partie)
            {
                persoJ.StartRound();
                persoO.StartRound();

                // affichage vie et rôle
                screen.Add(new Coordinates(1, 0), $"Joueur : {new String('♥', persoJ.pv)} PV");
                screen.Add(new Coordinates(1, 1), $"Rôle : {persoJ.GetType().ToString()}");
                screen.Add(new Coordinates(screen.width - persoO.pv -19, 0), $"Ordinateur : {new String('♥', persoO.pv)} PV");
                screen.Add(new Coordinates(screen.width - persoO.pv - 19, 1), $"Rôle : {persoO.GetType().ToString()}");

                List<KeyValuePair<int,  (IPersonnage, IPersonnage)>> actions = new(); // liste permettant d'effectuer les actions dans le bon ordre
                
                //Tour du joueur
                if (!persoJ.specialDruid)
                {
                    actions.Add(new KeyValuePair<int, (IPersonnage, IPersonnage)>(ChooseAction(persoJ, persoO, screen), (persoJ, persoO)));
                }

                // texte si immobilisé
                if (persoJ.specialDruid) {
                    screen.Add(new Coordinates(screen.width/2 - 25, screen.height/2), "Tu es immobilisé par les racines de ton adversaire", 1);
                    screen.Add(new Coordinates(screen.width/2 - 12, screen.height/2+1), "Tu ne peux rien faire ...", 1);
                    screen.Display();
                    screen.DeleteLayer(1);
                    System.Threading.Thread.Sleep(5000);
                } 
                

                //Tour de l'ordinateur
                // texte si immobilisé
                if (persoO.specialDruid) {
                    screen.Add(new Coordinates(screen.width/2 - 26, screen.height/2), "Ton adversaire ne peut plus bouger grâce à ta magie !", 1);
                    screen.Display();
                    screen.DeleteLayer(1);
                    System.Threading.Thread.Sleep(1500);
                }

                if (!persoO.specialDruid)
                {
                    screen.Add(new Coordinates(30, 5), "L'ordinateur dit :", 1);
                    for (int i = 0; i < 4; i++) // attente Fake
                    {
                        screen.Add(new Coordinates(screen.width/2-1, screen.height/2), new String('.', (i%4)), 2);
                        screen.Display();
                        screen.DeleteLayer(2);
                        System.Threading.Thread.Sleep(300);
                    }

                    int choixRobot = rand.Next(1, 4); // on choisit l'action du personnage de l'ordinateur aléatoirement
                    List<List<string>> actionsList = new List<List<string>>(){ new List<string>(){"Contre votre bravoure, je n'ai d'autre choix que la Défense", "Afin de protéger ma vie, j'utilise ma Défense", "Pour toute Défense, je me retourne et te pètes dessus coquin"}, // Défense
                                                                            new List<string>(){"Je brave le danger et décide d'Attaquer contre vents et marées", "Mon arme te transpercera malandrin, Yaa, à l'Attaque", "Je touche ton âme et transperce ton corps de mon Attaque"}, // Attaque
                                                                            new List<string>(){"Vous ne survivrez pas à la puissance de mon sortilège magique", "La magikaboo, la Bibidibabidiboo, **Pouf** (spécial)", "J'en appelle au pouvoir du crâne ancestral ... (Spécial)"}}; // Spécial
                    
                    string phraseOrdi = actionsList[choixRobot -1][rand.Next(0, 3)];
                    screen.Add(new Coordinates(screen.width/2 - phraseOrdi.Length/2, screen.height/2), phraseOrdi);
                    screen.Display();
                    System.Threading.Thread.Sleep(5000);


                    actions.Add(new KeyValuePair<int, (IPersonnage, IPersonnage)>(choixRobot, (persoO, persoJ)));
                
                }

                screen.Display();
                screen.Clear();

                ApplyAction(actions);

                //Fin du tour
                persoJ.EndRound();
                persoO.EndRound();
                

                screen.Clear();
                // Condition de fin = si un des 2 joueurs est mort
                if (persoJ.pv <= 0 || persoO.pv <=0) partie = false;

            }

            // Message de fin => affichage résultat partie
            if (persoJ.pv <= 0 && persoO.pv <=0) // égalité
            {
            screen.Add(new Coordinates(screen.width/2 - 12, 1), new string[]{
                @"     ___   ..   ___     ",
                @"  o-~   ~=[UU]=~   ~-o  ",
                @"  |        ||        |  ",
                @"  |        ||        |  ",
                @" /^\       ||       /^\ ",
                @"(___)      ||      (___)",
                @"           ||           ",
                @"           ||           ",
                @"          /VV\          ",
                @"        ~'~~~~`~        ",}, 0);
            screen.Add(new Coordinates(screen.width/2 - 16, 13), "Vos coups portent au même moment");
            screen.Add(new Coordinates(screen.width/2 - 18, 14), "vos vie s'éteignent en même temps.");
            }
            else if (persoJ.pv <= 0) // défaite joueur, victoire ordi
            {
                screen.Add(new Coordinates(screen.width/2 - 10, 1), new string[]{       
                    @"     _.--''--._     ",
                    @"    /  _    _  \    ",
                    @" _  ( (_\  /_) )  _ ",
                    @"{ \._\   /\   /_./ }",
                    @"/_'=-.}______{.-='_\",
                    @" _  _.=('''')=._  _ ",
                    @"(_''_.-'`~~`'-._''_)",
                    @" {_'            '_} "}, 0);
                screen.Add(new Coordinates(screen.width/2 - 17, 11), "Vous avez succombé à vos blessures.");
                screen.Add(new Coordinates(screen.width/2 - 11, 12), "Votre ennemi à gagner.");
            }
            else if (persoO.pv <= 0) // victoire joueur, défaite ordi
            {
                screen.Add(new Coordinates(screen.width/2 - 7, 1), new string[]{
                    @"  ___________  ",
                    @" '._==_==_=_.' ",
                    @" .-\:      /-. ",
                    @"| (|:.     |) |",
                    @" '-|:.     |-' ",
                    @"   \::.    /   ",
                    @"    '::. .'    ",
                    @"      ) (      ",
                    @"    _.' '._    ",
                    @"   `'''''''`   ", }, 0); 
                screen.Add(new Coordinates(screen.width / 2 - 23, 13), "Votre ennemi meurt sous vos assauts foudroyants");
                screen.Add(new Coordinates(screen.width / 2 - 11, 14), "la victoire est vôtre.");
            }
            screen.Display();
            System.Threading.Thread.Sleep(10000);

            // Rejouer ?
            screen.Clear();
            screen.Add(new Coordinates(30, 5), "Voulez-vous rejouer ?", 2);
            string[] choixReplay = new string[]{"OUI", "NON"};
            space = screen.width / (choixReplay.Length+1); 
            for(int i = 0; i < choixReplay.Length; i++)
            {
                screen.Add(new Coordinates(space * (i+1) - choixReplay[i].ToString().Length/2, 10), choixReplay[i].ToString(), 2);
            }
            screen.Display();

            selectorCoordinates = new Coordinates(screen.width / (choixReplay.Length+1) - choixReplay[0].ToString().Length/2, 11);
            selectorIndicator = new String('^', choixReplay[0].ToString().Length);
            screen.Add(selectorCoordinates, selectorIndicator, 1);
            screen.Display();

            selected = 0;
            while(true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.RightArrow) {
                    selected++;
                } else if (key == ConsoleKey.LeftArrow) {
                    selected--;
                } else if (key == ConsoleKey.Enter) {
                    break;
                }

                if (selected < 0) selected = choixReplay.Length-1;
                if (selected >= choixReplay.Count()) selected = 0;

                selectorCoordinates.x = (screen.width / (choixReplay.Length+1))*(selected+1) - choixReplay[selected].ToString().Length/2;
                selectorIndicator = new String('^', choixReplay[selected].ToString().Length);
                screen.DeleteLayer(1); // y a moyen de faire mieux, je sais juste pas
                screen.Add(selectorCoordinates, selectorIndicator, 1);
                screen.Display();
            }
            if (selected == 1) break;
            screen.Clear();

        }
    }

    /// <summary>
    /// Permet d'effectuer toutes les actions dans l'ordre suivant : défense, attaque, spécial; ainsi que dans le sous-ordre : Healer, Tank, Damager
    /// </summary>
    /// <param name="actions">Liste de clé/valeur, la clé représente l'action a éfféctuer, la valeur est un tuple du joueur et de l'ennemi</param>
    public static void ApplyAction(List<KeyValuePair<int,  (IPersonnage, IPersonnage)>> actions)
    {
        //Trier par la clé
        actions = actions.AsEnumerable().OrderBy(x => x.Key)
                              .ThenBy(x => Array.IndexOf(personnagesArray, x.Value.Item1.GetType()))
                              .ToList();


        foreach (KeyValuePair<int, (IPersonnage, IPersonnage)> action in actions)
        {
            switch (action.Key)
            {
                case 1:
                    action.Value.Item1.Defense();
                    break;
                case 2:
                    action.Value.Item1.Attack(action.Value.Item2);
                    break;
                case 3:
                    action.Value.Item1.Special(action.Value.Item2);
                    break;
            }
        }
    }

    /// <summary>
    /// Fonctione qui fais faire un choix au joueur
    /// </summary>
    /// <param name="joueur">joueur qui choisi</param>
    /// <param name="ennemi">l'ennemi du joueur</param>
    public static int ChooseAction(IPersonnage joueur, IPersonnage ennemi, Screen screen) {
        //TODO affichage dans screen
        List<string> actions = new List<string>(){"Défense", "Attaque", "Spécial"};
        int space = screen.width / (actions.Count + 1);
        for (int i = 0; i < actions.Count; i++)
        {
            screen.Add(new Coordinates(space * (i + 1) - actions[i].ToString().Length / 2, 10), actions[i].ToString(), 2);
        }

        // ? Surement moyen de pas faire en réccurcif (ca sera plus opti)
        // return VerifyAction(int.Parse(Console.ReadLine()), joueur, ennemi);

        screen.Add(new Coordinates(30, 5), "Choisir une action :", 2);
        Coordinates selectorCoordinates = new Coordinates(screen.width / (actions.Count+1) - actions[0].ToString().Length/2, 11);
        string selectorIndicator = new String('^', actions[0].ToString().Length);
        screen.Add(selectorCoordinates, selectorIndicator, 2);
        screen.Add(new Coordinates(screen.width/2 - joueur.descDefense.Length/2, 15), joueur.descDefense, 3);
        screen.Display();

        int selected = 0;
        while(true)
        {
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.RightArrow) {
                selected++;
            } else if (key == ConsoleKey.LeftArrow) {
                selected--;
            } else if (key == ConsoleKey.Enter) {
                break;
            }

            if (selected < 0) selected = actions.Count-1;
            selected = selected%actions.Count;
            selectorCoordinates.x = (screen.width / (actions.Count+1))*(selected+1) - actions[selected].Length / 2;
            selectorIndicator = new String('^', actions[selected%actions.Count].Length);

            string text = "";
            switch (selected){
                case 0:
                    text = joueur.descDefense;
                    break;
                case 1:
                    text = joueur.descAttaque;
                    break;
                case 2:
                    text = joueur.descSpecial;
                    break;
            }
            screen.Add(new Coordinates(screen.width/2 - text.Length/2, 15), text, 3);
            screen.Display();
            screen.DeleteLayer(3);
        }
        screen.DeleteLayer(2);
        return selected+1;
    }
}