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

namespace todo;

/// Interaction logic for MainWindow.xaml
public partial class MainWindow : Window
{
    public MainWindow()
    {
        try
        {
            InitializeComponent();
            var db = new Database();
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
        PrimaryList.ItemsSource = DataAccess.GetTasks();
    }

    private void RenderTodoListView()
    {
        PrimaryList.ItemsSource = DataAccess.GetTodoTasks();
    }

    private void RenderListView(List<Task>? tasks = null)
    {
        
            if (PrimaryList == null)
            {
                // Handle the case where PrimaryList is not initialized
                return;
            }

            if (tasks is null)
                RenderListView();

            PrimaryList.ItemsSource = tasks;
    }


    /// Clear all input form of MainWindow
    private void ClearForm()
    {
        //sem se pak přidá základní nastavení formuláře
        AddInput.Clear();

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


        ClearForm();

        // Add Task to tasks table in database
        DataAccess.AddTask(content);

        RenderListView();
    }

    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // Converting sender from object to Task
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
        var selectedState = ShowComboBox.SelectedItem.ToString();

        if (selectedState == "All")
        {
            RenderListView();
        }
        else if(selectedState == "Todo")
        {
            RenderTodoListView();
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

}