using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;


namespace todo;

/// Logika pro hlavní okno aplikace
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var db = new Database();
        try
        {
            db.CreateDatabase(); // Zavolání metody pro vytvoření databáze a tabulky
            RenderListView(); // Načtení úkolů do seznamu
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


    ///Zobrazení pouze zadaných úkolů (např. Todo / Done)
    private void RenderListView(List<Task>? tasks = null)
    {
        
            if (PrimaryList == null)
            {
                return;
            }

            if (tasks is null)
                RenderListView();// pokud nejsou zadané, zobrazí všechny

        PrimaryList.ItemsSource = tasks;
    }


    /// Přidání nového úkolu po kliknutí na tlačítko
    private void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        var content = AddInput.Text.Trim();
        // Kontrola prázdného vstupu
        if (string.IsNullOrWhiteSpace(content))
        {
            MessageBox.Show("Input field cannot be empty.");
            return;
        }

        // Získání data ze vstupního pole
        DateTime? dueDate = DueDatePicker.SelectedDate;

        // Získání hodin a minut z ComboBoxů
        if (HourComboBox.SelectedItem is ComboBoxItem hourItem && MinuteComboBox.SelectedItem is ComboBoxItem minuteItem)
        {
            int hour = int.Parse(hourItem.Content.ToString());
            int minute = int.Parse(minuteItem.Content.ToString());

            if (dueDate.HasValue)
            {   // Složení celého datumu i s časem
                dueDate = new DateTime(dueDate.Value.Year, dueDate.Value.Month, dueDate.Value.Day,hour, minute, 0);
            }
        }

        // Kontrola, zda datum není v minulosti
        if (dueDate.HasValue && dueDate.Value < DateTime.Now)
        {
            MessageBox.Show("Due date cannot be in the past.");
            return;
        }


        DataAccess.AddTask(content, dueDate);

        RenderListView();// Obnovení seznamu
        AddInput.Clear();// Vymazání vstupního pole
    }

    /// Dvojklik na položku v ListView – odstranění úkolu

    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {

        if (sender is ListViewItem item)
        {
            if (item.Content is Task task)
            {
                var id = task.Id;

                DataAccess.DeleteTask(id);// Smazání úkolu z DB
            }
        }

        RenderListView();// Aktualizace seznamu
    }

    /// Změna filtru v ComboBoxu (All, Todo, Done)

    private void Show_Event(object sender, RoutedEventArgs e)
    {
        var selectedState = (ShowComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        if (selectedState == "All")
        {
            RenderListView();// Zobrazit vše
        }
        else if(selectedState == "Todo")
        {
            var tasks = DataAccess.GetTasks().Where(t => t.State == selectedState).ToList();
            RenderListView(tasks);
        }
        else if (selectedState == "Done")
        {   // Získání jen úkolů s daným stavem
            var tasks = DataAccess.GetTasks().Where(t => t.State == selectedState).ToList();
            RenderListView(tasks);
        }
    }

    /// Označení úkolu jako "Todo"
    private void MarkAsTodo_Click(object sender, RoutedEventArgs e)
    {
        if (PrimaryList.SelectedItem is Task selectedTask)
        {
            selectedTask.State = "Todo";
            DataAccess.UpdateTaskState(selectedTask.Id, "Todo");
            RenderListView();
        }
    }

    /// Označení úkolu jako "Done"
    private void MarkAsDone_Click(object sender, RoutedEventArgs e)
    {
        if (PrimaryList.SelectedItem is Task selectedTask)
        {
            selectedTask.State = "Done";
            DataAccess.UpdateTaskState(selectedTask.Id, "Done");
            RenderListView();
        }
    }


    /// Smazání všech úkolů, které mají stav "Done"

    private void DeleteAllBtn_Click(object sender, RoutedEventArgs e)
    {
        // Potvrdenie pred vymazaním
        var result = MessageBox.Show("Are you sure you want to delete all done tasks?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            DataAccess.DeleteAllDoneTasks();// Smazání z DB
            RenderListView(); // Obnoviť zobrazenie zoznamu
            MessageBox.Show("All done tasks have been deleted!");
        }
    }

    /// Dynamické přizpůsobení šířky sloupců při změně velikosti okna

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

            // Dynamická šířka pro sloupec s obsahem úkolu
            var remainingWidth = totalWidth - gridView.Columns[0].Width - gridView.Columns[2].Width - gridView.Columns[3].Width- gridView.Columns[4].Width;
            gridView.Columns[1].Width = remainingWidth;
        }
    }

    /// Zobrazení obsahu úkolu v novém okně

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