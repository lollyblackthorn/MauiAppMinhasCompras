using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;//campo privado onde nosso bd será armazenado

        public static SQLiteDatabaseHelper Db // Propriedade pública que retorna o banco de dados, criando uma instância caso ela ainda não exista

        {
            get
            {// Verifica se a instância do banco de dados já foi criada
                if (_db == null)
                {   // Caminho onde o banco de dados SQLite será salvo
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.db3");
                 // Criação da instância do helper para o banco de dados, passando o caminho

                    _db = new SQLiteDatabaseHelper(path);
                }

                return _db;// Retorna a instância do banco de dados
            }
        }
        // Construtor do aplicativo, onde a inicialização acontece
        public App()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR"); //idioma padrao do app

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Views.ListaProduto());// Define a página principal do aplicativo como a lista de produtos (ListaProduto)
        }
    }
}
