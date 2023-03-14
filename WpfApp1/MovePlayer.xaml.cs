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
        { InitializeComponent(); }
        public MovePlayer(Player prismarine, MazeLevel mazeLevel)
        {
            InitializeComponent();

            MainFunction(prismarine, mazeLevel);
        }

        public void MainFunction(Player prismarine, MazeLevel mazeLevel)
        {
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
                Text = "Row:",
                Width = 50
            };
            Canvas.SetTop(rowBlock, 40);
            Canvas.SetLeft(rowBlock, 5);
            mpCanvas.Children.Add(rowBlock);

            TextBox rowBox = new TextBox
            {
                Width = 50
            };
            Canvas.SetTop(rowBox, 40);
            Canvas.SetLeft(rowBox, 55);
            mpCanvas.Children.Add(rowBox);

            TextBlock colBlock = new TextBlock
            {
                Text = "Column:",
                Width = 50
            };
            Canvas.SetTop(colBlock, 70);
            Canvas.SetLeft(colBlock, 5);
            mpCanvas.Children.Add(colBlock);

            TextBox colBox = new TextBox
            {
                Width = 50
            };
            Canvas.SetTop(colBox, 70);
            Canvas.SetLeft(colBox, 55);
            mpCanvas.Children.Add(colBox);

            Button confirmButton = new Button
            {
                Content = "Confirm",
                Width = 50
            };
            Canvas.SetTop(confirmButton, 120);
            Canvas.SetLeft(confirmButton, 5);
            mpCanvas.Children.Add(confirmButton);
            confirmButton.Click += (sender, e) => ConfirmButton_Click(sender, e, prismarine, mazeLevel, rowBox, colBox);
        }

        private void ConfirmButton_Click(object sender, EventArgs e, Player prismarine, MazeLevel mazeLevel, TextBox rowBox, TextBox colBox)
        {
            int row = 0, col = 0;
            bool valid = true;

            if (rowBox.Text != "" && colBox.Text != "")
            {
                bool valid_row = int.TryParse(rowBox.Text, out row);
                if (!valid_row || row < 0 || row > 20)
                { 
                    valid = false; 
                    MessageBox.Show("Row value must be a whole number between 0 and 20", "Error"); 
                }

                bool valid_col = int.TryParse(colBox.Text, out col);
                if (!valid_col || col < 0 || col > 20)
                { 
                    valid = false; 
                    MessageBox.Show("Column value must be a whole number between 0 and 20", "Error"); 
                }
            }

            else valid = false;

            if (!valid) MessageBox.Show("You must enter a number into each box", "Error");

            else
            {
                (Application.Current.Windows[0] as MainWindow).MovePlayer(prismarine, mazeLevel, row, col);
                this.Close();
            }
        }
    }
}
