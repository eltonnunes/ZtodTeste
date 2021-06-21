using System;
using System.Collections.Generic;

#nullable disable

namespace src.Models
{
    public partial class Produto
    {
        public Produto()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Estoque { get; set; }
        public int SubcategoriaIdSubcategoria { get; set; }

        public virtual Subcategorium SubcategoriaIdSubcategoriaNavigation { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
