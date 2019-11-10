using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using GVA.DataLocal;
using GVA.Dominio;
using GVA.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GVA
{
    [Activity(Label = "Cadastrar Venda", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CadastrarVendaActivity : Activity
    {
        EditText dataVenda;
        EditText dataPagamento;
        EditText dataVencimento;
        EditText descricao;
        EditText valor;
        Spinner spinnerCliente;

        public List<long> IDsCliente { get; set; }

        public int IdVenda { get; set; }

        public long ClienteSelecionado { get; set; }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.cadastrar_venda);

            CarregarElementos();

            CarregarClientes();

            if (!String.IsNullOrEmpty(Intent.GetStringExtra("FluxoEdicaoVenda")))
            {
                IdVenda = int.Parse(Intent.GetStringExtra("FluxoEdicaoVenda"));
                var dtVenda = UtilDataBase.GetItems(VendaDB.TableName, string.Format(" Id = {0}", IdVenda), null);

                ExibirVenda(new VendaDB(dtVenda.Rows[0]));
            }
        }

        private void ExibirVenda(VendaDB venda)
        {
            FindViewById<LinearLayout>(Resource.Id.linearApagar).Visibility = Android.Views.ViewStates.Visible;

            FindViewById<Button>(Resource.Id.btnApagarVenda).Click += Apagar_Click;

            descricao.Text = venda.Descricao;
            spinnerCliente.SetSelection(IDsCliente.IndexOf(venda.IdCliente));
            valor.Text = venda.Valor.ToString();
            dataVenda.Text = venda.DataVenda;
            dataVencimento.Text = venda.DataVencimento;
            dataPagamento.Text = venda.DataPagamento;
            //TODO: carregar imagem
        }


        private void CarregarElementos()
        {

            FindViewById<Button>(Resource.Id.btnSalvarVenda).Click += Salvar_Click;
            FindViewById<Button>(Resource.Id.btnCancelarVenda).Click += Cancelar_Click;

            descricao = FindViewById<EditText>(Resource.Id.txtDescricao);
            dataVenda = FindViewById<EditText>(Resource.Id.txtDataVenda);
            dataVencimento = FindViewById<EditText>(Resource.Id.txtDataVencimento);
            dataPagamento = FindViewById<EditText>(Resource.Id.txtDataPagamento);
            valor = FindViewById<EditText>(Resource.Id.txtValor);
            spinnerCliente = FindViewById<Spinner>(Resource.Id.spinnerCliente);

            spinnerCliente.ItemSelected += SpinnerCliente_ItemSelected;
        }

        private void SpinnerCliente_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ClienteSelecionado = IDsCliente[e.Position];
        }

        private void CarregarClientes()
        {
            var dtClientes = UtilDataBase.GetItemsQuery("SELECT IdCliente, Nome from Cliente order by Nome");

            if (dtClientes.Rows.Count > 0)
            {
                var itens = IConversoes.ConvertDataTable<ListagemClienteDTO>(dtClientes);

                IDsCliente = new List<long>() { 0 };

                IDsCliente.AddRange(itens.Select(o => o.IdCliente).ToList());

                var nomes = new List<string>() { "Selecionar cliente" };

                nomes.AddRange(itens.Select(o => o.Nome).ToList());

                var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, nomes);

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinnerCliente.Adapter = adapter;
            }
            else
            {
                ExibirAlertaNenhumClienteCadastrado();
            }

          
        }

        private void ExibirAlertaNenhumClienteCadastrado()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();
            alerta.SetTitle("Nenhum cliente cadastrado! Você será redirecionado para o cadastro.");
            alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
            alerta.SetButton("OK", (s, ev) =>
            {
                Finish();
                var intentCliente = new Intent(this, typeof(CadastrarClienteActivity));
                StartActivity(intentCliente);
            });
            alerta.Show();
        }

        private string GerarData(string adata)
        {
            return String.IsNullOrWhiteSpace(adata.Replace("_", "").Replace("/", "")) ? string.Empty : adata;
        }

        private bool ValidaDatasPreenchidas()
        {
            string mensagem = string.Empty;

            DateTime ldataVenda, ldataPagamento, ldataVencimento;
            bool datasValidas = true, datasPreenchidasCorretamente = true;

            DateTime.TryParseExact(GerarData(dataVenda.Text), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out ldataVenda);
            DateTime.TryParseExact(GerarData(dataPagamento.Text), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out ldataPagamento);
            DateTime.TryParseExact(GerarData(dataVencimento.Text), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out ldataVencimento);

            if (ldataVenda == DateTime.MinValue || ldataVencimento == DateTime.MinValue)
            {
                datasPreenchidasCorretamente = false;
            }

            if (!String.IsNullOrWhiteSpace(GerarData(dataPagamento.Text)) && ldataPagamento == DateTime.MinValue)
            {
                datasPreenchidasCorretamente = false;
            }

            if (datasPreenchidasCorretamente)
            {
                if (ldataVenda > DateTime.Now)
                {
                    datasValidas = false;
                    mensagem += Resources.GetString(Resource.String.DataVendaMaiorPermitido);
                }
                else if (ldataVenda > ldataVencimento)
                {
                    datasValidas = false;
                    mensagem += Resources.GetString(Resource.String.DataVencimentoInvalida);
                }
                else if (!String.IsNullOrWhiteSpace(GerarData(dataPagamento.Text)) && ldataVenda > ldataPagamento)
                {
                    datasValidas = false;
                    mensagem += Resources.GetString(Resource.String.DataPagamentoInvalida);
                }
            }
            else
            {
                mensagem += Resources.GetString(Resource.String.DataInvalida);
            }

            if (!String.IsNullOrEmpty(mensagem))
            {
                Toast.MakeText(this, mensagem, ToastLength.Long).Show();
            }

            return datasValidas && datasPreenchidasCorretamente;
        }


        private bool CamposObrigatoriosPreenchidos()
        {
            return !string.IsNullOrEmpty(descricao.Text)
                    && !string.IsNullOrEmpty(dataVenda.Text)
                    && !string.IsNullOrEmpty(dataVencimento.Text)
                    && !string.IsNullOrEmpty(valor.Text)
                    && ClienteSelecionado > 0;
        }


        private void AtualizarBancoLocal()
        {
            var tbVenda = new VendaDB()
            {
                IdCliente = (int)ClienteSelecionado,
                Descricao = descricao.Text.Trim(),
                DataVenda = dataVenda.Text,
                DataVencimento = dataVencimento.Text,
                DataPagamento = dataPagamento.Text,
                Valor = string.IsNullOrEmpty(valor.Text) ? "0.00" : valor.Text
            };

            var stringBuilder = new System.Text.StringBuilder();

            if (IdVenda > 0)
            {
                tbVenda.Id = IdVenda;
                stringBuilder.AppendFormat("{0};", tbVenda.UpdateQuery);
            }
            else
            {
                stringBuilder.AppendFormat("{0};", tbVenda.InsertQuery);
            }

            UtilDataBase.Save(stringBuilder.ToString());
        }

        #region Eventos
        private void Cancelar_Click(object sender, System.EventArgs e)
        {
            Finish();
        }

        private void Salvar_Click(object sender, System.EventArgs e)
        {
            if (CamposObrigatoriosPreenchidos())
            {
                if (ValidaDatasPreenchidas())
                {
                    AtualizarBancoLocal();

                    Toast.MakeText(this, "Dados salvos com sucesso.", ToastLength.Long).Show();

                    Finish();
                }
            }
            else
            {
                Toast.MakeText(this, "Favor preencher os campos obrigatórios.", ToastLength.Long).Show();
            }
        }

        private void Apagar_Click(object sender, System.EventArgs e)
        {
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);
            alerta.SetTitle("Tem certeza que deseja apagar?");
            alerta.SetPositiveButton("Sim", (senderAlert, args) =>
            {
                UtilDataBase.Delete(VendaDB.TableName, string.Format(" Id = {0}", IdVenda));
                Toast.MakeText(this, "Venda apagada com sucesso!", ToastLength.Long).Show();
                Finish();
            });
            alerta.SetNegativeButton("Não", (senderAlert, args) =>
            {

            });
            Dialog dialog = alerta.Create();
            dialog.Show();
        }
        #endregion


    }
}