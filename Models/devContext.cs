using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace src.Models
{
    public partial class devContext : DbContext
    {
        public devContext()
        {
        }

        public devContext(DbContextOptions<devContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categorium> Categoria { get; set; }
        public virtual DbSet<Endereco> Enderecos { get; set; }
        public virtual DbSet<Extrato> Extratos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<Subcategorium> Subcategoria { get; set; }
        public virtual DbSet<User> Users { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder
                .UseMySql("server=localhost;database=dev;user=user;password=pwd;treattinyasboolean=true", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"))
                .EnableDetailedErrors() ;
            }
        }*/


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");


            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PRIMARY");

                entity.ToTable("categoria");

                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasKey(e => e.IdEndereco)
                    .HasName("PRIMARY");

                entity.ToTable("endereco");

                entity.HasIndex(e => e.UserId, "fk_endereco_user1_idx");

                entity.Property(e => e.IdEndereco).HasColumnName("idEndereco");

                entity.Property(e => e.DsCidade)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("dsCidade");

                entity.Property(e => e.DsEstado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("dsEstado");

                entity.Property(e => e.DsLogradouro)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("dsLogradouro");

                entity.Property(e => e.DsNumero)
                    .HasMaxLength(6)
                    .HasColumnName("dsNumero");

                entity.Property(e => e.Dsbairro)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("dsbairro");

                entity.Property(e => e.IsAtivo).HasColumnName("isAtivo");

                entity.Property(e => e.NumCep)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("numCep");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Enderecos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_endereco_user1");
            });

            modelBuilder.Entity<Extrato>(entity =>
            {
                entity.HasKey(e => e.IdExtrato)
                    .HasName("PRIMARY");

                entity.ToTable("extrato");

                entity.HasIndex(e => e.UserId, "fk_extrato_user_idx");

                entity.HasIndex(e => e.FlTipo, "ix_flTipo");

                entity.Property(e => e.IdExtrato).HasColumnName("idExtrato");

                entity.Property(e => e.DtExtrato)
                    .HasColumnType("datetime")
                    .HasColumnName("dtExtrato");

                entity.Property(e => e.FlTipo)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("flTipo")
                    .HasComment("DEBIT OR CREDIT");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.VlMovimentado)
                    .HasPrecision(11, 2)
                    .HasColumnName("vlMovimentado");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Extratos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_extrato_user");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido)
                    .HasName("PRIMARY");

                entity.ToTable("pedido");

                entity.HasIndex(e => e.EnderecoIdEndereco, "fk_pedido_endereco1_idx");

                entity.HasIndex(e => e.ProdutoIdProduto, "fk_resgate_produto1_idx");

                entity.HasIndex(e => e.UserId, "fk_resgate_user1_idx");

                entity.Property(e => e.IdPedido).HasColumnName("idPedido");

                entity.Property(e => e.DataEntrega)
                    .HasColumnType("datetime")
                    .HasColumnName("dataEntrega");

                entity.Property(e => e.DataPedido)
                    .HasColumnType("datetime")
                    .HasColumnName("dataPedido");

                entity.Property(e => e.EnderecoIdEndereco).HasColumnName("enderecoIdEndereco");

                entity.Property(e => e.FlStatus).HasColumnName("flStatus");

                entity.Property(e => e.ProdutoIdProduto).HasColumnName("produtoIdProduto");

                entity.Property(e => e.QuantidadeProduto)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("quantidadeProduto");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.EnderecoIdEnderecoNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.EnderecoIdEndereco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pedido_endereco1");

                entity.HasOne(d => d.ProdutoIdProdutoNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.ProdutoIdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_resgate_produto1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_resgate_user1");
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.IdProduto)
                    .HasName("PRIMARY");

                entity.ToTable("produto");

                entity.HasIndex(e => e.SubcategoriaIdSubcategoria, "fk_produto_subcategoria1_idx");

                entity.Property(e => e.IdProduto).HasColumnName("idProduto");

                entity.Property(e => e.Estoque).HasColumnName("estoque");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("nome");

                entity.Property(e => e.SubcategoriaIdSubcategoria).HasColumnName("subcategoriaIdSubcategoria");

                entity.Property(e => e.ValorUnitario)
                    .HasPrecision(11, 2)
                    .HasColumnName("valorUnitario");

                entity.HasOne(d => d.SubcategoriaIdSubcategoriaNavigation)
                    .WithMany(p => p.Produtos)
                    .HasForeignKey(d => d.SubcategoriaIdSubcategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_produto_subcategoria1");
            });

            modelBuilder.Entity<Subcategorium>(entity =>
            {
                entity.HasKey(e => e.IdSubcategoria)
                    .HasName("PRIMARY");

                entity.ToTable("subcategoria");

                entity.HasIndex(e => e.CategoriaIdCategoria, "fk_subcategoria_categoria1_idx");

                entity.Property(e => e.IdSubcategoria).HasColumnName("idSubcategoria");

                entity.Property(e => e.CategoriaIdCategoria).HasColumnName("categoriaIdCategoria");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("nome");

                entity.HasOne(d => d.CategoriaIdCategoriaNavigation)
                    .WithMany(p => p.Subcategoria)
                    .HasForeignKey(d => d.CategoriaIdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_subcategoria_categoria1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Email, "email")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.NumCpf)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("numCpf");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(13)
                    .HasColumnName("phone");

                entity.Property(e => e.RealName)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("realName");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
