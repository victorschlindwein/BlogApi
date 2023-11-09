using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Tabela
            builder.ToTable("Category");

            //PK
            builder.HasKey(x => x.Id);

            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd() //GERADO AUTOMATICAMENTE PELO BANCO
                .UseIdentityColumn(); //PRIMARY KEY IDENTITY (1, 1)

            //Propriedades
            builder.Property(x => x.Name)
                .IsRequired() //NOT NULL
                .HasColumnName("Name") //OPCIONAL
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Slug)
                .IsRequired() //NOT NULL
                .HasColumnName("Slug") //OPCIONAL
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            //Índices
            builder.HasIndex(x => x.Slug, "IX_Category_Slug")
                .IsUnique();
        }
    }
}