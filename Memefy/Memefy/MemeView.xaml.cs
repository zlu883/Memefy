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
	public partial class MemeView : ContentPage
	{
		public MemeView (MediaFile sourceFile)
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            upperText.FontFamily = Device.OnPlatform(null, "LeagueGothic-Regular.otf#LeagueGothic-Regular", null);

            image.Source = ImageSource.FromStream(() =>
            {
                return sourceFile.GetStream();
            });

		}
	}
}