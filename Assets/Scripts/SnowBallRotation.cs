using UnityEngine;
using System.Collections;

public class SnowBallRotation : MonoBehaviour {

    PlayerMovement _player;
    Transform _snowBall;

	// Use this for initialization
	void Start ()
    {
        _player = GetComponentInParent<PlayerMovement>();
        _snowBall = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        _snowBall.eulerAngles = Quaternion.Euler(_player.PlayerDirectionVector * _player.PlayerCurrentMoveSpeed) * _snowBall.rotation.eulerAngles;
        //Rotate _snowBall bases on _player speed and direction	
    }
}
