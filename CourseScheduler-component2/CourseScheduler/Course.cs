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
        public int CourseID;
        public string Name;
        public int Credits;
        public bool NeedsLabRoom;
        public bool NeedsLargeRoom;
        public bool IsElective;
        public bool Hasrerequisite;
        public string Major;
        public int Capacity;


        public Course(int courseID, string name, int credits, bool needsLabRoom, bool needsLargeRoom,
            bool isElective, bool hasPrerequisite, string major, int capacity)
        {
            this.CourseID = courseID;
            this.Name = name;
            this.Credits = credits;
            this.NeedsLabRoom = needsLabRoom;
            this.NeedsLargeRoom = needsLargeRoom;
            this.IsElective = isElective;
            this.Hasrerequisite = hasPrerequisite;
            this.Major = major;
            this.Capacity = capacity;
        }

        public int getCourseLevel()
        {
            Int32.TryParse(new String(Name.Where(char.IsDigit).ToArray()),out int level);

            while (level >= 10)
                level /= 10;

            return level;
        }
    }

  

    /// <summary>
    /// Possible course object, used for determining possible courses for schedule
    /// </summary>
    class PossibleCourse
    {
        public Course course;
        public Instructor instructor;
        public int startTime;
        public int endTime;
        public string datesOffered;
        public Room room;

        public PossibleCourse(Course course, Instructor instructor, int startTime,
            int endTime, string datesOffered, Room room)
        {
            this.course = course;
            this.instructor = instructor;
            this.startTime = startTime;
            this.endTime = endTime;
            this.datesOffered = datesOffered;
            this.room = room;
        }
    }

    /// <summary>
    /// Instructor object, teacher of a course
    /// </summary>
    class Instructor
    {
        public int instructorID;
        public string name;
        public int maxCourses;

        public Instructor(string name, int instructorID, int maxCourses)
        {
            this.instructorID = instructorID;
            this.name = name;
            this.maxCourses = maxCourses;
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
        public Dictionary<String, bool> roomAvailbility = new Dictionary<string, bool>();

        public Room(string roomID, bool isLarge, bool isLab)
        {
            this.roomID = roomID;
            this.isLarge = isLarge;
            this.isLab = isLab;
        }

        public void initializeRoomAvailablity(int startFirstLecture, int startLastLecture)
        {
            int start = startFirstLecture;
            string days = "MW";
            for (; startFirstLecture < startLastLecture; startFirstLecture += 2)
            {
                string key = days + startFirstLecture;
                roomAvailbility.Add(key, true);
            }
            days = "TTH";
            startFirstLecture = start;
            for (; startFirstLecture < startLastLecture; startFirstLecture += 2)
            {
                string key = days + startFirstLecture;
                roomAvailbility.Add(key, true);
            }
        }
    }

 

    /// <summary>
    /// Object to be generated, a list of courses and a score to determine the relative value of the schedule. 
    /// </summary>
    class Schedule
    {
        public List<PossibleCourse> possibleCourses;
        public int score;
        public int scheduleID;
        static int idCounter = 0;

        public Schedule(List<PossibleCourse> possibleCourses, int score)
        {
            this.possibleCourses = possibleCourses;
            this.score = score;
            setID();
        }

        void setID()
        {
            idCounter++;
            this.scheduleID = idCounter;
        }
    }
}
