

# Schraubengott

Das von Griffindor It-Solutions entwickelte Schraubendimensionierungsprogramm soll einen einfachen Dimensionierungs- und Bestellvorgang verschiedenster Schrauben ermöglichen.

Das vorliegende Programm bietet dabei eine solide Grundlage für ein anwenderfreundliches Programm, mit einem breiten Spektrum an wichtigen Funktionen, die eine praktische Nutzung möglich machen sollen.
So schützt beispielsweise ein mit einer Liste aller Kunden verknüpftes Login-Fenster vor unauthorisierten Zugriffen. Zudem ermöglicht ein E-Mailclient das direkte Versenden einer Bestellung.
Außerdem ermöglicht das Programm dem Kunden das Sichern der Schrauben, in Form einer automatisch gernerierten Excel oder als Catia-Modell, inclusive Zeichnungsableitung. 

Da es sich bei dem Programm um ein Prototypen handelt, ist der Umfang der Funktionen zurzeit eingeschränkt. Beispielsweise beschränken sich die konfigurierbaren Schrauben auf Außen- bzw. Innensechskant Schrauben mit Metrischem Gewinde. Außerdem können derzeit maximal 5 Schrauben gleichzeitig erstellt werden. Zudem fehlen kundenspeziefische Elemente, wie eine Online-Datenbank, in der die Zugangsdaten zum Einloggen gespeichert werden können. 



## Aufbau und Nutzung des Programmes 

Das Programm ist grundlegend aus drei Ansichten aufgebaut. Nach dem Einloggen öffnet sich standardmäßig eine Konfigurationsseite, auf der Schrauben durch Auswählen der gewünschten Parameter erstellt werden können. 
Standardmäßig ist die 1. Schraube ausgewählt. Um eine weitere Schraube zu konfigurieren, muss diese zunächst über den Button "Neue Schraube erstellen" erzeugt werden. Anschließend wird diese Schraube automatisch ausgewählt. 
Um die Konfigurationen zu speichern muss der Button "Auswahl Speichern" betätigt werden. Nun werden die Daten der Schrauben auch auf der 2. Seite angezeigt. Um nachträglich eine Schraube zu verändern muss die jeweilige Schraube oberhalb des Konfigurators ausgewählt werden. Anschließend ist es wichtig, die Änderungen durch erneutes Drücken des Knopfes "Auswahl Speichern" zu sichern. 

Über die oben angezeigten Tabs kann man nun in die Übersicht wechseln, in der alle konfigurierten Schrauben aufgelistet sind. Durch setzen der Häkchen an den gewünschten Schrauben und betätigen des Knopfes " zum Warenkorb" können die Schrauben 
in den Warenkorb gelegt werden. Auch diese Auswahl kann nachträglich geändert werden. 

Der dritte Tab stellt den Warenkorb da. Hier werden alle ausgewählten Schrauben inklusive Preis und Gewicht angezeigt. Zudem kann von hier aus eine Excel-Übersicht generiert, das Angebot versendet oder das Catia-Modell erstellt werden. Um ein Catia-Modell zu erstellen muss zunächst die jeweilige Schraube ausgewählt werden, ehe über "Catia Erstellen" der Vorgang abgeschlossen werden kann. 

Um das Programm zu schließen kann entweder der Knopf "Exit" oder das Rote Kreuz in der oberen rechten Ecke genutzt werden. !! Achtung: durch Schließen des Programms werden alle Konfigurationen gelöscht !! 



## Nutzng des Software 
Um die Software einwandfrei nutzen zu können müssen einige Vorgaben gegeben sein 
	*	Die Nutzung des Emailclients setzt eine bestehende Verbindung zum Internet vorraus 
	*	Um die Catia-Anwendung nutzen zu können muss Catia V5 (getestet mit V5-6R2018) installiert und die Funktionen "Hybridkonstruktion ermöglichen" und "geometrisches Set erzeugen" aktiviert sein 



## Weiterentwicklung des Programmes 
 
Eine umfangreiche Weiterentwicklung des Programmes durch Griffindor IT-Solutions ist derzeit nicht geplant. Da das Programm über eine MIT-Lizenz lizensiert ist, ist allerdings jedem die Verwendung und Weiterentwicklung des Programms gestattet. 
Dabei auftretende Mängel oder durch das Programm entstehende Schäden liegen ncht in der Verantwortung von Griffindor IT-Solutions. 

Für die Catia-Api werden Folgende Catia-Bibliotheken benötigt:
	*	Interop.HybridShapeTypeLib
	*	Interop.INFITF
	*	Interop.MECMOD
	*	Interop.PARTITF
	*	Interop.DRAFTING
	*	Interop.ProductStructureTypeLib
