using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

    public static GlobalVariables instance;

    [SerializeField]
    float _snowBallInitialSize;
    public float SnowBallInitialSize { get { return _snowBallInitialSize; } }

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
