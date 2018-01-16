using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// A singleton class that manages stories and enables other classes to handle progression and problem construction.
/// 
/// Use StoryManager.Instance to access.
/// 
/// To setup base stories read SetupStreamingStories() function documentation.
/// </summary>
public class StoryManager : Singleton<StoryManager>
{
    /// <summary>
    /// Instantiates needed members and sets Json serialization settings.
    /// </summary>
    protected StoryManager()
    {
        this.currentStoryTextures = new Dictionary<string, Texture2D>();
        this.jsonSettings = new JsonSerializerSettings();
        this.jsonSettings.TypeNameHandling = TypeNameHandling.All;
        this.jsonSettings.Formatting = Formatting.Indented;
        this.jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        this.jsonSettings.MaxDepth = 99;
    }

    public Story currentStory;
    private Story.Path currentPath;
    private Story.Event currentPoint;
    private Story.Problem currentProblem;
    private bool lastProblemSolved = false;
    private bool streamingSetupDone = false;
    private WWW www;

    private Dictionary<string, Texture2D> currentStoryTextures;

    private JsonSerializerSettings jsonSettings;

    private string localStoryNames = "LocalStory";
    private string localStoryFolderName = "LocalStories"; // This is the folder name that will store stories in the persistent data path.

    private string streamingStoryNames = "StreamingStory"; // If you want to change this you must also change the name of the default stories in StreamingAssets.
    private string streamingStoryFolderName = "StreamingStories"; // If you want to change this you must also change the name of the folder in StreamingAssets.


    /// <summary>
    /// Locates all defined stories that are located in the StreamingAsset folder under 
    /// streamingStoryFolderName and places them in the PersistentDataPath folder under localStoryFolderName.
    /// 
    /// Stories in StreamingAssets must be named after streamingStoryNames in order with numbers from 1 and upward without any filetype endings.
    /// </summary>
    public void SetupStreamingStories()
    {
        /// Get path to local stories
        string storiesPath = Path.Combine(Application.persistentDataPath, this.localStoryFolderName);

        /// Create local story folder if it does not exist
        if (!Directory.Exists(storiesPath))
            Directory.CreateDirectory(storiesPath);

        /// If we are running on android
        if (Application.platform == RuntimePlatform.Android)
        {
            bool noMoreStories = false;
            int storyNumber = 1;

            /// While there are no more streaming stories
            while (!noMoreStories)
            {
                /// Get path for the next potential streaming story
                string filePath = Path.Combine(Application.streamingAssetsPath, Path.Combine(this.streamingStoryFolderName, this.streamingStoryNames + storyNumber));

                /// Try to load story file from apk with WWW
                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
                www.timeout = 3;
                www.SendWebRequest();

                /// Wait until WWW is done or throws exception
                while (!www.isDone && !www.isHttpError && !www.isNetworkError)
                {

                };

                /// If the WWW is done without error
                if (www.isDone && !www.isHttpError && !www.isNetworkError)
                {
                    /// Create local story path
                    string writePath = Path.Combine(Application.persistentDataPath, Path.Combine(this.localStoryFolderName, this.localStoryNames + storyNumber + ".json"));

                    /// Save streaming story to local
                    File.WriteAllText(writePath, www.downloadHandler.text);

                    storyNumber++;
                }
                /// Otherwise there are no more streaming stories
                else
                {
                    noMoreStories = true;
                }
            }
        }
        else
        {
            string[] baseStories = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, this.streamingStoryFolderName)).Where(name => !name.EndsWith(".meta")).ToArray();

            int t = 1;

