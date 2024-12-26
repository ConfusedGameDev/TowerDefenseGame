using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform head;
    [SerializeField] Transform shootPoint;
    [SerializeField] bool canAim=true;
    [SerializeField] float attackRange = 3f;

    public BaseBullet projectile;
    public float projectileSpeed = 100f;

    void Start()
    {
        
    }

    public void UpdateTarget(Transform target)
    {
        this.target = target;
    }
     
    void Update()
    {
        
        if(head && target && shootPoint && projectile )
        {
            canAim=  Vector3.Distance(head.position, target.position)<=attackRange;
            if (canAim)
            {
                head.LookAt(target);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                var p = Instantiate(projectile, shootPoint.position,shootPoint.rotation);
                if(p != null) 
                {
                    p.addForce(shootPoint.forward, projectileSpeed); 
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (head && target)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = canAim? Color.green: Color.red;
            Gizmos.DrawLine(head.position, target.position);
        }
    }
}
