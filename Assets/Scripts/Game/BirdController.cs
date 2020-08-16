using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{

    #region Input
    private InputMaster Input
    {
        get { return m_InputMaster = m_InputMaster ?? new InputMaster(); }
    }

    private InputMaster m_InputMaster = null;

    #endregion

    #region BirdBody (Rigidbody2D)

    private Rigidbody2D m_BirdBody = null;

    private Rigidbody2D BirdBody
    {
        get { return m_BirdBody = m_BirdBody ?? gameObject.GetComponent<Rigidbody2D>(); }
    }

    #endregion

    #region Animator

    private Animator m_Animator = null;

    private Animator Animator
    {
        get { return m_Animator = m_Animator ?? GetComponent<Animator>(); }
    }

    #endregion

    //Bird rotation angle
    private bool m_AngleLocked = false;
    private float m_BirdAngle = 0f;

    //Whether or not the bird has hit an obstacle and died
    private bool m_IsDead = false;
    private bool m_Jump = false;

    //We'll use this to store the bird's velocity when you pause the game so there's no inconsistencies with velocity after pausing
    private Vector2 pausedVelocity = Vector2.zero;

    //Callbacks for various game actions
    public event Action<Collision2D> OnCollision;
    public event Action<Collider2D> OnClearPipe;

    private void OnEnable()
    {
        Input.Enable();
        Input.Player.Confirm.performed += JumpCallback;
    }

    private void OnDisable()
    {
        this.Input.Player.Confirm.performed -= JumpCallback;
        this.Input.Disable();
    }

    // Update is called once per frame
    public void ManualUpdate()
    {
        //Jump in response to the callback
        if (m_Jump)
        {
            if (!m_IsDead)
            {
                //Reset the velocity and then apply an impulse force upward
                BirdBody.velocity = new Vector2(0, 0);
                BirdBody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                AudioController.PlaySFX(AudioController.Sound.Flap);
            }

            m_Jump = false;
        }

        //Bird is allowed to move horizontally
        BirdBody.velocity = new Vector2(0, BirdBody.velocity.y);

        //The angle is locked once you hit the ground so the bird doesn't keep turning once it's landed
        //This behavior is meant to replicate the one seen in the OG game where the bird faces toward the ground when it's dead
        if (!m_AngleLocked)
        {
            //Angle the bird to face its velocity
            Vector2 v = BirdBody.velocity;
            float tarAngle = (Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);

            //Tilt the bird when jumping up
            if (tarAngle == 90)
            {
                tarAngle -= 50;
            }

            //Interpolate between angles for smooth rotations
            m_BirdAngle = Mathf.Lerp(m_BirdAngle, tarAngle, Time.deltaTime * 10f);

            //Apply the rotation
            transform.rotation = Quaternion.AngleAxis(m_BirdAngle, Vector3.forward);
        }

        if (m_IsDead)
        {
            Animator.speed = 0f;
        }
    }

    #region Callbacks

    public void OnGamePause(bool isPaused)
    {

        if (isPaused)
        {
            pausedVelocity = BirdBody.velocity;
            BirdBody.velocity = Vector2.zero;
        }
        else
        {
            BirdBody.velocity = pausedVelocity;
        }

        BirdBody.isKinematic = isPaused;
        Animator.speed = isPaused ? 0f : 1f;
    }

    private void JumpCallback(InputAction.CallbackContext context)
    {
        //If the player's mouse is hovering over the pause button, and they click, we don't want the bird to also jump
        //We also check if the player is using the space key to jump, in which case it doesn't matter if youre hovering the pause button
        bool isHoveringUIElement = (EventSystem.current.currentSelectedGameObject != null);
        bool isAttemptingToJumpWithMouse = context.action.activeControl.ToString().Split('/')[1].Contains("Mouse");

        if (!isHoveringUIElement || !isAttemptingToJumpWithMouse)
        {
            m_Jump = true;
            Debug.Log("Jump");
        }
    }

    //This function handles collisions with obstacles
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        OnCollision.Invoke(collision);

        string tag = collision.collider.tag;

        if (!m_IsDead)
        {
            //Default smash sound for colliding into something
            AudioController.PlaySFX(AudioController.Sound.Hit);

            //Play falling sound if you hit a pipe
            if (tag.Contains("Pipe"))
            {
                AudioController.PlaySFX(AudioController.Sound.Fall);
            }

            //Allow the bird to fall through the bottom pipe like in the original game
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;

                if (g.tag.Contains("Pipe"))
                {
                    g.GetComponent<Collider2D>().enabled = false;
                }
            }

            //Kill the bird
            m_IsDead = true;
        }

        //Lock the birds angle when you hit the ground
        if (tag.Equals("Ground"))
        {
            m_AngleLocked = true;
        }
    }

    //This function handles collisions with trigger colliders
    void OnTriggerEnter2D(Collider2D collider)
    {
        OnClearPipe.Invoke(collider);
        AudioController.PlaySFX(AudioController.Sound.Point);
    }

    #endregion
}
