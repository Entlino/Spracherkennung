# Lern-Periode-5
23.4 bis 25.6.2024

(Website-Code: https://github.com/Entlino/Modul293/)

## Grob-Planung

1. Meine Noten befinden sich im akzeptablem Bereich, nebest einer leicht ungenügenden Note im Module 319 habe ich neben einer 4 alle Noten zwischen einer 5 und 6.
2. **VBV** Für den Verbesserungsvorschlag möchte ich in der letzten Lernperiode ausschliesslich an dem Projekt arbeiten und versuchen, auf Modularbeit zu verzichten.
3. Spracherkennung (Programm kann von einem Text die Sprache erkennen.) Das Programm soll eingereichte Texte Analysieren und von denen schliesslich die Sprache zu erkennen und als Antwort ausgeben soll.

## 03.5.2024
Heute habe ich damit begonnen mich mit meinem Projekt auseinander zusetzten. Dabei habe ich das Prinzip gelernt wie ich das ganze umsetzen möchte. Und zwar suche ich nach Texten in der Sprache die erkannt werden soll, lass mir daraus eine Wahrscheinlichkeit berechnen wie oft jeder Buchstabe vorkommt, und mithilfe dieser Wahrscheinlichkeit habe ich den Vorteil das es nicht all zu kompliziert ist Beispielsweise Schweizer Dialekte zu erkennen. (67)

## 17.5.2024

- [ ] Buchstabenwiederholungsdaten, für die Sprachen für Deutsch, Englisch, Französisch. (Texte finden und dann analysieren lassen.)
- [ ] Buchstabenwiederholungsdaten, für Berner Dialekt. (Texte finden und dann analysieren lassen.)
- [ ] Buchstabenwiederholungsdaten, für Zürcher Dialekt. (Texte finden und dann analysieren lassen.)
- [ ] Buchstabenwiederholungsdaten, für Walliser Dialekt. (Texte finden und dann analysieren lassen.)

Am heutigen Tag habe ich damit begonnen, zuerst die Aufträge, welche ich mir vorgenommen habe, zu erledigen. Da ich diese nach etwa 50 % der Zeit erledigen konnte, da ich schneller die Wahrscheinlichkeiten der verschiedenen Dialekten analysieren lassen konnte, habe ich danach weiter an meiner Website programmiert, welche ich gerade für das Modul 293. In Bezug auf die Website kam mir noch der Gedanke, dass es schliesslich praktisch wäre, wenn man das Spracherkennungsprogramm auf der Website einbinden kann, damit man das Tool auch auf dem Web brauchen könnte. Dies ist jedoch nur eine mögliche Erweiterung meines Projektes und bisher nicht sicher.

https://entlino.github.io/Modul293/ 
Meine Website


## 24.05.2024

- [x] Grundanwendung erstellen für meinen Spracherkenner.
- [x] Deutsche Sprache inkl. Daten einfügen, testen und evt verbessern.
- [x] Französische Sprache inkl. Daten einfügen, testen und evt verbessern.
- [x] Englische Sprache inkl. Daten einfügen, testen und evt verbessern.


Heute habe ich, wie in den Arbeitspaketen beschrieben, damit begonnen, das C# Programm zu schreiben. Dabei habe ich damit begonnen, das Programm zu erstellen, und erstmals die Sprachen Englisch, Französisch, Deutsch, Italienisch und Spanisch hinzugefügt. Da es leider noch gewisse Probleme mit dem Erkennen der Sprachen gibt, konnte ich bisher nicht damit fortfahren, die Dialekte hinzuzufügen. Dies werde ich, wenn möglich, nächste Woche erledigen, falls ich bis dann die Fehler beheben konnte. (72)

## 31.05.2024

- [ ] Fehlerbeheben damit die Texte erkannt werden.
- [ ] Programm mit verschiedene Texte testen.
- [x] Wiederholungen ins Programm einbauen.
- [x] Antworten, und eingabe Texte bearbeiten und erneuern. Ebenso den Code vorerst raus putzen und überarbeiten.

Am heutigen Tage bzw. in den letzten Tage als ich meine Arbeit erledigt habe habe ich erstmals versuch damit zu beginnen den Fehler zu beheben. Dies ist mir leider immer noch nicht gelungen. Als ich dann aus Frust erstmals weiter gegangen bin habe ich die letzteren Aufträge des Tages erfolgreich erledigen können. Da leider meine Motivation nicht mehr allzu vorhanden war habe ich mich dazu entschlossen, an meiner Website weiter zu programmieren. 

## 07.06.2024

- [ ] Website Modul 293 Kontakt formular einen Nutzen geben
- [ ] Website Modul 293 Kontakt formular einen Nutzen geben 2
- [ ] Navigationsprobleme beheben
- [ ] Website mit pseudo Texten sowie Bildern anschaulicher Gestalten

Nach dem heutigen Einzelgespräch habe ich damit begonnen, meine Arbeitspakete leicht anzupassen. Ich habe die Funfacts Seite auf meiner Webseite angepasst, in dem ich das Ganze in eine "Ol" eingefügt habe und nun darüber das CSS-File zu bearbeiten. Da dies den Code um einiges aufräumt. Zudem habe ich am Ende noch versucht, das ganze mit Variablen noch besser zu verbessern; dies gelang mir aber nicht vollständig.

## 14.06.2024

- [ ] HTML Liste (ol) mihilfe von nummerierenungen im CSS automatisch ansteuern
- [ ] Kontakt fomular automatisisieren
- [ ] Systemhinter Doppelbuchstabenwiederholungsabfrage finden
- [ ] Die Wiederholungen werden automatisch in einer Datei gespeichert und am Anfang eingelesen.


Am heutigen Tage habe ich an meinen beiden Projekten gearbeitet, zuerst habe ich mich am Anfang mit meiner Website den Text automatisch Links und Rechts anbinden lassen. Vorerst ging dies nicht, aber irgendwann konnte ich das ganze, ohne Veränderungen vorzunehmen, neu starten und dann ging alles wie gewollt. Das Arbeitspaket mit dem Kontakt-Formular habe ich nicht erledigt, da ich noch ein Gespräch mir Herrn Colic hatte, nach welchem ich mich direkt wieder ans C#-Programm und somit 2te Projekt gewandt habe. Bei welchem ich nun ein neues Ziel verfolge. Jetzt möchte ich nämlich, dass nicht nur auf die Buchstabenwahrscheinlichkeiten gesetzt wird, sondern dass man auf einen Doppelbuchstabenwahrscheinlichkeitsrechner setzt. 

Der Code für meine Website befindet sich in meinem Git "Modul293".

Folgend mein C# Code:

Als Erstes wird ein etwas schöneres "UI" ausgegeben und der Ablauf des Programms wird mit den Variablen bestimmt.


```csharp
        string pairFrequenciesFilePath = "pairFrequencies.txt";
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("========================================");
        Console.WriteLine("   Willkommen zum Textanalyse-Programm  ");
        Console.WriteLine("========================================");
        Console.ResetColor();

        Console.WriteLine("Bitte geben Sie den Text ein, den Sie analysieren möchten:");
        string text = Console.ReadLine();

        Dictionary<string, int> pairFrequencies = ReadPairFrequenciesFromFile(pairFrequenciesFilePath);

        UpdatePairFrequencies(pairFrequencies, text);

        WritePairFrequenciesToFile(pairFrequencies, pairFrequenciesFilePath);

        PrintPairProbabilities(pairFrequencies);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("========================================");
        Console.WriteLine("       Analyse abgeschlossen!           ");
        Console.WriteLine("========================================");
        Console.ResetColor();

```

Dann wird in der ersten Variablen erstmals die bereits gespeicherten Daten eingelesen.

```csharp
    static Dictionary<string, int> ReadPairFrequenciesFromFile(string filePath)
    {
        var frequencies = new Dictionary<string, int>();

        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    frequencies[parts[0]] = int.Parse(parts[1]);
                }
            }
        }

        return frequencies;
    }
```

Dann werden die Wahrscheinlichkeiten, welche gerade eingelesen wurde aktualisiert, aber noch im Array gespeichert.
```csharp
    static void UpdatePairFrequencies(Dictionary<string, int> frequencies, string text)
    {
        text = new string(text.Where(char.IsLetter).ToArray()).ToLower();

        for (int i = 0; i < text.Length - 1; i++)
        {
            string pair = text.Substring(i, 2);
            if (frequencies.ContainsKey(pair))
            {
                frequencies[pair]++;
            }
            else
            {
                frequencies[pair] = 1;
            }
        }
    }

```


Der vorher generierte und gedatete Array wird nun in das File übertragen, um beim nächsten Mal den aktuellsten Stand zu haben.
```csharp
    static void WritePairFrequenciesToFile(Dictionary<string, int> frequencies, string filePath)
    {
        var lines = frequencies.Select(kvp => $"{kvp.Key},{kvp.Value}");
        File.WriteAllLines(filePath, lines);
    }
```


Schlussendlich gibt mir nun das Programm die gerade errechneten Wahrscheinlichkeiten aus, dies würde ich in Zukunft noch weglassen jedoch habe ich es aktuell noch beibehalten, um zu überprüfen, ob das Ganze funktioniert.
```csharp
    static void PrintPairProbabilities(Dictionary<string, int> frequencies)
    {
        int total = frequencies.Values.Sum();
        Console.WriteLine("Buchstabenpaar-Wahrscheinlichkeiten:");
        Console.WriteLine("------------------------------------");

        foreach (var kvp in frequencies)
        {
            double probability = (double)kvp.Value / total;
            Console.WriteLine($"{kvp.Key}: {probability:P2}");
        }

        Console.WriteLine("------------------------------------");
    }
```

(461 Wörter)

## Arbeitspakete 21.06.2024


- [ ] C# Programm für Deutsch einrichten
- [ ] C# Programm für Englisch einrichten
- [ ] C# Programm für Franzöisch einrichten
- [ ] C# Programm Sprache ausgeben + Wahrscheinlichkeiten verstecken

Am heutigen Tag bin ich leider nicht so weit gekommen, wie ich gewünscht hätte, zum einen brauchte ich sehr viel Zeit, um mein Programm umzuschreiben, da ich inmitten eines Tipps von Herrn Colic nachkommen wollte, durch welchen ich schliesslich das Programm hätte einfacher schreiben könnten, dadurch das ich einen Trainingsmodus direkt ins richtige Programm zu implementieren, da ich so Fehlverbesserungen der Daten vermeiden könnte. Da dies nun jedoch so lange gedauert hat und ich nicht mehr weiß, wieso das Programm nicht mehr funktioniert, bin ich bedauerlicherweise bis jetzt nicht fertig geworden. (91)


## Arbeitspakete 28.06.2024

- [ ] Programm erkennt den Text im Trainingsmodus und kann die Daten im Textfile abspeichern.
- [ ] Programm kann im Testmodus zwischen Deutsch und Englisch unterscheiden.
