using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class LogicUtils
{
    public static int CalculateAccuracyPercentage(string expected, string recognized)
    {
        // Convert both strings to lowercase
        expected = expected.ToLower();
        recognized = recognized.ToLower();

        // Split recognized and expected strings into arrays of words
        string[] recognizedWords = recognized.Split(new char[] { ' ', ',', '.', '?', '!', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] expectedWords = expected.Split(new char[] { ' ', ',', '.', '?', '!', ';' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Count matching words
        int count = recognizedWords.Count(word => expectedWords.Contains(word));

        // Calculate accuracy percentage
        float accuracy = (float)count / expectedWords.Length;
        int percentAccuracy = Mathf.RoundToInt(accuracy * 100);

        return percentAccuracy;
    }

}
