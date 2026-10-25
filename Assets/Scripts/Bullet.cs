using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;
    
    public ColorType bulletColor { get; private set; }
    private Renderer objectRenderer;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void InitializeBullet(ColorType type, Color colorValue)
    {
        bulletColor = type;
        if (objectRenderer != null)
        {
            objectRenderer.material.color = colorValue;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(bulletColor, 1);
            Destroy(gameObject); 
        }
    }
}
