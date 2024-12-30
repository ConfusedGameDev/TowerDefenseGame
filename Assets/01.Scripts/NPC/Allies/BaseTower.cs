using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseTower : MonoBehaviour
{
    [BoxGroup("Tower Setup ")]
    [SerializeField] Transform target;
    [BoxGroup("Tower Setup ")]
    [SerializeField] Transform head;
    [BoxGroup("Tower Setup ")]
    [SerializeField] Transform shootPoint;
    [BoxGroup("Tower Setup ")]
    [SerializeField] float attackRange = 3f;
    [BoxGroup("Tower Setup ")]
    [SerializeField] float fireRate = 3f;

    [BoxGroup("Tower Projectile")]
    public BaseBullet projectile;
    [BoxGroup("Tower Projectile")]
    public float projectileSpeed = 100f;

    private Coroutine shootingCoroutine;
    private bool targetInRange = true;

    [SerializeField] List<GameObject> enemies= new List<GameObject>();
    
    void Start()
    {

        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy").OrderBy(x=> Vector3.Distance(head.position,x.transform.position)));
        
    }

    public void UpdateTarget(Transform target)
    {
        this.target = target;
    }
     
    void Update()
    {
        
        if(head && target && shootPoint && projectile )
        {
            targetInRange=  Vector3.Distance(head.position, target.position)<=attackRange;
            if (targetInRange)
            {
                head.LookAt(target);
                if(shootingCoroutine== null)
                {
                    shootingCoroutine = StartCoroutine(ShootProjectile());
                }
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }
    }

    public virtual void Attack()
    {
        var p = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
        if (p != null)
        {
            p.addForce(shootPoint.forward, projectileSpeed);
        }
    }

    public virtual IEnumerator ShootProjectile()
    {
        Attack();
        yield return new WaitForSeconds(fireRate); 
        if (targetInRange)
        {
            
            shootingCoroutine = StartCoroutine(ShootProjectile());

        }
        else
        {
            shootingCoroutine= null;
        }

    }
    private void OnDrawGizmos()
    {
        if (head && target)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = targetInRange? Color.green: Color.red;
            Gizmos.DrawLine(head.position, target.position);
        }
    }
}
