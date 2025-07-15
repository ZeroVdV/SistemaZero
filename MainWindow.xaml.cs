using SistemaZero.Views.Menu;
using HandyControl.Controls;
using SistemaZero.Controller;
using System.Windows;
using System.Windows.Controls;

namespace SistemaZero
{
    public partial class MainWindow : System.Windows.Window
    {
        private class TelaComTitulo
        {
            public UserControl Tela { get; set; }
            public string Titulo { get; set; }

            public TelaComTitulo(UserControl tela, string titulo)
            {
                Tela = tela;
                Titulo = titulo;
            }
        }

        private Stack<TelaComTitulo> historicoTelas = new();
        public MainWindow()
        {
            UserController userController = new UserController();
            userController.VerificarLogon();
            InitializeComponent();
            TelaInicial();
        }

        public void NavegarPara(UserControl novaTela, string titulo)
        {
            if (ConteudoPrincipal.Content is UserControl telaAtual)
            {
                var tituloAtual = cabecalho.Titulo; // <- Captura o título atualmente visível
                historicoTelas.Push(new TelaComTitulo(telaAtual, tituloAtual));
            }
            cabecalho.SetTitulo(titulo);
            ConteudoPrincipal.Content = novaTela;
            AtualizarBotaoVoltar();
        }

        public void Voltar(bool clear = true)
        {
            if (historicoTelas.Count > 1)
            {
                var anterior = historicoTelas.Pop();
                ConteudoPrincipal.Content = anterior.Tela;
                cabecalho.SetTitulo(anterior.Titulo);
            }

            AtualizarBotaoVoltar(clear);
        }

        public void TelaInicial()
        {
            historicoTelas.Clear();
            var paginaInicial = new PaginaInicial();
            ConteudoPrincipal.Content = paginaInicial;

            cabecalho.SetTitulo("Página Inicial");
            historicoTelas.Push(new TelaComTitulo(paginaInicial, "Página Inicial"));

            AtualizarBotaoVoltar();
        }

        private void AtualizarBotaoVoltar(bool clear = true)
        {
            cabecalho.SetBotaoVoltarVisivel(historicoTelas.Count > 1);
            if(clear) Growl.Clear("MessageTk");
        }
    }
}