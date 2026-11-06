namespace KalkulatorSerwisuIT
{
    public partial class MainPage : ContentPage
    {
        private decimal cena = 0.00m;


        //rekord przechowujacy zmienne do listy
        private record Znizka(string kodRabatowy, decimal znizkaProcentowa);

        //Lista do której można dodać kody i ilosc znizki procentowej z nimi zwiazanymi
        private List<Znizka> kodyRabatowe = new() { new("TECHNIK10", 10.00m) };
        public MainPage()
        {
            InitializeComponent();
        }

        private void WyczyscFormularz(object? sender, EventArgs e)
        {
            entryImie.Text = string.Empty;
            entryModel.Text = string.Empty;

            radioDiagnostyka.IsChecked = true;
            radioInstalacja.IsChecked = false;
            radioNaprawa.IsChecked = false;

            checkBoxKopia.IsChecked = false;
            checkBoxPasta.IsChecked = false;

            switchEkspres.IsToggled = false;

            entryRabat.Text = string.Empty;

            cena = 0.00m;

            lblPodsumowanie.Text = "Wprowadź dane i kliknij Oblicz...";
            lblPodsumowanie.TextColor = Colors.Black;
        }
        private void ObliczPodsumuj(object? sender, EventArgs e)
        {
            if (Waliduj(sender, e))
            {
                cena = 0.00m;

                if (radioDiagnostyka.IsChecked) { cena += 100.00m; }
                else if (radioInstalacja.IsChecked) { cena += 150.00m; }
                else if (radioNaprawa.IsChecked) { cena += 250.00m; }


                if (checkBoxKopia.IsChecked) { cena += 80.00m; }

                if (checkBoxPasta.IsChecked) { cena += 40.00m; }

                // Usługa ekspresowa - dodatkowe 50%.
                if (switchEkspres.IsToggled)
                {
                    cena += cena * 0.50m;
                }

                //?? "" zeby nie sprawdzac z nullem pozniej na wypadek
                string wpisanyKod = entryRabat.Text?.Trim().ToUpper() ?? "";

                //sprawdzanie formularza z lista kodow o znizke
                foreach (Znizka rabat in kodyRabatowe)
                {
                    if (wpisanyKod == rabat.kodRabatowy)
                    {
                        cena -= cena * (rabat.znizkaProcentowa / 100m);
                        break;
                    }
                }

                lblPodsumowanie.TextColor = Colors.Green;

                lblPodsumowanie.Text = $"Klient: {entryImie.Text}\n" +
                                       $"Urządzenie: {entryModel.Text}\n" +
                                       $"Cena: {cena:F2} zł";
            }
            else
            {
                lblPodsumowanie.TextColor = Colors.Red;
            }
        }

        private async void ZatwierdzZamowienie(object sender, EventArgs e)
        {
            if (Waliduj(sender, e))
            {
            //mozna wywolac displayalert bez obliczenia ceny w czego wypadku wypisze tu 0 ale to akurat nie jest wymagane w zadaniu
                await DisplayAlertAsync("Zlecenie przyjęte",
                                        $"Klient: {entryImie.Text}\n" +
                                        $"Urządzenie: {entryModel.Text}\n" +
                                        $"Cena: {cena:F2} zł\n" +
                                         "Zlecenie przyjęte",
                                         "OK");
            }
            else
            {
                await DisplayAlertAsync("Błąd", "Wypełnij poprawnie formularz", "OK");
            }
        }
        //funkcja waliduje formularz i zaleznie od tego zwraca odpowiednia wartosc bool
        private bool Waliduj(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entryImie.Text) ||
                !string.IsNullOrEmpty(entryModel.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
