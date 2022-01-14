using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.Services.Interfaces;
using FrontendApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FrontendApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private RegisterViewModel registerViewModel;
        private Stream ImgStream;
        private string FileNameImage;
        public RegisterPage()
        {
            registerViewModel = new RegisterViewModel();
            BindingContext = registerViewModel;
            InitializeComponent();
        }

        private async void register_Clicked(object sender, EventArgs e)
        {
            if (registerViewModel.NextRegister == true)
            {
                registerViewModel.NextRegister = false;
                register.Text = "REGISTER";
            }
            else
            {
                activityIndicator.IsRunning = true;

                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(ImgStream), "file", FileNameImage);

                string urlImage = "https://res.cloudinary.com/uit-information/image/upload/v1640925576/tutuanle/image/upload/" + FileNameImage;

                var httpClientSentImg = new HttpClient();
                //var rs = await httpClientSentImg.PostAsync("http://192.168.1.8:5000/api/message/image", content);




                User user = new User()
                {
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    Passwordd = Password.Text,
                    BirthDate = DateTime.Now,
                    Address1 = Address1.Text,
                    Address2 = Address2.Text,
                    Phone = Phone.Text,
                    ImgURL = urlImage,
                    Gmail = Email.Text
                };

                var json = JsonConvert.SerializeObject(user);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var httpClient = new HttpClient();
                
                var response = await httpClient.PostAsync("http://192.168.1.8:5000/api/user/register", httpContent);
                var statusCode = response.StatusCode.ToString();
                if(statusCode == "OK")
                {
                    DependencyService.Get<INotification>().CreateNotification("Notification", $" Register successfull ");

                    var httpClientGetUser = new HttpClient();
                    var responseUs = await httpClientGetUser.GetStringAsync($"http://192.168.1.8:5000/api/user/{Email.Text}&{Password.Text}");
                    var User = JsonConvert.DeserializeObject<UserModel>(responseUs);          
                    await Xamarin.Essentials.SecureStorage.SetAsync("storageUser", JsonConvert.SerializeObject(User));
                    config.userModel = new UserModel();
                    config.userModel = User;
                    await Navigation.PushAsync(new TabbedMessaagePage());
                }
                else
                {
                    DependencyService.Get<INotification>().CreateNotification("Notification", $" Number Phone or Gmail Is Already exist ");
                }

                activityIndicator.IsRunning = false;

            }


        }

        private void previous_Clicked(object sender, EventArgs e)
        {
            registerViewModel.NextRegister = true;
            register.Text = "CONTINUE";
        }

        private async void ImgURL_Clicked(object sender, EventArgs e)
        {
            var pickImage = await FilePicker.PickAsync(new PickOptions()
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick an Image"
            });

            if (pickImage != null)
            {
                var stream = await pickImage.OpenReadAsync();
                ImgURL.Source = ImageSource.FromStream(() => stream);
                ImgStream = stream;
                FileNameImage = pickImage.FileName;
            }
        }
    }
}