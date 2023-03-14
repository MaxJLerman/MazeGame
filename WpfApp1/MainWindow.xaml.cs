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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // create a maze level variable to hold all the essential information regarding the maze to be loaded in
            MazeLevel mazeLevel = new MazeLevel(new Canvas(), new Rectangle[21, 21], new char[21, 21], new List<TeleportSquare>(), -1);

            // creating components that need to be passed to multiple functions further down
            ComboBox flagSelector = new ComboBox();
            TextBlock currentMoveCount = new TextBlock();

            Player prismarine = new Player(0, 0, 0, ' ', false); // creating a player called Prismarine ;)

            this.KeyDown += delegate (object sender, KeyEventArgs e) { RootWindow_KeyDown(sender, e, prismarine, mazeLevel, flagSelector, currentMoveCount); };

            MainFunction(prismarine, mazeLevel, flagSelector, currentMoveCount); // run the game's main function
        }

        public void MainFunction(Player prismarine, MazeLevel mazeLevel, ComboBox flagSelector, TextBlock currentMoveCount)
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
            playMazes.Click += (sender, e) => PlayMazes_Click(sender, e, prismarine, mazeLevel, flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);

            /*
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
            solveMazes.Click += (sender, e) => SolveMazes_Click(sender, e, solveCanvas, mazeGrid, mazeLevel.GetTextForm(), flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);
            */

            /*
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
            createMazes.Click += (sender, e) => CreateMazes_Click(sender, e, createCanvas, mazeGrid, mazeLevel.GetTextForm());
            */
        }

        #region Menu choices

        private void PlayMazes_Click(object sender, EventArgs e, Player prismarine, MazeLevel mazeLevel, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, TextBlock currentMoveCount)
        {
            mazeLevel.SetCanvas(980, 1820, Brushes.LightGray);
            RootWindow.Content = mazeLevel.GetCanvas();

            LoadMazeComponents(prismarine, mazeLevel, flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);
            CreateMazeTemplate(mazeLevel);
        }

        /*
        private void SolveMazes_Click(object sender, EventArgs e, Canvas solveCanvas, Rectangle[,] mazeGrid, char[,] mazeTextForm, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, TextBlock currentMoveCount) 
        {
            solveCanvas.Height = 980;
            solveCanvas.Width = 1820;
            solveCanvas.Background = Brushes.LightGray;
            RootWindow.Content = solveCanvas;

            //LoadSolveComponents(solveCanvas, mazeGrid, mazeLevel.GetTextForm(), flagSelector, confirmFlag, resetFlagSelection, currentMoveCount);
            //CreateMazeTemplate(solveCanvas, mazeGrid);
        }
        */

        /*
        private void CreateMazes_Click(object sender, EventArgs e, Canvas createCanvas, Rectangle[,] mazeGrid, char[,] mazeTextForm)
        {
            createCanvas.Height = 980;
            createCanvas.Width = 1820;
            createCanvas.Background = Brushes.LightGray;
            RootWindow.Content = createCanvas;

            //LoadCreateComponents(createCanvas, mazeGrid, mazeLevel.GetTextForm());
            //CreateMazeTemplate(createCanvas, mazeGrid);

            Create_Minotaur(mazeGrid);
        }
        */

        #endregion

        #region Loading in respective components

        public void LoadMazeComponents(Player prismarine, MazeLevel mazeLevel, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, TextBlock currentMoveCount)
        {
            Button solveThisMaze = new Button();
            Button devTools = new Button();

            TextBlock statsTitle = new TextBlock();
            statsTitle.Width = 120;
            Canvas.SetTop(statsTitle, 180);
            Canvas.SetLeft(statsTitle, 50);
            mazeLevel.GetCanvas().Children.Add(statsTitle);

            TextBlock statsTitleUnderlined = new TextBlock();
            statsTitleUnderlined.Width = 120;
            Canvas.SetTop(statsTitleUnderlined, 180);
            Canvas.SetLeft(statsTitleUnderlined, 50);
            mazeLevel.GetCanvas().Children.Add(statsTitleUnderlined);

            TextBlock recordNoOfMoves = new TextBlock();
            recordNoOfMoves.Width = 120;
            Canvas.SetTop(recordNoOfMoves, 200);
            Canvas.SetLeft(recordNoOfMoves, 50);
            mazeLevel.GetCanvas().Children.Add(recordNoOfMoves);

            currentMoveCount.Width = 120;
            Canvas.SetTop(currentMoveCount, 220);
            Canvas.SetLeft(currentMoveCount, 50);
            mazeLevel.GetCanvas().Children.Add(currentMoveCount);

            TextBlock chooseFlag = new TextBlock();
            chooseFlag.Text = "Choose a maze to play:";
            chooseFlag.Width = 130;
            Canvas.SetTop(chooseFlag, 50);
            Canvas.SetLeft(chooseFlag, 50);
            mazeLevel.GetCanvas().Children.Add(chooseFlag);

            flagSelector.Width = 60;
            Canvas.SetTop(flagSelector, 70);
            Canvas.SetLeft(flagSelector, 50);
            mazeLevel.GetCanvas().Children.Add(flagSelector);
            LoadFlagSelection(flagSelector);

            confirmFlag.Content = "Confirm";
            confirmFlag.Width = 60;
            Canvas.SetTop(confirmFlag, 70);
            Canvas.SetLeft(confirmFlag, 115);
            mazeLevel.GetCanvas().Children.Add(confirmFlag);
            confirmFlag.Click += (sender, e) => ConfirmFlag_Click(sender, e, 0, prismarine, mazeLevel, statsTitle, statsTitleUnderlined, recordNoOfMoves, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, solveThisMaze);

            resetFlagSelection.Content = "Reset selection";
            resetFlagSelection.Width = 97.5;
            Canvas.SetTop(resetFlagSelection, 70);
            Canvas.SetLeft(resetFlagSelection, 180);
            mazeLevel.GetCanvas().Children.Add(resetFlagSelection);
            resetFlagSelection.IsEnabled = false;
            resetFlagSelection.Click += (sender, e) => ResetFlagSelection_Click(sender, e, 0, prismarine, mazeLevel, statsTitle, statsTitleUnderlined, recordNoOfMoves, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, solveThisMaze);

            solveThisMaze.Content = "Solve this maze";
            solveThisMaze.Width = 100;
            Canvas.SetTop(solveThisMaze, 400);
            Canvas.SetLeft(solveThisMaze, 50);
            mazeLevel.GetCanvas().Children.Add(solveThisMaze);
            solveThisMaze.IsEnabled = false;
            solveThisMaze.Click += (sender, e) => SolveThisMaze_Click(sender, e, prismarine, mazeLevel, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, solveThisMaze, devTools);

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
                mazeLevel.GetCanvas().Children.Add(textBlock);
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
                mazeLevel.GetCanvas().Children.Add(textBlock);
            }

            Button movePlayer = new Button();
            movePlayer.Content = "movePlayer";
            movePlayer.Width = 75;
            Canvas.SetTop(movePlayer, 70);
            Canvas.SetLeft(movePlayer, 780);
            mazeLevel.GetCanvas().Children.Add(movePlayer);
            movePlayer.Visibility = Visibility.Hidden;
            movePlayer.Click += (sender, e) => MovePlayer_Click(sender, e, prismarine, mazeLevel, flagSelector);

            devTools.Content = "devTools";
            devTools.Width = 60;
            Canvas.SetTop(devTools, 70);
            Canvas.SetLeft(devTools, 700);
            mazeLevel.GetCanvas().Children.Add(devTools);
            devTools.Click += (sender, e) => DevTools_Click(sender, e, rowAxis, colAxis, movePlayer);
        }

        /*
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
            //confirmFlag.Click += (sender, e) => ConfirmFlag_Click(sender, e, 1, null, mazeGrid, solvedTextForm, null, null, null, null, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, null);

            resetFlagSelection.Content = "Reset selection";
            resetFlagSelection.Width = 97.5;
            Canvas.SetTop(resetFlagSelection, 70);
            Canvas.SetLeft(resetFlagSelection, 180);
            solveCanvas.Children.Add(resetFlagSelection);
            //resetFlagSelection.Click += (sender, e) => ResetFlagSelection_Click(sender, e, 1, null, solveCanvas, mazeGrid, null, null, null, currentMoveCount, flagSelector, confirmFlag, resetFlagSelection, null);
        }
        */

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

        public void CreateMazeTemplate(MazeLevel mazeLevel)
        {
            // setting up the base properties of the 400 rectangle objects
            for (int row = 0; row < 21; row++)
            {
                for (int col = 0; col < 21; col++)
                {
                    mazeLevel.GetGrid()[row, col] = new Rectangle
                    {
                        Height = 21,
                        Width = 21,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black
                    };

                    // calculating the indexes to make the maze grid appear in the centre of the canvas
                    double topIndex = ((mazeLevel.GetCanvas().Height / 2) - (10 * 21) - 10.5);
                    double leftIndex = ((mazeLevel.GetCanvas().Width / 2) - (10 * 21)) - 10.5;

                    // inserting each rectangle object onto the canvas
                    Canvas.SetTop(mazeLevel.GetGrid()[row, col], topIndex + (row * 21));
                    Canvas.SetLeft(mazeLevel.GetGrid()[row, col], leftIndex + (col * 21));
                    mazeLevel.GetCanvas().Children.Add(mazeLevel.GetGrid()[row, col]);
                }
            }
        }

        public void ImportMaze(string filePath, MazeLevel mazeLevel)
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
                { mazeLevel.GetTextForm()[j, i] = line[i]; }
                
                j++;
            }
        }

        public void DisplayMaze(int mode, Player prismarine, MazeLevel mazeLevel)
        {
            mazeLevel.GetTeleportList().Clear();

            for (int row = 0; row < 21; row++)
            {
                for (int col = 0; col < 21; col++)
                {
                    switch (mazeLevel.GetTextForm()[row, col])
                    {
                        case ' ':
                            mazeLevel.GetGrid()[row, col].Fill = Brushes.White;
                            break;

                        case '#':
                            mazeLevel.GetGrid()[row, col].Fill = Brushes.Black;
                            break;

                        case 'F':
                            mazeLevel.GetGrid()[row, col].Fill = Brushes.Lime;
                            break;
                        
                        case 'T':
                            mazeLevel.GetGrid()[row, col].Fill = Brushes.Aqua;
                            mazeLevel.GetTeleportList().Add(new TeleportSquare(row, col, false));
                            break;

                        case '1':
                            mazeLevel.GetGrid()[row, col].Fill = Brushes.Coral;
                            break;

                        case 'E':
                            mazeLevel.GetGrid()[row, col].Fill = Brushes.Red;
                            break;

                        case 'S':
                            mazeLevel.GetGrid()[row, col].Fill = Brushes.DarkViolet; // gets changed to dark grey when prismarine moves off start square
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

        private void RootWindow_KeyDown(object sender, KeyEventArgs e, Player prismarine, MazeLevel mazeLevel, ComboBox flagSelector, TextBlock currentMoveCount)
        {
            bool mazeCompleted = false; // may need to raise these to MainWindow() for use in solve maze functions
            bool changeMoveCounter = false;
            
            if (prismarine.GetCanUseKeyboard())
            {
                switch (e.Key)
                {
                    case Key.W:
                        MoveUp(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                    break;

                    case Key.A:
                        MoveLeft(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                        break;

                    case Key.S:
                        MoveDown(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                        break;

                    case Key.D:
                        MoveRight(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                        break;

                    case Key.Space:
                        MoveTeleport(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter, flagSelector);
                        break;
                }

                ChangeMoveCounter(prismarine, currentMoveCount, ref changeMoveCounter);
            }

            MazeCompleted(prismarine, ref mazeCompleted);
        }

        public void MoveUp(Player prismarine, MazeLevel mazeLevel, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetY() - 1 >= 0)
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('N');

                    if (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] == 'F')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetY(prismarine.GetY() - 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                        mazeCompleted = true;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] == 'T')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetY(prismarine.GetY() - 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] == ' ')
                    {
                        if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'S')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'T')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == ' ')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                        prismarine.SetY(prismarine.GetY() - 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }
                }
            }
        }

        public void MoveLeft(Player prismarine, MazeLevel mazeLevel, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetX() - 1 >= 0)
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('W');

                    if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] == 'F')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetX(prismarine.GetX() - 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                        mazeCompleted = true;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] == 'T')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetX(prismarine.GetX() - 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] == ' ')
                    {
                        if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'S')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'T')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == ' ')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                        prismarine.SetX(prismarine.GetX() - 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }
                }
            }
        }

        public void MoveDown(Player prismarine, MazeLevel mazeLevel, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetY() + 1 <= 20)
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('S');

                    if (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] == 'F')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetY(prismarine.GetY() + 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                        mazeCompleted = true;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] == 'T')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetY(prismarine.GetY() + 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] == ' ')
                    {
                        if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'S')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'T')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == ' ')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                        prismarine.SetY(prismarine.GetY() + 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }
                }
            }
        }

        public void MoveRight(Player prismarine, MazeLevel mazeLevel, ref bool mazeCompleted, ref bool changeMoveCounter)
        {
            if (prismarine.GetX() + 1 <= 20)
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                {
                    changeMoveCounter = true;
                    prismarine.SetCompass('E');

                    if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] == 'F')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetX(prismarine.GetX() + 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;

                        mazeCompleted = true;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] == 'T')
                    {
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
                        prismarine.SetX(prismarine.GetX() + 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }

                    else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] == ' ')
                    {
                        if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'S')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DimGray; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == 'T')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua; }

                        else if (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()] == ' ')
                        { mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White; }

                        prismarine.SetX(prismarine.GetX() + 1);
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
                    }
                }
            }
        }

        public void MoveTeleport(Player prismarine, MazeLevel mazeLevel, ref bool mazeCompleted, ref bool changeMoveCounter, ComboBox flagSelector)
        {
            char teleportChar = mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()];

            if (teleportChar == 'T' || teleportChar == '1')
            {
                TeleportSquare teleportFrom = new TeleportSquare();
                TeleportSquare teleportTo = new TeleportSquare();
                foreach (TeleportSquare ts in mazeLevel.GetTeleportList())
                {
                    if (prismarine.GetY() == ts.GetTsY() && prismarine.GetX() == ts.GetTsX() && mazeLevel.GetTextForm()[ts.GetTsY(), ts.GetTsX()] == 'T') { teleportFrom = ts; }
                    if (prismarine.GetY() != ts.GetTsY() && prismarine.GetX() != ts.GetTsX() && mazeLevel.GetTextForm()[ts.GetTsY(), ts.GetTsX()] == 'T') { teleportTo = ts; }
                }

                if (!teleportFrom.GetUsed())
                {
                    changeMoveCounter = true;

                    if (teleportChar == 'T')
                    {
                        teleportFrom.SetUsed(true);
                        teleportTo.SetUsed(true);

                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.Aqua;
                        prismarine.SetYX(teleportTo.GetTsY(), teleportTo.GetTsX(), prismarine.GetMoveCount());
                        mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
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
            {
                prismarine.SetCanUseKeyboard(false);
                MessageBox.Show("You completed the maze in " + prismarine.GetMoveCount() + " moves!", "Congratulations!"); 
            }
        } 

        #endregion

        #region Solving algorithm functions

        public void Solve_RandomMouse(Player prismarine, MazeLevel mazeLevel, ref bool mazeCompleted, ref bool changeMoveCounter, TextBlock currentMoveCount, ComboBox flagSelector)
        {
            Random rnd = new Random();

            switch (rnd.Next(5))
            {
                case 0:
                    MoveUp(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                    break;

                case 1:
                    MoveLeft(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                    break;

                case 2:
                    MoveDown(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                    break;
                        
                case 3:
                    MoveRight(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter);
                    break;

                case 4:
                    MoveTeleport(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter, flagSelector);
                    break;
            }

            ChangeMoveCounter(prismarine, currentMoveCount, ref changeMoveCounter);
            MazeCompleted(prismarine, ref mazeCompleted);
        }

        public void Solve_HugLeftWall(Player prismarine, MazeLevel mazeLevel, ref bool mazeCompleted, ref bool changeMoveCounter, TextBlock currentMoveCount, ComboBox flagSelector) // favoured for solving mazes in play mazes mode
        {
            char[] teleportChars = { 'T', '1' };
            if (teleportChars.Contains(mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX()]))
            { MoveTeleport(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter, flagSelector); }

            if (prismarine.GetCompass() == 'N')
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }
            }

            else if (prismarine.GetCompass() == 'W')
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }
            }

            else if (prismarine.GetCompass() == 'S')
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }
            }

            else if (prismarine.GetCompass() == 'E')
            {
                if ((mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() - 1, prismarine.GetX()] != 'S'))
                { MoveUp(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() + 1] != 'S'))
                { MoveRight(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else  if ((mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY() + 1, prismarine.GetX()] != 'S'))
                { MoveDown(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }

                else if ((mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != '#') && (mazeLevel.GetTextForm()[prismarine.GetY(), prismarine.GetX() - 1] != 'S'))
                { MoveLeft(prismarine, mazeGrid, mazeLevel.GetTextForm(), ref mazeCompleted, ref changeMoveCounter); }
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

        private void DispatcherTimer_tick(object sender, EventArgs e, DispatcherTimer dispatcher, Player prismarine, MazeLevel mazeLevel, bool mazeCompleted, bool changeMoveCounter, TextBlock currentMoveCount, ComboBox flagSelector)
        {
            Solve_RandomMouse(prismarine, mazeGrid, mazeLevel.GetTextForm(), teleportList, ref mazeCompleted, ref changeMoveCounter, currentMoveCount, flagSelector);
            Solve_HugLeftWall(prismarine, mazeLevel, ref mazeCompleted, ref changeMoveCounter, currentMoveCount, flagSelector);
            if (mazeCompleted) { dispatcher.Stop(); }
        }

        #endregion

        #region Create maze functions

        public void Create_Minotaur(Rectangle[,] mazeGrid)
        {
            Random rnd = new Random();
            char[] compassChars = { 'N', 'W', 'S', 'E' };
            
            for (int row = 0; row < 21; row++)
            {
                for (int col = 0; col < 21; col++)
                {
                    if (row == 0 || row == 20 || col == 0 || col == 20)
                    { mazeGrid[row, col].Fill = Brushes.Black; }
                }
            }    

            int startRow = 0, startCol = 0;
            char startCompass = compassChars[rnd.Next(4)];
            switch (startCompass)
            {
                case 'N':
                    startRow = 20;
                    startCol = 2 * rnd.Next(10) + 1;
                    break;

                case 'W':
                    startRow = 2 * rnd.Next(10) + 1;
                    startCol = 20;
                    break;

                case 'S':
                    startRow = 0;
                    startCol = 2 * rnd.Next(10) + 1;
                    break;

                case 'E':
                    startRow = 2 * rnd.Next(10) + 1;
                    startCol = 0;
                    break;
            }

            mazeGrid[startRow, startCol].Fill = Brushes.DarkGray;
        }

        #endregion

        #region [Play mazes] buttons

        private void ConfirmFlag_Click(object sender, EventArgs e, int mode, Player prismarine, MazeLevel mazeLevel, TextBlock statsTitle, TextBlock statsTitleUnderlined, TextBlock recordNoOfMoves, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, Button solveThisMaze)
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

                ImportMaze(filePath, mazeLevel);
                DisplayMaze(mode, prismarine, mazeLevel);
            }
            
            else MessageBox.Show("Flag not selected!", "Error");
        }

        private void ResetFlagSelection_Click(object sender, EventArgs e, int mode, Player prismarine, MazeLevel mazeLevel, TextBlock statsTitle, TextBlock statsTitleUnderlined, TextBlock recordNoOfMoves, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, Button solveThisMaze)
        {
            flagSelector.SelectedIndex = -1;
            flagSelector.IsEnabled = true;
            confirmFlag.IsEnabled = true;

            prismarine.SetCanUseKeyboard(false);
            CreateMazeTemplate(mazeLevel);

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

        private void SolveThisMaze_Click(object sender, EventArgs e, Player prismarine, MazeLevel mazeLevel, TextBlock currentMoveCount, ComboBox flagSelector, Button confirmFlag, Button resetFlagSelection, Button solveThisMaze, Button devTools)
        {
            bool mazeCompleted = false;
            bool changeMoveCounter = false;

            DispatcherTimer dispatcher = new DispatcherTimer(DispatcherPriority.Send);
            dispatcher.Tick += (sender2, e2) => { DispatcherTimer_tick(sender2, e2, dispatcher, prismarine, mazeLevel, mazeCompleted, changeMoveCounter, currentMoveCount, flagSelector); };
            dispatcher.Interval = TimeSpan.FromMilliseconds(25); // change to 0 for instantaneous solve

            prismarine.SetCanUseKeyboard(false); // disables keyboard input to prevent user from interfering
            solveThisMaze.IsEnabled = false; // prevents the button from being clicked multiple times
            devTools.IsEnabled = false; // prevents the button from being clicked
            DisplayMaze(0, prismarine, mazeLevel); // resets the visual representation of the maze

            dispatcher.Start();
        }

        #endregion

        #region Developer tools

        private void DevTools_Click(object sender, EventArgs e, List<TextBlock> rowAxis, List<TextBlock> colAxis, Button movePlayer)
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

        private void MovePlayer_Click(object sender, EventArgs e, Player prismarine, MazeLevel mazeLevel, ComboBox flagSelector)
        {
            if (flagSelector.SelectedIndex > -1)
            {
                MovePlayer mp = new MovePlayer(prismarine, mazeLevel);
                mp.ShowDialog();
            }
        }

        public void MovePlayer(Player prismarine, MazeLevel mazeLevel, int row, int col)
        {
            mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.White;
            prismarine.SetYX(row, col, 0);
            mazeLevel.GetGrid()[prismarine.GetY(), prismarine.GetX()].Fill = Brushes.DarkViolet;
        }

        #endregion
    }
}

/* to do:
            implement method to change record number of moves
            make enemies appear in maze - red in colour, if you land on them before attacking them (press a key) you have to start again from the begining of the maze
            maze solver - automates finding the exit of the maze
            add timer
            add controls description
            add sound effects - voiced by courtney? "NO! SHOULD BE AT THE TOP OF THE LIST! GET RID OF THE QUESTION MARK!!!!!!! x"
            make default blank maze (first time starting up?) + black squares => the title of the game, display before main menu
            implement a way for people to store their name & scores in MySQL database or MongoDB database??? if not, then in notepad file
*/