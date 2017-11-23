using UnityEngine;

/// <summary>
/// An interface class that implements an on RaycastHit reaction.
/// To create a RayCastReaction create a class that inherits from this class and override the OnHit virtual method.
/// </summary>
public class RaycastReaction : MonoBehaviour
{
    /// <summary>
    /// The method that the TouchRaycast script will call on hit.
    /// </summary>
    public virtual void OnHit()
    {
        /// Reaction
    }
}
