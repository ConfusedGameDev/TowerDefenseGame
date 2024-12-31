using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(SphereCollider))]
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

    [SerializeField] List<BaseEnemy> enemies= new List<BaseEnemy>();
    [SerializeField] float rotationSpeed = 1f;
    SphereCollider spCollider;
    public LayerMask enemyLM;
    bool isCoolingDown;
    void Start()
    {
        spCollider = GetComponent<SphereCollider>();
        spCollider.isTrigger = true;
        spCollider.radius = attackRange;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemies.Add(enemy);
            if (target == null && !isCoolingDown)
                target = enemy.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            if(enemies.Contains(enemy))
                enemies.Remove(enemy);
            if(target== enemy.transform)
                target= enemies.Count > 0? getRandomTarget():null;
        }
    }


    public void UpdateTarget(Transform target)
    {
        this.target = target;
    }
     
    void Update()
    {

        if (head && target && shootPoint && projectile && !isCoolingDown)
        {
            FaceTarget(target.position);
            Ray r = new Ray(shootPoint.position, shootPoint.forward);

            if (Physics.Raycast(r, out RaycastHit hit, 100, enemyLM))
            {
                Attack();
                StartCoroutine(coolDown());
            }

        }
        else
        {
            FaceTarget(head.position + transform.forward * 10f);
        }
    }
    IEnumerator coolDown()
    {
        isCoolingDown= true;
        yield return new WaitForSeconds(fireRate);
        isCoolingDown = false;
        target= getRandomTarget();
    }
    private void FaceTarget(Vector3 target)
    {
        var direction = target - head.transform.position;
       // direction.y = 0;
        Quaternion targetRoataion = Quaternion.LookRotation(direction);
        head.transform.rotation = Quaternion.Lerp(head.transform.rotation, targetRoataion, Time.deltaTime * rotationSpeed);
    }

    public virtual void Attack()
    {
        var p = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
        if (p != null)
        {
            p.addForce(shootPoint.forward, projectileSpeed);
        }
    }

    Transform getRandomTarget()
    {
        for (int i = 0; i< enemies.Count; i++)
        {
            if(enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }
        var randomEnemy= enemies.Count>0? enemies[Random.Range(0,enemies.Count)]:null;

        return randomEnemy ? randomEnemy.transform : null ;
    }
   
    private void OnDrawGizmos()
    {
        if (head && target)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            
        }
    }
}
