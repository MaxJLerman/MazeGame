using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class MazeLevel
    {
        private Canvas canvas;
        private Rectangle[,] grid;
        private char[,] textForm;
        private List<TeleportSquare> teleportList;
        private int recordNumberOfMoves;

        public MazeLevel(Canvas canvas, Rectangle[,] grid, char[,] textForm, List<TeleportSquare> teleportList, int recordNumberOfMoves)
        {
            this.canvas = canvas;
            this.grid = grid;
            this.textForm = textForm;
            this.teleportList = teleportList;
            this.recordNumberOfMoves = recordNumberOfMoves;
        }

        public Canvas GetCanvas() { return canvas; }

        public void SetCanvas(int height, int width, SolidColorBrush brushColour)
        {
            this.canvas.Height = height;
            this.canvas.Width = width;
            this.canvas.Background = brushColour;
        }
        public Rectangle[,] GetGrid() { return grid; }

        public char[,] GetTextForm() { return textForm; }

        public List<TeleportSquare> GetTeleportList() { return teleportList; }

        public int GetRecordNumberOfMoves() { return recordNumberOfMoves; }

        public void SetRecordNumberOfMoves(int recordNumberOfMoves) { this.recordNumberOfMoves = recordNumberOfMoves; }
    }
}
