using SistemaZero.Helpers;
using System.Diagnostics;
using System.Windows;
using static Uno.UI.FeatureConfiguration;


namespace SistemaZero.Views.Produtos
{
    //Essa pagina é dedicada para calcular o preço do produto no mercado livre e com base na média geral do frete
    //margem de erro de até R$1 acima do preço do mercado livre (por causa de arredondamentos), porém é uma margem positiva no lucro
    //os calculos feitos são:
    //Calcular o minimo possivel para lucrar (abaixo desse preço é causado um prejuizo ao vendedor)
    //Calcular o frete médio pelo correios
    //Calcular o preço com uma margen de lucro em cima do preço real do produto (no caso o preço inicial, exemplo: [10% de R$80 -> R$8, e é feito o calculo do mercado livre em cima dos R$88]) 
    //todos os anteriores com o fator de quantidade



    //
    //public partial class PagDeCalculo : Window
    //{
    //    public PagDeCalculo()
    //    {
    //        InitializeComponent();
    //        freteComboBox.SelectedIndex = 0;
    //        cmbCaixas.SelectedIndex = 0;
    //        qtdUnidML.ResetData();
    //        resetarFrete(0.4M, 8, 13);
    //        inicializarTitulos();
    //    }

    //    private string tipo = "";

    //    private void inicializarTitulos()
    //    {
    //        precoUnidML.Titulo.Text = "Valor Unidade";
    //        taxaML.Titulo.Text = "Taxa Mercado Livre";
    //        lucroPercML.Titulo.Text = "Lucro em cima do produto";
    //        qtdUnidML.Titulo.Text = "Quantidade de Unidades";

    //        //correios
    //        pesoTotalML.Titulo.Text = "Peso total";
    //        pesoUnidML.Titulo.Text = "Peso Unidade";
    //        textosCaixa();

    //        //resultados
    //        VTProduto.Titulo.Text = "Preço Produto/Kit";
    //        VTLucroML.Titulo.Text = "Lucro";
    //        VTtaxaMinimaML.Titulo.Text = "Taxa sem o lucro, Mercado Livre";
    //        VTtaxaMaximaML.Titulo.Text = "Taxa total, Mercado Livre";
    //        VTcorreiosML.Titulo.Text = "Média Frete";
    //        VTfinalML.Titulo.Text = "Valor Total com lucro";
    //        VTvalorMinimo.Titulo.Text = "Valor Minimo sem lucro";

    //        //decoração texto
    //        VTProduto.Txt.HorizontalAlignment = HorizontalAlignment.Center;
    //        VTLucroML.Txt.HorizontalAlignment = HorizontalAlignment.Center;
    //        VTtaxaMinimaML.Txt.HorizontalAlignment = HorizontalAlignment.Center;
    //        VTtaxaMaximaML.Txt.HorizontalAlignment = HorizontalAlignment.Center;
    //        VTcorreiosML.Txt.HorizontalAlignment = HorizontalAlignment.Center;
    //        VTvalorMinimo.Txt.HorizontalAlignment = HorizontalAlignment.Center;
    //        VTfinalML.Txt.HorizontalAlignment = HorizontalAlignment.Center;
    //        VTfinalML.Txt.FontWeight = FontWeights.Bold;
    //    }

    //    private void textosCaixa()
    //    {
    //        alturaML.Visibility = Visibility.Visible;
    //        pesoEntregaML.Titulo.Text = "Peso da Caixa";
    //        alturaML.Titulo.Text = "Altura da Caixa";
    //        larguraML.Titulo.Text = "Largura da Caixa";
    //        comprimentoML.Titulo.Text = "Comprimento da Caixa";
    //    }

    //    private void textosEnvelope()
    //    {
    //        alturaML.Visibility = Visibility.Collapsed;
    //        pesoEntregaML.Titulo.Text = "Peso do Envelope";
    //        larguraML.Titulo.Text = "Largura do Envelope";
    //        comprimentoML.Titulo.Text = "Comprimento do Envelope";
    //    }


    //    private void freteComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    //    {
    //        if (freteComboBox.SelectedIndex == -1) return;
    //        if (freteComboBox.SelectedIndex == 0)
    //        {
    //            gridCalculoFrete.Visibility = Visibility.Collapsed;
    //            return;
    //        }
    //        else if (freteComboBox.SelectedIndex == 1)
    //        {
    //            pesoTotalML.Visibility = Visibility.Collapsed;
    //            pesoUnidML.Visibility = Visibility.Visible;
    //            pesoEntregaML.Visibility = Visibility.Visible;
    //        }
    //        else
    //        {
    //            pesoTotalML.Visibility = Visibility.Visible;
    //            pesoUnidML.Visibility = Visibility.Collapsed;
    //            pesoEntregaML.Visibility = Visibility.Collapsed;
    //        }
    //        gridCalculoFrete.Visibility = Visibility.Visible;
    //    }

