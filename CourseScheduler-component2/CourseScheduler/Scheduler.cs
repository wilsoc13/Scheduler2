using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseScheduler
{
    class Scheduler
    {
        // generate some sample objects
        Course course1 = new Course(100, "Computer Programming", 3, false, false, false, false, "DMSE", 30);
        Course course2 = new Course(101, "Computer Programming 2", 3, true, true, false, true, "DMSE", 30);
        Course course3 = new Course(210, "Project Management", 3, false, true, false, true, "DMSE", 30);
        Course course4 = new Course(300, "Data Structures & Algorithms", 3, false, true, false, true, "DMSE", 30);
        Course course5 = new Course(302, "Quality Assurance", 3, true, true, false, true, "DMSE", 30);

        Instructor instructor1 = new Instructor("Rick Baker", 001);
        Instructor instructor2 = new Instructor("Mohamed Abusharkh", 002);

        InstructorPreference preference1 = new InstructorPreference(1001, 001, 100, new TimeSpan( 3, 4, 1, 43), new DateTime(2019, 4, 6, 2, 3, 4));


        Room bigRoom = new Room("room1", true, true);
        Room smallRoom = new Room("room2", false, false);

    }
}
