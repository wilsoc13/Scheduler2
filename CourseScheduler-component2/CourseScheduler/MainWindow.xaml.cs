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
            dataBaseHandler.FillAdaptersWithDataSet();
            DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_StudentsTableAdapter.GetData();

            GrdReport.ItemsSource = dataBaseHandler.NoRelation_SchedulesTableAdapter.GetData();


        }

        private void UpdateDatabase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataBaseHandler.FillAdaptersWithDataSet();
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
                        dataBaseHandler.InsertNewInstructor(values[0]);
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
            switch (GetDbTableItem())
            {
                case "Combinations":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_CombinationsTableAdapter.GetData();
                    break;
                case "CourseCombinations":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_CourseCombinationsTableAdapter.GetData();
                    break;
                case "CourseEnrollments":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_CourseEnrollmentsTableAdapter.GetData();
                    break;
                case "Courses":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_CoursesTableAdapter.GetData();
                    break;
                case "InstructorPreferences":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_InstructorPreferencesTableAdapter.GetData();
                    break;
                case "Instructors":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_InstructorsTableAdapter.GetData();
                    break;
                case "Join_Schedules_PossibleCourses":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_Join_Schedules_PossibleCoursesTableAdapter.GetData();
                    break;
                case "PossibleCourses":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_PossibleCoursesTableAdapter.GetData();
                    break;
                case "Rooms":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_RoomsTableAdapter.GetData();
                    break;
                case "Schedules":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_SchedulesTableAdapter.GetData();
                    break;
                case "Students":
                    DataGrid_DbTable.ItemsSource = dataBaseHandler.NoRelation_StudentsTableAdapter.GetData();
                    break;
                default:
                    break;
            }
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
            grpControls.Visibility = Visibility.Hidden;

            GrdReport.ItemsSource = dataBaseHandler.NoRelation_SchedulesTableAdapter.GetData();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            SwapToDetailed();
            grpControls.Visibility = Visibility.Visible;
        }

        private void SwapToDetailed()
        {
            DataTable tbl;

            tbl = dataBaseHandler.GetPossibleCourses(1);

            List<int> ids = new List<int>();

            foreach (DataRow row in tbl.Rows)
            {
                ids.Add(Convert.ToInt32(row[1]));
            }

            var results = from myRow in tbl.AsEnumerable()
                          where ids.Contains(myRow.Field<int>("PossibleCourseID"))
                          select myRow;

            GrdReport.ItemsSource = results;
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
                courses.Add(new Course(row.CourseID, row.Name, row.Credits, row.NeedsLabRoom, row.NeedsLargeRoom, row.IsElective, row.Hasrerequisite, row.Major, row.Capacity));
            }

            return courses;
        }

        private List<Instructor> GetInstructors()
        {
            List<Instructor> instructors = new List<Instructor>();
            foreach (var row in dataBaseHandler.DataSet.Instructors)
            {
                instructors.Add(new Instructor(row.Name, row.InstructorID));
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
                possibleCourses.Add(new PossibleCourse(row.CourseID, row.InstructorID, row.TimeOffered, row.DateOffered, 42, row.RoomID));
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
                Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", c.courseID, c.name, c.credits, c.needsLabRoom, c.needsLargeRoom, c.isElective, c.hasPrerequisite, c.major, c.capacity);
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
    }
}

