using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class Player
    {
        private int y;
        private int x;
        private int moveCount;
        private char compass;

        public Player() { }

        public void SetY(int y) { this.y = y; }
        public int GetY() { return y; }
        public void SetX(int x) { this.x = x; }
        public int GetX() { return x; }
        public void SetMoveCount(int moveCount) { this.moveCount = moveCount; }
        public int GetMoveCount() { return moveCount; }
        public void SetCompass(char compass) { this.compass = compass; }
        public char GetCompass() { return compass; }

        public void SetYX(int y, int x, int moveCount)
        {
            this.y = y;
            this.x = x;
            this.moveCount = moveCount;
        }
    }

    public class TeleportSquare
    {
        private int TsY;
        private int TsX;

        public TeleportSquare() { }

        public int GetTsY() { return TsY; }
        public int GetTsX() { return TsX; }

        public void SetTsYX(int TsY, int TsX)
        {
            this.TsY = TsY;
            this.TsX = TsX;
        }
    }
    
    public partial class MainWindow : Window
    {
        // create a canvas to display the maze
        static Canvas mazeCanvas = new Canvas();

        // create 400 rectangle objects to be squares in the maze
        static Rectangle[,] mazeGrid = new Rectangle[21, 21];

        // create a 20x20 array of character objects to represent the maze in text form
        static char[,] mazeTextForm = new char[21, 21];

        // creates a player called prismarine ;)
        public static Player prismarine = new Player();
        public TextBlock currentMoveCount = new TextBlock();

        public string flag = "";

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(RootWindow_KeyDown);

            // setting up the base properties of the content canvas
            mazeCanvas.Height = 980; // 100 less than 1080p
            mazeCanvas.Width = 1820;
            mazeCanvas.Background = Brushes.LightGray;
            RootWindow.Content = mazeCanvas;

            MainFunction();
        }

        public void MainFunction()
        {
            LoadInComponents();
            CreateMazeTemplate();
        }
        
        public void LoadInComponents()
        {
            TextBlock statsTitle = new TextBlock();
            statsTitle.Width = 120;
            Canvas.SetTop(statsTitle, 180);
            Canvas.SetLeft(statsTitle, 50);
            mazeCanvas.Children.Add(statsTitle);

            TextBlock statsTitleUnderlined = new TextBlock();
            statsTitleUnderlined.Width = 120;
            Canvas.SetTop(statsTitleUnderlined, 180);
            Canvas.SetLeft(statsTitleUnderlined, 50);
            mazeCanvas.Children.Add(statsTitleUnderlined);

            TextBlock recordNoOfMoves = new TextBlock();
            recordNoOfMoves.Width = 120;
            Canvas.SetTop(recordNoOfMoves, 200);
            Canvas.SetLeft(recordNoOfMoves, 50);
            mazeCanvas.Children.Add(recordNoOfMoves);

            //TextBlock currentMoveCount = new TextBlock();
            currentMoveCount.Width = 120;
            Canvas.SetTop(currentMoveCount, 220);
            Canvas.SetLeft(currentMoveCount, 50);
            mazeCanvas.Children.Add(currentMoveCount);

            TextBlock chooseFlag = new TextBlock();
            chooseFlag.Text = "Choose a maze below:";
            chooseFlag.Width = 120;
            Canvas.SetTop(chooseFlag, 50);
            Canvas.SetLeft(chooseFlag, 50);
            mazeCanvas.Children.Add(chooseFlag);

            ComboBox flagSelector = new ComboBox();
            flagSelector.Width = 60;
            Canvas.SetTop(flagSelector, 70);
            Canvas.SetLeft(flagSelector, 50);
            mazeCanvas.Children.Add(flagSelector);

            Button confirmFlag = new Button();
            confirmFlag.Content = "Confirm";
            confirmFlag.Width = 60;
            Canvas.SetTop(confirmFlag, 70);
            Canvas.SetLeft(confirmFlag, 115);
            mazeCanvas.Children.Add(confirmFlag);
            confirmFlag.Click += (sender, e) => ConfirmFlag_click(sender, e, statsTitle, statsTitleUnderlined, recordNoOfMoves, currentMoveCount, flagSelector, confirmFlag);

            Button resetFlagSelection = new Button();
            resetFlagSelection.Content = "Reset selection";
            resetFlagSelection.Width = 97.5;
            Canvas.SetTop(resetFlagSelection, 70);
            Canvas.SetLeft(resetFlagSelection, 180);
            mazeCanvas.Children.Add(resetFlagSelection);
            resetFlagSelection.Click += (sender, e) => ResetFlagSelection_click(sender, e, statsTitle, statsTitleUnderlined, recordNoOfMoves, currentMoveCount, flagSelector, confirmFlag);

            List<TextBlock> rowAxis = new List<TextBlock>();
            for (int i = 0; i < 21; i++)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = i.ToString(),
                    Width = 20,
                    Height = 20,
                    Foreground = Brushes.Red
                };
                rowAxis.Add(textBlock);
                Canvas.SetTop(textBlock, 250);
                Canvas.SetLeft(textBlock, 697 + (i * 20.8));
                textBlock.Visibility = Visibility.Hidden;
                mazeCanvas.Children.Add(textBlock);
            }

            List<TextBlock> colAxis = new List<TextBlock>();
            for (int i = 0; i < 21; i++)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = i.ToString(),
                    Width = 20,
                    Height = 20,
                    Foreground = Brushes.Red,
                    TextAlignment = TextAlignment.Right
                };
                colAxis.Add(textBlock);
                Canvas.SetTop(textBlock, 274 + (i * 20.8));
                Canvas.SetLeft(textBlock, 660);
                textBlock.Visibility = Visibility.Hidden;
                mazeCanvas.Children.Add(textBlock);
            }

            Button movePlayer = new Button();
            movePlayer.Content = "movePlayer";
            movePlayer.Width = 75;
            Canvas.SetTop(movePlayer, 70);
            Canvas.SetLeft(movePlayer, 780);
            mazeCanvas.Children.Add(movePlayer);
            movePlayer.Visibility = Visibility.Hidden;
            movePlayer.Click += (sender, e) => MovePlayer_click(sender, e, flagSelector);

            Button devTools = new Button();
            devTools.Content = "devTools";
            devTools.Width = 60;
            Canvas.SetTop(devTools, 70);
            Canvas.SetLeft(devTools, 700);
            mazeCanvas.Children.Add(devTools);
            devTools.Click += (sender, e) => DevTools_click(sender, e, rowAxis, colAxis, movePlayer);

            string[] files = Directory.GetFiles(@"F:\Documents\Programming\C#\MazeGameV2\mazes\Loadable\", "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string file in files )
            {
                if (!file.Substring(54).Contains('_'))
                { flagSelector.Items.Add(file.Substring(54, 5)); }
            }
        }

        public void CreateMazeTemplate()
        {
            // setting up the base properties of the 400 rectangle objects
            for (int row = 0; row < 21; row++)
            {
                for (int col = 0; col < 21; col++)
                {
                    mazeGrid[row, col] = new Rectangle
                    {
                        Height = 21,
                        Width = 21,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black
                    };

                    // calculating the indexes to make the maze grid appear in the centre of the canvas
                    double topIndex = ((mazeCanvas.Height / 2) - (10 * 21) - 10.5);
                    double leftIndex = ((mazeCanvas.Width / 2) - (10 * 21)) - 10.5;

                    // inserting each rectangle object onto the canvas
                    Canvas.SetTop(mazeGrid[row, col], topIndex + (row * 21));
                    Canvas.SetLeft(mazeGrid[row, col], leftIndex + (col * 21));
                    mazeCanvas.Children.Add(mazeGrid[row, col]);
                }
            }
        }

        public void ImportMaze(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            /* // code to test if the text form maze has been properly loaded in
            foreach (string line in lines)
            { Console.WriteLine(line); }
            */

            int j = 0;
            foreach (string line in lines)
            {
                for (int i = 0; i < line.Length; i++)
                { mazeTextForm[j, i] = line[i]; }
                
                j++;
            }
        }

        public void DisplayMaze()
        {
            for (int row = 0; row < 21; row++)
            {
                for (int col = 0; col < 21; col++)
                {
                    switch (mazeTextForm[row, col])
                    {
                        case ' ':
                            mazeGrid[row, col].Fill = Brushes.White;
                            break;

                        case '#':
                            mazeGrid[row, col].Fill = Brushes.Black;
                            break;

                        case 'F':
                            mazeGrid[row, col].Fill = Brushes.Lime;
                            break;
                        
                        case 'T':
                            mazeGrid[row, col].Fill = Brushes.Aqua;
                            break;

                        case '1':
                            mazeGrid[row, col].Fill = Brushes.Coral;
                            break;

                        case 'E':
                            mazeGrid[row, col].Fill = Brushes.Red;
                            break;

                        case 'S':
                            mazeGrid[row, col].Fill = Brushes.DarkViolet; // gets changed to black when prismarine moves off start square
                            prismarine.SetYX(row, col, 0);
                            if (row == 0) prismarine.SetCompass('S');
                            else if (row == 20) prismarine.SetCompass('N');
                            else if (col == 0) prismarine.SetCompass('E');
                            else if (col == 20) prismarine.SetCompass('W');
                            break;
                    }
                }
            }
        }

        public TeleportSquare TeleportPosition(string fileName)
        {
            string filePath = @"F:\Documents\Programming\C#\MazeGameV2\mazes\Loadable\" + fileName;
            string[] lines = File.ReadAllLines(filePath);

            TeleportSquare ts = new TeleportSquare();

            int j = -1;
            bool breakLoops = false;
            foreach (string line in lines)
            {
                j++;
                for (int i = 0; i < line.Length; i++)
                {
                    if (mazeTextForm[j, i] == 'T' && prismarine.GetY() != j)
                    {
                        ts.SetTsYX(j, i);
                        breakLoops = true;
                        break;
                    }
                }

                if (breakLoops)
                { break; }
            }

            return ts;
        }

        public string PullFlagRecord(string flagName)
        {
            string filePath = @"F:\Documents\Programming\C#\MazeGameV2\maze_records\loadable.txt";
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (line.Substring(0, 5) == flagName)
                { return line.Substring(7); }
            }

            return "";
        }

        private void RootWindow_KeyDown(object sender, KeyEventArgs e)
        {
            bool mazeCompleted = false;
            bool changeMoveCounter = false;

            switch (e.Key)
            {
                case Key.W:
                    if (prismarine.GetY() - 1 >= 0)
                    {
                        if ((mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != 'S')) changeMoveCounter = true;

                        if (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] == 'F')
                        {
                            mazeTextForm[prismarine.GetY(), prismarine.GetX()] = '#';
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetY(prismarine.GetY() - 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                            mazeCompleted = true;
                        }

                        else if (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] == 'T')
                        {
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetY(prismarine.GetY() - 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }

                        else if (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] == ' ')
                        {
                            if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'S')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'T')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == ' ')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                            prismarine.SetY(prismarine.GetY() - 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }
                    }
                    break;

                case Key.A:
                    if (prismarine.GetX() - 1 >= 0)
                    {
                        if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != 'S')) changeMoveCounter = true;

                        if (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] == 'F')
                        {
                            mazeTextForm[prismarine.GetY(), prismarine.GetX()] = '#';
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetX(prismarine.GetX() - 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                            mazeCompleted = true;
                        }

                        else if (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] == 'T')
                        {
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetX(prismarine.GetX() - 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }

                        else if (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] == ' ')
                        {
                            if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'S')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'T')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == ' ')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                            prismarine.SetX(prismarine.GetX() - 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }
                    }
                    break;

                case Key.S:
                    if (prismarine.GetY() + 1 <= 20)
                    {
                        if ((mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != 'S')) changeMoveCounter = true;

                        if (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] == 'F')
                        {
                            mazeTextForm[prismarine.GetY(), prismarine.GetX()] = '#';
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetY(prismarine.GetY() + 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                            mazeCompleted = true;
                        }

                        else if (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] == 'T')
                        {
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetY(prismarine.GetY() + 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }

                        else if (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] == ' ')
                        {
                            if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'S')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'T')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == ' ')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                            prismarine.SetY(prismarine.GetY() + 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }
                    }
                    break;

                case Key.D:
                    if (prismarine.GetX() + 1 <= 20)
                    {
                        if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != 'S')) changeMoveCounter = true;

                        if (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] == 'F')
                        {
                            mazeTextForm[prismarine.GetY(), prismarine.GetX()] = '#';
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetX(prismarine.GetX() + 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                            mazeCompleted = true;
                        }

                        else if (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] == 'T')
                        {
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                            prismarine.SetX(prismarine.GetX() + 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }

                        else if (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] == ' ')
                        {
                            if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'S')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'T')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                            else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == ' ')
                            { mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                            prismarine.SetX(prismarine.GetX() + 1);
                            mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                        }
                    }
                    break;

                case Key.Space:
                    if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == 'T')
                    {
                        changeMoveCounter = true;
                        TeleportSquare ts = TeleportPosition(flag);

                        mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua;
                        prismarine.SetYX(ts.GetTsY(), ts.GetTsX(), prismarine.GetMoveCount());
                        mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }

                    else if (mazeTextForm[prismarine.GetY(), prismarine.GetX()] == '1')
                    {

                    }
                    break;
            }

            if (changeMoveCounter) prismarine.SetMoveCount(prismarine.GetMoveCount() + 1);
            currentMoveCount.Text = "Current: " + prismarine.GetMoveCount() + " moves";

            if (mazeCompleted)
            { MessageBox.Show("You completed the maze in " + prismarine.GetMoveCount() + " moves!"); }
        }

        private void ConfirmFlag_click(object sender, EventArgs e, TextBlock statsTitle, TextBlock statsTitleUnderlined, TextBlock recordNoOfMoves, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag)
        {
            if (flagSelector.SelectedIndex > -1)
            {
                flagSelector.IsEnabled = false;
                confirmFlag.IsEnabled = false;

                string flagName = flagSelector.SelectedItem.ToString();
                flag = flagName + ".txt"; // try and remove the need for this global variable
                string filePath = @"F:\Documents\Programming\C#\MazeGameV2\mazes\Loadable\" + flag;

                ImportMaze(filePath);
                DisplayMaze();

                statsTitle.Text = "prismarine stats:";
                statsTitleUnderlined.Text = "_________________";
                recordNoOfMoves.Text = "Record: " + PullFlagRecord(flagName) + " moves";
                currentMoveCount.Text = "Current: " + prismarine.GetMoveCount() + " moves";
            }
            
            else MessageBox.Show("Flag not selected!");
        }

        private void ResetFlagSelection_click(object sender, EventArgs e, TextBlock statsTitle, TextBlock statsTitleUnderlined, TextBlock recordNoOfMoves, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag)
        {
            flagSelector.SelectedIndex = -1;
            flagSelector.IsEnabled = true;
            confirmFlag.IsEnabled = true;

            prismarine.SetYX(0, 0, 0);
            CreateMazeTemplate();

            statsTitle.Text = "";
            statsTitleUnderlined.Text = "";
            recordNoOfMoves.Text = "";
            currentMoveCount.Text = "";
        }
        
        private void MovePlayer_click(object sender, EventArgs e, ComboBox flagSelector)
        {
            if (flagSelector.SelectedIndex > -1)
            {
                
            }
        }

        private void DevTools_click(object sender, EventArgs e, List<TextBlock> rowAxis, List<TextBlock> colAxis, Button movePlayer)
        {
            bool invisible = rowAxis.All(label => label.IsVisible == false);

            if (invisible)
            { 
                foreach (TextBlock label in rowAxis) label.Visibility = Visibility.Visible;
                foreach (TextBlock label in colAxis) label.Visibility = Visibility.Visible;
                movePlayer.Visibility = Visibility.Visible;
            }
            else
            { 
                foreach (TextBlock label in rowAxis) label.Visibility = Visibility.Hidden;
                foreach (TextBlock label in colAxis) label.Visibility = Visibility.Hidden;
                movePlayer.Visibility = Visibility.Hidden;
            }
        }
    }
}

/* to do:
            make enemies appear in maze - red in colour, if you land on them before attacking them (press space) you have to start again from the begining of the maze
            maze solver - automates finding the exit of the maze
            add timer
            add controls description
            add sound effects - voiced by courtney? "NO! SHOULD BE AT THE TOP OF THE LIST! GET RID OF THE QUESTION MARK!!!!!!! x"
            make default blank maze (first time starting up?) the title of the game
*/