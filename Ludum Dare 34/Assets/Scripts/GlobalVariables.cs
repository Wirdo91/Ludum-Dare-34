using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

    public static GlobalVariables instance;

    [SerializeField]
    float _snowBallInitialSize;
    public float SnowBallInitialSize { get { return _snowBallInitialSize; } }

    [SerializeField]
    float _snowRate;
    public float SnowRate { get { return _snowRate; } }

    [SerializeField]
    float _pickUpRate;
    public float PickUpRate { get { return _pickUpRate; } }

    [SerializeField]
    float _playerMaxMoveSpeed;
    public float PlayerMaxMoveSpeed { get { return _playerMaxMoveSpeed; } }

    [SerializeField]
    float _playerAcceleration;
    public float PlayerAcceleration { get { return _playerAcceleration; } }

    [SerializeField]
    float _playerTurningSpeed;
    public float PlayerTurningSpeed { get { return _playerTurningSpeed; } }

    [SerializeField]
    float _snowManAcceleration;
    public float SnowManAcceleration { get { return _snowManAcceleration; } }

    [SerializeField]
    KeyCode _turnLeftButton;
    public KeyCode TurnLeftButton { get { return _turnLeftButton; } }

    [SerializeField]
    KeyCode _turnRightButton;
    public KeyCode TurnRightButton { get { return _turnRightButton; } }

    void Awake()
    {
        instance = this;
    }
}
