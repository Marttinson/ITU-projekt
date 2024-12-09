using System;
using System.Collections.Generic;
using System.Linq;


namespace ITU_projekt.API;
public class QuestionUtils
{
    private List<Question> _questions;

    // Konstruktor pro předání seznamu otázek
    public QuestionUtils(List<Question> questions)
    {
        _questions = questions;
    }

    // Metoda pro vyhledání otázky podle ID
    public Question FindQuestionById(int id)
    {
        foreach (var question in _questions)
        {
            if (question.ID == id)
                return question;
        }
        return null;
    }

    // Metoda pro výběr X náhodných otázek
    public List<Question> GetRandomQuestions(int count)
    {
        Random random = new Random();
        return _questions.OrderBy(x => random.Next()).Take(count).ToList();
    }

    // Metoda pro přidání nové otázky
    public void AddQuestion(Question question)
    {
        _questions.Add(question);
    }

    // Metoda pro odstranění otázky podle ID
    public bool RemoveQuestionById(int id)
    {
        var question = FindQuestionById(id);
        if (question != null)
        {
            _questions.Remove(question);
            return true;
        }
        return false;
    }
}
