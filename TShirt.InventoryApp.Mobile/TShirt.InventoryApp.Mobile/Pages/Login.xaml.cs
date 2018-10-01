using System;
using TShirt.InventoryApp.Services.Mobile.Services;
using TShirt.InventoryApp.Services.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TShirt.InventoryApp.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {

        private UserServices services;
        private User user;
        public Login()
        {
            InitializeComponent();
            services = new UserServices();
            user= new User();
            ImgLogin.Source = ImageSource.FromFile("Images/Logo.png");
        }

        private async void EnterButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(EntUser.Text) || string.IsNullOrEmpty(EntPassword.Text))
                {
                    Lblmsg.Text = "Debe ingresar nombre de usuario y Password";
                }
                else
                {

                    var _user = EntUser.Text.Trim();
                    var _pass = EntPassword.Text.Trim();

                    var result = await services.GetProviderName(_user, _pass);

                    if (result == null)
                        Lblmsg.Text = "Ha ingresado un Usuario o Password icorrecto";
                    else
                        ((NavigationPage)this.Parent).PushAsync(new Page2());
                    //{
                    //    Lblmsg.Text = result.Name;
                    //}
                    //    //((NavigationPage)this.Parent).PushAsync(new Page2());

                }
            }
            catch (Exception ex)
            {
                Lblmsg1.Text =  "Error Label: " + ex.Message.ToString();            
            }          
        }
    }
}
