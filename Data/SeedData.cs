using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using System;
using HR_Management.Infrastructure;

namespace HR_Management.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            
            //if (context.Database.GetPendingMigrations().Any())
            //{
            //    context.Database.Migrate();
            //}

            //if (!context.Educations.Any())
            //{
            //    // Person
            //    context.People.AddRange(
            //        new Person
            //        {
            //            StaffId = string.Format("2020101001"),
            //            FullName = "Hoang Dung",
            //            Email = "hoangdung@gmail.com",
            //            Location = (byte)eLocation.HAN,
            //            Description = "Than tan ma dai",
            //            Phone = "0922446688",
            //            YearOfBirth = new DateTime(1900, 12, 20),
            //            Gender = (byte)eGender.Female,
            //            CreatedBy = "Quach Tinh",
            //            UpdatedBy = "Quach Tinh",
            //            Status = true
            //        },
            //        new Person
            //        {
            //            StaffId = string.Format("2020101002"),
            //            FullName = "Dong Ta",
            //            Email = "dongta@gmail.com",
            //            Location = (byte)eLocation.HAN,
            //            Description = "Than tan ma dai",
            //            Phone = "0922446688",
            //            YearOfBirth = new DateTime(1900, 12, 20),
            //            Gender = (byte)eGender.Male,
            //            CreatedBy = "Hoang Dung",
            //            UpdatedBy = "Hoang Dung",
            //            Status = true
            //        });
            //    // Education
            //    context.Educations.AddRange(
            //        new Education
            //        {
            //            CollegeName = "Hanoi University of Mining and Geology",
            //            Major = "Data science",
            //            StartDate = new DateTime(2017, 8, 8),
            //            OrderIndex = 1,
            //            Status = true
            //        },
            //        new Education
            //        {
            //            CollegeName = "Thien Long Bat Bo",
            //            Major = "Luc Mach Than Kiem",
            //            StartDate = new DateTime(2019, 8, 8),
            //            EndDate = new DateTime(2020, 8, 8),
            //            OrderIndex = 2,
            //            Status = true
            //        });
            //    context.SaveChanges();
            //}
        }
    }
}