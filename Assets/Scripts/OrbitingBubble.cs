using UnityEngine;

public class OrbitingBubble : MonoBehaviour
{
    private Transform orbitCenter;
    public float orbitSpeed = 1f;
    public float orbitRadius = 2f;

    private float angle;

    public void SetOrbitCenter(Transform center)
    {
        orbitCenter = center;
    }

    private void Update()
    {
        if (orbitCenter == null) return;

        angle += orbitSpeed * Time.deltaTime;
        float x = orbitCenter.position.x + Mathf.Cos(angle) * orbitRadius;
        float y = orbitCenter.position.y + Mathf.Sin(angle) * orbitRadius;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
