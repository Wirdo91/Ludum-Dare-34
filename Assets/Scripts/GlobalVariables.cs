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
    float _homeBaseStartHealth;
    public float HomeBaseStartHealth { get { return _homeBaseStartHealth; } }

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
    KeyCode _player1TurnLeftButton;
    public KeyCode Player1TurnLeftButton { get { return _player1TurnLeftButton; } }

    [SerializeField]
    KeyCode _player1TurnRightButton;
    public KeyCode Player1TurnRightButton { get { return _player2TurnRightButton; } }

    [SerializeField]
    KeyCode _player2TurnLeftButton;
    public KeyCode Player2TurnLeftButton { get { return _player1TurnLeftButton; } }

    [SerializeField]
    KeyCode _player2TurnRightButton;
    public KeyCode Player2TurnRightButton { get { return _player2TurnRightButton; } }

    void Awake()
    {
        instance = this;
    }
}
