using System;
using System.Data.SqlClient;  //gives access too all SQL command and connection classes

namespace PROforma_University
{
    internal class AdminActions: BaseAction
    {
       
        public AdminActions(DbConnection dbConnection)
            : base(dbConnection)
        {
           
        }

        internal void AddProfessor(string title, string name)
        {
            var cmd = new SqlCommand("INSERT INTO dbo.Professors (Title, Name) VALUES (@Title, @Name)");  //set up SQL command
            var titleParam = new SqlParameter("@Title", title);
            cmd.Parameters.Add(titleParam);   //add parameter for the title
            var nameParam = new SqlParameter("@Name", name);
            cmd.Parameters.Add(nameParam);   //add parameter for the name
            var reader = dbConn.Execute(cmd);
            reader.Close(); //close connection to use again
        }

        internal void AddCourse(int number, int level, string name, int room, TimeSpan startTime)
        {
            var cmd = new SqlCommand("INSERT INTO dbo.Courses (Number, [Level], Name, Room, StartTime) VALUES (@Number, @Level, @Name, @Room, @StartTime)");
            var numberParam = new SqlParameter("@Number", number);
            cmd.Parameters.Add(numberParam);
            var levelParam = new SqlParameter("@Level", level);
            cmd.Parameters.Add(levelParam);
            var nameParam = new SqlParameter("@Name", name);
            cmd.Parameters.Add(nameParam);
            var roomParam = new SqlParameter("@Room", room);
            cmd.Parameters.Add(roomParam);
            var startTimeParam = new SqlParameter("@StartTime", startTime);
            cmd.Parameters.Add(startTimeParam);
            var reader = dbConn.Execute(cmd);
            reader.Close();   //close connection to use again
        }

        internal SqlDataReader FindCourse(int courseNumber)
        {
            var cmd = new SqlCommand("SELECT dbo.Students.* " +
                "FROM dbo.Students " +
                "JOIN dbo.Enroll" +
                " ON dbo.Students.ID = dbo.Enroll.StudentId " +
                "JOIN dbo.Courses" +
                " ON dbo.Enroll.CourseId = dbo.Courses.ID" +
                " WHERE dbo.Courses.Number = @Number");
            var courseNumParam = new SqlParameter("@Number", courseNumber);
            cmd.Parameters.Add(courseNumParam);
            return dbConn.Execute(cmd);
        }

        internal SqlDataReader ViewAll()
        {
            var cmd = new SqlCommand("SELECT dbo.Courses.Name AS CourseName, dbo.Professors.Name AS ProfessorName, dbo.Students.FullName AS StudentName " + //Alias-ing 
                " FROM dbo.Courses " +
                "LEFT JOIN dbo.CourseList " +
                "ON dbo.Courses.ID = dbo.CourseList.CourseId " +
                "LEFT JOIN dbo.Professors" +
                " ON dbo.Professors.ID = dbo.CourseList.ProfessorId " +
                "JOIN dbo.Enroll " +
                "ON dbo.Courses.ID = dbo.Enroll.CourseId " +
                "JOIN dbo.Students " +
                "ON dbo.Students.ID = dbo.Enroll.StudentId " +
                "ORDER BY dbo.Courses.Number");
            return dbConn.Execute(cmd);
        }
    }
}