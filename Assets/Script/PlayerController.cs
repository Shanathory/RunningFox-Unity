using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables del movimiento del personaje
    
    /*
     * Creamos una variable de tipo flotante para
     * poder modificar la fuerza del salto y otra
     * para la velcidad de Runnig.
     */
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float runningSpeed = 2; 
    /*
     * Esta variable la usamos con el RayCast para
     * poder modificar la longitud del RayCast.
     */
    [SerializeField] private float distanciaDelSuelo = 0.49f;
    
    /*
     * Creamos una variable del tipo RigidBody2D para poder modificar
     * o utilizar las fisicas del RigidBody2D. Responde a acciones
     * relacionadas con la fisica.
     */
    private Rigidbody2D rigidBody;
    /*
     * La variable animator del tipo Animator funciona
     * como la variable del tipo RigdBody2D pero para
     * animaciones.
     */
    private Animator animator;
    /*
     * Creamos la variable groundMask del tipo LayerMask
     * para poder agregar un Layer ya creado o que hayamos 
     * creado en unity. A este tipo de variables, se las
     * conoce como variables de tipo objetos.
     */
    public LayerMask groundMask;
    /*
     * Creamos la variable sfxPlayer para poder agregar el sfx que produsca el jugador.
     */
    private AudioSource sfxPlayer;
    [SerializeField] private AudioClip sfxJump;
    
    /*
     * Por ahora nuestro personaje tiene la animación de correr en
     * cada momento, incluso cuando esta saltando. En esta clase
     * vamos a sincronizar las animaciones de nuestro personaje con
     * respecto a las modificaciones al estado del Animation Controller.
     * Para conseguir estos cambios de animación vamos a declarar las variables
     * STATE_ALIVE y STATE_ON_THE_GROUND para actualizarlas desde el método Update cada
     * vez que el método isTouchingTheGround nos indique que el personaje ha
     * saltado o ha terminado de saltar. Cuando nuestro personaje NO esta en
     * contacto con el suelo vamos a pausar la animación de correr, así podemos
     * darle a los usuarios un mejor feedback de que el personaje esta saltando correctamente.
     */
    /*
     * Variable constante que nunca va a cambiar.
     * La razon de que esten en mayuscula es porque es una
     * constante.
     */
    private const string STATE_ALIVE = "isAlive"; 
    /*
     * Estas variables siempre van a baler (en este caso)
     * isOnTheGround que lo que configuramos en el animator.
     * Esto es asi, porque son variables constantes.
     */
    private const string STATE_ON_THE_GROUND = "isOnTheGround";

    private int conteoJump;
    
    private void Awake()
    {
        /* GetComponent busca otra componente del mismo gameObjet. En este caso, el GameObjet es nuestro player(FOX)
         * En este caso, buscamos el componente RigidBody2D dentro del gameObjet de nombre Player.
         * Esto permite que podamos usar la variable rigidBody dentro del codigo.
         */
        rigidBody = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        sfxPlayer = GetComponent<AudioSource>();
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        /*
         * Le Pedimos al animator que configure nuestra variable
         * booleana STATE_ALIVE como verdadera al iniciar el juego.
         * Lo mismo sucede con STATE_ON_THE_GROUND.
         */
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);
        conteoJump = 0;
    }

    private void Update()
    {
        /*
         * Configuramos el rayo. Lo colocamos en el
         * Update, porque sino, se ve afectado por lafisica
         * y se desplaza con un pequeño destiempo con respecto
         * al character.
         * A este lo colocamos para hacer visible Phisics2D.RayCast
         */
        Debug.DrawRay(this.transform.position, Vector2.down * distanciaDelSuelo, Color.red);  
        /*
         * Setiamos la animacion segun lo que indique el metodo que creamos.
         */
        animator.SetBool(STATE_ON_THE_GROUND, IsTouchinTheGround());
    }

    private void FixedUpdate()
    {
        if (rigidBody.velocity.x < runningSpeed)
        {
            rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            if (conteoJump == 0)
            {
                /*
                 * Agregamos el sonido del salto del jugador. 1.0f indica que va a sonoar al 100% del volumen.
                 */
                sfxPlayer.PlayOneShot(sfxJump, 1.0f);
                Jump();
                conteoJump = 1;
            }
        }
    }
    
    /*
     * Los metodos comienzan con mayusculas. Las funciones van con minusculas.
     */
    void Jump()
    {
        if (IsTouchinTheGround()) // Si IsTouchinTheGround es true, salta.
        { 
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Agregamos un impulso hacia arriba
        }
    }
    
    /*
     * Creamos un metodo booleno para saber si el player esta tocando el suelo.
     */
    bool IsTouchinTheGround()
    {
        /*
         * Lanza un rayo hacia el suelo para poder medir la distancia del le personaje y el suelo.
         */
        if (Physics2D.Raycast(this.transform.position, Vector2.down, distanciaDelSuelo, groundMask))
        {
            GameManager.sharedInstance.currentGameState = GameState.inGame;
            conteoJump = 0;
            return true; 
        }
        else
        {
            return false;
        }
    }
}