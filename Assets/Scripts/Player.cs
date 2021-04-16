using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IBulletTarget
{
    [SerializeField] float speed;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject damageSprite;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject backend;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject laser;
    [SerializeField] public float timeToKeepLaserEnabled = 5f;
    [SerializeField] public float timeToKeepShieldEnabled = 10f;

    private bool isRunning;
    private Vector3 change;
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private bool shieldIsEnabled = false;
    private bool laserIsEnabled = false;
    private bool laserIsAvailable = false;
    private float counterTimeEnabledLaser = 0f;
    private float counterTimeEnabledShield = 0f;

    private void Awake()
    {
        isRunning = false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isRunning)
        {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            //change.y = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Jump"))
            {
                if (laserIsAvailable)
                {
                    laserIsAvailable = false;
                    EnableLaser();
                }
                else
                {
                    Attack();
                }
            }

            UpdateAnimationAndMove();
        }

        if (counterTimeEnabledShield > 0)
        {
            counterTimeEnabledShield = Mathf.Max(0, counterTimeEnabledShield - Time.deltaTime);
            if (counterTimeEnabledShield <= 0)
            {
                DisableShield();
            }
            else if (counterTimeEnabledShield <= timeToKeepShieldEnabled / 3)
            {
                ToggleeShield();
            }
        }

        if (counterTimeEnabledLaser > 0)
        {
            counterTimeEnabledLaser = Mathf.Max(0, counterTimeEnabledLaser - Time.deltaTime);
            if (counterTimeEnabledLaser <= 0)
            {
                DisableLaser();
            }
            else if (counterTimeEnabledLaser <= timeToKeepLaserEnabled / 3)
            {
                ToggleLaser();
            }
        }

        shield.SetActive(shieldIsEnabled);
        laser.SetActive(laserIsEnabled);
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        rigidbody2d.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    public void TakeDamage(int damage, Vector3 position)
    {
        if (backend == null || laserIsEnabled)
        {
            return;
        }

        var gameScore = backend.GetComponent<GameScore>();
        gameScore.HitDamage(damage);

        if (gameScore.PlayerHealthIsZero())
        {
            GameOver();
        }
        else
        {
            StartCoroutine(DamageCo(position));
        }
    }

    IEnumerator DamageCo(Vector3 position)
    {
        var damageObj = Instantiate(damageSprite, position, transform.rotation);

        yield return null;

        yield return new WaitForSeconds(.5f);
        Destroy(damageObj);
    }

    void GameOver()
    {
        var gameScore = backend.GetComponent<GameScore>();
        gameScore.GameOver();
    }

    void Attack()
    {
        var bulletObj = Instantiate(bullet, firePoint.position, firePoint.rotation);
        var rb = bulletObj.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = new Vector3(0, bulletSpeed, 0);
    }

    public void EnablePlayer()
    {
        isRunning = true;
    }

    public void DisablePlayer()
    {
        isRunning = false;
    }

    public void EnableShield()
    {
        counterTimeEnabledShield = timeToKeepShieldEnabled;
        shieldIsEnabled = true;
    }

    public void DisableShield()
    {
        counterTimeEnabledShield = 0;
        shieldIsEnabled = false;
    }

    public void ToggleeShield()
    {
        shieldIsEnabled = !shieldIsEnabled;
    }

    public void MakeLaserAvailable()
    {
        laserIsAvailable = true;
    }

    public void EnableLaser()
    {
        counterTimeEnabledLaser = timeToKeepLaserEnabled;
        laserIsEnabled = true;
    }

    public void DisableLaser()
    {
        laserIsEnabled = false;
    }

    public void ToggleLaser()
    {
        laserIsEnabled = !laserIsEnabled;
    }

    public bool HasShieldEnabled()
    {
        return counterTimeEnabledShield > 0;
    }

}
