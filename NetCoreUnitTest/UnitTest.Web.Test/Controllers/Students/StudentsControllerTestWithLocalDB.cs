using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UnitTest.Web.Controllers;
using UnitTest.Web.Models;
using Xunit;

namespace UnitTest.Web.Test.Controllers.Students
{
    /// <summary>
    /// Base Class'dan Context ve StudentController property'leri kullanılmaktadır.
    /// </summary>
    public class StudentsControllerTestWithLocalDB : BaseStudentsControllerTest
    {
        private readonly string _sqlLocalDbConnectionString
            = @"Server=(localdb)\ProjectsV13;Database=StudentUnitTestDB;Trusted_Connection=true;MultipleActiveResultSets=true";

        public StudentsControllerTestWithLocalDB()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<UnitTestDBContext>();
            dbContextOptionsBuilder.UseSqlServer(_sqlLocalDbConnectionString);

            SetContextOptions(dbContextOptionsBuilder.Options);
        }

        /// <summary>
        /// EF Core InMemory ile öğrenci ekleme ve test etme
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreatePOST_ValidModelState_ReturnRedirectIndexActionAndInsertStudents()
        {
            var student = new Student
            {
                Name = "Unit test name",
                Address = "Unit test adres",
                SchoolId = 1
            };

            using (var context = new UnitTestDBContext(_contextOptions))
            {
                var studentController = new StudentsController(context);

                //StudentsController base class'dan geliyor
                IActionResult actionResult = await studentController.Create(student);

                var redirectToActionResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }

            //EF Core Entity Track ettiğinden dolayı Context'i her defada new()'leyerek kullanıyorum.
            //Her istekte new()'leyip test sonucunu garantiliyorum.
            var studentDbEntity = await Context.Students.FirstOrDefaultAsync(p => p.Id == student.Id);
            Assert.NotNull(studentDbEntity);
            Assert.Equal<int>(student.Id, studentDbEntity.Id);
        }
    }
}
