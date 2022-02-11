
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // unit's stats
    [SerializeField] private float damage = default;
    [SerializeField] private float speed = default;
    [SerializeField] private float combatRange = default;
    [SerializeField] private float attackSpeed = default;
    [SerializeField] private float armor = default;
    public float health = default;
    public float isPlayerUnit = default; // -1 for enemy, 1 for player

    // variables used for checking for enemies in range
    private Vector2 movementDistance;
    private Vector2 enemyDetection;
    private BoxCollider2D unitCollider;
    private Collider2D[] detectedEnemy;
    private UnitController enemyController = default;
    //private CastleHealth targetedCastleHealth = default;

    // defines whether the unit is in combat
    private bool isFighting = false;
    public bool attackInitiated = false;
    public bool isDying = false;

    // animations
    public Animator unitAnimator = default;

    // Start is called before the first frame update
    void Start()
    {
        unitCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDying)
        {
            MarchOnwards();
        }
    }
    public void ReceiveDamage(float ammount)
    {
        health -= ammount * (100 - armor) / 100;
        if (health <= 0)
        {
            // Call the unit death animation
            isFighting = false;
            isDying = true;
            StartCoroutine(DeathAnimation());
        }
    }

    private void MarchOnwards()
    {
        if (isFighting == false)
        {
            unitAnimator.SetBool("isRunning", true);
            unitAnimator.SetBool("isFighting", false);
            // unit movement
            movementDistance = new Vector2(isPlayerUnit, 0) * Time.deltaTime * speed;
            transform.Translate(movementDistance);

            // enemy detection
            enemyDetection = new Vector2(transform.position.x + (unitCollider.size.x / 2) * isPlayerUnit, transform.position.y);
            detectedEnemy = Physics2D.OverlapCircleAll(enemyDetection, combatRange);

            // enemy detection logic
            if (detectedEnemy != null)
            {
                for (int i = 0; i < detectedEnemy.Length; i++)
                {
                    if (detectedEnemy[i].gameObject.CompareTag("Ally") && isPlayerUnit == -1 || detectedEnemy[i].gameObject.CompareTag("Enemy") && isPlayerUnit == 1)
                    {
                        isFighting = true;
                        unitAnimator.SetBool("isRunning", false);
                        unitAnimator.SetBool("isFighting", true);
                        StartCoroutine(EngageCombat());
                    }
                }
            }
        }
    }

    private IEnumerator EngageCombat()
    {
        while (isFighting == true)
        {
            attackInitiated = false;
            enemyDetection = new Vector2(transform.position.x + (unitCollider.size.x / 2) * isPlayerUnit, transform.position.y);
            detectedEnemy = Physics2D.OverlapCircleAll(enemyDetection, combatRange);

            if (detectedEnemy != null)
            {
                for (int i = 0; i < detectedEnemy.Length; i++)
                {
                    if (detectedEnemy[i].gameObject.CompareTag("Ally") && isPlayerUnit == -1 || detectedEnemy[i].gameObject.CompareTag("Enemy") && isPlayerUnit == 1)
                    {
                        enemyController = detectedEnemy[i].gameObject.GetComponent<UnitController>();
                        if (enemyController != null)
                        {
                            enemyController.ReceiveDamage(damage);
                        }

                        // Break out of looping through detected enemies after dealing damage
                        i += detectedEnemy.Length;

                        attackInitiated = true;
                    }
                }
            }
            // After not finding a valid target the unity will stop fighting
            if (!attackInitiated)
            {
                isFighting = false;
            }
            yield return new WaitForSeconds(0.8f);
            unitAnimator.SetBool("attackInitiated", true);
            yield return new WaitForSeconds(attackSpeed - 0.8f);
            unitAnimator.SetBool("attackInitiated", false);
        }
    }

    IEnumerator DeathAnimation()
    {
        /*
            isDying = true;
            boltRb.velocity = Vector3.zero;
            boltRb.isKinematic = true;
        */
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        unitAnimator.SetBool("attackInitiated", false);
        unitAnimator.SetBool("isRunning", false);
        unitAnimator.SetBool("isFighting", false);
        yield return new WaitForSeconds(0.5f);
        unitAnimator.SetBool("isDying", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
