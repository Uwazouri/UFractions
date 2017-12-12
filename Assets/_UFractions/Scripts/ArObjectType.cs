using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for what type of AR object. 
/// </summary>
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
    /// <summary>
    /// returns what type of object is set. 
    /// </summary>
    /// <returns></returns>
    public ARObjectType GetObjectType()
    {
        return this.objType;
    }
}
