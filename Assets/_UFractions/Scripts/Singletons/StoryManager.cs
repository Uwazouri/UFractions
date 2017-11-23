using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : Singleton<StoryManager>
{
    protected StoryManager() { }

    private Story currentStory;
    private Story.Problem currentProblem;

    public void SetCurrentStory(string filePath)
    {
        this.currentStory = Story.LoadFromFile(filePath);
        this.currentProblem = this.currentStory.problems[0];
    }

    public void DebugStory()
    {
        Story myStory = new Story("UFractions Demo Story", new Story.Version(1, 0, 0));
        myStory.editorVersion = new Story.Version(1, 0, 0);

        myStory.introductionInformation = "A simple demo story with not much real story.";

        myStory.avatars.Add("https://cdn.pixabay.com/photo/2017/10/02/02/30/black-swan-2807716_960_720.jpg");

        Story.Path path1 = new Story.Path("Concept of Fractions", "Contains simple problems for learning fraction concepts.", 0);
        path1.pathPoint.nextPoints.Add(new Story.Point(1));
        path1.pathPoint.nextPoints[0].nextPoints.Add(new Story.Point(2));
        Story.Path path2 = new Story.Path("Adding and Subtracting", "Contains problems for learning fraction adding and subtracting", 1);
        Story.Path path3 = new Story.Path("Multiplication and Division", "Contains problems for learning fraction multiplication and division", 2);
        myStory.paths.Add(path1);
        myStory.paths.Add(path2);
        myStory.paths.Add(path3);

        Story.ChoiceAnswerData choicesAnswer = new Story.ChoiceAnswerData();
        choicesAnswer.textChoices.Add("Bob 2 and Bill 8");
        choicesAnswer.textChoices.Add("Bob 4 and Bill 6");
        choicesAnswer.textChoices.Add("Bob 5 and Bill 5");
        choicesAnswer.textChoices.Add("Bob 7 and Bill 3");
        choicesAnswer.textChoices.Add("Bob 9 and Bill 1");
        choicesAnswer.answer = 2;

        Story.TextBoxQuestion textBoxQuestion = new Story.TextBoxQuestion();
        textBoxQuestion.texts.Add(new Story.TextBox("http://www.freeiconspng.com/uploads/cat-png-15.png", true, "Sharing is caring! Bob and Bill needs to share thier 10 candies so that they both have 1/2 of all thier candies."));
        textBoxQuestion.texts.Add(new Story.TextBox("http://www.freeiconspng.com/uploads/cat-png-15.png", true, "Select the option that have numbers equals to the way they should share."));

        Story.Problem problem1 = new Story.Problem(0, Story.QuestionType.TextBox, Story.AnswerType.Choices);
        problem1.answer = new Story.AnswerStructure();
        problem1.question = new Story.QuestionStructure();
        problem1.answer.choicesAnswer = choicesAnswer;
        problem1.question.textBoxQuestion = textBoxQuestion;
        myStory.problems.Add(problem1);

        problem1 = new Story.Problem(1, Story.QuestionType.TextBox, Story.AnswerType.Choices);
        problem1.answer = new Story.AnswerStructure();
        problem1.question = new Story.QuestionStructure();
        problem1.answer.choicesAnswer = choicesAnswer;
        problem1.question.textBoxQuestion = textBoxQuestion;
        myStory.problems.Add(problem1);

        problem1 = new Story.Problem(2, Story.QuestionType.TextBox, Story.AnswerType.Choices);
        problem1.answer = new Story.AnswerStructure();
        problem1.question = new Story.QuestionStructure();
        problem1.answer.choicesAnswer = choicesAnswer;
        problem1.question.textBoxQuestion = textBoxQuestion;
        myStory.problems.Add(problem1);

        problem1 = new Story.Problem(3, Story.QuestionType.TextBox, Story.AnswerType.Choices);
        problem1.answer = new Story.AnswerStructure();
        problem1.question = new Story.QuestionStructure();
        problem1.answer.choicesAnswer = choicesAnswer;
        problem1.question.textBoxQuestion = textBoxQuestion;
        myStory.problems.Add(problem1);

        this.currentStory = myStory;
        this.currentProblem = this.currentStory.problems[0];

        File.WriteAllText("UFractions Demo Story", JsonUtility.ToJson(myStory, true));
    }

    public AnswerBehaviour GetCurrentAnswerBehaviour(Transform parent)
    {
        return InterfaceFactory.GetInstance().GetAnswerInterface(parent, this.currentProblem.answerType);
    }

    public QuestionBehaviour GetCurrentQuestionBehaviour(Transform parent)
    {
        return InterfaceFactory.GetInstance().GetQuestionInterface(parent, this.currentProblem.questionType);
    }

    public Story.Problem GetCurrentProblem()
    {
        return this.currentProblem;
    }
}
