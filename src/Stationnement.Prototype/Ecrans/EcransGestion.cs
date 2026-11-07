using Stationnement.Prototype.Navigation;
using Stationnement.Prototype.Ui;

namespace Stationnement.Prototype.Ecrans;

public sealed class EcranAuthentification : EcranPrototype
{
    public override string Titre => "Authentification";
    public override string Fil => "Accueil  /  Gestionnaire  /  Authentification";
    public override string Contexte => "Gestionnaire";
    public override string Indice => "Cas « S'authentifier », inclus par « Gérer le stationnement ». Aucun mot de passe n'est vérifié.";

    public EcranAuthentification()
    {
        var courriel = UiKit.Zone("adresse@stationnement.exemple");
        var motDePasse = UiKit.Zone("Mot de passe", secret: true);
        var erreur = UiKit.ErrorLine();

        Content = UiKit.Page(640,
            UiKit.Titre("Connexion du gestionnaire"),
            UiKit.Lead("Saisissez un courriel et un mot de passe, puis choisissez le résultat à présenter."),
            UiKit.Carte(new StackPanel
            {
                Spacing = 14,
                Children =
                {
                    UiKit.Champ("Adresse courriel", courriel),
                    UiKit.Champ("Mot de passe", motDePasse)
                }
            }),
            erreur,
            UiKit.Actions(
                UiKit.Bouton("Connexion réussie", "primary", async () =>
                {
                    if (!ChampsRemplis(courriel, motDePasse, erreur))
                        return;
                    await Navigateur.Courant.Avertir(
                        "Connexion réussie",
                        $"Session ouverte pour {Demo.Gestionnaire}. Connexion affichée : {Demo.Connexion}.");
                    Navigateur.Courant.Aller(new EcranMenuGestion());
                }),
                UiKit.Bouton("Échec de connexion", "danger", async () =>
                {
                    if (!ChampsRemplis(courriel, motDePasse, erreur))
                        return;
                    await Navigateur.Courant.Avertir(
                        "Échec de connexion",
                        "Le courriel ou le mot de passe est refusé. Aucune session n'est ouverte. Ce dialogue illustre le scénario d'échec, sans vérification réelle.");
                })));
    }

    static bool ChampsRemplis(TextBox courriel, TextBox motDePasse, TextBlock erreur)
    {
        if (!string.IsNullOrWhiteSpace(courriel.Text) && !string.IsNullOrWhiteSpace(motDePasse.Text))
        {
            erreur.IsVisible = false;
            return true;
        }

        erreur.Text = "Renseignez le courriel et le mot de passe avant de montrer un résultat.";
        erreur.IsVisible = true;
        return false;
    }
}

public sealed class EcranMenuGestion : EcranPrototype
{
    public override string Titre => "Gestion du stationnement";
    public override string Fil => "Accueil  /  Gestionnaire";
    public override string Contexte => "Session ouverte";
    public override string Indice => "Cas « Gérer le stationnement ». Chaque fonction ouvre son écran.";

    public EcranMenuGestion()
    {
        Content = UiKit.Page(880,
            UiKit.Titre($"Bonjour, {Demo.Gestionnaire}"),
            UiKit.Fiche(
                ("Matricule", Demo.Matricule),
                ("Courriel", Demo.Courriel),
                ("Téléphone", Demo.Telephone),
                ("Adresse", Demo.AdresseEmploye),
                ("Session ouverte", Demo.Connexion)),
            GrilleActions(),
            UiKit.Actions(UiKit.Bouton("Déconnexion", "secondary", async () =>
            {
                await Navigateur.Courant.Avertir(
                    "Session fermée",
                    $"Déconnexion affichée pour {Demo.Gestionnaire}. Aucune session n'est conservée.");
                Navigateur.Courant.Accueil();
            })));
    }

    static Control GrilleActions()
    {
        var grille = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,*"),
            RowDefinitions = new RowDefinitions("Auto,Auto")
        };
        Poser(grille, 0, 0, "État du stationnement", "Places, tarifs en vigueur, bornes et barrières.", () => Navigateur.Courant.Aller(new EcranEtat()));
        Poser(grille, 1, 0, "Modifier les tarifs", "Tarif horaire et plafond, en semaine et en fin de semaine.", () => Navigateur.Courant.Aller(new EcranTarifs()));
        Poser(grille, 0, 1, "États statistiques", "Vues quotidienne, hebdomadaire et mensuelle.", () => Navigateur.Courant.Aller(new EcranChoixStats()));
        return grille;
    }

    static void Poser(Grid grille, int colonne, int ligne, string titre, string texte, Action clic)
    {
        var carte = UiKit.Carte(new StackPanel
        {
            Spacing = 10,
            Children =
            {
                new TextBlock { Text = titre, FontSize = 18, FontWeight = FontWeight.SemiBold, Foreground = UiKit.Teal, TextWrapping = TextWrapping.Wrap },
                new TextBlock { Text = texte, FontSize = 14, Foreground = UiKit.Secondaire, TextWrapping = TextWrapping.Wrap, MinHeight = 44 },
                UiKit.Bouton("Ouvrir", "primary", clic)
            }
        });
        carte.Margin = new Thickness(7);
        Grid.SetColumn(carte, colonne);
        Grid.SetRow(carte, ligne);
        grille.Children.Add(carte);
    }
}

