using SistemaZero.Helpers;
using SistemaZero.Model.Entity;
using SistemaZero.Views.ItemsReutilizaveis;
using HandyControl.Controls;
using SistemaZero.Controller;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero.Views.Usuarios
{
    public partial class PaginaUsuarios : UserControl
    {
        private Texto campoNome;
        private Email campoEmail;
        private Senha campoSenha;
        private Senha campoCSenha;
        private User user;
        private readonly UserController controller = new UserController();
        private ObservableCollection<string> cargos;
        private bool permissao;

        public PaginaUsuarios() : this(new User()) { }

        public PaginaUsuarios(User user)
        {
            InitializeComponent();

            this.user = user;
            permissao = controller.VerificarPermissao();

            if (!permissao)
            {
                Growl.Clear();
                Growl.Error("Você não tem permissão para acessar esta página.");

                if (Application.Current.MainWindow is MainWindow main)
                {
                    main.Dispatcher.InvokeAsync(() =>
                    {
                        main.TelaInicial();
                    });
                }

                return;
            }

            InicializarCampos();
            PreencherCampos();

            if (user.ID != null)
                ConfigurarModoEdicao();
        }

        private void InicializarCampos()
        {
            cargos = new ObservableCollection<string> { "Administrador", "Estoquista", "Vendedor" };
            gdCargo.ItemsSource = cargos;

            campoNome = new Texto("Nome Completo", "", user?.Nome, true, false);
            campoEmail = new Email("Email", "exemplo@empresa.com", user?.Email);
            campoSenha = new Senha("Senha", "*******");
            campoCSenha = new Senha("Confirmar Senha", "*******");

            gdNome.Content = campoNome;
            gdEmail.Content = campoEmail;
            gdSenha.Content = campoSenha;
            gdCSenha.Content = campoCSenha;
        }

        private void PreencherCampos()
        {
            gdCargo.SelectedIndex = user.ID != null ? (int)user.Cargo : -1;

            if (user.ID != null)
            {
                campoSenha.Visibility = Visibility.Collapsed;
                campoCSenha.Visibility = Visibility.Collapsed;
            }
        }

        private void ConfigurarModoEdicao()
        {
            btnAtualizar.Visibility = Visibility.Visible;
            btnSalvar.Visibility = Visibility.Collapsed;
            btnRefazerSenha.Visibility = Visibility.Visible;
        }

        private void btnRefazerSenha_Click(object sender, RoutedEventArgs e)
        {
            bool senhaVisivel = campoSenha.Visibility == Visibility.Visible;
            campoSenha.Visibility = campoCSenha.Visibility = senhaVisivel ? Visibility.Collapsed : Visibility.Visible;
        }

        private bool ValidarFormulario()
        {
            Growl.Clear("MessageTk");

            if (!campoNome.ValidarTexto() || !campoEmail.ValidarEmail())
            {
                Growl.Error("Preencha todos os campos corretamente!", "MessageTk");
                return false;
            }

            if (gdCargo.SelectedIndex == -1)
            {
                Growl.Error("Selecione um cargo!", "MessageTk");
                return false;
            }

            if (campoSenha.Visibility == Visibility.Visible)
            {
                if (!campoSenha.ValidarSenha() || !campoCSenha.ValidarSenha())
                {
                    Growl.Error("Senhas inválidas!", "MessageTk");
                    return false;
                }

                if (campoSenha.GetSenha() != campoCSenha.GetSenha())
                {
                    Growl.Error("Senhas estão diferentes!", "MessageTk");
                    return false;
                }
            }

            return true;
        }

        private void salvarUsuario()
        {
            if (!ValidarFormulario())
                return;

            try
            {
                bool? resultadoSenha = ConfirmacaoDeSenha.SolicitarConfirmacao();

                if (resultadoSenha == null)
                {
                    Growl.Info("Operação cancelada pelo usuário.", "MessageTk");
                    return;
                }
                else if (resultadoSenha == false)
                {
                    Growl.Warning("Senha incorreta. Operação cancelada.", "MessageTk");
                    return;
                }

                user.Nome = campoNome.GetTexto();
                user.Email = campoEmail.GetEmail();
                user.Cargo = (Cargo)gdCargo.SelectedIndex;

                if (user.ID != null)
                {
                    controller.EditarUsuario((int)user.ID, user.Email, user.Nome, (int)user.Cargo);

                    if (campoSenha.Visibility == Visibility.Visible)
                        controller.TrocarSenhaUsuario((int)user.ID, campoSenha.GetSenha());
                    Growl.Success("Usuário Atualizado com sucesso!", "MessageTk");
                }
                else
                {
                    controller.AdicionarUsuario(user.Email, user.Nome, campoSenha.GetSenha(), (int)user.Cargo);
                    Growl.Success("Usuário salvo com sucesso!", "MessageTk");
                    limparForms();
                }
            }
            catch (Exception ex)
            {
                string mensagem = controller.ObterMensagemErro(ex);
                Growl.Error($"Erro ao salvar usuário: {mensagem}", "MessageTk");
            }
        }

        private void limparForms()
        {
            campoNome.limparTexto();
            campoEmail.limparEmail();
            campoSenha.limparSenha();
            campoCSenha.limparSenha();
            gdCargo.SelectedIndex = -1;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e) => salvarUsuario();
        private void btnAtualizar_Click(object sender, RoutedEventArgs e) => salvarUsuario();
    }
}
