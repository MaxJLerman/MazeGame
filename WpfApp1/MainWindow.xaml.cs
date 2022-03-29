using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    public class Player
    {
        private int y;
        private int x;
        private int moveCount;
        private char compass;
        private bool canUseKeyboard;

        public Player() { }
        public Player(int y, int x, int moveCount, char compass, bool canUseKeyboard)
        {
            this.y = y;
            this.x = x;
            this.moveCount = moveCount;
            this.compass = compass;
            this.canUseKeyboard = canUseKeyboard;
        }

        public int GetY() { return y; }
        public void SetY(int y) { this.y = y; }
        public int GetX() { return x; }
        public void SetX(int x) { this.x = x; }
        public int GetMoveCount() { return moveCount; }
        public void SetMoveCount(int moveCount) { this.moveCount = moveCount; }
        public char GetCompass() { return compass; }
        public void SetCompass(char compass) { this.compass = compass; }
        public bool GetCanUseKeyboard() { return canUseKeyboard; }
        public void SetCanUseKeyboard(bool canUseKeyboard) { this.canUseKeyboard = canUseKeyboard; }

        public void SetYX(int y, int x, int moveCount)
        {
            this.y = y;
            this.x = x;
            this.moveCount = moveCount;
        } // for setting up the player when the selected maze is loaded in
    }

    public class TeleportSquare
    {
        private int TsY;
        private int TsX;
        private bool used;

        public TeleportSquare() { }
        public TeleportSquare(int TsY, int TsX, bool used)
        {
            this.TsY = TsY;
            this.TsX = TsX;
            this.used = used;
        }

        public int GetTsY() { return TsY; }
        public int GetTsX() { return TsX; }
        public bool GetUsed() { return used; }
        public void SetUsed(bool used) { this.used = used; }
        public void SetTsYX(int TsY, int TsX)
        {
            this.TsY = TsY;
            this.TsX = TsX;
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Canvas mazeCanvas = new Canvas(); // create a canvas to display the maze
            Rectangle[,] mazeGrid = new Rectangle[21, 21]; // create 400 rectangle objects to be squares in the maze
            char[,] mazeTextForm = new char[21, 21]; // create a 20x20 array of character objects to represent the maze in text form
            List<TeleportSquare> teleportList = new List<TeleportSquare>();

            ComboBox flagSelector = new ComboBox(); // creating components that need to be passed to multiple functions further down
            TextBlock currentMoveCount = new TextBlock();

            Player prismarine = new Player(0, 0, 0, ' ', false); // creating a player called Prismarine ;)

            this.KeyDown += delegate (object sender, KeyEventArgs e) { RootWindow_KeyDown(sender, e, prismarine, mazeGrid, mazeTextForm, teleportList, flagSelector, currentMoveCount); };

            MainFunction(prismarine, mazeCanvas, mazeGrid, mazeTextForm, teleportList, flagSelector, currentMoveCount);
        }

        public void MainFunction(Player prismarine, Canvas mazeCanvas, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, ComboBox flagSelector, TextBlock currentMoveCount)
        {
            Button confirmFlag = new Button();
            Button resetFlagSelection = new Button();

            Canvas menuCanvas = new Canvas
            {
                Height = 980,
                Width = 1820,
                Background = Brushes.LightGray
            };
            RootWindow.Content = menuCanvas;

            TextBlock mainTitle = new TextBlock
            {
                Text = "MazeGame",
                Width = 400,
                FontSize = 70
            };
            Canvas.SetTop(mainTitle, 200);
            Canvas.SetLeft(mainTitle, 1820 / 2 - 170);
            menuCanvas.Children.Add(mainTitle);

            Button playMazes = new Button
            {
                Content = "Play mazes",
                Width = 400,
                Height = 100,
                FontSize = 50
            };
            Canvas.SetTop(playMazes, 400);
            Canvas.SetLeft(playMazes, 1820 / 2 - 150);
            menuCanvas.Children.Add(playMazes);
            playMazes.Click += (sender, e) => PlayMazes_click(sender, e, prismarine, mazeCanvas, mazeGrid, mazeTextForm, teleportList, flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);

            Canvas solveCanvas = new Canvas();
            Button solveMazes = new Button
            {
                Content = "Solve mazes",
                Width = 400,
                Height = 100,
                FontSize = 50
            };
            Canvas.SetTop(solveMazes, 550);
            Canvas.SetLeft(solveMazes, 1820 / 2 - 150);
            menuCanvas.Children.Add(solveMazes);
            solveMazes.Click += (sender, e) => SolveMazes_click(sender, e, solveCanvas, mazeGrid, mazeTextForm, flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);

            Canvas createCanvas = new Canvas();
            Button createMazes = new Button
            {
                Content = "Create mazes",
                Width = 400,
                Height = 100,
                FontSize = 50
            };
            Canvas.SetTop(createMazes, 700);
            Canvas.SetLeft(createMazes, 1820 / 2 - 150);
            menuCanvas.Children.Add(createMazes);
            createMazes.Click += (sender, e) => CreateMazes_click(sender, e, createCanvas, mazeGrid, mazeTextForm);
        }

        #region Menu choices

        private void PlayMazes_click(object sender, EventArgs e, Player prismarine, Canvas mazeCanvas, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, TextBlock currentMoveCount)
        {
            mazeCanvas.Height = 980; // 100 less than 1080p
            mazeCanvas.Width = 1820;
            mazeCanvas.Background = Brushes.LightGray;
            RootWindow.Content = mazeCanvas;

            LoadMazeComponents(prismarine, mazeCanvas, mazeGrid, mazeTextForm, teleportList, flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);
            CreateMazeTemplate(mazeCanvas, mazeGrid);
        }

        private void SolveMazes_click(object sender, EventArgs e, Canvas solveCanvas, Rectangle[,] mazeGrid, char[,] mazeTextForm, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, TextBlock currentMoveCount) 
        {
            solveCanvas.Height = 980;
            solveCanvas.Width = 1820;
            solveCanvas.Background = Brushes.LightGray;
            RootWindow.Content = solveCanvas;

            LoadSolveComponents(solveCanvas, mazeGrid, mazeTextForm, flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);
            CreateMazeTemplate(solveCanvas, mazeGrid);
        }

        private void CreateMazes_click(object sender, EventArgs e, Canvas createCanvas, Rectangle[,] mazeGrid, char[,] mazeTextForm)
        {
            createCanvas.Height = 980;
            createCanvas.Width = 1820;
            createCanvas.Background = Brushes.LightGray;
            RootWindow.Content = createCanvas;

            //LoadCreateComponents(createCanvas, mazeGrid, mazeTextForm);
            CreateMazeTemplate(createCanvas, mazeGrid);
        }

        #endregion

        #region Loading in respective components

        public void LoadMazeComponents(Player prismarine, Canvas mazeCanvas, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, TextBlock currentMoveCount)
        {
            Button solveThisMaze = new Button();
            Button devTools = new Button();

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

            currentMoveCount.Width = 120;
            Canvas.SetTop(currentMoveCount, 220);
            Canvas.SetLeft(currentMoveCount, 50);
            mazeCanvas.Children.Add(currentMoveCount);

            TextBlock chooseFlag = new TextBlock();
            chooseFlag.Text = "Choose a maze to play:";
            chooseFlag.Width = 130;
            Canvas.SetTop(chooseFlag, 50);
            Canvas.SetLeft(chooseFlag, 50);
            mazeCanvas.Children.Add(chooseFlag);

            flagSelector.Width = 60;
            Canvas.SetTop(flagSelector, 70);
            Canvas.SetLeft(flagSelector, 50);
            mazeCanvas.Children.Add(flagSelector);
            LoadFlagSelection(flagSelector);

            confirmFlag.Content = "Confirm";
            confirmFlag.Width = 60;
            Canvas.SetTop(confirmFlag, 70);
            Canvas.SetLeft(confirmFlag, 115);
            mazeCanvas.Children.Add(confirmFlag);
            confirmFlag.Click += (sender, e) => ConfirmFlag_click(sender, e, 0, prismarine, mazeGrid, mazeTextForm, teleportList, statsTitle, statsTitleUnderlined, recordNoOfMoves, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, solveThisMaze);

            resetFlagSelection.Content = "Reset selection";
            resetFlagSelection.Width = 97.5;
            Canvas.SetTop(resetFlagSelection, 70);
            Canvas.SetLeft(resetFlagSelection, 180);
            mazeCanvas.Children.Add(resetFlagSelection);
            resetFlagSelection.IsEnabled = false;
            resetFlagSelection.Click += (sender, e) => ResetFlagSelection_click(sender, e, 0, prismarine, mazeCanvas, mazeGrid, statsTitle, statsTitleUnderlined, recordNoOfMoves, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, solveThisMaze);

            solveThisMaze.Content = "Solve this maze";
            solveThisMaze.Width = 100;
            Canvas.SetTop(solveThisMaze, 400);
            Canvas.SetLeft(solveThisMaze, 50);
            mazeCanvas.Children.Add(solveThisMaze);
            solveThisMaze.IsEnabled = false;
            solveThisMaze.Click += (sender, e) => SolveThisMaze_click(sender, e, prismarine, mazeGrid, mazeTextForm, teleportList, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, solveThisMaze, devTools);

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

            devTools.Content = "devTools";
            devTools.Width = 60;
            Canvas.SetTop(devTools, 70);
            Canvas.SetLeft(devTools, 700);
            mazeCanvas.Children.Add(devTools);
            devTools.Click += (sender, e) => DevTools_click(sender, e, rowAxis, colAxis, movePlayer);
        }

        public void LoadSolveComponents(Canvas solveCanvas, Rectangle[,] mazeGrid, char[,] solvedTextForm, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, TextBlock currentMoveCount)
        {
            TextBlock chooseFlag = new TextBlock();
            chooseFlag.Text = "Choose a maze to solve:";
            chooseFlag.Width = 130;
            Canvas.SetTop(chooseFlag, 50);
            Canvas.SetLeft(chooseFlag, 50);
            solveCanvas.Children.Add(chooseFlag);

            flagSelector.Width = 60;
            Canvas.SetTop(flagSelector, 70);
            Canvas.SetLeft(flagSelector, 50);
            solveCanvas.Children.Add(flagSelector);
            LoadFlagSelection(flagSelector);

            confirmFlag.Content = "Confirm";
            confirmFlag.Width = 60;
            Canvas.SetTop(confirmFlag, 70);
            Canvas.SetLeft(confirmFlag, 115);
            solveCanvas.Children.Add(confirmFlag);
            confirmFlag.Click += (sender, e) => ConfirmFlag_click(sender, e, 1, null, mazeGrid, solvedTextForm, null, null, null, null, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, null);

            resetFlagSelection.Content = "Reset selection";
            resetFlagSelection.Width = 97.5;
            Canvas.SetTop(resetFlagSelection, 70);
            Canvas.SetLeft(resetFlagSelection, 180);
            solveCanvas.Children.Add(resetFlagSelection);
            resetFlagSelection.Click += (sender, e) => ResetFlagSelection_click(sender, e, 1, null, solveCanvas, mazeGrid, null, null, null, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, null);
        }

        public void LoadFlagSelection(ComboBox flagSelector)
        {
            string[] files = Directory.GetFiles(@"F:\Documents\Programming\C#\MazeGameV2\mazes\Loadable\", "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (!file.Substring(54).Contains('_'))
                { flagSelector.Items.Add(file.Substring(54, 5)); }
            }
        }

        #endregion

        #region Setting up the visual maze

        public void CreateMazeTemplate(Canvas canvas, Rectangle[,] mazeGrid)
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
                    double topIndex = ((canvas.Height / 2) - (10 * 21) - 10.5);
                    double leftIndex = ((canvas.Width / 2) - (10 * 21)) - 10.5;

                    // inserting each rectangle object onto the canvas
                    Canvas.SetTop(mazeGrid[row, col], topIndex + (row * 21));
                    Canvas.SetLeft(mazeGrid[row, col], leftIndex + (col * 21));
                    canvas.Children.Add(mazeGrid[row, col]);
                }
            }
        }

        public void ImportMaze(string filePath, char[,] mazeTextForm)
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

        public void DisplayMaze(int mode, Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList)
        {
            teleportList.Clear();

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
                            teleportList.Add(new TeleportSquare(row, col, false));
                            break;

                        case '1':
                            mazeGrid[row, col].Fill = Brushes.Coral;
                            break;

                        case 'E':
                            mazeGrid[row, col].Fill = Brushes.Red;
                            break;

                        case 'S':
                            mazeGrid[row, col].Fill = Brushes.DarkViolet; // gets changed to dark grey when prismarine moves off start square
                            if (mode == 0)
                            {
                                prismarine.SetYX(row, col, 0);
                                if (row == 0) prismarine.SetCompass('S');
                                else if (row == 20) prismarine.SetCompass('N');
                                else if (col == 0) prismarine.SetCompass('E');
                                else if (col == 20) prismarine.SetCompass('W');
                            }
                            break;
                    }
                }
            }
        }

        public string PullFlagRecord(string flagName)
        {
            string filePath = @"F:\Documents\Programming\C#\MazeGameV2\maze_records\loadable.txt";
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (line.Substring(0, 5) == flagName.Substring(0, 5)) 
                { return line.Substring(7); }
            }

            return "NA";
        }

        #endregion

        #region Moving the player

        private void RootWindow_KeyDown(object sender, KeyEventArgs e, Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, ComboBox flagSelector, TextBlock currentMoveCount)
        {
            bool mazeCompleted = false; // may need to raise these to MainWindow() for use in solve maze functions
            bool changeMoveCounter = false;
            
            if (prismarine.GetCanUseKeyboard())
            {
                switch (e.Key)
                {
                    case Key.W:
                        MoveUp(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                    break;

                    case Key.A:
                        MoveLeft(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                        break;

                    case Key.S:
                        MoveDown(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                        break;

                    case Key.D:
                        MoveRight(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                        break;

                    case Key.Space:
                        MoveTeleport(prismarine, mazeGrid, mazeTextForm, mazeTextForm[prismarine.GetY(), prismarine.GetX()], teleportList, ref mazeCompleted, ref changeMoveCounter, flagSelector);
                        break;
                }

                ChangeMoveCounter(prismarine, currentMoveCount, ref changeMoveCounter);
            }

            MazeCompleted(prismarine, ref mazeCompleted);
        }

        public void MoveUp(Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetY() - 1 >= 0) // when solve maze is selected (2nd option) send player variable as a new player: "solver"
            {
                if ((mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('N');

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
            }
        }

        public void MoveLeft(Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetX() - 1 >= 0)
            {
                if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('W');

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
            }
        }

        public void MoveDown(Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetY() + 1 <= 20)
            {
                if ((mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('S');

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
            }
        }

        public void MoveRight(Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetX() + 1 <= 20)
            {
                if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('E');

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
            }
        }

        public void MoveTeleport(Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, char teleportChar, List<TeleportSquare> teleportList, ref bool mazeCompleted, ref bool changeMoveCounter, ComboBox flagSelector)
        {
            if (teleportChar == 'T' || teleportChar == '1')
            {
                TeleportSquare teleportFrom = new TeleportSquare();
                TeleportSquare teleportTo = new TeleportSquare();
                foreach (TeleportSquare ts in teleportList)
                {
                    if (prismarine.GetY() == ts.GetTsY() && prismarine.GetX() == ts.GetTsX() && mazeTextForm[ts.GetTsY(), ts.GetTsX()] == 'T') { teleportFrom = ts; }
                    if (prismarine.GetY() != ts.GetTsY() && prismarine.GetX() != ts.GetTsX() && mazeTextForm[ts.GetTsY(), ts.GetTsX()] == 'T') { teleportTo = ts; }
                }

                if (!teleportFrom.GetUsed())
                {
                    changeMoveCounter = true;

                    if (teleportChar == 'T')
                    {
                        teleportFrom.SetUsed(true);
                        teleportTo.SetUsed(true);

                        mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua;
                        prismarine.SetYX(teleportTo.GetTsY(), teleportTo.GetTsX(), prismarine.GetMoveCount());
                        mazeGrid[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }

                    else if (teleportChar == '1')
                    {

                    }
                }
            }
        }

        public void ChangeMoveCounter(Player prismarine, TextBlock currentMoveCount, ref bool changeMoveCounter)
        {
            if (changeMoveCounter) prismarine.SetMoveCount(prismarine.GetMoveCount() + 1);
            currentMoveCount.Text = "Current: " + prismarine.GetMoveCount() + " moves";
        }

        public void MazeCompleted(Player prismarine, ref bool mazeCompleted)
        {
            if (mazeCompleted)
            { MessageBox.Show("You completed the maze in " + prismarine.GetMoveCount() + " moves!"); }
        } 

        #endregion

        #region Solving algorithm functions

        public void Solve_RandomMouse(Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, ref bool mazeCompleted, ref bool changeMoveCounter, TextBlock currentMoveCount, ComboBox flagSelector)
        {
            Random rnd = new Random();

            switch (rnd.Next(5))
            {
                case 0:
                    MoveUp(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                    break;

                case 1:
                    MoveLeft(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                    break;

                case 2:
                    MoveDown(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                    break;
                        
                case 3:
                    MoveRight(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter);
                    break;

                case 4:
                    MoveTeleport(prismarine, mazeGrid, mazeTextForm, mazeTextForm[prismarine.GetY(), prismarine.GetX()], teleportList, ref mazeCompleted, ref changeMoveCounter, flagSelector);
                    break;
            }

            ChangeMoveCounter(prismarine, currentMoveCount, ref changeMoveCounter);
            MazeCompleted(prismarine, ref mazeCompleted);
        }

        public void Solve_HugLeftWall(Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, ref bool mazeCompleted, ref bool changeMoveCounter, TextBlock currentMoveCount, ComboBox flagSelector) // favoured for solving mazes in play mazes mode
        {
            char[] teleportChars = { 'T', '1' };
            if (teleportChars.Contains(mazeTextForm[prismarine.GetY(), prismarine.GetX()]))
            { MoveTeleport(prismarine, mazeGrid, mazeTextForm, mazeTextForm[prismarine.GetY(), prismarine.GetX()], teleportList, ref mazeCompleted, ref changeMoveCounter, flagSelector); }

            if (prismarine.GetCompass() == 'N')
            {
                if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }
            }

            else if (prismarine.GetCompass() == 'W')
            {
                if ((mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }
            }

            else if (prismarine.GetCompass() == 'S')
            {
                if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }
            }

            else if (prismarine.GetCompass() == 'E')
            {
                if ((mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else  if ((mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeTextForm[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeTextForm[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeTextForm, ref mazeCompleted, ref changeMoveCounter); }
            }

            ChangeMoveCounter(prismarine, currentMoveCount, ref changeMoveCounter);
            MazeCompleted(prismarine, ref mazeCompleted);
        }

        public void Solve_Tremaux(Player prismarine)
        {

        }

        public void Solve_BlindAlley(Player prismarine)
        {

        }

        public void FillDeadEnds(Player prismarine)
        {

        }

        private void DispatcherTimer_tick(object sender, EventArgs e, DispatcherTimer dispatcher, Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, bool mazeCompleted, bool changeMoveCounter, TextBlock currentMoveCount, ComboBox flagSelector)
        {
            //Solve_RandomMouse(prismarine, mazeGrid, mazeTextForm, teleportList, ref mazeCompleted, ref changeMoveCounter, currentMoveCount, flagSelector);
            Solve_HugLeftWall(prismarine, mazeGrid, mazeTextForm, teleportList, ref mazeCompleted, ref changeMoveCounter, currentMoveCount, flagSelector);
            if (mazeCompleted) dispatcher.Stop();
        }

        #endregion

        #region Create maze functions

        public void Create_Minotaur()
        {

        }

        #endregion

        #region [Play mazes] buttons

        private void ConfirmFlag_click(object sender, EventArgs e, int mode, Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, TextBlock statsTitle, TextBlock statsTitleUnderlined, TextBlock recordNoOfMoves, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, Button solveThisMaze)
        {
            if (flagSelector.SelectedIndex > -1)
            {
                flagSelector.IsEnabled = false;
                confirmFlag.IsEnabled = false;
                resetFlagSelection.IsEnabled = true;
                
                prismarine.SetCanUseKeyboard(true);

                string flagName = flagSelector.SelectedItem.ToString() + ".txt";
                string filePath = @"F:\Documents\Programming\C#\MazeGameV2\mazes\Loadable\" + flagName;

                if (mode == 0) // play mazes mode
                {   
                    solveThisMaze.IsEnabled = true;

                    statsTitle.Text = "prismarine stats:";
                    statsTitleUnderlined.Text = "_________________";
                    recordNoOfMoves.Text = "Record: " + PullFlagRecord(flagName) + " moves";
                    currentMoveCount.Text = "Current: " + prismarine.GetMoveCount() + " moves";
                }

                else if (mode == 1) // solve mazes mode
                {

                }

                ImportMaze(filePath, mazeTextForm);
                DisplayMaze(mode, prismarine, mazeGrid, mazeTextForm, teleportList);
            }
            
            else MessageBox.Show("Flag not selected!");
        }

        private void ResetFlagSelection_click(object sender, EventArgs e, int mode, Player prismarine, Canvas mazeCanvas, Rectangle[,] mazeGrid, TextBlock statsTitle, TextBlock statsTitleUnderlined, TextBlock recordNoOfMoves, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, Button solveThisMaze)
        {
            flagSelector.SelectedIndex = -1;
            flagSelector.IsEnabled = true;
            confirmFlag.IsEnabled = true;

            prismarine.SetCanUseKeyboard(false);
            CreateMazeTemplate(mazeCanvas, mazeGrid);

            if (mode == 0)
            {
                prismarine.SetYX(0, 0, 0);

                statsTitle.Text = "";
                statsTitleUnderlined.Text = "";
                recordNoOfMoves.Text = "";
                currentMoveCount.Text = "";
            }

            else if (mode == 1)
            {

            }
        }

        private void SolveThisMaze_click(object sender, EventArgs e, Player prismarine, Rectangle[,] mazeGrid, char[,] mazeTextForm, List<TeleportSquare> teleportList, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, Button solveThisMaze, Button devTools)
        {
            bool mazeCompleted = false;
            bool changeMoveCounter = false;

            DispatcherTimer dispatcher = new DispatcherTimer(DispatcherPriority.Send);
            dispatcher.Tick += (sender2, e2) => { DispatcherTimer_tick(sender2, e2, dispatcher, prismarine, mazeGrid, mazeTextForm, teleportList, mazeCompleted, changeMoveCounter, currentMoveCount, flagSelector); };
            dispatcher.Interval = TimeSpan.FromMilliseconds(25); // change to 0 for instantaneous solve

            prismarine.SetCanUseKeyboard(false); // disables keyboard input to prevent user from interfering
            solveThisMaze.IsEnabled = false; // prevents the button from being clicked multiple times
            devTools.IsEnabled = false; // prevents the button from being clicked
            DisplayMaze(0, prismarine, mazeGrid, mazeTextForm, teleportList); // resets the visual representation of the maze

            dispatcher.Start();
        }

        #endregion

        #region Developer tools

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

        private void MovePlayer_click(object sender, EventArgs e, ComboBox flagSelector)
        {
            if (flagSelector.SelectedIndex > -1)
            {
                MovePlayer mp = new MovePlayer();
                mp.ShowDialog();

                //((Window1)Application.Current.Windows[1]).textBlock1.Text="your text";
            }
        }

        #endregion
    }
}

/* to do:
            make enemies appear in maze - red in colour, if you land on them before attacking them (press a key) you have to start again from the begining of the maze
            maze solver - automates finding the exit of the maze
            add timer
            add controls description
            add sound effects - voiced by courtney? "NO! SHOULD BE AT THE TOP OF THE LIST! GET RID OF THE QUESTION MARK!!!!!!! x"
            make default blank maze (first time starting up?) + black squares => the title of the game, display before main menu
            implement a way for people to store their name & scores in MySQL database or MongoDB database??? if not, then in notepad file
*/