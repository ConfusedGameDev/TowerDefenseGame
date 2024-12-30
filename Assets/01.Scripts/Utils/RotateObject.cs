using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 3f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction*speed*100*Time.deltaTime);
    }
}
