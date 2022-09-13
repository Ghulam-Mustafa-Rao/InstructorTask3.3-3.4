using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecondColider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            GameManager.gameManager.playerAudioSource.PlayOneShot(GameManager.gameManager.coinCollectedClip);
            GameManager.gameManager.coinsGahtered++;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LeftBound"))
        {
            GameManager.gameManager.transform.position = new Vector3(GameManager.gameManager.transform.position.x + 0.1f, GameManager.gameManager.transform.position.y, 80);
        }
        if (collision.gameObject.CompareTag("RightBound"))
        {
            GameManager.gameManager.transform.position = new Vector3(GameManager.gameManager.transform.position.x - 0.1f, GameManager.gameManager.transform.position.y, 80);
        }
    }
}
