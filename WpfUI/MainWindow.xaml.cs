using Engine;
using Engine.Events;

using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Utils;

namespace WpfUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            Location.OnChange.AddEvent(new() {
                Action = (object? _, Location.OnChangeEventArgs e) => {
                    ChangeMainText(e.NewLocation.Body);
                    ShowLocationActs(e.NewLocation);
                },
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });
        }

        private void Update() {
            Game.UI.Update();
        }

        private void actButton_Click(object sender, RoutedEventArgs e) {
            Button button = (Button)sender;
            int index = (int)button.Tag;
            Location.Current.UseAct(index);
            button.IsEnabled = Location.Current.Acts[index].IsActive;
        }

        public void ChangeMainText(string text) {
            MainText.Text = text;
        }

        public void AddMainText(string text) {
            ChangeMainText(MainText.Text + text);
        }

        public void ShowLocationActs(Location location) {
            ActsStack.ItemsSource = location.Acts.Enumerate();
        }
    }

    public class Wpf : Engine.UI.UI {
        private readonly App app = new();
        private readonly MainWindow window = new();

        private readonly Queue<string> notifications = [];

        public override void ShowLocation(Location location) {

        }

        public override void Notify(string str) {
            notifications.Enqueue(str);
        }

        public override void ShowScene(Scene scene) {

        }

        public override void Update() {
            foreach (var notification in notifications) {
                window.AddMainText(notification);
            }
        }

        public override void Run() {
            window.ChangeMainText(Location.Current.Body);
            window.ShowLocationActs(Location.Current);
            app.Run(window);
        }
    }
}