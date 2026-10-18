using UnityEngine;

public class PickupRotator : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public int scoreValue = 10;
    private bool collected;
    private void Update() { transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World); }
    public void Collect()
    {
        if (collected) return;
        collected = true;
        GameSession session = FindFirstObjectByType<GameSession>();
        if (session != null) session.AddScore(scoreValue);
        gameObject.SetActive(false);
    }
}
