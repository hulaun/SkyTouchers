using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] GameObject goInText;
       
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            goInText.SetActive(true);
            if(Input.GetKeyDown(KeyCode.N))
            {
                ScenesManager.Instance.LoadNextScene();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ScenesManager.Instance.LoadNextScene();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            goInText.SetActive(false);
        }
    }
}
