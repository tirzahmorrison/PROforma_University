using System;
using System.Data.SqlClient;

namespace PROforma_University
{
    internal class StudentAction: BaseAction
    {
        public StudentAction(DbConnection dbConnection)
            : base(dbConnection)
        {
        }

        internal SqlDataReader FindAllCourses()
        {
            var cmd = new SqlCommand("SELECT Number, Name FROM dbo.Courses");
            return dbConn.Execute(cmd);
        }

        internal void EnrollInCourse(string fullName, string email, string phoneNumber, string major, int courseNumber)
        {
            var cmd = new SqlCommand("SELECT ID FROM dbo.Courses WHERE Number = @Number"); //get course Id from course number
            var numberParam = new SqlParameter("@Number", courseNumber);
            cmd.Parameters.Add(numberParam);
            var courseReader = dbConn.Execute(cmd);
            courseReader.Read();
            var courseId = (int)courseReader["ID"];
            courseReader.Close();   //close connection to use again
            var cmd2 = new SqlCommand("SELECT ID FROM dbo.Students WHERE FullName = @FullName");
            var fullNameParam = new SqlParameter("@FullName", fullName);
            cmd2.Parameters.Add(fullNameParam);
            var studentReader = dbConn.Execute(cmd2);
            int studentId;  //create variable
            if (studentReader.Read())   //student is in table
            {
                studentId = (int)studentReader["ID"];
                studentReader.Close();
            }
            else
            {
                studentReader.Close();
                var cmd3 = new SqlCommand("INSERT INTO dbo.Students (FullName, Email, PhoneNumber, Major) output INSERTED.ID VALUES(@FullName, @Email, @PhoneNumber, @Major)");
                cmd3.Parameters.AddWithValue("@FullName", fullName);
                cmd3.Parameters.AddWithValue("@Email", email);
                cmd3.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd3.Parameters.AddWithValue("@Major", major);
                studentId = (int)dbConn.ExecuteScalar(cmd3);
            }
            var cmd4 = new SqlCommand("INSERT INTO dbo.Enroll (StudentId, CourseId) VALUES(@StudentId, @CourseID)");
            cmd4.Parameters.AddWithValue("@StudentId", studentId);
            cmd4.Parameters.AddWithValue("@CourseId", courseId);
            var reader = dbConn.Execute(cmd4);
            reader.Close();
        }

        internal SqlDataReader ViewClassList(string fullName)
        {
            var cmd = new SqlCommand("SELECT dbo.Courses.* FROM dbo.Courses JOIN dbo.Enroll ON dbo.Courses.ID = dbo.Enroll.CourseId JOIN dbo.Students ON dbo.Students.ID = dbo.Enroll.StudentId WHERE Students.FullName = @FullName");
            cmd.Parameters.AddWithValue("@FullName", fullName);
            return dbConn.Execute(cmd);
        }
    }
}