using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WPFRetard
{
    public partial class WindowStat : Window
    {
        public DateTime DateDebut { get; }
        public DateTime DateFin { get; }

        public WindowStat(DateTime dateDebut, DateTime dateFin)
        {
            InitializeComponent();

            DateDebut = dateDebut;
            DateFin = dateFin;

            // Exemple de données de statistiques (à remplacer par les données réelles récupérées depuis votre API)
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Nombre d'abonnements",
                    Values = new ChartValues<int> { 10, 15, 7, 23 } // Remplacez ces valeurs par vos propres données
                },
                new ColumnSeries
                {
                    Title = "Nombre de créations de compte",
                    Values = new ChartValues<int> { 5, 8, 12, 17 } // Remplacez ces valeurs par vos propres données
                },
                // Ajoutez d'autres séries de données si nécessaire
            };

            // Configurez les libellés de l'axe X
            Labels = new[] { "Catégorie 1", "Catégorie 2", "Catégorie 3", "Catégorie 4" }; // Remplacez ces libellés par les vôtres

            DataContext = this;
        }

        // Propriétés pour lier les données au graphique
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

        // Méthode pour récupérer les statistiques de l'API (à remplacer par votre propre méthode)
        private async Task<string> GetStatistiques(DateTime dateDebut, DateTime dateFin)
        {
            // Ici, vous pouvez appeler votre API pour récupérer les statistiques
            // et retourner les données sous forme de chaîne (ou tout autre type de données)
            return "Statistiques récupérées depuis l'API";
        }

        public void AfficherStatistiques()
        {
            // Affichez les statistiques dans la fenêtre
            // Vous pouvez appeler cette méthode pour afficher les statistiques une fois qu'elles sont récupérées
        }

        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Autres méthodes et événements nécessaires pour votre application


        // Autres méthodes et événements nécessaires pour votre application


    }
}
