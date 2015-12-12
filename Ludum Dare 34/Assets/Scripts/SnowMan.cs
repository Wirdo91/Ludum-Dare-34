using UnityEngine;
using System.Collections;

public struct NonActiveSnowMan
{
    public readonly float _bottom;
    public readonly float _middle;
    public readonly float _top;

    public readonly float _movementSpeed;

    public readonly Teams _currentTeam;
    public NonActiveSnowMan(float bottom, float middle, float top, Teams team)
    {
        _bottom = bottom;
        _middle = middle;
        _top = top;
        _currentTeam = team;

        //TODO Movement speed dependent on size
        _movementSpeed = 1f;
    }
}

public class SnowMan : MonoBehaviour {

    float _bottom;
    float _middle;
    float _top;

    private float size { get { return (_bottom + _middle + _top) / 3; } }

    bool _fighting;

    float _currentMovementSpeed = 0;
    float _maxMovementSpeed;
    Vector3 _movementDirection = Vector3.zero;

    CapsuleCollider _collider;

    Teams _currentTeam;
    public Teams CurrentTeam
    {
        get { return _currentTeam; }
    }

    BattleSystem _battleController;

    SnowMan _currentOpponent;

    public void SetValues(float bottom, float middle, float top, Teams team)
    {
        _bottom = bottom;
        _middle = middle;
        _top = top;
        _currentTeam = team;

        //TODO Movement speed dependent on size
        _maxMovementSpeed = 5f;

        _battleController = GameObject.FindObjectOfType<BattleSystem>();

        _collider = this.GetComponent<CapsuleCollider>();
    }

    public void SetValues(NonActiveSnowMan existingSnowMan)
    {
        SetValues(existingSnowMan._bottom, existingSnowMan._middle, existingSnowMan._top, existingSnowMan._currentTeam);
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

    void FixedUpdate()
    {
        if (_currentOpponent == null)
        {
            _currentOpponent = _battleController.GetNearestSnowManOfOppesiteTeam(this);
        }
        
        _currentMovementSpeed = Mathf.Lerp(_currentMovementSpeed, _maxMovementSpeed, GlobalVariables.instance.SnowManAcceleration * Time.deltaTime);

        _movementDirection = (_currentOpponent.transform.position - this.transform.position).normalized;

        this.transform.Translate(_movementDirection * _currentMovementSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);

        SnowMan op = col.gameObject.GetComponent<SnowMan>();
        if (op != null)
        {
            _currentMovementSpeed -= (op.size * 2 + Mathf.Abs(op._currentMovementSpeed));
        }
    }
}
