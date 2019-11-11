using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GVA.Dominio;
using Java.IO;
using System;
using System.Collections.Generic;

namespace GVA.Adapter
{
    class VendaAdapter : BaseAdapter<ListagemVendaDTO>
    {
        private IList<ListagemVendaDTO> itens;
        Context context;

        public VendaAdapter(Context context, IList<ListagemVendaDTO> listaVendas)
        {
            this.context = context;
            this.itens = listaVendas;
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
                view = inflater.Inflate(Resource.Layout.venda_item, parent, false);
                holder.Descricao = view.FindViewById<TextView>(Resource.Id.txtDescricaoVenda);
                holder.Detalhes = view.FindViewById<TextView>(Resource.Id.txtDetalhes);
                holder.Valor = view.FindViewById<TextView>(Resource.Id.txtValorLista);
                holder.Imagem = view.FindViewById<ImageView>(Resource.Id.imgVenda);
                view.Tag = holder;
            }

            holder.Descricao.Text = itens[position].Descricao;
            holder.Detalhes.Text = itens[position].Nome;
            holder.Valor.Text = itens[position].Valor;

            var ldataVencimento = itens[position].DataVencimento;
            var lDataPagamento = itens[position].DataPagamento;
            var lStatus = VerificarStatus(
                String.IsNullOrEmpty(lDataPagamento) ? (DateTime?)null : DateTime.Parse(lDataPagamento),
                DateTime.Parse(ldataVencimento));

            if (!String.IsNullOrEmpty(itens[position].CaminhoImagem))
            {
                File imgFile = new File(itens[position].CaminhoImagem);

                if (imgFile.Exists())
                {
                    Bitmap myBitmap = BitmapFactory.DecodeFile(imgFile.AbsolutePath);
                    holder.Imagem.SetImageBitmap(myBitmap);
                    GC.Collect();
                }
            }

            switch (lStatus)
            {
                case (int)Status.Vencido:
                    holder.Detalhes.Text += " - Venc: " + itens[position].DataVencimento;
                    holder.Valor.SetTextColor(Color.Red);
                    break;
                case (int)Status.Pago:
                    holder.Valor.SetTextColor(Color.Green);
                    break;
                case (int)Status.Pendente:
                    holder.Detalhes.Text += " - Venc: " + itens[position].DataVencimento;
                    holder.Valor.SetTextColor(Color.Orange);
                    break;
            }

            return view;
        }


        private int VerificarStatus(DateTime? dataPagamento, DateTime dataVencimento)
        {
            if (dataVencimento < DateTime.Now && !dataPagamento.HasValue)
            {
                return (int)Status.Vencido;
            }
            else if (dataPagamento.HasValue)
            {
                return (int)Status.Pago;
            }

            return (int)Status.Pendente;
        }

        public override int Count {
            get {
                return itens.Count;
            }
        }

    }

    enum Status
    {
        Pendente = 1,
        Pago = 2,
        Vencido = 3
    }

    class VendaAdapterViewHolder : Java.Lang.Object
    {
        public TextView Descricao { get; set; }

        public TextView Detalhes { get; set; }

        public TextView Valor { get; set; }

        public ImageView Imagem { get; set; }

    }
}