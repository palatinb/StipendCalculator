using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StudyStipendCalcAPI.Models
{
    public partial class CalculatorDBContext : DbContext
    {
        public CalculatorDBContext()
        {
        }

        public CalculatorDBContext(DbContextOptions<CalculatorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LinkRolesMenus> LinkRolesMenus { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Universities> Universities { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LinkRolesMenus>(entity =>
            {
                entity.ToTable("link_roles_menus");

                entity.HasIndex(e => e.MenuId)
                    .HasName("menu_id");

                entity.HasIndex(e => e.RoleId)
                    .HasName("role_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MenuId)
                    .HasColumnName("menu_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.LinkRolesMenus)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("link_roles_menus_ibfk_1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.LinkRolesMenus)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("link_roles_menus_ibfk_2");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parent_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasIndex(e => e.Uid)
                    .HasName("uid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.Uid)
                    .HasConstraintName("Roles_ibfk_1");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.Uid)
                    .HasName("uid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccceptedCredit)
                    .HasColumnName("Acccepted_credit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActiveSemester)
                    .HasColumnName("Active_semester")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AllSemesters).HasColumnType("int(11)");

                entity.Property(e => e.CreditIndex).HasColumnName("Credit_index");

                entity.Property(e => e.EarnedCredit)
                    .HasColumnName("Earned_credit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExceptedCredit)
                    .HasColumnName("Excepted_credit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FinancialState)
                    .IsRequired()
                    .HasColumnName("Financial_state")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.FinishedSemester)
                    .HasColumnName("Finished_semester")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GroupIndex).HasColumnName("Group_index");

                entity.Property(e => e.ModulCode)
                    .IsRequired()
                    .HasColumnName("Modul_code")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ModulName)
                    .IsRequired()
                    .HasColumnName("Modul_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.NeptunCode)
                    .IsRequired()
                    .HasColumnName("Neptun_code")
                    .HasColumnType("varchar(6)");

                entity.Property(e => e.PassiveSemester)
                    .HasColumnName("Passive_semester")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StipendAmmount)
                    .HasColumnName("Stipend_ammount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StipendIndex).HasColumnName("Stipend_index");

                entity.Property(e => e.StudentGrop)
                    .IsRequired()
                    .HasColumnName("Student_grop")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.TelephelyName)
                    .IsRequired()
                    .HasColumnName("Telephely_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Year).HasColumnType("int(11)");

                entity.Property(e => e.YearOfEnrollment)
                    .IsRequired()
                    .HasColumnName("Year_of_enrollment")
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Student_ibfk_1");
            });

            modelBuilder.Entity<Universities>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dbname)
                    .HasColumnName("DBName")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("Role_id");

                entity.HasIndex(e => e.UiD)
                    .HasName("UiD");

                entity.HasIndex(e => new { e.Username, e.PasswordHash })
                    .HasName("Username")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RoleId)
                    .HasColumnName("Role_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UiD).HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("varchar(120)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_ibfk2");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UiD)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_ibfk_1");
            });
        }
    }
}
