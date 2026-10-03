using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private PlayerControls controls;

    private Vector2 mouseScreenPosition;

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
        // Get mouse position from the New Input System.
        mouseScreenPosition = controls.Player.Aim.ReadValue<Vector2>();

        // Convert screen coordinates into world coordinates.
        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Find the direction from the player toward the mouse.
        Vector2 direction =
            mouseWorldPosition - transform.position;

        // Convert direction into an angle.
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the player toward the mouse.
        transform.rotation =
            Quaternion.Euler(0f, 0f, angle); 
    }
}