using DesafioBackendAPI.Application.DTOs;

namespace DesafioBackendAPI.Application.Services
{
    /// <summary>
    /// Classe responsável por operações relacionadas a arquivos.
    /// </summary>
    public class RetornaArquivo
    {
        /// <summary>
        /// Salva um arquivo no disco, associando-o a uma conta específica.
        /// </summary>
        /// <param name="localArquivo">Caminho relativo do arquivo.</param>
        /// <param name="contaDto">Objeto DTO contendo informações da conta e o arquivo.</param>
        public static async Task SalvarArquivoDisco(string localArquivo, ContaDTO contaDto)
        {
            if (contaDto.Arquivo == null)
                throw new ArgumentException("Arquivo da conta não informado.", nameof(contaDto));

            var caminhoCompleto = Path.GetFullPath(localArquivo);
            var diretorio = Path.GetDirectoryName(caminhoCompleto);
            if (!string.IsNullOrEmpty(diretorio))
                Directory.CreateDirectory(diretorio);

            var fileCreated = false;
            try
            {
                await using var fileStream = new FileStream(caminhoCompleto, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fileCreated = true;
                await contaDto.Arquivo.CopyToAsync(fileStream).ConfigureAwait(false);
            }
            catch
            {
                if (fileCreated && System.IO.File.Exists(caminhoCompleto))
                    System.IO.File.Delete(caminhoCompleto);
                throw;
            }
        }

        public static void ExcluirArquivoDisco(string? localArquivo)
        {
            if (!string.IsNullOrWhiteSpace(localArquivo) && System.IO.File.Exists(localArquivo))
                System.IO.File.Delete(localArquivo);
        }

        /// <summary>
        /// Lê o conteúdo de um arquivo e retorna como uma string.
        /// </summary>
        /// <param name="filePath">Caminho do arquivo.</param>
        /// <returns>Conteúdo do arquivo como string.</returns>
        public static async Task<Byte[]> RecuperarArquivoDisco(string local_arquivo_completo)
        {
            return await System.IO.File.ReadAllBytesAsync(local_arquivo_completo);
        }
    }
}