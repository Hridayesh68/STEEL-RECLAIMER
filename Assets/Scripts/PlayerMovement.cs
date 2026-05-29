using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed=7f;
    public float jumpforce=4f;
    public Rigidbody rb;
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float movex=Input.GetAxis("Horizontal");
        float movez=Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(movez,0,movex);
         Vector3 velocity = movement * moveSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

    }
}
