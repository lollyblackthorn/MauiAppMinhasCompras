using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verifica se todos os campos foram preenchidos
            if (string.IsNullOrWhiteSpace(txt_descricao.Text) ||
                string.IsNullOrWhiteSpace(txt_quantidade.Text) ||
                string.IsNullOrWhiteSpace(txt_preco.Text) ||
                txt_categoria.SelectedItem == null)
            {
                await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
                return;
            }

            // Criação do objeto Produto com os dados preenchidos
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = txt_categoria.SelectedItem.ToString() // Categoria selecionada no Picker
            };

            // Salva o produto no banco de dados
            await App.Db.Insert(p);

            // Exibe uma mensagem de sucesso
            await DisplayAlert("Sucesso!", "Registro Inserido", "OK");

            // Retorna para a tela anterior
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // Em caso de erro, exibe a mensagem de erro
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
