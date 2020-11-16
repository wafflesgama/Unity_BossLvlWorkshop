using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMaster : MonoBehaviour
{
    public MovementController mov;
    public AttackingController att;

    [Header("Animator")]
    public Animator anim;
    public string xVectorName;
    public string yVectorName;
    public string jumpName;
    public string[] attackNames;
    public string attackConditionName;
    [Space(30)]
    public float smoothLerp = .1f;
    public float smoothAttackLerp = .1f;

    float xVectorBlend;
    float yVectorBlend;

    float unBlendedMoveSpeed;
    void Start()
    {
        xVectorBlend = 0;
        yVectorBlend = 0;
        mov.attackMoveSpeedAnim = 1;
        unBlendedMoveSpeed = 1;
    }

    void Update()
    {
        //xVectorBlend = Mathf.Lerp(xVectorBlend, -mov.directionVector.z / 2 + (mov.isSprinting ? .5f : 0), smoothLerp * Time.deltaTime);

        var absDirX = Mathf.Abs(mov.directionVector.x);
        var absDirZ = Mathf.Abs(mov.directionVector.z);
        yVectorBlend = Mathf.Lerp(yVectorBlend, (absDirX > absDirZ ? absDirX : absDirZ) / 2 + (mov.isSprinting ? .5f : 0), smoothLerp * Time.deltaTime);

        //Debug.Log("Direction Vector:" + xVectorBlend);
        //anim.SetFloat(xVectorName, xVectorBlend);
        anim.SetFloat(yVectorName, yVectorBlend);

        mov.attackMoveSpeedAnim = Mathf.Lerp(mov.attackMoveSpeedAnim, unBlendedMoveSpeed, smoothAttackLerp * Time.deltaTime);

        anim.SetBool(attackConditionName, att.attack);

    }
    public void StartJumpAnim()
    {
        anim.SetTrigger(jumpName);
    }
    public void EndJump()
    {
        mov.isJumping = false;
        mov.moveSpeed *= mov.jumpMoveSpeedCut;
        mov.rotationSmooth /= mov.jumpRotationCut;
    }

    public void Attack(int attackNum)
    {
        anim.SetTrigger(attackNames[attackNum - 1]);
        if (attackNum == 1)
        {
        mov.moveSpeed /= mov.attackSpeedCut;
        mov.rotationSmooth *= mov.attackRotationCut;
        }
    }

    public void EndAttack()
    {
        att.isAttacking = false;

        if (!att.attack)
        {
            att.attackOrder = 0;
            unBlendedMoveSpeed = 1;
            mov.moveSpeed *= mov.attackSpeedCut;
            mov.rotationSmooth /= mov.attackRotationCut;
        }
    }
    public void SetMoveSpeed(float speed)
    {
        unBlendedMoveSpeed = speed;
    }
}
