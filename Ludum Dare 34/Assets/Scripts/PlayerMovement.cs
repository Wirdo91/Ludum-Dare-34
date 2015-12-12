using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    float _playerMaxMoveSpeed;
    [SerializeField]
    float _playerAcceleration;
    [SerializeField]
    float _turningSpeed;

    [SerializeField]
    GameObject _snowBallPrefab;

    Vector3 _playerDirectionVector;
    public Vector3 PlayerDirectionVector { get { return _playerDirectionVector; } }
    float _playerCurrentMoveSpeed = 0f;
    public float PlayerCurrentMoveSpeed { get { return _playerCurrentMoveSpeed; } }

    GameObject _currentSnowBall;

	// Use this for initialization
	void Start ()
    {
        _playerDirectionVector = Vector3.forward;

        if (_currentSnowBall == null)
        {
            CreateNewSnowBall();
        }

        transform.GetChild(0).localPosition = 
            _playerDirectionVector * -GlobalVariables.instance.SnowBallInitialSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_playerCurrentMoveSpeed < _playerMaxMoveSpeed)
        {
            _playerCurrentMoveSpeed = Mathf.Lerp(_playerCurrentMoveSpeed, _playerMaxMoveSpeed, _playerAcceleration * Time.deltaTime);
        }

        if (Input.GetKey(GlobalVariables.instance.TurnLeftButton))
        {
            _playerDirectionVector = Quaternion.Euler(0, -_turningSpeed * Time.deltaTime, 0) * _playerDirectionVector;
        }
        else if (Input.GetKey(GlobalVariables.instance.TurnRightButton))
        {
            _playerDirectionVector = Quaternion.Euler(0, _turningSpeed * Time.deltaTime, 0) * _playerDirectionVector;
        }
        
        transform.rotation = Quaternion.LookRotation(_playerDirectionVector);

        transform.position += (transform.forward * _playerCurrentMoveSpeed * Time.deltaTime);

        //_currentSnowBall.transform.position = this.transform.position;
        //_currentSnowBall.transform.Translate(_playerDirectionVector * _playerCurrentMoveSpeed * Time.deltaTime);
    }

    void CreateNewSnowBall()
    {
        _currentSnowBall = Instantiate(_snowBallPrefab);
        _currentSnowBall.transform.position = transform.position;
        _currentSnowBall.transform.parent = this.transform;
    }
}
