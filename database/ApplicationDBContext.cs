using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using src.database.Models;

namespace src.database
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        /*public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Extrato> Extratos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<SubCategoria> SubCategorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }*/
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {}

        
    }
}