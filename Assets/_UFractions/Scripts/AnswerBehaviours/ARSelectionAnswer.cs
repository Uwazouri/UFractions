using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ARSelectionAnswer : AnswerBehaviour
{
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
        this.answare = data.answer;
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

        print(hilights.Length + " " + colorList.Length);

        for (int i = 0; i < hilights.Length; i++)
        {

            if (hilights[i].hitHighlight == true)
            {
                print(i);
                hilighChoices.Add(colorList[i].GetObjectType());

            }
        }
        return hilighChoices;
    }

    // Use this for initialization
    void Start ()
    {
        this.hilighChoices = new List<ARObjectType>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
