using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Story
{
    [Serializable]
    public struct Version
    {
        public uint main;
        public uint patch;
        public uint bug;

        public Version(uint main, uint patch, uint bug)
        {
            this.main = main;
            this.patch = patch;
            this.bug = bug;
        }
    }

    [Serializable]
    public class Problem
    {
        public uint problemID;
        public QuestionBehaviour.QuestionType questionType;
        public QuestionBehaviour.QuestionData questionData;
        public AnswerBehaviour.AnswerType answerType;
        public AnswerBehaviour.AnswerData answerData;

        public Problem(uint problemID, QuestionBehaviour.QuestionType questionType, QuestionBehaviour.QuestionData questionData, AnswerBehaviour.AnswerType answerType, AnswerBehaviour.AnswerData answerData)
        {
            this.problemID = problemID;
            this.questionType = questionType;
            this.questionData = questionData;
            this.answerType = answerType;
            this.answerData = answerData;
        }
    }

    [Serializable]
    public class Path
    {
        public string pathName;
        public string description;
        public Event pathPoint;

        public Path(string name, string description, uint startingProblemID)
        {
            this.pathName = name;
            this.description = description;
            pathPoint = new Event(startingProblemID);
        }
    }

    [Serializable]
    public struct Event
    {
        public uint problemID;
        public List<Event> nextPoints;

        public Event(uint problemID)
        {
            this.problemID = problemID;
            this.nextPoints = new List<Event>();
        }

        public void AddPoint(uint problemID)
        {
            this.nextPoints.Add(new Event(problemID));
        }

        public void AddPoint(Story.Event point)
        {
            this.nextPoints.Add(point);
        }
    }

    public Story(string storyName)
    {
        this.name = storyName;
        this.storyVersion = new Version();
        this.imgUrlsDictionary = new Dictionary<string, string>();
        this.paths = new List<Path>();
        this.problems = new List<Problem>();
    }

    public string name;
    public string description;
    public Version storyVersion;
    public Version editorVersion;
    public List<Path> paths;
    public List<Problem> problems;
    public Dictionary<string, string> imgUrlsDictionary;
    public Dictionary<string, string> vidUrlsDictionary;

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetDescription(string description)
    {
        this.description = description;
    }

    public void SetStoryVersion(uint main, uint patch, uint bug)
    {
        this.storyVersion = new Version(main, patch, bug);
    }

    public void SetEditorVersion(uint main, uint patch, uint bug)
    {
        this.editorVersion = new Version(main, patch, bug);
    }

    public void SetIntroduction(string name, string description, uint startingProblemID)
    {
        this.paths.Insert(0, new Path(name, description, startingProblemID));
    }

    public void AddPath(string name, string description, uint startingProblem)
    {
        this.paths.Add(new Path(name, description, startingProblem));
    }

    public void AddProblem(uint problemID, QuestionBehaviour.QuestionType questionType, QuestionBehaviour.QuestionData questionData, AnswerBehaviour.AnswerType answerType, AnswerBehaviour.AnswerData answerData)
    {
        this.problems.Add(new Problem(problemID, questionType, questionData, answerType, answerData));
    }

    public void AddImage(string name, string path)
    {
        this.imgUrlsDictionary[name] = path;
    }

    public void AddVideo(string name, string path)
    {
        this.vidUrlsDictionary[name] = path;
    }

    public static Story LoadFromJSON(string filePath)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.All;
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

        Story returnStory = JsonConvert.DeserializeObject<Story>(File.ReadAllText(filePath), settings);

        if (returnStory == null)
            Debug.Log("Story could not be loaded from file path.");

        return returnStory;
    }
}
