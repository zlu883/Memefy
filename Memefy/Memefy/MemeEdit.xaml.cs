using Memefy.Model;
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
    public partial class MemeEdit : ContentPage
    {
        bool IsAdd;
        MemeListViewModel MemeList;
        MemeCaptions CurrentlyEditingMeme;

        public MemeEdit(MemeCaptions meme, bool isAdd, MemeListViewModel memeList)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            IsAdd = isAdd;
            MemeList = memeList;

            if (isAdd)
            {
                // this is work around for slider bug in Xamarin, valueChanged not firing on first time tap
                neutralSlider.Value = 0.01;
                happinessSlider.Value = 0.01;
                sadnessSlider.Value = 0.01;
                angerSlider.Value = 0.01;
                fearSlider.Value = 0.01;
                contemptSlider.Value = 0.01;
                disgustSlider.Value = 0.01;
                surpriseSlider.Value = 0.01;
            }
            else
            {
                CurrentlyEditingMeme = meme;

                lowerCaptionEditor.Text = meme.LowerCaption;
                upperCaptionEditor.Text = meme.UpperCaption;

                neutralSlider.Value = meme.NeutralVal;
                happinessSlider.Value = meme.HappinessVal;
                sadnessSlider.Value = meme.SadnessVal;
                angerSlider.Value = meme.AngerVal;
                fearSlider.Value = meme.FearVal;
                contemptSlider.Value = meme.ContemptVal;
                disgustSlider.Value = meme.DisgustVal;
                surpriseSlider.Value = meme.SurpriseVal;

                // this is work around for slider bug in Xamarin, valueChanged not firing on first time tap
                if (neutralSlider.Value == 0)
                    neutralSlider.Value = 0.01;
                if (happinessSlider.Value == 0)
                    happinessSlider.Value = 0.01;
                if (sadnessSlider.Value == 0)
                    sadnessSlider.Value = 0.01;
                if (angerSlider.Value == 0)
                    angerSlider.Value = 0.01;
                if (fearSlider.Value == 0)
                    fearSlider.Value = 0.01;
                if (contemptSlider.Value == 0)
                    contemptSlider.Value = 0.01;
                if (disgustSlider.Value == 0)
                    disgustSlider.Value = 0.01;
                if (surpriseSlider.Value == 0)
                    surpriseSlider.Value = 0.01;

                deleteButton.IsEnabled = true;
            }
        }

        private async void SaveMeme(object sender, EventArgs e)
        {
            ShowActivityIndicator();

            if (IsAdd)
            {
                MemeCaptions newMeme = new MemeCaptions();

                newMeme.UpperCaption = upperCaptionEditor.Text;
                newMeme.LowerCaption = lowerCaptionEditor.Text;
                newMeme.computeFullCaption();

                newMeme.NeutralVal = neutralSlider.Value;
                newMeme.SadnessVal = sadnessSlider.Value;
                newMeme.HappinessVal = happinessSlider.Value;
                newMeme.DisgustVal = disgustSlider.Value;
                newMeme.ContemptVal = contemptSlider.Value;
                newMeme.AngerVal = angerSlider.Value;
                newMeme.FearVal = fearSlider.Value;
                newMeme.SurpriseVal = surpriseSlider.Value;

                MemeList.Items.Add(newMeme);

                await AzureManager.AzureManagerInstance.CreateNewMeme(newMeme);
            }
            else
            {
                CurrentlyEditingMeme.UpperCaption = upperCaptionEditor.Text;
                CurrentlyEditingMeme.LowerCaption = lowerCaptionEditor.Text;
                CurrentlyEditingMeme.computeFullCaption();

                CurrentlyEditingMeme.NeutralVal = neutralSlider.Value;
                CurrentlyEditingMeme.SadnessVal = sadnessSlider.Value;
                CurrentlyEditingMeme.HappinessVal = happinessSlider.Value;
                CurrentlyEditingMeme.DisgustVal = disgustSlider.Value;
                CurrentlyEditingMeme.ContemptVal = contemptSlider.Value;
                CurrentlyEditingMeme.AngerVal = angerSlider.Value;
                CurrentlyEditingMeme.FearVal = fearSlider.Value;
                CurrentlyEditingMeme.SurpriseVal = surpriseSlider.Value;

                await AzureManager.AzureManagerInstance.UpdateMeme(CurrentlyEditingMeme);
            }
            await Navigation.PopAsync();

        }

        private async void DeleteMeme(object sender, EventArgs e)
        {
            ShowActivityIndicator();

            MemeList.Items.Remove(CurrentlyEditingMeme);
            await AzureManager.AzureManagerInstance.DeleteMeme(CurrentlyEditingMeme);
            await Navigation.PopAsync();
        }

        void ShowActivityIndicator()
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            scrollView.IsVisible = false;
        }

    }
}