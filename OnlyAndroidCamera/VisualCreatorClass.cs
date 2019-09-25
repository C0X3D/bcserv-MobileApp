using Android.Content;
using Android.Media;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.Messaging;
using System;
using static Android.Support.Design.Widget.AppBarLayout;

namespace OnlyAndroidCamera
{
    internal class VisualCreatorClass
    {
        public VisualCreatorClass()
        {
        }

        internal LinearLayout CreateCardView(Android.Content.Context context,bizCommon.bizCard bizCard,MainActivity OwnerThread)//,Image img)
        {
            var butLay = new FrameLayout.LayoutParams(150, 150, GravityFlags.Right) { TopMargin = 5,LeftMargin=20 };
            var fill = new FrameLayout.LayoutParams(Android.Support.Design.Widget.AppBarLayout.LayoutParams.MatchParent, 300) { TopMargin = 25 };
            //image
            ImageView imageView = new ImageView(context)
            {
                LayoutParameters = new FrameLayout.LayoutParams(300, 200)
            };
            imageView.SetImageResource(Resource.Mipmap.Logo);
            //call but
            ImageButton callButton = new ImageButton(context);
            callButton.SetScaleType(ImageView.ScaleType.FitCenter);
            //callButton.BackgroundImageLayout = ImageLayout.Stretch
                callButton.LayoutParameters = butLay;
            callButton.SetImageResource(Resource.Mipmap.phone);
            callButton.Click += (x,y) => {
                var phoneDialer = CrossMessaging.Current.PhoneDialer;
                if (phoneDialer.CanMakePhoneCall)
                    phoneDialer.MakePhoneCall(bizCard.phone1);
            };
            //mail but
            ImageButton mailButton = new ImageButton(context);
            mailButton.LayoutParameters = butLay;
            mailButton.SetImageResource(Resource.Mipmap.mail);
            mailButton.SetScaleType(ImageView.ScaleType.FitCenter);
            mailButton.Click += (x,y) =>
            {
                var uri = Android.Net.Uri.Parse("mailto:"  + bizCard.email);
                var intent = new Intent(Intent.ActionView, uri);
                OwnerThread.StartActivity(intent);
            };
            //edit but
            ImageButton editButton = new ImageButton(context);
            editButton.LayoutParameters = butLay;
            editButton.SetImageResource(Resource.Mipmap.edit);
            editButton.SetScaleType(ImageView.ScaleType.FitCenter);
            ImageButton deleteButton = new ImageButton(context);
            deleteButton.LayoutParameters = butLay;
            deleteButton.SetImageResource(Resource.Mipmap.trash);
            deleteButton.SetScaleType(ImageView.ScaleType.FitCenter);
            deleteButton.Click += (x, y) =>
            {
                Tests.ApiFunctions.DeleteCard(bizCard.id_owner, bizCard.Id);
            };
            LinearLayout ml = new LinearLayout(context);
            ml.Orientation = Android.Widget.Orientation.Vertical;
            ml.LayoutParameters = fill;
            GridLayout ll = new GridLayout(context);
            //ml.Background = CSSType.gd;
           // ll.Orientation = Android.Widget.Orientation.Vertical;
            ml.Background = new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.BlueViolet);
            ll.LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.MatchParent, 200);
            TextView tv = new TextView(context);
            tv.LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.MatchParent, 100);
            tv.TextAlignment = TextAlignment.Center;
            tv.SetTextSize(ComplexUnitType.Dip, 24);
            tv.Text = bizCard.Nume + " " + bizCard.Prenume;
            ml.AddView(tv);
            //imageView.AddChildrenForAccessibility(tv);
            ll.AddView(imageView);
            ll.AddView(callButton);
            ll.AddView(mailButton);
            ll.AddView(editButton);
            ll.AddView(deleteButton);

            ml.AddView(ll);
            //ll.AddView(tv, 105, 105);

            return ml;
        }
    }
}