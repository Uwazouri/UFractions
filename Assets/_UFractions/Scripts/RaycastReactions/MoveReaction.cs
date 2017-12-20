using UnityEngine;

/// <summary>
/// Makes the game object this script is attached to react to TouchRaycast with movement along the xz-plane.
/// NOTE: To move objects, a floor plane is needed that lies in the "Floor" layer.
/// </summary>
public class MoveReaction : RaycastReaction
{
    private bool hitHold = false;
	
	/// <summary>
    /// Moves the object every frame touch is held down.
    /// </summary>
	void Update ()
    {
		if (this.hitHold)
        {
            if (!Input.GetMouseButton(0))
            {
                this.hitHold = false;
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("Floor");
            Physics.Raycast(ray, out hit, 9999, layerMask);
            if (hit.collider != null)
            {
                Vector3 pos = hit.point;
                pos.y = 0.5f;
                this.transform.position = Vector3.MoveTowards(this.transform.position, pos, 0.5f);
            }
        }
	}

    /// <summary>
    /// The on hit reaction activates move on hold.
    /// </summary>
    public override void OnHit()
    {
        if (Input.GetMouseButton(0))
            this.hitHold = true;
    }
}
