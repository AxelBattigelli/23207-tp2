using Stationnement.Prototype.Navigation;
using Stationnement.Prototype.Ui;

namespace Stationnement.Prototype.Ecrans;

public sealed class EcranAccueil : EcranPrototype
{
    public override string Titre => "Simulation des barrières de péage";
    public override string Fil => "Accueil";
    public override string Indice => "Choisissez la borne ou le poste à présenter. Aucun traitement n'est lancé.";

    public EcranAccueil()
    {
        Content = UiKit.Page(1040,
            UiKit.Titre(Demo.Stationnement),
            UiKit.Lead("Prototype des écrans d'entrée, de sortie, de paiement et de gestion. Les données affichées sont fictives."),
            Cartes());
    }

    static Control Cartes()
    {
        var grille = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*,*") };
        Ajouter(grille, 0, "Borne d'entrée", "Ticket ou carte d'abonnement. La barrière se lève, puis se baisse après le passage.", "Ouvrir la borne", () => Navigateur.Courant.Aller(new EcranBorneEntree()));
        Ajouter(grille, 1, "Borne de sortie", "Ticket à payer ou carte d'abonnement. Reçu, puis ouverture de la barrière.", "Ouvrir la borne", () => Navigateur.Courant.Aller(new EcranBorneSortie()));
        Ajouter(grille, 2, "Poste du gestionnaire", "Authentification, état du stationnement, tarifs et statistiques.", "Ouvrir la session", () => Navigateur.Courant.Aller(new EcranAuthentification()));
        return grille;
    }

    static void Ajouter(Grid grille, int colonne, string titre, string texte, string action, Action clic)
    {
        var bouton = UiKit.Bouton(action, "primary", clic);
        var carte = UiKit.Carte(new StackPanel
        {
            Spacing = 12,
            Children =
            {
                new TextBlock { Text = titre, FontSize = 20, FontWeight = FontWeight.SemiBold, Foreground = UiKit.Teal, TextWrapping = TextWrapping.Wrap },
                new TextBlock { Text = texte, FontSize = 14, Foreground = UiKit.Secondaire, TextWrapping = TextWrapping.Wrap, MinHeight = 72 },
                bouton
            }
        });
        carte.Margin = new Thickness(8);
        Grid.SetColumn(carte, colonne);
        grille.Children.Add(carte);
    }
}