public sealed class EcranEtat : EcranPrototype
{
    public override string Titre => "État du stationnement";
    public override string Fil => "Accueil  /  Gestionnaire  /  État";
    public override string Contexte => "Session ouverte";
    public override string Indice => "Cas « Consulter l'état du stationnement ». Lecture seule.";

    public EcranEtat()
    {
        Content = UiKit.Page(800,
            UiKit.Titre(Demo.Stationnement),
            UiKit.Indicateurs(
                ("Places occupées", $"{Demo.Vehicules} / {Demo.Capacite}"),
                ("Places libres", $"{Demo.Capacite - Demo.Vehicules}"),
                ("Borne d'entrée", "Active"),
                ("Borne de sortie", "Active")),
            UiKit.Fiche(
                ("Numéro", Demo.NumeroStationnement),
                ("Description", Demo.Description),
                ("Adresse", Demo.Adresse),
                ("Capacité", Demo.Capacite.ToString()),
                ("Véhicules stationnés", Demo.Vehicules.ToString()),
                ("Tarif horaire, semaine", Demo.TarifHeureSemaine),
                ("Tarif horaire, fin de semaine", Demo.TarifHeureFinSemaine),
                ("Plafond, semaine", Demo.TarifMaxSemaine),
                ("Plafond, fin de semaine", Demo.TarifMaxFinSemaine),
                ("Borne d'entrée", $"{Demo.BorneEntree} — en service"),
                ("Barrière d'entrée", $"{Demo.BarriereEntree} — baissée"),
                ("Borne de sortie", $"{Demo.BorneSortie} — en service"),
                ("Barrière de sortie", $"{Demo.BarriereSortie} — baissée")));
    }
}

public sealed class EcranTarifs : EcranPrototype
{
    public override string Titre => "Modifier les tarifs";
    public override string Fil => "Accueil  /  Gestionnaire  /  Tarifs";
    public override string Contexte => "Session ouverte";
    public override string Indice => "Cas « Modifier les tarifs ». La confirmation ne conserve pas les valeurs.";

    public EcranTarifs()
    {
        var heureSemaine = UiKit.Zone("0,00 $", Demo.TarifHeureSemaine);
        var heureFin = UiKit.Zone("0,00 $", Demo.TarifHeureFinSemaine);
        var maxSemaine = UiKit.Zone("0,00 $", Demo.TarifMaxSemaine);
        var maxFin = UiKit.Zone("0,00 $", Demo.TarifMaxFinSemaine);
        var erreur = UiKit.ErrorLine();

        Content = UiKit.Page(720,
            UiKit.Titre("Tarifs du stationnement"),
            UiKit.Lead($"Valeurs affichées pour {Demo.Stationnement}. Elles ne sont pas enregistrées."),
            UiKit.Carte(new StackPanel
            {
                Spacing = 14,
                Children =
                {
                    UiKit.Champ("Tarif horaire, jour de semaine", heureSemaine),
                    UiKit.Champ("Tarif horaire, fin de semaine", heureFin),
                    UiKit.Champ("Plafond journalier, semaine", maxSemaine),
                    UiKit.Champ("Plafond journalier, fin de semaine", maxFin)
                }
            }),
            erreur,
            UiKit.Actions(UiKit.Bouton("Enregistrer les tarifs", "primary", async () =>
            {
                var zones = new[] { heureSemaine, heureFin, maxSemaine, maxFin };
                if (zones.Any(zone => string.IsNullOrWhiteSpace(zone.Text)))
                {
                    erreur.Text = "Indiquez les quatre tarifs pour afficher la confirmation.";
                    erreur.IsVisible = true;
                    return;
                }

                erreur.IsVisible = false;
                await Navigateur.Courant.Avertir(
                    "Tarifs présentés",
                    $"Aperçu non conservé : {heureSemaine.Text?.Trim()} / h en semaine, {heureFin.Text?.Trim()} / h en fin de semaine, plafonds {maxSemaine.Text?.Trim()} et {maxFin.Text?.Trim()}.");
            })));
    }
}

