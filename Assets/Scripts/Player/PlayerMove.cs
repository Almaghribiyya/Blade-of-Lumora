using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float jump;
    [SerializeField] public int maxHealth;
    [SerializeField] public float attackRadius;
    public Slider healthBar;
    public Text healthText;
    public Text keyText;

    private float horizontalInput;
    public Rigidbody2D body;
    public Animator animator;
    public bool grounded;
    public int currentKey = 0;
    private bool isDead;

    public Transform attackPoint;
    public LayerMask attackLayer;

    private AudioManager audioManager; // Referensi ke AudioManager
    private GameManager gameManager; // Referensi ke GameManager
    private SceneManagement sceneManager;

    private void Start()
    {
        // Inisialisasi health bar
        healthBar.maxValue = maxHealth;

        // Simpan referensi AudioManager dan GameManager
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();
        sceneManager = FindObjectOfType<SceneManagement>();
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Periksa jika player mati
        if (maxHealth <= 0)
        {
            Die();
            return;
        }
        

        // Perbarui health bar dan UI teks
        healthBar.value = maxHealth;
        healthText.text = maxHealth.ToString();
        keyText.text = currentKey.ToString();

        // Input horizontal dan gerakan player
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector2(1, 1); // Menghadap kanan
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-1, 1); // Menghadap kiri

        // Input untuk lompat
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
        
            Jump();
        }

        // Animasi berjalan
        if (Mathf.Abs(horizontalInput) > .1f)
            animator.SetFloat("Run", 1f);
        else
            animator.SetFloat("Run", 0f);

        // Input untuk menyerang
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(horizontalInput, 0f, 0f) * Time.deltaTime * speed;
    }

    private void Jump()
    {
        body.AddForce(new Vector2(body.velocity.x, jump), ForceMode2D.Impulse);
        grounded = false; // Set grounded ke false
        animator.SetBool("Jump", true);
        audioManager.PlayAudioPlayerJump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            grounded = true;
            animator.SetBool("Jump", false);
        }
        if (collision.gameObject.tag == "Water")
        {
            //Set collision water ngeluarin UI gameover juga
            if (!isDead)

            {
                Die();
            }
        }
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<EnemyMove>() != null)
            {
                collInfo.gameObject.GetComponent<EnemyMove>().TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;

        maxHealth -= damage;
        animator.SetTrigger("Hurt");

        CameraShake.instance.Shake(.12f, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Key")
        {
            currentKey++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            audioManager.PlayAudioCoinCollected();
            Destroy(other.gameObject, 1f);
        }

        if (other.gameObject.tag == "VictoryPoint")
        {
            if (currentKey >= 5)
            {
                // Jika jumlah kunci mencukupi, lanjut ke level berikutnya
                sceneManager.LoadLevel();
            }
        }
    }

    private void Die()
    {
    
        // Set parameter bool isDead
        animator.SetBool("isDead", true);

        // Kamera shake
        CameraShake.instance.Shake(1f, 1f);

        // Nonaktifkan game
        gameManager.GameActive = false;

        // Mulai Coroutine untuk menghancurkan player dan menampilkan tombol
        StartCoroutine(HandleDeathSequence());
    }

    private IEnumerator HandleDeathSequence()
    {
        // Tunggu hingga animasi selesai
        yield return new WaitForSeconds(2f); // Sesuaikan dengan durasi animasi Die

        // Hancurkan player
        Destroy(gameObject);

        // Tampilkan tombol restart
        FindObjectOfType<GameManager>().gameOver();
    }

    // Fungsi untuk dipanggil melalui Animation Event
    public void PlayPlayerAttackAudio()
    {
        audioManager.PlayAudioPlayerSword();
    }

    public void PlayPlayerDeathAudio()
    {
        audioManager.PlayAudioPlayerDeath();
    }

    public void PlayPlayerHurtAudio()
    {
        audioManager.PlayAudioPlayerHurt();
    }

    public void PlayAudioCoinCollected()
    {
        audioManager.PlayAudioCoinCollected();
    }
}
