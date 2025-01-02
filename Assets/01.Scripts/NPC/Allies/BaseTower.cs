using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(SphereCollider))]
public class BaseTower : MonoBehaviour
{
    [BoxGroup("Tower Setup ")]
    [SerializeField] protected Transform target;
    [BoxGroup("Tower Setup ")]
    [SerializeField] protected Transform head;
    [BoxGroup("Tower Setup ")]
    [SerializeField] protected Transform shootPoint;
    [BoxGroup("Tower Setup ")]
    [SerializeField] float attackRange = 3f;
    [BoxGroup("Tower Setup ")]
    [SerializeField] protected float fireRate = 3f;

    [BoxGroup("Tower Visuals Setup")]
    [SerializeField] private Renderer[] glowingRenderers;
    [BoxGroup("Tower Visuals Setup")]
    [Range(0f, 25f)]
    [SerializeField] private float maxGlow = 25f;

    [BoxGroup("Tower Projectile")]
    public BaseBullet projectile;
    [BoxGroup("Tower Projectile")]
    public float projectileSpeed = 100f;

    [SerializeField] protected float damage = 7f;

    private Coroutine shootingCoroutine;
    private bool targetInRange = true;

    [SerializeField] List<BaseEnemy> enemies = new List<BaseEnemy>();
    [SerializeField] float rotationSpeed = 1f;
    SphereCollider spCollider;
    public LayerMask enemyLM, obstacleMask;
    bool isCoolingDown;
    protected Vector3 attackPoint;

    [SerializeField] EnemyType priorityTargetType;
    int glowID = Shader.PropertyToID("_EmmisionIntensity");
    protected virtual void Start()
    {
        spCollider = GetComponent<SphereCollider>();
        spCollider.isTrigger = true;
        spCollider.radius = attackRange;
        isCoolingDown = true;
        StartCoroutine(coolDown());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
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
            if (enemies.Contains(enemy))
                enemies.Remove(enemy);
            if (target == enemy.transform)
                target = enemies.Count > 0 ? getNextTarget() : null;
        }
    }


    public void UpdateTarget(Transform target)
    {
        this.target = target;
    }

    protected virtual void Update()
    {

        if (head && target && shootPoint && projectile && !isCoolingDown)
        {
            FaceTarget(target.position);
            Ray r = new Ray(shootPoint.position, shootPoint.forward);
            if (Physics.Raycast(r, out RaycastHit hito, 100, obstacleMask)) return;

            if (Physics.Raycast(r, out RaycastHit hit, 100, enemyLM))
            {
                if (hit.transform != target) return;
                isCoolingDown = true;
                if (hit.transform.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
                {
                    attackPoint = enemy.getCenterPoint();
                }
                else
                    attackPoint = hit.point;
                Attack();
                StartCoroutine(coolDown());
            }

        }
        else if (!isCoolingDown)
        {
            FaceTarget(head.position + transform.forward * 10f);
        }
    }
    protected virtual IEnumerator coolDown()
    {

        var delta = 0f;
        if (glowingRenderers != null)
        {
            foreach (var r in glowingRenderers)
            {
                r.material.SetFloat(glowID, 0);
            }
        }


        while (delta < fireRate)
        {
            yield return null;
            if (glowingRenderers != null)
            {
                foreach (var r in glowingRenderers)
                {
                    r.material.SetFloat(glowID, maxGlow * (delta / fireRate));
                }
            }

            delta += Time.deltaTime;

        }


        isCoolingDown = false;
        target = getNextTarget();
    }
    private void FaceTarget(Vector3 target)
    {
        var direction = target - head.transform.position;
        // direction.y = 0;
        Quaternion targetRoataion = Quaternion.LookRotation(direction.normalized);
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

    Transform getNextTarget()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }
        if (enemies.Count != 0)
        {
            enemies = enemies.OrderByDescending(x => x.totalDistance).ToList();
            var priorityEnemies = enemies.Where(x => x.enemyType == priorityTargetType).ToList();
            return priorityEnemies.Count > 0 ? priorityEnemies[^1].transform : enemies[^1].transform;
        }
        return null;
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
