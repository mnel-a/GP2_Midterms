using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Enemy Settings")]
    public ColorType enemyColor;
    public float moveSpeed = 3f;
    public float stopDistance = 1f;

    protected Transform playerTransform;

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    protected virtual void Update()
    {
        MoveTowardsPlayer();
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position);
        direction.y = 0f;
        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, playerTransform.position) <= stopDistance)
        {
            TriggerGameOver();
        }
    }

    protected virtual void TriggerGameOver()
    {
        Debug.Log("GAME OVER: Enemy reached player!");
        Time.timeScale = 0f; 
    }

    public virtual void TakeDamage(ColorType bulletColor, int amount)
    {
        if (bulletColor == enemyColor)
        {
            Destroy(gameObject);
        }
    }
}