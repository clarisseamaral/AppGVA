
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace GVA
{
    [Activity(Label = "Clientes", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ListarClienteActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.listar_clientes);
        }
    }
}