using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using ITU_projekt.Models;
using System.Windows;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ITU_projekt.API;

public class Question : INotifyPropertyChanged
{
    public int ID { get; set; }

    private string _questionText;
    public string QuestionText
    {
        get => _questionText;
        set
        {
            if (_questionText != value)
            {
                _questionText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsModified));  // Notify that IsModified may have changed
            }
        }
    }

    private string _answer;
    public string Answer
    {
        get => _answer;
        set
        {
            if (_answer != value)
            {
                _answer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsModified));  // Notify that IsModified may have changed
            }
        }
    }

    private bool _hasDuplicate;
    public bool HasDuplicate
    {
        get => _hasDuplicate;
        set
        {
            if (_hasDuplicate != value)
            {
                _hasDuplicate = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isModified;
    public bool IsModified
    {
        get => _isModified;
        set
        {
            if (_isModified != value)
            {
                _isModified = value;
                OnPropertyChanged();
            }
        }
    }

    // Add INotifyPropertyChanged for proper binding
    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class Streak
{
    public DateTime last_date;
    private int _length;

    public int length
    {
        get => _length;
        set
        {
            if (_length != value)
            {
                _length = value;
                OnPropertyChanged();
            }
        }
    }

    // Add INotifyPropertyChanged for proper binding
    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

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

                // Ověření, že root je pole
                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement unitElement in root.EnumerateArray())
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
                    Console.WriteLine("Root element in JSON is not an array.");
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
        ""ID"": 10000,
        ""QuestionText"": ""Dog"",
        ""Answer"": ""Pes""
    },
    {
        ""ID"": 10001,
        ""QuestionText"": ""Cat"",
        ""Answer"": ""Kočka""
    },
    {
        ""ID"": 10002,
        ""QuestionText"": ""House"",
        ""Answer"": ""Dům""
    },
    {
        ""ID"": 10003,
        ""QuestionText"": ""Book"",
        ""Answer"": ""Kniha""
    },
    {
        ""ID"": 10004,
        ""QuestionText"": ""Car"",
        ""Answer"": ""Auto""
    },
    {
        ""ID"": 10005,
        ""QuestionText"": ""Water"",
        ""Answer"": ""Voda""
    },
    {
        ""ID"": 10006,
        ""QuestionText"": ""Chair"",
        ""Answer"": ""Židle""
    },
    {
        ""ID"": 10007,
        ""QuestionText"": ""Apple"",
        ""Answer"": ""Jablko""
    },
    {

        ""ID"": 10008,
        ""QuestionText"": ""Table"",
        ""Answer"": ""Stůl""
    },
    {
        ""ID"": 10009,
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
                    }
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



    public static string LoadStreakSymbol()
    {
        try
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string jsonPath = Path.Combine(appDataPath, "ITU", "streakSymb.json");

            // Check if the file exists
            if (File.Exists(jsonPath))
            {
                // Read the file content
                string json = File.ReadAllText(jsonPath);

                // Deserialize the JSON content to a char
                return JsonConvert.DeserializeObject<string>(json);
            }
            else
            {
                // default
                return "✔";
            }
        }
        catch (Exception ex)
        {
            // Handle errors
            Console.WriteLine($"Error loading streak symbol: {ex.Message}");
            return "✔"; // Return default if there's an error
        }
    }

    // Save the streak symbol
    public static void SaveStreakSymbol(string streakSymbol)
    {
        try
        {
            // Serialize the char to JSON
            string json = JsonConvert.SerializeObject(streakSymbol);

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string jsonPath = Path.Combine(appDataPath, "ITU", "streakSymb.json");

            // Write the serialized character to the file
            File.WriteAllText(jsonPath, json);
        }
        catch (Exception ex)
        {
            // Handle errors
            Console.WriteLine($"Error saving streak symbol: {ex.Message}");
        }
    }


    public static bool SaveStreak(int length, DateTime date)
    {
        try
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string jsonPath = Path.Combine(appDataPath, "ITU", "streak.json");

            // Check if the JSON file exists
            if (!File.Exists(jsonPath))
            {
                string dirPath = Path.Combine(appDataPath, "ITU");
                // Ensure the directory exists
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

            }

            var streak = new Streak { length = length, last_date = date };
            string new_streak = JsonConvert.SerializeObject(streak);

            File.WriteAllText(jsonPath, new_streak);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving user streak: {ex.Message}");
            return false;
        }
    }

    public static Streak? ReadStreak()
    {
        try
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string jsonPath = Path.Combine(appDataPath, "ITU", "streak.json");


            // Check if the JSON file exists
            if (!File.Exists(jsonPath))
            {
                MessageBox.Show("Streak file not found. Will be created.");
                var defaultStreak = new Streak
                {
                    length = 0,
                    last_date = DateTime.Now
                };

                // Ensure the directory exists
                string dirPath = Path.Combine(appDataPath, "ITU");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // Write default streak to file
                string defaultStreakJson = JsonConvert.SerializeObject(defaultStreak);
                File.WriteAllText(jsonPath, defaultStreakJson);
            }

            // Read and deserialize the existing JSON data
            var json = File.ReadAllText(jsonPath);
            Streak? streak = JsonConvert.DeserializeObject<Streak>(json);
            return streak;
        }
        catch(Exception ex)
        {
            MessageBox.Show($"An error occurred while reading user streak: {ex.Message}");
            return null;
        }
    }


    // Save the UserQuestions to JSON under the specific Unit ID
    public static bool SaveUserQuestions(int unitId, ObservableCollection<Question> updatedQuestions)
    {
        try
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string jsonPath = Path.Combine(appDataPath, "ITU", "lekce.json");
            

            // Check if the JSON file exists
            if (!File.Exists(jsonPath))
            {
                MessageBox.Show("JSON file not found. Please load the units first.");
                return false;
            }

            // Read and deserialize the existing JSON data
            var json = File.ReadAllText(jsonPath);
            var units = JsonConvert.DeserializeObject<ObservableCollection<UnitModel>>(json);

            // Find the unit by ID
            var unit = units.FirstOrDefault(u => u.ID == unitId);
            if (unit == null)
            {
                MessageBox.Show($"Unit with ID {unitId} not found.");
                return false;
            }

            // Update the UserQuestions with the new data
            unit.UserQuestions = updatedQuestions.ToList();

            // Serialize the updated units back to JSON
            var updatedJson = JsonConvert.SerializeObject(units, Formatting.Indented);

            // Write the updated JSON back to the file
            File.WriteAllText(jsonPath, updatedJson);

            MessageBox.Show("User questions saved successfully.");
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving user questions: {ex.Message}");
            return false;
        }
        return true;
    }
}
