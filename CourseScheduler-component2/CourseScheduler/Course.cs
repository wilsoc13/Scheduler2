using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseScheduler
{
    /// <summary>
    /// Course object, key component of course schedule with elements such as name, credits, and if it's an elective. 
    /// </summary>
    class Course
    {
        public int courseID;
        public string name;
        public int credits;
        public bool needsLabRoom;
        public bool needsLargeRoom;
        public bool isElective;
        public bool hasPrerequisite;
        public string major;
        public int capacity;

        public Course(int courseID, string name, int credits, bool needsLabRoom, bool needsLargeRoom,
            bool isElective, bool hasPrerequisite, string major, int capacity)
        {
            this.courseID = courseID;
            this.name = name;
            this.credits = credits;
            this.needsLabRoom = needsLabRoom;
            this.needsLargeRoom = needsLargeRoom;
            this.isElective = isElective;
            this.hasPrerequisite = hasPrerequisite;
            this.major = major;
            this.capacity = capacity;
        }
    }

    /// <summary>
    /// Possible course object, used for determining possible courses for schedule
    /// </summary>
    class PossibleCourse
    {
        public int courseID;
        public int instructorID;
        public TimeSpan timeOffered;
        public DateTime datesOffered;
        public int scheduleID;
        public string roomID;

        public PossibleCourse(int courseID, int instructorID, TimeSpan timeOffered,
            DateTime datesOffered, int scheduleID, string roomID)
        {
            this.courseID = courseID;
            this.instructorID = instructorID;
            this.timeOffered = timeOffered;
            this.datesOffered = datesOffered;
            this.scheduleID = scheduleID;
            this.roomID = roomID;
        }
    }

    /// <summary>
    /// Instructor object, teacher of a course
    /// </summary>
    class Instructor
    {
        public int instructorID;
        public string name;
        public Instructor(string name, int instructorID)
        {
            this.instructorID = instructorID;
            this.name = name;
        }
    }

    /// <summary>
    /// Instructor's preference for courses to teach
    /// </summary>
    class InstructorPreference
    {
        public int preferredID;
        public int instructorID;
        public int courseID;
        TimeSpan timeOffered;
        DateTime dateOffered;

        public InstructorPreference(int preferredID, int instructorID, int courseID,
            TimeSpan timeOffered, DateTime dateOffered)
        {
            this.preferredID = preferredID;
            this.instructorID = instructorID;
            this.courseID = courseID;
            this.timeOffered = timeOffered;
            this.dateOffered = dateOffered;
        }
    }

    /// <summary>
    /// Room object, information about room a course is held in
    /// </summary>
    class Room
    {
        public string roomID;
        public bool isLarge;
        public bool isLab;

        public Room(string roomID, bool isLarge, bool isLab)
        {
            this.roomID = roomID;
            this.isLarge = isLarge;
            this.isLab = isLab;
        }
    }

    /// <summary>
    /// Object to be generated, a list of courses and a score to determine the relative value of the schedule. 
    /// </summary>
    class Schedule
    {
        public List<Course> courseList;
        public int score;

        public Schedule(List<Course> courseList, int score)
        {
            this.courseList = courseList;
            this.score = score;
        }
    }

}
