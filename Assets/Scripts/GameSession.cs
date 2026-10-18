using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public int Score { get; private set; }
    public int Collected { get; private set; }
    private int total;
    private PlayerController player;
    private void Start() { total = FindObjectsByType<PickupRotator>(FindObjectsSortMode.None).Length; player = FindFirstObjectByType<PlayerController>(); }
    public void AddScore(int amount) { Score += amount; Collected++; Debug.Log($"[GameSession] Collected {Collected}/{total}, score={Score}"); }
    public static void Restart() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    private void OnGUI()
    {
        GUI.color = new Color(.08f, .13f, .21f, .96f);
        GUI.Box(new Rect(20, 20, 350, 162), GUIContent.none);
        GUI.color = Color.white;
        var title = new GUIStyle(GUI.skin.label) { fontSize = 25, fontStyle = FontStyle.Bold };
        var body = new GUIStyle(GUI.skin.label) { fontSize = 16 };
        GUI.Label(new Rect(36, 30, 320, 36), "CRYSTAL LAB", title);
        GUI.Label(new Rect(36, 72, 310, 28), $"CRYSTALS  {Collected} / {total}     SCORE  {Score}", body);
        if (player != null) GUI.Label(new Rect(36, 103, 300, 28), $"SPEED  {player.moveSpeed:0}     HEALTH  {player.health}", body);
        GUI.Label(new Rect(36, 137, 310, 27), "WASD / ARROWS   |   R to restart", body);
        if (GUI.Button(new Rect(Screen.width - 130, 24, 106, 36), "Restart")) Restart();
        if (total > 0 && Collected == total)
        {
            GUI.color = new Color(.2f, 1f, .8f);
            var win = new GUIStyle(GUI.skin.box) { fontSize = 30, alignment = TextAnchor.MiddleCenter };
            GUI.Box(new Rect(Screen.width / 2f - 200, Screen.height / 2f - 48, 400, 96), $"LEVEL COMPLETE\nScore: {Score}", win);
            GUI.color = Color.white;
        }
    }
}
