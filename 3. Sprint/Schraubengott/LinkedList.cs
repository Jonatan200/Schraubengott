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

        public void AddNodToFront(int kundennr, String password, String name, String firma, String email, String plz, String str)
        {
            LinkedListElement node = new LinkedListElement(kundennr, password,name,firma,email,plz,str);
            node.next = head;
            head = node;
            count++;
        }
        public void AddNodToBack(int kundennr, String password, String name, String firma, String email, String plz, String str)
        {
            LinkedListElement node = new LinkedListElement(kundennr, password, name, firma, email, plz, str);

            LinkedListElement runner = head;
            if (runner == null)
            {
                head = node;
            }
            else
            {
                while (runner.next != null)
                {
                    runner = runner.next;
                }
                runner.next = node;
            }           
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
        public Boolean check(int nummer, String pass)
        {
            LinkedListElement runner = head;
            while (runner != null)
            {
                if (runner.kundennr == nummer&&runner.getpassword()==pass)
                {
                    return true;
                }
                runner = runner.next;
            }
            return false;
        }
        public String[] getdata(int nummer)
        {
            String[] data = new String[6];
            LinkedListElement runner = head;

            while (runner != null)
            {
                if (runner.kundennr == nummer)
                {
                    data[0] = runner.kundennr.ToString();
                    data[1] = runner.name;
                    data[2] = runner.firmenName;
                    data[3] = runner.adresse;
                    data[4] = runner.postleitzahl;
                    data[5] = runner.email;

                }
                runner = runner.next;
            }

            return data;
        }

    }
    class LinkedListElement
    {
        public int kundennr;
        private String password;

        public String name;
        public String firmenName;
        public String adresse;
        public String postleitzahl;
        public String email;

        public LinkedListElement next;

        public LinkedListElement(int kundennr, String password, String name, String firma, String email, String plz, String str)
        {
            this.kundennr = kundennr;
            this.password = password;
            this.name = name;
            this.firmenName = firma;
            this.email = email;
            this.adresse = str;
            this.postleitzahl = plz;

            next = null;
        }
        public String getpassword()
        {
            return this.password;
        }
    }
}
