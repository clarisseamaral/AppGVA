
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace GVA
{
    [Activity(Label = "Cadastrar Venda", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CadastrarVendaActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.cadastrar_venda);
        }
    }
}