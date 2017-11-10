using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveReaction : RayCastReaction {

    public float movementSpeed = 10;

    private bool hitHold = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
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

    public override void OnHit()
    {
        if (Input.GetMouseButton(0))
            this.hitHold = true;
    }
}
