using Stationnement.Prototype.Navigation;
using Stationnement.Prototype.Ui;

namespace Stationnement.Prototype.Ecrans;

public sealed class EcranBorneSortie : EcranPrototype
{
    public override string Titre => "Borne de sortie";
    public override string Fil => "Accueil  /  Sortie";
    public override string Contexte => "Borne " + Demo.BorneSortie;
    public override string Indice => "Cas « Sortir d'un stationnement » : ticket ou carte d'abonnement.";

    public EcranBorneSortie()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Sortir du stationnement"),
            UiKit.Lead($"Borne {Demo.BorneSortie}, barrière {Demo.BarriereSortie}. Insérez le ticket ou présentez la carte."),
            UiKit.Bandeau("La barrière est baissée tant que le titre d'accès n'est pas accepté.", "info"),
            UiKit.Actions(
                UiKit.Bouton("Insérer un ticket", "primary", () => Navigateur.Courant.Aller(new EcranTicketLu())),
                UiKit.Bouton("Présenter une carte d'abonnement", "secondary", () => Navigateur.Courant.Aller(new EcranLectureCarteSortie()))));
    }
}

public sealed class EcranTicketLu : EcranPrototype
{
    public override string Titre => "Ticket lu";
    public override string Fil => "Accueil  /  Sortie  /  Ticket";
    public override string Contexte => "Borne " + Demo.BorneSortie;
    public override string Indice => "Cas « Sortir avec un ticket ». Le ticket valide mène au montant, l'autre au refus.";

    public EcranTicketLu()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Ticket inséré"),
            UiKit.Ticket("TICKET LU",
                ("Numéro", Demo.Ticket),
                ("Transaction", Demo.Transaction),
                ("Entrée", Demo.DateEntree),
                ("Borne d'entrée", Demo.BorneEntree)),
            UiKit.Actions(
                UiKit.Bouton("Ticket valide : voir le montant", "primary", () => Navigateur.Courant.Aller(new EcranMontant())),
                UiKit.Bouton("Ticket non valide", "danger", () => Navigateur.Courant.Aller(Messages.TicketInvalide()))));
    }
}

public sealed class EcranMontant : EcranPrototype
{
    public override string Titre => "Montant à payer";
    public override string Fil => "Accueil  /  Sortie  /  Ticket  /  Montant";
    public override string Contexte => "Borne " + Demo.BorneSortie;
    public override string Indice => "Le montant est un exemple fixe. Il n'est pas calculé.";

    public EcranMontant()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Montant dû"),
            UiKit.Lead("Affichage de démonstration pour un séjour en jour de semaine."),
            UiKit.Fiche(
                ("Ticket", Demo.Ticket),
                ("Entrée", Demo.DateEntree),
                ("Sortie", Demo.DateSortie),
                ("Durée", Demo.Duree),
                ("Tarif horaire de semaine", Demo.TarifHeureSemaine),
                ("Plafond de semaine", Demo.TarifMaxSemaine),
                ("Montant à payer", Demo.Montant)),
            UiKit.Actions(UiKit.Bouton("Payer", "primary", () => Navigateur.Courant.Aller(new EcranPaiement()))));
    }
}

public sealed class EcranPaiement : EcranPrototype
{
    public override string Titre => "Paiement";
    public override string Fil => "Accueil  /  Sortie  /  Paiement";
    public override string Contexte => "Borne " + Demo.BorneSortie;
    public override string Indice => "Cas « Payer ». Les champs sont montrés. Rien n'est envoyé au système de paiement.";

    public EcranPaiement()
    {
        var numero = UiKit.Zone("1234 5678 9012 4242");
        var titulaire = UiKit.Zone("Nom figurant sur la carte");
        var expiration = UiKit.Zone("MM/AA");
        var erreur = UiKit.ErrorLine();

        Content = UiKit.Page(760,
            UiKit.Titre("Paiement par carte"),
            UiKit.Lead("Type de paiement présenté : carte bancaire. La borne ne contacte pas le système de paiement dans ce prototype."),
            UiKit.Bandeau($"Montant présenté : {Demo.Montant}", "attention"),
            UiKit.Carte(new StackPanel
            {
                Spacing = 14,
                Children =
                {
                    UiKit.Champ("Numéro de carte", numero),
                    UiKit.Champ("Titulaire", titulaire),
                    UiKit.Champ("Date d'expiration", expiration)
                }
            }),
            erreur,
            UiKit.Actions(
                UiKit.Bouton("Confirmer le paiement", "primary", () =>
                {
                    if (string.IsNullOrWhiteSpace(numero.Text) ||
                        string.IsNullOrWhiteSpace(titulaire.Text) ||
                        string.IsNullOrWhiteSpace(expiration.Text))
                    {
                        erreur.Text = "Renseignez le numéro, le titulaire et la date d'expiration pour afficher le reçu.";
                        erreur.IsVisible = true;
                        return;
                    }

                    Navigateur.Courant.Aller(new EcranRecu(numero.Text.Trim(), titulaire.Text.Trim(), expiration.Text.Trim()));
                }),
                UiKit.Bouton("Paiement refusé", "danger", () => Navigateur.Courant.Aller(Messages.PaiementRefuse()))));
    }
}

public sealed class EcranRecu : EcranPrototype
{
    public override string Titre => "Reçu";
    public override string Fil => "Accueil  /  Sortie  /  Paiement  /  Reçu";
    public override string Contexte => "Borne " + Demo.BorneSortie;
    public override string Indice => "Le reçu est affiché, puis la barrière de sortie se lève.";

