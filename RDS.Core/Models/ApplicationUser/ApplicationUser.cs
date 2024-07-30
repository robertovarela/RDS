﻿namespace RDS.Core.Models.ApplicationUser;

public class ApplicationUser : IdentityUser<long>
{
    public string? Name { get; set; }
    public string? Cpf { get; set; }
    public DateOnly? BirthDate { get; set; }
    public List<ApplicationUserAddress>? Address { get; set; }
    public List<ApplicationUserTelephone>? Telephone { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;

    //public IList<ApplicationRole> Roles { get; set; } = [];
}