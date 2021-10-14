using System;
using System.Collections.Generic;
using System.Linq;

// Classe permettant de créer et de manipulez des éléments dans un écran en représentation ascii
// L'écran s'affiche dans la Console avec des bords de représentation ainsi que les éléments ajouté à ce-dernier
class Screen
{
    // Les coordonnées 0,0 sont placé en haut à gauche de l'écran
    public int height;
    public int width;

    //Dictionary<Coordinates, string[]> elements = new();
    Dictionary<int, Dictionary<Coordinates, string[]>> layers = new();

    /// <summary>
    /// Constructeur de la classe, crée une fenêtre carré
    /// </summary>
    /// <param name="size">Permet de définir la taille d'un côté de l'écran</param>
    public Screen(int size) {
        this.height = size;
        this.width = size;
    }

    /// <summary>
    /// Constructeur de la classe, crée une fenêtre rectangulaire
    /// </summary>
    /// <param name="width">Permet de définir la longueur de l'écran</param>
    /// <param name="height">Permet de définir la largeur de l'écran</param>
    public Screen(int width, int height) {
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Permet d'afficher l'écran dans la console   (oskuuuur)
    /// </summary>
    public void Display() {
        List<string> lines = BuildBorder(); // on initialise les bord de l'écran
        var layers = this.layers.OrderBy(x => x.Key).Select(x => x.Value).ToList(); // on trie les éléments à afficher en fonction de leurs layers
        foreach (var elements in layers)
        {
            // on affiche un élément
            foreach (KeyValuePair<Coordinates, string[]> element in elements) 
            {
                // on parcours chaque string (épaisseur) composant l'élément
                int index = element.Key.x + 1;
                for (int i = 0; i < element.Value.Count(); i++)
                {
                    // on vérifie que le string à afficher est plus petite que la longueur de l'écran
                    // sinon, on découpe l'élément afin de le faire retourner à la ligne
                    if (index + element.Value[i].Length > this.width) // si le string a affiché dépasse de l'écran
                    {
                        // on affiche la partie qui ne dépasse pas
                        string celuiQuiDepassePas = element.Value[i].Substring(0, (this.width - index) -1);
                        lines[element.Key.y + 1 + i] = lines[element.Key.y + 1].Remove(index, celuiQuiDepassePas.Length)
                                                                            .Insert(index, celuiQuiDepassePas);

                        // on place le reste qui dépasse dans une file
                        Queue<string> file = new();
                        file.Enqueue(element.Value[i].Substring(celuiQuiDepassePas.Length, element.Value[i].Length - celuiQuiDepassePas.Length));

                        int lineIndex = 1; // permet de connaître l'index y de la ligne à laquelle affiché (permet le retour à la ligne)
                        while(file.Count() != 0) { // tant que la file n'est pas vide
                            string elmt = file.Dequeue(); // on défile
                            if (elmt.Length > this.width) // si l'élément défilé est trop grand (+ que la largeur de l'écran) 
                            {
                                // on affiche ce qui ne dépasse pas
                                celuiQuiDepassePas = elmt.Substring(0, this.width - 2);
                                lines[element.Key.y + 1 + lineIndex] = lines[element.Key.y + 1 + lineIndex]
                                                                            .Remove(1, celuiQuiDepassePas.Length)
                                                                            .Insert(1, celuiQuiDepassePas);
                                // on enfile la partie qui dépasse
                                file.Enqueue(elmt.Substring(celuiQuiDepassePas.Length, elmt.Length - celuiQuiDepassePas.Length));
                            } else // sinon, on affiche seulement le string
                                lines[element.Key.y + 1 + lineIndex] = lines[element.Key.y + 1 + lineIndex].Remove(1, elmt.Length)
                                                                                                            .Insert(1, elmt);
                            lineIndex++;
                        }

                    } else{
                        lines[element.Key.y + 1 + i] = lines[element.Key.y + 1].Remove(index, element.Value[i].Length) // affichage d'une string dans l'écran
                                                                               .Insert(index, element.Value[i]);
                    }
                }
            }
        }
        Console.SetCursorPosition(0,0);
        Console.Write(string.Join("", lines)); // on joint tout les éléments de lines (liste permettant une représentation du tableau) en une string qu'on affiche
        Console.SetCursorPosition(0, 0); // on place le curseur de la console au début de l'écran (pour réecrire dessus = avoir un seul écran)
    }

    // différentes fonction Add qui permettent d'effectuer la méthode Add principale avec tout les paramètres nécéssaire (remplissage par défaut de paramètres si non renseigné ou incomplet)
    public void Add(Coordinates coordinates, string text) {
        this.Add(coordinates, new string[]{text}, 0);
    }
    public void Add(Coordinates coordinates, string text, int layer) {
        this.Add(coordinates, new string[]{text}, layer);
    }

    /// <summary>
    /// Méthode permettant d'ajouter un élément dans l'écran
    /// </summary>
    /// <param name="coordinates">Une 'Coordinates' permettant de placer le premier caractère de l'élément</param>
    /// <param name="text">Contenu à afficher, chaque élément est placé sous le précédent, le premier est placé selon coordinates</param>
    /// <param name="layer">Int permettant de placer l'élément sur une certaine épaisseur (permet manipulation/suppression de plusieurs éléments simultanément)</param>
    public void Add(Coordinates coordinates, string[] text, int layer) {
        // ! Verifier si le texte ne dépasse pas en largeur ou hauteur (ca sera mieux que de faire un retour à la ligne)
        if (coordinates.y < 0)
            throw new ArgumentOutOfRangeException("Y","Y coordinate must be greater than 0.");
        if (coordinates.y > this.height)
            throw new ArgumentOutOfRangeException("Y","Y coordinate must be lesser than the height screen.");
        if (coordinates.x < 0)
            throw new ArgumentOutOfRangeException("X","X coordinate must be greater than 0.");
        if (coordinates.x > this.width)
            throw new ArgumentOutOfRangeException("X","X coordinate must be lesser than the width screen.");

        if (!layers.ContainsKey(layer))
            layers.Add(layer, new Dictionary<Coordinates, string[]>());
        if(layers[layer].ContainsKey(coordinates)) {
            layers[layer].Remove(coordinates);
        }
        layers[layer].Add(coordinates, text);
    }


    /// <summary>
    /// Méthode permettant de supprimer un élément en fonction de ses coordonnées dans l'écran
    /// </summary>
    /// <param name="coordinates">Une 'Coordinates' étant les coordonnées de l'élément à supprimer</param>
    public void Delete(Coordinates coordinates) {
        this.Delete(coordinates, 0);
    }

    /// <summary>
    /// Méthode permettant de supprimer un élément en fonction de ses coordonnées et de son layer dans l'écran
    /// Si plusieurs éléments sont sur les mêmes coordonnées, permet de ne supprimer que ceux à une certaine couche
    /// </summary>
    /// <param name="coordinates">Une 'Coordinates' étant les coordonnées de l'élément à supprimer</param>
    /// <param name="layer">Un int indiquant le layer de l'élément à supprimert</param>
    public void Delete(Coordinates coordinates, int layer) {
        if(this.layers[layer].ContainsKey(coordinates)) // on vérifie que l'élément voulant être supprimé existe
            this.layers[layer].Remove(coordinates);
    }

    /// <summary>
    /// Méthode permettant de supprimer un layer entier de l'écran (séléction multiple)
    /// </summary>
    /// <param name="layer">Un int étant celui du layer à supprimer</param>
    public void DeleteLayer(int layer) {
        if (this.layers.ContainsKey(layer)) {
            this.layers.Remove(layer);
        }
    }

    /// <summary>
    /// Méthode permettant de supprimer tout les éléments et toutes les couches de l'écran (nettoyage intégrale)
    /// </summary>
    public void Clear() {
        this.layers.Clear();
    }

    /// <summary>
    /// Méthode permettant de construire la base de l'écran (les bords constituer des caractères + | et -
    /// </summary>
    /// Renvoi une Liste de string étant la représentation d'un écran vide
    private List<string> BuildBorder() {
        string top = "+" + new String('-', this.width-2) + "+\n";
        string mid = "|" + new String(' ', this.width-2) + "|\n";
        List<string> lines = Enumerable.Repeat(mid, this.height-2).ToList();
        lines.Insert(0, top);
        lines.Add(top);
        return lines;
    }
    
}

// Classe permettant de définir des coordonnées en 2D
class Coordinates
{
    public int x;
    public int y;

    /// <summary>
    /// Constructeur de la classe, initialise les coordonnées
    /// </summary>
    /// <param name="x">Permet de définir l'abcisse des coordonnées</param>
    /// <param name="y">Permet de définir l'ordonnée des coordonnées</param>
    public Coordinates(int x, int y) {
        this.x = x;
        this.y = y;
    }
}