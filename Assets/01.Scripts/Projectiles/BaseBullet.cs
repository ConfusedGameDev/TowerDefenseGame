using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseBullet : MonoBehaviour
{
    Rigidbody rb;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
