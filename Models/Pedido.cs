using System;
using System.Collections.Generic;

#nullable disable

namespace src.Models
{
    public partial class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public int ProdutoIdProduto { get; set; }
        public string QuantidadeProduto { get; set; }
        public int UserId { get; set; }
        public bool FlStatus { get; set; }
        public DateTime? DataEntrega { get; set; }
        public int EnderecoIdEndereco { get; set; }

        public virtual Endereco EnderecoIdEnderecoNavigation { get; set; }
        public virtual Produto ProdutoIdProdutoNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
