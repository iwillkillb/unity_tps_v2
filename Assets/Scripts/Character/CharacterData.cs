using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [Header("Check")]
    public bool isDead = false;
    public bool isDown = false;
    public bool isBleeing = false;
    public bool isPoisoned = false;
    public float statusCheckDelay = 0.1f;

    [Header("Stat")]
    public float statLife = 100f;
    public float statAction = 100f;
    public float statStealth = 100f;
    public float statDetect = 100f;
    public float statAttackDamage = 100f;
    public float statAttackDelay = 1f;
    //public float statAttackType = 100f;
    [Range(0f, 100f)]public float statDefense = 0f;

    // Current stat
    [Header("Current Stat")]
    public float currentLife = 0f;
    public float currentAction = 0f;
    public float currentStealth = 0f;

    [Header("Recovery per second")]
    public float recoveryLife = 0.1f;
    public float recoveryAction = 0.1f;
    public float recoveryStealth = 0.1f;

    [Header("Ability")]
    public int abilityHealth = 1;
    public int abilityMentality = 1;
    public int abilityStrength = 1;
    public int abilityDexterity = 1;
    public int abilityAgility = 1;



    // Start is called before the first frame update
    void Start()
    {
        // Current values Initialization.
        currentLife = statLife;
        currentAction = statAction;
        currentStealth = statStealth;

        StartCoroutine(UpdateStatus());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This event checks status.
    IEnumerator UpdateStatus ()
    {
        while (!isDead)
        {
            // Life check
            isDead = !isDead && (currentLife <= 0f) ? true : false;

            // Recovery
            Recovery(statusCheckDelay);

            // Current values Clamping
            currentLife = Mathf.Clamp(currentLife, 0f, statLife);
            currentAction = Mathf.Clamp(currentAction, 0f, statAction);
            currentStealth = Mathf.Clamp(currentStealth, 0f, statStealth);

            yield return new WaitForSeconds(statusCheckDelay);
        }
    }

    public void TakeDamage(float rawDamage)
    {
        float damage;

        // defense : 10 -> Take damage (100 - 10)%
        damage = rawDamage - (rawDamage * (statDefense * 0.01f));

        currentLife -= damage;
    }

    void Recovery (float recoveryDelay)
    {
        if(currentLife < statLife)
        {
            currentLife += recoveryLife * recoveryDelay;
        }

        if (currentAction < statAction)
        {
            currentAction += recoveryAction * recoveryDelay;
        }

        if (currentStealth < statStealth)
        {
            currentStealth += recoveryStealth * recoveryDelay;
        }
    }
}
