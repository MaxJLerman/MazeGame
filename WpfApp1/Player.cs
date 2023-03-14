using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
