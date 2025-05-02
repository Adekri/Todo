using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;


namespace todo;

/// Interaction logic for MainWindow.xaml
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var db = new Database();
        try
        {
            db.CreateDatabase(); // Zavolání metody pro vytvoření databáze a tabulky
            RenderListView();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }

    private void RenderListView()
    {
        if (PrimaryList == null)
        {
            return;
        }
        PrimaryList.ItemsSource = DataAccess.GetTasks();
    }


    // toto se využívá pro zobrazení pouze Todo nebo Done
    private void RenderListView(List<Task>? tasks = null)
    {
        
            if (PrimaryList == null)
            {
                return;
            }

            if (tasks is null)
                RenderListView();

            PrimaryList.ItemsSource = tasks;
    }



    private void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        // Task content
        var content = AddInput.Text.Trim();
        // Handle empty input
        if (string.IsNullOrWhiteSpace(content))
        {
            MessageBox.Show("Input field cannot be empty.");
            return;
        }

        // Get the due date from the DatePicker
        DateTime? dueDate = DueDatePicker.SelectedDate;

        // Get the selected hour and minute
        if (HourComboBox.SelectedItem is ComboBoxItem hourItem && MinuteComboBox.SelectedItem is ComboBoxItem minuteItem)
        {
            int hour = int.Parse(hourItem.Content.ToString());
            int minute = int.Parse(minuteItem.Content.ToString());

            // Combine date and time
            if (dueDate.HasValue)
            {
                dueDate = new DateTime(dueDate.Value.Year, dueDate.Value.Month, dueDate.Value.Day,hour, minute, 0);
            }
        }

        // Check if the due date is in the past
        if (dueDate.HasValue && dueDate.Value < DateTime.Now)
        {
            MessageBox.Show("Due date cannot be in the past.");
            return;
        }


        // Add Task to tasks table in database
        DataAccess.AddTask(content, dueDate);

        RenderListView();
        AddInput.Clear();
    }

    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {

        if (sender is ListViewItem item)
        {
            if (item.Content is Task task)
            {
                var id = task.Id;

                DataAccess.DeleteTask(id);
            }
        }

        RenderListView();
    }

    private void Show_Event(object sender, RoutedEventArgs e)
    {
        var selectedState = (ShowComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        if (selectedState == "All")
        {
            RenderListView();
        }
        else if(selectedState == "Todo")
        {
            var tasks = DataAccess.GetTasks().Where(t => t.State == selectedState).ToList();
            RenderListView(tasks);
        }
        else if (selectedState == "Done")
        {
            var tasks = DataAccess.GetTasks().Where(t => t.State == selectedState).ToList();
            RenderListView(tasks);
        }
    }

    private void MarkAsTodo_Click(object sender, RoutedEventArgs e)
    {
        if (PrimaryList.SelectedItem is Task selectedTask)
        {
            selectedTask.State = "Todo";
            DataAccess.UpdateTaskState(selectedTask.Id, "Todo");
            RenderListView();
        }
    }

    private void MarkAsDone_Click(object sender, RoutedEventArgs e)
    {
        if (PrimaryList.SelectedItem is Task selectedTask)
        {
            selectedTask.State = "Done";
            DataAccess.UpdateTaskState(selectedTask.Id, "Done");
            RenderListView();
        }
    }



    private void DeleteAllBtn_Click(object sender, RoutedEventArgs e)
    {
        // Potvrdenie pred vymazaním
        var result = MessageBox.Show("Are you sure you want to delete all done tasks?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            DataAccess.DeleteAllDoneTasks();
            RenderListView(); // Obnoviť zobrazenie zoznamu
            MessageBox.Show("All done tasks have been deleted!");
        }
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (PrimaryList.View is GridView gridView)
        {
            // Šířka celého ListView
            var totalWidth = PrimaryList.ActualWidth -27;


            // Nastavení pevné šířky pro ostatní sloupce
            gridView.Columns[0].Width = 35;
            gridView.Columns[3].Width = 170;
            gridView.Columns[2].Width = 170;
            gridView.Columns[4].Width = 70;

            // Zbývající šířka pro sloupec Task
            var remainingWidth = totalWidth - gridView.Columns[0].Width - gridView.Columns[2].Width - gridView.Columns[3].Width- gridView.Columns[4].Width;
            gridView.Columns[1].Width = remainingWidth;
        }
    }

    private void ShowTaskDetail_Click(object sender, RoutedEventArgs e)
    {
        if (PrimaryList.SelectedItem is Task selectedTask)
        {
            // Vytvoření nového okna
            var detailWindow = new TaskDetailWindow();

            // Nastavení textu úkolu
            detailWindow.SetTaskContent(selectedTask.Content ?? "Žádný obsah");

            // Zobrazení okna
            detailWindow.Show();
        }
        else
        {
            MessageBox.Show("Choose task, which you would like to see in new window.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }



}