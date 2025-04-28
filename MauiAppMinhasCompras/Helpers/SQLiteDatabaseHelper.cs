using MauiAppMinhasCompras.Models;
using SQLite;
using System.Collections;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        // Construtor sem chamadas assíncronas
        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Método para inicializar, criando a tabela e migrando o banco
        public async Task InicializarAsync()
        {
            await _conn.CreateTableAsync<Produto>();  // Cria a tabela Produto
            await VerificarEMigrarAsync();            // Verifica e faz a migração se necessário
        }

        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id
            );
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        public async Task VerificarEMigrarAsync()
        {
            try
            {
                var info = await _conn.GetTableInfoAsync("Produto");
                var categoriaExiste = info.Any(x => x.Name.ToLower() == "categoria");

                if (!categoriaExiste)
                {
                    await _conn.ExecuteAsync("ALTER TABLE Produto ADD COLUMN Categoria TEXT");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao migrar banco: {ex.Message}");
            }
        }
        // SQLiteDatabaseHelper.cs

        public async Task<List<RelatorioCategoria>> GetGastosPorCategoria()
        {
            string sql = @"
                SELECT Categoria, SUM(Preco * Quantidade) as TotalGasto
                FROM Produto
                GROUP BY Categoria
                ORDER BY TotalGasto DESC";

            // Aqui criamos uma classe que vai conter os dados do relatório
            var resultados = await _conn.QueryAsync<RelatorioCategoria>(sql);

            return resultados;
        }

        internal async Task<IList> GetCategorias()
        {
            throw new NotImplementedException();
        }
    }
}
