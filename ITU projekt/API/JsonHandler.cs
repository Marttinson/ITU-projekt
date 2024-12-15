/**
 * JSONHandler
 * M
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * Slouží k práci s JSON soubory
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using JsonSerializer = System.Text.Json.JsonSerializer;

using ITU_projekt.Models;

namespace ITU_projekt.API;

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
    // Example strings that will be written to files
    static string reading_statements_example = @"{
  ""1"": [
    {
      ""Text"": ""Sarah wakes up at 6:00 in the morning."",
      ""id"": 1,
      ""answer"": false,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""Sarah has cereal and milk for breakfast."",
      ""id"": 2,
      ""answer"": false,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""She finishes school at 3:00 PM."",
      ""id"": 3,
      ""answer"": true,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""Sarah plays with her dog after school."",
      ""id"": 4,
      ""answer"": true,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""Sarah goes to bed at 9:00 PM."",
      ""id"": 5,
      ""answer"": false,
      ""UserAnswer"": null
    }
  ],
  ""2"": [
    {
      ""Text"": ""Winter is a warm season."",
      ""id"": 1,
      ""answer"": false,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""People wear coats and hats in winter."",
      ""id"": 2,
      ""answer"": true,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""Flowers bloom in the summer."",
      ""id"": 3,
      ""answer"": false,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""Autumn is a time for harvest."",
      ""id"": 4,
      ""answer"": true,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""People go swimming in the winter."",
      ""id"": 5,
      ""answer"": false,
      ""UserAnswer"": null
    }
  ],
  ""3"": [
    {
      ""Text"": ""John likes to eat unhealthy food."",
      ""id"": 1,
      ""answer"": false,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""His favorite food is chicken."",
      ""id"": 2,
      ""answer"": true,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""John drinks milk with his meals."",
      ""id"": 3,
      ""answer"": false,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""For breakfast, he usually eats eggs and toast."",
      ""id"": 4,
      ""answer"": true,
      ""UserAnswer"": null
    },
    {
      ""Text"": ""John never eats chocolate."",
      ""id"": 5,
      ""answer"": false,
      ""UserAnswer"": null
    }
  ]
}";

    static string listening_statements_example = @"{
              ""1"": [
                {
                  ""Text"": ""Susan is Mike's wife."",
                  ""id"": 1,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Julie's been great."",
                  ""id"": 2,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Traffic was bad."",
                  ""id"": 3,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Laura is not running a small business"",
                  ""id"": 4,
                  ""answer"": false,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Party is rocking."",
                  ""id"": 5,
                  ""answer"": true,
                  ""UserAnswer"": null
                }
              ],
              ""2"": [
                {
                  ""Text"": ""Mark's new job is good."",
                  ""id"": 1,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""His boss is not hillarious."",
                  ""id"": 2,
                  ""answer"": false,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""There is dress code."",
                  ""id"": 3,
                  ""answer"": false,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Mike prefers finishing early."",
                  ""id"": 4,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Likes coffee."",
                  ""id"": 5,
                  ""answer"": true,
                  ""UserAnswer"": null
                }
              ],
              ""3"": [
                {
                  ""Text"": ""The weather is hot."",
                  ""id"": 1,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""John wants to buy sun-block."",
                  ""id"": 2,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Yumi is from Hakaido, Japan."",
                  ""id"": 3,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""Yumi's dog loves the water."",
                  ""id"": 4,
                  ""answer"": true,
                  ""UserAnswer"": null
                },
                {
                  ""Text"": ""John hates parks."",
                  ""id"": 5,
                  ""answer"": false,
                  ""UserAnswer"": null
                }
              ]
            }";

    static string defaultLections = @"
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

    static string translateword_example = @"
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

    static string choice_example = @"
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

// 
public class SentenceQuestion
{
    public int ID { set; get; }
    public string Sentence { set; get; }
    public string Translate { set; get; }
}

    public class JsonHandler
    {
        static string sentence_example = @"
        {
            ""units"": {
                ""Unit 1"": [
                    { ""ID"": 1, ""Sentence"": ""Pes je věrný přítel."", ""Translate"": ""A dog is a faithful friend."" },
                    { ""ID"": 2, ""Sentence"": ""Kočka ráda loví myši."", ""Translate"": ""A cat likes to hunt mice."" },
                    { ""ID"": 3, ""Sentence"": ""Pták létá na obloze."", ""Translate"": ""A bird flies in the sky."" }
                ],
                ""Unit 2"": [
                    { ""ID"": 4, ""Sentence"": ""Jablko je červené."", ""Translate"": ""The apple is red."" },
                    { ""ID"": 5, ""Sentence"": ""Mrkev je oranžová."", ""Translate"": ""The carrot is orange."" },
                    { ""ID"": 6, ""Sentence"": ""Banán je sladký."", ""Translate"": ""The banana is sweet."" }
                ],
                ""Unit 3"": [
                    { ""ID"": 7, ""Sentence"": ""Obloha je modrá."", ""Translate"": ""The sky is blue."" },
                    { ""ID"": 8, ""Sentence"": ""Tráva je zelená."", ""Translate"": ""The grass is green."" },
                    { ""ID"": 9, ""Sentence"": ""Slunce je žluté."", ""Translate"": ""The sun is yellow."" }
                ],
                ""Unit 4"": [
                    { ""ID"": 10, ""Sentence"": ""Prší venku."", ""Translate"": ""It is raining outside."" },
                    { ""ID"": 11, ""Sentence"": ""Je velmi slunečno."", ""Translate"": ""It is very sunny."" },
                    { ""ID"": 12, ""Sentence"": ""Sněží na horách."", ""Translate"": ""It is snowing in the mountains."" }
                ],
                ""Unit 5"": [
                    { ""ID"": 13, ""Sentence"": ""Pondělí je první pracovní den."", ""Translate"": ""Monday is the first working day."" },
                    { ""ID"": 14, ""Sentence"": ""Červen je letní měsíc."", ""Translate"": ""June is a summer month."" },
                    { ""ID"": 15, ""Sentence"": ""Sobota je den odpočinku."", ""Translate"": ""Saturday is a day of rest."" }
                ]
            }
        }";

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

    // Funkce pro načtení otázek uživatele podle názvu lekce
    public List<SentenceQuestion> LoadSenteceMakingQuestion(string filePath, string unit)
    {
        try
        {
            // Kontrola, zda soubor existuje
            if (!File.Exists(filePath))
            {
                // Pokud soubor neexistuje, zavoláme funkci pro vytvoření nového souboru
                CreateSentenceFile();
            }

            // Načítání JSON obsahu ze souboru
            string jsonContent = File.ReadAllText(filePath);

            // Načítání a deserializace JSON
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;

                // Ověření, že root obsahuje klíč "units"
                if (root.TryGetProperty("units", out JsonElement unitsElement))
                {
                    // Ověření, že jednotky obsahují požadovaný název
                    if (unitsElement.TryGetProperty(unit, out JsonElement unitQuestions))
                    {
                        // Našli jsme jednotku podle názvu, nyní načítáme otázky
                        List<SentenceQuestion> questions = new List<SentenceQuestion>();

                        foreach (JsonElement questionElement in unitQuestions.EnumerateArray())
                        {
                            SentenceQuestion question = new SentenceQuestion
                            {
                                ID = questionElement.GetProperty("ID").GetInt32(),
                                Sentence = questionElement.GetProperty("Sentence").GetString(),
                                Translate = questionElement.GetProperty("Translate").GetString()
                            };
                            questions.Add(question);
                        }

                        return questions;
                    }
                    else
                    {
                        Console.WriteLine($"Unit with name {unit} not found.");
                        return new List<SentenceQuestion>();
                    }
                }
                else
                {
                    Console.WriteLine("Root element does not contain 'units'.");
                    return new List<SentenceQuestion>();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<SentenceQuestion>();
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
        string jsonString = translateword_example;

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
            string jsonContent = choice_example;

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



    /// <summary>
    /// Reads statements for listening/reading exercise from json
    /// </summary>
    /// <param name="unit"> Unit to read </param>
    /// <param name="fileName"> String to diferentiate between reading/listening/...</param>
    /// <returns>List of statements</returns>
    public static List<ExerciseStatement> LoadExerciseStatements(string unit, string fileName)
    {
        // Read from appdata
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string filePath = Path.Combine(appDataPath, "ITU", fileName);

        Dictionary<string, List<ExerciseStatement>> allUnitsData;

        // Check if the file exists
        if (!File.Exists(filePath))
        {
            // If the file doesn't exist, create one with example data
            allUnitsData = new Dictionary<string, List<ExerciseStatement>>();
            allUnitsData[unit] = GenerateExampleData(unit, fileName)[unit];

            File.WriteAllText(filePath, JsonSerializer.Serialize(allUnitsData, new JsonSerializerOptions { WriteIndented = true }));
            return allUnitsData[unit];
        }

        // If the file exists, read and deserialize the data
        string json = File.ReadAllText(filePath);
        allUnitsData = JsonSerializer.Deserialize<Dictionary<string, List<ExerciseStatement>>>(json);

        if (allUnitsData == null || !allUnitsData.ContainsKey(unit))
        {
            // Handle missing unit data by generating example data
            if (allUnitsData == null)
            {
                allUnitsData = new Dictionary<string, List<ExerciseStatement>>();
            }

            MessageBox.Show($"Unit '{unit}' not found. Generating example data.");
            allUnitsData[unit] = GenerateExampleData(unit, fileName)[unit];

            File.WriteAllText(filePath, JsonSerializer.Serialize(allUnitsData, new JsonSerializerOptions { WriteIndented = true }));
            return allUnitsData[unit];
        }

        return allUnitsData[unit];
    }



    /// <summary>
    /// Generate example data that will be written to a file
    /// </summary>
    /// <param name="unit"> Unit to fill</param>
    /// <param name="file"> String to diferentiate between reading/listening/... </param>
    /// <returns> Dictionary (UnitID; List with statements) </returns>
    private static Dictionary<string, List<ExerciseStatement>> GenerateExampleData(string unit, string file)
    {
        var allUnitsData = new Dictionary<string, List<ExerciseStatement>>();
        string example = "";
        if (file == "Listening_statements.json")
        {
            example = listening_statements_example;

        }


        if (file == "Reading_statements.json")
        {
            example = reading_statements_example;
        }

            // Parse JSON into a dictionary by unit
            var parsedData = JsonSerializer.Deserialize<Dictionary<string, List<ExerciseStatement>>>(example);

        if (parsedData != null)
        {
            // Add the parsed data to allUnitsData
            foreach (var keyValuePair in parsedData)
            {
                allUnitsData[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        return allUnitsData;
    }

    // Funkce pro vytvoření souboru z řetězce
    private void CreateSentenceFile()
    {
        try
        {
            // Definování výchozího JSON obsahu jako textového řetězce
            

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataPath, "ITU", "Sentence.json");

            // Uložení do souboru
            File.WriteAllText(filePath, sentence_example);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Došlo k chybě při vytváření souboru: {ex.Message}");
        }
    }


    // Load units as a collection
    public static ObservableCollection<UnitModel> LoadUnits()
        {
            try
            {
                // Získání cesty k AppData
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                // Cesta k souboru JSON
                string jsonPath = Path.Combine(appDataPath, "ITU", "lekce.json");

            // If file does not exist
            if (!File.Exists(jsonPath))
            {
                // NO LECTIONS FOUND, GENERATE PREDEFINED

                    // Vytvoření adresáře, pokud neexistuje
                    string dirPath = Path.Combine(appDataPath, "ITU");
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    // Uložení přednastavených dat do souboru
                    File.WriteAllText(jsonPath, defaultLections);
                }

                // Načtení a deserializace JSON
                var json = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<ObservableCollection<UnitModel>>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading units: {ex.Message}");
                return new ObservableCollection<UnitModel>();
            }
        }


     /// <summary>
     /// Loads user prefered streak symbol
     /// </summary>
     /// <returns>string with streak symbol</returns>
    public static string LoadStreakSymbol()
    {
        try
        {
            // get from appdata
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string jsonPath = Path.Combine(appDataPath, "ITU", "streakSymb.json");

                // Check if the file exists
                if (File.Exists(jsonPath))
                {
                    // Read the file content
                    string json = File.ReadAllText(jsonPath);

                    // Deserialize the JSON content to string
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

    /// <summary>
    /// Saves user prefered streak symbol
    /// </summary>
    /// <param name="streakSymbol">User prefered streak symbol to be saved</param>
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


    /// <summary>
    /// Saves user streak information
    /// </summary>
    /// <param name="length"> length - how many days in a row </param>
    /// <param name="date"> date - when user finished last unit </param>
    /// <returns></returns>
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

    /// <summary>
    /// get user streak information
    /// </summary>
    /// <returns>Streak (length, date)</returns>
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading user streak: {ex.Message}");
                return null;
            }
        }


    /// <summary>
    /// Saves user questions
    /// </summary>
    /// <param name="unitId"> Unit to save to </param>
    /// <param name="updatedQuestions"> Questions to be saved </param>
    /// <returns>false if failed, else true</returns>
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


    /// <summary>
    /// Saves statistics (user's error rate)
    /// </summary>
    /// <param name="iD">What unit to save to</param>
    /// <param name="v">Error rate which will be added</param>
    public static void SaveStatistic(int iD, float v)
    {
        try
        {
            // Get path
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Debug.WriteLine($"AppData Path: {appDataPath}");

                // Construct the full path 
                string jsonPath = Path.Combine(appDataPath, "ITU", "lekce.json");

                var units = LoadUnits();

                // Find the unit with the specified ID
                var unit = units.FirstOrDefault(u => u.ID == iD);


                if (unit != null)
                {
                    // Add the statistic to the unit's ErrorRates
                    unit.ErrorRates.Add(v);

                // Serialize the updated units 
                var updatedJson = JsonConvert.SerializeObject(units, Formatting.Indented);
                File.WriteAllText(jsonPath, updatedJson);
            }
            else
            {
                MessageBox.Show($"Unit with ID {iD} not found.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving the statistic: {ex.Message}");
        }
    }

    /// <summary>
    /// Returns path to audio
    /// </summary>
    /// <param name="unit">which audio to choose</param>
    /// <returns>path to audio</returns>
    public static string getAudio(string unit)
    {
        string audioFilePath = "Resources/Audio/empty.mp3"; // Default path

        switch (unit)
        {
            case "Unit 1":
                audioFilePath = "Resources/Audio/u1.mp3";
                break;
            case "Unit 2":
                audioFilePath = "Resources/Audio/u2.mp3";
                break;
            case "Unit 3":
                audioFilePath = "Resources/Audio/u3.mp3";
                break;
            default:
                MessageBox.Show("Error retrieving audio file.");
                break;
        }

        return audioFilePath; // Return the relative file path
    }

    /// <summary>
    /// Loads text for reading exercise
    /// </summary>
    /// <param name="id">Which unit's text will be returned</param>
    /// <returns>string with text</returns>
    public string LoadReadingExercise(string id)
    {
        string filePath = "reading_exercise_texts.json";

        // Try to load the file
        Dictionary<string, string> data = LoadJsonFile(filePath);

        // If ID is not found in the loaded data, add the new ID with predefined value
        if (!data.ContainsKey(id))
        {
            string predefinedValue = "";

            switch (id)
            {
                case "1":
                    predefinedValue = "Sarah is a student. She wakes up at 7:00 every morning. After she gets up, she takes a shower and then has breakfast. She usually eats cereal and drinks orange juice. Sarah goes to school at 8:00 and finishes at 3:00 in the afternoon. After school, she does her homework and then plays with her dog in the park. She goes to bed at 10:00.";
                    break;
                case "2":
                    predefinedValue = "In winter, the weather is cold. It snows in many places. People wear coats, hats, and gloves to stay warm. Spring is a season with mild weather. Flowers bloom and trees get green again. In summer, it is hot. People go swimming and enjoy the sun. Autumn is a time when the weather gets cooler, and leaves change color. It is also a time for harvest.";
                    break;
                case "3":
                    predefinedValue = "John likes to eat healthy food. He eats vegetables, fruits, and meat. His favorite food is chicken. He also enjoys eating salads with tomatoes, cucumbers, and lettuce. For breakfast, John usually eats eggs and toast. He drinks water with his meals. Sometimes, he eats chocolate for dessert.";
                    break;
            }

            data.Add(id, predefinedValue);

            // Save the data
            SaveJsonFile(filePath, data);
        }

        // Return the value for the ID
        return data[id];
    }

    // Method to load JSON data from the file
    private static Dictionary<string, string> LoadJsonFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            // If the file doesn't exist, return a new dictionary
            return new Dictionary<string, string>();
        }

        // else return data as a dictionary
        string jsonContent = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
    }

    // Method to save data to the JSON file
    private static void SaveJsonFile(string filePath, Dictionary<string, string> data)
    {
        string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, jsonContent);
    }
}
