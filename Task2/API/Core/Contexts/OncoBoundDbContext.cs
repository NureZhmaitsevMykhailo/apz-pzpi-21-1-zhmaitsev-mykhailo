using API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Core.Contexts;

public class OncoBoundDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<MedicationLog> MedicationLogs { get; set; }

    public OncoBoundDbContext(DbContextOptions<OncoBoundDbContext> options)  : base(options) {}
}
