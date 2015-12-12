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
