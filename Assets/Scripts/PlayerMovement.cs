using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    GameObject _snowBallPrefab;

    Vector3 _playerDirectionVector;
    public Vector3 PlayerDirectionVector { get { return _playerDirectionVector; } }
    float _playerCurrentMoveSpeed = 0f;
    public float PlayerCurrentMoveSpeed { get { return _playerCurrentMoveSpeed; } }

    SnowBallMovement _currentSnowBall;
    Transform _playerObject;

    [SerializeField]
    bool _move = false;

    // Use this for initialization
    void Start()
    {
        _playerDirectionVector = Vector3.forward;

        if (_currentSnowBall == null)
        {
            CreateNewSnowBall();
        }

        _playerObject = transform.FindChild("Player");

        FindObjectOfType<BattleSystem>().InitializeBattle(
            new System.Collections.Generic.List<NonActiveSnowMan> { new NonActiveSnowMan(3, 2, 1, Teams.TEAM1) }, 
            new System.Collections.Generic.List<NonActiveSnowMan> { new NonActiveSnowMan(3, 2, 1, Teams.TEAM2) });

        //transform.GetChild(0).localPosition = _playerDirectionVector * -GlobalVariables.instance.SnowBallInitialSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_playerCurrentMoveSpeed < GlobalVariables.instance.PlayerMaxMoveSpeed)
        {
            _playerCurrentMoveSpeed = Mathf.Lerp(_playerCurrentMoveSpeed, GlobalVariables.instance.PlayerMaxMoveSpeed, GlobalVariables.instance.PlayerAcceleration * Time.deltaTime);
        }

        if (Input.GetKey(GlobalVariables.instance.TurnLeftButton))
        {
            _playerDirectionVector = Quaternion.Euler(0, -GlobalVariables.instance.PlayerTurningSpeed * Time.deltaTime, 0) * _playerDirectionVector;
        }
        else if (Input.GetKey(GlobalVariables.instance.TurnRightButton))
        {
            _playerDirectionVector = Quaternion.Euler(0, GlobalVariables.instance.PlayerTurningSpeed * Time.deltaTime, 0) * _playerDirectionVector;
        }
        _playerDirectionVector.Normalize();

        if (_move)
        {
            transform.position += (_playerDirectionVector * _playerCurrentMoveSpeed * Time.deltaTime);
        }
        _playerObject.localPosition = _playerDirectionVector * -((_currentSnowBall.CurrentThickness / 2) + .5f);
    }

    void CreateNewSnowBall()
    {
        _currentSnowBall = Instantiate(_snowBallPrefab).GetComponent<SnowBallMovement>();
        _currentSnowBall.transform.position = transform.position;
        _currentSnowBall.transform.parent = this.transform;
        _currentSnowBall.transform.localScale = Vector3.one;
    }
}
