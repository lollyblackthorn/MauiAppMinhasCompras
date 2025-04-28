﻿using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        private string _descricao = string.Empty; // Fix: Initialize with a default non-null value

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a descrição");
                }

                _descricao = value;
            }
        }
        public double Quantidade { get; set; }
        public double Preco { get; set; }

        public string Categoria { get; set; } = string.Empty; // Fix: Initialize with a default non-null value
        public double Total { get => Quantidade * Preco; }
        
    }
}
