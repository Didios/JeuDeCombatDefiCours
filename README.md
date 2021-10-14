# Défi USSR45
## Credits
PELET Camille 	Co-développeur
PINEDA Joris 	Co-développeur
DIDIER Mathias 	Co-développeur

## Principe du jeu
Jeu en console (ligne de commande)
Programmé en C# en .NET

Jeu RPG de Combat au tour par tour. 
Chaque combat se fait en 1 VS 1 et il ne peut y avoir plusieurs combat simultanément.
Le joueur se bat contre un ordinateur.

## Regles du jeu
Les 2 joueurs choisissent tout d'abord une classe de personnage parmi les suivants présent dans 
l'Annexe "Fiche Personnages" :
- Damager
- Healer
- Tank
- Druid
- Robot

Chaque classe à ses propres caractéristiques, voir Annexe "Fiche Personnages".
Déroulement d'un tour :
- Les vies restantes des 2 joueurs sont affichées. 
- Chaque joueur choisit une action parmi celles proposées, voir Annexe "Actions possibles".
- Les actions s'effectuent simultanément.

Le jeu se poursuit tant que les deux personnages ont encore des points de vie et s’arrête
dès que l’un ou les deux personnages perdent leur dernier point de vie.

## Annexe
Fiche Personnages :
| Classe | PV	(♥) | Force d'attaque | Actions spéciales |
| ------ | ------ | ------ | ------ |
| Damager | 3 ♥♥♥  | 2 ♦♦|  Rage - Inflige en retour les dégâts qui lui sont infligés durant ce tour. Les dégâts reçus sont quand même subis. |
| Healer | 4 ♥♥♥♥|  1 ♦	|Soin - Récupère un point de vie (PV). |
| Tank | 5 ♥♥♥♥♥ |  1 ♦| Attaque Puissante - Correspond à une attaque durant laquelle le Tank sacrifie un de ses points de vie pour augmenter sa force d'attaque et ce  uniquement durant le tour en cours (-1♥ => +1♦ pendant le tour en cours, puis -1♦ => +1♥ au tour suivant). |
| Druid | 4 ♥♥♥♥ | 1 ♦	| Immobilisation Retardé - Permet de bloquer la prochaine action de l'adversaire, le rendant ainsi inerte au prochain tour.|
| Robot | 3 ♥♥♥ | 1 ♦	| Mise à Jour - Permet d'augmenter la force d'attaque  pendant 2 tours. Mais le robot étant défaillant, sa défense ne fonctionnera pas dans 20% des cas|

Actions Possibles :
| Actions | Effet appliqué | Exemple |
| ------ | ------ | ------ |
| Défense | Permet de se prémunir des dégâts infligés par l’adversaire. En cas de défense contre une attaque puissante d’un Tank, l’action Défendre ne permet de se prémunir que d’un seul dégât et conduit donc à prendre une blessure. | Si un Healer qui choisit l’action Défense contre un Damager qui attaque alors aucun des deux personnages ne perd ne point de vie. Cependant si un Damager défend face à une action spéciale "Attaque puissante" d’un Tank alors le Damager prévient une blessure mais est quand même touché par l’Attaque Puissante et perd un point de vie. Le Tank perd également un point de vie pour pour avoir activé son Attaque Puissante.| 
| Attaque | Infliger à l’adversaire autant de dégâts que sa force d’attaque. | Un Healer qui choisit l’action Attaque inflige à son adversaire 1 dégât du fait de sa force d’attaque (♦) alors qu’un Damager infligera 2 dégâts du fait de sa force d’attaque (♦♦).|
| Spéciale | Permet d'effectuer l'Action spéciale du Personnage (Voir colonnes "Actions spéciales" les capacités spéciales de chaque personnage). | Si un Healer choisit l'action spéciale, il gagne un point de vie (+1♥). |