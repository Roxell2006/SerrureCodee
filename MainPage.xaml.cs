using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace SerrureCodee
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            Label[,] label = new Label[4, 5];
            int offset = 0; // Permet de sauter la deuxième rangée (row)

            // Création de la Grid (dessin du plateau de jeu)
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    // BoxView
                    BoxView boxView = new BoxView { Color = Color.FromRgb(48, 48, 48) };
                    if (r > 0)
                        offset = 1;
                    GameGrid.SetRow(boxView, r + offset);
                    GameGrid.SetColumn(boxView, c);

                    // Label
                    label[c, r] = new Label
                    {
                        Text = c.ToString(),
                        FontSize = 48,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };
                    GameGrid.SetRow(label[c, r], r + offset);
                    GameGrid.SetColumn(label[c, r], c);

                    // Ajoute les enfants(box et label) à la grid 
                    GameGrid.Children.Add(boxView);
                    GameGrid.Children.Add(label[c, r]);
                }
            }

            //mise à jour de la grid dans la ContentPage;
            Content = GameGrid;

            //Change le text du Label 1 (test)
            label[1, 0].Text = "R";

            //Ajoute un cercle sur le label 2 (test)
            AfficheCercle(0, 2);
        }

        void AfficheCercle(int col, int row)
        {
            if (row < 4 && col < 4)
            {
                Ellipse ellipse = new Ellipse
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 60,
                    HeightRequest = 60,
                    Stroke = Brush.Yellow,
                    StrokeThickness = 2,
                    Fill = Brush.Transparent,
                };
                GameGrid.SetRow(ellipse, row + 2);
                GameGrid.SetColumn(ellipse, col);
                GameGrid.Children.Add(ellipse);
                Content = GameGrid;
            }
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Vérifie si l'utilisateur entre bien un nombre
            bool isDigit = true;
            try
            {
                if (e.NewTextValue != null)
                {
                    int value = int.Parse(e.NewTextValue);
                    // vérifie si le code contient 4 chiffres
                    if (EntryCode.Text.Length == 4)
                    {
                        DisplayAlert("Alert", "Le code est: " + EntryCode.Text, "X");
                        EntryCode.Text = null;
                    }
                }
            }
            catch
            {
                isDigit = false;
            }
            if (!isDigit)
                EntryCode.Text = e.OldTextValue;
        }
    }
}
