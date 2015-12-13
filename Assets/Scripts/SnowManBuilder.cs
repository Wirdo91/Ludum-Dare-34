using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowManBuilder : MonoBehaviour {

    List<SnowBallMovement> _balls;
    int _currentIndex = 0;

    BattleSystem _battleSystem;
    HomeBase _localHomeBase;

    public Teams Team { get { return _localHomeBase.BaseTeam; } }

    void Start()
    {
        _balls = new List<SnowBallMovement>();
        _battleSystem = FindObjectOfType<BattleSystem>();
        _localHomeBase = GetComponentInParent<HomeBase>();
    }

    public void TransferBall(SnowBallMovement ball)
    {
        if (ball.CurrentTeam != this._localHomeBase.BaseTeam)
        {
            return;
        }

        ball.transform.parent = this.transform;

        Vector3 ballPosition = this.transform.position;

        for (int i = 0; i < _balls.Count; i++)
        {
            ballPosition.y += _balls[i].CurrentThickness;
        }
        ballPosition.y += ball.CurrentThickness / 2;
        ball.transform.position = ballPosition;

        _balls.Add(ball);

        if (_balls.Count == 3)
        {
            //Build SnowMan
            _battleSystem.SpawnSnowMan(
                new NonActiveSnowMan(_balls[0].CurrentThickness, _balls[1].CurrentThickness, _balls[2].CurrentThickness, _localHomeBase.BaseTeam), 
                this.transform.position);

            //Destroy old balls
            foreach (SnowBallMovement oldBall in _balls)
            {
                Destroy(oldBall.gameObject);
            }

            _balls = new List<SnowBallMovement>();
        }
    }
}
