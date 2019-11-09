using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GVA.Dominio;
using System.Collections.Generic;

namespace GVA.Adapter
{
    class ClienteAdapter : BaseAdapter<ListagemClienteDTO>
    {
        private IList<ListagemClienteDTO> itens;
        Context context;

        public ClienteAdapter(Context context, IList<ListagemClienteDTO> listaClientes)
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

        public override ListagemClienteDTO this[int position] {
            get {
                return itens[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            ClienteAdapterViewHolder holder = null;

            if (view != null)
            {
                holder = view.Tag as ClienteAdapterViewHolder;
            }

            if (holder == null)
            {
                holder = new ClienteAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.cliente_item, parent, false);
                holder.Nome = view.FindViewById<TextView>(Resource.Id.txtClienteNome);
                holder.DataNascimento = view.FindViewById<TextView>(Resource.Id.txtClienteDataNascimento);
                view.Tag = holder;
            }

            holder.Nome.Text = itens[position].Nome;
            holder.DataNascimento.Text = itens[position].DataNascimento;

            return view;
        }

        public override int Count {
            get {
                return itens.Count;
            }
        }

    }

    class ClienteAdapterViewHolder : Java.Lang.Object
    {
        public TextView Nome { get; set; }

        public TextView DataNascimento { get; set; }

    }
}