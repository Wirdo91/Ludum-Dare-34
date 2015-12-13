using UnityEngine;
using System.Collections;

public struct NonActiveSnowMan
{
    public readonly float Bottom;
    public readonly float Middle;
    public readonly float Top;

    public readonly Teams CurrentTeam;
    public NonActiveSnowMan(float bottom, float middle, float top, Teams team)
    {
        Bottom = bottom;
        Middle = middle;
        Top = top;
        CurrentTeam = team;
    }
}

public class SnowMan : MonoBehaviour {

    float _bottom;
    float _middle;
    float _top;

    public float Size { get { return (_bottom + _middle + _top) / 3; } }

    float _currentMovementSpeed = 0;
    public float MovementSpeed
    {
        get { return _currentMovementSpeed; }
    }

    //TODO Movement speed dependent on size
    float _maxMovementSpeed;
    Vector3 _movementDirection = Vector3.zero;

    CapsuleCollider _collider;

    float _currentHealth = 0f;

    Teams _currentTeam;
    public Teams CurrentTeam
    {
        get { return _currentTeam; }
    }

    BattleSystem _battleController;

    Transform _currentOpponent;

    public void SetValues(float bottom, float middle, float top, Teams team)
    {
        _bottom = bottom;
        _middle = middle;
        _top = top;
        _currentTeam = team;

        //HACK Possibly redo the health function
        //_currentHealth = (((_bottom) + (_middle * _bottom) + (_top * _bottom)) * 10) - (_bottom - _middle - _top);
        _currentHealth = (_bottom * 3 + _middle * 2 + _top) *10;

        //TODO Movement speed dependent on size
        _maxMovementSpeed = 5f;

        _battleController = GameObject.FindObjectOfType<BattleSystem>();

        _collider = this.GetComponent<CapsuleCollider>();
    }

    public void SetValues(NonActiveSnowMan existingSnowMan)
    {
        SetValues(existingSnowMan.Bottom, existingSnowMan.Middle, existingSnowMan.Top, existingSnowMan.CurrentTeam);
    }

    public void Create()
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.localScale = Vector3.one * _bottom;
        ball.transform.position = new Vector3(this.transform.position.x, (_bottom / 2) - (_bottom / 10), this.transform.position.z);
        ball.transform.parent = this.transform;
        Destroy(ball.GetComponent<SphereCollider>());

        ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.localScale = Vector3.one * _middle;
        ball.transform.position = new Vector3(this.transform.position.x, _bottom - (_bottom / 10) + (_middle / 2) - (_middle / 10), this.transform.position.z);
        ball.transform.parent = this.transform;
        Destroy(ball.GetComponent<SphereCollider>());

        ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.localScale = Vector3.one * _top;
        ball.transform.position = new Vector3(this.transform.position.x, _bottom - (_bottom / 10) + _middle - (_middle / 10) + (_top / 2) - (_top / 10), this.transform.position.z);
        ball.transform.parent = this.transform;
        Destroy(ball.GetComponent<SphereCollider>());

        _collider.height = _bottom + _middle + _top;
        _collider.radius = Mathf.Max(_bottom, _middle, _top) / 2;
        _collider.center = new Vector3(0, _collider.height / 2, 0);
    }

    void Update()
    {
        if (_currentHealth < 0f)
        {
            Transform internalBall;
            //Death
            for (int i = 0; i < this.transform.childCount; i++)
            {
                internalBall = this.transform.GetChild(i);
                internalBall.parent = null;
                internalBall.gameObject.AddComponent<Rigidbody>();
                internalBall.gameObject.AddComponent<BallExpire>();
            }
            internalBall = null;
            Destroy(this.gameObject);
        }
        if (_currentOpponent == null)
        {
            _currentOpponent = _battleController.GetNearestTarget(this);
            //If no enemy exists
            if (_currentOpponent == null)
            {
                return;
            }
        }
        
        _currentMovementSpeed = Mathf.Lerp(_currentMovementSpeed, _maxMovementSpeed, GlobalVariables.instance.SnowManAcceleration * Time.deltaTime);

        _movementDirection = (_currentOpponent.transform.position - this.transform.position).normalized;

        this.transform.Translate(_movementDirection * _currentMovementSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        SnowMan op = col.gameObject.GetComponent<SnowMan>();
        if (op != null)
        {
            _currentMovementSpeed -= (op.Size * 2 + Mathf.Abs(op._currentMovementSpeed));
            op.Damage(this);
        }

        HomeBase hb = col.gameObject.GetComponent<HomeBase>();
        if (hb != null)
        {
            hb.Damage(this);
            _currentMovementSpeed *= -2;
        }
    }

    void Damage(SnowMan enemy)
    {
        if (enemy.CurrentTeam != this.CurrentTeam)
        {
            _currentHealth -= enemy.Size * Mathf.Abs(enemy.MovementSpeed);
        }
    }
}
