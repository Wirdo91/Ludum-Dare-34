using UnityEngine;
using System.Collections;

public class HomeBase : MonoBehaviour {

    float _currentHealth;

    [SerializeField]
    Teams _baseTeam;
    public Teams BaseTeam { get { return _baseTeam; } }

    float _speedOfDestruction = 2f;

    [SerializeField]
    GameObject _deathParticle;

    [SerializeField]
    bool destroy = false;

    void Start()
    {
        _currentHealth = GlobalVariables.instance.HomeBaseStartHealth;
    }

    void Update()
    {
        if (destroy)
        {
            destroy = false;
            Death();
        }
    }

    public void SetBaseTeam(Teams team)
    {
        _baseTeam = team;
        _currentHealth = GlobalVariables.instance.HomeBaseStartHealth;
    }

    void Death()
    {
        Instantiate(_deathParticle, this.transform.position, Quaternion.identity);
        StartCoroutine(AnimateDestruction());
    }

    float _shake = .3f;
    IEnumerator AnimateDestruction()
    {
        while(this.transform.position.y > -10)
        {
            this.transform.Translate(new Vector3(Random.Range(-_shake, _shake), -_speedOfDestruction * Time.deltaTime, Random.Range(-_shake, _shake)));
            yield return null;
        }
        
        Destroy(this.gameObject);
    }

    public void Damage(SnowMan enemy)
    {
        if (GlobalVariables.instance.GameRunning)
        {
            if (enemy.CurrentTeam != _baseTeam)
            {
                _currentHealth -= enemy.Size * Mathf.Abs(enemy.MovementSpeed);
            }
            if (_currentHealth <= 0)
            {
                Death();
            }
        }
    }
}
