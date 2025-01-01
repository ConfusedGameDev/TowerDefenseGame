using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class CrossBowTower : BaseTower
{

    [SerializeField] private Animator anim;

    [SerializeField] private LineRenderer laserRenderer;

    [SerializeField] private float laserDuration=0.3f;


    [FoldoutGroup("Head Rotor Setup")]
    [SerializeField] Transform headRotor, headRotorStartPos, headRotorEndPos, headRotorLeftLaserStart, headRotorLeftLaserEnd, headRotorRightLaserStart, headRotorRightLaserEnd, headRotorLeftLaserEnd2, headRotorRightLaserEnd2;
    [FoldoutGroup("Head Rotor Setup")]
    [SerializeField] LineRenderer leftRotorLaser, rightRotorLaser, leftRotorLaser2, rightRotorLaser2;
    public GameObject onHitFX;
    protected override void Start()
    {
        base.Start();
        laserRenderer = GetComponent<LineRenderer>();
    }
    public override void Attack()
    {
        head.LookAt(attackPoint);
        
        laserRenderer.SetPosition(0, shootPoint.position);
        laserRenderer.SetPosition(1, attackPoint);
        if (onHitFX)
        {
            var hitFX = Instantiate(onHitFX, attackPoint,Quaternion.identity);
        }
        laserRenderer.enabled = true;
        if(target)
        {
            if (target.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.onGetDamage(damage);
            }
        }
        Debug.DrawLine(shootPoint.position, attackPoint, Color.yellow);
        CameraShake.Instance.Shake();
    }

    protected override IEnumerator coolDown()
    {
        if(headRotor && headRotorEndPos && headRotorStartPos)
            StartCoroutine(animateHeadRotor());
        StartCoroutine(turnLaserOff());
        yield return base.coolDown();
    }

    IEnumerator turnLaserOff()
    {
        yield return new WaitForSeconds(laserDuration);
        laserRenderer.enabled = false;
    }
    IEnumerator animateHeadRotor()
    {
        var delta = 0f;
        headRotor.position = headRotorStartPos.position;
        while(delta<fireRate)
        {
            yield return null;
            headRotor.position= Vector3.Lerp(headRotorStartPos.position, headRotorEndPos.position, delta/fireRate);

            delta += Time.deltaTime;
        }
        headRotor.position = headRotorEndPos.position;
    }

    protected override void Update()
    {
        base.Update();
        if(leftRotorLaser && rightRotorLaser && headRotorLeftLaserStart && headRotorLeftLaserEnd)
        {
            leftRotorLaser.SetPosition(0, headRotorLeftLaserStart.position);
            leftRotorLaser.SetPosition(1, headRotorLeftLaserEnd.position);
        }
        if (rightRotorLaser && rightRotorLaser && headRotorRightLaserStart && headRotorRightLaserEnd)
        {
            rightRotorLaser.SetPosition(0, headRotorRightLaserStart.position);
            rightRotorLaser.SetPosition(1, headRotorRightLaserEnd.position);
        }
        if (leftRotorLaser2 && rightRotorLaser && headRotorLeftLaserStart && headRotorLeftLaserEnd2)
        {
            leftRotorLaser2.SetPosition(0, headRotorLeftLaserStart.position);
            leftRotorLaser2.SetPosition(1, headRotorLeftLaserEnd2.position);
        }
        if (rightRotorLaser2 && rightRotorLaser && headRotorRightLaserStart && headRotorRightLaserEnd2)
        {
            rightRotorLaser2.SetPosition(0, headRotorRightLaserStart.position);
            rightRotorLaser2.SetPosition(1, headRotorRightLaserEnd2.position);
        }
    }
}
