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
    public class StudentsControllerTestWithInMemory : BaseStudentsControllerTest
    {
        public StudentsControllerTestWithInMemory()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<UnitTestDBContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase("UnitTestDB");

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

            //StudentsController base class'dan geliyor
            IActionResult actionResult = await StudentsController.Create(student);

            var redirectToActionResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            //EF Core Entity Track ettiğinden dolayı Context'i her defada new()'leyerek kullanıyorum.
            //Her istekte new()'leyip test sonucunu garantiliyorum.
            var studentDbEntity = await Context.Students.FirstOrDefaultAsync(p => p.Id == student.Id);
            Assert.NotNull(studentDbEntity);
            Assert.Equal<int>(student.Id, studentDbEntity.Id);
        }
    }
}
