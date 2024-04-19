using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] int Speedbase;
    public int playerSpeed;
    public bool sneaking;
    public float rotateSpeed;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // sneak mode
        {
            playerSpeed = Speedbase / 2;
            sneaking = true;
        }
        else
        {
            playerSpeed = Speedbase;
            sneaking = false;
        }

        if (Input.GetKey(KeyCode.W)) // move forward or back
        {
            //forward
            transform.position += transform.forward * playerSpeed * Time.deltaTime;
            //Quaternion yAng = transform.rotation.y;
            //transform.rotation = Quaternion.Euler(0, yAng, 0);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);


        }
        else if (Input.GetKey(KeyCode.S))
        {
            //back
            transform.position -= transform.forward * playerSpeed * Time.deltaTime;
            //float yAng = transform.rotation.y;
            //transform.rotation = Quaternion.Euler(0, yAng, 0);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);


        }

        //separate so both can happen at same time
        if (Input.GetKey(KeyCode.D)) // turn left or right
        {
            //right
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            transform.Rotate(0.0f, rotateSpeed, 0.0f, Space.World);
            //transform.rotation = Quaternion.Euler(0f, transform.rotation.y*Time.deltaTime, 0f);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            transform.Rotate(0.0f, -rotateSpeed, 0.0f, Space.World);
            //transform.rotation = Quaternion.Euler(0f, transform.rotation.y*Time.deltaTime, 0f);
            //left
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }

    public void GotCaught()
    {
        Debug.Log("HIT");
        GetComponent<PlayerScoreScript>().OpenLoseScreen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackHitBox"))
        {
            GotCaught();
        } else if (other.gameObject.CompareTag("Goal"))
        {
            GetComponent<PlayerScoreScript>().UpScore();
            other.gameObject.SetActive(false);
        }
    }
}
