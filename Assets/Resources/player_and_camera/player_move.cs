using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20f;

    private Rigidbody rb;
    private void Start()
    {
        rb=GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D 或 左/右箭头
        float moveVertical = Input.GetAxis("Vertical"); // W/S 或 上/下箭头

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity=movement*speed;
        
    }

}