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
    Material _player1Material;
    public Material Player1Material { get { return _player1Material; } }

    [SerializeField]
    Material _player2Material;
    public Material Player2Material { get { return _player2Material; } }

    public Material GetTeamMaterial(Teams team)
    {
        switch(team)
        {
            case Teams.TEAM1:
                return Player1Material;
            case Teams.TEAM2:
                return Player2Material;
            default:
                return null;
        }
    }

    void Awake()
    {
        instance = this;
    }
}
