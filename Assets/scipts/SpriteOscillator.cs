using UnityEngine;

public class SpriteOscillator : MonoBehaviour
{
    public float maxAngle = 15f;
    public float oscillationSpeed = 6f;
    public float returnSpeed = 8f;

    private Transform player;
    private PlayerMovement movementScript;

    void Start()
    {
        player = transform.parent;
        movementScript = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (movementScript == null) return;

        if (movementScript.IsMoving())
        {
            float angle = Mathf.Sin(Time.time * oscillationSpeed) * maxAngle;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Smoothly return to center
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                Quaternion.identity,
                returnSpeed * Time.deltaTime
            );
        }
    }
}