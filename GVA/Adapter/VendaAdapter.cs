using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GVA.Dominio;
using System.Collections.Generic;

namespace GVA.Adapter
{
    class VendaAdapter : BaseAdapter<ListagemVendaDTO>
    {
        private IList<ListagemVendaDTO> itens;
        Context context;

        public VendaAdapter(Context context, IList<ListagemVendaDTO> listaClientes)
        {
            this.context = context;
            this.itens = listaClientes;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ListagemVendaDTO this[int position] {
            get {
                return itens[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            VendaAdapterViewHolder holder = null;

            if (view != null)
            {
                holder = view.Tag as VendaAdapterViewHolder;
            }

            if (holder == null)
            {
                holder = new VendaAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.cliente_item, parent, false);
                holder.Descricao = view.FindViewById<TextView>(Resource.Id.txtDescricaoVenda);
                holder.Detalhes = view.FindViewById<TextView>(Resource.Id.txtDetalhes);
                view.Tag = holder;
            }

            holder.Descricao.Text = itens[position].Descricao;
            holder.Detalhes.Text = itens[position].NomeCliente + "  " + itens[position].Valor;

            //TODO: imgStatus
            

            return view;
        }

        public override int Count {
            get {
                return itens.Count;
            }
        }

    }

    class VendaAdapterViewHolder : Java.Lang.Object
    {
        public TextView Descricao { get; set; }

        public TextView Detalhes { get; set; }

    }
}