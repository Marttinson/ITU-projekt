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

// Třída reprezentující strukturu otázky pro překlad
public class TranslateWordQuestion
{
    public int ID { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }
}

// Třída reprezentující strukturu otázky pro výběr ze tří možností
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
    public List<TranslateWordQuestion> LoadTranslateWordQuestions(string filePath, string unit)
    {
        try
        {
            // Načítání JSON obsahu ze souboru
            string jsonContent = File.ReadAllText(filePath);

            // Načítání a deserializace JSON pole
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("units", out JsonElement unitsElement))
                {
                    // Zkontrolujeme, zda lekce existuje v JSON
                    if (unitsElement.TryGetProperty(unit, out JsonElement questionsElement))
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
                        Console.WriteLine($"Lekce '{unit}' nebyla nalezena v JSON souboru.");
                        return new List<TranslateWordQuestion>();
                    }
                }
                else
                {
                    Console.WriteLine("JSON soubor neobsahuje žádné jednotky.");
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

    // Funkce pro načtení otázek uživatele podle názvu lekce
    public List<TranslateWordQuestion> LoadTranslateWordUserQuestions(string filePath, string unit)
    {
        try
        {
            // Načítání JSON obsahu ze souboru
            string jsonContent = File.ReadAllText(filePath);

            // Načítání a deserializace JSON pole
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("units", out JsonElement unitsElement))
                {
                    foreach (JsonElement unitElement in unitsElement.EnumerateArray())
                    {
                        string name = unitElement.GetProperty("Name").GetString();
                        if (name == unit)
                        {
                            // Našli jsme jednotku podle názvu, nyní načítáme otázky
                            List<TranslateWordQuestion> questions = new List<TranslateWordQuestion>();
                            JsonElement userQuestionsElement = unitElement.GetProperty("UserQuestions");

                            foreach (JsonElement questionElement in userQuestionsElement.EnumerateArray())
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
                    }

                    // Pokud nebyla nalezena jednotka s daným názvem
                    Console.WriteLine($"Unit with name {unit} not found.");
                    return new List<TranslateWordQuestion>();
                }
                else
                {
                    Console.WriteLine("JSON soubor neobsahuje jednotky.");
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

    // Načtení otázek pro pexeso (Načtení všech otazek, bez ohledu na lekce, poskytnutých v základním json souboru)
    public List<TranslateWordQuestion> LoadAllQuestions(string filePath)
    {
        try
        {
            // Načtení obsahu JSON ze souboru
            string jsonContent = File.ReadAllText(filePath);

            // Načtení a deserializace JSON
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("units", out JsonElement unitsElement))
                {
                    List<TranslateWordQuestion> allQuestions = new List<TranslateWordQuestion>();

                    // Iterace přes všechny jednotky (lekce)
                    foreach (JsonProperty unit in unitsElement.EnumerateObject())
                    {
                        foreach (JsonElement questionElement in unit.Value.EnumerateArray())
                        {
                            TranslateWordQuestion question = new TranslateWordQuestion
                            {
                                ID = questionElement.GetProperty("ID").GetInt32(),
                                QuestionText = questionElement.GetProperty("QuestionText").GetString(),
                                Answer = questionElement.GetProperty("Answer").GetString()
                            };
                            allQuestions.Add(question);
                        }
                    }

                    return allQuestions;
                }
                else
                {
                    Console.WriteLine("JSON soubor neobsahuje klíč 'units'.");
                    return new List<TranslateWordQuestion>();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba: {ex.Message}");
            return new List<TranslateWordQuestion>();
        }
    }

    // Funkce pro načtení otázek pro výběr ze tří možností
    public List<PickFromThreeQuestion> LoadOptionsQuestions(string filePath, string unitName)
    {
        try
        {
            // Načtení obsahu JSON ze souboru
            string jsonContent = File.ReadAllText(filePath);

            // Načtení a deserializace JSON
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("units", out JsonElement unitsElement))
                {
                    // Najdi pole otázek podle názvu lekce (unitName)
                    if (unitsElement.TryGetProperty(unitName, out JsonElement questionsElement))
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
                        Console.WriteLine($"Lekce '{unitName}' nebyla nalezena.");
                        return new List<PickFromThreeQuestion>();
                    }
                }
                else
                {
                    Console.WriteLine("JSON soubor neobsahuje klíč 'units'.");
                    return new List<PickFromThreeQuestion>();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba: {ex.Message}");
            return new List<PickFromThreeQuestion>();
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
                        ""UserQuestions"": [
    {
        ""ID"": 0,
        ""QuestionText"": ""Dog"",
        ""Answer"": ""Pes""
    },
    {
        ""ID"": 1,
        ""QuestionText"": ""Cat"",
        ""Answer"": ""Kočka""
    },
    {
        ""ID"": 2,
        ""QuestionText"": ""House"",
        ""Answer"": ""Dům""
    },
    {
        ""ID"": 3,
        ""QuestionText"": ""Book"",
        ""Answer"": ""Kniha""
    },
    {
        ""ID"": 4,
        ""QuestionText"": ""Car"",
        ""Answer"": ""Auto""
    },
    {
        ""ID"": 5,
        ""QuestionText"": ""Water"",
        ""Answer"": ""Voda""
    },
    {
        ""ID"": 6,
        ""QuestionText"": ""Chair"",
        ""Answer"": ""Židle""
    },
    {
        ""ID"": 7,
        ""QuestionText"": ""Apple"",
        ""Answer"": ""Jablko""
    },
    {

        ""ID"": 8,
        ""QuestionText"": ""Table"",
        ""Answer"": ""Stůl""
    },
    {
        ""ID"": 9,
        ""QuestionText"": ""Tree"",
        ""Answer"": ""Strom""
    }
]
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
