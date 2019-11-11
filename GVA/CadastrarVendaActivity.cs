using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using GVA.DataLocal;
using GVA.Dominio;
using GVA.Util;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace GVA
{
    [Activity(Label = "Cadastrar Venda", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CadastrarVendaActivity : Activity
    {
        #region Variaveis
        EditText dataVenda;
        EditText dataPagamento;
        EditText dataVencimento;
        EditText descricao;
        EditText valor;
        Spinner spinnerCliente;
        ImageView _imageView;

        public List<long> IDsCliente { get; set; }

        public int IdVenda { get; set; }

        public long ClienteSelecionado { get; set; }

        File _file;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
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

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                _imageView.Click += ImageViewFoto_Click;
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

            if (!String.IsNullOrEmpty(venda.CaminhoImagem))
            {
                _file = new File(venda.CaminhoImagem);

                Bitmap image = BitmapFactory.DecodeFile(venda.CaminhoImagem);
                _imageView.SetImageBitmap(image);

                GC.Collect();
            }
        }

        #region Imagem

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Bitmap image = BitmapFactory.DecodeFile(App._file.Path);
            _imageView.SetImageBitmap(image);

            _file = App._file;

            GC.Collect();
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "GVA");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void ImageViewFoto_Click(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            App._file = new File(App._dir, String.Format("gva_{0}.jpg", DateTime.Now));

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);
        }

        #endregion

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
            _imageView = FindViewById<ImageView>(Resource.Id.imageViewFoto);
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
                Valor = string.IsNullOrEmpty(valor.Text) ? "0.00" : valor.Text,
                CaminhoImagem = _file != null ? _file.Path : string.Empty
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
        private void SpinnerCliente_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ClienteSelecionado = IDsCliente[e.Position];
        }

        #endregion
    }

    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }

}