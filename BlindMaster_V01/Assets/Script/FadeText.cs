using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{

    float mo_Speed = 0.1f;
    //Time that will be exist in the world
    float t = 1f;
    float fadeTime = 1f;

    public CanvasGroup canvas;
    //private Text pointText;


    private void OnEnable()
    {
        StartCoroutine(Fade());
    }


    IEnumerator Fade()
    {
        while (t >= 0)
        {
            //Fade out 
            canvas.alpha = t;
            t -= 0.1f;

            //Move Up
            Vector2 newPos = new Vector2
                (transform.position.x, transform.position.y + mo_Speed);

            this.gameObject.transform.position = Vector2.Lerp(transform.position, newPos, fadeTime);

            yield return new WaitForSeconds(0.05f);
        }

        //Debug.Log("Display destroyed");
        Destroy(this.gameObject);

    }
}
