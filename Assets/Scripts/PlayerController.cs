using UnityEngine;

public class PlayerController : MonoBehaviour, ISwitchable
{
    [Header("Color/State Settings")]
    public ColorType currentColor = ColorType.Red;
    public Color redColor = Color.red;
    public Color blueColor = Color.blue;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRateSeconds = 1.0f;
    private float fireTimer;

    [Header("Targeting & Rotation Settings")]
    public float detectionRange = 10f;
    public float rotationSpeed = 10f;
    private Transform nearestEnemy;

    private Renderer objectRenderer;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        UpdatePlayerVisuals();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                SwitchState();
            }
        }
    }
        FindNearestEnemy();
        RotateTowardsTarget();
        HandleAutoShooting();
    }

    public void SwitchState()
    {
        currentColor = (currentColor == ColorType.Red) ? ColorType.Blue : ColorType.Red;
        UpdatePlayerVisuals();
    }

    private void UpdatePlayerVisuals()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = GetColor(currentColor);
        }
    }

    public Color GetColor(ColorType type)
    {
        return (type == ColorType.Red) ? redColor : blueColor;
    }

    private void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider col in hitColliders)
        {
            if (col.GetComponent<EnemyBase>() != null)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = col.transform;
                }
            }
        }
        nearestEnemy = closestTarget;
    }

    private void RotateTowardsTarget()
    {
        if (nearestEnemy == null) return;

        Vector3 direction = (nearestEnemy.position - transform.position);
        direction.y = 0f; 

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleAutoShooting()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRateSeconds)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();

        if (bulletComponent != null)
        {
            bulletComponent.InitializeBullet(currentColor, GetColor(currentColor));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}