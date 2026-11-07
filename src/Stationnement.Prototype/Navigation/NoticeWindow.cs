namespace Stationnement.Prototype.Navigation;

public sealed class NoticeWindow : Window
{
    public NoticeWindow(string titre, string message)
    {
        Title = titre;
        Width = 480;
        MinHeight = 220;
        SizeToContent = SizeToContent.Height;
        CanResize = false;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Background = Ui.UiKit.Brush("#F7F4EF");
        SystemDecorations = SystemDecorations.BorderOnly;

        var fermer = Ui.UiKit.Bouton("Fermer", "primary", Close);
        fermer.HorizontalAlignment = HorizontalAlignment.Right;
        fermer.MinWidth = 140;

        Content = new Border
        {
            Padding = new Thickness(28),
            Child = new StackPanel
            {
                Spacing = 16,
                Children =
                {
                    new TextBlock
                    {
                        Text = titre,
                        FontSize = 22,
                        FontWeight = FontWeight.SemiBold,
                        Foreground = Ui.UiKit.Teal,
                        TextWrapping = TextWrapping.Wrap
                    },
                    new TextBlock
                    {
                        Text = message,
                        FontSize = 15,
                        Foreground = Ui.UiKit.Encre,
                        TextWrapping = TextWrapping.Wrap
                    },
                    fermer
                }
            }
        };
    }
}
