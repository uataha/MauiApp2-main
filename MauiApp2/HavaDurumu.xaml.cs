using System.Collections.ObjectModel;

namespace MauiApp2;

public partial class HavaDurumu : ContentPage
{
    public ObservableCollection<SehirModel> ImageList { get; set; } = new ObservableCollection<SehirModel>();

    public HavaDurumu()
    {
        InitializeComponent();
        ImageCollection.ItemsSource = ImageList;
    }

    public void OnButtonClick(object sender, EventArgs e)
    {
        _ = a(); 
    }

    public async Task a()
    {
        string sehir = await DisplayPromptAsync("Þehir:", "Þehir ismi", "OK", "Cancel");

        if (!string.IsNullOrEmpty(sehir))
        {
            sehir = sehir.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
            sehir = sehir.Replace('Ç', 'C');
            sehir = sehir.Replace('Ð', 'G');
            sehir = sehir.Replace('Ý', 'I');
            sehir = sehir.Replace('Ö', 'O');
            sehir = sehir.Replace('Ü', 'U');
            sehir = sehir.Replace('Þ', 'S');

            var sehirModel = new SehirModel { Name = sehir };
            ImageList.Add(sehirModel);

            string src = sehirModel.Source;

            Console.WriteLine(src);
        }
    }

    private void ImageCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedSehirModel = e.CurrentSelection[0] as SehirModel;
            if (selectedSehirModel != null)
            {
                string selectedImageUrl = selectedSehirModel.Source;
                Console.WriteLine(selectedImageUrl);

                if (!string.IsNullOrEmpty(selectedImageUrl))
                {
                    DisplayAlert("Seçilen Resim", $"Seçilen Resim: {selectedImageUrl}", "Tamam");
                }
            }

            ImageCollection.SelectedItem = null;
        }
    }

    private async void OnRefreshButtonClick(object sender, EventArgs e)
    {
        foreach (var sehir in ImageList)
        {
            string src = sehir.Source;
            Console.WriteLine($"Yenilendi: {src}"); 
        }

        await DisplayAlert("Yenile", "Hava durumu verileri yenilenmiþtir.", "Tamam");
    }
}
