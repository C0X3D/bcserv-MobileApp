using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
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
        Button backButt;
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
            SetContentView(Resource.Layout.activity_main);
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
                if(textUsername.Text.Length < 4)
                {
                    loginResponse.Text = "Username must be longer than 4 chars.";
                    return;
                }
                if(textPassword.Text.Length < 6)
                {
                    loginResponse.Text = "Pasword must be longer than 6 chars.";
                    return;
                }
                if(!textEmail.Text.Contains("@"))
                {
                    loginResponse.Text = "Email has no valid domains.";
                    return;
                }
                loginResponse.Text =  await Tests.ApiFunctions.CreateUser(textUsername.Text, textPassword.Text, textEmail.Text, false);
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
                    LoadActivityMain();
                }
                loginResponse.Text = resp;
            }
            catch(Exception xe)
            {
                //MessageBox.Show()
                loginResponse.Text = xe.Message;
            }

        }

        private void LoadActivityMain()
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

        private void Capture_Click(object sender, EventArgs e)
        {
            textResponse.Text = "Proccesing please wait...";
            TakePhoto();
            ReadText.Visibility = ViewStates.Visible;
            ReadText.Click += ReadText_Click;
        }

        private void ReadText_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.content_main);
            FindViewById<TextView>(Resource.Id.readedText).Text = "The return of readed text...";
            FindViewById<Button>(Resource.Id.cmainBackBut).Click += BackButtClick;
            FindViewById<Button>(Resource.Id.saveLocal).Click += SaveResponse;
        }

        private void SaveResponse(object sender, EventArgs e)
        {
            CrossClipboard.Current.SetText(FindViewById<TextView>(Resource.Id.readedText).Text);
        }

        private void BackButtClick(object sender, EventArgs e)
        {
            LoadActivityMain();
        }

        async void  TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {PhotoSize= Plugin.Media.Abstractions.PhotoSize.Medium,CompressionQuality=40,Directory = "testing",Name="myimage.jpg"});

            if(file == null)
            {
                return;
            }

            byte[] img_arr = System.IO.File.ReadAllBytes(file.Path);
            Bitmap BTM = BitmapFactory.DecodeByteArray(img_arr, 0, img_arr.Length);
            this.CameraView.SetImageBitmap(BTM);
            SendImageToServer(file.Path);


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }
}

