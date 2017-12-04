using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ARObjectType
{
    OrangeRod,
    BlueRod,
    BrownRod,
    BlackRod,
    DarkGreenRod,
    YellowRod,
    PurpleRod,
    LightGreenRod,
    RedRod,
    WhiteRod
};



public class ArObjectType : MonoBehaviour
{
    public ARObjectType objType;

	// Use this for initialization
	void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public ARObjectType GetObjectType()
    {
        return this.objType;
    }
}
