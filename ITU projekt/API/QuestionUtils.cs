using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;


namespace ITU_projekt.API;
public class QuestionUtils
{
    public QuestionUtils()
    {

    }

    // ------------------ Překlad slova ------------------
    // Metoda pro vyhledání otázky podle ID
    public TranslateWordQuestion FindTranslateWordQuestionById(List<TranslateWordQuestion> questions, int id)
    {
        foreach (var question in questions)
        {
            if (question.ID == id)
                return question;
        }
        return null;
    }

    public TranslateWordQuestion FindTranslateWordQuestionByText(List<TranslateWordQuestion> questions, string text)
    {
        foreach(var question in questions)
        {
            if (question.QuestionText == text)
                return question;
        }
        return null;
    }

    public TranslateWordQuestion FindTranslateWordQuestionByAnswer(List<TranslateWordQuestion> questions, string answer)
    {
        foreach (var question in questions)
        {
            if (question.Answer == answer)
                return question;
        }
        return null;
    }

    // Metoda pro výběr X náhodných otázek
    public List<TranslateWordQuestion> GetRandomTranslateWordQuestions(List<TranslateWordQuestion> questions, int count)
    {
        Random random = new Random();
        return questions.OrderBy(x => random.Next()).Take(count).ToList();
    }

    // Metoda pro přidání nové otázky
    public void AddTranslateWordQuestion(List<TranslateWordQuestion> questions, TranslateWordQuestion question)
    {
        questions.Add(question);
    }

    // Metoda pro odstranění otázky podle ID
    public bool RemoveTranslateWordQuestionById(List<TranslateWordQuestion> questions, int id)
    {
        var question = FindTranslateWordQuestionById(questions, id);
        if (question != null)
        {
            questions.Remove(question);
            return true;
        }
        return false;
    }

    // ------------------ Výběr ze tří možností ------------------
    // Metoda pro vyhledání otázky podle ID
    public PickFromThreeQuestion FindOptionsQuestionById(List<PickFromThreeQuestion> questions, int id)
    {
        foreach (var question in questions)
        {
            if (question.ID == id)
                return question;
        }
        return null;
    }

    // Metoda pro výběr X náhodných otázek
    public List<PickFromThreeQuestion> GetRandomOptionsQuestions(List<PickFromThreeQuestion> questions, int count)
    {
        Random random = new Random();
        return questions.OrderBy(x => random.Next()).Take(count).ToList();
    }

    // Metoda pro přidání nové otázky
    public void AddOptionsQuestion(List<PickFromThreeQuestion> questions, PickFromThreeQuestion question)
    {
        questions.Add(question);
    }

    // Metoda pro odstranění otázky podle ID
    public bool RemoveOptionsQuestionById(List<PickFromThreeQuestion> questions, int id)
    {
        var question = FindOptionsQuestionById(questions, id);
        if (question != null)
        {
            questions.Remove(question);
            return true;
        }
        return false;
    }

    // ------------------ Skládání vět ------------------
    // Metoda pro vyhledání otázky podle ID
    public SentenceQuestion FindSentenceQuestionById(List<SentenceQuestion> questions, int id)
    {
        foreach (var question in questions)
        {
            if (question.ID == id)
                return question;
        }
        return null;
    }

    // Metoda pro výběr X náhodných otázek
    public List<SentenceQuestion> GetRandomSentenceQuestions(List<SentenceQuestion> questions, int count)
    {
        Random random = new Random();
        return questions.OrderBy(x => random.Next()).Take(count).ToList();
    }

    // Metoda pro přidání nové otázky
    public void AddSentenceQuestion(List<SentenceQuestion> questions, SentenceQuestion question)
    {
        questions.Add(question);
    }

    // Metoda pro odstranění otázky podle ID
    public bool RemoveSentenceQuestionById(List<SentenceQuestion> questions, int id)
    {
        var question = FindSentenceQuestionById(questions, id);
        if (question != null)
        {
            questions.Remove(question);
            return true;
        }
        return false;
    }
}
