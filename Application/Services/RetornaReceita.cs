using DesafioBackendAPI.Application.ViewModel;
using DesafioBackendAPI.Application.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DesafioBackendAPI.Application.Services
{
    /// <summary>
    /// Serviço responsável por retornar o nome associado a um CNPJ consultando a ReceitaWS.
    /// </summary>
    public class RetornaReceita : IRetornaReceita
    {
        private readonly HttpClient _httpClient;

        public RetornaReceita(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retorna o nome associado ao CNPJ fornecido.
        /// </summary>
        /// <param name="cnpj">O CNPJ a ser consultado.</param>
        /// <returns>O nome associado ao CNPJ ou null se não encontrado.</returns>
        public async Task<string?> RetornaNome(string cnpj, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "cnpj/" + cnpj);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(content))
                {
                    var receitaCnpj = JsonConvert.DeserializeObject<ReceitaCnpjVM>(content);
                    return receitaCnpj?.Nome;
                }
            }
            else if ((int)response.StatusCode == 429)
            {
                return "TooManyRequests";
            }

            return null;
        }
    }
}