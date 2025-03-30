using MauiAppMinhasCompras.Models;  // Importa o modelo Produtos
using SQLite;  // Importa a biblioteca SQLite para interagir com o banco de dados

namespace MauiAppMinhasCompras.Helpers
{
    // Classe que fornece métodos auxiliares para interagir com o banco de dados SQLite
    public class SQLiteDatabaseHelper
    {
        // Cria uma conexão assíncrona com o banco de dados SQLite
        readonly SQLiteAsyncConnection _conn;

        // Construtor assíncrono da classe, que recebe o caminho do banco de dados como parâmetro
        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
        }

        // Método assíncrono para inicializar a tabela de Produtos
        public async Task InitializeDatabaseAsync()
        {
            // Cria a tabela de Produtos no banco de dados de forma assíncrona
            await _conn.CreateTableAsync<Produtos>();
        }

        // Método para inserir um novo produto na tabela
        public Task<int> Insert(Produtos p)
        {
            return _conn.InsertAsync(p);  // Insere o produto e retorna uma Task com o número de linhas afetadas
        }

        // Método para atualizar um produto existente na tabela
        public Task<int> Update(Produtos p)
        {
            // Define a query SQL para atualizar os dados do produto
            string sql = "UPDATE Produtos SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            // Executa a query de atualização no banco de dados
            return _conn.ExecuteAsync(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }

        // Método para deletar um produto da tabela com base no seu ID
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produtos>().DeleteAsync(i => i.Id == id);  // Deleta o produto com o ID fornecido
        }

        // Método para obter todos os produtos da tabela
        public Task<List<Produtos>> GetAll()
        {
            return _conn.Table<Produtos>().ToListAsync();  // Retorna todos os produtos em uma lista
        }

        // Método para buscar produtos pelo nome (ou descrição) com base em uma string de busca
        public Task<List<Produtos>> Search(string q)
        {
            // Define a query SQL para buscar produtos cujas descrições contenham a string fornecida
            string sql = "SELECT * FROM Produtos WHERE Descricao LIKE ?";

            // Executa a query e retorna os resultados
            return _conn.QueryAsync<Produtos>(sql, "%" + q + "%");
        }
    }
}
