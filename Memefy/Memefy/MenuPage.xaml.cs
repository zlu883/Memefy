using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private async void LoadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Directory = "Sample",
                    Name = $"{DateTime.UtcNow}.jpg"
                });

                if (file == null)
                    return;

                await this.Navigation.PushAsync(new MemeView(file));

                file.Dispose();
            }
            else
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
        }
    }
}