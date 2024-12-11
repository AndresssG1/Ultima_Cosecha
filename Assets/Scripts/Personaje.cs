using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float velocidadMovimiento;
    public float fuerzaSalto; 
    public GameManager gameManager; 

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private Animator cofreAnimator; 
    private bool enSuelo = false; 
    

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation; 
        rigidbody2d.gravityScale = 2f; 
        rigidbody2d.mass = 1f;
    }

   void Update()
    {
        if (gameManager.menuPrincipal.activeSelf || gameManager.menuGameOver.activeSelf || gameManager.menuFinal.activeSelf)
        {
            animator.SetBool("EstaQuieto", true); 
            return; 
        }

        Vector3 nuevaPosicion = transform.position;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            nuevaPosicion += Vector3.right * velocidadMovimiento * Time.deltaTime;
            animator.SetBool("EstaQuieto", false);
            transform.localScale = new Vector3(4, 4, 1);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            nuevaPosicion += Vector3.left * velocidadMovimiento * Time.deltaTime;
            animator.SetBool("EstaQuieto", false);
            transform.localScale = new Vector3(-4, 4, 1);
        }
        else
        {
            animator.SetBool("EstaQuieto", true);
        }

        nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, -2, 2000);
        transform.position = nuevaPosicion;

        if (transform.position.y < -5)
        {
            gameManager.vidas = 0; 
            gameManager.ActualizarUI();
            gameManager.gameOver = true;
            rigidbody2d.linearVelocity = Vector2.zero;
        }

        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2d.AddForce(new Vector2(0, fuerzaSalto));
            enSuelo = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("Obstaculo") || collision.gameObject.CompareTag("Cofre"))
        {
            enSuelo = true;
        }
        
        if (collision.gameObject.CompareTag("Cofre"))
        {
            cofreAnimator = collision.gameObject.GetComponent<Animator>();

            if (cofreAnimator != null)
            {
                cofreAnimator.SetBool("Premio", true);
                gameManager.granos++;
                gameManager.ActualizarUI();
            }
        }

        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            gameManager.vidas--;
            gameManager.ActualizarUI();

            if (gameManager.vidas <= 0)
            {
                gameManager.gameOver = true;
            }
        }

        if (collision.gameObject.CompareTag("Final") && 4 <= gameManager.granos)
        {
            gameManager.final = true;
        }

        
    }
}
