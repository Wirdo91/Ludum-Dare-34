using UnityEngine;
using System.Collections;

public class BallExpire : MonoBehaviour {
    	
	// Update is called once per frame
	void Update () {
	    if (this.transform.position.y < -this.transform.localScale.y)
        {
            Destroy(this.gameObject);
        }
	}
}
