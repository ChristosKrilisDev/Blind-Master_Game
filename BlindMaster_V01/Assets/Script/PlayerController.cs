using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Player Members")]
    public int p_MaxHealth = 100;
    private int p_CurrentHealth;
    [Space]
    public Color onhitColor;
    private SpriteRenderer playerGraphics;
    private Color defaultColor;

    [Header("Health UI")]
    public Text healthTxt;
    public Image healthBar;

    private bool isDead;
    private Animator p_Anim;

    //private delegate void OnTakeDamageDisplay(int num);
    //event OnTakeDamageDisplay onTakeDamageDisplay; 


    private void Awake()
    {
        p_Anim = GetComponent<Animator>();

        playerGraphics = transform.GetChild(0).GetComponent<SpriteRenderer>();
        defaultColor = playerGraphics.GetComponent<SpriteRenderer>().color;
    }

    void Start()
    {
        isDead = false;
        p_Anim.SetBool("IsDead", isDead);

        p_CurrentHealth = p_MaxHealth;

        //Set the ui 
        healthBar.fillAmount = 1f;
        healthTxt.text = "100%";

    }

    void OnGUI()
    {
        if (isDead || GameManager.instance.isMenuActive)
            return;
        if (Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            //On key up return None  
            if (Event.current.keyCode.ToString() == "None")
                return;

            //Debug.Log("Player Pressed : "+Event.current.keyCode);


            p_Anim.SetTrigger("IsAttaking");
            //else check the input
            GameManager.instance.CheckPlayerInput(Event.current.keyCode);
        }
    }

    void Update()
    {
        if (isDead)
            return;

        HealthController();
    }

    void HealthController()
    {
        if (p_CurrentHealth <= 0)
        {

            //Is dead

            isDead = true;
            p_Anim.SetBool("IsDead", isDead);
            p_Anim.SetTrigger("IsDeadtr");


            GameManager.instance.OnDeath();
            //Do stuffs
            //Pasue menu. score etc.
        }
    }

    public void TakeDamage(int dmg)
    {     
        p_CurrentHealth -= dmg;


        //Display it 
        DisplayHealth(p_CurrentHealth);

        StartCoroutine(OnTakeDamage());
    }

    IEnumerator OnTakeDamage()
    {
      
        //On hit change players color
        playerGraphics.color = onhitColor;

        yield return new WaitForSeconds(1f);
        //reset the color
        playerGraphics.color = defaultColor;
    }


    private void DisplayHealth(int currentH)
    {
        float val = (float)currentH / 100; 

        healthBar.fillAmount = val;

        healthTxt.text = (val * 100f).ToString()+"%";
    }
}
