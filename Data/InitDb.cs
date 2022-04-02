using Data.Domain;
using Data.Domain.HomeInfo;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class InitDb
    {
        public static async Task Initialize(IServiceProvider serivce)
        {
            var db = serivce.GetRequiredService<ApplicationDbContext>();
            var userManager = serivce.GetRequiredService<UserManager<AppUser>>();
            var rolesManager = serivce.GetRequiredService<RoleManager<IdentityRole>>();

            await CreateBranches(db);
            await CreateSpecs(db);
            await CreateRoles(rolesManager);
            await CreateAdmin(userManager);
            await CreateDoctor(db, userManager);
            await CreatePacients(db, userManager);

        }

        private static async Task CreateBranches(ApplicationDbContext context)
        {
            if (context.Branches.Count() <= 0)
            {
                var b = new List<Branch>()
                {
                    new Branch(){Name = "Отделение 1", Description = "Описание отделения 1"},
                    new Branch(){Name = "Отделение 2", Description = "Описание отделения 2"},
                    new Branch(){Name = "Отделение 3", Description = "Описание отделения 3"}

                };
                await context.Branches.AddRangeAsync(b);
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateSpecs(ApplicationDbContext context)
        {
            if (context.Specialities.Count() <= 0)
            {
                var s = new List<Speciality>()
                {
                    new Speciality(){ Name = "Специальность 1", Description = "Описание специальности 1"},
                    new Speciality(){ Name = "Специальность 2", Description = "Описание специальности 2"}
                };
                await context.Specialities.AddRangeAsync(s);
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            var pacientRole = "Pacient";
            var doctorRole = "Doctor";
            var adminRole = "Admin";
            var role = await roleManager.FindByNameAsync(pacientRole);
            if (role == null)
            {
                await roleManager.CreateAsync(new IdentityRole(pacientRole));
            }

            role = await roleManager.FindByNameAsync(doctorRole);
            if (role == null)
            {
                await roleManager.CreateAsync(new IdentityRole(doctorRole));
            }

            role = await roleManager.FindByNameAsync(adminRole);
            if (role == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }
        }

        private static async Task CreateAdmin(UserManager<AppUser> userManager)
        {
            var adminEmail = "admin@gmail.com";
            var adminPassword = "1234567";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser()
                {
                    Email = adminEmail,
                    Name1 = "Администратор",
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, adminPassword);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        private static async Task CreateDoctor(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            //var doctorEmail = "doctor@gmail.com";
            //var doctorPassword = "1234567";

            //var doctorUser = await userManager.FindByEmailAsync(doctorEmail);
            //if (doctorUser == null)
            //{
            //    var user = new Doctor()
            //    {
            //        Email = doctorEmail,
            //        Name1 = "Алексей",
            //        Name2 = "Шелов",
            //        Name3 = "Генадьевич",
            //        UserName = doctorEmail,
            //        EmailConfirmed = true,
            //        RegistrationDate = DateTime.Now,
            //        Contacts = new Contacts(),
            //        Male = Gender.Male,
            //        Address = new Address()
            //    };

            //    user.Speciality = context.Specialities.FirstOrDefault();
            //    user.Branch = context.Branches.FirstOrDefault();
            //    await userManager.CreateAsync(user, doctorPassword);
            //    await userManager.AddToRoleAsync(user, "Doctor");
            //}

            if (context.Doctors.Count() == 0)
            {
                var doctors = new List<Doctor>()
                {
                    new Doctor()
                    {
                        Email = "doctor1@gmail.com",
                        Name1 = "Алексей",
                        Name2 = "Шелов",
                        Name3 = "Генадьевич",
                        UserName = "doctor1@gmail.com",
                        EmailConfirmed = true,
                        RegistrationDate = DateTime.Now,
                        Contacts = new Contacts(),
                        Male = Gender.Male,
                        Address = new Address()
                        {
                            Country = "Беларусь",
                            District = "Московский",
                            Street = "ул.Кижеватова",
                            StreetType = StreetType.Street,
                            Town = "Минск",
                            TownType = TownType.Town,
                            HomeNumber = "5",
                            ApartmentNumber = "122"

                        }
                    },

                    new Doctor()
                    {
                        Email = "doctor2@gmail.com",
                        Name1 = "Елена",
                        Name2 = "Шилко",
                        Name3 = "Генадьевна",
                        UserName = "doctor2@gmail.com",
                        EmailConfirmed = true,
                        RegistrationDate = DateTime.Now,
                        Contacts = new Contacts(),
                        Male = Gender.Female,
                        Address = new Address()
                        {
                            Country = "Беларусь",
                            District = "Заводской",
                            Street = "ул.Гаврилова",
                            StreetType = StreetType.Street,
                            Town = "Минск",
                            TownType = TownType.Town,
                            HomeNumber = "11",
                            ApartmentNumber = "311"

                        }
                    }
                };

                foreach(var user in doctors)
                {
                    user.Speciality = context.Specialities.FirstOrDefault();
                    user.Branch = context.Branches.FirstOrDefault();
                    await userManager.CreateAsync(user, "1234567");
                    await userManager.AddToRoleAsync(user, "Doctor");
                }
            }
        }

        private static async Task CreatePacients(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            if (context.Pacients.Count() == 0)
            {
                var pacients = new List<Pacient>()
                {
                    new Pacient()
                    {
                        Email = "pacient1@gmail.com",
                        Name1 = "Иван",
                        Name2 = "Иванович",
                        Name3 = "Иванов",
                        UserName = "pacient1@gmail.com",
                        EmailConfirmed = true,
                        RegistrationDate= DateTime.Now,
                        Card = new Card()
                        {
                            Number = 11,
                            DateOfIssue = DateTime.Now,
                        },
                        Male = Gender.Male,
                        Address = new Address()
                        {
                            Country = "Беларусь",
                            District = "Московский",
                            Street = "ул.Кижеватова",
                            StreetType = StreetType.Street,
                            Town = "Минск",
                            TownType = TownType.Town,
                            HomeNumber = "5",
                            ApartmentNumber = "122"

                        },
                        Contacts = new Contacts()
                    },

                    new Pacient()
                    {
                        Email = "pacient2@gmail.com",
                        Name1 = "Петр",
                        Name2 = "Комаров",
                        Name3 = "Иванович",
                        UserName = "pacient2@gmail.com",
                        EmailConfirmed = true,
                        RegistrationDate= DateTime.Now,
                        Card = new Card()
                        {
                            Number = 12,
                            DateOfIssue = DateTime.Now,
                        },
                        Male = Gender.Male,
                        Address = new Address()
                        {
                            Country = "Беларусь",
                            District = "Октябрьский",
                            Street = "ул.Корженевского",
                            StreetType = StreetType.Street,
                            Town = "Минск",
                            TownType = TownType.Town,
                            HomeNumber = "26",
                            ApartmentNumber = "140"

                        },
                        Contacts = new Contacts()
                    },

                };

                foreach (var user in pacients)
                {
                    await userManager.CreateAsync(user, "1234567");
                    await userManager.AddToRoleAsync(user, "Pacient");
                }
            }
        }
    }
}
