using HandyControl.Controls;
using SistemaZero.Controller;
using SistemaZero.Model.Entity;
using SistemaZero.Views.ItemsReutilizaveis;
using System.Windows;

namespace SistemaZero.Views.ItemsUnicos
{
    /// <summary>
    /// Lógica interna para RedefinirSenha.xaml
    /// </summary>
    public partial class RedefinirSenha : HandyControl.Controls.Window
    {
        private Senha campoSenha;
        private Senha campoNovaSenha;
        private Senha campoNovaCSenha;
        private readonly UserController controller = new UserController();

        public RedefinirSenha()
        {
            InitializeComponent();
            campoSenha = new Senha("Digite sua senha:", "*******");
            campoNovaSenha = new Senha("Digite a nova senha:", "*******");
            campoNovaCSenha = new Senha("Confirme a nova senha:", "*******");
            SenhaContainer.Content = campoSenha;
            NovaSenhaContainer.Content = campoNovaSenha;
            NovaCSenhaContainer.Content = campoNovaCSenha;
        }
        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            int id = UserSession.atual?.ID ?? -1;
            if (id == -1)
            {
                Close();
                return;
            }

            if (!campoSenha.ValidarSenha() || !campoNovaSenha.ValidarSenha() || !campoNovaCSenha.ValidarSenha())
                return;

            if(campoSenha.GetSenha() == campoNovaSenha.GetSenha())
            {
                campoNovaSenha.ExibirAviso("A Senha é igual a antiga!");
                return;
            }

            if(campoNovaSenha.GetSenha() != campoNovaCSenha.GetSenha())
            {
                campoNovaCSenha.ExibirAviso("Senhas diferentes");
                return;
            }
            
            try
            {
                if (controller.ConfirmarSenha(id, campoSenha.GetSenha()))
                {
                    controller.TrocarSenhaUsuario(id, campoNovaSenha.GetSenha());
                    Growl.Success("Senha redefinida!", "MessageTk");
                }
                else
                {
                    campoSenha.ExibirAviso("A Senha está errada!");
                    return;
                }
            }
            catch(Exception ex)
            {
                Growl.Error("Erro ao redefinir a Senha!", "MessageTk");
            }
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
