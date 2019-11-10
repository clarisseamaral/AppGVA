
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using System.Threading;
using System.Threading.Tasks;

namespace GVA
{
    [Activity(Label = "GVA", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true, WindowSoftInputMode = SoftInput.StateHidden, Theme = "@style/NoActionBar")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.splash);

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                RunOnUiThread(() =>
                {
                    StartActivity(typeof(ListarVendaActivity));
                });
            });
        }
    }
}