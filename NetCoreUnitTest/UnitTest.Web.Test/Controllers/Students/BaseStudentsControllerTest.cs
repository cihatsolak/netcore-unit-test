using Microsoft.EntityFrameworkCore;
using UnitTest.Web.Controllers;
using UnitTest.Web.Models;

namespace UnitTest.Web.Test.Controllers.Students
{
    public class BaseStudentsControllerTest
    {
        #region Fields and Properties
        protected DbContextOptions<UnitTestDBContext> _contextOptions { get; set; }

        /// <summary>
        /// Her istekte yeni bir context oluşturur. Insert,Update,Delete işlemleri için kullanılmamalıdır.
        /// </summary>
        protected UnitTestDBContext Context => new(_contextOptions);

        /// <summary>
        /// Her istekte yeni bir context oluşturur. Insert,Update,Delete işlemleri için kullanılmamalıdır.
        /// </summary>
        protected StudentsController StudentsController => new(new UnitTestDBContext(_contextOptions));
        #endregion

        #region Ctor
        public void SetContextOptions(DbContextOptions<UnitTestDBContext> contextOptions)
        {
            _contextOptions = contextOptions;
            SeedData();
        }
        #endregion


        /// <summary>
        /// Test metotu ayaga kalktıgında seed metotu çalışacak, veriler tablolara eklenecek
        /// </summary>
        public void SeedData()
        {
            using var context = new UnitTestDBContext(_contextOptions);
            context.Database.EnsureDeleted(); //Veri tabanını sil
            context.Database.EnsureCreated(); //Veri tabanını sıfırdan tekrar oluştur.

            context.Schools.AddRange(new School
            {
                Name = "Test Okul 1"
            }, new School
            {
                Name = "Test Okul 2"
            });

            context.Students.AddRange(new Student
            {
                SchoolId = 1,
                Name = "Ali",
                Address = "İstanbul"
            }, new Student
            {
                SchoolId = 2,
                Name = "Ahmet",
                Address = "İzmir"
            }, new Student
            {
                SchoolId = 2,
                Name = "Ayşe",
                Address = "Antalya"
            });

            context.SaveChanges();
        }
    }
}

