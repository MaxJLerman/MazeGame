using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MovePlayer.xaml
    /// </summary>
    public partial class MovePlayer : Window
    {
        public MovePlayer()
        {
            InitializeComponent();

            Canvas mpCanvas = new Canvas
            {
                Height = 200,
                Width = 300,
                Background = Brushes.LightGray
            };
            MovePlayerWindow.Content = mpCanvas;

            TextBlock title = new TextBlock
            {
                Text = "Enter new co-ordinates:",
                Width = 140
            };
            Canvas.SetTop(title, 5);
            Canvas.SetLeft(title, 5);
            mpCanvas.Children.Add(title);

            TextBlock rowBlock = new TextBlock
            {

            };

            TextBox rowBox = new TextBox
            {
                
            };
            Canvas.SetTop(rowBox, 40);
            Canvas.SetLeft(rowBox, 5);
            mpCanvas.Children.Add(rowBox);

            TextBlock colBlock = new TextBlock
            {

            };

            TextBox colBox = new TextBox
            {

            };
            Canvas.SetTop(colBox, 70);
            Canvas.SetLeft(colBox, 5);
            mpCanvas.Children.Add(colBox);

            Button confirmButton = new Button
            {
                Content = "Confirm",
                Width = 50
            };
            Canvas.SetTop(confirmButton, 100);
            Canvas.SetLeft(confirmButton, 5);
            mpCanvas.Children.Add(confirmButton);
        }
    }
}
