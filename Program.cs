using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text; // Für Console.OutputEncoding

class Program
{
    // Speichere die Datei auf dem Desktop des Benutzers
    static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    static readonly string PairProbabilitiesFilePath = Path.Combine(DesktopPath, "pairProbabilities.txt");

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8; // Wichtig für korrekte Darstellung von Sonderzeichen

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("  Willkommen zum Spracherkennungs-Programm ");
        Console.WriteLine("========================================");
        Console.ResetColor();
        Console.WriteLine();

        Console.WriteLine("Bitte wählen Sie den Modus aus:");
        Console.WriteLine("  1 - Trainingsmodus (Wahrscheinlichkeiten aktualisieren)");
        Console.WriteLine("  2 - Testmodus (Sprache eines Textes bestimmen)");

        int modeSelection = GetValidatedInput(1, 2, "Geben Sie die entsprechende Zahl ein: ");
        bool trainingMode = modeSelection == 1;
        bool operationSuccessful = true;

        if (trainingMode)
        {
            operationSuccessful = TrainingMode();
        }
        else
        {
            operationSuccessful = TestMode();
        }

        Console.WriteLine();
        if (operationSuccessful)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("========================================");
            Console.WriteLine("        Operation erfolgreich         ");
            Console.WriteLine("       Analyse abgeschlossen!        ");
            Console.WriteLine("========================================");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("========================================");
            Console.WriteLine("      Operation nicht vollständig       ");
            Console.WriteLine("     oder mit Fehlern beendet.       ");
            Console.WriteLine("========================================");
        }
        Console.ResetColor();
        Console.WriteLine("\nDrücken Sie eine beliebige Taste zum Beenden...");
        Console.ReadKey();
    }

    static int GetValidatedInput(int min, int max, string prompt)
    {
        int selection;
        Console.Write(prompt);
        while (!int.TryParse(Console.ReadLine(), out selection) || selection < min || selection > max)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Ungültige Eingabe. Bitte geben Sie eine Zahl zwischen {min} und {max} ein.");
            Console.ResetColor();
            Console.Write(prompt);
        }
        return selection;
    }

    static bool TrainingMode()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n--- Trainingsmodus ---");
        Console.ResetColor();

        EnsurePairProbabilitiesFileExists();
        var pairProbabilities = ReadPairProbabilitiesFromFile();

        Console.WriteLine("Bitte wählen Sie die Sprache für das Training:");
        Console.WriteLine("  1 - Englisch");
        Console.WriteLine("  2 - Deutsch");
        int langChoiceNum = GetValidatedInput(1, 2, "Ihre Wahl: ");

        string selectedLanguage = (langChoiceNum == 1) ? "englisch" : "deutsch";
        Console.WriteLine($"Sprache für Training ausgewählt: {selectedLanguage}");

        Console.WriteLine("Bitte geben Sie den Text ein, den Sie für das Training verwenden möchten:");
        string text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Kein Text eingegeben. Training wird abgebrochen.");
            Console.ResetColor();
            return false;
        }

        var pairFrequencies = ExtractPairFrequencies(text);
        if (pairFrequencies.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Im eingegebenen Text konnten keine Buchstabenpaare zur Analyse gefunden werden.");
            Console.WriteLine("(Möglicherweise war der Text zu kurz oder enthielt keine Buchstaben.)");
            Console.ResetColor();
            return false; // Kein Training möglich, wenn keine Paare extrahiert wurden
        }

        UpdatePairProbabilities(pairFrequencies, selectedLanguage, pairProbabilities);
        WritePairProbabilitiesToFile(pairProbabilities);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Training abgeschlossen. Die Wahrscheinlichkeiten wurden aktualisiert und gespeichert.");
        Console.ResetColor();
        return true;
    }

    static bool TestMode()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n--- Testmodus ---");
        Console.ResetColor();

        var pairProbabilities = ReadPairProbabilitiesFromFile();

        if (pairProbabilities.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Es konnten keine Sprachdaten (Wahrscheinlichkeiten) gefunden werden.");
            Console.WriteLine("Bitte führen Sie zuerst den Trainingsmodus aus, um Daten zu generieren.");
            Console.ResetColor();
            return false;
        }

        Console.WriteLine("Bitte geben Sie den Text ein, den Sie analysieren möchten:");
        string text = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(text))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Kein Text eingegeben. Testmodus wird abgebrochen.");
            Console.ResetColor();
            return false;
        }

        var pairFrequencies = ExtractPairFrequencies(text);
        if (pairFrequencies.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Im eingegebenen Text konnten keine Buchstabenpaare zur Analyse gefunden werden.");
            Console.ResetColor();
            return false;
        }

        string detectedLanguage = DetectLanguage(pairProbabilities, pairFrequencies);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nErgebnis der Analyse:");
        Console.WriteLine($"Die vermutete Sprache des Textes ist: {detectedLanguage}");
        Console.ResetColor();
        return true;
    }

    static Dictionary<string, int> ExtractPairFrequencies(string text)
    {
        var frequencies = new Dictionary<string, int>();
        // Nur Buchstaben behalten und in Kleinbuchstaben umwandeln
        text = new string(text.Where(char.IsLetter).ToArray()).ToLower();

        if (text.Length < 2)
        {
            return frequencies; // Nicht genug Zeichen für Paare
        }

        for (int i = 0; i < text.Length - 1; i++)
        {
            string pair = text.Substring(i, 2);
            if (frequencies.ContainsKey(pair))
                frequencies[pair]++;
            else
                frequencies[pair] = 1;
        }
        return frequencies;
    }

    static void UpdatePairProbabilities(Dictionary<string, int> frequencies, string language, Dictionary<string, Dictionary<string, double>> pairProbabilities)
    {
        if (frequencies.Count == 0) return;

        int totalPairsInText = frequencies.Values.Sum();
        if (totalPairsInText == 0) return;

        foreach (var entry in frequencies)
        {
            string digraph = entry.Key;
            double relativeFrequency = (double)entry.Value / totalPairsInText;

            if (!pairProbabilities.ContainsKey(digraph))
            {
                pairProbabilities[digraph] = new Dictionary<string, double>();
            }

            if (!pairProbabilities[digraph].ContainsKey(language))
            {
                pairProbabilities[digraph][language] = 0.0;
            }
            // Additive Aktualisierung der Wahrscheinlichkeit/Score
            pairProbabilities[digraph][language] += relativeFrequency;
        }
    }

    static void WritePairProbabilitiesToFile(Dictionary<string, Dictionary<string, double>> pairProbabilities)
    {
        try
        {
            var lines = pairProbabilities
                .OrderBy(digraphEntry => digraphEntry.Key) // Sortiere nach Digraph für konsistente Ausgabe
                .Select(digraphEntry =>
                {
                    var probabilitiesString = string.Join(", ", digraphEntry.Value
                        .OrderBy(langEntry => langEntry.Key) // Sortiere Sprachen für konsistente Ausgabe
                        .Select(kvp => $"{kvp.Key}={kvp.Value.ToString("0.00000", CultureInfo.InvariantCulture)}")); // 5 Dezimalstellen
                    return $"{digraphEntry.Key}: {probabilitiesString}";
                });

            File.WriteAllLines(PairProbabilitiesFilePath, lines);
        }
        catch (IOException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fehler beim Schreiben der Datei {PairProbabilitiesFilePath}: {ex.Message}");
            Console.ResetColor();
        }
    }

    static Dictionary<string, Dictionary<string, double>> ReadPairProbabilitiesFromFile()
    {
        var pairProbabilities = new Dictionary<string, Dictionary<string, double>>();
        if (!File.Exists(PairProbabilitiesFilePath))
        {
            // Die Datei wird ggf. im Trainingsmodus erstellt. Hier nur eine Info, falls sie fehlt.
            Console.WriteLine($"Hinweis: Die Datei {Path.GetFileName(PairProbabilitiesFilePath)} wurde nicht gefunden. Im Trainingsmodus wird sie erstellt.");
            return pairProbabilities;
        }

        try
        {
            var lines = File.ReadAllLines(PairProbabilitiesFilePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string digraph = parts[0].Trim();
                    var langProbs = new Dictionary<string, double>();
                    var probabilityKeyValuePairs = parts[1].Split(',');

                    foreach (var p_kvp_str in probabilityKeyValuePairs)
                    {
                        var kvp_parts = p_kvp_str.Split('=');
                        if (kvp_parts.Length == 2)
                        {
                            string lang = kvp_parts[0].Trim();
                            // NumberStyles.Any erlaubt führende/folgende Leerzeichen, Dezimalpunkt, etc.
                            if (double.TryParse(kvp_parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double prob))
                            {
                                langProbs[lang] = prob;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Warnung: Ungültiger Wahrscheinlichkeitswert in Zeile: '{line}' für Paar '{p_kvp_str}'. Wird ignoriert.");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Warnung: Ungültiges Sprach-Wahrscheinlichkeits-Format in Zeile: '{line}' für Teil '{p_kvp_str}'. Wird ignoriert.");
                            Console.ResetColor();
                        }
                    }
                    if (langProbs.Any())
                    {
                        pairProbabilities[digraph] = langProbs;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Warnung: Ungültiges Zeilenformat in {Path.GetFileName(PairProbabilitiesFilePath)}: '{line}'. Wird ignoriert.");
                    Console.ResetColor();
                }
            }
        }
        catch (IOException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fehler beim Lesen der Datei {PairProbabilitiesFilePath}: {ex.Message}");
            Console.ResetColor();
            return new Dictionary<string, Dictionary<string, double>>(); // Im Fehlerfall mit leeren Daten starten
        }
        catch (Exception ex) // Für andere unerwartete Fehler beim Parsen
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Allgemeiner Fehler beim Verarbeiten der Datei {PairProbabilitiesFilePath}: {ex.Message}");
            Console.ResetColor();
            return new Dictionary<string, Dictionary<string, double>>();
        }
        return pairProbabilities;
    }

    static void EnsurePairProbabilitiesFileExists()
    {
        if (!File.Exists(PairProbabilitiesFilePath))
        {
            try
            {
                File.WriteAllText(PairProbabilitiesFilePath, ""); // Erstellt eine leere Datei
                Console.WriteLine($"Hinweis: Datei {Path.GetFileName(PairProbabilitiesFilePath)} wurde auf dem Desktop neu erstellt.");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Fehler: Datei {PairProbabilitiesFilePath} konnte nicht erstellt werden: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    static string DetectLanguage(Dictionary<string, Dictionary<string, double>> pairProbabilities, Dictionary<string, int> frequenciesInText)
    {
        if (pairProbabilities == null || pairProbabilities.Count == 0)
        {
            return "Unbekannt (Keine Trainingsdaten vorhanden)";
        }
        if (frequenciesInText == null || frequenciesInText.Count == 0)
        {
            return "Unbekannt (Keine analysierbaren Daten im Eingabetext)";
        }

        // Ermittle alle Sprachen, die in den Trainingsdaten vorkommen
        var knownLanguages = pairProbabilities.Values
                                .SelectMany(langDict => langDict.Keys)
                                .Distinct()
                                .ToList();

        if (!knownLanguages.Any())
        {
            return "Unbekannt (Trainingsdaten enthalten keine Sprachinformationen)";
        }

        int totalPairsInQueryText = frequenciesInText.Values.Sum();
        if (totalPairsInQueryText == 0) return "Unbekannt (Leere Eingabe nach Filterung)";

        var observedProbabilities = frequenciesInText.ToDictionary(
            kvp => kvp.Key,
            kvp => (double)kvp.Value / totalPairsInQueryText
        );

        double minDifference = double.MaxValue;
        string detectedLanguage = "Unbekannt (Keine Übereinstimmung)"; // Standard, falls keine Sprache passt

        foreach (var lang in knownLanguages)
        {
            double currentTotalDifference = 0.0;
            foreach (var observedPair in observedProbabilities)
            {
                double storedProbForPairAndLang = 0.0; // Standard, falls Paar/Sprache nicht in Trainingsdaten
                if (pairProbabilities.TryGetValue(observedPair.Key, out var langProbsForDigraph) &&
                    langProbsForDigraph.TryGetValue(lang, out var prob))
                {
                    storedProbForPairAndLang = prob;
                }
                currentTotalDifference += Math.Abs(observedPair.Value - storedProbForPairAndLang);
            }

            // Debug-Ausgabe (optional, kann bei Bedarf einkommentiert werden)
            // Console.WriteLine($"Debug: Sprache '{lang}', Gesamtdifferenz: {currentTotalDifference:F5}");

            if (currentTotalDifference < minDifference)
            {
                minDifference = currentTotalDifference;
                detectedLanguage = lang;
            }
        }
        return detectedLanguage;
    }
}