using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataBaseHandler dataBaseHandler;
        private int scheduleID;

        public MainWindow()
        {
            InitializeComponent();
            dataBaseHandler = new DataBaseHandler();
        }

        /// <summary>
        /// Logic to be executed upon application's opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Students;

            GrdReport.ItemsSource = dataBaseHandler.SchedulesTableAdapter.GetData();


        }

        private void UpdateDatabase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataBaseHandler.TableAdapterManagerUpdateAll();
                MessageBox.Show("Database Updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            courses.Content = "File Location";
            courses_Loaded.Visibility = Visibility.Hidden;

            string courseLocation = Load_CSV();
            courses.Content = courseLocation;

            if (courses.Content.ToString() != "File Location")
            {
                courses_Loaded.Visibility = Visibility.Visible;
            }
        }

        public string Load_CSV()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Read_CSV(filename);
                return filename;
            }
            return "File Location";
        }

        public void Read_CSV(string fileLocation)
        {
            using (var reader = new StreamReader(fileLocation))
            {
                //Add File To DataBase
                if (fileLocation.Contains("Courses"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        // String, Int, Bool, Bool, Bool, Bool, String, Int
                        dataBaseHandler.InsertNewCourse(values[0], Convert.ToInt32(values[1]), Convert.ToBoolean(Convert.ToInt32(values[2])),
                                                        Convert.ToBoolean(Convert.ToInt32(values[3])), Convert.ToBoolean(Convert.ToInt32(values[4])),
                                                        Convert.ToBoolean(Convert.ToInt32(values[5])), values[6], Convert.ToInt32(values[7]));
                    }
                }

                if (fileLocation.Contains("Instructors"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        dataBaseHandler.InsertNewInstructor(values[0], Convert.ToInt32(values[2]));
                    }
                }

                if (fileLocation.Contains("Students"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        dataBaseHandler.InsertNewStudent(values[0], values[1], values[2], values[3], Convert.ToInt32(values[4]));
                    }
                }

                if (fileLocation.Contains("Rooms"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        dataBaseHandler.InsertNewRoom(values[0], Convert.ToBoolean(Convert.ToInt32(values[1])), Convert.ToBoolean(Convert.ToInt32(values[2])));
                    }
                }
            }
        }

        private string GetDbTableItem()
        {
            return TableSelector.SelectionBoxItem.ToString();
        }

        private void UpdateTable()
        {
            if (GetDbTableItem() == "Courses" || GetDbTableItem() == "CourseEnrollments" || GetDbTableItem() == "Students" || GetDbTableItem() == "Combinations")
                AddNewItem.Visibility = Visibility.Visible;
            else
                AddNewItem.Visibility = Visibility.Hidden;

            switch (GetDbTableItem())
            {
                case "Combinations":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Combinations;
                    break;
                case "CourseCombinations":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.CourseCombinations;
                    break;
                case "CourseEnrollments":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.CourseCombinations;
                    break;
                case "Courses":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Courses;
                    break;
                case "InstructorPreferences":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.InstructorPreferences;
                    break;
                case "Instructors":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Instructors;
                    break;
                case "Join_Schedules_PossibleCourses":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Join_Schedules_PossibleCourses;
                    break;
                case "PossibleCourses":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.PossibleCourses;
                    break;
                case "Rooms":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Rooms;
                    break;
                case "Schedules":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Schedules;
                    break;
                case "Students":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.DataSet.Students;
                    break;
                default:
                    break;
            }

            foreach (DataGridTextColumn col in DataGrid_DbTable.Columns.Where(c => c.Header.ToString().Contains("FK")))
                col.Visibility = Visibility.Hidden;
        }

        private void TableSelector_DropDownClosed(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            DbItemCreationWindow dbItemCreationWindow = new DbItemCreationWindow(GetDbTableItem(), dataBaseHandler);
            dbItemCreationWindow.ShowDialog();
            UpdateTable();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            grpControls.Visibility = Visibility.Collapsed;

            GrdReport.ItemsSource = dataBaseHandler.NoRelation_SchedulesTableAdapter.GetData();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grpControls.Visibility == Visibility.Visible)
                return;
            DataRowView row = (DataRowView)GrdReport.SelectedItem;
            if (row != null)
            {
                scheduleID = (int)row[0];
                SwapToDetailed();
                grpControls.Visibility = Visibility.Visible;
            }
        }

        private void SwapToDetailed()
        {
            GrdReport.ItemsSource = dataBaseHandler.GetPossibleCourses(scheduleID).AsDataView();
        }

        private void SwapToWeekly()
        {
            Dictionary<string, int> days = new Dictionary<string, int>();
            days.Add("M", 1);
            days.Add("T", 2);
            days.Add("W", 3);
            days.Add("H", 4);
            days.Add("F", 5);

            DataTable weeklyTable = new DataTable();

            DataColumn time = new DataColumn(Name = "Time");
            DataColumn monday = new DataColumn(Name = "Monday");
            DataColumn tuesday = new DataColumn(Name = "Tuesday");
            DataColumn wednesday = new DataColumn(Name = "Wednesday");
            DataColumn thursday = new DataColumn(Name = "Thursday");
            DataColumn friday = new DataColumn(Name = "Friday");


            weeklyTable.Columns.Add(time);
            weeklyTable.Columns.Add(monday);
            weeklyTable.Columns.Add(tuesday);
            weeklyTable.Columns.Add(wednesday);
            weeklyTable.Columns.Add(thursday);
            weeklyTable.Columns.Add(friday);


            DataRow eight = weeklyTable.NewRow();
            eight["Time"] = 8;
            weeklyTable.Rows.Add(eight);

            DataRow nine = weeklyTable.NewRow();
            nine["Time"] = 9;
            weeklyTable.Rows.Add(nine);

            DataRow ten = weeklyTable.NewRow();
            ten["Time"] = 10;
            weeklyTable.Rows.Add(ten);

            DataRow eleven = weeklyTable.NewRow();
            eleven["Time"] = 11;
            weeklyTable.Rows.Add(eleven);

            DataRow twelve = weeklyTable.NewRow();
            twelve["Time"] = 12;
            weeklyTable.Rows.Add(twelve);

            DataRow thirteen = weeklyTable.NewRow();
            thirteen["Time"] = 13;
            weeklyTable.Rows.Add(thirteen);

            DataRow fourteen = weeklyTable.NewRow();
            fourteen["Time"] = 14;
            weeklyTable.Rows.Add(fourteen);

            DataRow fifteen = weeklyTable.NewRow();
            fifteen["Time"] = 15;
            weeklyTable.Rows.Add(fifteen);

            DataRow sixteen = weeklyTable.NewRow();
            sixteen["Time"] = 16;
            weeklyTable.Rows.Add(sixteen);

            DataRow seventeen = weeklyTable.NewRow();
            seventeen["Time"] = 17;
            weeklyTable.Rows.Add(seventeen);

            DataRow eighteen = weeklyTable.NewRow();
            eighteen["Time"] = 18;
            weeklyTable.Rows.Add(eighteen);

            DataTable dataTbl = dataBaseHandler.GetPossibleCourses(scheduleID);

            foreach(DataRow row in dataTbl.Rows)
            {
                string daysString = row["DateOffered"].ToString();
                while (daysString.Length > 0)
                {
                    string day = daysString.Substring(daysString.Length-1, 1);
                    daysString = daysString.Remove(daysString.Length-1, 1);

                    for (int i = (int)row["TimeStart"] - 8; i < (int)row["TimeEnd"] - 8; i++)
                    {
                        weeklyTable.Rows[i][days[day]] = row["Name"].ToString();
                    }
                }

            }

            GrdReport.ItemsSource = weeklyTable.AsDataView();
        }

        private void BtnShowWeekly_Click(object sender, RoutedEventArgs e)
        {
            SwapToWeekly();
            btnShowOverview.Visibility = Visibility.Visible;
            btnShowWeekly.Visibility = Visibility.Hidden;
        }

        private void BtnShowOverview_Click(object sender, RoutedEventArgs e)
        {
            SwapToDetailed();
            btnShowOverview.Visibility = Visibility.Hidden;
            btnShowWeekly.Visibility = Visibility.Visible;
        }

        // start of logic for saving to database
        //private void setCourses()
        //{
        //    dataBaseHandler.DataSet.Courses.AddCoursesRow(schedulerComponentTwoDataSet.CoursesRow);
        //    TableAdapterManager.UpdateAll(DataSet);
        //}

        // Below: pulling from database into objects
        private List<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();
            foreach (var row in dataBaseHandler.DataSet.Courses)
            {
                Course course = new Course(row.CourseID, row.Name, row.Credits, row.NeedsLabRoom, row.NeedsLargeRoom,row.IsElective,row.Hasrerequisite,row.Major,row.Capacity);
                //How to go about storing class level?
                courses.Add(course);
            }

            return courses;
        }

        private List<Instructor> GetInstructors()
        {
            List<Instructor> instructors = new List<Instructor>();
            foreach (var row in dataBaseHandler.DataSet.Instructors)
            {
                //Add instructor max courses column
                instructors.Add(new Instructor(row.Name, row.InstructorID,row.maxClasses));
            }

            return instructors;
        }


        private List<InstructorPreference> GetInstructorPreferences()
        {
            List<InstructorPreference> instructorPreferences = new List<InstructorPreference>();
            foreach (var row in dataBaseHandler.DataSet.InstructorPreferences)
            {
                instructorPreferences.Add(new InstructorPreference(row.PreferenceID, row.InstructorID, row.CourseID, row.TimeOffered, row.DateOffered));
            }

            return instructorPreferences;
        }

        private List<PossibleCourse> GetPossibleCourses()
        {
            List<PossibleCourse> possibleCourses = new List<PossibleCourse>();
            foreach (var row in dataBaseHandler.DataSet.PossibleCourses)
            {
                Course course = new Course(dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).CourseID,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).Name,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).Credits,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).NeedsLabRoom,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).NeedsLargeRoom,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).IsElective,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).Hasrerequisite,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).Major,
                                           dataBaseHandler.DataSet.Courses.FindByCourseID(row.CourseID).Capacity);
                Instructor instructor = new Instructor(dataBaseHandler.DataSet.Instructors.FindByInstructorID(row.InstructorID).Name,
                                           dataBaseHandler.DataSet.Instructors.FindByInstructorID(row.InstructorID).InstructorID,
                                           dataBaseHandler.DataSet.Instructors.FindByInstructorID(row.InstructorID).maxClasses);
                Room room = new Room(dataBaseHandler.DataSet.Rooms.FindByRoomID(row.RoomID).RoomID,
                                           dataBaseHandler.DataSet.Rooms.FindByRoomID(row.RoomID).IsLarge,
                                           dataBaseHandler.DataSet.Rooms.FindByRoomID(row.RoomID).IsLab);


                PossibleCourse possibleCourse = new PossibleCourse(course, instructor, row.TimeStart,row.TimeEnd, row.DateOffered, room);
                //Is possible courses table even needed?
                possibleCourses.Add(possibleCourse);
            }
            return possibleCourses;
        }

        private List<Room> GetRooms()
        {
            List<Room> rooms = new List<Room>();
            foreach (var row in dataBaseHandler.DataSet.Rooms)
            {
                rooms.Add(new Room(row.RoomID, row.IsLarge, row.IsLab));
            }

            return rooms;
        }

        private void doThings()
        {
            var rooms = GetRooms();
            var courses = GetCourses();
            var instructors = GetInstructors();
            var preferences = GetInstructorPreferences();

            foreach (Room r in rooms)
            {
                Console.WriteLine(r);
            }
            Console.WriteLine();
            foreach (Course c in courses)
            {
                Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", c.CourseID, c.Name, c.Credits, c.NeedsLabRoom, c.NeedsLargeRoom, c.IsElective, c.Hasrerequisite, c.Major, c.Capacity,c.getCourseLevel().ToString());
            }
            Console.WriteLine();
            foreach (Instructor i in instructors)
            {
                Console.WriteLine("{0}, {1}", i.name, i.instructorID);
            }
            Console.WriteLine();
            foreach (InstructorPreference p in preferences)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("objects finished");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            doThings();
        }

        /// <summary>
        /// Generates schedules, first from program database, then from example data in Scheduler class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Generate_Schedules(object sender, RoutedEventArgs e)
        {
            int score;
            List<Room> rooms = GetRooms();
            List<Instructor> instructors = GetInstructors();
            List<Course> courses = GetCourses();

            Scheduler scheduler = new Scheduler();
            //Schedule scheduleFromDataBase = s.generateScheduleLvl1(courses, s.instructorList, rooms);
            //s.printSchedule(scheduleFromDataBase);

            Schedule scheduleFromExampleData = scheduler.generateScheduleLvl1(scheduler.courseList, scheduler.instructorList, scheduler.roomList);
            //Console.WriteLine("First schedule: \n\n");
            //scheduler.printSchedule(scheduleFromExampleData);

            score = ScoreSchedule(scheduleFromExampleData);

            CourseSchedulerDBDataSet.InstructorsDataTable instructorTbl = new CourseSchedulerDBDataSet.InstructorsDataTable();
            CourseSchedulerDBDataSet.CoursesDataTable courseTbl = new CourseSchedulerDBDataSet.CoursesDataTable();
            CourseSchedulerDBDataSet.RoomsDataTable roomTbl = new CourseSchedulerDBDataSet.RoomsDataTable();
            dataBaseHandler.RoomsTableAdapter.Fill(roomTbl);
            dataBaseHandler.CoursesTableAdapter.Fill(courseTbl);
            dataBaseHandler.InstructorsTableAdapter.Fill(instructorTbl);
            dataBaseHandler.InsertNewSchedule(scheduleFromExampleData.scheduleID, scheduleFromExampleData.score);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[0], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[0], 8, 10, "MW", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[0]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[1], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[0], 12, 14, "TH", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[1]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[2], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[0], 16, 17, "MWF", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[2]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[3], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[1], 8, 10, "TF", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[3]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[4], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[1], 12, 14, "MTW", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[4]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[5], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[1], 16, 17, "HF", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[5]);
            CourseSchedulerDBDataSet.PossibleCoursesDataTable tbl = new CourseSchedulerDBDataSet.PossibleCoursesDataTable();
            dataBaseHandler.PossibleCoursesTableAdapter.Fill(tbl);
            CourseSchedulerDBDataSet.SchedulesDataTable scheduleTbl = new CourseSchedulerDBDataSet.SchedulesDataTable();
            dataBaseHandler.SchedulesTableAdapter.Fill(scheduleTbl);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[0], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[0]);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[0], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[1]);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[0], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[2]);

            GrdReport.ItemsSource = dataBaseHandler.SchedulesTableAdapter.GetData();

            foreach (var possibleCourse in scheduleFromExampleData.possibleCourses)
            {
                scheduleOutput.Text += "Course Name: " + possibleCourse.course.Name + "\n";
                scheduleOutput.Text += "Instructor Name: " + possibleCourse.instructor.name + "\n";
                scheduleOutput.Text += "Start Time: " + possibleCourse.startTime.ToString() + "\n";
                scheduleOutput.Text += "End Time: " + possibleCourse.endTime.ToString() + "\n";
                scheduleOutput.Text += "Room: " + possibleCourse.room.roomID + "\n";
                scheduleOutput.Text += "Days Offered: " + possibleCourse.datesOffered + "\n";
                scheduleOutput.Text += "\n";

            }
            scheduleOutput.Text += "score: " + score + "\n";

            //Console.WriteLine("\n Twenty other schedules: ");

            //var manySchedules = scheduler.getTwentySchedules(courses, scheduler.instructorList, scheduler.roomList);
            //manySchedules.ForEach(delegate (Schedule s)
            //{
            //    scheduler.printSchedule(s);
            //});

        }

        private int ScoreSchedule(Schedule schedule)
        {
            int score = 0;
            int count = 0;
            int score1 = 0;
            int score2 = 0;
            int stuMin = 3;
            int stuMax = 6;

            int numberOfCourses = schedule.possibleCourses.Count;

            if (numberOfCourses > stuMin)
            {
                score = stuMin * 10;
                count = stuMax - numberOfCourses;
                if(count >= 1)
                {
                    score1 = count * 5;
                }
                else
                {
                    score2 = ((stuMax - stuMin) * 5 + (Math.Abs(count) * 2));
                }
            }
            else
            {
                score = 0;
                score1 = 0;
                score2 = 0;
            }

            int totalScore = score1 + score2 + score;
            return totalScore;

        }

        private void BtnCreateScheduleInfo_Click(object sender, RoutedEventArgs e)
        {
            CourseSchedulerDBDataSet.InstructorsDataTable instructorTbl = new CourseSchedulerDBDataSet.InstructorsDataTable();

            CourseSchedulerDBDataSet.CoursesDataTable courseTbl = new CourseSchedulerDBDataSet.CoursesDataTable();

            CourseSchedulerDBDataSet.RoomsDataTable roomTbl = new CourseSchedulerDBDataSet.RoomsDataTable();

            dataBaseHandler.RoomsTableAdapter.Fill(roomTbl);

            dataBaseHandler.CoursesTableAdapter.Fill(courseTbl);

            dataBaseHandler.InstructorsTableAdapter.Fill(instructorTbl);

            dataBaseHandler.InsertNewSchedule(1, 1);
            dataBaseHandler.InsertNewSchedule(2, 2);


            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[0], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[0], 8, 10,"MW", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[0]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[1], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[0], 12, 14, "TH", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[1]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[2], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[0], 16, 17, "MWF", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[2]);

            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[3], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[1], 8, 10, "TF", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[3]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[4], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[1], 12, 14, "MTW", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[4]);
            dataBaseHandler.InsertNewPossibleCourse((CourseSchedulerDBDataSet.CoursesRow)courseTbl.Rows[5], (CourseSchedulerDBDataSet.InstructorsRow)instructorTbl.Rows[1], 16, 17, "HF", (CourseSchedulerDBDataSet.RoomsRow)roomTbl.Rows[5]);

            CourseSchedulerDBDataSet.PossibleCoursesDataTable tbl = new CourseSchedulerDBDataSet.PossibleCoursesDataTable();

            dataBaseHandler.PossibleCoursesTableAdapter.Fill(tbl);

            CourseSchedulerDBDataSet.SchedulesDataTable scheduleTbl = new CourseSchedulerDBDataSet.SchedulesDataTable();

            dataBaseHandler.SchedulesTableAdapter.Fill(scheduleTbl);

            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[0], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[0]);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[0], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[1]);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[0], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[2]);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[1], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[3]);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[1], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[4]);
            dataBaseHandler.InsertNewJoin_Schedules_PossibleCourses((CourseSchedulerDBDataSet.SchedulesRow)scheduleTbl.Rows[1], (CourseSchedulerDBDataSet.PossibleCoursesRow)tbl.Rows[5]);

        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            GrdReport.ItemsSource = dataBaseHandler.SchedulesTableAdapter.GetData();
        }


    }
}

