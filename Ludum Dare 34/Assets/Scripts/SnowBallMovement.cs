using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SnowBallRotation))]
public class SnowBallMovement : MonoBehaviour {

    PlayerMovement _player;

    float val;
    float _currentThickness
    {
        get { return val; }
        set { val = value; Debug.Log(value); }
    }

	// Use this for initialization
	void Start ()
    {
        _player = GetComponentInParent<PlayerMovement>();
        float _currentThickness = GlobalVariables.instance.SnowBallInitialSize;
        Debug.Log(_currentThickness + " " + GlobalVariables.instance.SnowBallInitialSize);
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.GetChild(0).localScale = Vector3.one * _currentThickness;
        Debug.Log(_currentThickness);

        this.transform.position =
            new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
    }
}
