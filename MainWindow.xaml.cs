using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aviron_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // Pour lier les propriétés au DataContext de la fenêtre
            DateDebut = DateTime.Now;
            DateFin = DateTime.Now;

            // Remplir les ComboBox pour les heures et les minutes
            RemplirComboBoxHeures(hourPickerDebut);
            RemplirComboBoxHeures(hourPickerFin);
            RemplirComboBoxMinutes(minutePickerDebut);
            RemplirComboBoxMinutes(minutePickerFin);

            LoadStatisticsHistory();
        }

        private void RemplirComboBoxHeures(ComboBox comboBox)
        {
            for (int i = 0; i < 24; i++)
            {
                comboBox.Items.Add(i.ToString("00")); // Ajouter les heures de 00 à 23
            }
        }

        private void RemplirComboBoxMinutes(ComboBox comboBox)
        {
            for (int i = 0; i < 60; i++)
            {
                comboBox.Items.Add(i.ToString("00")); // Ajouter les minutes de 00 à 59
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Récupérez les valeurs sélectionnées dans les DatePicker et les TimePicker
            DateTime dateDebut = datePickerDebut.SelectedDate.Value.Date
                                + new TimeSpan(Convert.ToInt32(hourPickerDebut.SelectedValue), Convert.ToInt32(minutePickerDebut.SelectedValue), 0);
            DateTime dateFin = datePickerFin.SelectedDate.Value.Date
                                + new TimeSpan(Convert.ToInt32(hourPickerFin.SelectedValue), Convert.ToInt32(minutePickerFin.SelectedValue), 0);

            // Créer une nouvelle instance de la fenêtre StatistiquesWindow
            StatistiquesWindow statistiquesWindow = new StatistiquesWindow(dateDebut, dateFin);

            // Afficher la nouvelle fenêtre
            statistiquesWindow.Show();
        }

        private async void LoadStatisticsHistory()
        {
            try
            {
                string statisticsJson = await GetAllStatisticsJson();
                if (!string.IsNullOrEmpty(statisticsJson))
                {
                    // Effacez d'abord tous les éléments précédents dans la ListBox
                    historiqueListBox.Items.Clear();

                    // Analyser le JSON en tant que document JSON
                    using (JsonDocument doc = JsonDocument.Parse(statisticsJson))
                    {
                        // Vérifiez que le document est un tableau JSON
                        if (doc.RootElement.ValueKind == JsonValueKind.Array)
                        {
                            // Parcourez chaque élément du tableau
                            foreach (JsonElement element in doc.RootElement.EnumerateArray())
                            {
                                // Extrayez les valeurs des propriétés "dateDebut" et "dateFin" et convertissez-les en DateTime
                                if (element.TryGetProperty("dateDebut", out JsonElement dateDebutElement) &&
                                    element.TryGetProperty("dateFin", out JsonElement dateFinElement))
                                {
                                    if (DateTime.TryParse(dateDebutElement.GetString(), out DateTime dateDebut) &&
                                        DateTime.TryParse(dateFinElement.GetString(), out DateTime dateFin))
                                    {
                                        // Ajoutez les valeurs extraites à la ListBox
                                        historiqueListBox.Items.Add($"Du {dateDebut} au {dateFin}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private async Task<string> GetAllStatisticsJson()
        {
            try
            {
                // Créer une instance de votre client
                Client apiClient = new Client();

                // Récupérer les statistiques à partir de la méthode GetAll
                var statistics = apiClient.GetAll();

                // Convertir les statistiques en une chaîne JSON à l'aide de Newtonsoft.Json
                string statisticsJson = JsonConvert.SerializeObject(statistics);

                return statisticsJson;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
                return null;
            }
        }
        private void historiqueListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtenez la statistique sélectionnée dans la ListBox
            string selectedStat = historiqueListBox.SelectedItem as string;

            // Vérifiez si une statistique est sélectionnée
            if (!string.IsNullOrEmpty(selectedStat))
            {
                // Divisez la chaîne sélectionnée pour extraire les datesù
                // La chaîne est de la forme "Du {dateDebut} au {dateFin}" 
                // On supprime les parties "Du " pour obtenir "{dateDebut} au {dateFin}"
                string selectedStat2 = selectedStat.Substring(3);

                string[] dates = selectedStat2.Split(new string[] { " au " }, StringSplitOptions.RemoveEmptyEntries);

                // Vérifiez si les deux dates ont été correctement extraites
                if (dates.Length == 2)
                {
                    // Convertissez les chaînes de date en objets DateTime
                    if (DateTime.TryParse(dates[0], out DateTime dateDebut) &&
                        DateTime.TryParse(dates[1], out DateTime dateFin))
                    {
                        // Créez une nouvelle instance de StatistiquesWindow avec les dates sélectionnées
                        StatistiquesWindow statistiquesWindow = new StatistiquesWindow(dateDebut, dateFin);

                        // Affichez la nouvelle fenêtre
                        statistiquesWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la conversion des dates.");
                    }
                }
                else
                {
                    MessageBox.Show("Format de date invalide dans la sélection.");
                }
            }
        }
    }
}