using System.Collections;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class CrossBowTower : BaseTower
{

    [SerializeField] private Animator anim;

    [SerializeField] private LineRenderer laserRenderer;

    [SerializeField] private float laserDuration=0.3f;

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
        laserRenderer.enabled = true;
        Debug.DrawLine(shootPoint.position, attackPoint, Color.yellow);
        CameraShake.Instance.Shake();
    }

    protected override IEnumerator coolDown()
    {
        yield return new WaitForSeconds(laserDuration);
        laserRenderer.enabled = false;

        yield return base.coolDown();
    }
}
