using System;
using System.Collections.Generic;
using System.Linq;

class Robot : IPersonnage
{

    Queue<int> queue = new Queue<int>();

    public Robot()
        : base(3, 1,
        "Mode RTX ON, votre force augmente de 1 pendant 2 tours",
        "Activation Onde 5G, votre ennemi n'est pas vacciné et subit votre force",
        "La base virale VPS a été mise à jour, arrêtant peut-être l'attaque adverse") { }

    /// <summary>
    /// Fonction qui baisse augmente l'attaque de 1 pendant 2 tours.
    /// </summary>
    /// <param name="ennemi">Personnage a attaquer</param>
    public override void Special(IPersonnage ennemi) {
        this.attackForce += 1;
        for(int i = 0; i < 2; i++){
            this.queue.Enqueue(0);
        }
        this.queue.Enqueue(1);
    }

    /// <summary>
    /// Se défend 4 fois sur 5
    /// </summary>
    public override void Defense() {
        Random rand = new Random();
        if (rand.Next(0,5) != 0) {
            this.isDefense = true;
        }
    }

    /// <summary>
    /// On enleve à attackForce ce qu'on enleve de la file
    /// </summary>
    public override void EndRound() {
        if (this.queue.Count != 0) this.attackForce -= this.queue.Dequeue();
        base.EndRound();
    }

}