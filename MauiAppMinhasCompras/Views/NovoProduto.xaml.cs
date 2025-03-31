using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage // criando página para adicionar um novo produto
{
	public NovoProduto()
	{
		InitializeComponent(); //inicializando a interface da page
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {//evento de clique do botao salvar
		try
		{
			Produto p = new Produto //criando o "esqueleto" do produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text)//conversao de string para number
			};

			await App.Db.Insert(p);//adiciona o produto no BD
			await DisplayAlert("Sucesso!", "Registro Inserido", "OK");//msg de sucesso
			await Navigation.PopAsync();//volta para a page anterior

		} catch(Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK"); //msg de erro caso ocorra
		}
    }
}