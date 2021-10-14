using System;

class Damager : IPersonnage 
{

    /// <summary>
    /// Le total de dégats subit pendant le tour (surtout utili)
    /// </summary>
    public int takenDamage;

    public Damager()
        : base( 3, 2,
        "Grâce à votre fine analyse, vous infligé autant de dégâts que reçus",
        "Votre bombe explose et cause 2 dégâts à l'adversaire",
        "En plaçant judicieusement votre bouclier, vous parez l'attaque adverse") { }

    private bool specialActive = false;

    /// <summary>
    /// Le personnage à qui renvoyer les dégats.
    /// </summary>
    private IPersonnage specialPerso;

    /// <summary>
    /// Fonction qui renvoies les dégats subi pendant le tour à la fin
    /// </summary>
    /// <param name="ennemi">Personnage à qui renvoyer tout les dégats</param>
    public override void Special(IPersonnage ennemi) {
        this.specialActive = true;
        this.specialPerso = ennemi;
    }

    public override void Damage(int damagePts) {
        this.takenDamage += damagePts;
        base.Damage(damagePts);
    }

    public override void EndRound() {
        if (this.specialActive) {
            Console.WriteLine("Renvois les dégats subient pendant le tour : {0}", this.takenDamage);
            this.Attack(this.takenDamage, this.specialPerso);
            this.specialActive = false;
        }
        this.takenDamage = 0;
        base.EndRound();
    }
}