using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace Schraubengott
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        Schraube[] feld = new Schraube[5];      //Array vom Typ Schraube erstellen
        int nr = 0;                             //Variable für den Index des Feldes Schraube
        int new_screw_int = 1;                  //Varibale für Neue Schraube Button und Combobox mit Schraubenauswahl

        //Zufällige Bestellnummer erstellen
        Random nummer = new Random();
        int bestellnummer;

        int kundennummer;
        int currentKdNr;
        
        LinkedList datenbank = new LinkedList(); //Kunendatenbank wird erstellt
       
        public MainWindow()
        {
            for (int i = 0; i < feld.Length; i++)
            {
                feld[i] = new Schraube();         // Array wird mit Objekten gefüllt
            }
            Schraube a = new Schraube();

            kundennummer = 1004; //Kundennr auf 1004 festlegen, weil schon 3 einträge existieren

            datenbank.AddNodToBack(1001, "hallo123", "Meyer", "hallo gmbh", "Meyer@hallogmbh.de", "23456", "Musterstraße 1"); // Datenbankeinträge
            datenbank.AddNodToBack(1002, "hallo456", "Meyer", "hallo gmbh", "Meyer@hallogmbh.de", "23456", "Musterstraße 1");
            datenbank.AddNodToBack(1003, "hallo789", "Meyer", "hallo gmbh", "Meyer@hallogmbh.de", "23456", "Musterstraße 1");

            InitializeComponent();
            cmb_nr.SelectedIndex = 0;           //Combobox hat von Anfang an die erste Schraube ausgewählt

            //Comboboxen werden von Anfange an auf Default gesetzt
            cbfk.SelectedIndex = 0;
            cbgewinde.SelectedIndex = 0;
            cbmat.SelectedIndex = 0;
            cbkopf.SelectedIndex = 0;

            // Bestellnummer 
            bestellnummer = nummer.Next(10000000, 99999999);
        }
      
        private void Btnexit_Click(object sender, RoutedEventArgs e)
        {
            //Wenn Exit-Button geklickt wird, wird gefragt, ob das Fenster geschlossen werden soll. Mit klick auf ja wird die App beendet
            if (MessageBox.Show("Das Fenster wirklich schließen?\nAlle Konfigurationen werden gelöscht!", "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                return;
            }
        }

        private void Cbmat_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {           
            //ComboBoxItems für Festigkeitsklasse werden abhängig von dem Material erstellt
            if (cbmat.SelectedValue.ToString() == "V2A")
            {
                cbfk.Items.Clear();
                cbfk.Items.Add("--Bitte auswählen--");
                cbfk.Items.Add("V2A 50");
                cbfk.Items.Add("V2A 70");
            }
            else if (cbmat.SelectedValue.ToString() == "V4A")
            {
                cbfk.Items.Clear();
                cbfk.Items.Add("--Bitte auswählen--");
                cbfk.Items.Add("V4A 70");
                cbfk.SelectedIndex = 1;
            }
            else if (cbmat.SelectedValue.ToString() == "Verzinkter Stahl")
            {
                cbfk.Items.Clear();
                cbfk.Items.Add("--Bitte auswählen--");
                cbfk.Items.Add("5.8");
                cbfk.Items.Add("6.8");
                cbfk.Items.Add("8.8");
                cbfk.Items.Add("9.8");
                cbfk.Items.Add("10.9");
                cbfk.Items.Add("12.9");
            }

            cbfk.SelectedIndex = 0; //Cobobox Index zurücksetzen
        }

        #region "Zurücksetzen der Leerfunktion von Textboxen"
        void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text == "0" || box.Text == "z.B Bitte in 50er Paketen versenden")
            {
                box.Text = string.Empty;
            }
        }
        void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text.Trim().Equals(string.Empty))
            {
                box.Text = "0";
            }
        }
       
        void TextBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text.Trim().Equals(string.Empty))
            {
                box.Text = "z.B Bitte in 50er Paketen versenden";
            }
        }
        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox a_textBox = (TextBox)sender;
            string a_newText = string.Empty;

            //In der Textbox können nur Zahlen eingegeben werden
            for (int i = 0; i < a_textBox.Text.Length; i++)
            {
                if (Regex.IsMatch(a_textBox.Text[i].ToString(), "^[0-9]+$"))
                {
                    a_newText += a_textBox.Text[i];
                }
            }

            a_textBox.Text = a_newText;
            a_textBox.SelectionStart = a_textBox.Text.Length;
        }

        private void Cbkopf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Grafik der Schraube ändern, abhängig von dem Schraubenkopf
            if (cbkopf.SelectedIndex == 1)
            {
                Image1.Visibility = Visibility.Visible;
                Image2.Visibility = Visibility.Collapsed;
            }
            else if (cbkopf.SelectedIndex == 2)
            {
                Image1.Visibility = Visibility.Collapsed;
                Image2.Visibility = Visibility.Visible;
            }
            else
            {
                Image1.Visibility = Visibility.Visible;
                Image2.Visibility = Visibility.Collapsed;
            }
        }

        private void Btnauswahl_Click(object sender, RoutedEventArgs e)
        {
            #region Fehlermeldung bei Falscheingaben"
            if (cbfk.SelectedIndex == 0)
            {
                MessageBox.Show("Es ist kein Eingabe für Festigkeit getätigt worden.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return; //Methode beenden, wenn keine Festigkeit angegeben
            }

            if (cbkopf.SelectedIndex == 0)
            {
                MessageBox.Show("Es ist kein Eingabe für Kopf getätigt worden.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return; //Methode beenden, wenn kein Typ angegeben
            }

            if (cbgewinde.SelectedIndex == 0)
            {
                MessageBox.Show("Es ist keine Eingabe für Gewinde getätigt worden.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return; //Methode beenden, wenn kein Gewinde angegeben
            }

            if (txt_len.Text == "" || txt_gewlen.Text == "")
            {
                MessageBox.Show("Für Gewindelänge und oder Länge liegt keine Eingabe vor.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;// wenn min eine Eingabe leer ist, wird die Methode beendet
            }
            else if (Convert.ToInt32(txt_len.Text) < 5 || Convert.ToInt32(txt_len.Text) > 150)
            {
                MessageBox.Show("Eingaben für Länge außerhalb des möglichen Wertebereichs.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;//wenn len kleiner als 5 oder größer als 150 wird Methode beendet
            }
            else if (Convert.ToInt32(txt_gewlen.Text) < 5 || Convert.ToInt32(txt_gewlen.Text) > 150)
            {
                MessageBox.Show("Eingaben für Gewindelänge außerhalb des möglichen Wertebereichs.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;//wenn Gewlen kleiner als 5 oder größer als 150 wird Methode beendet
            }
            else if (Convert.ToInt32(txt_len.Text) < Convert.ToInt32(txt_gewlen.Text))
            {
                MessageBox.Show("Eingaben für Länge und Gewindelänge sind nicht kompatibel.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;//wenn Gewlen größer als Len wird Methode beendet
            }            
      
            if (txt_menge.Text.ToString() == "" || txt_menge.Text.ToString() == "0")
            {
                MessageBox.Show("Es wurde keine Eingabe für die Menge gemacht.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return; //Methode beenden, wenn keine Menge angegeben
            }

            if (cbgewinde.SelectedValue.ToString() == "M4" && Convert.ToInt32(txt_gewlen.Text) > 100)
            {
                MessageBox.Show("Eingaben für Länge außerhalb des möglichen Wertebereichs.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;//wenn M4 und len größer als 100 wird Methode beendet
            }
            else if (cbgewinde.SelectedValue.ToString() == "M5" && Convert.ToInt32(txt_gewlen.Text) > 100)
            {
                MessageBox.Show("Eingaben für Länge außerhalb des möglichen Wertebereichs.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;//wenn M5 und len größer als 100 wird Methode beendet
            }
            #endregion

            #region "Ausgewählten Werte werden dem Objekt zugewiesen"
            if (gewartcheck.IsChecked == true)
            {
                feld[nr].gewindeart = "Feingewinde";
            }
            else if (gewartcheck.IsChecked == false)
            {
                feld[nr].gewindeart = "Standardgewinde";
            }

            feld[nr].menge = Convert.ToInt32(txt_menge.Text.ToString());
            feld[nr].typ = cbkopf.SelectedValue.ToString();
            feld[nr].gewinde = cbgewinde.SelectedValue.ToString();
            feld[nr].festigkeit = cbfk.SelectedItem.ToString();
            feld[nr].material = cbmat.SelectedValue.ToString();
            feld[nr].gewindelaenge = Convert.ToInt32(txt_gewlen.Text);
            feld[nr].laenge = Convert.ToInt32(txt_len.Text);
            feld[nr].bemerkung = text.Text.ToString();
            #endregion

            //Berechnungen für die ausgewählte Schraube           
            feld[nr].Berechnen();
            CreateDataTable();

            MessageBox.Show("Die aktuelle Konfiguration wurde in die Übersicht hinzugefügt.", "Konfiguration gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);

            //Schraubenauswahl für Catia sichtbar machen
            cbcatia.Visibility = Visibility.Visible;
            catialbl.Content = "Schraube auswählen\num Catia Part zu\nerstellen:";

            if (cmb_nr.SelectedIndex == 0)
            {
                cbcatia.Items.Remove("Schraube 1");
                cbcatia.Items.Add("Schraube 1");
            }
            if (cmb_nr.SelectedIndex == 1)
            {
                cbcatia.Items.Remove("Schraube 2"); 
                cbcatia.Items.Add("Schraube 2");
            }
            if (cmb_nr.SelectedIndex == 2)
            {
                cbcatia.Items.Remove("Schraube 3"); 
                cbcatia.Items.Add("Schraube 3");
            }
            if (cmb_nr.SelectedIndex == 3)
            {
                cbcatia.Items.Remove("Schraube 4"); 
                cbcatia.Items.Add("Schraube 4");
            }
            if (cmb_nr.SelectedIndex == 4)
            {
                cbcatia.Items.Remove("Schraube 5"); 
                cbcatia.Items.Add("Schraube 5");
            }

            //Übersichts Tab sichtbar machen
            tab_1.Visibility = Visibility.Visible;
            tabcontrol.SelectedIndex = 1;
        }

        private void Cmb_nr_SelectionChanged(object sender, SelectionChangedEventArgs e)//auswahl der Schraubennummer (Index vom Feld)
        {
            switch (cmb_nr.SelectedIndex)//Auswahl der Schraube
            {
                case 0:
                    nr = 0;
                    break;

                case 1:
                    nr = 1;
                    break;

                case 2:
                    nr = 2;
                    break;

                case 3:
                    nr = 3;
                    break;

                case 4:
                    nr = 4;
                    break;
            }

            #region "Die gespeicherten Werte des Objekts werden in den Comboboxen angezeigt"
            if (feld[nr].material == "Verzinkter Stahl")
            {
                cbmat.SelectedIndex = 1;

                switch (feld[nr].festigkeit)
                {
                    case "5.8":
                        cbfk.SelectedIndex = 1;
                        break;

                    case "6.8":
                        cbfk.SelectedIndex = 2;
                        break;

                    case "8.8":
                        cbfk.SelectedIndex = 3;
                        break;

                    case "9.8":
                        cbfk.SelectedIndex = 4;
                        break;

                    case "10.8":
                        cbfk.SelectedIndex = 5;
                        break;

                    case "12.8":
                        cbfk.SelectedIndex = 6;
                        break;
                }
            }
            else if (feld[nr].material == "V2A")
            {
                cbmat.SelectedIndex = 2;

                switch (feld[nr].festigkeit)
                {
                    case "V2A 50":
                        cbfk.SelectedIndex = 1;
                        break;

                    case "V2A 70":
                        cbfk.SelectedIndex = 2;
                        break;
                }
            }
            else if (feld[nr].material == "V4A")
            {
                cbmat.SelectedIndex = 3;
                cbfk.SelectedIndex = 1;
            }
            else
            {
                cbmat.SelectedIndex = 0;
                cbfk.SelectedIndex = 0;
            }

            switch (feld[nr].gewinde)
            {
                case null:
                    cbgewinde.SelectedIndex = 0;
                    break;

                case "M4":
                    cbgewinde.SelectedIndex = 1;
                    break;

                case "M5":
                    cbgewinde.SelectedIndex = 2;
                    break;

                case "M6":
                    cbgewinde.SelectedIndex = 3;
                    break;

                case "M8":
                    cbgewinde.SelectedIndex = 4;
                    break;

                case "M10":
                    cbgewinde.SelectedIndex = 5;
                    break;

                case "M12":
                    cbgewinde.SelectedIndex = 6;
                    break;

                case "M16":
                    cbgewinde.SelectedIndex = 7;
                    break;

                case "M20":
                    cbgewinde.SelectedIndex = 8;

                    break;
            }

            if (feld[nr].gewindeart == "Feingewinde")
            {
                gewartcheck.IsChecked = true;
            }
            else
            {
                gewartcheck.IsChecked = false;
            }
            if (feld[nr].typ == "Außensechskant")
            {
                cbkopf.SelectedIndex = 1;
                Image1.Visibility = Visibility.Visible;
                Image2.Visibility = Visibility.Collapsed;
            }
            else if (feld[nr].typ == "Innensechskant")
            {
                cbkopf.SelectedIndex = 2;
                Image1.Visibility = Visibility.Collapsed;
                Image2.Visibility = Visibility.Visible;
            }
            else
            {
                cbkopf.SelectedIndex = 0;
            }

            txt_gewlen.Text = feld[nr].gewindelaenge.ToString();
            txt_len.Text = feld[nr].laenge.ToString();
            txt_menge.Text = feld[nr].menge.ToString();
            #endregion
        }

        private void New_screw_Click(object sender, RoutedEventArgs e)
        {
            //neue Schraube wird erstellt und alle Auswahlen werden aus Default zurückgesetzt
            text.Text = "z.B Bitte in 50er Paketen versenden";
            cbfk.SelectedIndex = 0;
            cbgewinde.SelectedIndex = 0;
            cbkopf.SelectedIndex = 0;
            cbmat.SelectedIndex = 0;
            txt_gewlen.Text = "";
            txt_len.Text = "";
            txt_menge.Text = "";
            gewartcheck.IsChecked = false;

            //neue Schraube zum konfigurieren in der Combobox anzeigen
            switch (new_screw_int)
            {
                case 1:
                    screw2.Visibility = Visibility.Visible;
                    cmb_nr.SelectedItem = screw2;
                    new_screw_int++;
                    break;

                case 2:
                    screw3.Visibility = Visibility.Visible;
                    cmb_nr.SelectedItem = screw3;
                    new_screw_int++;
                    break;

                case 3:
                    screw4.Visibility = Visibility.Visible;
                    cmb_nr.SelectedItem = screw4;
                    new_screw_int++;
                    new_screw.Content = "letzte Schraube erstellen";
                    break;

                case 4:
                    screw5.Visibility = Visibility.Visible;
                    cmb_nr.SelectedItem = screw5;
                    new_screw_int++;
                    tab_konfig.Header = "5. Konfiguration";
                    new_screw.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void btnwarenkorb_Click(object sender, RoutedEventArgs e)
        {
            #region "Fehlermeldung bei Falscheingabe"
            if (check1.IsChecked == true && feld[0].material == null || check2.IsChecked == true && feld[1].material == null || check3.IsChecked == true && feld[2].material == null || check4.IsChecked == true && feld[3].material == null || check5.IsChecked == true && feld[4].material == null)
            {
                MessageBox.Show("Die Auswahl ist leer.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;//wenn die ausgewählte schraube leer ist, wird die Methode beendet
            }
            else if (check1.IsChecked == true || check2.IsChecked == true || check3.IsChecked == true || check4.IsChecked == true || check5.IsChecked == true)
            {
                MessageBox.Show("Auswahl wurde dem Warenkorb hinzugefügt.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                tab_2.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Es ist nichts ausgewählt.", "Fehlerhafte Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;//wenn keine Checkbox ausgewählt, wird die Methode beendet
            }
            #endregion

            #region "Die Werte des ausgewälten Objekts werden im Warenkorb gespeichert"
            CreateDataTable2(Summe());
            tab_2.Visibility = Visibility.Visible;
            tabcontrol.SelectedIndex = 2;
            #endregion
        }

        private void Btnexcel_Click(object sender, RoutedEventArgs e)
        {
            bool senden = false;
            ExelControl.ExelContoll_aufrufen(Feld_anpassen(feld), senden, bestellnummer, kundennummer.ToString()) ;
        }

        public void Btnangebot_Click(object sender, RoutedEventArgs e)
        {
            bool senden = true;

            ExelControl.ExelContoll_aufrufen(Feld_anpassen(feld), senden, bestellnummer, kundennummer.ToString());
            MessageBox.Show("Angebot wurde erfolgreich abgesendet!", "Bestellt", MessageBoxButton.OK);
        }

        private Schraube[] Feld_anpassen(Schraube[] feld)
        {
            Schraube[] newfeld = new Schraube[5];
            for (int s = 0; s < newfeld.Length; s++)
            {
                newfeld[s] = new Schraube();
            }

            if (check1.IsChecked == true)
            {
                newfeld[0] = feld[0];
            }

            if (check2.IsChecked == true)
            {
                newfeld[1] = feld[1];
            }

            if (check3.IsChecked == true)
            {
                newfeld[2] = feld[2];
            }

            if (check4.IsChecked == true)
            {
                newfeld[3] = feld[3];
            }

            if (check5.IsChecked == true)
            {
                newfeld[4] = feld[4];
            }

            return newfeld;
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Catia erstellen
        {
            string[] kundendaten = datenbank.getdata(currentKdNr);

            if (cbcatia.Visibility == Visibility.Collapsed || cbcatia.SelectedIndex == 0)
            {
                MessageBox.Show("Es ist keine Schraube ausgewählt.", "", MessageBoxButton.OK);
            }
            else
            {               
                switch (cbcatia.SelectedItem.ToString())
                {
                    case "Schraube 1":
                        CatiaControl.Catia_Starten(feld[0],bestellnummer,kundendaten);
                        break;
                    case "Schraube 2":
                        CatiaControl.Catia_Starten(feld[1],bestellnummer, kundendaten);
                        break;
                    case "Schraube 3":
                        CatiaControl.Catia_Starten(feld[2],bestellnummer,kundendaten);
                        break;
                    case "Schraube 4":
                        CatiaControl.Catia_Starten(feld[3],bestellnummer, kundendaten);
                        break;
                    case "Schraube 5":
                        CatiaControl.Catia_Starten(feld[4],bestellnummer, kundendaten);
                        break;
                }
            }
        }


        #region "login"
        private void login_Click(object sender, RoutedEventArgs e)  
        {
            if(txtkundennr.Text == "" || passwortbox.Password == "" || !datenbank.check(Convert.ToInt32(txtkundennr.Text), passwortbox.Password ))
            {
                MessageBox.Show("Die Kundennummer oder das Passwort ist falsch", "Falsche Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);               
            }
            else
            {
                //Login Fenster Collapsen und Konfigurationsfenster sichtbar machen
                gridlogin.Visibility = Visibility.Collapsed;
                logo1.Visibility = Visibility.Visible;
                tabcontrol.Visibility = Visibility.Visible;
                ausloggen.Visibility = Visibility.Visible;

                currentKdNr = Convert.ToInt32(txtkundennr.Text); //Kundennummer wird auf die aktuelle gesetzt
                txtkundennr.Clear();    //Textbox leeren
                passwortbox.Clear();    //Textbox leeren
            }
        }

        private void regrist_Click(object sender, RoutedEventArgs e)
        {
            gridlogin.Visibility = Visibility.Collapsed;
            gridregr.Visibility = Visibility.Visible;
            txtkundennr.Clear();    //Textbox leeren
            passwortbox.Clear();    //Textbox leeren
        }

        private void zurück_Click(object sender, RoutedEventArgs e)
        {
            gridlogin.Visibility = Visibility.Visible;
            gridregr.Visibility = Visibility.Collapsed;

            //Textboxen leeren
            txtname.Clear();
            txtfirma.Clear();
            txtplz.Clear();
            txtstrasse.Clear();
            txtemail.Clear();
            passwortbox2.Clear();
            passwortbox3.Clear();
        }

        private void kontoerstellen_Click(object sender, RoutedEventArgs e)
        {
            if (passwortbox2.Password!=passwortbox3.Password)
            {
                MessageBox.Show("Passwörter stimmen nicht überein.", "Passwort falsch", MessageBoxButton.OK, MessageBoxImage.Error);
                return;             //Wenn Passwörter sind nicht identisch, a gleichen Punkt wie vorher

            }
            if (txtname.Text == ""|| txtfirma.Text == "" || txtplz.Text == "" || txtstrasse.Text == "" || txtplz.Text == "" || txtemail.Text == "" || passwortbox2.Password == "" || passwortbox3.Password == "")
            {
                MessageBox.Show("Mindestens ein Feld wurde nicht ausgefüllt!", "Fehlende Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                String name = txtname.Text;   //Alle Eingaben werden in einem Listeneintrag der Liste hinzugefügt
                String firma = txtfirma.Text;
                String email = txtemail.Text;
                String plz = txtplz.Text;
                String str = txtstrasse.Text;
                String password = passwortbox2.Password;

                datenbank.AddNodToBack(kundennummer, password, name, firma, email, plz, str);
                               
                gridregr.Visibility = Visibility.Collapsed;
                logo1.Visibility = Visibility.Visible;
                tabcontrol.Visibility = Visibility.Visible;
                MessageBox.Show("Ihr Kundenkonto wurde angelegt.\n\nIhre Kundennummer:" + kundennummer, "Erfolgreich Registriert", MessageBoxButton.OK);

                kundennummer++;     //Kundennummer wird erst hier um eins erhöht, damit in der Messagebox die richtige Zahl steht
                                    //Kundennummer wird einfach immer erhöht, wenn ein neuer Eintrag in die Liste gemacht wurde

                ausloggen.Visibility = Visibility.Visible;
                //Textboxen leeren
                txtname.Clear();
                txtfirma.Clear();
                txtplz.Clear();
                txtstrasse.Clear();
                txtemail.Clear();
                passwortbox2.Clear();
                passwortbox3.Clear();
            }
        }


        private void ausloggen_Click(object sender, RoutedEventArgs e)
        {
            //Alle Einstellungen zurücksetzen
            tabcontrol.Visibility = Visibility.Collapsed;
            gridlogin.Visibility = Visibility.Visible;
            logo1.Visibility = Visibility.Collapsed;
            cbcatia.Visibility = Visibility.Collapsed;
            cbcatia.Items.Clear();
            cbcatia.Items.Add("--Bitte auswählen--");
            tab_1.Visibility = Visibility.Collapsed;
            tab_2.Visibility = Visibility.Collapsed;
            tabcontrol.SelectedIndex = 0;
            check1.IsChecked = false;
            check2.IsChecked = false;
            check3.IsChecked = false;
            check4.IsChecked = false;
            check5.IsChecked = false;
            cbfk.SelectedIndex = 0;
            cbgewinde.SelectedIndex = 0;
            cbkopf.SelectedIndex = 0;
            cbmat.SelectedIndex = 0;
            txt_gewlen.Text = "";
            txt_len.Text = "";
            txt_menge.Text = "";
            gewartcheck.IsChecked = false;
            cmb_nr.SelectedIndex = 0;
            screw2.Visibility = Visibility.Collapsed;
            screw3.Visibility = Visibility.Collapsed;
            screw4.Visibility = Visibility.Collapsed;
            screw5.Visibility = Visibility.Collapsed;
            new_screw_int = 1;

            for (int i = 0; i < feld.Length; i++)
            {
                feld[i] = new Schraube();         // Array wird mit Objekten gefüllt
            }
        }
        #endregion

        public void CreateDataTable()//Datatable wird erzeugt und zugewiesen
        {
            System.Data.DataTable dt = new DataTable("MyTable");
            dt.Columns.Add("Schraube", typeof(string));
            dt.Columns.Add("Material", typeof(string));
            dt.Columns.Add("Festigkeit", typeof(string));
            dt.Columns.Add("Kopf", typeof(string));
            dt.Columns.Add("Gewinde", typeof(string));
            dt.Columns.Add("Typ", typeof(string));
            dt.Columns.Add("Länge", typeof(int));
            dt.Columns.Add("Gewindelänge", typeof(int));
            dt.Columns.Add("Menge", typeof(int));
            for (int i = 0; i < feld.Length; i++)
            {
                dt.Rows.Add("Schraube "+ (i+1), feld[i].material, feld[i].festigkeit, feld[i].typ, feld[i].gewinde, feld[i].gewindeart, feld[i].laenge, feld[i].gewindelaenge, feld[i].menge);
            }
            Datagrid1.DataContext = dt;
        }
        
        public void CreateDataTable2(double summe)//Datatable2 wird erzeugt und zugewiesen
        {
            System.Data.DataTable dt2 = new DataTable("MyTable2");
            dt2.Columns.Add("Schraube", typeof(string));
            dt2.Columns.Add("Menge", typeof(int));
            dt2.Columns.Add("Gewicht in g", typeof(double));
            dt2.Columns.Add("Stückpreis in Euro", typeof(double));
            dt2.Columns.Add("Preis in Euro", typeof(double));

            if (check1.IsChecked == true)
            {
                dt2.Rows.Add("Schraube 1", feld[0].menge, Math.Round(feld[0].gesamtgewicht ,2), Math.Round(feld[0].stückpreis,2), Math.Round(feld[0].preis_summe, 2));
            }
            else if (check1.IsChecked == false)
            {
                dt2.Rows.Add("Schraube 1", 0,0,0,0);
            }

            if (check2.IsChecked == true)
            {
                dt2.Rows.Add("Schraube 2", feld[1].menge, Math.Round(feld[1].gesamtgewicht,2), Math.Round(feld[1].stückpreis, 2), Math.Round(feld[1].preis_summe, 2));
            }
            else if (check2.IsChecked == false)
            {
                dt2.Rows.Add("Schraube 2", 0, 0, 0, 0);
            }

            if (check3.IsChecked == true)
            {
                dt2.Rows.Add("Schraube 3", feld[2].menge, Math.Round(feld[2].gesamtgewicht, 2), Math.Round(feld[2].stückpreis, 2), Math.Round(feld[2].preis_summe, 2));
            }
            else if (check3.IsChecked == false)
            {
                dt2.Rows.Add("Schraube 3", 0, 0, 0, 0);
            }

            if (check4.IsChecked == true)
            {
                dt2.Rows.Add("Schraube 4", feld[3].menge, Math.Round(feld[3].gesamtgewicht,2), Math.Round(feld[3].stückpreis, 2), Math.Round(feld[3].preis_summe, 2));
            }
            else if (check4.IsChecked == false)
            {
                dt2.Rows.Add("Schraube 4", 0, 0, 0, 0);
            }

            if (check5.IsChecked == true)
            {
                dt2.Rows.Add("Schraube 5", feld[4].menge, Math.Round(feld[4].masse,2), Math.Round(feld[4].stückpreis,2), Math.Round(feld[4].preis_summe,2));
            }
            else if (check5.IsChecked == false)
            {
                dt2.Rows.Add("Schraube 5", 0, 0, 0, 0);
            }

            gesamtpreistxt.Text= Convert.ToString(Math.Round(summe,2));

            Datagrid2.DataContext = dt2;
        }

        public double Summe()
        {
            double summe = 0;
            for (int i = 0; i < feld.Length; i++)
            {
                if (feld[i].preis_summe != 0)
                {
                    summe = summe + feld[i].preis_summe ;
                }           
            }

            return summe;
        }
    }
}
