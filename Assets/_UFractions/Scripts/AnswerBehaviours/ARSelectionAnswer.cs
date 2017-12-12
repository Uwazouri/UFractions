using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ARSelectionAnswer : AnswerBehaviour
{
    /// <summary>
    /// Setups the list off objects and answare. 
    /// </summary>
    [Serializable]
    public class ARSElectionAnswerData : AnswerData
    {
        public List<ARObjectType> answer;
        public List<ARObjectType> objects;

        public ARSElectionAnswerData(List<ARObjectType> objects, List<ARObjectType> answer)
        {
            this.answer = answer;
            this.objects = objects;
        }
    }

    List<ARObjectType> highlightChoices;
    List<ARObjectType> answare;
    public List<GameObject> objectPrefab;
    float yPos = 0;

    // Use this for initialization
    void Start()
    {
        this.highlightChoices = new List<ARObjectType>();
    }

    /// <summary>
    /// Turns ON the possibility to highlight the rods 
    /// </summary>
    public override void Show()
    {
        HighlightReaction[] list = GetComponentsInChildren<HighlightReaction>();
        foreach (HighlightReaction h in list)
        {
            h.isActive = true;
        }
    }

    /// <summary>
    /// Turns OFF the possibility to highlight the rods 
    /// </summary>
    public override void Hide()
    {
        HighlightReaction[] list = GetComponentsInChildren<HighlightReaction>();
        foreach( HighlightReaction h in list)
        {
            h.isActive = false;
        }
    }

    /// <summary>
    /// Setups the list of answare that will be compared to the choices of the player. 
    /// </summary>
    /// <param name="answer"></param>
    public override void SetupAnswer(AnswerData answer)
    {
        ARSElectionAnswerData data = (ARSElectionAnswerData)answer;
        this.answare = data.answer;
        foreach(ARObjectType t in data.objects)
        {
            this.SpawnObject(t);
        }
    }

    /// <summary>
    /// Creats the rods that will be active in the scen
    /// </summary>
    /// <param name="aRObjectType"></param>
    private void SpawnObject(ARObjectType aRObjectType)
    {
        foreach (GameObject g in this.objectPrefab)
        {
            ArObjectType arObj = g.GetComponent<ArObjectType>();
            if(arObj != null)
            {
                if (arObj.objType == aRObjectType)
                {
                    GameObject go = Instantiate(g, this.transform);
                    go.transform.position = new Vector3(0, 1, 0);
                    go.transform.rotation = Quaternion.identity;
                    go.transform.Translate(UnityEngine.Random.Range(-2.0f, 2.0f), this.yPos, UnityEngine.Random.Range(-2.0f, 2.0f));
                    this.yPos += 1.5f;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Returns if the answare is true or false. 
    /// </summary>
    /// <returns>Outcome of the players answare</returns>
    public override bool GetResult()
    {
        return this.CompareAnsware();
    }

    /// <summary>
    /// Compares the list of highlighted rods to the answare list.
    /// </summary>
    private bool CompareAnsware()
    {
        List<ARObjectType> choice = new List<ARObjectType>(GetSelected());

        if (answare.Count != choice.Count)
        {
            highlightChoices.Clear();
            return false;
        }

        for (int i = 0; i < choice.Count; i++)
        {
            if (!answare.Contains(choice[i]))
            {
                highlightChoices.Clear();
                return false;
            }
        }
        highlightChoices.Clear();
        return true;

    }

    /// <summary>
    /// Returns a list of the highlighted rods 
    /// </summary>
    /// <returns>
    /// List of the highlighted rods
    /// </returns>
    private List<ARObjectType> GetSelected()
    {
        HighlightReaction[] highlights = GetComponentsInChildren<HighlightReaction>();
        ArObjectType[] colorList = GetComponentsInChildren<ArObjectType>();

        for (int i = 0; i < highlights.Length; i++)
        {
            if (highlights[i].hitHighlight == true)
            {     
                highlightChoices.Add(colorList[i].GetObjectType());
            }
        }
        return highlightChoices;
    }
}
