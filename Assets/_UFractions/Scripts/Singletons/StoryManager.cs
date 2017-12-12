using System.IO;
using System.Linq;
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

    private string localStoryNames = "LocalStory";
    private string localStoryFolderName = "LocalStories"; // This is the folder name that will store stories in the persistent data path.

    private string streamingStoryNames = "StreamingStory"; // If you want to change this you must also change the name of the default stories in StreamingAssets.
    private string streamingStoryFolderName = "StreamingStories"; // If you want to change this you must also change the name of the folder in StreamingAssets.


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
        this.SetProblemSolved(false);

        string storiesPath = Path.Combine(Application.persistentDataPath, this.localStoryFolderName);

        if (!Directory.Exists(storiesPath))
            Directory.CreateDirectory(storiesPath);

        if (Application.platform == RuntimePlatform.Android)
        {
            bool noMoreStories = false;
            int storyNumber = 1;
            while (!noMoreStories)
            {
                string filePath = Path.Combine(Application.streamingAssetsPath, Path.Combine(this.streamingStoryFolderName, this.streamingStoryNames + storyNumber));

                //InterfaceFactory.GetInstance().DebugLog("Loading Streaming Story: " + filePath);

                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
                www.timeout = 3;
                www.SendWebRequest();

                //float time = 0;

                while (!www.isDone && !www.isHttpError && !www.isNetworkError)
                {
                    //time += Time.deltaTime;
                    //if (time > 2)
                    //    break;
                };

                if (www.isDone && !www.isHttpError && !www.isNetworkError)
                {
                    string writePath = Path.Combine(Application.persistentDataPath, Path.Combine(this.localStoryFolderName, this.localStoryNames + storyNumber + ".json"));

                    //InterfaceFactory.GetInstance().DebugLog("Writing Local Story: " + filePath);

                    File.WriteAllText(writePath, www.downloadHandler.text);

                    storyNumber++;
                }
                else
                {
                    noMoreStories = true;
                    //InterfaceFactory.GetInstance().DebugLog("Last Story Not Found. No more Streaming Stories");
                }
            }
        }
        else
        {
            DirectoryInfo directoryInfo = Directory.GetParent(Path.Combine(Application.streamingAssetsPath, this.streamingStoryFolderName));

            string[] baseStories = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, this.streamingStoryFolderName)).Where(name => !name.EndsWith(".meta")).ToArray();

            int t = 1;

            foreach (string s in baseStories)
            {
                string path = Path.Combine(Application.persistentDataPath, Path.Combine(this.localStoryFolderName, this.localStoryNames + t++ + ".json"));
                print("Streaming Story To Local:" + path);
                File.Copy(s, path, true);
            }
        }

        List<Story> localStories = new List<Story>();

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.All;
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        foreach (string s in Directory.GetFiles(Path.Combine(Application.persistentDataPath, this.localStoryFolderName), "*.json"))
        {
            //InterfaceFactory.GetInstance().DebugLog("Parsed Local Story " + s + ".");
            print("Parsed Local Story " + s + ".");
            localStories.Add(Story.LoadFromJSON(s));
        }

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
        this.SetProblemSolved(false);
        return this.currentStory.paths;
    }

    public void SetCurrentPath(Story.Path path)
    {
        this.currentPath = path;
        this.currentPoint = this.currentPath.pathEvent;
    }

    public Story.Problem GetProblem(uint ID)
    {
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

    public AnswerBehaviour GetCurrentAnswerBehaviour(Transform parent, uint problemID)
    {
        return InterfaceFactory.GetInstance().GetAnswerInterface(parent, this.GetProblem(problemID).answerType);
    }

    public QuestionBehaviour GetCurrentQuestionBehaviour(Transform parent, uint problemID)
    {
        return InterfaceFactory.GetInstance().GetQuestionInterface(parent, this.GetProblem(problemID).questionType);
    }

    public AnswerBehaviour.AnswerData GetCurrentAnswerData(uint problemID)
    {
        return this.GetProblem(problemID).answerData;
    }

    public QuestionBehaviour.QuestionData GetCurrentQuestionData(uint problemID)
    {
        return this.GetProblem(problemID).questionData;
    }


    public void CreateDebugStory(string name)
    {
        Story myStory = new Story(name);
        myStory.SetStoryVersion(0, 0, 0);
        myStory.SetEditorVersion(0, 0, 0);
        myStory.SetDescription("Mother leopard has given birth to a cub whose name is Senatla. The leopard is a rare and endangered animal and predators had killed the previous cubs mother leopard had given birth to. In this story you will help mother leopard to raise her cub and teach him different kinds of skills so that he will be able to survive in the forestalone.");
        myStory.SetIntroduction("Mother introduction", "Meet with the mother leopard and learn how to help her and her cub.", 1);

        myStory.AddImage("Phone", "https://openclipart.org/image/2400px/svg_to_png/66883/1276610754.png");
        myStory.AddImage("Mother Leopard", "http://www.clipartoday.com/_thumbs/022/Nature/animals_creature_197072_tnb.png");
        myStory.AddImage("Cub Leopard", "http://clipartsign.com/upload/2016/03/01/free-jaguar-clipart-3.png");

        // Introduction problem with text only.
        myStory.AddProblem(
            1, // ID of the problem
            QuestionBehaviour.QuestionType.TextBox, // QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData( new List<TextBoxQuestion.TextBox>( new TextBoxQuestion.TextBox[] // The QuestionData
                    {
                        new TextBoxQuestion.TextBox("Phone", true, "Mother leopard gives birth to a cub whose name is Senatla. Leopard is a " +
                        "rare and endangered animal and predators had killed the previous cubs Mother leopard had given birth to."),
                        new TextBoxQuestion.TextBox("Cub Leopard", false, "Senatla is in a quite weak condition, so odds are high that he would not live " +
                        "to be much older. Leopard cubs are completely helpless during the first few months and there are several threats during their first year."),
                        new TextBoxQuestion.TextBox("Phone", true, "Through this story you can help mother leopard to raise her cub and teach " +
                        "him different kinds of skills so that he will be able to survive in the forest alone."),
                        new TextBoxQuestion.TextBox("Phone", true, "While playing this game and helping Mother and Senatla you will learn " +
                        "fractions and many things of leopard’s life. With the help of cuisenaire rods you will solve different kinds of problems. " +
                        "From each solved task you will get points that help Senatla cub.")

                    })), 
            AnswerBehaviour.AnswerType.None, // The AnswerType of the problem
            null
            ); // Answer Index, Answer Choices Type 

        /// First introduction problem.
        myStory.AddProblem(
            2, // ID of the problem
            QuestionBehaviour.QuestionType.TextBox, // QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] // The QuestionData
                    {
                        new TextBoxQuestion.TextBox("Cub Leopard", false, "Yellow rod represents one day and orange rod rod is all the days that Senatla has lived. " +
                        "How many days Senatla has lived?"),
                        new TextBoxQuestion.TextBox("Phone", true, "Hint: Take two yellow rods and compare their length to the length of orange rod.")

                    })),
            AnswerBehaviour.AnswerType.Input, // The AnswerType of the problem
            new InputAnswer.InputAnswerData(new List<string>(new string[] // The AnswerData
                    {
                        "2",
                        "two",
                        "two yellow",
                        "two yellow rods"
                    }),
                    new List<ARObjectType>(new ARObjectType[]
                    {
                        ARObjectType.OrangeRod,
                        ARObjectType.YellowRod,
                        ARObjectType.YellowRod
                    })));

        /// Second introduction problem.
        myStory.AddProblem(
            3, // ID of the problem
            QuestionBehaviour.QuestionType.TextBox, // QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] // The QuestionData
                    {
                                    new TextBoxQuestion.TextBox("Cub Leopard", false, "Green rod represents the number of Senatla’s legs and light " +
                                    "blue rod is one leg. How do you mark one leg’s portion of the " +
                                    "whole number of legs as a fraction ? "),
                                    new TextBoxQuestion.TextBox("Phone", true, "Hint: First find out how many light blue rods equal green rod and then think about the fraction.")
                    })),
            AnswerBehaviour.AnswerType.Choices, // The AnswerType of the problem
            new ChoiceAnswer.ChoiceAnswerData(new List<string>(new string[] // The AnswerData
                    {
                                    "1/2",
                                    "2/5",
                                    "1/4",
                                    "1/8"
                    }), 2, 1)); // Answer Index, Answer Choices Type 

        /// Third introduction problem.
        myStory.AddProblem(
            4, // ID of the problem
            QuestionBehaviour.QuestionType.TextBox, // QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] // The QuestionData
                    {
                        new TextBoxQuestion.TextBox("Mother Leopard", false, "Red rod is 8 kg. Black rod tells Mother leopard’s weight. What is Mother leopard’s weight?"),
                        new TextBoxQuestion.TextBox("Phone", true, "Hint: First find out how many red rods equal one black rod.")

                    })),
            AnswerBehaviour.AnswerType.Input, // The AnswerType of the problem
            new InputAnswer.InputAnswerData(new List<string>(new string[] // The AnswerData
                    {
                        "32",
                        "32 kg",
                        "thirty two",
                        "thirty two kg"
                    }),
                    new List<ARObjectType>(new ARObjectType[]
                    {
                        ARObjectType.BlackRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod
                    })));

        /// Add a problem with textbox question and choice answer
        myStory.AddProblem(
            5, // ID of the problem
            QuestionBehaviour.QuestionType.TextBox, // QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] // The QuestionData
                {
                            new TextBoxQuestion.TextBox("Mother Leopard", false, "Let us assume that blue rod is 240 cm. Mother leopards height is 80 cm. " +
                            "Highlight only the rod that represents Mother Leopards height. You can highlight a rod by touching it after pressing the exclamation mark on the right."),
                            new TextBoxQuestion.TextBox("Phone", true, "Hint: Find out which rod is one third of the blue rod.")
                })),
            AnswerBehaviour.AnswerType.ARSelection, // The AnswerType of the problem
            new ARSelectionAnswer.ARSElectionAnswerData(new List<ARObjectType>(new ARObjectType[] // The AnswerData
            {
                                    ARObjectType.BlueRod,
                                    ARObjectType.LightGreenRod,
                                    ARObjectType.PurpleRod,
                                    ARObjectType.RedRod,
                                    ARObjectType.WhiteRod,
                                    ARObjectType.BrownRod
            }),
            new List<ARObjectType>(new ARObjectType[]
            {
                                    ARObjectType.LightGreenRod
            })));

        myStory.paths[0].pathEvent.AddPoint(2);
        myStory.paths[0].pathEvent.AddPoint(3);
        myStory.paths[0].pathEvent.nextEvents[0].AddPoint(4);
        myStory.paths[0].pathEvent.nextEvents[0].nextEvents[0].AddPoint(5);
        myStory.paths[0].pathEvent.nextEvents[1].AddPoint(myStory.paths[0].pathEvent.nextEvents[0].nextEvents[0]);

        this.currentStory = myStory;

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.All;
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

        File.WriteAllText(name + ".json", JsonConvert.SerializeObject(myStory, settings));
    }

}
