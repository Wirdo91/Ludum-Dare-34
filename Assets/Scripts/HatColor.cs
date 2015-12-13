using UnityEngine;
using System.Collections;

public class HatColor : MonoBehaviour {

    public void SetTeamMaterial(Material mat)
    {
        this.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (this.transform.position.y < -this.transform.localScale.y)
        {
            Destroy(this.gameObject);
        }
    }
}
