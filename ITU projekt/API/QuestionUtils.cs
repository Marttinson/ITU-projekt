using System;
using System.Collections.Generic;
using System.Linq;


namespace ITU_projekt.API;
public class QuestionUtils
{
    // Konstruktor pro předání seznamu otázek
    public QuestionUtils()
    {

    }

    // Metoda pro vyhledání otázky podle ID
    public Question FindQuestionById(List<Question> questions, int id)
    {
        foreach (var question in questions)
        {
            if (question.ID == id)
                return question;
        }
        return null;
    }

    // Metoda pro výběr X náhodných otázek
    public List<Question> GetRandomQuestions(List<Question> questions, int count)
    {
        Random random = new Random();
        return questions.OrderBy(x => random.Next()).Take(count).ToList();
    }

    // Metoda pro přidání nové otázky
    public void AddQuestion(List<Question> questions, Question question)
    {
        questions.Add(question);
    }

    // Metoda pro odstranění otázky podle ID
    public bool RemoveQuestionById(List<Question> questions, int id)
    {
        var question = FindQuestionById(questions, id);
        if (question != null)
        {
            questions.Remove(question);
            return true;
        }
        return false;
    }
}
