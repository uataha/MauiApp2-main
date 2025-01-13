using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class Kurlar : ContentPage
    {
        private static Kurlar instance;

        public Kurlar()
        {
            InitializeComponent();
        }

        public static Kurlar Page
        {
            get
            {
                if (instance == null)
                    instance = new Kurlar();
                return instance;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Load();
        }

        private string CalculateFark(string alis, string satis)
        {
            if (decimal.TryParse(satis, out decimal satisValue) && decimal.TryParse(alis, out decimal alisValue))
            {
                decimal fark = satisValue - alisValue;
                return fark.ToString("0.00");
            }
            return "0.00";
        }

        private string GetImage(string deðiþim)
        {
            if (decimal.TryParse(deðiþim.Replace("%", "").Replace(",", "."), out decimal deðiþimValue))
            {
                if (deðiþimValue < 0)
                    return "asagi.png";
                if (deðiþimValue > 0)
                    return "yukari.png";
                return "sabit.png";
            }
            return "";
        }


        AltinDoviz kurlar;

        async Task Load()
        {
            try
            {
                string jsondata = await Dovizler.GetAltinDovizGuncelKurlar();
                kurlar = JsonSerializer.Deserialize<AltinDoviz>(jsondata, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                DovizListe.ItemsSource = new List<DovizK>()
                {
                    new DovizK()
                    {
                        Dname = "Dolar Kuru",
                        Alýþ = kurlar.USD.Alýþ,
                        Satýþ = kurlar.USD.Satýþ,
                        Deðiþim = kurlar.USD.Deðiþim,
                        Yon = GetImage(kurlar.USD.Deðiþim)
                    },
                    new DovizK()
                    {
                        Dname = "Euro Kuru",
                        Alýþ = kurlar.EUR.Alýþ,
                        Satýþ = kurlar.EUR.Satýþ,
                        Deðiþim = kurlar.EUR.Deðiþim,
                        Yon = GetImage(kurlar.EUR.Deðiþim)
                    },

                    new DovizK()
                    {
                        Dname = "GBP Kuru",
                        Alýþ = kurlar.GBP.Alýþ,
                        Satýþ = kurlar.GBP.Satýþ,
                        Deðiþim = kurlar.GBP.Deðiþim,
                        Yon = GetImage(kurlar.GBP.Deðiþim)
                    },

                };
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Load();
        }

        public class DovizK
        {
            public string Dname { get; set; }
            public string Alýþ { get; set; }
            public string Satýþ { get; set; }
            public string Deðiþim { get; set; }
            public string Yon { get; set; }
        }
    }

    public class AltinDoviz
    {
        public USD USD { get; set; }
        public EUR EUR { get; set; }
        public GBP GBP { get; set; }
    }

    public class EUR { public string Satýþ { get; set; } public string Alýþ { get; set; } public string Deðiþim { get; set; } }
    public class GBP { public string Satýþ { get; set; } public string Alýþ { get; set; } public string Deðiþim { get; set; } }
    public class USD { public string Satýþ { get; set; } public string Alýþ { get; set; } public string Deðiþim { get; set; } }
}
