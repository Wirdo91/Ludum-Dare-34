using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    GameObject _snowBallPrefab;

    Vector3 _playerDirectionVector;
    public Vector3 PlayerDirectionVector { get { return _playerDirectionVector; } }
    float _playerCurrentMoveSpeed = 0f;
    public float PlayerCurrentMoveSpeed { get { return _playerCurrentMoveSpeed; } }

    CapsuleCollider EntireCollider;

    [SerializeField]
    KeyCode _turnLeftButton;
    [SerializeField]
    KeyCode _turnRightButton;

    [SerializeField]
    Vector3 _initialDirection = Vector3.forward;

    SnowBallMovement _currentSnowBall;
    Transform _playerObject;

    [SerializeField]
    bool _move = false;

    [SerializeField]
    Teams _playerTeam;
    public Teams PlayerTeam {  get { return _playerTeam; } }

    // Use this for initialization
    void Start()
    {
        _playerDirectionVector = _initialDirection;

        EntireCollider = this.GetComponent<CapsuleCollider>();

        if (_currentSnowBall == null)
        {
            CreateNewSnowBall();
        }

        _playerObject = transform.FindChild("Player");

        FindObjectOfType<BattleSystem>().OnWinCondition += OnWinCondition;

        //transform.GetChild(0).localPosition = _playerDirectionVector * -GlobalVariables.instance.SnowBallInitialSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_currentSnowBall == null)
        {
            CreateNewSnowBall();
            _playerDirectionVector *= -1;
        }

        if (_playerCurrentMoveSpeed < GlobalVariables.instance.PlayerMaxMoveSpeed)
        {
            _playerCurrentMoveSpeed = Mathf.Lerp(_playerCurrentMoveSpeed, GlobalVariables.instance.PlayerMaxMoveSpeed, GlobalVariables.instance.PlayerAcceleration * Time.deltaTime);
        }

        if (Input.GetKey(_turnLeftButton))
        {
            _playerDirectionVector = Quaternion.Euler(0, -GlobalVariables.instance.PlayerTurningSpeed * Time.deltaTime, 0) * _playerDirectionVector;
        }
        else if (Input.GetKey(_turnRightButton))
        {
            _playerDirectionVector = Quaternion.Euler(0, GlobalVariables.instance.PlayerTurningSpeed * Time.deltaTime, 0) * _playerDirectionVector;
        }
        _playerDirectionVector.Normalize();

        if (_move)
        {
            transform.position += (_playerDirectionVector * _playerCurrentMoveSpeed * Time.deltaTime);
        }
        this.transform.LookAt(this.transform.position + PlayerDirectionVector);
        _playerObject.localPosition = Vector3.forward * -((_currentSnowBall.CurrentThickness / 2) + .5f);
        //_playerObject.LookAt(this.transform.position);

        EntireCollider.height = 1 + _currentSnowBall.CurrentThickness;
        EntireCollider.center = new Vector3(0, _currentSnowBall.transform.localPosition.y, -.5f);
        EntireCollider.radius = _currentSnowBall.CurrentThickness / 2;
    }

    void OnWinCondition(Teams winningTeam)
    {
        _move = false;
    }

    public void RemoveBallReference()
    {
        _currentSnowBall = null;
    }

    void CreateNewSnowBall()
    {
        _currentSnowBall = Instantiate(_snowBallPrefab).GetComponent<SnowBallMovement>();
        _currentSnowBall.transform.position = transform.position;
        _currentSnowBall.transform.parent = this.transform;
        _currentSnowBall.transform.localScale = Vector3.one;
    }
}
