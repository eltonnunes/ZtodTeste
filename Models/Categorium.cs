using System;
using System.Collections.Generic;

#nullable disable

namespace src.Models
{
    public partial class Categorium
    {
        public Categorium()
        {
            Subcategoria = new HashSet<Subcategorium>();
        }

        public int IdCategoria { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Subcategorium> Subcategoria { get; set; }
    }
}
