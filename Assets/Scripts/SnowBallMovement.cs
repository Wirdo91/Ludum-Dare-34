using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SnowBallRotation))]
public class SnowBallMovement : MonoBehaviour {

    PlayerMovement _player;
    TerrainHandler _terrainHandler;
    [SerializeField]
    AnimationCurve _pickupmodifier;
    [SerializeField]
    float _maxsize = 5;

    public Teams CurrentTeam
    {
        get { return _player.PlayerTeam; }
    }

    float _currentThickness;
    public float CurrentThickness { get { return _currentThickness; } }

    bool _inPlay = false;

    SphereCollider col;

    //TODO If collided with snow, remove old, adjust value picked up dependent on alpha
	void Start ()
    {
        _inPlay = true;
        _player = GetComponentInParent<PlayerMovement>();
        _currentThickness = GlobalVariables.instance.SnowBallInitialSize;
        _terrainHandler = GameObject.Find("World").GetComponent<TerrainHandler>();
        col = this.GetComponent<SphereCollider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _currentThickness += (_terrainHandler.GetSnow(this.transform) * Time.deltaTime * GlobalVariables.instance.PickUpRate) * _pickupmodifier.Evaluate(CurrentThickness / _maxsize);
        if (_inPlay)
        {
            _currentThickness += GlobalVariables.instance.PickUpRate * Time.deltaTime;

            col.radius = CurrentThickness / 2;

            this.transform.GetChild(0).localScale = Vector3.one * _currentThickness;

            this.transform.position =
                new Vector3(transform.position.x, _currentThickness / 2, transform.position.z);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        SnowManBuilder builder = col.GetComponent<SnowManBuilder>();
        if (builder != null && builder.Team == this.CurrentTeam)
        {
            builder.TransferBall(this);
            _player.RemoveBallReference();
            _inPlay = false;
        }
    }
}
