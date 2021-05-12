using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UnitTest.Web.Models;
using Xunit;

namespace UnitTest.Web.Test.Controllers.Students
{
    public class StudentsControllerTestWithSQLite : BaseStudentsControllerTest
    {
        public StudentsControllerTestWithSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<UnitTestDBContext>();
            dbContextOptionsBuilder.UseSqlite(connection);

            SetContextOptions(dbContextOptionsBuilder.Options);
        }

        /// <summary>
        /// Ef Core InMemory'de ögrencinin bağlı bulunduğu okul silinse bile ögrencileri listeleyebiliyorduk.
        /// Sqlite ilişkisel veri tabanı olduğu için okul silindiğinde okula bağlı ögrencilerde silinecektir.
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(1)]
        public async Task DeleteSchool_ExitsSchoolId_DeletedAllStudents(int schoolId)
        {
            using (var context = new UnitTestDBContext(_contextOptions))
            {
                var school = await context.Schools.FindAsync(schoolId);
                Assert.NotNull(school);

                context.Schools.Remove(school);
                await context.SaveChangesAsync();
            }

            using (var context = new UnitTestDBContext(_contextOptions))
            {
                var students = await context.Students.Where(p => p.SchoolId == schoolId).ToListAsync();
                Assert.Empty(students);
            }
        }
    }
}
