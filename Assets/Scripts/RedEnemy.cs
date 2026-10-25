using UnityEngine;

public class RedEnemy : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        enemyColor = ColorType.Red;
        GetComponent<Renderer>().material.color = Color.red;
    }
}