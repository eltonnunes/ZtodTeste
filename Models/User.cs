using System;
using System.Collections.Generic;

#nullable disable

namespace src.Models
{
    public partial class User
    {
        public User()
        {
            Enderecos = new HashSet<Endereco>();
            Extratos = new HashSet<Extrato>();
            Pedidos = new HashSet<Pedido>();
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string NumCpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Endereco> Enderecos { get; set; }
        public virtual ICollection<Extrato> Extratos { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
