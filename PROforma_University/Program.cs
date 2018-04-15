using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyConsole; 

namespace PROforma_University
{
    class Program
    {
        static void Main(string[] args)
        {
            var conn = new DbConnection();
            Console.WriteLine("Hello, are you logging in as a:");
            var menu = new Menu();
            menu.Add("Administrator",() => {
                var adminActions = new AdminActions(conn);
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                var adminMenu = new Menu();
               adminMenu.Add("Add a Professor", () => {
                   var title = Input.ReadString("Please Enter Professor's Title (Mr/Ms/Mrs/Dr):");
                   var name = Input.ReadString("Please Enter First and Last Name of Professor:");
                   adminActions.AddProfessor(title, name);
               });
               adminMenu.Add("Add a Class", () => {
                   var number = Input.ReadInt("Please Enter New Course Number:", 1, 500);
                   var level = Input.ReadInt("Please Enter New Course Level:", 101, 401);
                   var name = Input.ReadString("Please Enter New Course Name:");
                   var room = Input.ReadInt("Please Enter Room Number of New Course:", 100, 1000);
                   var startTime = Input.ReadString("Please Enter Start Time of New Course (HH:MM):");
                   adminActions.AddCourse(number, level, name, room, TimeSpan.Parse(startTime));
               });
               adminMenu.Add("View Course Enrollment List", () => {
                   var courseNumber = Input.ReadInt("Please Enter Course Number to View Enrollment List:", 1, 500);
                   var course = adminActions.FindCourse(courseNumber);
                   while (course.Read())
                   {
                       Console.WriteLine("Student's Name: {0}", course["FullName"]);
                       Console.WriteLine("Student's Email: {0}", course["Email"]);
                       Console.WriteLine("Student's Phone Number: {0}", course["PhoneNumber"]);
                       Console.WriteLine("Student's Major: {0}", course["Major"]);
                       Console.WriteLine("----------");
                   }
                   course.Close();   //close reader to use connection again
                   Console.ReadLine();
               });
               adminMenu.Add("View All Classes", () => {
                   var courses = adminActions.ViewAll();
                   while (courses.Read())
                   {
                       Console.WriteLine("Courses: {0}", courses["CourseName"]);
                       Console.WriteLine("Professor: {0}", courses["ProfessorName"]);
                       Console.WriteLine("Student: {0}", courses["StudentName"]);
                       Console.WriteLine("----------");
                   }
                   courses.Close();   //close reader to use connection again
                   Console.ReadLine();
               });
               adminMenu.Display();
            });
            menu.Add("Professor", () => { });
            menu.Add("Student", () => {
                var studentAction = new StudentAction(conn);
                var studentMenu = new Menu();
                studentMenu.Add("Enroll in a Course", () =>
                {
                    var courses = studentAction.FindAllCourses();
                    var courseMenu = new Menu();
                    int selection = 0;
                    while (courses.Read())
                    {
                        var number = courses["Number"];
                        courseMenu.Add(String.Format("{0}-{1}", courses["Number"], courses["Name"]), () =>
                        {
                            selection = (int)number;
                        });
                    }
                    courses.Close();  //close reader to use connection again
                    courseMenu.Display();
                    var fullName = Input.ReadString("Please Enter Your First and Last Name:");
                    var email = Input.ReadString("Please Enter Your Email:");
                    var phoneNumber = Input.ReadString("Please Enter Your Phone Numer:");
                    var major = Input.ReadString("Please Enter Your Intended Major:");
                    studentAction.EnrollInCourse(fullName, email, phoneNumber, major, selection);
                });
                studentMenu.Add("View Your Classes For This Semester", () =>
                {
                    var fullName = Input.ReadString("Please Enter Your First and Last Name:");
                    var courses = studentAction.ViewClassList(fullName);
                    while (courses.Read())
                    {
                        Console.WriteLine("Course Name: {0}", courses["Name"]);
                        Console.WriteLine("Course Number: {0}", courses["Number"]);
                        Console.WriteLine("Course Level: {0}", courses["Level"]);
                        Console.WriteLine("Course Room Number: {0}", courses["Room"]);
                        Console.WriteLine("Course Start Time: {0}", courses["StartTime"]);
                        Console.WriteLine("----------");
                    }
                    courses.Close();   //close reader to use connection again
                    Console.ReadLine();
                });
                studentMenu.Display();
             });
            menu.Add("Exit", () => System.Environment.Exit(0));
            while (true)
            {
                Console.Clear();
                menu.Display();
            }
        }
    }
}
