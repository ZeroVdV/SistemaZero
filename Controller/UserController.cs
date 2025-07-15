using SistemaZero.Model;
using SistemaZero.Model.Entity;
using SistemaZero.Views.Usuarios;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace SistemaZero.Controller
{
    class UserController
    {
        private ConexaoBD conexao = new();

        #region Sessão do usuario
        public bool Logon(string email, string senha)
        {
            UserSession.Iniciar(conexao.ObterLogin(email, HashSenha(senha)));
            if (UserSession.atual != null)
            {
                return true;
            }
            return false;
        }

        public bool ConfirmarSenha(int idUsuario, string senha)
        {
            User? user = new ConexaoBD().ObterUsuarioPorIdSenha(idUsuario, HashSenha(senha));
            return user != null;
        }

        public string ObterMensagemErro(Exception ex)
        {
            string mensagem = ex.Message;
            Exception inner = ex.InnerException;

            while (inner != null)
            {
                mensagem += "\n→ " + inner.Message;
                inner = inner.InnerException;
            }

            return mensagem;
        }

        public void Logout()
        {
            UserSession.Encerrar();
            var login = new Login();
            login.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window != login)
                    window.Close();
            }
        }

        public void VerificarLogon()
        {
            if (UserSession.atual == null) { Logout(); }
        }

        public bool VerificarAcessoAPagina()
        {
            if (!VerificarPermissao())
            {
                MessageBox.Show("Você não tem permissão para acessar essa página.");
                return false;
                //chamar antes na iniciação da pagina e dar um Close(); se falso
            }
            return true;

        }

        public string GetNomeUsuario()
        {
            if (UserSession.atual == null || string.IsNullOrWhiteSpace(UserSession.atual.Nome))
                return "Desconectado";

            string nome = UserSession.atual.Nome.Split(' ')[0];

            if (nome.Length > 15)
                nome = nome.Substring(0, 15) + "...";

            return nome;
        }

        public int GetIdUsuario() //essa função só é chamada após verificação do login do usuario
        {
            return (int)UserSession.atual!.ID!;
        }

        public bool VerificarPermissao() //esse é pra verificar na inicialização da pagina
        {
            VerificarLogon();
            return GetPermissao();
        }

        public bool GetPermissao() //esse é p switch do cargo
        {
            return UserSession.atual!.Cargo switch
            {
                0 => true, //Administrador 
                _ => false
            };
        }

        public bool ConfirmarComSenha(int id, string senha)
        {
            return conexao.confirmacaoComSenha(id, senha);
        }

        /*
        public static bool MostrarPopUpSenha() //arrumar isso depois
        {
            var dialog = new PswConfirmationPopup();
            if (dialog.ShowDialog() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */

        #endregion

        #region Controle de Usuarios

        public User? ObterUmUsuario(int id)
        {
            return conexao.ObterUsuario(id);
        }

        public List<User> ObterVariosUsuarios(int id_atual, int qtd_retorno)
        {
            return conexao.ObterUsuarios(id_atual, qtd_retorno);
        }

        public List<User> PesquisarUsuarios(int id_atual, int qtd_retorno, string pesq)
        {
            return conexao.PesquisarUsuario(id_atual, qtd_retorno, pesq);
        }

        public bool VerificaEmail(string email)
        {
            return conexao.verificaEmail(email);
        }

        public void AdicionarUsuario(string email, string nome, string senha, int cargo)
        {
            conexao.adicionarUsuario(email, nome, HashSenha(senha), cargo);
        }

        public void MudarStatusUsuariio(int id, bool status)
        {
            conexao.mudarStatusUsuariio(id, status);
        }

        public void EditarUsuario(int id, string email, string nome, int cargo)
        {
            conexao.editarUsuario(id, email, nome, cargo);
        }

        public void TrocarSenhaUsuario(int id, string senha)
        {
            conexao.TrocarSenhaUsuario(id, HashSenha(senha));
        }

        #endregion

        private static string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }
    }
}
