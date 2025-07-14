using HandyControl.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SistemaZero.Views.ItemsReutilizaveis
{

    public partial class SeletorDeImagemUnica : UserControl
    {
        private string? url = null;

        public SeletorDeImagemUnica()
        {
            InitializeComponent();
        }

        public SeletorDeImagemUnica(string? url, bool edit) : this()
        {
            if (url != null)
            {
                this.url = url;
                CarregarImagem(url);
            }
            if (!edit)
            {
                btnAddImg.Visibility = Visibility.Collapsed;
                btnRemoveImg.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAddImg_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Selecione uma imagem",
                Filter = "Arquivos de imagem|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (dialog.ShowDialog() == true)
            {
                url = dialog.FileName;
                CarregarImagem(url);
            }
        }

        private void btnRemoveImg_Click(object sender, RoutedEventArgs e)
        {
            ClearImg();
        }

        public void ClearImg()
        {
            containerImg.Background = null; // limpa o fundo
            url = null;
            btnRemoveImg.Visibility = Visibility.Collapsed;
            btnAddImg.Visibility = Visibility.Visible;
        }

        private void CarregarImagem(string caminho)
        {
            if (File.Exists(caminho))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(caminho, UriKind.Absolute);
                bitmap.EndInit();

                // Cria e seta o ImageBrush com a imagem
                var brush = new ImageBrush(bitmap)
                {
                    Stretch = Stretch.UniformToFill
                };

                containerImg.Background = brush;

                btnAddImg.Visibility = Visibility.Collapsed;
                btnRemoveImg.Visibility = Visibility.Visible;
            }
        }

        public string? GetUrl()
        {
            return url;
        }
    }
}
