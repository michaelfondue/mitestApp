using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cameratest
{
    public class Globals
    {
        private static Globals instance;

        // Global variable
        private string data;

        // Restrict the constructor from being instantiated
        private Globals() { }

        public void setData(string d)
        {
            this.data = d;
        }
        public string getData()
        {
            return this.data;
        }

        public static Globals getInstance()
        {
            if (instance == null)
            {
                instance = new Globals();
            }
            return instance;
        }
    }
}
