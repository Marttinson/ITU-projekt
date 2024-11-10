using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ITU_projekt.Models;


public struct Task
{
    public string TaskType { get; set; }
    public string Type { get; set; }
    public string TaskText { get; set; }
    public string TaskAnswer { get; set; }

    public int CurrentTaskNumber { get; set; }
    public int MaxTaskNumber { get; set; }
}

internal class TaskGenerator
{
    private string _filePath;
    private int _currentTaskCount;
    private int _maxTaskCount;

    private Random random = new Random();

    public TaskGenerator(string unit)
    {
        switch (unit)
        {
            case "1":
                _filePath = "unit1.json";
                _currentTaskCount = 0;
                _maxTaskCount = 5;
                break;
        }

    }

    public Task generate()
    {

        if(_currentTaskCount == _maxTaskCount)
        {
            return new Task
            {
                TaskType = "STOP",
                Type = "STOP",
                TaskText = "STOP",
                TaskAnswer = "STOP",
                CurrentTaskNumber = _maxTaskCount,
                MaxTaskNumber = _maxTaskCount
            };
        }

        try
        {
            // Read JSON content from the file
            string jsonString = File.ReadAllText(_filePath);

            // Deserialize JSON to Dictionary
            var tasks = JsonSerializer.Deserialize<Dictionary<string, Task>>(jsonString);

            // Example: Retrieve information for task number "1"
            int taskNumber = random.Next(1, 5);

            if (tasks != null && tasks.TryGetValue(taskNumber.ToString(), out Task task))
            {
                task.CurrentTaskNumber = _currentTaskCount;
                task.MaxTaskNumber = _maxTaskCount;
                _currentTaskCount++;

                return task;
            }
            else
            {
                Console.WriteLine($"Task number {taskNumber} not found.");

                return new Task
                {
                    TaskType = "ERROR",
                    Type = "ERROR",
                    TaskText = "ERROR",
                    TaskAnswer = "ERROR",
                    CurrentTaskNumber = 0,
                    MaxTaskNumber = 0
                };
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"The file '{_filePath}' was not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine("There was an error deserializing the JSON data.");
        }

        return new Task
        {
            TaskType = "ERROR",
            Type = "ERROR",
            TaskText = "ERROR",
            TaskAnswer = "ERROR",
            CurrentTaskNumber = 0,
            MaxTaskNumber = 0
        };

    }


}
