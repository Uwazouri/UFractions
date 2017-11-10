using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRaycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
            this.ShootRay(Input.mousePosition);
        
	}   

    private void ShootRay(Vector3 input)
    {
        Ray ray = Camera.main.ScreenPointToRay(input);
        Debug.DrawRay(ray.origin, ray.direction * 10, new Color(1, 0, 0), 5);
        RaycastHit hit;
        RayCastReaction[] r;
        LayerMask mask = LayerMask.GetMask("Default");
        Physics.Raycast(ray, out hit, 9999, mask);
        if (hit.collider != null)
        {
            print("Hit With Raycast");
            r = hit.collider.GetComponents<RayCastReaction>();
            if (r.Length > 0)
            {
                foreach (RayCastReaction rr in r)
                    rr.OnHit();
            }
        }
    }
}
