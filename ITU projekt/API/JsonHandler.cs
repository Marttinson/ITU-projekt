using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
}
