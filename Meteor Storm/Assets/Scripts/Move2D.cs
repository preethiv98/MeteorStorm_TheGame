using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class Move2D : MonoBehaviour
{

    public float speed;


    public Boundary boundary;


    void FixedUpdate()
    {
        //CONTROLS
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        GetComponent<Rigidbody2D>().velocity = movement * speed;

        GetComponent<Rigidbody2D>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax),
                0.0f
                );
        //ANIMATIONS
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Animator>().SetBool("WalkingUp", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("WalkingUp", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Animator>().SetBool("WalkingLeft", true);            
        }
        else
        {
            GetComponent<Animator>().SetBool("WalkingLeft", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Animator>().SetBool("WalkingDown", true);            
        }
        else
        {
            GetComponent<Animator>().SetBool("WalkingDown", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Animator>().SetBool("WalkingRight", true);            
        }
        else
        {
            GetComponent<Animator>().SetBool("WalkingRight", false);
        }
    }
}
