﻿using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// A serializable class that is used to store the contents of a story.
/// Note: It uses interfaces to store data for different problems and dictionaries so it needs to use a JSON parser that can handle inheritance.
/// </summary>
[Serializable] public class Story
{
    /// <summary>
    /// Stores simple version data.
    /// </summary>
    [Serializable] public struct Version
    {
        /// <summary>
        /// The main version.
        /// </summary>
        public uint main;

        /// <summary>
        /// The patch version.
        /// </summary>
        public uint patch;

        /// <summary>
        /// The bug version.
        /// </summary>
        public uint bug;

        /// <summary>
        /// Sets version numbers on construction.
        /// </summary>
        /// <param name="main">The main version to set.</param>
        /// <param name="patch">The patch version to set.</param>
        /// <param name="bug">The bug version to set.</param>
        public Version(uint main, uint patch, uint bug)
        {
            this.main = main;
            this.patch = patch;
            this.bug = bug;
        }
    }

    /// <summary>
    /// Contains required data for the game to setup up a problem in the problem scene.
    /// </summary>
    [Serializable] public class Problem
    {
        /// <summary>
        /// The ID number of the problem. Needs to be unique for every problem in a story.
        /// </summary>
        public uint problemID;

        /// <summary>
        /// The type of question interface the problem will be using.
        /// </summary>
        public QuestionBehaviour.QuestionType questionType;

        /// <summary>
        /// The question data that will be used to setup the problem. Must be the data interface of the question interface type used.
        /// </summary>
        public QuestionBehaviour.QuestionData questionData;

        /// <summary>
        /// The type of answer interface the problem will be using.
        /// </summary>
        public AnswerBehaviour.AnswerType answerType;

        /// <summary>
        /// The answer data that will be used to setup the problem. Must be the data interface of the answer interface type used.
        /// </summary>
        public AnswerBehaviour.AnswerData answerData;

        /// <summary>
        /// Sets Problem variables on construction.
        /// </summary>
        /// <param name="problemID">The ID of the problem.</param>
        /// <param name="questionType">The question interface type for the problem.</param>
        /// <param name="questionData">The question data for the problem.</param>
        /// <param name="answerType">The answer interface type for the problem.</param>
        /// <param name="answerData">The answer data for the problem.</param>
        public Problem(uint problemID, QuestionBehaviour.QuestionType questionType, QuestionBehaviour.QuestionData questionData, AnswerBehaviour.AnswerType answerType, AnswerBehaviour.AnswerData answerData)
        {
            this.problemID = problemID;
            this.questionType = questionType;
            this.questionData = questionData;
            this.answerType = answerType;
            this.answerData = answerData;
        }

        /// <summary>
        /// Checks if this problem has an answer.
        /// </summary>
        /// <returns>Returns true if it does.</returns>
        public bool HasAnswer()
        {
            if (this.answerType == AnswerBehaviour.AnswerType.None)
                return false;
            else
                return true;
        }
    }

    /// <summary>
    /// Contains a problem ID and a list of Events.
    /// </summary>
    [Serializable] public struct Event
    {
        /// <summary>
        /// This events problems ID.
        /// </summary>
        public uint problemID;

        /// <summary>
        /// The list of events that continues after this event. Leave empty if event is last in a path.
        /// </summary>
        public List<Event> nextEvents;

        /// <summary>
        /// Constructor with problem ID parameter.
        /// </summary>
        /// <param name="problemID">The problem ID.</param>
        public Event(uint problemID)
        {
            this.problemID = problemID;
            this.nextEvents = new List<Event>();
        }

        /// <summary>
        /// Adds a new event to the next event list of this event.
        /// </summary>
        /// <param name="problemID">The problem ID of the new event.</param>
        public void AddPoint(uint problemID)
        {
            this.nextEvents.Add(new Event(problemID));
        }

        /// <summary>
        /// Adds an event to the next event list of this event.
        /// </summary>
        /// <param name="pathEvent">The event to add.</param>
        public void AddPoint(Story.Event pathEvent)
        {
            this.nextEvents.Add(pathEvent);
        }
    }

    /// <summary>
    /// Contains a path of events.
    /// </summary>
    [Serializable] public class Path
    {
        /// <summary>
        /// The name of the path.
        /// </summary>
        public string name;

        /// <summary>
        /// The description of the path.
        /// </summary>
        public string description;

        /// <summary>
        /// The starting event of the path.
        /// </summary>
        public Event pathEvent;

        /// <summary>
        /// Sets the variables of Path on construction.
        /// </summary>
        /// <param name="name">The name of the path.</param>
        /// <param name="description">The description of the path.</param>
        /// <param name="startingProblemID">The first events problems ID.</param>
        public Path(string name, string description, uint startingProblemID)
        {
            this.name = name;
            this.description = description;
            pathEvent = new Event(startingProblemID);
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
