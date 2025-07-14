using System.IO;

namespace SistemaZero.Model
{
    public class ServidorDeArquivos
    {
        //caminho para o servidor de arquivos, no meu exemplo é para servidor interno e não uma api
        public readonly string caminhoServ = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); //mudei para a pasta de imagens padrão do windows

        public string SalvarImagemNoServidor(string caminhoImg)
        {
            try
            {
                string nomeBase = Path.GetFileNameWithoutExtension(caminhoImg);
                string extensao = Path.GetExtension(caminhoImg);
                string nomeFinal = nomeBase + extensao;
                string destino = Path.Combine(caminhoServ, nomeFinal);

                int contador = 1;
                while (File.Exists(destino))
                {
                    nomeFinal = $"{nomeBase}({contador}){extensao}";
                    destino = Path.Combine(caminhoServ, nomeFinal);
                    contador++;
                }

                // Abrir a imagem como stream
                using (FileStream origemStream = new FileStream(caminhoImg, FileMode.Open, FileAccess.Read))
                using (FileStream destinoStream = new FileStream(destino, FileMode.Create, FileAccess.Write))
                {
                    origemStream.CopyTo(destinoStream);
                }

                return nomeFinal;
            }
            catch (Exception ex)
            {
                throw new IOException("Erro ao copiar imagem para o servidor.", ex);
            }
        }

        public string CarregarImagemDoServidor(string? link)
        {
            string caminhoImagem = Path.Combine(caminhoServ, link ?? "");

            if (File.Exists(caminhoImagem))
            {
                return caminhoImagem;
            }

            throw new FileNotFoundException("Imagem não encontrada no servidor.");
        }

        public void deletarImg(string nomeArq)
        {
            string caminhoImagem = Path.Combine(caminhoServ, nomeArq ?? "");

            try
            {
                if (File.Exists(caminhoImagem))
                {
                    File.Delete(caminhoImagem);
                }
                else
                {
                    throw new FileNotFoundException("Imagem a ser deletada não existe no servidor.");
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Erro ao deletar a imagem '{nomeArq}': {ex.Message}", ex);
            }
        }
    }
}
