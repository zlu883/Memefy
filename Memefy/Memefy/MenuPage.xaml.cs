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
        MobileServiceClient client;

        public MenuPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            image.Source = ImageSource.FromFile("defaultface.png");

            client = AzureManager.AzureManagerInstance.AzureClient;
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

                await RequestEmotionService(file);

            }
            else
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task RequestEmotionService(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "215a704d96294185a8392d0928df1f67");

            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    List<EmotionModel> responseModel = JsonConvert.DeserializeObject<List<EmotionModel>>(responseString);

                }

                file.Dispose();
            }
        }

        private async void ShowList(object sender, EventArgs e)
        {
            List<MemeCaptions> captionsList = await AzureManager.AzureManagerInstance.GetCaptionList();
            await Navigation.PushAsync(new MemeList(captionsList));
        }

    }
}