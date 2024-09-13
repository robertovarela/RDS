namespace RDS.Api.Data.Mappings.Identity;

public class IdentityUserMapping : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("IdentityUser");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).IsRequired().HasColumnType("BIGINT").HasColumnOrder(1);
        builder.Property(u => u.UserName).IsRequired().HasColumnType("NVARCHAR").HasMaxLength(256).HasColumnOrder(2);
        builder.Property(u => u.NormalizedUserName).IsRequired().HasColumnType("NVARCHAR").HasMaxLength(256).HasColumnOrder(3);
        builder.Property(u => u.Email).IsRequired().HasColumnType("NVARCHAR").HasMaxLength(256).HasColumnOrder(4);
        builder.Property(u => u.NormalizedEmail).IsRequired().HasColumnType("NVARCHAR").HasMaxLength(256).HasColumnOrder(5);
        builder.Property(u => u.EmailConfirmed).IsRequired().HasColumnType("BIT").HasMaxLength(1).HasColumnOrder(6);

        builder.Property(u => u.Name).IsRequired().HasColumnType("NVARCHAR").HasMaxLength(256).HasColumnOrder(7);
        builder.Property(u => u.Cpf).IsRequired(false).HasColumnType("NVARCHAR").HasMaxLength(1).HasMaxLength(11).HasColumnOrder(8);
        builder.Property(u => u.BirthDate).IsRequired(false).HasColumnType("DATE").HasColumnOrder(9);
        builder.Property(u => u.CreateAt).IsRequired().HasColumnType("DATETIME").HasColumnOrder(10);

        builder.Property(u => u.PasswordHash).IsRequired().HasColumnType("NVARCHAR(MAX)").HasColumnOrder(11);
        builder.Property(u => u.SecurityStamp).IsRequired(false).HasColumnType("NVARCHAR(MAX)").HasColumnOrder(12);
        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken().HasColumnOrder(13);
        builder.Property(u => u.PhoneNumber).IsRequired(false).HasColumnType("NVARCHAR").HasMaxLength(20).HasColumnOrder(14);
        builder.Property(u => u.PhoneNumberConfirmed).IsRequired().HasColumnType("BIT").HasColumnOrder(15);
        builder.Property(u => u.TwoFactorEnabled).IsRequired().HasColumnType("BIT").HasColumnOrder(16);
        builder.Property(u => u.LockoutEnd).IsRequired(false).HasColumnType("DATETIMEOFFSET").HasColumnOrder(17);
        builder.Property(u => u.LockoutEnabled).IsRequired().HasColumnType("BIT").HasColumnOrder(18);
        builder.Property(u => u.AccessFailedCount).IsRequired().HasColumnType("INT").HasColumnOrder(19);

        builder.HasIndex(u => new { u.NormalizedUserName }).IsUnique(false);
        builder.HasIndex(u => new { u.NormalizedEmail }).IsUnique();
        builder.HasIndex(u => new { u.Name }).IsUnique(false);
        builder.HasIndex(u => u.PhoneNumber).IsUnique(false);
        builder.HasIndex(u => u.Cpf).IsUnique().HasFilter("[Cpf] IS NOT NULL");

        builder.HasMany<IdentityUserClaim<long>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
        builder.HasMany<IdentityUserLogin<long>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
        builder.HasMany<IdentityUserToken<long>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        builder.HasMany<ApplicationUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

        builder.HasMany(u => u.Telephone)
                   .WithOne(t => t.User)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(a => a.Address)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
    }
}