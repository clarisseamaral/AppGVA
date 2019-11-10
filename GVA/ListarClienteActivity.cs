
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
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

        #region Menu
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            MenuInflater inflater = this.MenuInflater;
            inflater.Inflate(Resource.Menu.menu1, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.MenuAdicionarVenda:
                    Finish();
                    var intent = new Intent(this, typeof(CadastrarVendaActivity));
                    StartActivity(intent);
                    break;
                case Resource.Id.MenuClientes:
                    return true;
                case Resource.Id.MenuAdicionarCliente:
                    var intentCliente = new Intent(this, typeof(CadastrarClienteActivity));
                    StartActivity(intentCliente);
                    return true;
                case Resource.Id.MenuVendas:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion
    }
}