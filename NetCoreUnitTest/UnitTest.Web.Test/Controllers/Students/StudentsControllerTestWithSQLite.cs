using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UnitTest.Web.Models;

namespace UnitTest.Web.Test.Controllers.Students
{
    public class StudentsControllerTestWithSQLite : BaseStudentsControllerTest
    {
        public StudentsControllerTestWithSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory");
            connection.Open();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<UnitTestDBContext>();
            dbContextOptionsBuilder.UseSqlite(connection);

            SetContextOptions(dbContextOptionsBuilder.Options);
        }
    }
}
