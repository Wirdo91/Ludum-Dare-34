using UnityEngine;
using System.Collections;

public class HomeBase : MonoBehaviour {

    float _currentHealth;

    [SerializeField]
    Teams _baseTeam;
    public Teams BaseTeam { get { return _baseTeam; } }

    void Start()
    {
        _currentHealth = GlobalVariables.instance.HomeBaseStartHealth;
    }

    public void Damage(SnowMan enemy)
    {
        if (enemy.CurrentTeam != _baseTeam)
        {
            _currentHealth -= enemy.Size * Mathf.Abs(enemy.MovementSpeed);
        }
        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
