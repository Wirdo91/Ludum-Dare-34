using UnityEngine;
using System.Collections;

public class GrassExpire : MonoBehaviour {

    Color currentColor;
    SpriteRenderer sRenderer;

    [SerializeField]
    float temp;

    void Start()
    {
        sRenderer = transform.FindChild("SnowLayer").GetComponent<SpriteRenderer>();
        currentColor = sRenderer.material.color;
    }

	// Update is called once per frame
	void Update ()
    {
        currentColor.a += (GlobalVariables.instance.SnowRate * Time.deltaTime);
        sRenderer.material.color = currentColor;
        //this.transform.localScale = this.transform.localScale - ((Vector3.one * GlobalVariables.instance.SnowRate) * Time.deltaTime);

        temp = currentColor.a;

        if (currentColor.a >= 5)
        {
            Destroy(this.gameObject);
        }
	}
}
