using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using INFITF;
using MECMOD;
using PARTITF;
using HybridShapeTypeLib;
using DRAFTINGITF;


namespace Schraubengott
{
    class CatiaConnection
    {
     
        INFITF.Application hsp_catiaApp;
        INFITF.Documents catDokuments1;
        MECMOD.PartDocument hsp_catiaPartDoc;
        MECMOD.Sketch hsp_catiaSkizze;

        ShapeFactory shapefac;
        HybridShapeFactory hybridshapefac;
        Pad schaft;
        Part part_Schraube;
        Sketches sketches_Schraube;
        Body body_Schraube;

        Sketch skizze_kopf;
        Sketch skizze_tasche;
        
        #region catiastart 
        public bool CATIALaeuft()
        {            
            try
            {
                //"Abfangen" der Laufenden CATIA 
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
            //Dokument erstellen
            catDokuments1 = hsp_catiaApp.Documents;

            //Part erstellen
             hsp_catiaPartDoc =(PartDocument) catDokuments1.Add("Part");
        }
        private void ErzeugeAchsensystem()
        {
            object[] arr = new object[] {0.0, 0.0, 0.0,
                                         0.0, 1.0, 0.0,
                                         0.0, 0.0, 1.0 };
            hsp_catiaSkizze.SetAbsoluteAxisData(arr);
        }
        #endregion

        #region Zylinder 
        internal void SkizzeZylinderErstellen()
        {
            shapefac = (ShapeFactory)hsp_catiaPartDoc.Part.ShapeFactory;
            HybridBodies catHybridbodys1 = hsp_catiaPartDoc.Part.HybridBodies;
            HybridBody catHybridBody1;
            try
            {
               // "Abfangen" des geometrischen Sets 
                catHybridBody1 = catHybridbodys1.Item("Geometrisches Set.1");
            }
            catch (Exception)
            {
               /* MessageBox.Show("Kein Geometrisches Set gefunden", "Fehler");MessageBoxButton.OK, MessageBoxImage.Error);
                */return;  
            }
            //Geometrisches Set Benennen 
            catHybridBody1.set_Name("Schraube Außensechskannt");

            //Typ Skizzen definieren 
             sketches_Schraube = catHybridBody1.HybridSketches;

            // Referenz für die Skizze festlgen  
            OriginElements catoriginElements = hsp_catiaPartDoc.Part.OriginElements;
            Reference catRef1 = (Reference)catoriginElements.PlaneYZ;
            
            //Skizze in YZ Ebene hizufügen 
             hsp_catiaSkizze = sketches_Schraube.Add(catRef1);

            ErzeugeAchsensystem();

            hsp_catiaPartDoc.Part.Update();
        }

        internal void SkizzeZylinder(Schraube arr)
        {
            part_Schraube = hsp_catiaPartDoc.Part;
            Bodies bodies = part_Schraube.Bodies;
            body_Schraube = part_Schraube.MainBody;

            part_Schraube.InWorkObject = part_Schraube.MainBody;
            
            // hsp_catiaPartDoc.Part.InWorkObject = hsp_catiaPartDoc.Part.MainBody;   eventuell überflüssig 
            hsp_catiaSkizze.set_Name("Zylinder");

            // Bearbeitungsumgebung der Skizze Öffnen 
            Factory2D catfactory2D1 = hsp_catiaSkizze.OpenEdition();

            Circle2D Zylinder1 = catfactory2D1.CreateClosedCircle(0, 0, 0.5 * arr.durchmesser);

            hsp_catiaSkizze.CloseEdition();
        }
      
        internal void ErzeugeZylinder(Schraube arr)
        {
            //Referenz für den Volumenkörper aus Skizze übernehmen 
            Reference RefMySchaft = part_Schraube.CreateReferenceFromObject(hsp_catiaSkizze);
            //Volumenkörper erstellen 
            schaft = shapefac.AddNewPadFromRef(RefMySchaft, Convert.ToDouble(arr.laenge));
            schaft.set_Name("Zylinder");
            hsp_catiaPartDoc.Part.Update();

            hsp_catiaPartDoc.Part.Update();
        }
        #endregion

        #region Gewinde
        internal void ErzeugeGewindeFeature(Schraube [] arr, int i)
        {
            //Referenzen für das Gewinde Festlegen
            Reference RefMantelfläche = part_Schraube.CreateReferenceFromBRepName("RSur:(Face:(Brp:(Pad.1;0:(Brp:(Sketch.1;1)));None:();Cf11:());WithTemporaryBody;WithoutBuildError;WithSelectingFeatureSupport;MFBRepVersion_CXR15)", schaft);
            Reference RefFrontfläche = part_Schraube.CreateReferenceFromBRepName("RSur:(Face:(Brp:(Pad.1;2);None:();Cf11:());WithTemporaryBody;WithoutBuildError;WithSelectingFeatureSupport;MFBRepVersion_CXR15)", schaft);

            //Gewinde Erzeugen, Parameter setzen 

            PARTITF.Thread thread1 = shapefac.AddNewThreadWithOutRef();
            thread1.Side = CatThreadSide.catRightSide;
            thread1.Diameter = arr[i].durchmesser;          //Gewindedurchmesser
            thread1.Depth = arr[i].gewindelaenge;           //Gewindelänge
            thread1.Pitch = arr[i].gewindesteigung;         //Gewindesteigung 
            thread1.LateralFaceElement = RefMantelfläche;   //Mantelfäache als Referenz 
            thread1.LimitFaceElement = RefFrontfläche;      //Startelement 

            part_Schraube.Update();
        }

        internal void ErzeugeFase()
        {
            part_Schraube.InWorkObject = part_Schraube;
          
            Reference reffase = (Reference)part_Schraube.CreateReferenceFromName("");

            Chamfer fase1 = shapefac.AddNewChamfer(reffase, CatChamferPropagation.catMinimalChamfer, CatChamferMode.catLengthAngleChamfer, CatChamferOrientation.catNoReverseChamfer, 1, 45);

            Bodies bodies1 = part_Schraube.Bodies;
            Body bodie1 = bodies1.Item("Hauptkörper");
            Shapes schapes1 = bodie1.Shapes;
            ConstRadEdgeFillet constRadEdgeFillet1 = (ConstRadEdgeFillet) schapes1.Item("Kanntenverundung");
            Reference reference2 = part_Schraube.CreateReferenceFromBRepName("REdge:(Edge:(Face:(Brp:(Pad.1;1);None:();Cf11:());Face:(Brp:(Pad.1;0:(Brp:(Sketch.1;1)));None:();Cf11:());None:(Limits1:();Limits2:());Cf11:());WithTemporaryBody;WithoutBuildError;WithSelectingFeatureSupport;MFBRepVersion_CXR15)", constRadEdgeFillet1);
            fase1.AddElementToChamfer(reference2);
            fase1.Mode = CatChamferMode.catLengthAngleChamfer;
            fase1.Propagation = CatChamferPropagation.catTangencyChamfer;
            fase1.Orientation = CatChamferOrientation.catNoReverseChamfer;

            part_Schraube.Update();           
        }
        
        internal void ErzeugeGewindehelix(Schraube arr)
        {
            hybridshapefac = (HybridShapeFactory) part_Schraube.HybridShapeFactory;

            // Skizze für Gewindeprofiel 
            Sketch gewinde = Gewindeskizze(arr);

            // "Rotationsachse" festlegen
            HybridShapeDirection HelixDir = hybridshapefac.AddNewDirectionByCoord(1, 0, 0);
            Reference RefHelxDir = part_Schraube.CreateReferenceFromObject(HelixDir);

            //Startpunkt festlegen 
            HybridShapePointCoord Helixstartpunkt = hybridshapefac.AddNewPointCoord(0, 0, 0.5 * arr.durchmesser);
            Reference RefHelixstartpunkt = part_Schraube.CreateReferenceFromObject(Helixstartpunkt);

            //Helix Erstellen 
            HybridShapeHelix Helix = hybridshapefac.AddNewHelix(RefHelxDir, false, RefHelixstartpunkt, arr.gewindesteigung, arr.gewindelaenge - 1, false, 0, 0, false);

            // Drehrichtung, Startpunkt             Steigung                Höhe             Drehrichtung  Anfangswinkel ...
            Reference RefHelix = part_Schraube.CreateReferenceFromObject(Helix);
            Reference RefGewinde = part_Schraube.CreateReferenceFromObject(gewinde);

            part_Schraube.Update();

            part_Schraube.InWorkObject = body_Schraube;

            OriginElements catoriginElements = this.part_Schraube.OriginElements;
            Reference RefPlanezx = (Reference) catoriginElements.PlaneZX;

            //Rille erzeugen 
            Slot GewindeRille = shapefac.AddNewSlotFromRef(RefGewinde, RefHelix);

            Reference RefmyPad = part_Schraube.CreateReferenceFromObject(schaft);
            HybridShapeSurfaceExplicit GewindestangenSurface = hybridshapefac.AddNewSurfaceDatum(RefmyPad);
            Reference RefGewindestangenSurface = part_Schraube.CreateReferenceFromObject(GewindestangenSurface);

            GewindeRille.ReferenceSurfaceElement = RefGewindestangenSurface;

            Reference RefGewindeRille = part_Schraube.CreateReferenceFromObject(GewindeRille);

            part_Schraube.Update();
        }
        
        private Sketch Gewindeskizze(Schraube arr)
        {
            // Referenzen für Skizze festlegen 
            OriginElements catoriginElements = part_Schraube.OriginElements;
            Reference RefPlanezx = (Reference)catoriginElements.PlaneZX;

            //Neue Skizze erstellen 
            Sketch Skizze_gewinde = sketches_Schraube.Add(RefPlanezx);
            part_Schraube.InWorkObject = Skizze_gewinde;
            Skizze_gewinde.set_Name("Gewinde");

            //Koordinaten Für gewindeSkizze berechnen 
            double zInnen = 0.5 * arr.kerndurchmesser + arr.gewinderundung - Math.Sin((30 * Math.PI) / 180) * arr.gewinderundung;
            double xInnen = Math.Cos((30 * Math.PI) / 180) * arr.gewinderundung;

            double zAußen = 0.5 * arr.durchmesser;
            double xAußen = 2 * 0.1875 * arr.gewindesteigung;
            
            double zRadius =0.5 * (arr.kerndurchmesser + arr.gewinderundung);
            double xRadius = 0;

            // Geometrie zeichnen 
            Factory2D catfactory2D2 = Skizze_gewinde.OpenEdition();

            Point2D geweindepunkt1 = catfactory2D2.CreatePoint(zInnen, -xInnen);
            Point2D geweindepunkt2 = catfactory2D2.CreatePoint(zInnen, xInnen);

            Point2D geweindepunkt3 = catfactory2D2.CreatePoint(zAußen, xAußen);
            Point2D geweindepunkt4 = catfactory2D2.CreatePoint(zAußen, -xAußen);

            Point2D geweindepunkt5 = catfactory2D2.CreatePoint(zRadius, xRadius);

            Line2D linieOben = catfactory2D2.CreateLine(zInnen, xInnen, zAußen, xAußen);
            linieOben.StartPoint = geweindepunkt2;
            linieOben.EndPoint = geweindepunkt3;

            Line2D linieAußen = catfactory2D2.CreateLine(zAußen, xAußen, zAußen, -xAußen);
            linieAußen.StartPoint = geweindepunkt3;
            linieAußen.EndPoint = geweindepunkt4;

            Line2D linieUnten = catfactory2D2.CreateLine(zAußen, -xAußen, zInnen, -xInnen);
            linieUnten.StartPoint = geweindepunkt4;
            linieUnten.EndPoint = geweindepunkt1;

            Circle2D gewindeRundung = catfactory2D2.CreateCircle(zRadius, xRadius, arr.gewinderundung, 0, 0);
            gewindeRundung.CenterPoint = geweindepunkt5;
            gewindeRundung.EndPoint = geweindepunkt1;
            gewindeRundung.StartPoint = geweindepunkt2;

            Skizze_gewinde.CloseEdition();

            return Skizze_gewinde;
        }
        #endregion

        #region Kopf
        internal void ErstelleSkizzeKopf(Schraube arr)
        {
            part_Schraube.InWorkObject = body_Schraube;

            //Referenz für Skizze (Auf oberseite des Zylinders) 
            OriginElements catoriginElements = hsp_catiaPartDoc.Part.OriginElements;
            Reference ref_Kopf = part_Schraube.CreateReferenceFromName("Selection_RSur:(Face:(Brp:(Pad.1;2);None:();Cf11:());Slot.1_ResultOUT;Z0;G8251)");
            
            // neue Skizze erstellen 
            skizze_kopf = sketches_Schraube.Add(ref_Kopf);
            skizze_kopf.set_Name("Kopf");
            // Ebene festlegen ? 
            
           hsp_catiaPartDoc.Part.Update();
        }

        internal void SkizzeKopf(Schraube arr)
        {
            //Skizze für den Kopf öffnen 
            part_Schraube.InWorkObject = skizze_kopf;
            skizze_kopf.set_Name("Kopf");
            Factory2D shapefac = skizze_kopf.OpenEdition();

            // je nach gewählten kopfty die richtige Geometrie Zeichnen     
            if (arr.typ == "Außensechskant")
            {
                SechseckZeichnen(arr, skizze_kopf);
            }
            else
            {
                Circle2D Zylinder1 = shapefac.CreateClosedCircle(0, 0, 0.5 * arr.kopfdurchmesser);
            }

            skizze_kopf.CloseEdition();
            hsp_catiaPartDoc.Part.Update();
        }

        internal void SechseckZeichnen(Schraube arr, Sketch skizze)
        {
            double schlüsselweite = 0.5 * Convert.ToDouble(arr.schluesselbreite);
            double außenkreisSchraubenkopf = schlüsselweite / (Math.Sqrt(3) / 2);
          
            Factory2D catfactory2D = skizze.OpenEdition();

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
            Point2D point2D5 = catfactory2D.CreatePoint(-1 * außenkreisSchraubenkopf, 0);
            Point2D point2D6 = catfactory2D.CreatePoint(-0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            Point2D point2D7 = catfactory2D.CreatePoint(0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
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

            Line2D line2D7 = catfactory2D.CreateLine(0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite, außenkreisSchraubenkopf, 0);
            line2D7.StartPoint = point2D7;
            line2D7.EndPoint = point2D8;

            Line2D line2D8 = catfactory2D.CreateLine(außenkreisSchraubenkopf, 0, 0.5 * außenkreisSchraubenkopf, schlüsselweite);
            line2D8.StartPoint = point2D8;
            line2D8.EndPoint = point2D3;

            Line2D line2D9 = catfactory2D.CreateLine(0, 0, 0, schlüsselweite);
            line2D9.Construction = true;
            line2D9.StartPoint = point2D1;
            line2D9.EndPoint = point2D2;
        }

        internal void ErzeugeKopf(Schraube arr)
        {
            hsp_catiaPartDoc.Part.InWorkObject = hsp_catiaPartDoc.Part.MainBody;
            ShapeFactory shapFac = (ShapeFactory)hsp_catiaPartDoc.Part.ShapeFactory;

            // Volumenkörper ertsellen 
            Pad pad_Kopf = shapFac.AddNewPad(skizze_kopf, arr.kopfhöhe);
            pad_Kopf.set_Name("Kopf");
            hsp_catiaPartDoc.Part.Update();
        }

        internal void SkizzeTasche(Schraube arr)
        {
            //Referenzen für Skizze festlegen (Auf Schraubenkopf) 
            part_Schraube.InWorkObject = body_Schraube;
            OriginElements catoriginElements = hsp_catiaPartDoc.Part.OriginElements;
            Reference ref_tasche = part_Schraube.CreateReferenceFromName("Selection_RSur:(Face:(Brp:(Pad.2;2);None:();Cf11:());Pad.2_ResultOUT;Z0;G8251)");
            //Skizze erzeugen 
            skizze_tasche = sketches_Schraube.Add(ref_tasche);
            skizze_tasche.set_Name("Tasche Innensechskannt");
           
            hsp_catiaPartDoc.Part.Update();
            
            // Sechseck auf Skizze Zeichnen 
            part_Schraube.InWorkObject = skizze_tasche;
            Factory2D shapefac2 = skizze_tasche.OpenEdition();

            SechseckZeichnen(arr,skizze_tasche);

            skizze_tasche.CloseEdition();
            hsp_catiaPartDoc.Part.Update();           
        }

        internal void TascheErzeugen(Schraube arr)
        {
            hsp_catiaPartDoc.Part.InWorkObject = hsp_catiaPartDoc.Part.MainBody;
            ShapeFactory shapFac2 = (ShapeFactory)hsp_catiaPartDoc.Part.ShapeFactory;
            // Tasche aus Skizze erzeugen 
            Pocket pocket_innensechskannt = shapFac2.AddNewPocket(skizze_tasche, 0.5 * arr.kopfhöhe);
            pocket_innensechskannt.set_Name("Innensechskannt");
            hsp_catiaPartDoc.Part.Update();
        }
        #endregion

        internal void Zeichnungsableitung(Schraube arr, int bestellnummer, string[] kundendaten)
        {
            #region Erste Ansicht einfügen
            //Neues Dokument aus Vorlage erstellen 
            Documents dokuments1 = hsp_catiaApp.Documents;
            string pfad = System.IO.Path.GetFullPath("Vorlage_HSP_Drawing.CATDrawing");
            //DrawingDocument drawingDokument1 = (DrawingDocument)dokuments1.NewFrom(@"C:\Users\jonat\Documents\GitHub\Schraubengott\3. Sprint\Schraubengott\Catia\Vorlage_HSP_Drawing.CATDrawing");
            
            //DrawingDocument drawingDokument1 = (DrawingDocument)dokuments1.NewFrom(pfad);
            DrawingDocument drawingDokument1 = (DrawingDocument)dokuments1.NewFrom(pfad);

            //Neues Zeichenblatt 
            DrawingSheets drawingSheets1 = drawingDokument1.Sheets;
            DrawingSheet drawingSheet1 = drawingSheets1.Item("A4_Zeichnungsrahmen");
            //Neue Zeichenansicht (Frontansicht)
            DrawingViews drawingViews1 = drawingSheet1.Views;
            DrawingView drawingView1 = drawingViews1.Add("AutomaticNaming");
            DrawingViewGenerativeLinks drawingViewGenerativeLinks1 = drawingView1.GenerativeLinks;
            DrawingViewGenerativeBehavior drawingViewGenerativeBehavior1 = drawingView1.GenerativeBehavior;

            //Verbinden der Schraube mit der Zeichnung 
            PartDocument partDocument1 = (PartDocument) dokuments1.Item("Part1.CATPart");
            ProductStructureTypeLib.Product product1 = (ProductStructureTypeLib.Product) partDocument1.GetItem("Part1");
            drawingViewGenerativeBehavior1.Document =  product1;
            drawingViewGenerativeBehavior1.DefineFrontView(0, 0, 1, 1, 0, 0);
           // Positionierung der Ansicht auf Zeichenblatt 
            drawingView1.x = 105;
            drawingView1.y = 190;
            drawingView1.Scale = 1;

            drawingViewGenerativeBehavior1 = drawingView1.GenerativeBehavior;
            drawingViewGenerativeBehavior1.Update();
            drawingView1.Activate();
            #endregion

            // 2. Zeichenansicht erstellen (Draufsicht)
            DrawingView drawingView2 = drawingViews1.Add("AutomaticNaming");
            DrawingViewGenerativeBehavior drawingViewGenerativeBehavior2 = drawingView2.GenerativeBehavior;

            //Projektion der ersten Ansicht in Ansicht einfügen 
            drawingViewGenerativeBehavior2.DefineProjectionView(drawingViewGenerativeBehavior1, CatProjViewType.catTopView);
            DrawingViewGenerativeLinks drawingViewGenerativeLinks2 = (DrawingViewGenerativeLinks) drawingView2.GenerativeLinks;
            drawingViewGenerativeLinks1.CopyLinksTo(drawingViewGenerativeLinks2);

            //Position der 2. Ansicht
            drawingView2.x = 108;
            drawingView2.y = 90;
            drawingView2.Scale = 1;

            drawingViewGenerativeBehavior2 = drawingView2.GenerativeBehavior;
            drawingViewGenerativeBehavior2.Update();
            
            //2. Skizze fest an erster Skizze ausrichten 
            drawingView2.ReferenceView = drawingView1;
            drawingView2.AlignedWithReferenceView();

            #region Textfelder 
            DrawingView drawingView3 = drawingViews1.Add("Textfeld");
            DrawingViewGenerativeLinks drawingViewGenerativeLinks4 = drawingView3.GenerativeLinks;
            DrawingViewGenerativeBehavior drawingViewGenerativeBehavior3 = drawingView3.GenerativeBehavior;

            
            // Positionierung der Ansicht auf Zeichenblatt 
            drawingView3.x = 0;
            drawingView3.y = 0;
            drawingView1.Scale = 1;

            drawingViewGenerativeBehavior3 = drawingView3.GenerativeBehavior;
            drawingViewGenerativeBehavior3.Update();
            
            DrawingTexts texts1 = drawingView3.Texts;
            DrawingText text1 = texts1.Add(kundendaten[0], 44, 46.5);
            text1.SetFontSize(0, 0, 2.2);

            DrawingTexts texts2 = drawingView3.Texts;
            DrawingText text2 = texts2.Add(arr.typ, 141, 40.5);
            text2.SetFontSize(0, 0, 2.2);

            DrawingTexts texts3 = drawingView3.Texts;
            DrawingText text3 = texts2.Add(arr.material, 141, 36);
            text3.SetFontSize(0, 0, 2.2);

            DrawingTexts texts4 = drawingView3.Texts;
            DrawingText text4 = texts4.Add(bestellnummer.ToString(), 162, 26);
            text4.SetFontSize(0, 0, 3.5);

            string anschrift = kundendaten[2] + "\n" + kundendaten[3] + "\n" + kundendaten[4];

            DrawingTexts texts5 = drawingView3.Texts;
            DrawingText text5 = texts5.Add(anschrift, 21.5, 43);
            text5.SetFontSize(0, 0, 2.2);
            #endregion
        }
    }
}
