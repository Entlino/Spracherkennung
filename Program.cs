using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Willkommen zur Textanalyse-Anwendung!");

            Console.WriteLine("Bitte geben Sie Ihren Text ein:");

            string userInput = Console.ReadLine().ToLower();

            Console.WriteLine("Sie haben folgenden Text eingegeben:");
            Console.WriteLine(userInput);

            string detectedLanguage = DetermineLanguage(userInput);

            Console.WriteLine($"Die erkannte Sprache ist: {detectedLanguage}");

            Console.WriteLine("Drücken Sie eine beliebige Taste zum Beenden.");
            Console.ReadKey();
        }

        static string DetermineLanguage(string text)
        {
            double[] englishFrequencies = { 8.167, 1.492, 2.782, 4.253, 12.702, 2.228, 2.015, 6.094, 6.966, 0.153, 0.772, 4.025, 2.406, 6.749, 7.507, 1.929, 0.095, 5.987, 6.327, 9.056, 2.758, 0.978, 2.360, 0.150, 1.974, 0.074 };
            double[] germanFrequencies = { 6.516, 1.886, 2.732, 5.076, 16.396, 1.656, 3.009, 4.577, 6.550, 0.268, 1.417, 3.437, 2.534, 9.776, 2.594, 0.670, 0.018, 7.003, 7.273, 6.154, 4.166, 0.846, 1.921, 0.034, 0.039, 1.134 };
            double[] frenchFrequencies = { 7.636, 0.901, 3.260, 3.669, 14.715, 1.066, 0.866, 0.737, 7.529, 0.613, 0.049, 5.456, 2.968, 7.095, 5.796, 2.521, 1.362, 6.693, 7.948, 7.244, 6.311, 1.838, 0.017, 0.427, 0.128, 0.326 };
            double[] italianFrequencies = { 11.745, 0.927, 4.501, 3.736, 11.792, 1.153, 1.644, 0.637, 10.143, 0.011, 0.009, 6.510, 2.512, 6.883, 9.832, 3.056, 0.505, 6.367, 4.981, 5.623, 3.011, 2.097, 0.033, 0.003, 0.020, 1.181 };
            double[] spanishFrequencies = { 11.525, 2.215, 4.019, 5.010, 12.181, 0.692, 1.768, 0.703, 6.247, 0.493, 0.011, 4.967, 3.157, 6.712, 8.683, 2.510, 0.877, 6.871, 7.977, 4.632, 2.927, 1.138, 0.017, 0.215, 1.008, 0.467 };


            double[] textFrequencies = new double[26];
            int totalLetters = 0;

            foreach (char c in text)
            {
                if (c >= 'a' && c <= 'z')
                {
                    textFrequencies[c - 'a']++;
                    totalLetters++;
                }
            }

            for (int i = 0; i < textFrequencies.Length; i++)
            {
                textFrequencies[i] = (textFrequencies[i] / totalLetters) * 100;
            }

            double englishDifference = CalculateDifference(textFrequencies, englishFrequencies);
            double germanDifference = CalculateDifference(textFrequencies, germanFrequencies);
            double frenchDifference = CalculateDifference(textFrequencies, frenchFrequencies);
            double italianDifference = CalculateDifference(textFrequencies, italianFrequencies);
            double spanishDifference = CalculateDifference(textFrequencies, spanishFrequencies);

            double minDifference = Math.Min(Math.Min(Math.Min(Math.Min(englishDifference, germanDifference), frenchDifference), italianDifference), spanishDifference);

            if (minDifference == englishDifference) return "Englisch";
            if (minDifference == germanDifference) return "Deutsch";
            if (minDifference == frenchDifference) return "Französisch";
            if (minDifference == italianDifference) return "Italienisch";
            return "Spanisch";
        }

        static double CalculateDifference(double[] textFrequencies, double[] referenceFrequencies)
        {
            double difference = 0.0;

            for (int i = 0; i < textFrequencies.Length; i++)
            {
                difference += Math.Abs(textFrequencies[i] - referenceFrequencies[i]);
            }

            return difference;
        }
    }
}
