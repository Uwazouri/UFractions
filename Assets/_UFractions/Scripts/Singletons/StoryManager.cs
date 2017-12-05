using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class StoryManager : Singleton<StoryManager>
{
    protected StoryManager() { }

    public Story currentStory;
    private Story.Path currentPath;
    private Story.Event currentPoint;
    private Story.Problem currentProblem;
    private bool lastProblemSolved = false;
    private string storiesFolderName = "Stories";
    private string baseStoryFolder = "BaseStories";

    public string GetVideoPath(string name)
    {
        return this.currentStory.vidUrlsDictionary[name];
    }

    public string GetImagePath(string name)
    {
        return this.currentStory.imgUrlsDictionary[name];
    }

    public void SetProblemSolved(bool solved)
    {
        this.lastProblemSolved = solved;
    }

    public bool LastProblemSolved()
    {
        return this.lastProblemSolved;
    }

    public List<Story> GetLocalStories()
    {
        string storiesPath = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + this.storiesFolderName;

        print(storiesPath);

        if (!Directory.Exists(storiesPath))
            Directory.CreateDirectory(storiesPath);

        string[] baseStories = Directory.GetFiles(Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar + this.baseStoryFolder, "*.json");

        int t = 1;

        foreach (string s in baseStories)
        {
            File.Copy(s, Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + this.storiesFolderName + System.IO.Path.DirectorySeparatorChar + "BaseStory" + t++ + ".json", true);
        }

        List<Story> localStories = new List<Story>();

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.All;
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        foreach (string s in Directory.GetFiles(Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + this.storiesFolderName, "*.json"))
            localStories.Add(Story.LoadFromJSON(s));

        return localStories;
    }

    public void SetCurrentStory(Story story)
    {
        this.currentStory = story;
    }

    public void SetCurrentStory(string filePath)
    {
        this.SetCurrentStory(Story.LoadFromJSON(filePath));
    }

    public List<Story.Path> GetAllPaths()
    {
        return this.currentStory.paths;
    }

    public void SetCurrentPath(Story.Path path)
    {
        this.currentPath = path;
        this.currentPoint = this.currentPath.pathPoint;
    }

    public Story.Problem GetProblem(uint ID)
    {
        this.lastProblemSolved = false;
        foreach (Story.Problem p in this.currentStory.problems)
        {
            if (p.problemID == ID)
                return p;
        }
        return null;
    }

    public Story.Problem GetCurrentProblem()
    {
        return this.GetProblem(this.currentPoint.problemID);
    }

    public Story.Event GetCurrentEvent()
    {
        return this.currentPoint;
    }

    public void SetCurrentEvent(Story.Event point)
    {
        this.currentPoint = point;
    }

    public AnswerBehaviour GetCurrentAnswerBehaviour(Transform parent)
    {
        return InterfaceFactory.GetInstance().GetAnswerInterface(parent, this.GetCurrentProblem().answerType);
    }

    public QuestionBehaviour GetCurrentQuestionBehaviour(Transform parent)
    {
        return InterfaceFactory.GetInstance().GetQuestionInterface(parent, this.GetCurrentProblem().questionType);
    }

    public AnswerBehaviour.AnswerData GetCurrentAnswerData()
    {
        return this.GetCurrentProblem().answerData;
    }

    public QuestionBehaviour.QuestionData GetCurrentQuestionData()
    {
        return this.GetCurrentProblem().questionData;
    }




    public void CreateDebugStory(string name)
    {
        Story myStory = new Story(name);
        myStory.SetStoryVersion(0, 0, 0);
        myStory.SetEditorVersion(0, 0, 0);
        myStory.SetDescription("A demo story made for testing.");
        myStory.SetIntroduction("Demo Introduction", "The introduction to the demo story.", 0);


        myStory.AddImage("Bob", "https://upload.wikimedia.org/wikipedia/en/c/c5/Bob_the_builder.jpg");
        myStory.AddImage("Bill", "https://tinyclipart.com/resource/bill-clipart/bill-clipart-148.jpg");
        myStory.AddImage("Duck", "https://images-na.ssl-images-amazon.com/images/I/41sfz8dKX1L._SL500_AC_SS350_.jpg");
        myStory.AddImage("Shark", "https://ih1.redbubble.net/image.345993783.8678/st%2Csmall%2C215x235-pad%2C210x230%2Cf8f8f8.lite-1.jpg");

        myStory.AddImage("Eggs", "https://qph.ec.quoracdn.net/main-qimg-a134e103f988c48d7cf42978f4c40de8-c");
        myStory.AddImage("Avocados", "https://static.seekingalpha.com/uploads/2017/11/916375_15102053376085_rId5.jpg");
        myStory.AddImage("FriedEgg", "https://www.rodalesorganiclife.com/sites/rodalesorganiclife.com/files/styles/listicle_slide_custom_user_phone_1x/public/brainfood-eggs-1000.jpg?itok=--rN3XHc");
        myStory.AddImage("Eggplant", "http://cdn.shopify.com/s/files/1/0206/9470/products/Eggplant__baby_eb64eca5-fd42-4db6-9238-ead8fe34572b_grande.jpeg?v=1477037957");

        myStory.AddProblem
            (1,
            QuestionBehaviour.QuestionType.TextBox,
            new TextBoxQuestion.TextBoxQuestionData(
                new List<TextBoxQuestion.TextBox>(
                    new TextBoxQuestion.TextBox[]
                    {
                        new TextBoxQuestion.TextBox("Bill", true, "Sharing is caring! Bob and Bill needs to share thier 10 candies so that they both have 1/2 of all thier candies."),
                        new TextBoxQuestion.TextBox("Bob", false, "Select the option that have numbers equals to the way they should share.")
                    })),
            AnswerBehaviour.AnswerType.Choices,
            new ChoiceAnswer.ChoiceAnswerData(
                new List<string>(
                    new string[]
                    {
                        "Bob 2 and Bill 8",
                        "Bob 4 and Bill 6",
                        "Bob 5 and Bill 5",
                        "Bob 7 and Bill 3",
                        "Bob 9 and Bill 1"
                    }), 2, 1));

        myStory.AddProblem
        (2,
        QuestionBehaviour.QuestionType.TextBox,
        new TextBoxQuestion.TextBoxQuestionData(
            new List<TextBoxQuestion.TextBox>(
                new TextBoxQuestion.TextBox[]
                {
                            new TextBoxQuestion.TextBox("Duck", true, "One Duck laid 4 eggs."),
                            new TextBoxQuestion.TextBox("Duck", true, "One of the eggs got destroyed!"),
                            new TextBoxQuestion.TextBox("Duck", true, "How many fractions of eggs remains?")
                })),
        AnswerBehaviour.AnswerType.Choices,
        new ChoiceAnswer.ChoiceAnswerData(
            new List<string>(
                new string[]
                {
                            "3/4 of all eggs remains.",
                            "2/4 of all eggs remains.",
                            "1/4 of all eggs remains.",
                            "30/4 of all eggs remains.",
                            "No eggs remains."
                }), 0, 1));

        myStory.AddProblem
        (3,
        QuestionBehaviour.QuestionType.TextBox,
        new TextBoxQuestion.TextBoxQuestionData(
            new List<TextBoxQuestion.TextBox>(
                new TextBoxQuestion.TextBox[]
                {
                                    new TextBoxQuestion.TextBox("Shark", true, "Sharks are dangerous!"),
                                    new TextBoxQuestion.TextBox("Shark", false, "Sharks are flying underwater...!"),
                                    new TextBoxQuestion.TextBox("Shark", true, "Sharks can have 9000000 jaws!"),
                                    new TextBoxQuestion.TextBox("Shark", false, "1/8 of our sharks jaws dropped!"),
                                    new TextBoxQuestion.TextBox("Shark", true, "How many eggs remains?")
                })),
        AnswerBehaviour.AnswerType.Choices,
        new ChoiceAnswer.ChoiceAnswerData(
            new List<string>(
                new string[]
                {
                                    "Eggs",
                                    "Avocados",
                                    "FriedEgg",
                                    "Eggplant"
                }), 1, 0));

        myStory.AddProblem(4,
        QuestionBehaviour.QuestionType.TextBox,
        new TextBoxQuestion.TextBoxQuestionData(
            new List<TextBoxQuestion.TextBox>(
                new TextBoxQuestion.TextBox[]{
                                    new TextBoxQuestion.TextBox("Shark", true, "Sharks are dangerous!"),
                                    new TextBoxQuestion.TextBox("Shark", false, "Sharks are flying underwater...!"),
                                    new TextBoxQuestion.TextBox("Shark", true, "Sharks can have 9000000 jaws!"),
                                    new TextBoxQuestion.TextBox("Shark", false, "1/8 of our sharks jaws dropped!"),
                                    new TextBoxQuestion.TextBox("Shark", true, "How many eggs remains?")}
                )),
        AnswerBehaviour.AnswerType.ARSelection,
        new ARSelectionAnswer.ARSElectionAnswerData(
            new List<ARObjectType>(new ARObjectType[]{
                                    ARObjectType.BlackRod,
                                    ARObjectType.BlueRod,
                                    ARObjectType.BrownRod,
                                    ARObjectType.DarkGreenRod,
                                    ARObjectType.LightGreenRod,
                                    ARObjectType.OrangeRod,
                                    ARObjectType.PurpleRod,
                                    ARObjectType.RedRod,
                                    ARObjectType.WhiteRod,
                                    ARObjectType.YellowRod }),
            new List<ARObjectType>(new ARObjectType[]{
                                    ARObjectType.BlackRod,
                                    ARObjectType.YellowRod})));

        myStory.AddPath("One Problem Path", "This path has only one problem.", 1);

        myStory.AddPath("One Line Path", "This path has all problems in order", 1);
        myStory.paths[2].pathPoint.nextEvents.Add(new Story.Event(2));
        myStory.paths[2].pathPoint.nextEvents[0].nextEvents.Add(new Story.Event(3));
        myStory.paths[2].pathPoint.nextEvents[0].nextEvents[0].nextEvents.Add(new Story.Event(4));

        myStory.AddPath("A Path with Branch!", "This path has a branch", 1);
        myStory.paths[3].pathPoint.nextEvents.Add(new Story.Event(2));
        myStory.paths[3].pathPoint.nextEvents.Add(new Story.Event(3));
        myStory.paths[3].pathPoint.nextEvents[0].nextEvents.Add(new Story.Event(4));
        myStory.paths[3].pathPoint.nextEvents[1].nextEvents.Add(myStory.paths[3].pathPoint.nextEvents[0].nextEvents[0]);

        this.currentStory = myStory;

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.All;
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

        File.WriteAllText(name + ".json", JsonConvert.SerializeObject(myStory, settings));
    }

}
