using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int health = 100;
    private CharacterController controller;
    private void Awake() { controller = GetComponent<CharacterController>(); }
    private void Start() { Debug.Log($"[PlayerController] speed={moveSpeed}, health={health}"); }
    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        controller.Move((Vector3.ClampMagnitude(input, 1f) * moveSpeed + Vector3.down * 9.81f) * Time.deltaTime);
        if (input.sqrMagnitude > .01f) transform.rotation = Quaternion.LookRotation(input);
        if (Input.GetKeyDown(KeyCode.R)) GameSession.Restart();
    }
    private void OnTriggerEnter(Collider other)
    {
        PickupRotator pickup = other.GetComponent<PickupRotator>();
        if (pickup != null) pickup.Collect();
    }
}
