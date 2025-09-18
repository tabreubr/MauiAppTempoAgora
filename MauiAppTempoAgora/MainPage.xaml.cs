using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {

                    lbl_res.Text = "Buscando...";

                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"🌍 Localização:\n" +
                                           $"Latitude: {t.lat:F4}\n" +
                                           $"Longitude: {t.lon:F4}\n\n" +
                                           $"🌤️ Condição: {(t.description)}\n\n" +
                                           $"🌅 Nascer do Sol: {t.sunrise:HH:mm}\n" +
                                           $"🌇 Por do Sol: {t.sunset:HH:mm}\n\n" +
                                           $"🌡️ Temperatura Máxima: {t.temp_max}°C\n" +
                                           $"🌡️ Temperatura Mínima: {t.temp_min}°C\n\n" +
                                           $"💨 Velocidade do Vento: {t.speed} m/s\n" +
                                           $"👁️ Visibilidade: {t.visibility} m";


                        lbl_res.Text = dados_previsao;

                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    }
                }
                else
                {
                    await DisplayAlert("Atenção", "Por favor, digite o nome de uma cidade.", "OK");
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (Exception ex)
            {
                // Tratamento específico para diferentes tipos de erro
                if (ex.Message.Contains("Cidade não encontrada"))
                {
                    await DisplayAlert("Cidade Não Encontrada",
                        "Não encontramos esta cidade. Verifique:\n\n• A ortografia do nome\n• Use o formato 'Cidade,País'", "OK");
                    lbl_res.Text = "Cidade não encontrada";
                }
                else if (ex.Message.Contains("Sem conexão com a internet"))
                {
                    await DisplayAlert("Sem Conexão",
                        "Você está offline. Conecte-se à internet para buscar previsões do tempo.", "OK");
                    lbl_res.Text = "Sem conexão com a internet";
                }
                else if (ex.Message.Contains("Erro na consulta"))
                {
                    await DisplayAlert("Erro na API", ex.Message, "OK");
                    lbl_res.Text = "Erro na consulta";
                }
                else
                {
                    await DisplayAlert("Erro", $"Ocorreu um erro inesperado:\n\n{ex.Message}", "OK");
                    lbl_res.Text = "Erro inesperado";
                }
            }
        }

        // Método para capitalizar a primeira letra
        private string CapitalizeFirstLetter(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}