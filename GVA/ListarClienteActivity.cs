
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using GVA.Adapter;
using GVA.DataLocal;
using GVA.Dominio;
using GVA.Util;
using System.Collections.Generic;

namespace GVA
{
    [Activity(Label = "Clientes", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ListarClienteActivity : Activity
    {
        ListView listaClientes;
        IList<ListagemClienteDTO> itens;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.listar_clientes);

            listaClientes = FindViewById<ListView>(Resource.Id.listViewClientes);
            listaClientes.ItemClick += ListaClientes_ItemClick; ;

            CarregarLista();
        }

        private void ListaClientes_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var activity = new Intent(this, typeof(CadastrarClienteActivity));
            activity.PutExtra("FluxoEdicaoCliente", itens[e.Position].IdCliente.ToString());
            StartActivity(activity);
        }

        protected override void OnResume()
        {
            base.OnResume();
            CarregarLista();
        }

        private void CarregarLista()
        {
            var dtClientes = UtilDataBase.GetItems(ClienteDB.TableName, null, "order by Nome");

            if (dtClientes.Rows.Count > 0)
            {
                itens = IConversoes.ConvertDataTable<ListagemClienteDTO>(dtClientes);

                listaClientes.Adapter = new ClienteAdapter(this, itens);
            }
            else
            {
                listaClientes.Adapter = new ClienteAdapter(this, new List<ListagemClienteDTO>());
            }
        }
    }
}