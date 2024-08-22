namespace RDS.Api.Data.Mappings.Company
{
    public class CompanyUserMapping : IEntityTypeConfiguration<Core.Models.Company.CompanyUser>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Company.CompanyUser> builder)
        {
            builder.ToTable("CompanyUser");

            builder.HasKey(cu => new { cu.CompanyId, cu.UserId });

            builder.Property(cu => cu.CompanyId)
                .IsRequired()
                .HasColumnType("BIGINT");

            builder.Property(cu => cu.UserId)
                .IsRequired()
                .HasColumnType("BIGINT");

            builder.Property(cu => cu.IsAdmin)
                .IsRequired()
                .HasColumnType("BIT");

            builder.HasOne(cu => cu.Company)
                .WithMany(c => c.CompanyUsers)
                .HasForeignKey(cu => cu.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cu => cu.User)
                .WithMany(u => u.CompanyUsers)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}