    public EcranRecu(string numero, string titulaire, string expiration)
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Reçu imprimé"),
            UiKit.Ticket("REÇU DE PAIEMENT",
                ("Paiement", Demo.Paiement),
                ("Transaction", Demo.Transaction),
                ("Montant", Demo.Montant),
                ("Carte", numero),
                ("Titulaire", titulaire),
                ("Expiration", expiration),
                ("Date", Demo.DateSortie)),
            UiKit.Bandeau($"Paiement présenté comme accepté. Barrière {Demo.BarriereSortie} levée.", "succes"),
            UiKit.Actions(UiKit.Bouton("Le véhicule passe", "primary", () =>
                Navigateur.Courant.Aller(new EcranPassage("sortie", Demo.BarriereSortie, Demo.Vehicules - 1)))));
    }
}

public sealed class EcranLectureCarteSortie : EcranPrototype
{
    public override string Titre => "Carte d'abonnement";
    public override string Fil => "Accueil  /  Sortie  /  Carte";
    public override string Contexte => "Borne " + Demo.BorneSortie;
    public override string Indice => "Cas « Sortir avec une carte d'abonnement ». Aucun montant n'est demandé si la carte est acceptée.";

    public EcranLectureCarteSortie()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Présentez la carte"),
            UiKit.Lead("Le lecteur cherche la transaction ouverte liée à l'abonnement."),
            UiKit.Actions(
                UiKit.Bouton("Carte acceptée", "primary", () => Navigateur.Courant.Aller(new EcranAbonnementSortie())),
                UiKit.Bouton("Carte refusée", "danger", () => Navigateur.Courant.Aller(Messages.CarteRefusee("sortie")))));
    }
}

public sealed class EcranAbonnementSortie : EcranPrototype
{
    public override string Titre => "Sortie avec abonnement";
    public override string Fil => "Accueil  /  Sortie  /  Carte  /  Acceptée";
    public override string Contexte => "Borne " + Demo.BorneSortie;
    public override string Indice => "L'abonnement couvre le séjour. La barrière se lève sans paiement.";

    public EcranAbonnementSortie()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Abonnement reconnu"),
            UiKit.Fiche(
                ("Abonnement", Demo.Abonnement),
                ("Titulaire", Demo.Client),
                ("Entrée", Demo.DateEntree),
                ("Sortie", Demo.DateSortie),
                ("Montant dû", "0,00 $")),
            UiKit.Bandeau($"Barrière {Demo.BarriereSortie} levée. Avancez.", "succes"),
            UiKit.Actions(UiKit.Bouton("Le véhicule passe", "primary", () =>
                Navigateur.Courant.Aller(new EcranPassage("sortie", Demo.BarriereSortie, Demo.Vehicules - 1)))));
    }
}

public static class Messages
{
    public static EcranMessage CarteRefusee(string sens) => new(
        "Carte refusée",
        sens == "entrée" ? "Accueil  /  Entrée  /  Carte refusée" : "Accueil  /  Sortie  /  Carte refusée",
        sens == "entrée" ? "Borne " + Demo.BorneEntree : "Borne " + Demo.BorneSortie,
        "La carte d'abonnement n'est pas acceptée",
        "Le lecteur ne reconnaît pas la carte, ou l'abonnement est expiré. La barrière reste baissée. Ce message est un scénario de démonstration.",
        "erreur",
        ("Retour à la borne", "primary", () =>
            Navigateur.Courant.AllerVers(
                new EcranAccueil(),
                sens == "entrée" ? new EcranBorneEntree() : new EcranBorneSortie())));

    public static EcranMessage TicketInvalide() => new(
        "Ticket non valide",
        "Accueil  /  Sortie  /  Ticket non valide",
        "Borne " + Demo.BorneSortie,
        "Ce ticket ne peut pas ouvrir la barrière",
        $"Le ticket {Demo.Ticket} est inconnu, déjà utilisé ou illisible. La barrière {Demo.BarriereSortie} reste baissée.",
        "erreur",
        ("Retour à la borne", "primary", () => Navigateur.Courant.AllerVers(new EcranAccueil(), new EcranBorneSortie())));

    public static EcranMessage PaiementRefuse() => new(
        "Paiement refusé",
        "Accueil  /  Sortie  /  Paiement refusé",
        "Borne " + Demo.BorneSortie,
        "Le système de paiement refuse la carte",
        "Scénario de démonstration : aucun message n'a été envoyé à un système externe. La barrière reste baissée et le reçu n'est pas imprimé.",
        "erreur",
        ("Reprendre le paiement", "primary", () => Navigateur.Courant.Aller(new EcranPaiement())),
        ("Annuler et revenir à la borne", "secondary", () => Navigateur.Courant.AllerVers(new EcranAccueil(), new EcranBorneSortie())));
}

public sealed class EcranMessage : EcranPrototype
{
    public override string Titre { get; }
    public override string Fil { get; }
    public override string Contexte { get; }

    public EcranMessage(
        string titre,
        string fil,
        string contexte,
        string entete,
        string corps,
        string ton,
        params (string Libelle, string Classe, Action Aller)[] actions)
    {
        Titre = titre;
        Fil = fil;
        Contexte = contexte;
        var boutons = actions.Select(action => UiKit.Bouton(action.Libelle, action.Classe, action.Aller)).ToArray();
        Content = UiKit.Page(760,
            UiKit.Titre(entete),
            UiKit.Bandeau(corps, ton),
            UiKit.Actions(boutons));
    }
}
