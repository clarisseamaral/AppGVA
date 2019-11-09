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
    [Activity(Label = "Vendas", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class MainActivity : Activity
    {
        ListView listaVendas;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            listaVendas = FindViewById<ListView>(Resource.Id.listViewVendas);

            var dtVendas = UtilDataBase.GetItems(VendaDB.TableName);

            if (dtVendas.Rows.Count > 0)
            {
                IList<ListagemVendaDTO> itens = IConversoes.ConvertDataTable<ListagemVendaDTO>(dtVendas);

                listaVendas.Adapter = new VendaAdapter(this, itens);
                listaVendas.ItemClick += ListaVendas_ItemClick;
            }
            else
            {
                SetContentView(Resource.Layout.activity_main);
            }
        }

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
                    var intent = new Intent(this, typeof(CadastrarVendaActivity));
                    StartActivity(intent);
                    break;
                case Resource.Id.MenuClientes:
                    var intentLista = new Intent(this, typeof(ListarClienteActivity));
                    StartActivity(intentLista);
                    return true;
                case Resource.Id.MenuAdicionarCliente:
                    var intentCliente = new Intent(this, typeof(CadastrarClienteActivity));
                    StartActivity(intentCliente);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void ListaVendas_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;
        }

    }
}