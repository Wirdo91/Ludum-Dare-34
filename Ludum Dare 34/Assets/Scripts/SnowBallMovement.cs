using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SnowBallRotation))]
public class SnowBallMovement : MonoBehaviour {

    PlayerMovement _player;

    float _currentThickness;
    public float CurrentThickness { get { return _currentThickness; } }

    [SerializeField]
    GameObject _snowRemovedArea;

    //TODO If collided with snow, remove old, adjust value picked up dependent on alpha
	void Start ()
    {
        _player = GetComponentInParent<PlayerMovement>();
        float _currentThickness = GlobalVariables.instance.SnowBallInitialSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        _currentThickness += GlobalVariables.instance.PickUpRate * Time.deltaTime;

        this.transform.GetChild(0).localScale = Vector3.one * _currentThickness;

        this.transform.position =
            new Vector3(transform.position.x, _currentThickness / 2, transform.position.z);

        GameObject currentGrass = Instantiate(_snowRemovedArea);
        currentGrass.transform.localScale = Vector3.one * _currentThickness;
        currentGrass.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        currentGrass.transform.rotation = Quaternion.LookRotation(_player.PlayerDirectionVector);
    }
}