public sealed class EcranChoixStats : EcranPrototype
{
    public override string Titre => "États statistiques";
    public override string Fil => "Accueil  /  Gestionnaire  /  Statistiques";
    public override string Contexte => "Session ouverte";
    public override string Indice => "Cas « Consulter les états statistiques », puis la période voulue.";

    public EcranChoixStats()
    {
        Content = UiKit.Page(760,
            UiKit.Titre("Choisir une période"),
            UiKit.Lead("Les trois vues spécialisent la consultation des statistiques."),
            UiKit.Actions(
                UiKit.Bouton("État quotidien", "primary", () => Navigateur.Courant.Aller(new EcranStats("jour"))),
                UiKit.Bouton("État hebdomadaire", "secondary", () => Navigateur.Courant.Aller(new EcranStats("semaine"))),
                UiKit.Bouton("État mensuel", "secondary", () => Navigateur.Courant.Aller(new EcranStats("mois")))));
    }
}

public sealed class EcranStats : EcranPrototype
{
    readonly string _periode;

    public override string Titre => _periode switch
    {
        "semaine" => "Statistiques hebdomadaires",
        "mois" => "Statistiques mensuelles",
        _ => "Statistiques quotidiennes"
    };

    public override string Fil => "Accueil  /  Gestionnaire  /  Statistiques  /  " + Titre;
    public override string Contexte => "Session ouverte";
    public override string Indice => "Chiffres de démonstration, identiques à chaque ouverture.";

    public EcranStats(string periode)
    {
        _periode = periode;
        var (intervalle, indicateurs, entetes, lignes) = Donnees(periode);
        Content = UiKit.Page(920,
            UiKit.Titre(Titre),
            UiKit.Lead(intervalle),
            UiKit.Indicateurs(indicateurs),
            UiKit.Tableau(entetes, lignes),
            AutresPeriodes());
    }

    static Control AutresPeriodes()
    {
        var jour = UiKit.Bouton("Quotidien", "quiet", () => Navigateur.Courant.Aller(new EcranStats("jour")));
        var semaine = UiKit.Bouton("Hebdomadaire", "quiet", () => Navigateur.Courant.Aller(new EcranStats("semaine")));
        var mois = UiKit.Bouton("Mensuel", "quiet", () => Navigateur.Courant.Aller(new EcranStats("mois")));
        return new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            Children = { jour, semaine, mois }
        };
    }

    static (string Intervalle, (string, string)[] Indicateurs, string[] Entetes, string[][] Lignes) Donnees(string periode) =>
        periode switch
        {
            "semaine" => (
                "Semaine du 2 au 8 novembre 2026.",
                [("Entrées", "1 184"), ("Sorties", "1 156"), ("Recettes", "8 420 $"), ("Occupation moy.", "64 %")],
                ["Jour", "Entrées", "Sorties", "Recettes"],
                [
                    ["Lundi 2 nov.", "186", "171", "1 240 $"],
                    ["Mardi 3 nov.", "174", "169", "1 180 $"],
                    ["Mercredi 4 nov.", "191", "188", "1 310 $"],
                    ["Jeudi 5 nov.", "168", "160", "1 090 $"],
                    ["Vendredi 6 nov.", "203", "198", "1 460 $"],
                    ["Samedi 7 nov.", "142", "147", "1 120 $"],
                    ["Dimanche 8 nov.", "120", "123", "1 020 $"]
                ]),
            "mois" => (
                "Novembre 2026, données partielles de démonstration.",
                [("Entrées", "4 860"), ("Sorties", "4 702"), ("Recettes", "34 150 $"), ("Occupation moy.", "61 %")],
                ["Semaine", "Entrées", "Sorties", "Recettes"],
                [
                    ["1er au 2 nov.", "360", "340", "2 480 $"],
                    ["2 au 8 nov.", "1 184", "1 156", "8 420 $"],
                    ["9 au 15 nov.", "1 210", "1 188", "8 640 $"],
                    ["16 au 22 nov.", "1 096", "1 070", "7 910 $"],
                    ["23 au 30 nov.", "1 010", "948", "6 700 $"]
                ]),
            _ => (
                "Jeudi 5 novembre 2026.",
                [("Entrées", "168"), ("Sorties", "160"), ("Recettes", "1 090 $"), ("Occupation max.", "78 %")],
                ["Heure", "Entrées", "Sorties", "Recettes"],
                [
                    ["7 h - 9 h", "46", "12", "180 $"],
                    ["9 h - 12 h", "38", "29", "260 $"],
                    ["12 h - 14 h", "31", "34", "240 $"],
                    ["14 h - 17 h", "29", "41", "250 $"],
                    ["17 h - 20 h", "24", "44", "160 $"]
                ])
        };
}
