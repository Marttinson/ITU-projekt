using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using ITU_projekt.Models;
using System.Windows;
using System.Linq;

namespace ITU_projekt.API;

public class TranslateWordQuestion
{
    public int ID { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }
}

public class PickFromThreeQuestion
{
    public int ID { get; set; }
    public string QuestionText { get; set; }
    public string[] Options { get; set; } = new string[3];
    public string Answer { get; set; }
}

public class JsonHandler
{

    public JsonHandler()
    {
        
    }

    // Funkce pro načtení otázek pro překlad slova
    public List<TranslateWordQuestion> LoadTranslateWordQuestions(string filePath)
    {
        try
        {
            // Načítání JSON obsahu ze souboru
            string jsonContent = File.ReadAllText(filePath);

            //Console.WriteLine(jsonContent);  // Výpis načteného obsahu pro kontrolu

            // Načítání a deserializace JSON pole
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("questions", out JsonElement questionsElement))
                {
                    List<TranslateWordQuestion> questions = new List<TranslateWordQuestion>();

                    foreach (JsonElement questionElement in questionsElement.EnumerateArray())
                    {
                        TranslateWordQuestion question = new TranslateWordQuestion
                        {
                            ID = questionElement.GetProperty("ID").GetInt32(),
                            QuestionText = questionElement.GetProperty("QuestionText").GetString(),
                            Answer = questionElement.GetProperty("Answer").GetString()
                        };
                        questions.Add(question);
                    }

                    return questions;
                }
                else
                {
                    Console.WriteLine("JSON soubor neobsahuje žádné otázky (JsonHandler.cs)");
                    return new List<TranslateWordQuestion>();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<TranslateWordQuestion>();
        }
    }

    // Funkce pro načtení otázek pro výběr ze tří možností
    public List<PickFromThreeQuestion> LoadOptionsQuestions(string filePath)
    {
        try
        {
            // Načítání JSON obsahu ze souboru
            string jsonContent = File.ReadAllText(filePath);

            //Console.WriteLine(jsonContent);  // Výpis načteného obsahu pro kontrolu

            // Načítání a deserializace JSON pole
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("questions", out JsonElement questionsElement))
                {
                    List<PickFromThreeQuestion> questions = new List<PickFromThreeQuestion>();

                    foreach (JsonElement questionElement in questionsElement.EnumerateArray())
                    {
                        PickFromThreeQuestion question = new PickFromThreeQuestion
                        {
                            ID = questionElement.GetProperty("ID").GetInt32(),
                            QuestionText = questionElement.GetProperty("QuestionText").GetString(),
                            Options = questionElement.GetProperty("Options").EnumerateArray().Select(option => option.GetString()).ToArray(),
                            Answer = questionElement.GetProperty("Answer").GetString()
                        };
                        questions.Add(question);
                    }

                    return questions;
                }
                else
                {
                    Console.WriteLine("JSON soubor neobsahuje žádné otázky (JsonHandler.cs)");
                    return new List<PickFromThreeQuestion>();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<PickFromThreeQuestion>();
        }
    }

    public static ObservableCollection<UnitModel> LoadUnits()
    {

        var json = File.ReadAllText("../../../Data/Anglictina/units.json");
        if (String.IsNullOrEmpty(json))
        {
            MessageBox.Show("Units not found!");
            return null;
        }

        return JsonConvert.DeserializeObject<ObservableCollection<UnitModel>>(json);

    }
}
