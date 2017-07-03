using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public GameObject player;
    private Rigidbody rb;
    private Quaternion rot;
    [SerializeField]
    float speed = 5f;
    float boostTime;

    // Use this for initialization
    void Start()
    {
        boostTime = Time.time;
        player.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        rb = player.GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.gyro.enabled)
        {
            GyroMove();
        }
        else
        {
            AcceleroMove();
        }
    }

    public void Boost()
    {
        if (Time.time > boostTime)
        {
            boostTime = Time.time + 3f;
            rb.velocity += transform.forward * 6f;
        }
    }
    
    //IEnumerator BoostDecelerate()
    //{
    //    while ()
    //}
    void GyroMove()
    {
        float moveHorizontal = Input.gyro.gravity.x;
        float moveVertical = Input.gyro.gravity.y;
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        rb.AddForce(movement.normalized * speed);

    }
    public void AcceleroMove()
    {
        float moveHorizontal = Input.acceleration.x;
        float moveVertical = Input.acceleration.y;
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        rb.AddForce(movement.normalized * speed);
    }
}
