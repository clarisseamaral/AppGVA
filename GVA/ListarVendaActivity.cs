using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using GVA.Adapter;
using GVA.Dominio;
using GVA.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GVA
{
    [Activity(Label ="Vendas", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class ListarVendaActivity : Activity
    {
        ListView listaVendas;
        IList<ListagemVendaDTO> itens;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            listaVendas = FindViewById<ListView>(Resource.Id.listViewVendas);
            listaVendas.ItemClick += ListaVendas_ItemClick;

            CarregarLista();
        }

        protected override void OnResume()
        {
            base.OnResume();
            CarregarLista();
        }

        private void CarregarLista()
        {
            var dtVendas = UtilDataBase.GetItemsQuery("SELECT v.Id, v.IdCliente, c.Nome, v.Descricao, v.Valor, v.DataVenda, v.DataVencimento, v.DataPagamento, v.CaminhoImagem  from venda v INNER JOIN Cliente c ON v.IdCliente = c.IdCliente");

            if (dtVendas.Rows.Count > 0)
            {
                itens = IConversoes.ConvertDataTable<ListagemVendaDTO>(dtVendas);

                //itens = itens.OrderBy(x => x.DataVenda).ToList();

                listaVendas.Adapter = new VendaAdapter(this, itens);
            }
            else
            {
                listaVendas.Adapter = new VendaAdapter(this, new List<ListagemVendaDTO>());
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
                case Resource.Id.MenuVendas:
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion

        private void ListaVendas_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;

            var activity = new Intent(this, typeof(CadastrarVendaActivity));
            activity.PutExtra("FluxoEdicaoVenda", itens[e.Position].Id.ToString());
            StartActivity(activity);
        }

    }
}