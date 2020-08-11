using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
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

    [SerializeField] private Rigidbody2D m_RigidBody = null;
    private Animator m_Animator = null;

    //Bird rotation angle
    private bool m_AngleLocked = false;
    private float m_BirdAngle = 0f;

    //Whether or not the bird has hit an obstacle and died
    private bool m_IsDead = false;

    //Callbacks for various game actions
    public event Action<Collision2D> OnCollision;
    public event Action<Collider2D> OnClearPipe;

    private void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        this.Input.Enable();
        this.Input.Player.Confirm.performed += Jump;
    }

    private void OnDisable()
    {
        this.Input.Player.Confirm.performed -= Jump;
        this.Input.Disable();
    }

    // Update is called once per frame
    public void ManualUpdate()
    {
        //Bird is allowed to move horizontally
        m_RigidBody.velocity = new Vector2(0, m_RigidBody.velocity.y);

        //The angle is locked once you hit the ground so the bird doesn't keep turning once it's landed
        //This behavior is meant to replicate the one seen in the OG game where the bird faces toward the ground when it's dead
        if (!m_AngleLocked)
        {
            //Angle the bird to face its velocity
            Vector2 v = m_RigidBody.velocity;
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
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!m_IsDead)
        {
            //Reset the velocity and then apply an impulse force upward
            m_RigidBody.velocity = new Vector2(0, 0);
            m_RigidBody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            AudioManager.PlaySFX(AudioManager.Sound.Flap);
        }
    }

    //This function handles collisions with obstacles
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (OnCollision != null)
        {
            OnCollision.Invoke(collision);
        }

        string tag = collision.collider.tag;

        if (!m_IsDead)
        {
            m_Animator.speed = 0f;

            //Default smash sound for colliding into something
            AudioManager.PlaySFX(AudioManager.Sound.Hit);

            //Play falling sound if you hit a pipe
            if (tag.Contains("Pipe"))
            {
                AudioManager.PlaySFX(AudioManager.Sound.Fall);
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
            //Debug.Log("Lock");
        }
    }

    //This function handles collisions with trigger colliders
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (OnClearPipe != null)
        {
            OnClearPipe.Invoke(collider);
        }

        AudioManager.PlaySFX(AudioManager.Sound.Point);
    }
}
