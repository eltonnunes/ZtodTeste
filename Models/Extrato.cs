using System;
using System.Collections.Generic;

#nullable disable

namespace src.Models
{
    public partial class Extrato
    {
        public int IdExtrato { get; set; }
        public int UserId { get; set; }
        public DateTime DtExtrato { get; set; }
        public string FlTipo { get; set; }
        public decimal VlMovimentado { get; set; }

        public virtual User User { get; set; }
    }
}
