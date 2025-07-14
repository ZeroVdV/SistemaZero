using SistemaZero.Controller;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SistemaZero.Views.Menu
{
    public partial class Cabecalho : UserControl
    {
        public Cabecalho()
        {
            var userController = new UserController();
            string nome = userController.GetNomeUsuario();

            InitializeComponent();

            txtUsuario.Text = nome;
        }

        public static readonly DependencyProperty TituloProperty =
            DependencyProperty.Register("Titulo", typeof(string), typeof(Cabecalho), new PropertyMetadata("Título"));


        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Window.GetWindow(this)?.DragMove();
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.TelaInicial();
            }
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Abrir configurações");
        }

        private void Usuario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Perfil do usuário");
        }

        private void Minimizar_Click(object sender, RoutedEventArgs e)
        {
            var janela = Window.GetWindow(this);
            if (janela != null) janela.WindowState = WindowState.Minimized;
        }

        private void Maximizar_Click(object sender, RoutedEventArgs e)
        {
            var janela = Window.GetWindow(this);
            if (janela != null)
            {
                if (janela.WindowState == WindowState.Maximized)
                {
                    janela.WindowState = WindowState.Normal;
                    imgMaximizar.Source = new BitmapImage(new Uri("pack://application:,,,/Views/Imagens/Maximize.png"));
                }
                else
                {
                    janela.WindowState = WindowState.Maximized;
                    imgMaximizar.Source = new BitmapImage(new Uri("pack://application:,,,/Views/Imagens/Minimize.png"));
                }
            }
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void backWindow_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.Voltar();
            }
        }
        public void SetBotaoVoltarVisivel(bool visivel)
        {
            backWindow.Visibility = visivel ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetTitulo(string titulo)
        {
            this.titulo.Text = titulo;
        }
        public string Titulo => titulo.Text;

        private void TrocarSenha_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Abrir tela de trocar senha");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new UserController().Logout();
        }
    }
}