    //    private void resetarFrete(decimal minAlt, decimal minLargura, decimal minComprimento)
    //    {
    //        alturaML.MinimumValue = minAlt;
    //        larguraML.MinimumValue = minLargura;
    //        comprimentoML.MinimumValue = minComprimento;

    //        pesoEntregaML.resetData();
    //        alturaML.resetData();
    //        larguraML.resetData();
    //        comprimentoML.resetData();
    //    }

    //    private void cmbCaixas_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    //    {
    //        if (cmbCaixas.SelectedIndex == -1) return;

    //        switch (cmbCaixas.SelectedIndex)
    //        {
    //            case 0:
    //                tipo = "Caixa";
    //                resetarFrete(0.4M, 8, 13);
    //                break;
    //            case 1:
    //                tipo = "Caixa";
    //                pesoEntregaML.GramasTextBox.Text = 60.ToString();
    //                alturaML.CmTextBox.Text = (6.5).ToString();
    //                larguraML.CmTextBox.Text = (15.5).ToString();
    //                comprimentoML.CmTextBox.Text = (16).ToString();
    //                break;
    //            case 2:
    //                tipo = "Envelope";
    //                resetarFrete(0, 11, 16);
    //                break;
    //            case 3:
    //                tipo = "Envelope";
    //                pesoEntregaML.GramasTextBox.Text = 5.ToString();
    //                larguraML.CmTextBox.Text = (12).ToString();
    //                comprimentoML.CmTextBox.Text = (18).ToString();
    //                break;
    //        }

    //        if (tipo == "Caixa") textosCaixa();
    //        else if (tipo == "Envelope") textosEnvelope();
    //    }
    //    private bool calculaPerimetro(decimal altura, decimal largura, decimal comprimento)
    //    {
    //        if (tipo == "Caixa" && altura + largura + comprimento > 200)
    //        {
    //            MessageBox.Show("A soma da Altura, Largura e Comprimento, não pode passar de 200 cm");
    //            return false;
    //        }
    //        else if (tipo == "Envelope" && largura + comprimento > 120)
    //        {
    //            MessageBox.Show("A soma da Largura e Comprimento, não pode passar de 120 cm");
    //            return false;
    //        }

    //        else return true;
    //    }

    //    private void btnCalcular_Click(object sender, RoutedEventArgs e)
    //    {
    //        inicializarConta();
    //    }

    //    private decimal calcularTaxaML(decimal preco, decimal taxML)
    //    {
    //        decimal precoSemTaxa = preco * (1 - (taxML / 100));

    //        decimal taxaPercentual = preco - precoSemTaxa;

    //        decimal taxaFixa = ObterCustoFixo(precoSemTaxa);

    //        return taxaPercentual + taxaFixa;
    //    }

    //    //eu sei que era apenas pegar o preço total bruto (sem frete ou taxa) e fazer a % de lucro, mas assim é apenas para confirmar
    //    private decimal calcularLucro(decimal precoTotal, decimal taxaTotal, decimal lucroPerc, decimal mediaFrete)
    //    {
    //        if (lucroPerc == 0) return 0;
    //        decimal valorComLucro = precoTotal - taxaTotal - mediaFrete;
    //        decimal valorSemLucro = valorComLucro / (1 + (lucroPerc / 100));
    //        decimal lucroAdicionado = valorComLucro - valorSemLucro;
    //        return lucroAdicionado;
    //    }

    //    private decimal ObterCustoFixo(decimal valor)
    //    {
    //        if (valor < 29) return 6.25M;
    //        if (valor < 50) return 6.50M;
    //        if (valor < 79) return 6.75M;
    //        return 0;
    //    }

    //    private decimal calculoValorMin(decimal precoTotal, decimal taxML, decimal mediaFrete)
    //    {
    //        precoTotal += mediaFrete;
    //        decimal custoFixo = ObterCustoFixo(precoTotal);

    //        precoTotal = (precoTotal + custoFixo) / (1 - (taxML / 100));
    //        decimal custoFixoRefatorizado = ObterCustoFixo(precoTotal);

    //        precoTotal -= custoFixo / (1 - (taxML / 100));
    //        precoTotal += custoFixoRefatorizado / (1 - (taxML / 100));

    //        return precoTotal;
    //    }

