using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ARSelectionAnswer : AnswerBehaviour
{
    [Serializable]
    public class ARSElectionAnswerData : AnswerData
    {
        public List<ARObjectType> answare;
        public List<ARObjectType> objects;

        
    }

    List<ARObjectType> hilighChoices;
    List<ARObjectType> answare;
    public List<GameObject> objectPrefab;
    float yPos = 0;

    public override void Show()
    {
        HighlightReaction[] list = GetComponentsInChildren<HighlightReaction>();
        foreach (HighlightReaction h in list)
        {
            h.isActive = true;
        }
    }

    public override void Hide()
    {
        HighlightReaction[] list = GetComponentsInChildren<HighlightReaction>();
        foreach( HighlightReaction h in list)
        {
            h.isActive = false;
        }
    }

    public override void SetupAnswer(AnswerData answer)
    {
        ARSElectionAnswerData data = (ARSElectionAnswerData)answer;
        this.answare = data.answare;
        foreach(ARObjectType t in data.objects)
        {
            this.SpawnObject(t);
        }
    }

    private void SpawnObject(ARObjectType aRObjaectType)
    {
        foreach (GameObject g in this.objectPrefab)
        {
            ArObjectType arObj = g.GetComponent<ArObjectType>();
            if(arObj != null)
            {
                if (arObj.objType == aRObjaectType)
                {
                    Instantiate(g, this.transform).transform.Translate(0, this.yPos, 0);
                    this.yPos++;
                    return;
                }
            }
        }
    }

    public override bool GetResult()
    {
        return this.CompareAnsware();
    }

    /// <summary>
    /// takes in the answare and the list of hilighted rods 
    /// returns TRUE if thay are THE SAME
    /// returns FALSE if the list are NOT the same
    /// NOTE:  dose not support multiple answare alternatives at this point. 
    /// </summary>
    private bool CompareAnsware()
    {

        List<ARObjectType> choice = new List<ARObjectType>(GetSelected());
        if (answare.Count != choice.Count)
        {
            hilighChoices.Clear();
            return false;
        }

        for (int i = 0; i < choice.Count; i++)
        {

            if (!answare.Contains(choice[i]))
            {
                hilighChoices.Clear();
                return false;
            }

        }
        hilighChoices.Clear();
        return true;

    }

    /// <summary>
    /// NOTE: thinking of changing function to "GetSelected"  and let the function only return the selected list. 
    /// function checks if any of the rods are hilighted and if thay are add it to a list.
    /// Calls compare answare with the answare that we send in and the list of hiligthed rods
    /// if thay are the same it returns true otherwise false. 
    /// 
    /// </summary>
    private List<ARObjectType> GetSelected()
    {

        HighlightReaction[] hilights = GetComponentsInChildren<HighlightReaction>();
        ArObjectType[] colorList = GetComponentsInChildren<ArObjectType>();

        for (int i = 0; i < hilights.Length; i++)
        {

            if (hilights[i].hitHighlight == true)
            {

                hilighChoices.Add(colorList[i].GetObjectType());

            }
        }
        return hilighChoices;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
