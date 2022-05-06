using Data.Domain;
using Data.Domain.HomeInfo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<DiagnoseHistory> DiagnoseHistories { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<Pacient> Pacients { get; set; }
        public DbSet<PacientVaccination> PacientVaccinations { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Country> Countries { get; set; } //
        public DbSet<District> Districts { get; set; } //
        public DbSet<Region> Regions { get; set; } //
        public DbSet<Street> Streets { get; set; } //
        public DbSet<Town> Towns { get; set; } //
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasOne(x => x.Address)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppUser>()
                .HasOne(x => x.Contacts)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Pacient>()
                .HasOne(x => x.Card)
                .WithOne(x => x.Pacient)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Card>()
                .HasOne(x => x.Pacient)
                .WithOne(x => x.Card)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Ticket>()
                .HasOne(x => x.Pacient)
                .WithMany(x => x.Tickets)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Schedule>()
                .HasOne(x => x.Doctor)
                .WithMany(x => x.Schedule)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<PacientVaccination>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Vaccinations)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Examination>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Examinations)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Allergy>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Allergies)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Recommendation>()
                .HasOne(x => x.Doctor)
                .WithMany(x => x.Recomemndations)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Examination>()
                .HasOne(x => x.Doctor)
                .WithMany(x => x.Examinations)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<DiagnoseHistory>()
                .HasOne(x => x.Doctor)
                .WithMany(x => x.Histories)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Diagnose>()
                .HasOne(x => x.DoctorEstablishe)
                .WithMany(x => x.EstablisheDiagnoses)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Diagnose>()
                .HasOne(x => x.DoctorConfirm)
                .WithMany(x => x.ConfirmedDiagnoses)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Diagnose>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Diagnoses)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DiagnoseHistory>()
                .HasOne(x => x.Diagnose)
                .WithMany(x => x.DiagnoseHistorie)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Allergy>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Allergies)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Examination>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Examinations)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<PacientVaccination>()
               .HasOne(x => x.Card)
               .WithMany(x => x.Vaccinations)
               .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Recommendation>()
               .HasOne(x => x.Card)
               .WithMany(x => x.Recommendations)
               .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
