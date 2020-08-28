using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Input
    public InputMaster Input
    {
        get { return m_InputMaster = m_InputMaster ?? new InputMaster(); }
    }

    private InputMaster m_InputMaster = null;

    #endregion

    #region PlayerBody (Rigidbody2D)

    private Rigidbody2D m_PlayerBody = null;

    private Rigidbody2D PlayerBody
    {
        get { return m_PlayerBody = m_PlayerBody ?? gameObject.GetComponent<Rigidbody2D>(); }
    }

    #endregion

    #region Animator

    private Animator m_Animator = null;

    private Animator Animator
    {
        get { return m_Animator = m_Animator ?? GetComponent<Animator>(); }
    }

    #endregion

    #region Player States

    //Whether or not the player has began playing yet. The game waits on them.
    private bool m_HasStartedPlaying = false;

    //Player rotation angle
    private bool m_AngleLocked = false;
    private float m_PlayerAngle = 0f;

    //Whether or not the Player has hit an obstacle and died
    private bool m_IsDead = false;
    private bool m_Jump = false;

    //We'll use this to store the Player's velocity when you pause the game so there's no inconsistencies with velocity after pausing
    private Vector2 pausedVelocity = Vector2.zero;

    #endregion

    //Callbacks for various game actions
    public event Action OnStartPlaying;
    public event Action<Collision2D> OnCollision;
    public event Action<Collider2D> OnClearPipe;

    private void OnEnable()
    {
        this.Input.Enable();
        this.Input.Player.Confirm.performed += JumpCallback;
        this.OnStartPlaying += OnGameStart;
    }

    private void OnDisable()
    {
        this.Input.Disable();
        this.Input.Player.Confirm.performed -= JumpCallback;
        this.OnStartPlaying -= OnGameStart;
    }

    // Update is called once per frame
    public void ManualUpdate()
    {

        //Don't start falling until the player starts the game by jumping
        if (!m_HasStartedPlaying)
        {
            this.PlayerBody.gravityScale = 0f;
            this.PlayerBody.velocity = Vector2.zero;
        }

        //Jump in response to the callback
        if (m_Jump)
        {
            if (!m_IsDead)
            {
                //Reset the velocity and then apply an impulse force upward
                this.PlayerBody.velocity = new Vector2(0, 0);
                this.PlayerBody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                AudioController.PlaySFX(AudioController.Sound.Flap);
            }

            m_Jump = false;
        }

        //Player is not allowed to move horizontally
        this.PlayerBody.velocity = new Vector2(0, this.PlayerBody.velocity.y);

        //The angle is locked once you hit the ground so the Player doesn't keep turning once it's landed
        //This behavior is meant to replicate the one seen in the OG game where the Player faces toward the ground when it's dead
        if (!m_AngleLocked)
        {
            //Angle the Player to face its velocity
            Vector2 v = this.PlayerBody.velocity;
            float tarAngle = (Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);

            //Tilt the Player when jumping up
            if (tarAngle == 90)
            {
                tarAngle -= 50;
            }

            //Interpolate between angles for smooth rotations
            m_PlayerAngle = Mathf.Lerp(m_PlayerAngle, tarAngle, Time.deltaTime * 10f);

            //Apply the rotation
            transform.rotation = Quaternion.AngleAxis(m_PlayerAngle, Vector3.forward);
        }

        //Stop animating Player once it dies
        if (m_IsDead)
        {
            this.Animator.speed = 0f;
        }
    }

    #region Gameplay Callbacks

    public void OnGameStart()
    {
        m_HasStartedPlaying = true;
        this.PlayerBody.gravityScale = 1f;
    }

    public void OnGamePause(bool isPaused)
    {

        if (isPaused)
        {
            pausedVelocity = PlayerBody.velocity;
            this.PlayerBody.velocity = Vector2.zero;
        }
        else
        {
            this.PlayerBody.velocity = pausedVelocity;
        }

        this.PlayerBody.isKinematic = isPaused;
        this.Animator.speed = isPaused ? 0f : 1f;
    }

    private void JumpCallback(InputAction.CallbackContext context)
    {

        //Detects if the mouse is hovering the UI. Everything connected to the EventSystem is detected.
        //The splash screen is excluded from this because the "raycast target" option is unchecked
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        //Check if the player is using the space key to jump, in which case it doesn't matter if youre hovering the pause button
        bool isUsingJumpKey = !context.action.activeControl.ToString().Split('/')[1].Contains("Mouse");


        if (!isPointerOverUI || isUsingJumpKey)
        {
            //Add flag for the jump
            m_Jump = true;

            if (m_HasStartedPlaying == false)
            {
                OnStartPlaying.Invoke();
            }
        }
    }


    //This function handles collisions with obstacles
    private void OnCollisionEnter2D(Collision2D collision)
    {

        this.OnCollision.Invoke(collision);

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

            //Allow the Player to fall through the bottom pipe like in the original game
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;

                if (g.tag.Contains("Pipe"))
                {
                    g.GetComponent<Collider2D>().enabled = false;
                }
            }

            //Kill the Player
            m_IsDead = true;
        }

        //Lock the Players angle when you hit the ground
        if (tag.Equals("Ground"))
        {
            m_AngleLocked = true;
        }
    }

    //This function handles collisions with trigger colliders
    void OnTriggerEnter2D(Collider2D collider)
    {
        this.OnClearPipe.Invoke(collider);
        AudioController.PlaySFX(AudioController.Sound.Point);
    }

    #endregion
}
