using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shotsPerSecond = 5f;

    private float nextShotTime;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (controls.Player.Shoot.IsPressed() &&
            Time.time >= nextShotTime)
        {
            Shoot();

            nextShotTime =
                Time.time + (1f / shotsPerSecond);
        }
    }

    private void Shoot()
    {
        Vector2 direction =
            firePoint.right;

        Projectile projectile =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                firePoint.rotation
            );

        projectile.SetDirection(direction);
    }
}