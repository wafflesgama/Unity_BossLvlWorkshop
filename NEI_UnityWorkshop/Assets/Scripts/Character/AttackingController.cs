using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingController : MonoBehaviour
{
    public AnimatorMaster animMaster;
    public MovementController mov;

    public float waitToDisableAttackTime = 1.5f;
    //[HideInInspector]
    public bool attack;
    //[HideInInspector]
    public bool isAttacking;
    //[HideInInspector]
    public int attackOrder;

    void Start()
    {
        isAttacking = false;
        attackOrder = 0;
    }

    void Update()
    {
        if (mov.canAct && Input.GetMouseButtonDown(1))
        {
            if (attackOrder < 3 && ((isAttacking && attackOrder > 0) || (!isAttacking && attackOrder == 0)))
                attack = true;

        }
        if (!isAttacking && attack)
        {
            attack = false;
            isAttacking = true;
            attackOrder++;
            animMaster.Attack(attackOrder);
        }
    }
}
