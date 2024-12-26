using Divisas.Models;
using Divisas.ViewModels;
using Divisas.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Divisas.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }


        private async void OnLabelTapped(object sender, EventArgs e)
        {
            // URL que deseas abrir
            string url = "https://anthonybvivenes.github.io/";

            // Abre la URL en el navegador
            await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        }

    }
}