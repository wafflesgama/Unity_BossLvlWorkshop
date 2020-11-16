using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool canAct = true;
    public bool canMove = true;

    public Rigidbody body;
    public Transform activeCamera;
    public AnimatorMaster animMaster;
    public AttackingController att;

    public float moveSpeed = 6;
    public float sprintMoveFactor = 1.5f;
    public float sprintRotationFactor = 1.1f;

    public float rotationSmooth = 0.1f;

    public float jumpForce;
    public float jumpIntensity;
    public float jumpMoveSpeedCut = 2;
    public float jumpRollSpeedCut = 1.3f;
    public float jumpRotationCut = 2.1f;

    public float attackSpeedCut = 2;
    public float attackRotationCut = 2;
    public float attackMoveSpeedAnim = 1;

    [HideInInspector]
    public Vector3 directionVector;
    [HideInInspector]
    public bool isSprinting;
    [HideInInspector]
    float rotationVelocity;

    [HideInInspector]
    public bool isJumping = false;

    Vector3 jumpDirection;
    void Start()
    {
        directionVector = Vector3.zero;
        jumpDirection = Vector3.zero;
        isSprinting = false;
        isJumping = false;
    }

    void Update()
    {
        if (!canAct) return;

        if (canMove)
        {

            #region Horizontal Key Detection


            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                directionVector.z = 1;

            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                directionVector.z = -1;

            else
                directionVector.z = 0;


            #endregion

            #region Vertical Key Detection

            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                directionVector.x = -1;

            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                directionVector.x = 1;

            else
                directionVector.x = 0;

            #endregion

            isSprinting = Input.GetKey(KeyCode.LeftShift);


            if (directionVector.magnitude > 0.1f)
            {

                float angle = Mathf.Atan2(-directionVector.z, -directionVector.x) * Mathf.Rad2Deg + activeCamera.eulerAngles.y;
                //float angle = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg + activeCamera.eulerAngles.y;
                float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref rotationVelocity, rotationSmooth * (isSprinting ? 1 / sprintRotationFactor : 1));
                transform.rotation = Quaternion.Euler(0, dampedAngle, 0);

                Vector3 moveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                if (!att.isAttacking)
                    body.MovePosition(transform.position + moveDir.normalized * moveSpeed * (isSprinting ? sprintMoveFactor : 1) * Time.deltaTime);
                else
                    body.MovePosition(transform.position + moveDir.normalized * moveSpeed * attackMoveSpeedAnim  * Time.deltaTime);

                //body.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime);
                //charCtr.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
                //charCtr.Move(activeCamera.forward * directionVector* moveSpeed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !att.isAttacking)
            {
                float angle = Mathf.Atan2(-directionVector.z, -directionVector.x) * Mathf.Rad2Deg + activeCamera.eulerAngles.y;
                jumpDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;

                isJumping = true;
                moveSpeed /= jumpMoveSpeedCut;
                rotationSmooth *= jumpRotationCut;
                body.AddForce(new Vector3(jumpDirection.normalized.x, jumpForce, jumpDirection.normalized.z) * jumpIntensity, ForceMode.Impulse);
                animMaster.StartJumpAnim();

            }
        }

        if (isJumping)
        {
            body.AddForce(-jumpDirection.normalized * jumpRollSpeedCut, ForceMode.Acceleration);
        }
    }


}

