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
            // Recupera os produtos do banco de dados de forma assíncrona
            List<Produto> tmp = await App.Db.GetAll();
            // Adiciona os produtos recuperados na lista
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Método para carregar as categorias no Picker
    private async void CarregarCategorias()
    {
        try
        {
            var categorias = await App.Db.GetCategorias(); // Supõe que você tem um método que retorna categorias
            categorias.Insert(0, "Todos"); // Adiciona "Todos" como opção
            picker_categoria.ItemsSource = categorias;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Método para tratar o evento de seleção de categoria
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
            // Caso contrário, filtra pela categoria selecionada
            var produtosFiltrados = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            produtosFiltrados.ForEach(i => lista.Add(i));
        }
    }

    // Pesquisa por descrição
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

    // Exibição do total de produtos
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        // Exibe o total em um pop-up
        string msg = $"O total é {soma:C}";

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
                bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");
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

    // Método para tratar o evento de seleção de item na ListView
    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // Verifica se o item selecionado é do tipo Produto
            if (e.SelectedItem is Produto p)
            {
                // Navega para a tela de edição do produto, passando o produto como BindingContext
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
            // Chama o método correto para obter o relatório de gastos por categoria
            var relatorio = await App.Db.GetGastosPorCategoria(); // Usando GetGastosPorCategoria

            // Cria uma string para exibir o relatório
            string relatorioTexto = "Relatório de Gastos por Categoria:\n\n";

            foreach (var item in relatorio)
            {
                relatorioTexto += $"{item.Categoria}: {item.TotalGasto:C}\n";
            }

            // Exibe o relatório em um alerta
            await DisplayAlert("Relatório de Gastos", relatorioTexto, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }


}
