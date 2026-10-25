using UnityEngine;

public class BlueEnemy : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        enemyColor = ColorType.Blue;
        GetComponent<Renderer>().material.color = Color.blue;
    }
}