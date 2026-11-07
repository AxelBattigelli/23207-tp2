namespace Stationnement.Prototype.Navigation;

public sealed class Navigateur
{
    readonly Stack<EcranPrototype> _pile = new();
    readonly MainWindow _fenetre;

    public Navigateur(MainWindow fenetre) => _fenetre = fenetre;

    public static Navigateur Courant { get; private set; } = null!;

    public static void Attacher(MainWindow fenetre) => Courant = new Navigateur(fenetre);

    public void Reinitialiser(EcranPrototype ecran)
    {
        _pile.Clear();
        _pile.Push(ecran);
        Afficher();
    }

    public void Aller(EcranPrototype ecran)
    {
        _pile.Push(ecran);
        Afficher();
    }

    public void AllerVers(params EcranPrototype[] ecrans)
    {
        _pile.Clear();
        foreach (var ecran in ecrans)
            _pile.Push(ecran);
        Afficher();
    }

    public void Retour()
    {
        if (_pile.Count <= 1)
            return;
        _pile.Pop();
        Afficher();
    }

    public void Accueil() => Reinitialiser(new Ecrans.EcranAccueil());

    public Task Avertir(string titre, string message)
    {
        var dialogue = new NoticeWindow(titre, message);
        return dialogue.ShowDialog(_fenetre);
    }

    void Afficher()
    {
        var ecran = _pile.Peek();
        _fenetre.Afficher(ecran, _pile.Count > 1);
    }
}
