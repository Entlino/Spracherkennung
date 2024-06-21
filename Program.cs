using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string pairProbabilitiesFilePath = "pairProbabilities.txt";
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("========================================");
        Console.WriteLine("   Willkommen zum Spracherkennungs-Programm  ");
        Console.WriteLine("========================================");
        Console.ResetColor();

        Console.WriteLine("Bitte wählen Sie den Modus aus:");
        Console.WriteLine("1 - Trainingsmodus (Wahrscheinlichkeiten aktualisieren)");
        Console.WriteLine("2 - Testmodus (Wahrscheinlichkeiten nicht aktualisieren)");
        Console.Write("Geben Sie die entsprechende Zahl ein: ");

        int modeSelection;
        while (!int.TryParse(Console.ReadLine(), out modeSelection) || (modeSelection != 1 && modeSelection != 2))
        {
            Console.WriteLine("Ungültige Eingabe. Bitte geben Sie 1 für Trainingsmodus oder 2 für Testmodus ein.");
            Console.Write("Versuchen Sie es erneut: ");
        }

        bool trainingMode = modeSelection == 1;

        if (trainingMode)
        {
            TrainingMode(pairProbabilitiesFilePath);
        }
        else
        {
            TestMode(pairProbabilitiesFilePath);
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("========================================");
        Console.WriteLine("       Analyse abgeschlossen!           ");
        Console.WriteLine("========================================");
        Console.ResetColor();
    }

    static void TrainingMode(string pairProbabilitiesFilePath)
    {
        // Überprüfe, ob die Datei existiert; falls nicht, erstelle sie mit initialen Werten
        EnsurePairProbabilitiesFileExists(pairProbabilitiesFilePath);

        // Lese die initialen Wahrscheinlichkeiten aus der Datei
        Dictionary<string, Dictionary<string, double>> pairProbabilities = ReadPairProbabilitiesFromFile(pairProbabilitiesFilePath);

        Console.WriteLine("Bitte geben Sie die Sprache ein, für die Sie trainieren möchten (Englisch, Französisch, Deutsch):");
        string trainingLanguage = Console.ReadLine().Trim().ToLower();

        if (string.IsNullOrEmpty(trainingLanguage) ||
            (trainingLanguage != "englisch" && trainingLanguage != "französisch" && trainingLanguage != "deutsch"))
        {
            Console.WriteLine("Ungültige Eingabe. Nur Englisch, Französisch und Deutsch sind erlaubt.");
            return;
        }

        Console.WriteLine("Bitte geben Sie den Text ein, den Sie analysieren möchten:");
        string text = Console.ReadLine();

        // Extrahiere Paar-Frequenzen aus dem eingegebenen Text
        Dictionary<string, int> pairFrequencies = ExtractPairFrequencies(text);

        // Aktualisiere die Wahrscheinlichkeiten basierend auf den extrahierten Frequenzen und der ausgewählten Sprache
        UpdatePairProbabilities(pairFrequencies, trainingLanguage, pairProbabilities);

        // Schreibe die aktualisierten Wahrscheinlichkeiten zurück in die Datei
        WritePairProbabilitiesToFile(pairProbabilities, pairProbabilitiesFilePath);

        // Optional: Feedback an den Benutzer, dass das Training abgeschlossen ist
        Console.WriteLine("Training abgeschlossen. Die Wahrscheinlichkeiten wurden aktualisiert und gespeichert.");
    }

    static void TestMode(string pairProbabilitiesFilePath)
    {
        // Lese die aktuellen Wahrscheinlichkeiten aus der Datei
        Dictionary<string, Dictionary<string, double>> pairProbabilities = ReadPairProbabilitiesFromFile(pairProbabilitiesFilePath);

        if (pairProbabilities.Count == 0)
        {
            Console.WriteLine("Es konnten keine Sprachdaten gefunden werden. Bitte führen Sie zuerst den Trainingsmodus aus.");
            return;
        }

        Console.WriteLine("Bitte geben Sie den Text ein, den Sie analysieren möchten:");
        string text = Console.ReadLine();

        // Extrahiere Paar-Frequenzen aus dem eingegebenen Text
        Dictionary<string, int> pairFrequencies = ExtractPairFrequencies(text);

        // Erkenne die Sprache basierend auf den vorhandenen Wahrscheinlichkeiten
        string detectedLanguage = DetectLanguage(pairProbabilities, pairFrequencies);

        // Gib die erkannte Sprache aus
        Console.WriteLine($"Sprache erkannt als: {detectedLanguage}");
    }

    static Dictionary<string, int> ExtractPairFrequencies(string text)
    {
        var frequencies = new Dictionary<string, int>();

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

        return frequencies;
    }

    static void UpdatePairProbabilities(Dictionary<string, int> frequencies, string language, Dictionary<string, Dictionary<string, double>> pairProbabilities)
    {
        // Berechne die Gesamtsumme der Frequenzen
        int total = frequencies.Values.Sum();

        // Iteriere über die extrahierten Frequenzen und aktualisiere die Wahrscheinlichkeiten
        foreach (var pair in frequencies)
        {
            string digraph = pair.Key;
            int frequency = pair.Value;

            // Berechne die relative Häufigkeit dieses Digraphs
            double relativeFrequency = (double)frequency / total;

            // Aktualisiere die Wahrscheinlichkeiten für die ausgewählte Sprache in der Tabelle
            if (!pairProbabilities.ContainsKey(digraph))
            {
                pairProbabilities[digraph] = new Dictionary<string, double>
                {
                    { "englisch", 0.0 },
                    { "französisch", 0.0 },
                    { "deutsch", 0.0 }
                };
            }

            pairProbabilities[digraph][language] += relativeFrequency;
        }
    }

    static void WritePairProbabilitiesToFile(Dictionary<string, Dictionary<string, double>> pairProbabilities, string filePath)
    {
        List<string> lines = new List<string>();

        foreach (var digraph in pairProbabilities)
        {
            string line = $"{digraph.Key}: {string.Join(", ", digraph.Value.Select(kvp => kvp.Value.ToString("0.0000")))}";
            lines.Add(line);
        }

        File.WriteAllLines(filePath, lines);
    }

    static Dictionary<string, Dictionary<string, double>> ReadPairProbabilitiesFromFile(string filePath)
    {
        var pairProbabilities = new Dictionary<string, Dictionary<string, double>>();

        try
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string digraph = parts[0].Trim();
                        var probabilities = parts[1].Split(',').Select(p => double.Parse(p.Trim())).ToArray();

                        if (probabilities.Length >= 3) // Mindestens drei Wahrscheinlichkeiten (Englisch, Französisch, Deutsch)
                        {
                            pairProbabilities[digraph] = new Dictionary<string, double>
                            {
                                { "englisch", probabilities[0] },
                                { "französisch", probabilities[1] },
                                { "deutsch", probabilities[2] }
                                // Weitere Sprachen nach Bedarf hinzufügen
                            };
                        }
                        else
                        {
                            Console.WriteLine($"Ungültiges Format in der Datei: {filePath}, Zeile: {line}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ungültiges Format in der Datei: {filePath}, Zeile: {line}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Die Datei {filePath} existiert nicht.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Lesen der Datei {filePath}: {ex.Message}");
        }

        return pairProbabilities;
    }

    static void EnsurePairProbabilitiesFileExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            // Erstelle eine neue Datei und initialisiere sie mit leeren Paar-Wahrscheinlichkeiten
            File.WriteAllText(filePath, "aa: 0.0000, 0.0000, 0.0000\n" +
                                       "ab: 0.0000, 0.0000, 0.0000\n" +
                                       "ac: 0.0000, 0.0000, 0.0000\n" +
                                       // Füge weitere Digraphen nach Bedarf hinzu
                                       "zz: 0.0000, 0.0000, 0.0000\n");
        }
    }

    static string DetectLanguage(Dictionary<string, Dictionary<string, double>> pairProbabilities, Dictionary<string, int> frequencies)
    {
        int total = frequencies.Values.Sum();
        Dictionary<string, double> observedProbabilities = new Dictionary<string, double>();

        foreach (var kvp in frequencies)
        {
            double probability = (double)kvp.Value / total;
            observedProbabilities[kvp.Key] = probability;
        }

        double minDifference = double.MaxValue;
        string detectedLanguage = "Unbekannt";

        foreach (var lang in pairProbabilities.First().Value.Keys)
        {
            double difference = 0;

            foreach (var pair in observedProbabilities)
            {
                if (pairProbabilities.ContainsKey(pair.Key) && pairProbabilities[pair.Key].ContainsKey(lang))
                {
                    difference += Math.Abs(pair.Value - pairProbabilities[pair.Key][lang]);
                }
            }

            if (difference < minDifference)
            {
                minDifference = difference;
                detectedLanguage = lang;
            }
        }

        return detectedLanguage;
    }
}
