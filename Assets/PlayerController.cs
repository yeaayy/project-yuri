using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float rn_speed, ro_speed;
    public Transform playerTrans;
    public float dashSpeed;
    public float dashTime;
    private bool isDashing = false;

    void FixedUpdate()
    {
        if (!isDashing)
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerRigid.velocity = transform.forward * rn_speed * Time.deltaTime;
            }
            else
            {
                playerRigid.velocity = Vector3.zero;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("Running");
            playerAnim.ResetTrigger("Idle");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("Running");
            playerAnim.SetTrigger("Idle");
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        playerRigid.velocity = transform.forward * dashSpeed;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
}