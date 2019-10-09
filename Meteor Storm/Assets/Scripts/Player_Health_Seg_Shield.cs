using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health_Seg_Shield : MonoBehaviour
{
    // InstaDeath objects should be tagged "Death" and set as a trigger
    // Enemies (and other 1-damage obstacles) should be tagged "Enemy" and should NOT be set as a trigger

    // The shield/health system here is designed to be similar to the one in The Binding of Isaac.
    // This will create two different types of health, life hearts and shield hearts. Each will have a 
    //      different pickup item, and behave slightly differently. More functionality can be scripted
    //      to make each more unique.
    // A player can hold *maxLife* life hearts, and after taking damage, an empty container is visible where the heart
    //      once was. Picking up a health item restores one heart, but health cannot exceed *maxLife*. If the player
    //      has zero life hearts after taking damage, they die and respawn with all of their life hearts replenished.
    // A player can hold *maxShield* shield hearts, but the number held can vary. After taking damage, the shield heart 
    //      disappears from the UI. If a player takes damage while they have shield hearts, they lose a shield heart
    //      instead of a life heart. On respawn, no shield hearts are replenished.
    private GameObject respawn;

    private int playerScore;


    [Tooltip("The score value of a coin or pickup.")]
    public int coinValue = 5;
    [Tooltip("The amount of points a player loses on death.")]
    public int deathPenalty = 20;

    public Text scoreText;

    //This script doesn't support the player gaining health containers in the same scene.
    //You'd have to make some changes in how these are assigned in order to do that
    [SerializeField] private int maxLife = 3;

    [SerializeField] private int maxShield = 6;
    private GameObject[] healthArr;

    public Transform uiPos;
    public GameObject heartProto;
    public Sprite heartContainer;
    public Sprite lifeHeart;
    public Sprite shieldHeart;

    //The distance between hearts in the UI
    //TODO: Make it work with different screen sizes
    public float heartInterval;

    private int maxTotalHearts;

    private int currLife;
    private int currShield;

    // Use this for initialization
    void Start()
    {
        maxTotalHearts = maxLife + maxShield;
        healthArr = new GameObject [maxTotalHearts];
        for (int i = 0; i < maxTotalHearts; ++i)
        {
            healthArr[i] = Instantiate(heartProto, uiPos, false);
            healthArr[i].transform.Translate(heartInterval * i, 0, 0);
            if (i < maxLife)
            {
                healthArr[i].SetActive(true);
                healthArr[i].GetComponent<Image>().sprite = lifeHeart;
            } else
            {
                healthArr[i].SetActive(false);
                healthArr[i].GetComponent<Image>().sprite = shieldHeart;
            }

        }
        currLife = maxLife;
        currShield = 0;
        respawn = GameObject.FindGameObjectWithTag("Respawn");
        playerScore = 0;
        //scoreText.text = playerScore.ToString("D4");

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Respawn();
        }
        else if (collision.CompareTag("Coin"))
        {
            AddPoints(coinValue);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Finish"))
        {
            Time.timeScale = 0;
        }
        else if (collision.CompareTag("Health"))
        {
            AddHealth();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Shield"))
        {
            AddShield();
            Destroy(collision.gameObject);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (currShield > 0)
        {
            healthArr[maxLife + currShield - 1].SetActive(false);
            --currShield;
        }
        else
        {
            healthArr[currLife - 1].GetComponent<Image>().sprite = heartContainer;

            --currLife;
            if (currLife == 0)
            {
                Respawn();
            }
        }

    }

    private void AddHealth()
    {
        if (currLife < maxLife)
        {
            healthArr[currLife].GetComponent<Image>().sprite = lifeHeart;
            ++currLife;
        }

        // For more health, just copy the else if block for health3 and change the name.*/
    }

    private void AddShield()
    {
        if (currShield < maxShield)
        {
            healthArr[maxLife + currShield].SetActive(true);
            ++currShield;
        }
    }

    public void Respawn()
    {

        while (currLife < maxLife)
        {
            healthArr[currLife].GetComponent<Image>().sprite = lifeHeart;
            ++currLife;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.transform.position = respawn.transform.position;
        AddPoints(deathPenalty);
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void AddPoints(int amount)
    {
        playerScore += amount;
        scoreText.text = playerScore.ToString("D4");
    }
}
