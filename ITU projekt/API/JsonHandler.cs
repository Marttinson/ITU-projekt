using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using ITU_projekt.Models;
using System.Windows;

namespace ITU_projekt.API;

public class Question
{
    public int ID { get; set; }

    public string Type { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }
}

public class JsonHandler
{
    private string _filePath;

    public JsonHandler(string filePath)
    {
        _filePath = filePath;
    }

    public List<Question> LoadQuestions()
    {
        try
        {
            // Načítání JSON obsahu ze souboru
            string jsonContent = File.ReadAllText(_filePath);

            //Console.WriteLine(jsonContent);  // Výpis načteného obsahu pro kontrolu

            // Načítání a deserializace JSON pole
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("questions", out JsonElement questionsElement))
                {
                    List<Question> questions = new List<Question>();

                    foreach (JsonElement questionElement in questionsElement.EnumerateArray())
                    {
                        Question question = new Question
                        {
                            ID = questionElement.GetProperty("ID").GetInt32(),
                            Type = questionElement.GetProperty("Type").GetString(),
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
                    return new List<Question>();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<Question>();
        }
    }

    // Load units
    public static ObservableCollection<UnitModel> LoadUnits()
    {
        try
        {
            // Get the application data path
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Construct the full path to lekce.json
            string jsonPath = Path.Combine(appDataPath, "ITU", "lekce.json");

            // If file does not exist
            if (!File.Exists(jsonPath))
            {
                // NO LECTIONS FOUND, GENERATE PREDEFINED
                string defaultLections = @"
                [
                    {
                        ""Name"": ""Unit 1"",
                        ""ID"": 1,
                        ""Description"": ""To Be"",
                        ""ErrorRates"": [ 0.1, 0.2, 0.15, 0.1, 0.8, 0.9, 0.5, 0.7, 0.6, 0.23, 0.64, 0.35 ],
                        ""UserQuestions"": []
                    },
                    {
                        ""Name"": ""Unit 2"",
                        ""ID"": 2,
                        ""Description"": ""Basic vocabulary"",
                        ""ErrorRates"": [ 0.05, 0.07, 0.9, 0.5, 0.7, 0.6, 0.28 ],
                        ""UserQuestions"": []
                    },
                    {
                        ""Name"": ""Unit 3"",
                        ""ID"": 3,
                        ""Description"": ""Plural"",
                        ""ErrorRates"": [ 0.1, 0.2, 0.15 ],
                        ""UserQuestions"": []
                    },
                    {   
                        ""Name"": ""Unit 4"",
                        ""ID"": 4,
                        ""Description"": ""Numbers"",
                        ""ErrorRates"": [],
                        ""UserQuestions"": []
                    },
                ]";

                string dirPath = Path.Combine(appDataPath, "ITU");
                // Ensure the directory exists
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // generate basic units 
                File.WriteAllText(jsonPath, defaultLections);
            }

            // Read and deserialize the JSON
            var json = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<ObservableCollection<UnitModel>>(json);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while loading units: {ex.Message}");
            return new ObservableCollection<UnitModel>();
        }

    }
}
