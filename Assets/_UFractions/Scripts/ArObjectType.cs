using UnityEngine;

/// <summary>
/// Enum for what type of AR object. New AR Objects must get thier own ARObjectType in this enum.
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


/// <summary>
/// Attach to a GameObject to make it register as an AR object.
/// </summary>
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
