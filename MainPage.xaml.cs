using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace SerrureCodee
{
    public partial class MainPage : ContentPage
    {
        Random rand = new Random();
        int code = 0;
        int chance = 0;
        int score = 0;
        Label[,] label = new Label[4, 5];

        public MainPage()
        {
            InitializeComponent();

            // tire un code aléatoire entre 0 et 9999
            code = rand.Next(10000);
            
            int offset = 0; // Permet de sauter la deuxième rangée (row)

            // Création de la Grid (dessin du plateau de jeu et création des labels)
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (r > 0)
                        offset = 1;
                    
                    // BoxView
                    BoxView boxView = new BoxView { Color = Color.FromRgb(48, 48, 48) };
                    GameGrid.SetRow(boxView, r + offset);
                    GameGrid.SetColumn(boxView, c);
 
                    // Label
                    label[c, r] = new Label
                    {
                        Text = "",
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
            try
            {
                if (e.NewTextValue != null)
                {
                    string value = e.NewTextValue;
                    int test = int.Parse(value);
                    // Affiche la proposition
                    label[value.Length -1, chance + 1].Text = value[value.Length-1].ToString();
                    // vérifie si le code contient 4 chiffres
                    if (EntryCode.Text.Length == 4)
                    {
                        //"D4" permet de formater la chaine avec 4 chiffres (ajoute des 0 devant) ex 0576
                        VerifieCode(code.ToString("D4"), EntryCode.Text);
                        EntryCode.Text = null;
                    }
                }
            }
            catch
            {
                // si l'utilisateur a tapé autre chose qu'un chiffre on annule le dernier changement
                EntryCode.Text = e.OldTextValue;
            }
        }

        async void VerifieCode(string code, string proposition)
        {
            for(int i = 0; i< 4; i++)
            {
                if (code.Contains(proposition[i]))
                {
                    if (code[i] == proposition[i])
                        label[i, 0].Text = code[i].ToString();  // Dévoile les bons chiffres
                    else
                        AfficheCercle(i, chance);               // Entoure le chiffre mal placé
                }
            }
            if(code == proposition)
            {
                score += (4 - chance) * 10;
                bool response = await DisplayAlert("Vous Avez Gagné !", "Votre Score: " + score + "\nVoulez-vous rejouer? ", "Oui", "Non");
                if (response)
                    Init();
                else
                    chance = 4;
            }
            else
            {
                chance++;
                if (chance == 4)
                {
                    bool response = await DisplayAlert("Vous Avez Perdu !", "Le code était: " + code + "\nVoulez-vous rejouer? ", "Oui", "Non");
                    if (response)
                        Init();
                }
            }
        }

        void Init()
        {
            // tire un code aléatoire entre 0 et 9999
            code = rand.Next(10000);
            // vide tous les label
            for (int i=0; i< 4; i++)
            {
                for(int j=0; j< 5; j++)
                {
                    label[i, j].Text = "";
                }
            }
            // Efface les Ellipse
            // On excécute la boucle à l'envert avec Reverse() pour éviter des erreurs comme on supprime des éléments à l'intérieur de la boucle
            // LinQ permet de rechercher par Type, on recherche donc un élément de Type Ellipse, si trouvé on l'efface
            foreach (var i in GameGrid.Children.Reverse().Where(x => x.GetType() == typeof(Ellipse)))
            {
                GameGrid.Children.Remove(i);
            }
            // réinitialise les variables:
            chance = 0;
        }
    }
}