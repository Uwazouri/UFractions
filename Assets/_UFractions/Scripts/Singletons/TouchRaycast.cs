using UnityEngine;

/// <summary>
/// Handles touch or mouse raycast shooting that works with RayCastReactions.
/// </summary>
public class TouchRaycast : Singleton<TouchRaycast>
{
    protected TouchRaycast() { }

    private bool active = true;
	
	/// <summary>
    /// Checks mouse or touch input every frame and shoots raycast.
    /// </summary>
	void Update ()
    {
        if (this.active)
            if (Input.GetMouseButtonDown(0))
                this.ShootRay(Input.mousePosition);
	}   

    /// <summary>
    /// Shoots a raycast from mouse/touch camera screen position in the screens normal direction.
    /// Raycast only hits objects in default layer and will check if the object contains any RayCastReactions and then call them if found.
    /// </summary>
    /// <param name="input">The position of the mouse/touch on camera screen.</param>
    private void ShootRay(Vector3 input)
    {
        Ray ray = Camera.main.ScreenPointToRay(input);
        Debug.DrawRay(ray.origin, ray.direction * 10, new Color(1, 0, 0), 5);
        RaycastHit hit;
        RaycastReaction[] r;
        LayerMask mask = LayerMask.GetMask("Default");
        Physics.Raycast(ray, out hit, 9999, mask);
        if (hit.collider != null)
        {
            print("Hit With Raycast");
            r = hit.collider.GetComponents<RaycastReaction>();
            if (r.Length > 0)
            {
                foreach (RaycastReaction rr in r)
                    rr.OnHit();
            }
        }
    }

    /// <summary>
    /// Activates or deactivates the raycast shooting.
    /// </summary>
    /// <param name="state">True to activate and false to deactivate.</param>
    public void SetActive(bool state)
    {
        this.active = state;
    }
}
