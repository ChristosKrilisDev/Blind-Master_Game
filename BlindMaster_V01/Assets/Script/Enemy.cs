using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IKillable
{

    [Header("My Key ui")]
    public Text keyText;
    private KeyCode keyCode;


    [Header("Enemy Stats")]
    private Rigidbody2D e_Rb;
    public float speed = 5;
    private int damage = 10;
    [HideInInspector]public bool isReadyToDie;


    [Header("Explosion prefs")]
    public GameObject explosionPref;
    public GameObject explosionPos;

    private void Awake()
    {
        e_Rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        isReadyToDie = false;
    }

    void Update()
    {
        float force = speed * Time.deltaTime;

        Vector2 posToGo = Vector2.left * speed;

        e_Rb.velocity = posToGo;   
    }


    public KeyCode GetKey()
    {
        return this.keyCode;
    }
    
    public void SetKey(KeyCode keyCode)
    {
        this.keyCode = keyCode;

        keyText.text = this.keyCode.ToString();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StartGate")
        {
            //Enemy is inside the player view 
            //enable killable methods;
            isReadyToDie = true;
        }

        if (collision.tag == "Destroyer")
        {
            //Remove enemy from the active list
            GameManager.instance.ResetEnemyOnDefaultDeath(this);
            gameObject.SetActive(false);
        }

        IDamagable playerTarget = (IDamagable)collision.gameObject.GetComponent(typeof(IDamagable));
        if (collision.gameObject.tag == "Player" && playerTarget != null)
        {
            //Deal damage to the player
            playerTarget.TakeDamage(damage);

            //Remove enemy from the active list
            GameManager.instance.ResetEnemyOnDefaultDeath(this);

            //Destroy itself
            Kill();

        }
    }


    public void Kill()
    {
        if (isReadyToDie)
        {
            //Create the explotion
            GameObject newExplo = Instantiate(explosionPref);

            newExplo.transform.position = explosionPos.transform.position;

            //destroy your self
            gameObject.SetActive(false);
        }
    }
}
