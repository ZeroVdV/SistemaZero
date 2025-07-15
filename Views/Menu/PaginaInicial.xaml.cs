using SistemaZero.Controller;
using SistemaZero.Views.Produtos;
using SistemaZero.Views.Usuarios;
using System.Windows;
using System.Windows.Controls;
namespace SistemaZero.Views.Menu
{
    public partial class PaginaInicial : UserControl
    {
        public PaginaInicial()
        {
            UserController userController = new UserController();
            InitializeComponent();
            if (!userController.GetPermissao())
            {
                PainelInicial.Children.Remove(PagAddUser);
                PainelInicial.Children.Remove(PagListUser);
                PainelInicial.Children.Remove(PagListLog);
            }
        }
        private MainWindow? ObterMainWindow()
        {
            return Application.Current.MainWindow as MainWindow;
        }

        private void PagAddProd_Click(object sender, RoutedEventArgs e)
        {
            ObterMainWindow()?.NavegarPara(new PaginaProduto(), "Adicionar Produto");
        }
        private void PagListProd_Click(object sender, RoutedEventArgs e)
        {
            ObterMainWindow()?.NavegarPara(new PaginaListarProdutos(), "Listagem dos Produtos");
        }

        private void PagAddUser_Click(object sender, RoutedEventArgs e)
        {
            ObterMainWindow()?.NavegarPara(new PaginaUsuarios(), "Adicionar Usuario");
        }

        private void PagListUser_Click(object sender, RoutedEventArgs e)
        {
            ObterMainWindow()?.NavegarPara(new PaginaListarUsuarios(), "Listagem dos Usuarios");
        }

        private void PagListLog_Click(object sender, RoutedEventArgs e)
        {
            ObterMainWindow()?.NavegarPara(new LogProduto(), "Listagem de Registos");
        }
    }
}
