using UnityEngine;

public class CropSway : MonoBehaviour
{
    public float maxAngle = 10f;
    public float speed = 2f;

    private Vector3 parentPosition;
    private float currentAngle;

    void Start()
    {
        parentPosition = transform.parent.position;
    }

    void Update()
    {
        float newAngle = Mathf.Sin(Time.time * speed) * maxAngle;
        float delta = newAngle - currentAngle;

        transform.RotateAround(parentPosition, Vector3.forward, delta);
        currentAngle = newAngle;
    }
}   