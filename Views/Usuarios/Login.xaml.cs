using SistemaZero.Views.ItemsReutilizaveis;
using HandyControl.Controls;
using SistemaZero.Controller;
using System.Windows;

namespace SistemaZero.Views.Usuarios
{
    public partial class Login : System.Windows.Window
    {
        private Email campoEmail;
        private Senha campoSenha;

        public Login()
        {
            InitializeComponent();

            string emailSalvo = Properties.Settings.Default.EmailSalvo;
            if (!string.IsNullOrWhiteSpace(emailSalvo))
            {
                campoEmail = new Email("Insira seu Email", "usuario@gmail.com", emailSalvo);
                lembrar.IsChecked = true;
            }
            else campoEmail = new Email("Insira seu Email", "usuario@gmail.com");

            campoSenha = new Senha("Insira sua Senha", "***********");

            gdEmail.Children.Add(campoEmail);
            gdSenha.Children.Add(campoSenha);
            
        }

        private void Enviar()
        {
            Growl.Clear("LoginToken");
            if (!campoEmail.ValidarEmail() || !campoSenha.ValidarSenha()) { return; };

            UserController userController = new UserController();
            try
            {
                if (!userController.Logon(campoEmail.GetEmail(), campoSenha.GetSenha()))
                {
                    Growl.Error("Login/Senha Incorreto", "LoginToken");
                    return;
                }
            }
            catch (Exception ex)
            {
                Growl.Error(userController.ObterMensagemErro(ex), "LoginToken");
                return;
            }
            if (lembrar.IsChecked == true)
            {
                Properties.Settings.Default.EmailSalvo = campoEmail.GetEmail();
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.EmailSalvo = string.Empty;
                Properties.Settings.Default.Save();
            }

            openMain();
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            
            Enviar();
        }

        private void openMain()
        {
            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            System.Windows.Window.GetWindow(this).Close();
        }

        //se o user clicr em lembrar, salvar o email, para o prox login
    }
}
