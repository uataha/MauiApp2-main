using MauiApp2.Model;
using System;
namespace MauiApp2;

public partial class Login : ContentPage
{
	public Login()
    {
        InitializeComponent();
    }

    private void KayitLoginEkraniGoster(object sender, EventArgs e)
    {
        if (kayitEkran.IsVisible)
        {
            kayitEkran.IsVisible = false;
            loginEkran.IsVisible = true;
            lblTitle.Text = "Giriþ Yapýnýz";
        }
        else
        {
            loginEkran.IsVisible = false;
            kayitEkran.IsVisible = true;
            lblTitle.Text = "Kayýt Olunuz";
        }
    }

        private async void RegisterClicked(object sender, EventArgs e)
    {
        var user = await FireBaseServices.Register(txtName.Text, txtREmail.Text, txtRPassword.Text);
        if (user != null)
        {
            await DisplayAlert("Baþarýlý", "Kayýt baþarýlý", "OK");
            KayitLoginEkraniGoster(sender, e);
        }
        else
        {
            await DisplayAlert("Hata", "Kayýt baþarýsýz", "OK");
        }
    }

    private async void LoginInClicked(object sender, EventArgs e)
    {
        var user = await FireBaseServices.Login(txtEmail.Text, txtPassword.Text);
        if (user != null)
        {
            await DisplayAlert($"Hoþgeldin {user.Info.DisplayName}", "Giriþ yaptýnýz", "OK");
            ((App)Application.Current).NavigateToMainPage();
        }
        else
        {
            await DisplayAlert("Hata", "Kullanýcý adý veya þifre hatalý", "OK");
        }
    }

}
