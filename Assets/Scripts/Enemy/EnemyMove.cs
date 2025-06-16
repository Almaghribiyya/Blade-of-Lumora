using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float maxDistance; // Jarak maksimum sebelum musuh berbalik arah
    [SerializeField] public float attackRange;
    [SerializeField] public float retrieveDistance;
    [SerializeField] public float chaseSpeed;
    [SerializeField] public float attackRadius;
    [SerializeField] public int maxHealth;

    public bool Range = false;
    public Transform attackPoint;
    public Transform player;
    public bool movingLeft = true;
    private Vector3 startingPosition;
    public Animator animator;
    public LayerMask attackLayer;

    private GameManager gameManager; // Referensi GameManager
    private AudioManager audioManager; // Referensi AudioManager

    private void Start()
    {
        // Simpan referensi GameManager dan AudioManager
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        // Simpan posisi awal musuh
        startingPosition = transform.position;
    }

    private void Update()
    {
        // Jika game tidak aktif, berhenti
        if (!gameManager.GameActive) return;

        // Jika musuh mati
        if (maxHealth <= 0)
        {
            Die();
            return;
        }

        // Tentukan apakah player berada dalam attack range
        Range = Vector2.Distance(transform.position, player.position) <= attackRange;

        if (Range)
        {
            // Pastikan musuh menghadap ke player
            FacePlayer();

            // Check distance untuk memutuskan chase atau attack
            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                // Mode chase
                animator.SetBool("Attack", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                // Mode attack
                animator.SetBool("Attack", true);
            }
        }
        else
        {
            // Mode patrol
            Patrol();
        }
    }

    private void Patrol()
    {
        // Menentukan arah gerak berdasarkan movingLeft
        if (movingLeft)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }

        // Mengecek jarak dari posisi awal ke posisi saat ini
        if (Vector3.Distance(startingPosition, transform.position) >= maxDistance)
        {
            Flip();
        }
    }

    // Fungsi untuk membalik arah musuh
    private void Flip()
    {
        movingLeft = !movingLeft; // Mengubah arah gerak

        // Mengubah tampilan musuh untuk berbalik arah
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Membalik skala pada sumbu x
        transform.localScale = localScale;

        // Memperbarui posisi awal ke titik balik saat ini
        startingPosition = transform.position;
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<PlayerMove>() != null)
            {
                collInfo.gameObject.GetComponent<PlayerMove>().TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;

        maxHealth -= damage;

        animator.SetTrigger("Hurt");

        CameraShake.instance.Shake(.11f, 2f);
    }

    // Fungsi untuk memutar audio saat animasi Attack
    public void PlayEnemyAttackAudio()
    {
        audioManager.PlayAudioEnemySword();
    }

    public void PlayEnemyDeathAudio()
    {
        audioManager.PlayAudioEnemyDeath();
    }

    public void PlayEnemyHurtAudio()
    {
        audioManager.PlayAudioEnemyHurt();
    }

    // Function to make the enemy face the player
    private void FacePlayer()
    {
        if (player.position.x < transform.position.x && !movingLeft)
        {
            Flip();
        }
        else if (player.position.x > transform.position.x && movingLeft)
        {
            Flip();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        Destroy(gameObject, 1f);
    }
}
