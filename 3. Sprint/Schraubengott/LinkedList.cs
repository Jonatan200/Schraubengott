using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schraubengott
{
    class LinkedList
    {
        public int count = 0; //Countetr, um immer zu wissen, wie viele Elemente in der liste sind
        LinkedListElement head;

        public void AddNodToFront(int kundennr, String pass)
        {
            LinkedListElement node = new LinkedListElement(kundennr, pass);
            node.next = head;
            head = node;
            count++;
        }
        public void PrintList()// nur zum Testen, kommt später wech
        {

            LinkedListElement runner = head;
            while (runner != null)
            {
                //irgendeinlable.content=runner.kundennr;
                runner = runner.next;
            }
        }
        public Boolean Search(int nummer)
        {
            LinkedListElement runner = head;
            while (runner != null)
            {
                if (runner.kundennr == nummer)
                {
                    return true;
                }
                runner = runner.next;
            }
            return false;
        }



    }
    class LinkedListElement
    {
        public int kundennr;
        private String password;
        public LinkedListElement next;

        public LinkedListElement(int kundennr, String password)
        {
            this.kundennr = kundennr;
            this.password = password;
            next = null;
        }


    }
}
