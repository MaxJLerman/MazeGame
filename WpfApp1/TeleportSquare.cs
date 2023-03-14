using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
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
}
