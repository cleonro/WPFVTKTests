using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Class1
    {
        public Class1(int m_q, int m_p)
        {
            this.m_q = m_q;
            this.m_p = m_p;
        }

        ~Class1()
        {

        }

        public void doSomething()
        {
            Console.WriteLine("Class1::doSomething");
        }
        public int parseInt(string arg)
        {
            int r = int.Parse(arg);
            return r;
        }

        private void comment(int x, int y)
        {
            if(x == y)
            {

            }
            else if(x != y)
            {

            }
        }

        private int m_q;
        private int m_p;

        
    }
}
