using Memefy.Model;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Memefy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        public MenuPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            image.Source = ImageSource.FromFile("defaultface.png");
        }

        private async void MemefyPhoto(object sender, EventArgs e)
        {
                await this.Navigation.PushAsync(new MemeView());
        }

        private async void ShowList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MemeList());
        }

    }
}