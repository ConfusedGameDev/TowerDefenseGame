using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    public float rotationSpeed=1f;
    public LayerMask GroundLayerMask;
    public Transform visuals;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (visuals && Physics.Raycast(visuals.position, -Vector3.up , out RaycastHit hit,100f,GroundLayerMask))
        { 
            Quaternion targetRotation= Quaternion.FromToRotation(visuals.up, hit.normal)* visuals.rotation;
            visuals.rotation= Quaternion.Slerp(visuals.rotation, targetRotation, Time.deltaTime*rotationSpeed);
        }
    }
}
