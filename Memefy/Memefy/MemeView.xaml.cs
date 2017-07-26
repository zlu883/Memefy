using Memefy.Model;
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
	public partial class MemeView : ContentPage
	{
		public MemeView ()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            upperCaption.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaption.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowTop.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowTopLeft.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowTopRight.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowLeft.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowRight.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowBottom.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowBottomLeft.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            upperCaptionShadowBottomRight.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowTop.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowTopLeft.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowTopRight.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowLeft.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowRight.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowBottom.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowBottomLeft.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);
            lowerCaptionShadowBottomRight.FontFamily = Device.OnPlatform(null, "impact.ttf#Impact", null);

            LoadCamera();
        }

        private async void LoadCamera()
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
                {
                    await DisplayAlert("Alert", "Can't access photo file", "OK");
                    await Navigation.PopAsync();
                    return;
                }

                this.indicator.IsRunning = true;

                image.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });

                await MatchPhotoToMeme(file);

                this.indicator.IsRunning = false;
                this.memePhoto.IsVisible = true;

                file.Dispose();
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

        async Task MatchPhotoToMeme(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "215a704d96294185a8392d0928df1f67");

            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            List<EmotionModel> emotionModels = null;

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    emotionModels = JsonConvert.DeserializeObject<List<EmotionModel>>(responseString);
                }
            }
            
            if (emotionModels == null)
            {
                await DisplayAlert("Connection Error", "Unable to analyse photo", "OK");
                return;
            }
            else if (emotionModels.Count < 1)
            {
                AttachMeme("THIS IMAGE", "IS NOT EVEN WORTH MEMEFYING");
            }
            else
            {
                if (emotionModels.Count > 1)
                {
                    await DisplayAlert("Hey", "Memefy currently works best with only one face in the photo :)", "OK");
                }

                List<MemeCaptions> memeList = await AzureManager.AzureManagerInstance.GetCaptionList();

                double minEmotionDifference = Double.MaxValue;
                double emotionDifference;
                MemeCaptions bestMeme = null;
                foreach (MemeCaptions meme in memeList)
                {
                    emotionDifference = 0;
                    emotionDifference += Math.Abs(meme.AngerVal - emotionModels[0].Scores.Anger);
                    emotionDifference += Math.Abs(meme.NeutralVal - emotionModels[0].Scores.Neutral);
                    emotionDifference += Math.Abs(meme.HappinessVal - emotionModels[0].Scores.Happiness);
                    emotionDifference += Math.Abs(meme.SadnessVal - emotionModels[0].Scores.Sadness);
                    emotionDifference += Math.Abs(meme.ContemptVal - emotionModels[0].Scores.Contempt);
                    emotionDifference += Math.Abs(meme.FearVal - emotionModels[0].Scores.Fear);
                    emotionDifference += Math.Abs(meme.DisgustVal - emotionModels[0].Scores.Disgust);
                    emotionDifference += Math.Abs(meme.SurpriseVal - emotionModels[0].Scores.Surprise);

                    if (emotionDifference < minEmotionDifference)
                    {
                        minEmotionDifference = emotionDifference;
                        bestMeme = meme;
                    }
                }

                if (bestMeme == null)
                {
                    await DisplayAlert("No memes found", "No memes found", "OK");
                    return;
                }
                else
                {
                    AttachMeme(bestMeme.UpperCaption, bestMeme.LowerCaption);
                }
            }
        }

        void AttachMeme(String upper, String lower)
        {
            upperCaption.Text = upper;
            upperCaptionShadowTop.Text = upper;
            upperCaptionShadowTopLeft.Text = upper;
            upperCaptionShadowTopRight.Text = upper;
            upperCaptionShadowLeft.Text = upper;
            upperCaptionShadowRight.Text = upper;
            upperCaptionShadowBottom.Text = upper;
            upperCaptionShadowBottomLeft.Text = upper;
            upperCaptionShadowBottomRight.Text = upper;

            lowerCaption.Text = lower;
            lowerCaptionShadowTop.Text = lower;
            lowerCaptionShadowTopLeft.Text = lower;
            lowerCaptionShadowTopRight.Text = lower;
            lowerCaptionShadowLeft.Text = lower;
            lowerCaptionShadowRight.Text = lower;
            lowerCaptionShadowBottom.Text = lower;
            lowerCaptionShadowBottomLeft.Text = lower;
            lowerCaptionShadowBottomRight.Text = lower;
        }
    }
}