using UnityEngine;

/// <summary>
/// Put on a GameObject with material that uses outlines to allow highlight selection by TouchRaycast.
/// </summary>
public class HighlightReaction : RaycastReaction
{
    public bool isActive = false; 
    public bool hitHighlight;
    int hitCounter = 0;

    /// <summary>
    /// if the raycast hits it will first set the online border on the object it hit, and put the hit bool to true
    /// when you klick the same object again it turns of the outline
    /// </summary>
    public override void OnHit()
    {
        if (isActive == false)
            return; 
        Renderer rend = GetComponent<Renderer>();
    
        rend.material.SetFloat("_Outline", 10);
        this.hitHighlight = true;
        hitCounter++;
        if (hitCounter == 2)
        {
            rend.material.SetFloat("_Outline", 0);
            hitCounter = 0;
            this.hitHighlight = false;
        }
    }
}
