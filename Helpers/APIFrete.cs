using RestSharp;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace SistemaZero.Helpers
{
    class APIFrete
    {
        private const string Url = ""; //Api do calculo do frete
        private const string BearerToken = ""; // Substituir com seu token real
        private const string UserAgent = ""; //usuario
        private static readonly List<string> Ceps = new() { "", "", "", "" };// o primeiro espaço é o seu Cep, os outros sendo os ceps para calcular a média (recomendavel poucos ceps por ser uso de uma API)

        public static async Task<decimal> FreteResponseBox(decimal altura, decimal largura, decimal comprimento, decimal peso)
        {
            var options = new RestClientOptions(Url);
            var client = new RestClient(options);

            var sedexPrices = new List<decimal>();

            for (int i = 1; i < Ceps.Count; i++) // Ceps[0] é origem
            {
                var request = new RestRequest("");
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", BearerToken);
                request.AddHeader("User-Agent", UserAgent);

                var body = new
                {
                    from = new { postal_code = Ceps[0] },
                    to = new { postal_code = Ceps[i] },
                    products = new[]
                    {
                new
                {
                    id = i,
                    width = largura,
                    height = altura,
                    length = comprimento,
                    weight = peso,
                    insurance_value = 0,
                    quantity = 1
                }
            }
                };

                Debug.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
                request.AddJsonBody(body);

                var response = await client.PostAsync(request);
                Debug.WriteLine($"Response: {response}");

                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    var json = JsonDocument.Parse(response.Content);
                    var sedex = json.RootElement.EnumerateArray()
                        .FirstOrDefault(x => x.GetProperty("name").GetString()?.Contains("SEDEX", StringComparison.OrdinalIgnoreCase) == true);

                    if (sedex.ValueKind != JsonValueKind.Undefined && sedex.TryGetProperty("price", out var priceProp))
                    {
                        if (decimal.TryParse(priceProp.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                        {
                            sedexPrices.Add(price);
                            Debug.WriteLine($"Frete SEDEX para {Ceps[i]}: R$ {price:F2}");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"Erro ao consultar o frete para {Ceps[i]}: {response.Content}");
                }
            }

            if (sedexPrices.Any())
            {
                var media = sedexPrices.Average();
                return media;
            }
            else
            {
                Debug.WriteLine("Nenhum preço de SEDEX foi retornado.");
                return 0;
            }

        }
        //caso especifico de envelope
        public static async Task<decimal> FreteResponseEnv(decimal largura, decimal comprimento, decimal peso)
        {
            var options = new RestClientOptions(Url);
            var client = new RestClient(options);

            var sedexPrices = new List<decimal>();

            for (int i = 1; i < Ceps.Count; i++) // Ceps[0] é origem
            {
                var request = new RestRequest("");
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", BearerToken);
                request.AddHeader("User-Agent", UserAgent);

                var body = new
                {
                    from = new { postal_code = Ceps[0] },
                    to = new { postal_code = Ceps[i] },
                    products = new[]
                    {
                new
                {
                    id = i,
                    width = largura,
                    height = 1, // altura fixa ou mínima para envelopes
                    length = comprimento,
                    weight = peso,
                    insurance_value = 0,
                    quantity = 1
                }
            }
                };

                Debug.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
                request.AddJsonBody(body);

                var response = await client.PostAsync(request);
                Debug.WriteLine($"Response: {response}");

                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    var json = JsonDocument.Parse(response.Content);
                    var sedex = json.RootElement.EnumerateArray()
                        .FirstOrDefault(x => x.GetProperty("name").GetString()?.Contains("SEDEX", StringComparison.OrdinalIgnoreCase) == true);

                    if (sedex.ValueKind != JsonValueKind.Undefined && sedex.TryGetProperty("price", out var priceProp))
                    {
                        if (decimal.TryParse(priceProp.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                        {
                            sedexPrices.Add(price);
                            Debug.WriteLine($"Frete SEDEX para {Ceps[i]}: R$ {price:F2}");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"Erro ao consultar o frete para {Ceps[i]}: {response.Content}");
                }
            }

            if (sedexPrices.Any())
            {
                return sedexPrices.Average();
            }
            else
            {
                Debug.WriteLine("Nenhum preço de SEDEX foi retornado.");
                return 0;
            }
        }
    }

}

