using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produtos
    {   //Anotattion do SQLite
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
    }
}
