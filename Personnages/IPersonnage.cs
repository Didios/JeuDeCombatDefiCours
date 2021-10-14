using System;

class IPersonnage
{
    /// <summary>
    /// Les pv du personnage
    /// </summary>
    public int pv;

    /// <summary>
    /// La force d'attaque du personnage
    /// </summary>
    public int attackForce;

    /// <summary>
    /// Si le personnage est en état de défense ou non
    /// </summary>
    public bool isDefense = false;

    /// <summary>
    /// Booléen pour le pouvoir spécial du druide
    /// </summary>
    public bool specialDruid = false;

    public string descSpecial;
    public string descAttaque;
    public string descDefense;

    /// <summary>
    /// Method constructeur de la class
    /// </summary>
    /// <param name="pv">Le nombre de pv de base du personnage</param>
    /// <param name="attackForce">La force d'attaque de base du personnage</param>
    public IPersonnage(int pv, int attackForce, string descSpecial, string descAttaque, string descDefense) 
    {
        this.pv = pv;
        this.attackForce = attackForce;

        this.descSpecial = descSpecial;
        this.descAttaque = descAttaque;
        this.descDefense = descDefense;
    }

    /// <summary>
    /// Fonction permettant à chaque Class héritant de IPersonnage de pourvoir avoir un Spécial different.
    /// </summary>
    public virtual void Special(IPersonnage ennemi) { }

    /// <summary>
    /// Fonction permettant aux class héritant de IPersonnage d'attaquer
    /// Les dégats causer son attackForce
    /// </summary>
    /// <param name="perso">Personnage attaqué</param>
    public void Attack(IPersonnage perso)
    {
        this.Attack(this.attackForce, perso);
    }

    /// <summary>
    /// Fonction permettant aux class héritant de IPersonnage d'attaquer
    /// Les dégats causer son attackForce
    /// </summary>
    /// <param name="force">Force de l'attaque</param>
    /// <param name="perso">Personnage attaqué</param>
    public void Attack(int force, IPersonnage perso) 
    {
        if (perso.isDefense){ // Si le personnage attaqué se défend
            return;
        }
        perso.Damage(force);
    }

    /// <summary>
    /// Fonction permettant d'activer la défense pour le personnage l'executant
    /// </summary>
    public virtual void Defense()
    {
        this.isDefense = true;
    }

    /// <summary>
    /// Fonction permettant de faire subir des dégats au personnage l'executant
    /// </summary>
    /// <param name="damagePts">Nombre de points de dégats</param>
    public virtual void Damage(int damagePts) {
        this.pv -= damagePts;
    }

    public virtual void StartRound() {}

    /// <summary>
    /// Fonction qui reset les stats à fin d'un round
    /// </summary>
    public virtual void EndRound() {
        this.isDefense = false;
    }
}