    //    private decimal calcularTotal(decimal precoTotal, decimal taxML, decimal lucro, decimal mediaFrete)
    //    {
    //        if (lucro != 0)
    //        {
    //            precoTotal *= (1 + (lucro / 100));
    //        }
    //        return calculoValorMin(precoTotal, taxML, mediaFrete);
    //    }

    //    private async Task<decimal> calcularMediaFrete(int qtd)
    //    {
    //        decimal altura = alturaML.Value;
    //        decimal largura = larguraML.Value;
    //        decimal comprimento = comprimentoML.Value;
    //        if (!calculaPerimetro(altura, largura, comprimento)) return 0;
    //        //isso é so para separar em calculos diferentes, se é caixa ou envelope
    //        if (freteComboBox.SelectedIndex == 1)
    //        {
    //            int pUnid = pesoUnidML.Value;
    //            int pEntrega = pesoEntregaML.Value;

    //            decimal pFinal = ((pUnid * qtd) + pEntrega) / 1000m;
    //            if (pFinal < 0.01m) { MessageBox.Show("Peso não pode ser menor que 10 g"); return 0; }

    //            if(tipo == "Caixa")
    //                return ArredondarParaCima(await APIFrete.FreteResponseBox(altura, largura, comprimento, pFinal));
    //            else if (tipo == "Envelope")
    //                return ArredondarParaCima(await APIFrete.FreteResponseEnv(largura, comprimento, pFinal));
    //        }
    //        else
    //        {
    //            decimal pTotal = pesoTotalML.Value;
    //            if (pTotal <= 0.01m) { MessageBox.Show("Peso não pode ser menor que 10 g"); return 0; }

    //            if (tipo == "Caixa")
    //                return ArredondarParaCima(await APIFrete.FreteResponseBox(altura, largura, comprimento, pTotal));
    //            else if (tipo == "Envelope")
    //                return ArredondarParaCima(await APIFrete.FreteResponseEnv(largura, comprimento, pTotal));
    //        }
    //        return 0;
    //    }

    //    private async void inicializarConta()
    //    {
    //        if (freteComboBox.SelectedIndex == -1) return;
    //        if (precoUnidML.Value == 0) { precoUnidML.ShowError("Não pode ser 0"); return; }


    //        decimal precUnid = precoUnidML.Value;
    //        int qtd = qtdUnidML.Value;
    //        decimal precoProd = precUnid * qtd;
    //        decimal taxML = taxaML.Value;
    //        decimal lucro = lucroPercML.Value;
    //        decimal mediaFrete = 0;

    //        if (freteComboBox.SelectedIndex != 0)
    //        {
    //            mediaFrete = await calcularMediaFrete(qtd);
    //            if (mediaFrete == 0) return;
    //            VTcorreiosML.Visibility = Visibility.Visible;
    //            VTcorreiosML.Txt.Text = $"R$ {mediaFrete}";
    //        }
    //        else VTcorreiosML.Visibility = Visibility.Hidden;

    //        decimal totalML = ArredondarParaCima(calcularTotal(precoProd, taxML, lucro, mediaFrete));
    //        decimal taxaTotal = ArredondarParaCima(calcularTaxaML(totalML, taxML));
    //        decimal lucroTotal = ArredondarParaCima(calcularLucro(totalML, taxaTotal, lucro, mediaFrete)); //mesmo ja tendo um lucro (que é fixo, essa conta é só para ter a total certeza)
    //        decimal valorMinimo = ArredondarParaCima(calculoValorMin(precoProd, taxML, mediaFrete));
    //        decimal taxaMinima = ArredondarParaCima(calcularTaxaML(valorMinimo, taxML));

    //        VTProduto.Txt.Text = $"R$ {precoProd:F2}";
    //        VTLucroML.Txt.Text = $"R$ {lucroTotal:F2}";
    //        VTvalorMinimo.Txt.Text = $"R$ {valorMinimo:F2}";
    //        VTtaxaMinimaML.Txt.Text = $"R$ {taxaMinima:F2}";
    //        VTtaxaMaximaML.Txt.Text = $"R$ {taxaTotal:F2}";
    //        VTfinalML.Txt.Text = $"R$ {totalML:F2}";

    //        ResultadoML.Visibility = Visibility.Visible;
    //        ResultadoML2.Visibility = Visibility.Visible;

    //        MessageBox.Show("Calculo realizado");
    //    }

    //    private decimal ArredondarParaCima(decimal valor)
    //    {
    //        return Math.Ceiling(valor * 100) / 100;
    //    }
    //}

}