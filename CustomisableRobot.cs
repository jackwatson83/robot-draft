using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CustomisableRobot : MonoBehaviour
{
    #region variables
    [Header("Part Slots")]
    public GameObject headSlot;
    public GameObject bodySlot;
    public GameObject baseSlot;
    public GameObject weaponSlot;

    private PartSlot head;
    private PartSlot body;
    private PartSlot baseSl;
    private PartSlot weapon;

    RobotPart headPart;
    RobotPart bodyPart;
    RobotPart basePart;
    RobotPart weaponPart;

    NavMeshAgent agent;
    GameObject otherRobotGO;
    CustomisableRobot otherRobotCR;
    [SerializeField] RobotBattleManager BM;

    bool isChasing = false;
    bool isAttacking = false;
    public bool isDead = false;
    int attacks;

    [Header("UI Elements")]
    [SerializeField] private Text healthTextBox;
    [SerializeField] private Text armourTextBox;
    [SerializeField] private Text damageTextBox;
    [SerializeField] private Text rangeTextBox;
    [SerializeField] private Text speedTextBox;

    [Header("Robot Stats [DEBUG]")]
    [SerializeField] float robotHealth;
    [SerializeField] float robotArmour;
    [SerializeField] float robotDamage;
    [SerializeField] float robotAttackRange;
    [SerializeField] float robotSpeed;

    // Testing alternative for stats
    //RobotStatistics statModifiers;

    float htemp;
    float atemp;
    float dtemp;
    float rtemp;
    float stemp;
    #endregion

    #region methods
    private void Awake()
    {
        head = headSlot.GetComponent<PartSlot>();
        body = bodySlot.GetComponent<PartSlot>();
        baseSl = baseSlot.GetComponent<PartSlot>();
        weapon = weaponSlot.GetComponent<PartSlot>();

        SetHead(head.currentPart);
        SetBody(body.currentPart);
        SetBase(baseSl.currentPart);
        SetWeapon(weapon.currentPart);

        Debug.Log("Setting Parts: " + headPart.name + bodyPart.name + basePart.name + weaponPart.name);

        SetStats(0f, 0f, 0f, 0f, 0f);

        UpdateRobot();

        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (BM.battleState == BATTLESTATE.ACTIVE)
        {
            
            if (isChasing && !isAttacking)
            {
                Chase();
            }
            
            //CheckForAttack(); <- part of Chase().
        }
    }

    /// <summary>
    /// Updates all of the slots of the robot at once
    /// </summary>
    public void UpdateRobot()
    {
        head.UpdateSlot();
        body.UpdateSlot();
        baseSl.UpdateSlot();
        weapon.UpdateSlot();        

        UpdateStats();
    }

    /// <summary>
    /// Changes the ScriptableObject used in the Head slot
    /// </summary>
    /// <param name="p">The part to put in the head slot</param>
    public void ChangeHead(RobotPart p)
    {
        if (p.type == PartType.Head)
        {
            head.SetPart(p);
        }        
    }

    /// <summary>
    /// Changes the ScriptableObject used in the Body slot
    /// </summary>
    /// <param name="p">The part to put in the body slot</param>
    public void ChangeBody(RobotPart p)
    {
        if (p.type == PartType.Body)
        {
            body.SetPart(p);
        }        
    }


    /// <summary>
    /// Changes the ScriptableObject used in the Base slot
    /// </summary>
    /// <param name="p">The part to put in the base slot</param>
    public void ChangeBase(RobotPart p)
    {
        if (p.type == PartType.Base)
        {
            baseSl.SetPart(p);
        }        
    }

    /// <summary>
    /// Changes the ScriptableObject used in the weapon slot
    /// </summary>
    /// <param name="p">The part to put in the weapon slot</param>
    public void ChangeWeapon(RobotPart p)
    {
        if (p.type == PartType.Weapon)
        {
            weapon.SetPart(p);
        }        
    }

    public void SetHead(RobotPart p)
    {
        headPart = p;
        Debug.Log("Setting Head " + p.name);
        ChangeHead(headPart);
    }

    public void SetBody(RobotPart p)
    {
        bodyPart = p;
        Debug.Log("Setting Body " + p.name);
        ChangeBody(bodyPart);
    }

    public void SetBase(RobotPart p)
    {
        basePart = p;
        Debug.Log("Setting Base " + p.name);
        ChangeBase(basePart);
    }

    public void SetWeapon(RobotPart p)
    {
        weaponPart = p;
        Debug.Log("Setting Weapon " + p.name);
        ChangeWeapon(weaponPart);
    }

    /// <summary>
    /// Calculate the overall stats of the robot, using the values from each part.
    /// </summary>
    void UpdateStats()
    {
        //float h = 0;
        //Debug.Log(h + " before calc");
        //Debug.Log("Current Parts: " + headPart.name + bodyPart.name + basePart.name + weaponPart.name);
        float h = headPart.healthMod + bodyPart.healthMod + basePart.healthMod + weaponPart.healthMod;
        //Debug.Log(h + " after");
        float a = headPart.armourMod + bodyPart.armourMod + basePart.armourMod + weaponPart.armourMod;
        float d = headPart.damageMod + bodyPart.damageMod + basePart.damageMod + weaponPart.damageMod;
        float r = headPart.rangeMod + bodyPart.rangeMod + basePart.rangeMod + weaponPart.rangeMod;
        float s = headPart.speedMod + bodyPart.speedMod + basePart.speedMod + weaponPart.speedMod;
        //Debug.Log(h + "," + a + "," + d + "," + r + "," + s);
        //statModifiers.SetStats(htemp, atemp, dtemp, rtemp, stemp);
        SetStats(h, a, d, r, s);
        UpdateStatUI();
    }

    void SetStats(float h, float a, float d, float r, float s)
    {
        robotHealth = h;
        robotArmour = a;
        robotDamage = d;
        robotAttackRange = r;
        robotSpeed = s;
    }

    /// <summary>
    /// This method changes the values displayed on the screen, so the players know the state(s) of each robot.
    /// </summary>
    void UpdateStatUI()
    {
        healthTextBox.text = "Health: " + robotHealth;
        armourTextBox.text = "Armour: " + robotArmour;
        damageTextBox.text = "Damage: " + robotDamage;
        rangeTextBox.text = "Range: " + robotAttackRange;
        speedTextBox.text = string.Format("Speed: {0}", robotSpeed);
    }

    /// <summary>
    /// Method for changing the health of the robot
    /// </summary>
    /// <param name="difference">The amount to change the health value by</param>
    /// <param name="isDamage">This bool identifies whether to subtract the value (true) or add the value (false)</param>
    public void UpdateHealth(float difference, bool isDamage)
    {
        Debug.Log(this.gameObject.name + " is taking damage");
        if (isDamage)
        {
            robotHealth -= difference;
            UpdateStatUI();
        }
        else if (!isDamage)
        {
            robotHealth += difference;
            UpdateStatUI();
        }

        if(robotHealth <= 0)
        {
            isDead = true;
        }

        string t = "";
        if (isDamage)
        {
            t = " lost ";
        }
        else
        {
            t = " gained ";
        }
        Debug.Log(this.gameObject.name + t + difference + " health");
    }

    #region return methods
    public float GetRobotHealth()
    {
        return robotHealth;
    }

    public float GetRobotArmour()
    {
        return robotArmour;
    }

    public float GetRobotDamage()
    {
        return robotDamage;
    }

    public float GetRobotAttackRange()
    {
        return robotAttackRange;
    }

    public float GetRobotSpeed()
    {
        return robotSpeed;
    }
    #endregion

    #region battle ai methods
    /// <summary>
    /// Set the other robot to be this one's opponent, and chase after it.
    /// </summary>
    /// <param name="opponent">The other robot</param>
    public void SetTargetRobot(CustomisableRobot opponent)
    {
        otherRobotCR = opponent;
        otherRobotGO = otherRobotCR.gameObject;

        isChasing = true;
    }

    /// <summary>
    /// This method sets the robot's navmesh destination to that of the opposing robot
    /// Also checks if the robot can attack the other robot
    /// </summary>
    void Chase()
    {
        agent.SetDestination(otherRobotGO.transform.position);
        agent.isStopped = false;
        CheckForAttack();
    }

    /// <summary>
    /// This method checks if the robot is in range of the attack target, and attacks if it is.
    /// </summary>
    void CheckForAttack()
    {
        if (Vector3.Distance(this.gameObject.transform.position, otherRobotGO.transform.position) <= robotAttackRange)
        {
            isAttacking = true;
            isChasing = false;
            Attack(otherRobotCR);
        }
        else
        {
            isAttacking = false;
        }
    }

    /// <summary>
    /// This method calculates and applies damage to the opposing robot, based on stat values from each robot
    /// Contains a check for if the opposing robot dies after taking damage,
    ///     if yes, end the game
    ///     if no, start the attack cooldown
    /// </summary>
    /// <param name="opponent">The robot that is being attacked.</param>
    void Attack(CustomisableRobot opponent)
    {     
        //Armour prevents a percentage of the damage done
        float opponentArmourModifier = (100 - opponent.robotArmour) / 100;
        float damage = robotDamage * opponentArmourModifier;
        if (damage < 1)
        {
            damage = 1;
        }
        Debug.Log(this.gameObject.name + " damage to deal: " + damage);

        opponent.UpdateHealth(damage, true);

        if(!opponent.isDead)
        {
            StartCoroutine(AttackCooldown());
        }
        else
        {
            BM.EndBattle(this.gameObject);
        }

        //attacks++;
        //Debug.Log(this.gameObject.name + ": attack #" + attacks);
    }
    #endregion

    /// <summary>
    /// Coroutine to space out robot attacks;
    /// Without this, the fight would be decided instantly and players would not be able to watch the battle.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCooldown()
    {
        Debug.Log(this.gameObject.name + " cooldown");
        isAttacking = false;
        
        yield return new WaitForSeconds(robotSpeed);

        isChasing = true;
    }
    #endregion
}
