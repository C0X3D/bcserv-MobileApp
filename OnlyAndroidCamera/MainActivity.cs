using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using bizCommon;
using Newtonsoft.Json;
using Plugin.Clipboard;
using Plugin.Media;
using Tests;

//using Tesseract;
//using System.Drawing;
namespace OnlyAndroidCamera
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //public static MainActivity mainActivity => this;


        Button capture;
        Button LogIn;
        Button SignUp;
        ImageView CameraView;
        TextView textUsername;
        TextView textPassword;
        TextView textEmail;
        TextView textResponse;
        TextView loginResponse;
        Button ReadText;
        // Button backButt;
        LinearLayout cardsLayout;
        readonly string[] permisionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
            Manifest.Permission.AccessNetworkState,
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.Internet


        };
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainMenu);
            RequestPermissions(permisionGroup, 0);
            Tests.Statics.WebLink = @"http://192.168.43.204/biz";
            var txt = "Server " + Tests.Statics.WebLink;

            if (string.IsNullOrEmpty(Statics.uname))
            {
                SetContentView(Resource.Layout.LandingPage);
                textUsername = FindViewById<EditText>(Resource.Id.textUsername);
                LogIn = FindViewById<Button>(Resource.Id.loginbut);
                SignUp = FindViewById<Button>(Resource.Id.signupbut);
                textPassword = FindViewById<EditText>(Resource.Id.textPassword);
                loginResponse = FindViewById<TextView>(Resource.Id.textView2);
                loginResponse.Text = txt;
                textEmail = FindViewById<EditText>(Resource.Id.textEmail);
                textUsername.Click += TextUsername_Click;
                textPassword.Click += TextPassword_Click;
                SignUp.Click += SignUp_Click;
                LogIn.Click += LogIn_ClickAsync;
            }
        }
        public async Task ViewCardsAsync()
        {
            //FindViewById<Button>(Resource.Id.newCard);

            SetContentView(Resource.Layout.MainMenu);
            cardsLayout = FindViewById<LinearLayout>(Resource.Id.cardList);

            FindViewById<Button>(Resource.Id.newCard).Click += MainActivity_Click;

            await GetBizCards();

            //List<bizCard> bizCards = JsonConvert.DeserializeObject<List<bizCard>>(p);

            var VisualCreator = new VisualCreatorClass();
            foreach (var bc in CardList)
            {

                var child = VisualCreator.CreateCardView(this, bc,this);

                cardsLayout.AddView(child);
                
            }




        }
        public void _startActivity(Intent intent)
        {
            StartActivity(intent);
        }
        private void MainActivity_Click(object sender, EventArgs e)
        {
           UploadForm();
        }

        List<bizCard> CardList = new List<bizCard>();
        public async Task GetBizCards()

        {
            // var request = (HttpWebRequest)WebRequest.Create(Statics.WebLink + "/api/users");

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", "brt94")
            };
            //YOU can set a request authentication string etc here...
            //pairs.Add(new KeyValuePair<string, string>("password", pw));
            //pairs.Add(new KeyValuePair<string, string>("email", email));

            FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);
            var responseString = "";

            using (var client = new HttpClient())
            {
                //client.PostAsync
                HttpResponseMessage response = client.PostAsync(Tests.Statics.WebLink + "/api/GetCards", content).GetAwaiter().GetResult();  // Blocking call!  
                if (response.IsSuccessStatusCode)
                {

                    await response.Content.ReadAsStringAsync().ContinueWith((task) =>
                    {
                        responseString = task.Result;
                        var deserialized = JsonConvert.DeserializeObject<string>(responseString);
                        //var deserialized2 = JsonConvert.DeserializeObject<string>(deserialized);
                        //TODO Deserialize to bizCard list


                        CardList = JsonConvert.DeserializeObject<List<bizCard>>(deserialized);

                    });
                }
            }
            //return null;
        }

        private async void SignUp_Click(object sender, EventArgs e)
        {

            if (textEmail.Visibility == ViewStates.Invisible)
            {

                textEmail.Visibility = ViewStates.Visible;
                LogIn.Visibility = ViewStates.Invisible;
                return;
            }
            else
            {
                if (textUsername.Text.Length < 4)
                {
                    loginResponse.Text = "Username must be longer than 4 chars.";
                    return;
                }
                if (textPassword.Text.Length < 6)
                {
                    loginResponse.Text = "Pasword must be longer than 6 chars.";
                    return;
                }
                if (!textEmail.Text.Contains("@"))
                {
                    loginResponse.Text = "Email has no valid domains.";
                    return;
                }
                loginResponse.Text = await Tests.ApiFunctions.CreateUser(textUsername.Text, textPassword.Text, textEmail.Text, false);
                LogIn.Visibility = ViewStates.Visible;
            }
        }

        private async void LogIn_ClickAsync(object sender, EventArgs e)
        {
            try
            {

                string resp = "";

                resp = await ApiFunctions.Login(textUsername.Text, textPassword.Text);

                if (resp == "Login succesfull.")
                {
                    await LoadActivityMainAsync();
                }
                loginResponse.Text = resp;
            }
            catch (Exception xe)
            {
                //MessageBox.Show()
                loginResponse.Text = xe.Message;
            }

        }

        private async Task LoadActivityMainAsync()
        {
            SetContentView(Resource.Layout.MainMenu);
            await ViewCardsAsync();
        }
        public void UploadForm()
        {
            SetContentView(Resource.Layout.activity_main);
            capture = FindViewById<Button>(Resource.Id.butonas);
            CameraView = FindViewById<ImageView>(Resource.Id.cameraFeed);
            // Activity.
            textResponse = FindViewById<TextView>(Resource.Id.ocrResponse);
            ReadText = FindViewById<Button>(Resource.Id.readFile);

            capture.Click += Capture_Click;
        }
        private void TextPassword_Click(object sender, EventArgs e)
        {
            // if (textPassword.Text == "Username")
            //  (sender as EditText).Text = "";
        }

        private void TextUsername_Click(object sender, EventArgs e)
        {
            /// if(textUsername.Text =="Username")
            // (sender as EditText).Text = "";
        }

        /*
         * capture = FindViewById<Button>(2131230760);
            CameraView = FindViewById<ImageView>(2131230762);
            capture.Click += Capture_Click;
         * 
         */
        public async void SendImageToServer(string img)
        {

            textResponse.Text = await CommunicationEngine.InsertCard(img);


        }

        private async void Capture_Click(object sender, EventArgs e)
        {
            textResponse.Text = "Proccesing please wait...";
            string v1 = await TakePhoto();
            ReadText.Visibility = ViewStates.Visible;
            ReadText.Click += async (x, y) =>
            {
                SetContentView(Resource.Layout.content_main);
                FindViewById<TextView>(Resource.Id.readedText).Text = await Tests.ApiFunctions.RequestFile(v1 + ".xml");
                FindViewById<Button>(Resource.Id.cmainBackBut).Click += BackButtClickAsync;
                FindViewById<Button>(Resource.Id.saveLocal).Click += SaveResponse;
            };
        }


        private void SaveResponse(object sender, EventArgs e)
        {
            CrossClipboard.Current.SetText(FindViewById<TextView>(Resource.Id.readedText).Text);
        }

        private async void BackButtClickAsync(object sender, EventArgs e)
        {
            await LoadActivityMainAsync();
        }

        async Task<string> TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium, CompressionQuality = 40, Directory = "testing", Name = "myimage.jpg" });

            if (file == null)
            {
                return "ERROR";
            }

            byte[] img_arr = System.IO.File.ReadAllBytes(file.Path);
            Bitmap BTM = BitmapFactory.DecodeByteArray(img_arr, 0, img_arr.Length);
            this.CameraView.SetImageBitmap(BTM);
            SendImageToServer(file.Path);

            return System.IO.Path.GetFileName(file.Path);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }
}

