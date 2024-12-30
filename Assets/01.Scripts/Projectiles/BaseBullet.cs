using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseBullet : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float lifeTime=3f;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
