using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vaquinha.Domain.Entities;

namespace Vaquinha.Repository.Mapping
{
    public class DonationMapping : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(e => e.Valor)
                .IsRequired()
                .HasColumnType("decimal(9,2)");

            builder.Property(e => e.DataHora)
                .IsRequired();

            builder.HasOne(d => d.DadosPessoais)
                .WithMany(p => p.Doacoes)
                .HasForeignKey(d => d.DadosPessoaisId);

            builder.HasOne(d => d.AddressCobranca)
                .WithMany(e => e.Doacoes)
                .HasForeignKey(d => d.AddressCobrancaId);

            // nao salva os dados de cartao na base de dados
            builder.Ignore(e => e.FormaPagamento);
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.ErrorMessages);

            builder.ToTable("Donation");
        }
    }
}