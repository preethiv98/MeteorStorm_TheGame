using System.Diagnostics;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC_Audio : MonoBehaviour
{
    private Text message;
    private bool notHit;
    private AudioSource audioSource;
    public AudioClip textSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = textSound;
        //Create Text component of a canvas object through unity named "Message"
        message = GameObject.Find("Message").GetComponent<Text>();
        //message.color = Color.white;
        message.text = "";
        notHit=true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && notHit)
        {
            StartCoroutine(displayText(other));

        }
        //Message to display if main dialogue is done
        else if(other.gameObject.tag == "Player" && !notHit)
        {
            message.text = "You are going the wrong way";
        }
    }
    IEnumerator displayText(Collider2D other)
    {
        message.text = "Green Water kills Robots";
        other.gameObject.GetComponent<Move2D>().speed = 0;
        //will continue after mouse button is clicked
        //copy this block for every new line of dialouge
        while (!Input.GetMouseButtonDown(0)) {
        yield return null;
        }
        audioSource.Play();

        message.text = "Beware the Green Water";
        //



        //let player move again
        other.gameObject.GetComponent<Move2D>().speed = 5;
        notHit=false;
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            message.text = "";
        }
        audioSource.Play();
    }
}
