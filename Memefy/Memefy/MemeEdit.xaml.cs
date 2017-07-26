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
        MemeCaptions Meme;
        MemeCaptions memeToEdit;

        public MemeEdit (MemeCaptions meme, bool isAdd, MemeListViewModel memeList)
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            IsAdd = isAdd;
            MemeList = memeList;

            Meme = new MemeCaptions();
            BindingContext = Meme;
            memeToEdit = meme;
        }

        private void SaveMeme(object sender, EventArgs e)
        {
            if (IsAdd)
            {
                
                MemeList.Items.Add(Meme);
            }
            else
            {

            }
            Navigation.PopAsync();

        }
    }
}