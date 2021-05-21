using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using INFITF;
using MECMOD;
using PARTITF;

namespace Schraubengott
{
    class CatiaConnection
    {
       
        INFITF.Application hsp_catiaApp;
        MECMOD.PartDocument hsp_catiaPart;
        Sketch sketch_zylinder;
        Sketch sketch_Kopf;
        HybridBody catHybridBody1;
       


        public bool CATIALaeuft()
        {
            try
            {
                object catiaObject = System.Runtime.InteropServices.Marshal.GetActiveObject(
                    "CATIA.Application");
                hsp_catiaApp = (INFITF.Application)catiaObject;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        internal void ErzeugePart()
        {
            Documents catDokument = hsp_catiaApp.Documents;

            //Datei erstellen
             hsp_catiaPart =(PartDocument) catDokument.Add("Part");
        }

        #region Gewindestange 
        internal void ErsteSkizzeErstellen()
        {
            HybridBodies catHybridbodys1 = hsp_catiaPart.Part.HybridBodies;
            
            try
            {
                catHybridBody1 = catHybridbodys1.Item("Geometrisches Set.1");
            }
            catch (Exception)
            {
               /* MessageBox.Show("Kein Geometrisches Set gefunden", "Fehler");MessageBoxButton.OK, MessageBoxImage.Error);
                */return;  
            }
            //Geometrisches Set Benennen 
            catHybridBody1.set_Name("Schraube Außensechskannt");

            //Skizze erstellen 
            Sketches catSketches = catHybridBody1.HybridSketches;

            // Ebene festlegen ? 
            OriginElements catoriginElements = hsp_catiaPart.Part.OriginElements;
            Reference catRef = (Reference) catoriginElements.PlaneYZ;
            
            //Zkizze in YZ Ebene hizufügen 
             sketch_zylinder = catSketches.Add(catRef);

            ErzeugeAchsensystem();

            hsp_catiaPart.Part.Update();
        }

        private void ErzeugeAchsensystem()
        {
            object[] arr = new object[] {0.0, 0.0, 0.0,
                                         0.0, 1.0, 0.0,
                                         0.0, 0.0, 1.0 };
            sketch_zylinder.SetAbsoluteAxisData(arr);
        }


        internal void ZkizzeZylinder(Schraube [] arr)
        {
            sketch_zylinder.set_Name("Zylinder");
            Factory2D catfactory2D = sketch_zylinder.OpenEdition();

            Circle2D Zylinder1 = catfactory2D.CreateClosedCircle(0, 0, arr[0].durchmesser);

            sketch_zylinder.CloseEdition();
            hsp_catiaPart.Part.Update();
        }


        internal void ErzeigeZylinder(Schraube [] arr)
        {
            hsp_catiaPart.Part.InWorkObject = hsp_catiaPart.Part.MainBody;
            ShapeFactory shapFac = (ShapeFactory) hsp_catiaPart.Part.ShapeFactory;

            Pad newPad = shapFac.AddNewPad(sketch_zylinder, Convert.ToDouble(arr[0].laenge));
            newPad.set_Name("Zylinder");
            hsp_catiaPart.Part.Update();
        }


        #endregion
        
        internal void ZweiteSkizzeErzeugen()
        {
            //Skizze erstellen 
            Sketches catSketches = catHybridBody1.HybridSketches;

            // Ebene festlegen ? 
            OriginElements catoriginElements = hsp_catiaPart.Part.OriginElements;
            Reference catRef = (Reference)catoriginElements.PlaneYZ;

            //Zkizze in YZ Ebene hizufügen 
            sketch_Kopf = catSketches.Add(catRef);


            hsp_catiaPart.Part.Update();
        }


        internal void ZkizzeKopf(Schraube [] arr)
        {
            double schlüsselweite = Convert.ToDouble(arr[0].schluesselbreite);
            double außenkreisSchraubenkopf = schlüsselweite / (Math.Sqrt(3) / 2);

            sketch_Kopf.set_Name("Kopf");
            Factory2D catfactory2D = sketch_Kopf.OpenEdition();

            Point2D point2D1 = catfactory2D.CreatePoint(0, 0);
            Point2D point2D2 = catfactory2D.CreatePoint(0, schlüsselweite);

            Circle2D circle2D1 = catfactory2D.CreateClosedCircle(0, 0, außenkreisSchraubenkopf);
            circle2D1.CenterPoint = point2D1;
            circle2D1.Construction = true;
            

            Circle2D circle2D2 = catfactory2D.CreateClosedCircle(0, 0, schlüsselweite);
            circle2D2.CenterPoint = point2D1;
            circle2D2.Construction = true;

            Point2D point2D3 = catfactory2D.CreatePoint(0.5 * außenkreisSchraubenkopf, schlüsselweite);
            Point2D point2D4 = catfactory2D.CreatePoint(-0.5 * außenkreisSchraubenkopf, schlüsselweite);
            Point2D point2D5 = catfactory2D.CreatePoint(-1*außenkreisSchraubenkopf,0);
            Point2D point2D6 = catfactory2D.CreatePoint(-0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            Point2D point2D7 = catfactory2D.CreatePoint(0.5 * außenkreisSchraubenkopf,-1 * schlüsselweite);
            Point2D point2D8 = catfactory2D.CreatePoint(außenkreisSchraubenkopf, -0);

            Line2D line2D3 = catfactory2D.CreateLine(0.5 * außenkreisSchraubenkopf, schlüsselweite, -0.5 * außenkreisSchraubenkopf, schlüsselweite);
            line2D3.StartPoint = point2D3;
            line2D3.EndPoint = point2D4;

            Line2D line2D4 = catfactory2D.CreateLine(-0.5 * außenkreisSchraubenkopf, schlüsselweite, -1 * außenkreisSchraubenkopf, 0);
            line2D4.StartPoint = point2D4;
            line2D4.EndPoint = point2D5;

            Line2D line2D5 = catfactory2D.CreateLine(-1 * außenkreisSchraubenkopf, 0, -0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            line2D5.StartPoint = point2D5;
            line2D5.EndPoint = point2D6;

            Line2D line2D6 = catfactory2D.CreateLine(-0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite, 0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            line2D6.StartPoint = point2D6;
            line2D6.EndPoint = point2D7;

            Line2D line2D7 = catfactory2D.CreateLine(0.5*außenkreisSchraubenkopf, -1 * schlüsselweite, außenkreisSchraubenkopf,0);
            line2D7.StartPoint = point2D7;
            line2D7.EndPoint = point2D8;

            Line2D line2D8 = catfactory2D.CreateLine(außenkreisSchraubenkopf, 0, 0.5 * außenkreisSchraubenkopf, schlüsselweite);
            line2D8.StartPoint = point2D8;
            line2D8.EndPoint = point2D3;

            Line2D line2D9 = catfactory2D.CreateLine(0, 0, 0, schlüsselweite);
            line2D9.Construction = true;
            line2D9.StartPoint = point2D1;
            line2D9.EndPoint = point2D2;

            Constraints Constraints1 = sketch_Kopf.Constraints;
            

            // Reference reference1 = hsp_catiaPart.
            //zeile 115 

            sketch_Kopf.CloseEdition();
            hsp_catiaPart.Part.Update();
        }

        internal void ErzeugeKopf(Schraube [] arr)
        {
            double schraubenkopfhöhe = 7;
            hsp_catiaPart.Part.InWorkObject = hsp_catiaPart.Part.MainBody;
            ShapeFactory shapFac = (ShapeFactory)hsp_catiaPart.Part.ShapeFactory;

            Pad pad_Kopf = shapFac.AddNewPad(sketch_Kopf, -schraubenkopfhöhe);
            pad_Kopf.set_Name("Kopf");
            hsp_catiaPart.Part.Update();
        }


        /*
           public Boolean ErzeugePart()
           {
               INFITF.Documents catDocuments1 = hsp_catiaApp.Documents;
               hsp_catiaPart = catDocuments1.Add("Part") as MECMOD.PartDocument;
               return true;
           }

           public void ErstelleLeereSkizze()
           {
               // geometrisches Set auswaehlen und umbenennen
               HybridBodies catHybridBodies1 = hsp_catiaPart.Part.HybridBodies;
               HybridBody catHybridBody1;
               try
               {
                   catHybridBody1 = catHybridBodies1.Item("Geometrisches Set.1");
               }
               catch (Exception)
               {
                   MessageBox.Show("Kein geometrisches Set gefunden! " + Environment.NewLine +
                       "Ein PART manuell erzeugen und ein darauf achten, dass 'Geometisches Set' aktiviert ist.",
                       "Fehler", MessageBoxButton.OK, MessageBoxImage.Information);
                   return;
               }
               catHybridBody1.set_Name("Profile");
               // neue Skizze im ausgewaehlten geometrischen Set anlegen
               Sketches catSketches1 = catHybridBody1.HybridSketches;
               OriginElements catOriginElements = hsp_catiaPart.Part.OriginElements;
               Reference catReference1 = (Reference)catOriginElements.PlaneYZ;
               hsp_catiaProfil = catSketches1.Add(catReference1);

               // Achsensystem in Skizze erstellen 
               ErzeugeAchsensystem();

               // Part aktualisieren
               hsp_catiaPart.Part.Update();
           }

           private void ErzeugeAchsensystem()
           {
               object[] arr = new object[] {0.0, 0.0, 0.0,
                                        0.0, 1.0, 0.0,
                                        0.0, 0.0, 1.0 };
               hsp_catiaProfil.SetAbsoluteAxisData(arr);
           }

           public void ErzeugeProfil(Double b, Double h)
           {
               // Skizze umbenennen
               hsp_catiaProfil.set_Name("Rechteck");

               // Rechteck in Skizze einzeichnen
               // Skizze oeffnen
               Factory2D catFactory2D1 = hsp_catiaProfil.OpenEdition();

               // Rechteck erzeugen

               // erst die Punkte
               Point2D catPoint2D1 = catFactory2D1.CreatePoint(-50, 50);
               Point2D catPoint2D2 = catFactory2D1.CreatePoint(50, 50);
               Point2D catPoint2D3 = catFactory2D1.CreatePoint(50, -50);
               Point2D catPoint2D4 = catFactory2D1.CreatePoint(-50, -50);

               // dann die Linien
               Line2D catLine2D1 = catFactory2D1.CreateLine(-50, 50, 50, 50);
               catLine2D1.StartPoint = catPoint2D1;
               catLine2D1.EndPoint = catPoint2D2;

               Line2D catLine2D2 = catFactory2D1.CreateLine(50, 50, 50, -50);
               catLine2D2.StartPoint = catPoint2D2;
               catLine2D2.EndPoint = catPoint2D3;

               Line2D catLine2D3 = catFactory2D1.CreateLine(50, -50, -50, -50);
               catLine2D3.StartPoint = catPoint2D3;
               catLine2D3.EndPoint = catPoint2D4;

               Line2D catLine2D4 = catFactory2D1.CreateLine(-50, -50, -50, 50);
               catLine2D4.StartPoint = catPoint2D4;
               catLine2D4.EndPoint = catPoint2D1;

               // Skizzierer verlassen
               hsp_catiaProfil.CloseEdition();
               // Part aktualisieren
               hsp_catiaPart.Part.Update();
           }

           public void ErzeugeBalken(Double l)
           {
               // Hauptkoerper in Bearbeitung definieren
               hsp_catiaPart.Part.InWorkObject = hsp_catiaPart.Part.MainBody;

               // Block(Balken) erzeugen
               ShapeFactory catShapeFactory1 = (ShapeFactory)hsp_catiaPart.Part.ShapeFactory;
               Pad catPad1 = catShapeFactory1.AddNewPad(hsp_catiaProfil, l);

               // Block umbenennen
               catPad1.set_Name("Balken");

               // Part aktualisieren
               hsp_catiaPart.Part.Update();
           }
       */
    }

}
