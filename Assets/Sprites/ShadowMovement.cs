using UnityEngine;

public class ShadowMovement : MonoBehaviour
{
    private Vector3 startPos;
    public float speed = 0.5f;
    public float amplitude = 0.2f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + new Vector3(Mathf.Sin(Time.time * speed) * amplitude, 0, 0);
    }
}
