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
            // Kontrola, zda soubor existuje
            if (!File.Exists(filePath))
            {
                // Pokud soubor neexistuje, zavoláme funkci pro vytvoření nového souboru
                CreateTranslateWordFile();
            }

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
            // Kontrola, zda soubor existuje
            if (!File.Exists(filePath))
            {
                // Pokud soubor neexistuje, zavoláme funkci pro vytvoření nového souboru
                CreateTranslateWordFile();
            }

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
            // Kontrola, zda soubor existuje
            if (!File.Exists(filePath))
            {
                // Pokud soubor neexistuje, zavoláme funkci pro vytvoření nového souboru
                CreateTranslateWordFile();
            }

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
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                CreateChoiceFile();
            }

            // Read the JSON content from the file
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize the JSON content
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;

                // Check if the 'units' property exists
                if (root.TryGetProperty("units", out JsonElement unitsElement))
                {
                    // Check if the specified unit exists in the 'units' property
                    if (unitsElement.TryGetProperty(unitName, out JsonElement unitElement))
                    {
                        List<PickFromThreeQuestion> questions = new List<PickFromThreeQuestion>();

                        // Iterate through each question in the specified unit
                        foreach (JsonElement questionElement in unitElement.EnumerateArray())
                        {
                            PickFromThreeQuestion question = new PickFromThreeQuestion
                            {
                                ID = questionElement.GetProperty("ID").GetInt32(),
                                QuestionText = questionElement.GetProperty("QuestionText").GetString(),
                                Options = questionElement.GetProperty("Options").EnumerateArray()
                                                  .Select(option => option.GetString())
                                                  .ToArray(),
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
            Console.WriteLine($"Chyba při načítání souboru: {ex.Message}");
            return new List<PickFromThreeQuestion>();
        }
    }


    // Vytvoření dat pro překlad
    public void CreateTranslateWordFile()
    {
        // Cesta k adresáři AppData + "ITU"
        string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ITU");

        // Cesta k souboru JSON
        string filePath = Path.Combine(appDataPath, "TranslateWord.json");

        // Obsah JSON
        string jsonString = @"
        {
          ""units"": {
            ""Unit 1"": [
              { ""ID"": 1, ""QuestionText"": ""Pes"", ""Answer"": ""Dog"" },
              { ""ID"": 2, ""QuestionText"": ""Kočka"", ""Answer"": ""Cat"" },
              { ""ID"": 3, ""QuestionText"": ""Kůň"", ""Answer"": ""Horse"" },
              { ""ID"": 4, ""QuestionText"": ""Králík"", ""Answer"": ""Rabbit"" },
              { ""ID"": 5, ""QuestionText"": ""Pták"", ""Answer"": ""Bird"" },
              { ""ID"": 6, ""QuestionText"": ""Krab"", ""Answer"": ""Crab"" },
              { ""ID"": 7, ""QuestionText"": ""Pes"", ""Answer"": ""Dog"" },
              { ""ID"": 8, ""QuestionText"": ""Koala"", ""Answer"": ""Koala"" },
              { ""ID"": 9, ""QuestionText"": ""Lev"", ""Answer"": ""Lion"" },
              { ""ID"": 10, ""QuestionText"": ""Slon"", ""Answer"": ""Elephant"" }
            ],
            ""Unit 2"": [
              { ""ID"": 11, ""QuestionText"": ""Jablko"", ""Answer"": ""Apple"" },
              { ""ID"": 12, ""QuestionText"": ""Hruška"", ""Answer"": ""Pear"" },
              { ""ID"": 13, ""QuestionText"": ""Banán"", ""Answer"": ""Banana"" },
              { ""ID"": 14, ""QuestionText"": ""Mrkev"", ""Answer"": ""Carrot"" },
              { ""ID"": 15, ""QuestionText"": ""Rajče"", ""Answer"": ""Tomato"" },
              { ""ID"": 16, ""QuestionText"": ""Cibule"", ""Answer"": ""Onion"" },
              { ""ID"": 17, ""QuestionText"": ""Okurka"", ""Answer"": ""Cucumber"" },
              { ""ID"": 18, ""QuestionText"": ""Celer"", ""Answer"": ""Celery"" },
              { ""ID"": 19, ""QuestionText"": ""Brambory"", ""Answer"": ""Potatoes"" },
              { ""ID"": 20, ""QuestionText"": ""Kapusta"", ""Answer"": ""Cabbage"" }
            ],
            ""Unit 3"": [
              { ""ID"": 21, ""QuestionText"": ""Červená"", ""Answer"": ""Red"" },
              { ""ID"": 22, ""QuestionText"": ""Modrá"", ""Answer"": ""Blue"" },
              { ""ID"": 23, ""QuestionText"": ""Zelená"", ""Answer"": ""Green"" },
              { ""ID"": 24, ""QuestionText"": ""Žlutá"", ""Answer"": ""Yellow"" },
              { ""ID"": 25, ""QuestionText"": ""Oranžová"", ""Answer"": ""Orange"" },
              { ""ID"": 26, ""QuestionText"": ""Bílá"", ""Answer"": ""White"" },
              { ""ID"": 27, ""QuestionText"": ""Černá"", ""Answer"": ""Black"" },
              { ""ID"": 28, ""QuestionText"": ""Hnědá"", ""Answer"": ""Brown"" },
              { ""ID"": 29, ""QuestionText"": ""Šedá"", ""Answer"": ""Gray"" },
              { ""ID"": 30, ""QuestionText"": ""Růžová"", ""Answer"": ""Pink"" }
            ],
            ""Unit 4"": [
              { ""ID"": 31, ""QuestionText"": ""Slunce"", ""Answer"": ""Sun"" },
              { ""ID"": 32, ""QuestionText"": ""Déšť"", ""Answer"": ""Rain"" },
              { ""ID"": 33, ""QuestionText"": ""Sníh"", ""Answer"": ""Snow"" },
              { ""ID"": 34, ""QuestionText"": ""Vítr"", ""Answer"": ""Wind"" },
              { ""ID"": 35, ""QuestionText"": ""Oblaka"", ""Answer"": ""Clouds"" },
              { ""ID"": 36, ""QuestionText"": ""Bouřka"", ""Answer"": ""Thunderstorm"" },
              { ""ID"": 37, ""QuestionText"": ""Mlha"", ""Answer"": ""Fog"" },
              { ""ID"": 38, ""QuestionText"": ""Teplo"", ""Answer"": ""Heat"" },
              { ""ID"": 39, ""QuestionText"": ""Chladno"", ""Answer"": ""Cold"" },
              { ""ID"": 40, ""QuestionText"": ""Počasí"", ""Answer"": ""Weather"" }
            ],
            ""Unit 5"": [
              { ""ID"": 41, ""QuestionText"": ""Leden"", ""Answer"": ""January"" },
              { ""ID"": 42, ""QuestionText"": ""Únor"", ""Answer"": ""February"" },
              { ""ID"": 43, ""QuestionText"": ""Březen"", ""Answer"": ""March"" },
              { ""ID"": 44, ""QuestionText"": ""Duben"", ""Answer"": ""April"" },
              { ""ID"": 45, ""QuestionText"": ""Květen"", ""Answer"": ""May"" },
              { ""ID"": 46, ""QuestionText"": ""Červen"", ""Answer"": ""June"" },
              { ""ID"": 47, ""QuestionText"": ""Červenec"", ""Answer"": ""July"" },
              { ""ID"": 48, ""QuestionText"": ""Srpen"", ""Answer"": ""August"" },
              { ""ID"": 49, ""QuestionText"": ""Září"", ""Answer"": ""September"" },
              { ""ID"": 50, ""QuestionText"": ""Říjen"", ""Answer"": ""October"" }
            ]
          }
        }";

        // Přečtení JSON stringu a serializace do objektu
        var jsonObject = JsonConvert.DeserializeObject(jsonString);

        // Uložení do souboru
        File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
    }

    // Vytvoření dat pro výběr ze tří
    public void CreateChoiceFile()
    {
        // Cesta k adresáři AppData + "ITU"
        string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ITU");

        // Cesta k souboru JSON
        string filePath = Path.Combine(appDataPath, "Choice.json");

        {
            // Vytvoření JSON jako textového řetězce
            string jsonContent = @"
{
  ""units"": {
    ""Unit 1"": [
      {
        ""ID"": 1,
        ""QuestionText"": ""How do you say 'pes' in English?"",
        ""Options"": [ ""Dog"", ""Cat"", ""Horse"" ],
        ""Answer"": ""Dog""
      },
      {
        ""ID"": 2,
        ""QuestionText"": ""How do you say 'kočka' in English?"",
        ""Options"": [ ""Rabbit"", ""Cat"", ""Bird"" ],
        ""Answer"": ""Cat""
      },
      {
        ""ID"": 3,
        ""QuestionText"": ""How do you say 'slon' in English?"",
        ""Options"": [ ""Elephant"", ""Lion"", ""Bear"" ],
        ""Answer"": ""Elephant""
      },
      {
        ""ID"": 4,
        ""QuestionText"": ""What animal is known for its black and white fur?"",
        ""Options"": [ ""Koala"", ""Penguin"", ""Panda"" ],
        ""Answer"": ""Panda""
      },
      {
        ""ID"": 5,
        ""QuestionText"": ""Which animal is the king of the jungle?"",
        ""Options"": [ ""Lion"", ""Tiger"", ""Bear"" ],
        ""Answer"": ""Lion""
      }
    ],
    ""Unit 2"": [
      {
        ""ID"": 6,
        ""QuestionText"": ""How do you say 'jablko' in English?"",
        ""Options"": [ ""Apple"", ""Banana"", ""Orange"" ],
        ""Answer"": ""Apple""
      },
      {
        ""ID"": 7,
        ""QuestionText"": ""How do you say 'hruška' in English?"",
        ""Options"": [ ""Pear"", ""Grapes"", ""Plum"" ],
        ""Answer"": ""Pear""
      },
      {
        ""ID"": 8,
        ""QuestionText"": ""What fruit is known for having a thick skin and a sweet, juicy inside?"",
        ""Options"": [ ""Orange"", ""Lemon"", ""Apple"" ],
        ""Answer"": ""Orange""
      },
      {
        ""ID"": 9,
        ""QuestionText"": ""How do you say 'rajče' in English?"",
        ""Options"": [ ""Tomato"", ""Cucumber"", ""Lettuce"" ],
        ""Answer"": ""Tomato""
      },
      {
        ""ID"": 10,
        ""QuestionText"": ""Which vegetable is orange and often used in salads?"",
        ""Options"": [ ""Carrot"", ""Cucumber"", ""Spinach"" ],
        ""Answer"": ""Carrot""
      }
    ],
    ""Unit 3"": [
      {
        ""ID"": 11,
        ""QuestionText"": ""How do you say 'červená' in English?"",
        ""Options"": [ ""Red"", ""Blue"", ""Green"" ],
        ""Answer"": ""Red""
      },
      {
        ""ID"": 12,
        ""QuestionText"": ""How do you say 'modrá' in English?"",
        ""Options"": [ ""Pink"", ""Blue"", ""Yellow"" ],
        ""Answer"": ""Blue""
      },
      {
        ""ID"": 13,
        ""QuestionText"": ""How do you say 'zelená' in English?"",
        ""Options"": [ ""Green"", ""Black"", ""White"" ],
        ""Answer"": ""Green""
      },
      {
        ""ID"": 14,
        ""QuestionText"": ""What color is associated with the sky on a clear day?"",
        ""Options"": [ ""Red"", ""Blue"", ""Yellow"" ],
        ""Answer"": ""Blue""
      },
      {
        ""ID"": 15,
        ""QuestionText"": ""How do you say 'žlutá' in English?"",
        ""Options"": [ ""Orange"", ""Yellow"", ""Pink"" ],
        ""Answer"": ""Yellow""
      }
    ],
    ""Unit 4"": [
      {
        ""ID"": 16,
        ""QuestionText"": ""What is the opposite of sunny weather?"",
        ""Options"": [ ""Rainy"", ""Windy"", ""Snowy"" ],
        ""Answer"": ""Rainy""
      },
      {
        ""ID"": 17,
        ""QuestionText"": ""What is the weather like when snow falls?"",
        ""Options"": [ ""Snowy"", ""Cloudy"", ""Windy"" ],
        ""Answer"": ""Snowy""
      },
      {
        ""ID"": 18,
        ""QuestionText"": ""What do you call strong winds during a storm?"",
        ""Options"": [ ""Tornado"", ""Hurricane"", ""Thunderstorm"" ],
        ""Answer"": ""Hurricane""
      },
      {
        ""ID"": 19,
        ""QuestionText"": ""What is the weather like when it's very hot?"",
        ""Options"": [ ""Cold"", ""Warm"", ""Hot"" ],
        ""Answer"": ""Hot""
      },
      {
        ""ID"": 20,
        ""QuestionText"": ""How do you say 'déšť' in English?"",
        ""Options"": [ ""Rain"", ""Snow"", ""Wind"" ],
        ""Answer"": ""Rain""
      }
    ],
    ""Unit 5"": [
      {
        ""ID"": 21,
        ""QuestionText"": ""What is the first month of the year?"",
        ""Options"": [ ""January"", ""February"", ""March"" ],
        ""Answer"": ""January""
      },
      {
        ""ID"": 22,
        ""QuestionText"": ""Which month comes after January?"",
        ""Options"": [ ""February"", ""March"", ""April"" ],
        ""Answer"": ""February""
      },
      {
        ""ID"": 23,
        ""QuestionText"": ""How do you say 'středa' in English?"",
        ""Options"": [ ""Monday"", ""Tuesday"", ""Wednesday"" ],
        ""Answer"": ""Wednesday""
      },
      {
        ""ID"": 24,
        ""QuestionText"": ""Which month is the middle of the year?"",
        ""Options"": [ ""June"", ""December"", ""August"" ],
        ""Answer"": ""June""
      },
      {
        ""ID"": 25,
        ""QuestionText"": ""How do you say 'pátek' in English?"",
        ""Options"": [ ""Thursday"", ""Friday"", ""Saturday"" ],
        ""Answer"": ""Friday""
      }
    ]
  }
}
";

            // Zápis JSON jako textového řetězce do souboru
            try
            {
                File.WriteAllText(filePath, jsonContent);
                Console.WriteLine("JSON soubor byl úspěšně vytvořen jako textový řetězec.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při vytváření souboru: {ex.Message}");
            }
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
}
