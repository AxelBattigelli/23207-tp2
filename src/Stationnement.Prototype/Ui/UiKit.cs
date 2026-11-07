namespace Stationnement.Prototype.Ui;

public static class UiKit
{
    public static readonly SolidColorBrush Encre = Brush("#122029");
    public static readonly SolidColorBrush Secondaire = Brush("#52606B");
    public static readonly SolidColorBrush Teal = Brush("#0F3D4C");
    public static readonly SolidColorBrush Or = Brush("#8A5A00");
    public static readonly SolidColorBrush FondOr = Brush("#FFF6E0");
    public static readonly SolidColorBrush Ligne = Brush("#E4EBEF");

    public static SolidColorBrush Brush(string hex) => new(Color.Parse(hex));

    public static Control Page(double maxWidth, params Control[] children)
    {
        var pile = new StackPanel
        {
            Spacing = 16,
            MaxWidth = maxWidth,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        foreach (var enfant in children)
            pile.Children.Add(enfant);
        return pile;
    }

    public static TextBlock Titre(string texte) => new()
    {
        Text = texte,
        FontSize = 28,
        FontWeight = FontWeight.SemiBold,
        Foreground = Teal,
        TextWrapping = TextWrapping.Wrap
    };

    public static TextBlock Lead(string texte) => new()
    {
        Text = texte,
        FontSize = 16,
        Foreground = Secondaire,
        TextWrapping = TextWrapping.Wrap,
        MaxWidth = 720
    };

    public static TextBlock ErrorLine(string texte = "") => new()
    {
        Text = texte,
        FontSize = 14,
        Foreground = Brush("#9F1239"),
        TextWrapping = TextWrapping.Wrap,
        IsVisible = false
    };

    public static Button Bouton(string texte, string classe, Action clic)
    {
        var bouton = new Button
        {
            Content = texte,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        bouton.Classes.Add(classe);
        bouton.Click += (_, _) => clic();
        return bouton;
    }

    public static Control Actions(params Control[] boutons)
    {
        var pile = new StackPanel { Spacing = 10, MaxWidth = 460 };
        foreach (var bouton in boutons)
            pile.Children.Add(bouton);
        return pile;
    }

    public static Border Carte(Control contenu, string? fond = null)
    {
        return new Border
        {
            Background = Brush(fond ?? "#FFFFFF"),
            BorderBrush = Ligne,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(14),
            Padding = new Thickness(22),
            BoxShadow = BoxShadows.Parse("0 8 24 0 #140F3D4C"),
            Child = contenu
        };
    }

    public static Control Bandeau(string texte, string ton)
    {
        var (fond, trait, encre) = ton switch
        {
            "erreur" => ("#FFF1F2", "#FECDD3", "#9F1239"),
            "succes" => ("#ECFDF5", "#A7F3D0", "#065F46"),
            "attention" => ("#FFF6E0", "#F3D38A", "#8A5A00"),
            _ => ("#F0F7FA", "#D3E4EC", "#0F3D4C")
        };

        return new Border
        {
            Background = Brush(fond),
            BorderBrush = Brush(trait),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(12),
            Padding = new Thickness(16, 14),
            Child = new TextBlock
            {
                Text = texte,
                FontSize = 15,
                Foreground = Brush(encre),
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeight.Medium
            }
        };
    }

    public static Control LigneInfo(string libelle, string valeur)
    {
        var grille = new Grid { ColumnDefinitions = new ColumnDefinitions("220,*") };
        grille.Children.Add(new TextBlock
        {
            Text = libelle,
            Foreground = Secondaire,
            FontSize = 15,
            VerticalAlignment = VerticalAlignment.Center
        });
        var val = new TextBlock
        {
            Text = valeur,
            Foreground = Encre,
            FontSize = 15,
            FontWeight = FontWeight.SemiBold,
            TextWrapping = TextWrapping.Wrap
        };
        Grid.SetColumn(val, 1);
        grille.Children.Add(val);
        return grille;
    }

    public static Control Fiche(params (string Libelle, string Valeur)[] lignes)
    {
        var pile = new StackPanel { Spacing = 10 };
        foreach (var (libelle, valeur) in lignes)
            pile.Children.Add(LigneInfo(libelle, valeur));
        return Carte(pile);
    }

    public static Control Champ(string libelle, TextBox zone)
    {
        return new StackPanel
        {
            Spacing = 6,
            Children =
            {
                new TextBlock { Text = libelle, FontSize = 13, FontWeight = FontWeight.SemiBold, Foreground = Teal },
                zone
            }
        };
    }

    public static TextBox Zone(string indication, string texte = "", bool secret = false) => new()
    {
        Watermark = indication,
        Text = texte,
        PasswordChar = secret ? '•' : default
    };

    public static Control Indicateurs(params (string Libelle, string Valeur)[] items)
    {
        var grille = new Grid();
        for (var i = 0; i < items.Length; i++)
            grille.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        for (var i = 0; i < items.Length; i++)
        {
            var (libelle, valeur) = items[i];
            var carte = Carte(new StackPanel
            {
                Spacing = 4,
                Children =
                {
                    new TextBlock { Text = libelle, FontSize = 13, Foreground = Secondaire, TextWrapping = TextWrapping.Wrap },
                    new TextBlock { Text = valeur, FontSize = 22, FontWeight = FontWeight.SemiBold, Foreground = Teal }
                }
            });
            carte.Margin = new Thickness(6);
            Grid.SetColumn(carte, i);
            grille.Children.Add(carte);
        }

        return grille;
    }

    public static Control Tableau(string[] entetes, string[][] lignes)
    {
        var grille = new Grid();
        foreach (var _ in entetes)
            grille.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        grille.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        for (var c = 0; c < entetes.Length; c++)
        {
            var cellule = new Border
            {
                Background = Brush("#F4F7F8"),
                Padding = new Thickness(12, 10),
                Child = new TextBlock { Text = entetes[c], FontWeight = FontWeight.SemiBold, Foreground = Teal, FontSize = 13 }
            };
            Grid.SetColumn(cellule, c);
            grille.Children.Add(cellule);
        }

        for (var r = 0; r < lignes.Length; r++)
        {
            grille.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            for (var c = 0; c < lignes[r].Length; c++)
            {
                var cellule = new Border
                {
                    BorderBrush = Ligne,
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Padding = new Thickness(12, 10),
                    Child = new TextBlock { Text = lignes[r][c], FontSize = 14, Foreground = Encre }
                };
                Grid.SetRow(cellule, r + 1);
                Grid.SetColumn(cellule, c);
                grille.Children.Add(cellule);
            }
        }

        return Carte(grille);
    }

    public static Control Ticket(string titre, params (string Libelle, string Valeur)[] lignes)
    {
        var pile = new StackPanel { Spacing = 8 };
        pile.Children.Add(new TextBlock
        {
            Text = titre,
            FontSize = 13,
            FontWeight = FontWeight.Bold,
            Foreground = Or,
            LetterSpacing = 1
        });
        foreach (var (libelle, valeur) in lignes)
            pile.Children.Add(LigneInfo(libelle, valeur));

        return new Border
        {
            Background = FondOr,
            BorderBrush = Brush("#E7C56A"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(14),
            Padding = new Thickness(22),
            Child = pile
        };
    }
}
