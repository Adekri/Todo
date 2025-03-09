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

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        RenderListView();
    }

    private void RenderListView()
    {
        //PrimaryList.ItemsSource = Tasks = DataAccess.GetTasks();
    }

    private void RenderListView(List<Task>? tasks = null)
    {
        if (tasks is null)
            RenderListView();

        PrimaryList.ItemsSource = tasks;
    }

    /// <summary>
    /// Clear all input form of MainWindow
    /// </summary>
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
       // DataAccess.AddTask(content);

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

               // DataAccess.DeleteTask(id);
            }
        }

        RenderListView();
    }

    private void Show_Event(object sender, RoutedEventArgs e)
    {
        //sem se pak přidá akce na zobrazení těch správných
    }

}