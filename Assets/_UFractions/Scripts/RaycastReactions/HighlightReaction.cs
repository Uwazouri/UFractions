using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightReaction : RaycastReaction
{
    public bool isActive = true; 
    public bool hitHighlight;
    int hitCounter = 0;

    // Use this for initialization
    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    /// <summary>
    /// if the raycast hits it will first set the online border on the object it hit, and put the hit bool to true
    /// when you klick the same object again it turns of the outline
    /// 
    /// </summary>
    public override void OnHit()
    {

        if (isActive == false)
            return; 
        Renderer rend = GetComponent<Renderer>();
    
        rend.material.SetFloat("_Outline", 1);
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
