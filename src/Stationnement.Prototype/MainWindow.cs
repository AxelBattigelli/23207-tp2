using Stationnement.Prototype.Navigation;

namespace Stationnement.Prototype;

public sealed class MainWindow : Window
{
    readonly TextBlock _titre;
    readonly TextBlock _fil;
    readonly Border _badge;
    readonly TextBlock _badgeTexte;
    readonly ContentControl _hote;
    readonly TextBlock _indice;
    readonly Button _retour;

    public MainWindow()
    {
        Title = "Simulation des barrières de péage";
        Width = 1180;
        Height = 780;
        MinWidth = 980;
        MinHeight = 680;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Background = new SolidColorBrush(Color.Parse("#F3F5F7"));
        FontFamily = new FontFamily("Inter, Segoe UI, sans-serif");

        _titre = new TextBlock
        {
            FontSize = 22,
            FontWeight = FontWeight.SemiBold,
            Foreground = Brushes.White
        };
        _fil = new TextBlock
        {
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#D5E4EA")),
            Margin = new Thickness(0, 4, 0, 0)
        };
        _badgeTexte = new TextBlock
        {
            FontSize = 13,
            FontWeight = FontWeight.SemiBold,
            Foreground = new SolidColorBrush(Color.Parse("#0F3D4C")),
            VerticalAlignment = VerticalAlignment.Center
        };
        _badge = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#F2C14E")),
            CornerRadius = new CornerRadius(999),
            Padding = new Thickness(12, 6),
            VerticalAlignment = VerticalAlignment.Center,
            Child = _badgeTexte,
            IsVisible = false
        };

        var entete = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#0F3D4C")),
            Padding = new Thickness(28, 16),
            Child = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                Children =
                {
                    new StackPanel
                    {
                        Children =
                        {
                            new TextBlock
                            {
                                Text = "SYSTÈME DE PÉAGE",
                                FontSize = 11,
                                FontWeight = FontWeight.Bold,
                                Foreground = new SolidColorBrush(Color.Parse("#F2C14E")),
                                LetterSpacing = 1.2
                            },
                            _titre,
                            _fil
                        }
                    },
                    _badge
                }
            }
        };
        Grid.SetColumn(_badge, 1);

        _hote = new ContentControl();
        _indice = new TextBlock
        {
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#52606B")),
            VerticalAlignment = VerticalAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(16, 0)
        };
        _retour = new Button { Content = "Retour", MinWidth = 120 };
        _retour.Classes.Add("secondary");
        _retour.Click += (_, _) => Navigateur.Courant.Retour();

        var accueil = new Button { Content = "Accueil", MinWidth = 120 };
        accueil.Classes.Add("secondary");
        accueil.Click += (_, _) => Navigateur.Courant.Accueil();

        var pied = new Border
        {
            Background = Brushes.White,
            BorderBrush = new SolidColorBrush(Color.Parse("#E2E8EE")),
            BorderThickness = new Thickness(0, 1, 0, 0),
            Padding = new Thickness(28, 12),
            Child = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"),
                Children = { _retour, _indice, accueil }
            }
        };
        Grid.SetColumn(_indice, 1);
        Grid.SetColumn(accueil, 2);

        Content = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,*,Auto"),
            Children = { entete, _hote, pied }
        };
        Grid.SetRow(_hote, 1);
        Grid.SetRow(pied, 2);

        Navigateur.Attacher(this);
        Navigateur.Courant.Reinitialiser(new Ecrans.EcranAccueil());
    }

    public void Afficher(EcranPrototype ecran, bool peutRevenir)
    {
        _titre.Text = ecran.Titre;
        _fil.Text = ecran.Fil;
        _indice.Text = ecran.Indice;
        _retour.IsEnabled = peutRevenir;
        _badge.IsVisible = !string.IsNullOrWhiteSpace(ecran.Contexte);
        _badgeTexte.Text = ecran.Contexte;
        _hote.Content = new ScrollViewer
        {
            Content = ecran,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Padding = new Thickness(32, 24)
        };
    }
}
