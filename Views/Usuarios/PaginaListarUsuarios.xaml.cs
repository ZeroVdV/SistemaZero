using SistemaZero.Model.Entity;
using HandyControl.Controls;
using SistemaZero.Controller;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.Usuarios
{
    public partial class PaginaListarUsuarios : UserControl
    {
        private List<User> usuarios = new();
        private readonly UserController controller;
        private int ultimoId = 0;
        private int qtdRetorno => ObterQtdRetorno();
        private string termoAtual = "";

        public PaginaListarUsuarios()
        {
            InitializeComponent();

            controller = new UserController();
            if (!controller.VerificarPermissao())
            {
                Growl.Clear();
                Growl.Error("Você não tem permissão para acessar esta página.");
                if (Application.Current.MainWindow is MainWindow main)
                    main.Dispatcher.InvokeAsync(main.TelaInicial);
                return;
            }

            dgUsuarios.RowHeight = double.NaN;
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                AtualizarDataGrid(ExecutarPesquisa(ultimoId), limparLista: true);
        }

        private void AtualizarDataGrid(List<User>? novos, bool limparLista = false)
        {
            if (novos == null || novos.Count == 0)
            {
                Growl.Error("Nenhum usuário encontrado.", "MessageTk");
                btnContinuar.IsEnabled = false;
                return;
            }

            var exibidos = qtdRetorno > 0 ? novos.Take(qtdRetorno).ToList() : novos;

            if (limparLista)
                usuarios = new List<User>();

            usuarios ??= new List<User>();
            usuarios.AddRange(exibidos);

            dgUsuarios.ItemsSource = null;
            dgUsuarios.ItemsSource = usuarios;

            // Proteção contra lista vazia no Max
            ultimoId = usuarios.Any() ? usuarios.Max(u => u.ID ?? 0) : 0;

            btnContinuar.IsEnabled = novos.Count > qtdRetorno && qtdRetorno != -1;
        }

        private List<User>? ExecutarPesquisa(int idAtual)
        {
            if (string.IsNullOrWhiteSpace(termoAtual))
                return controller.ObterVariosUsuarios(idAtual, qtdRetorno + 1);
            else
                return controller.PesquisarUsuarios(idAtual, qtdRetorno + 1, termoAtual);
        }

        private int ObterQtdRetorno()
        {
            string? valor = (cmbQtdSaidas.SelectedItem as ComboBoxItem)?.Content?.ToString()?.ToLower();
            return valor switch
            {
                "1" => 1,
                "5" => 5,
                "10" => 10,
                "20" => 20,
                "50" => 50,
                "todos" => -1,
                _ => 20
            };
        }

        private void Att_Click(object sender, RoutedEventArgs e)
        {
            ultimoId = 0;
            var atualizados = ExecutarPesquisa(ultimoId);
            AtualizarDataGrid(atualizados, limparLista: true);
            Growl.Clear("MessageTk");
            Growl.Info("Usuarios Atualizados!", "MessageTk");
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            var novos = ExecutarPesquisa(ultimoId);
            AtualizarDataGrid(novos);
        }

        private void BtnAddUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow main)
                main.NavegarPara(new PaginaUsuarios(), "Adicionar Usuário");
        }

        private void BtnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is User usuario)
            {
                var pagina = new PaginaUsuarios(usuario);
                if (Application.Current.MainWindow is MainWindow main)
                    main.NavegarPara(pagina, "Editar Usuário");
            }
        }

        private void pesquisa_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            ultimoId = 0;
            termoAtual = e.Info?.Trim() ?? "";
            var resultado = ExecutarPesquisa(ultimoId);
            AtualizarDataGrid(resultado, limparLista: true);
        }
    }
}