            foreach (string s in baseStories)
            {
                string path = Path.Combine(Application.persistentDataPath, Path.Combine(this.localStoryFolderName, this.localStoryNames + t++ + ".json"));
                print("Streaming Story To Local:" + path);
                File.Copy(s, path, true);
            }
        }

    }

    /// <summary>
    /// Download all images needed for the current selected story.
    /// </summary>
    private void SetupCurrentStoryImages()
    {
        /// Clear local stories images
        this.currentStoryTextures.Clear();

        /// For every name and url keypair download and store image.
        foreach (KeyValuePair<string, string> kvp in this.currentStory.imgUrlsDictionary)
        {
            www = new WWW(kvp.Value);
            
            while (!www.isDone && www.error == null)
            {

            }
            if (www.isDone)
            {
                print("Image Loaded " + kvp.Key);
                this.currentStoryTextures.Add(kvp.Key, www.texture);
            }

            www.Dispose();
        }
    }


    /// <summary>
    /// Get the path url or system for a sepecific video of the current selected story.
    /// </summary>
    /// <param name="name">The name of the video.</param>
    /// <returns>The path.</returns>
    public string GetVideoPath(string name)
    {
        return this.currentStory.vidUrlsDictionary[name];
    }

    /// <summary>
    /// Get the path url or system for a sepecific image of the current selected story.
    /// </summary>
    /// <param name="name">The name of the image.</param>
    /// <returns>The path.</returns>
    public string GetImagePath(string name)
    {
        return this.currentStory.imgUrlsDictionary[name];
    }

    /// <summary>
    /// Use to get a texture from an image in the current story.
    /// </summary>
    /// <param name="name">The name of the image.</param>
    /// <returns>A texture with the image of given name.</returns>
    public Texture GetImageTexture(string name)
    {
        if (this.currentStoryTextures.ContainsKey(name))
            return this.currentStoryTextures[name];
        else
        {
            return new Texture();
        }
    }

    /// <summary>
    /// Use to get a sprite from an image in the current story.
    /// </summary>
    /// <param name="name">The name of the image.</param>
    /// <returns>A sprite with the image of given name.</returns>
    public Sprite GetImageSprite(string name)
    {
        if (this.currentStoryTextures.ContainsKey(name))
            return Sprite.Create(this.currentStoryTextures[name], new Rect(0, 0, this.currentStoryTextures[name].width, this.currentStoryTextures[name].height), new Vector2(0.1f, 0.1f));
        else
            return new Sprite();
    }


    /// <summary>
    /// Used by the problem controller to set last problem to solved.
    /// </summary>
    /// <param name="solved">If the problem was solved.</param>
    public void SetProblemSolved(bool solved)
    {
        this.lastProblemSolved = solved;
    }

    /// <summary>
    /// Used by the path progression to check if the last problem was solved.
    /// </summary>
    /// <returns>True if the last problem was solved.</returns>
    public bool LastProblemSolved()
    {
        return this.lastProblemSolved;
    }


    /// <summary>
    /// Use to get a list of all the stories stored localy.
    /// </summary>
    /// <returns>A list of the local stories.</returns>
    public List<Story> GetLocalStories()
    {
        /// If streaming stories has not been loaded before
        if (!this.streamingSetupDone)
        {
            /// Load streaming stories into local
            this.SetupStreamingStories();
            this.streamingSetupDone = true;
        }

        /// Reset problem solved for safety
        this.SetProblemSolved(false);

        List<Story> localStories = new List<Story>();

        /// Get all the local stories and parse them with json
        foreach (string s in Directory.GetFiles(Path.Combine(Application.persistentDataPath, this.localStoryFolderName), "*.json"))
        {
            localStories.Add(Story.LoadFromJSON(s, this.jsonSettings));
        }

        return localStories;
    }

    /// <summary>
    /// Use to get a string representing the path to the local stories folder.
    /// </summary>
    /// <returns>Local stories path as string.</returns>
    public string GetLocalStoriesPath()
    {
        return Path.Combine(Application.persistentDataPath, this.localStoryFolderName);
    }

    /// <summary>
    /// Clears local stories from the local story folder. Will reset streaming stories to not setup.
    /// </summary>
    public void ClearLocalStories()
    {
        this.streamingSetupDone = false;

        System.IO.DirectoryInfo di = new DirectoryInfo(this.GetLocalStoriesPath());

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    /// <summary>
    /// Sets the current story.
    /// </summary>
    /// <param name="story">The story to set as current story.</param>
    public void SetCurrentStory(Story story)
    {
        this.currentStory = story;
        this.SetupCurrentStoryImages();
    }

    /// <summary>
    /// Sets the current story.
    /// </summary>
    /// <param name="filePath">The path to the story json file.</param>
    public void SetCurrentStory(string filePath)
    {
        this.SetCurrentStory(Story.LoadFromJSON(filePath));
        this.SetupCurrentStoryImages();
    }


    /// <summary>
    /// Sets the current story path.
    /// </summary>
    /// <param name="path">The path to set as current path.</param>
    public void SetCurrentPath(Story.Path path)
    {
        this.currentPath = path;
        this.currentPoint = this.currentPath.pathEvent;
    }

    /// <summary>
    /// Use to get all paths in current story.
    /// </summary>
    /// <returns>A list of paths.</returns>
    public List<Story.Path> GetAllPaths()
    {
        this.SetProblemSolved(false);
        return this.currentStory.paths;
    }


    /// <summary>
    /// Use to get a specific problem from the current story.
    /// </summary>
    /// <param name="ID">The id of the problem to get.</param>
    /// <returns>The story problem.</returns>
    public Story.Problem GetProblem(uint ID)
    {
        foreach (Story.Problem p in this.currentStory.problems)
        {
            if (p.problemID == ID)
                return p;
        }
        return null;
    }

    /// <summary>
    /// Use to get the problem of the current event in the current path.
    /// </summary>
    /// <returns>The current problem.</returns>
    public Story.Problem GetCurrentProblem()
    {
        return this.GetProblem(this.currentPoint.problemID);
    }


    /// <summary>
    /// Sets current event.
    /// </summary>
    /// <param name="storyEvent">The event to set as current.</param>
    public void SetCurrentEvent(Story.Event storyEvent)
    {
        this.currentPoint = storyEvent;
    }

    /// <summary>
    /// Use to get current event.
    /// </summary>
    /// <returns>The current event.</returns>
    public Story.Event GetCurrentEvent()
    {
        return this.currentPoint;
    }


    /// <summary>
    /// Creates and returns the AnswerBehaviour interface of the current problem.
    /// This requires there to be an instance of InterfaceFactory setup in the current scene.
    /// </summary>
    /// <param name="parent">The parent the interface should have.</param>
    /// <returns>The AnswerBehaviour interface.</returns>
    public AnswerBehaviour GetCurrentAnswerBehaviour(Transform parent)
    {
        return InterfaceFactory.GetInstance().GetAnswerInterface(parent, this.GetCurrentProblem().answerType);
    }

    /// <summary>
    /// Creates and returns the QuestionBehaviour interface of the current problem.
    /// This requires there to be an instance of InterfaceFactory setup in the current scene.
    /// </summary>
    /// <param name="parent">The parent the interface should have.</param>
    /// <returns>The QuestionBehaviour interface.</returns>
    public QuestionBehaviour GetCurrentQuestionBehaviour(Transform parent)
    {
        return InterfaceFactory.GetInstance().GetQuestionInterface(parent, this.GetCurrentProblem().questionType);
    }

    /// <summary>
    /// Returns the AnswerData of the current problem.
    /// </summary>
    /// <returns>The AnswerData.</returns>
    public AnswerBehaviour.AnswerData GetCurrentAnswerData()
    {
        return this.GetCurrentProblem().answerData;
    }

    /// <summary>
    /// Returns the QuestionData of the current problem.
    /// </summary>
    /// <returns>The QuestionData.</returns>
    public QuestionBehaviour.QuestionData GetCurrentQuestionData()
    {
        return this.GetCurrentProblem().questionData;
    }

    /// <summary>
    /// Creates and returns the AnswerBehaviour interface of the given problem.
    /// This requires there to be an instance of InterfaceFactory setup in the current scene.
    /// </summary>
    /// <param name="parent">The parent the interface should have.</param>
    /// <param name="problemID">The id of the problem to get the AnswerBehaviour from.</param>
    /// <returns>The AnswerBehaviour interface.</returns>
    public AnswerBehaviour GetAnswerBehaviour(Transform parent, uint problemID)
    {
        return InterfaceFactory.GetInstance().GetAnswerInterface(parent, this.GetProblem(problemID).answerType);
    }

    /// <summary>
    /// Creates and returns the QuestionBehaviour interface of the given problem.
    /// This requires there to be an instance of InterfaceFactory setup in the current scene.
    /// </summary>
    /// <param name="parent">The parent the interface should have.</param>
    /// <param name="problemID">The id of the problem to get the QuestionBehaviour from.</param>
    /// <returns>The QuestionBehaviour interface.</returns>
    public QuestionBehaviour GetQuestionBehaviour(Transform parent, uint problemID)
    {
        return InterfaceFactory.GetInstance().GetQuestionInterface(parent, this.GetProblem(problemID).questionType);
    }

    /// <summary>
    /// Returns the AnswerData of the given problem.
    /// </summary>
    /// <param name="problemID">The id of the problem to get the AnswerData from.</param>
    /// <returns>The AnswerData.</returns>
    public AnswerBehaviour.AnswerData GetAnswerData(uint problemID)
    {
        return this.GetProblem(problemID).answerData;
    }

    /// <summary>
    /// Returns the QuestionData of the given problem.
    /// </summary>
    /// <param name="problemID">The id of the problem to get the QuestionData from.</param>
    /// <returns>The QuestionData.</returns>
    public QuestionBehaviour.QuestionData GetCurrentQuestionData(uint problemID)
    {
        return this.GetProblem(problemID).questionData;
    }


    /// <summary>
    /// Used to create a hardcoded story. 
    /// Study this to see how a story can be built when designing the editor.
    /// </summary>
    /// <param name="name">The name of the story.</param>
    public void CreateDebugStory(string name)
    {
        /// Declare the story
        Story myStory = new Story(name);

        /// Set version variables
        myStory.SetStoryVersion(0, 0, 0);
        myStory.SetEditorVersion(0, 0, 0);

        /// Set story description
        myStory.SetDescription("Mother leopard has given birth to a cub whose name is Senatla. The leopard is a rare and endangered animal and predators had killed the previous cubs mother leopard had given birth to. In this story you will help mother leopard to raise her cub and teach him different kinds of skills so that he will be able to survive in the forestalone.");
        myStory.SetIntroduction("Mother introduction", "Meet with the mother leopard and learn how to help her and her cub.", 1);

        /// Add some images to use in story
        myStory.AddImage("Phone", "https://openclipart.org/image/2400px/svg_to_png/66883/1276610754.png");
        myStory.AddImage("Mother Leopard", "http://www.clipartoday.com/_thumbs/022/Nature/animals_creature_197072_tnb.png");
        myStory.AddImage("Cub Leopard", "http://clipartsign.com/upload/2016/03/01/free-jaguar-clipart-3.png");

        /// Create Introduction problem with text only.
        myStory.AddProblem(
            1, /// ID of the problem, must be unique
            QuestionBehaviour.QuestionType.TextBox, /// QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData( new List<TextBoxQuestion.TextBox>( new TextBoxQuestion.TextBox[] /// The QuestionData as a list of strings
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

                    }), null), /// A list of ARObject types to spawn in the scene, null and no items will spawn from this question component
            AnswerBehaviour.AnswerType.None, /// The AnswerType of the problem, None will have no answer just story
            null /// Set data as null when None as type.
            );

        /// Create first introduction problem with textbox question and input answer
        myStory.AddProblem(
            2, /// ID of the problem
            QuestionBehaviour.QuestionType.TextBox, /// QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] /// The QuestionData
                    {
                        new TextBoxQuestion.TextBox("Cub Leopard", false, "Yellow rod represents one day and orange rod rod is all the days that Senatla has lived. " +
                        "How many days Senatla has lived?"),
                        new TextBoxQuestion.TextBox("Phone", true, "Hint: Take two yellow rods and compare their length to the length of orange rod.")

                    }), new List<ARObjectType>( new ARObjectType[] 
                    {
                        ARObjectType.YellowRod,
                        ARObjectType.YellowRod,
                        ARObjectType.OrangeRod
                    } )),
            AnswerBehaviour.AnswerType.Input, /// The AnswerType of the problem
            new InputAnswer.InputAnswerData(new List<string>(new string[] /// The AnswerData of input is a list of strings as accepted answers
                    {
                        "2",
                        "two",
                        "two yellow",
                        "two yellow rods"
                    })));

        /// Create second introduction problem with textbox question and choice answer.
        myStory.AddProblem(
            3, /// ID of the problem
            QuestionBehaviour.QuestionType.TextBox, /// QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] /// The QuestionData
                    {
                                    new TextBoxQuestion.TextBox("Cub Leopard", false, "Brown rod represents the number of Senatla’s legs and " +
                                    "red rod is one leg. How do you mark one leg’s portion of the " +
                                    "whole number of legs as a fraction ? "),
                                    new TextBoxQuestion.TextBox("Phone", true, "Hint: First find out how many red rods equal blue and then think about the fraction.")
                    }),
                    new List<ARObjectType>( new ARObjectType[] /// List of ARObjects to spawn as question text talks about them
                    {
                        ARObjectType.BrownRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod
                    })),
            AnswerBehaviour.AnswerType.Choices, /// The AnswerType of the problem
            new ChoiceAnswer.ChoiceAnswerData(new List<string>(new string[] /// The AnswerData contains a list of text choices
                    {
                                    "1/2",
                                    "2/5",
                                    "1/4",
                                    "1/8"
                    }), 2, 1)); // Answer Index, Answer Choices Type 

        /// Create third introduction problem.
        myStory.AddProblem(
            4, /// ID of the problem
            QuestionBehaviour.QuestionType.TextBox, /// QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] /// The QuestionData
                    {
                        new TextBoxQuestion.TextBox("Mother Leopard", false, "Red rod is 8 kg. Brown rod tells Mother leopard’s weight. What is Mother leopard’s weight?"),
                        new TextBoxQuestion.TextBox("Phone", true, "Hint: First find out how many red rods equal one black rod.")

                    }), new List<ARObjectType>(new ARObjectType[] /// The ARObjects to be spawned with this question
                    {
                        ARObjectType.RedRod,
                        ARObjectType.BrownRod,
                        ARObjectType.RedRod,
                        ARObjectType.RedRod
                    })),
            AnswerBehaviour.AnswerType.Input, // The AnswerType of the problem
            new InputAnswer.InputAnswerData(new List<string>(new string[] // The AnswerData
                    {
                        "32",
                        "32 kg",
                        "thirty two",
                        "thirty two kg"
                    })));

        /// Creates a problem with textbox question and ar selection answer
        myStory.AddProblem(
            5, /// ID of the problem
            QuestionBehaviour.QuestionType.TextBox, /// QuestionType of the problem 
            new TextBoxQuestion.TextBoxQuestionData(new List<TextBoxQuestion.TextBox>(new TextBoxQuestion.TextBox[] /// The QuestionData
                {
                            new TextBoxQuestion.TextBox("Mother Leopard", false, "Let us assume that blue rod is 240 cm. Mother leopards height is 80 cm. " +
                            "Highlight only the rod that represents Mother Leopards height. You can highlight a rod by touching it after pressing the exclamation mark on the right."),
                            new TextBoxQuestion.TextBox("Phone", true, "Hint: Find out which rod is one third of the blue rod.")
                }), null),
            AnswerBehaviour.AnswerType.ARSelection, /// The AnswerType of the problem
            new ARSelectionAnswer.ARSelectionAnswerData(new List<ARObjectType>(new ARObjectType[] /// The AnswerData has List of ARObject types to spawn
            {
                                    ARObjectType.BlueRod,
                                    ARObjectType.LightGreenRod,
                                    ARObjectType.PurpleRod,
                                    ARObjectType.RedRod,
                                    ARObjectType.WhiteRod,
                                    ARObjectType.BrownRod
            }),
            new List<ARObjectType>(new ARObjectType[] /// And a list of ARObjects that is correct answer
            {
                                    ARObjectType.LightGreenRod
            })));

        /// Adding paths is a bit tricky and unoptimized at the moment. Every path has a starting event that has a problem id and a list of events it can branch into.
        myStory.paths[0].pathEvent.AddPoint(2);
        myStory.paths[0].pathEvent.AddPoint(3);
        myStory.paths[0].pathEvent.nextEvents[0].AddPoint(4);
        myStory.paths[0].pathEvent.nextEvents[0].nextEvents[0].AddPoint(5);
        myStory.paths[0].pathEvent.nextEvents[1].AddPoint(myStory.paths[0].pathEvent.nextEvents[0].nextEvents[0]);

        this.currentStory = myStory;

        /// Serialize and save the object to a file
        File.WriteAllText(name + ".json", JsonConvert.SerializeObject(myStory, this.jsonSettings));
    }
}
