using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Rocks : MonoBehaviour
{
    // public TextMeshProUGUI scoreText;
    public int life = 5;
    GameObject spawnedObject;
    public int initialLife = 0;
    public SpwanManager spwanManager;
    Vector3 initialSize;
    public GameObject animationImage;
    public GameObject rockImage;
    public float minimumSize = 10;
    public float maximumSize = 100;
    bool destroyercalled = false;
    public TextMeshPro scoreText;
    public int score = 0;
    bool collisionNotDone = true;
    public GameObject pausePowerUpImage;
    public bool pausePowerEnabled = false;
    bool afterPause = true;
    private void Awake()
    {
        initialSize = new Vector3(84, 84, 10);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().rotation = Random.Range(-5000, 5000);
    }

    // Update is called once per frame
    void Update()
    {
        //set ball size according to life
        if (life >= 4)
        {
            transform.localScale = new Vector3(120, 120, 1);
        }
        else if (life == 3)
        {
            transform.localScale = new Vector3(60, 60, 1);
        }
        else if (life == 2)
        {
            transform.localScale = new Vector3(30, 30, 1);
        }
        else if (life == 1)
        {
            transform.localScale = new Vector3(20, 20, 1);
        }


        //check to destroy rock and give player points
        if (life <= 0 && !destroyercalled)
        {
            collisionNotDone = false;


            //coin spwan
            /*spawnedObject = Instantiate(GameManager.gameManager.CoinPrefab, new Vector3(transform.position.x, GameManager.gameManager.CoinHolder.transform.position.y, 80), GameManager.gameManager.CoinPrefab.transform.rotation);
            spawnedObject.transform.SetParent(GameManager.gameManager.CoinHolder.transform);
            spawnedObject.transform.position = transform.position;*/

            //Destroy ball
            StartCoroutine(DestroyTheRock());

        }


        //Check to pause or unpause ball 
        if (GameManager.gameManager.pauseAllballs)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            afterPause = true;
        }
        else
        {
            if (afterPause)
            {
                afterPause = false;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(Vector2.down * 30, ForceMode2D.Impulse);
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!destroyercalled)
        {
            if (collision.gameObject.CompareTag("Bullet") && collisionNotDone)
            {
                collisionNotDone = false;
                Destroy(collision.gameObject);
                life--;
                if (life > 0)
                {
                    //Divide ball into two
                    for (int i = 0; i < 2; i++)
                    {
                        spawnedObject = Instantiate(spwanManager.stone, transform.position, Quaternion.identity);
                        spawnedObject.transform.SetParent(spwanManager.rockHolder.transform);
                        spawnedObject.GetComponent<Rocks>().spwanManager = spwanManager;
                        spawnedObject.GetComponent<Rocks>().life = life;
                        spawnedObject.GetComponent<Rocks>().initialLife = life;
                        if (life == 1)
                        {
                            if (Random.Range(1, 6) % 2 == 0)
                            {
                                spawnedObject.GetComponent<Rocks>().pausePowerUpImage.SetActive(true);
                                spawnedObject.GetComponent<Rocks>().pausePowerEnabled = true;
                            }
                        }
                        if (i == 0)
                            spawnedObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 30, ForceMode2D.Impulse);
                        else
                            spawnedObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
                    }
                }

                StartCoroutine(DestroyTheRock());
            }
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!destroyercalled)
        {
            if (collision.gameObject.CompareTag("Bullet") && collisionNotDone)
            {
                collisionNotDone = false;
                Destroy(collision.gameObject);
                life--;
                if (life > 0)
                {
                    //Divide ball into two
                    for (int i = 0; i < 2; i++)
                    {
                        spawnedObject = Instantiate(spwanManager.stone, transform.position, Quaternion.identity);
                        spawnedObject.transform.SetParent(spwanManager.rockHolder.transform);
                        spawnedObject.GetComponent<Rocks>().spwanManager = spwanManager;
                        spawnedObject.GetComponent<Rocks>().life = life;
                        spawnedObject.GetComponent<Rocks>().initialLife = life;
                        if (i == 0)
                            spawnedObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 30, ForceMode2D.Impulse);
                        else
                            spawnedObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
                    }
                }
                StartCoroutine(DestroyTheRock());


            }
            if (collision.gameObject.CompareTag("Player") && !GameManager.gameManager.pauseAllballs)
            {
                GameManager.gameManager.playerAudioSource.PlayOneShot(GameManager.gameManager.gameOverClip);
                GameManager.gameManager.playerLifes--;
                GameManager.gameManager.gameOver = true;
            }
            if (collision.gameObject.CompareTag("Ground") && !GameManager.gameManager.pauseAllballs)
            {
                //give extra force from left or right
                if (GetComponent<Rigidbody2D>().velocity.x == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        GetComponent<Rigidbody2D>().AddForce(Vector2.left * 30, ForceMode2D.Impulse);
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().AddForce(Vector2.right * 30, ForceMode2D.Impulse);
                    }
                }
            }
            //add some random angualr velocity to rotate 
            if (GetComponent<Rigidbody2D>().angularVelocity == 0)
                GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-1000, 1000);
        }

    }

    IEnumerator DestroyTheRock()
    {
        destroyercalled = true;
        GameManager.gameManager.playerAudioSource.PlayOneShot(GameManager.gameManager.stoneDestroyedClip);
        rockImage.SetActive(false);
        float waitTimeToShowScore = 0.001f;
        //scoreText.gameObject.SetActive(false);
        animationImage.SetActive(true);

        if (life <= 0)
        {
            score = 1800;
            GameManager.gameManager.score += score;
            scoreText.text = score.ToString();
            scoreText.gameObject.SetActive(true);
            if (pausePowerEnabled && !GameManager.gameManager.pauseAllballs)
            {
                GameManager.gameManager.pauseAllballs = true;
                GameManager.gameManager.pauseAllBallsTimer();
            }
            waitTimeToShowScore = 0.30f;
        }
        
        yield return new WaitForSeconds(0.30f);
        animationImage.SetActive(false);
        
        //wait to show score
        yield return new WaitForSeconds(waitTimeToShowScore);
        Destroy(gameObject);
    }





}
