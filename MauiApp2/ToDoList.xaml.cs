using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class ToDoList : ContentPage
    {
        private ObservableCollection<TaskItem> tasks;
        private FirebaseClient firebaseClient;

        public ToDoList()
        {
            InitializeComponent();
            BindingContext = this;
            firebaseClient = new FirebaseClient("https://mauiapp-61414-default-rtdb.firebaseio.com/");
            LoadTasks();
        }

        private async void LoadTasks()
        {
            try
            {
                var taskItems = await firebaseClient.Child("tasks").OnceAsync<TaskItem>();
                tasks = new ObservableCollection<TaskItem>();

                foreach (var item in taskItems)
                {
                    tasks.Add(new TaskItem
                    {
                        Baslik = item.Object.Baslik,
                        Yapilacak = item.Object.Yapilacak,
                        Tarih = item.Object.Tarih.Date, // Sadece tarih kýsmý alýný
                        Saat = item.Object.Saat,
                    });
                }

                YapilacaklarListView.ItemsSource = tasks;
                YapilacaklarLayout.IsVisible = tasks.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

        private async void SaveTasks()
        {
            try
            {
                foreach (var task in tasks)
                {
                    await firebaseClient.Child("tasks").PostAsync(new TaskItem
                    {
                        Baslik = task.Baslik,
                        Yapilacak = task.Yapilacak,
                        Tarih = task.Tarih.Date, // Sadece tarih kýsmý kaydedilir
                        Saat = task.Saat,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            YapilacaklarLayout.IsVisible = !YapilacaklarLayout.IsVisible;
        }

        private void KaydetButton_Clicked(object sender, EventArgs e)
        {
            if (tasks == null)
            {
                tasks = new ObservableCollection<TaskItem>();
            }

            var task = new TaskItem
            {
                Baslik = BaslikEntry.Text,
                Yapilacak = YapilacakEntry.Text,
                Tarih = TarihPicker.Date.Date,
                Saat = SaatPicker.Time
            };

            tasks.Add(task);
            SaveTasks();
            BaslikEntry.Text = string.Empty;
            YapilacakEntry.Text = string.Empty;
            YapilacaklarListView.ItemsSource = tasks;
        }

        private void SelectButton_Clicked(object sender, EventArgs e)
        {
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var selectedTasks = tasks.Where(t => t.IsSelected).ToList();
            foreach (var task in selectedTasks)
            {
                var firebaseObject = (await firebaseClient.Child("tasks")
                                            .OnceAsync<TaskItem>())
                                            .FirstOrDefault(a => a.Object.Baslik == task.Baslik &&
                                                                 a.Object.Yapilacak == task.Yapilacak &&
                                                                 a.Object.Tarih == task.Tarih &&
                                                                 a.Object.Saat == task.Saat);

                if (firebaseObject != null)
                {
                    await firebaseClient.Child("tasks").Child(firebaseObject.Key).DeleteAsync();
                    tasks.Remove(task);
                }
            }
            YapilacaklarListView.ItemsSource = tasks;
        }

        public class TaskItem
        {
            public string Baslik { get; set; }
            public string Yapilacak { get; set; }
            public DateTime Tarih { get; set; }
            public TimeSpan Saat { get; set; }
            public bool IsSelected { get; set; }

            public string TarihFormatted => Tarih.ToString("dd.MM.yyyy");
            public string SaatFormatted => Saat.ToString(@"hh\:mm");
        }
    }
}
