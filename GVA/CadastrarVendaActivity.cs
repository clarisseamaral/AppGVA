
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using GVA.DataLocal;
using GVA.Util;

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

        private void ListarVendas()
        {

        }


        private void InserirBancoLocal()
        {
            var txtValor = FindViewById<EditText>(Resource.Id.txtValor).Text;

            var tbVenda = new VendaDB()
            {
                IdCliente = 1, //todo: (FindViewById<EditText>(Resource.Id.txtDataNascimento)).Text,
                Descricao = (FindViewById<EditText>(Resource.Id.txtDescricao)).Text,
                DataVenda = (FindViewById<EditText>(Resource.Id.txtDataVenda)).Text,
                DataVencimento = (FindViewById<EditText>(Resource.Id.txtDataVencimento)).Text,
                DataPagamento = (FindViewById<EditText>(Resource.Id.txtDataPagamento)).Text,
                Valor = string.IsNullOrEmpty(txtValor) ? 0 : double.Parse(txtValor)
            };

            var stringBuilder = new System.Text.StringBuilder();

            stringBuilder.AppendFormat("{0};", tbVenda.InsertQuery);

            UtilDataBase.Save(stringBuilder.ToString());
        }
    }
}