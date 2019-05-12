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

        Instructor instructor1 = new Instructor("Rick Baker", 001, 2);
        Instructor instructor2 = new Instructor("Mohamed Abusharkh", 002, 3);

        InstructorPreference preference1 = new InstructorPreference(1001, 001, 100, new TimeSpan(3, 4, 1, 43), new DateTime(2019, 4, 6, 2, 3, 4));

        Room bigRoom = new Room("ATC160", true, true);
        Room smallRoom1 = new Room("ATC158", false, false);
        Room smallRoom2 = new Room("ATC176", false, false);
        Room smallRoom3 = new Room("ATC166", false, false);
        Room smallRoom4 = new Room("ATC142", false, false);

        


        public List<Course> courseList = new List<Course>();
        public List<Instructor> instructorList = new List<Instructor>();
        public List<Room> roomList = new List<Room>();


        public Scheduler()
        {
            courseList.Add(course1);
            courseList.Add(course2);
            courseList.Add(course3);
            courseList.Add(course4);
            courseList.Add(course5);
            instructorList.Add(instructor1);
            instructorList.Add(instructor2);
            roomList.Add(bigRoom);
            roomList.Add(smallRoom1);
            roomList.Add(smallRoom2);
            roomList.Add(smallRoom3);
            roomList.Add(smallRoom4);

            //Initialize the rooms availability
            foreach (Room room in roomList)
            {
                room.initializeRoomAvailablity(8, 20);
            }
        }


        public void printSchedule(Schedule schedule)
        {
            for (int i = 0; i < schedule.possibleCourses.Count; i++)
            {
                Console.WriteLine(schedule.possibleCourses[i].course.Name);
                Console.WriteLine(schedule.possibleCourses[i].instructor.name);
                Console.WriteLine(schedule.possibleCourses[i].startTime);
                Console.WriteLine(schedule.possibleCourses[i].endTime);
                Console.WriteLine(schedule.possibleCourses[i].room.roomID);
                Console.WriteLine(schedule.possibleCourses[i].datesOffered);
                Console.WriteLine();
            }
            Console.WriteLine("Valid schedule:");
            Console.WriteLine(isValidSchedule(schedule));
            Console.WriteLine();
        }

        public List<Course> randomizeCourses(List<Course> courses)
        {
            List<Course> temp = new List<Course>(courses);
            List<Course> newCourseList = new List<Course>();
            Random rnd = new Random();
            int randomNum = rnd.Next(temp.Count);
            while (temp.Count > 0)
            {

                if (temp[randomNum] != null)
                {
                    newCourseList.Add(temp[randomNum]);
                    temp.RemoveAt(randomNum);
                    randomNum = rnd.Next(temp.Count);
                }
                else
                {
                    randomNum = rnd.Next(courses.Count);
                }
            }

            return newCourseList;
        }

        public Schedule generateScheduleLvl1(List<Course> courseList, List<Instructor> instructorList, List<Room> roomList)
        {
            courseList = randomizeCourses(courseList);
            List<PossibleCourse> possibleCoursesList = new List<PossibleCourse>();
            Room possibleRoom = null;
            int c = 0;
            //For loop through instuctors and assign as many courses until limit of courses per instructor is met
            //Given the room is available and times are not conflicting

            //1.For each instructor
            for (int i = 0; i < instructorList.Count; i++)
            {
                int startTime = 8;
                int endTime = 10;
                string weekDays = "MW";
                //Counter to compare to a instructors maximum classes they can teach
                int maxCoursesCounter = 0;
                //2.Look through all the courses and the professor isn't maxed
                for (; c < courseList.Count && maxCoursesCounter < instructorList[i].maxCourses; c++)
                {
                    bool keepGoing = true;
                    //4. If the room is available
                    for (int r = 0; r < roomList.Count && keepGoing; r++)
                    {
                        if (courseList[c].NeedsLabRoom && roomList[r].isLab)
                        {
                            if (roomList[r].roomAvailbility[weekDays + startTime])
                            {
                                roomList[r].roomAvailbility[weekDays + startTime] = false;
                                possibleRoom = roomList[r];
                                keepGoing = false;
                            }
                        }
                        else if (courseList[c].NeedsLargeRoom && roomList[r].isLarge)
                        {
                            if (roomList[r].roomAvailbility[weekDays + startTime])
                            {
                                roomList[r].roomAvailbility[weekDays + startTime] = false;
                                possibleRoom = roomList[r];
                                keepGoing = false;
                            }
                        }
                        else
                        {
                            if (roomList[r].roomAvailbility[weekDays + startTime])
                            {
                                roomList[r].roomAvailbility[weekDays + startTime] = false;
                                possibleRoom = roomList[r];
                                keepGoing = false;
                            }
                        }
                    }
                    //5. Assign a time
                    //
                    //6. Add the course
                    //Fix the possible room != null
                    if (possibleRoom != null)
                    {
                        PossibleCourse possibleCourse = new PossibleCourse(courseList[c],
                            instructorList[i], startTime, endTime, weekDays, possibleRoom);
                        possibleCoursesList.Add(possibleCourse);
                        //Remove the course from the list to prevent multiple 
                        //instructors being assigned the same course
                        //courseList.RemoveAt(c);
                        maxCoursesCounter++;
                    }
                    startTime += 2;
                    endTime += 2;
                    if (endTime == 16)
                    {
                        weekDays = "TTH";
                        startTime = 8;
                        endTime = 10;
                    }

                }
            }
            Schedule schedule = new Schedule(possibleCoursesList, 100);

            return schedule;

        }

        //TODO: Needs rework for validating the schedule
        /// <summary>
        /// Logic for generating a new level 1 schedule
        /// </summary>
        /// <returns>True if the schedule is valid</returns>
        public bool isValidSchedule(Schedule unvalidatedSchedule)
        {
            bool isValid = true;

            //Iterate through each course
            for (int i = 0; i < unvalidatedSchedule.possibleCourses.Count; i++)
            {
                //Check the course with each other course
                for (int j = 0; j < unvalidatedSchedule.possibleCourses.Count; j++)
                {

                    //FIRST: If it is not the same course
                    if (unvalidatedSchedule.possibleCourses[i] != unvalidatedSchedule.possibleCourses[j])
                    {
                        //SECOND: If the times and days conflicting check the other factors
                        if (isTimeOk(unvalidatedSchedule.possibleCourses[i].startTime, unvalidatedSchedule.possibleCourses[i].endTime,
                            unvalidatedSchedule.possibleCourses[j].startTime, unvalidatedSchedule.possibleCourses[j].endTime) == false)
                        {
                            // THIRD: Check that there are no time conflicts with same level classes
                            //If the course levels are the same
                            if (unvalidatedSchedule.possibleCourses[i].course.getCourseLevel() == unvalidatedSchedule.possibleCourses[j].course.getCourseLevel())
                            {
                                //Remove the conflicted course from the list
                                isValid = false;
                            }
                            // FOURTH: Check that there are no conflicts with professors
                            //If this course shares the same professor
                            if (unvalidatedSchedule.possibleCourses[i].instructor == unvalidatedSchedule.possibleCourses[j].instructor)
                            {
                                //Remove the conflicted course from the list
                                isValid = false;
                            }
                            // FIFTH: Check that there are no conflicts for rooms
                            //If the courses share the same room
                            if (unvalidatedSchedule.possibleCourses[i].room == unvalidatedSchedule.possibleCourses[j].room)
                            {
                                isValid = false;
                            }

                        }
                    }

                }
            }
            return isValid;
        }

        /// <summary>
        /// Method to test for time conflicts between two classes
        /// </summary>
        /// <param name="classAStart">Start time of class A</param>
        /// <param name="classAEnd">End time of class A</param>
        /// <param name="classBStart">Start time of class B</param>
        /// <param name="classBEnd">End time of class B</param>
        /// <returns>True if there are no conflicts</returns>
        public bool isTimeOk(int classAStart, int classAEnd, int classBStart, int classBEnd)
        {
            bool isOk = true;

            if (classAStart == classBStart && classAEnd == classBEnd)
            {
                isOk = false;
            }
            else if (classAStart <= classBStart && classAEnd <= classBEnd && classBStart < classAEnd)
            {
                isOk = false;
            }
            else if (classAStart >= classBStart && classAEnd >= classBEnd && classAStart < classBEnd)
            {
                isOk = false;
            }
            return isOk;
        }

        public List<Schedule> getTwentySchedules(List<Course> courseList, List<Instructor> instructorList, List<Room> roomList)
        {
            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < 20; i++)
            {
                schedules.Add(generateScheduleLvl1(courseList, instructorList, roomList));
            }

            return schedules;
        }

    }
}
