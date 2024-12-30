using UnityEngine;

public class PlayerCastle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float health = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            Damage(enemy.DestroyEnemy());
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        if(health<=0)
        {
            Destroy(gameObject);
        }
    }
}
