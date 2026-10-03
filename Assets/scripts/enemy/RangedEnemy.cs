using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.2f;

    [Header("Combat")]
    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootingDistance = 7f;
    [SerializeField] private float shotsPerSecond = 0.8f;

    private Transform player;
    private Rigidbody2D rb;

    private float nextShotTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            player.position - transform.position;

        float distance =
            direction.magnitude;

        direction.Normalize();

        if (distance > shootingDistance)
        {
            rb.linearVelocity =
                direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (distance <= shootingDistance &&
            Time.time >= nextShotTime)
        {
            Shoot(direction);

            nextShotTime =
                Time.time + (1f / shotsPerSecond);
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (projectilePrefab == null ||
            firePoint == null)
        {
            return;
        }

        EnemyProjectile projectile =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.LookRotation(
                    Vector3.forward,
                    direction
                )
            );

        projectile.SetDirection(direction);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyShoot();
        }
    }
}