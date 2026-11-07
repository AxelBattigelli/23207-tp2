namespace Stationnement.Prototype.Navigation;

public abstract class EcranPrototype : UserControl
{
    public abstract string Titre { get; }
    public abstract string Fil { get; }
    public virtual string Contexte => "";
    public virtual string Indice =>
        "Prototype statique : les valeurs sont des exemples. Rien n'est enregistré et aucun paiement n'est transmis.";
}
