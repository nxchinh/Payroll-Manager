using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Persistence.Configuration
{
    public class DocumentInfoConfiguration : IEntityTypeConfiguration<DocumentInfo>
    {
        public void Configure(EntityTypeBuilder<DocumentInfo> builder)
        {
            builder
                .HasMany(d => d.Familiarizations)
                .WithOne(f => f.Document)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}