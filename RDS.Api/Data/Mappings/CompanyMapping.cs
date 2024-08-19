namespace RDS.Api.Data.Mappings;

public class CompanyMapping : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Company");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("BIGINT");

        // builder.HasMany(c => c.Transactions)
        //     .WithOne(t => t.Category)
        //     .HasForeignKey(t => t.CategoryId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}