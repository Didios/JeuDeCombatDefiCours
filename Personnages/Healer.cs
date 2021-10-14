class Healer : IPersonnage 
{
    public Healer()
        : base(4, 1,
        "Usant de votre magie, vous vous régénéré 1 ♥",
        "En lancant une potion sur votre adversaire, vous infligez 1 dégât",
        "Votre esquive vous fait évité le coup de l'adversaire") { }

    /// <summary>
    /// Fonction augmentant les pv du personnage de 1
    /// </summary>
    public override void Special(IPersonnage ennemi) {
        if (this.pv < 4) this.pv += 1;
    }
}
