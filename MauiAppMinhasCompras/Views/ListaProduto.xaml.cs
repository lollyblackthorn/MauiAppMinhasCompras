using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
        CarregarCategorias();
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();
            // Recupera os produtos do banco de dados de forma ass�ncrona
            List<Produto> tmp = await App.Db.GetAll();
            // Adiciona os produtos recuperados na lista
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // M�todo para carregar as categorias no Picker
    private async void CarregarCategorias()
    {
        try
        {
            var categorias = await App.Db.GetCategorias(); // Sup�e que voc� tem um m�todo que retorna categorias
            categorias.Insert(0, "Todos"); // Adiciona "Todos" como op��o
            picker_categoria.ItemsSource = categorias;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // M�todo para tratar o evento de sele��o de categoria
    private async void picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        string categoriaSelecionada = picker_categoria.SelectedItem.ToString();

        lista.Clear();

        List<Produto> tmp;

        // Aguarda a tarefa que recupera todos os produtos do banco de dados
        tmp = await App.Db.GetAll();

        // Se a categoria selecionada for "Todos", busca todos os produtos
        if (categoriaSelecionada == "Todos")
        {
            tmp.ForEach(i => lista.Add(i));
        }
        else
        {
            // Caso contr�rio, filtra pela categoria selecionada
            var produtosFiltrados = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            produtosFiltrados.ForEach(i => lista.Add(i));
        }
    }

    // Pesquisa por descri��o
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);
            // Adiciona os produtos encontrados na lista
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    // Exibi��o do total de produtos
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        // Exibe o total em um pop-up
        string msg = $"O total � {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    // Adiciona novo produto
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Remover produto
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem? selecionado = sender as MenuItem;
            if (selecionado?.BindingContext is Produto p)
            {
                bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "N�o");
                if (confirm)
                {
                    await App.Db.Delete(p.Id);
                    lista.Remove(p);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Atualiza a lista de produtos ao fazer um pull-to-refresh
    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    // M�todo para tratar o evento de sele��o de item na ListView
    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // Verifica se o item selecionado � do tipo Produto
            if (e.SelectedItem is Produto p)
            {
                // Navega para a tela de edi��o do produto, passando o produto como BindingContext
                await Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p,
                });
            }
        }
        catch (Exception ex)
        {
            // Exibe um alerta em caso de erro
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // ListaProduto.xaml.cs

    // ListaProduto.xaml.cs

    private async void ExibirRelatorioGastos_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Chama o m�todo correto para obter o relat�rio de gastos por categoria
            var relatorio = await App.Db.GetGastosPorCategoria(); // Usando GetGastosPorCategoria

            // Cria uma string para exibir o relat�rio
            string relatorioTexto = "Relat�rio de Gastos por Categoria:\n\n";

            foreach (var item in relatorio)
            {
                relatorioTexto += $"{item.Categoria}: {item.TotalGasto:C}\n";
            }

            // Exibe o relat�rio em um alerta
            await DisplayAlert("Relat�rio de Gastos", relatorioTexto, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }


}
