﻿using System;
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
