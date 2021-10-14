class Tank : IPersonnage
{
    public Tank()
        : base(5, 1,
        "Têtu comme une mule, vous baissez votre garde de 1 ♥ mais infligé 2 dégâts",
        "D'un coup d'épée bien placé, vous vous élancé et ingligé 1 dégât",
        "Votre armure de fer résiste au coup de votre adversaire") { }

    public bool specialActive = false;

    /// <summary>
    /// Fonction qui baisse la vie de 1, mais augmente la force d'attaque de 1.
    /// </summary>
    /// <param name="ennemi">Personnage a attaquer</param>
    public override void Special(IPersonnage ennemi) {
        this.specialActive = true;
        this.pv -= 1;
        this.attackForce += 1;
        if (ennemi.isDefense) { 
            ennemi.Damage(1);
            return;
        }
        ennemi.Damage(this.attackForce);
    }

    public override void StartRound() {
        if (this.specialActive) {
            this.pv += 1;
            this.attackForce -= 1;
            this.specialActive = false;
        }
        base.StartRound();
    }
}
