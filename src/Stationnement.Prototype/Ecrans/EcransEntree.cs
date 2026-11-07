using Stationnement.Prototype.Navigation;
using Stationnement.Prototype.Ui;

namespace Stationnement.Prototype.Ecrans;

public sealed class EcranBorneEntree : EcranPrototype
{
    readonly bool _complet;

    public override string Titre => "Borne d'entrée";
    public override string Fil => "Accueil  /  Entrée";
    public override string Contexte => "Borne " + Demo.BorneEntree;
    public override string Indice => "Cas « Entrer dans un stationnement ». Le commutateur montre l'état stationnement complet.";

    public EcranBorneEntree(bool complet = false)
    {
        _complet = complet;
        var ticket = UiKit.Bouton("Prendre un ticket", "primary", () => Navigateur.Courant.Aller(new EcranTicketEmis()));
        var carte = UiKit.Bouton("Présenter une carte d'abonnement", "secondary", () => Navigateur.Courant.Aller(new EcranLectureCarteEntree()));
        ticket.IsEnabled = !complet;
        carte.IsEnabled = !complet;

        var interrupteur = new CheckBox
        {
            Content = "Afficher l'état « stationnement complet »",
            IsChecked = complet,
            FontSize = 14,
            Foreground = UiKit.Secondaire
        };
        interrupteur.IsCheckedChanged += (_, _) =>
            Navigateur.Courant.AllerVers(new EcranAccueil(), new EcranBorneEntree(interrupteur.IsChecked == true));

        var contenu = new List<Control>
        {
            UiKit.Titre("Entrer dans le stationnement"),
            UiKit.Lead($"Borne {Demo.BorneEntree}, barrière {Demo.BarriereEntree}. {Demo.Vehicules} véhicules sur {Demo.Capacite} places.")
        };

        contenu.Add(complet
            ? UiKit.Bandeau("Stationnement complet. Le bouton ticket et le lecteur de carte sont inactifs. La barrière reste baissée.", "erreur")
            : UiKit.Bandeau("Des places sont libres. Choisissez un ticket ou une carte d'abonnement.", "succes"));

        contenu.Add(UiKit.Actions(ticket, carte));
        contenu.Add(interrupteur);
        Content = UiKit.Page(760, contenu.ToArray());
    }
}

public sealed class EcranTicketEmis : EcranPrototype
{
    public override string Titre => "Ticket émis";
    public override string Fil => "Accueil  /  Entrée  /  Ticket";
    public override string Contexte => "Borne " + Demo.BorneEntree;
    public override string Indice => "Cas « Entrer avec un ticket ». La barrière d'entrée est levée.";

    public EcranTicketEmis()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Ticket imprimé"),
            UiKit.Lead("Une transaction d'entrée est présentée. Aucun enregistrement n'est fait."),
            UiKit.Ticket("TICKET D'ENTRÉE",
                ("Numéro", Demo.Ticket),
                ("Transaction", Demo.Transaction),
                ("Entrée", Demo.DateEntree),
                ("Stationnement", Demo.Stationnement),
                ("Borne", Demo.BorneEntree)),
            UiKit.Bandeau($"Barrière {Demo.BarriereEntree} levée. Avancez.", "succes"),
            UiKit.Actions(UiKit.Bouton("Le véhicule passe", "primary", () =>
                Navigateur.Courant.Aller(new EcranPassage("entrée", Demo.BarriereEntree, Demo.Vehicules + 1)))));
    }
}

public sealed class EcranLectureCarteEntree : EcranPrototype
{
    public override string Titre => "Carte d'abonnement";
    public override string Fil => "Accueil  /  Entrée  /  Carte";
    public override string Contexte => "Borne " + Demo.BorneEntree;
    public override string Indice => "Cas « Entrer avec une carte d'abonnement ». Les deux réponses du lecteur sont affichables.";

    public EcranLectureCarteEntree()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Présentez la carte"),
            UiKit.Lead("Approchez la carte du lecteur de la borne d'entrée."),
            UiKit.Fiche(
                ("Lecteur", Demo.BorneEntree),
                ("Attente", "Carte d'abonnement")),
            UiKit.Actions(
                UiKit.Bouton("Carte acceptée", "primary", () => Navigateur.Courant.Aller(new EcranAbonnementEntree())),
                UiKit.Bouton("Carte refusée", "danger", () => Navigateur.Courant.Aller(Messages.CarteRefusee("entrée")))));
    }
}

public sealed class EcranAbonnementEntree : EcranPrototype
{
    public override string Titre => "Abonnement reconnu";
    public override string Fil => "Accueil  /  Entrée  /  Carte  /  Acceptée";
    public override string Contexte => "Borne " + Demo.BorneEntree;
    public override string Indice => "La carte est valide. La barrière se lève sans émettre de ticket.";

    public EcranAbonnementEntree()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Entrée avec abonnement"),
            UiKit.Ticket("CARTE D'ABONNEMENT",
                ("Numéro", Demo.Abonnement),
                ("Titulaire", Demo.Client),
                ("Adresse", Demo.ClientAdresse),
                ("Cellulaire", Demo.ClientCellulaire),
                ("Valide jusqu'au", Demo.AbonnementExpiration),
                ("Transaction", "TA-2207")),
            UiKit.Bandeau($"Barrière {Demo.BarriereEntree} levée. Avancez.", "succes"),
            UiKit.Actions(UiKit.Bouton("Le véhicule passe", "primary", () =>
                Navigateur.Courant.Aller(new EcranPassage("entrée", Demo.BarriereEntree, Demo.Vehicules + 1)))));
    }
}

public sealed class EcranPassage : EcranPrototype
{
    readonly string _sens;

    public override string Titre => "Passage du véhicule";
    public override string Fil => "Accueil  /  Passage";
    public override string Contexte => _sens == "entrée" ? "Borne " + Demo.BorneEntree : "Borne " + Demo.BorneSortie;
    public override string Indice => "La barrière se baisse après le passage. Le compteur montré est un exemple, il n'est pas calculé.";

    public EcranPassage(string sens, string barriere, int vehicules)
    {
        _sens = sens;
        var retour = sens == "entrée"
            ? UiKit.Bouton("Nouvelle entrée", "secondary", () => Navigateur.Courant.AllerVers(new EcranAccueil(), new EcranBorneEntree()))
            : UiKit.Bouton("Nouvelle sortie", "secondary", () => Navigateur.Courant.AllerVers(new EcranAccueil(), new EcranBorneSortie()));

        Content = UiKit.Page(760,
            UiKit.Titre(sens == "entrée" ? "Véhicule entré" : "Véhicule sorti"),
            UiKit.Bandeau($"Passage enregistré. Barrière {barriere} baissée.", "succes"),
            UiKit.Fiche(
                ("Stationnement", Demo.Stationnement),
                ("Places occupées (exemple)", $"{vehicules} / {Demo.Capacite}"),
                ("Barrière", "Baissée")),
            UiKit.Actions(
                UiKit.Bouton("Terminer", "primary", () => Navigateur.Courant.Accueil()),
                retour));
    }
}
