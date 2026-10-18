using UnityEngine;

public class LevelValidator : MonoBehaviour
{
    private void Start()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        int count = FindObjectsByType<PickupRotator>(FindObjectsSortMode.None).Length;
        if (player == null || count != 3 || player.moveSpeed <= 0 || player.health <= 0)
        { Debug.LogError("[LevelValidator] Invalid scene: check player and pickups."); return; }
        Debug.Log($"[LevelValidator] Scene valid. Pickups: {count}. Speed: {player.moveSpeed}, health: {player.health}.");
    }
}
