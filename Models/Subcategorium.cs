using System;
using System.Collections.Generic;

#nullable disable

namespace src.Models
{
    public partial class Subcategorium
    {
        public Subcategorium()
        {
            Produtos = new HashSet<Produto>();
        }

        public int IdSubcategoria { get; set; }
        public string Nome { get; set; }
        public int CategoriaIdCategoria { get; set; }

        public virtual Categorium CategoriaIdCategoriaNavigation { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
