using System;
using System.Collections.Generic;

#nullable disable

namespace src.Models
{
    public partial class Endereco
    {
        public Endereco()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdEndereco { get; set; }
        public string NumCep { get; set; }
        public string DsLogradouro { get; set; }
        public string Dsbairro { get; set; }
        public string DsCidade { get; set; }
        public string DsEstado { get; set; }
        public string DsNumero { get; set; }
        public int UserId { get; set; }
        public bool IsAtivo { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
