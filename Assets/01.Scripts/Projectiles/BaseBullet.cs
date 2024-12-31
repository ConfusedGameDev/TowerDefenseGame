using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseBullet : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float lifeTime=3f;
    [SerializeField] float damage = 10f;

    public void addForce(Vector3 direction, float speed=30,  ForceMode forceMode= ForceMode.Impulse)
    {
        if (rb == null)
            Start();
        rb.AddForce(direction * speed, forceMode);
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if(collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.onGetDamage(damage);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.onGetDamage(damage);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
