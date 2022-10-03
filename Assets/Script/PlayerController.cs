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
    [SerializeField] private float jumpForce;
    
    private int jumpDie; //Salto cuando se muere

    [SerializeField] private float runningSpeed;
    
    /*
     * Esta variable la usamos con el RayCast para
     * poder modificar la longitud del RayCast.
     */
    [SerializeField] private float distanciaDelSuelo;
    
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

    private Vector3 StartPosition; // Se utiliza para guardar la posicin inicial de cuando se da play.
    
    private void Awake()
    {
        /* GetComponent busca otra componente del mismo gameObjet. En este caso, el GameObjet es nuestro player(FOX)
         * En este caso, buscamos el componente RigidBody2D dentro del gameObjet de nombre Player.
         * Esto permite que podamos usar la variable rigidBody dentro del codigo.
         */
        rigidBody = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
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

        /*
         * Almacenamso la posicion en la que espawnea el personaje apenas inicia el juego. Esta la utilizaremos para
         * cuando queramos volver a jugar luego de haber muerto.
         */
        StartPosition = this.transform.position;
    }

    public void StartGame()
    {
        /*
         * Hacemos que el player spawne en el punto de inicio, el punto de cuando le dimos play al juego. 
         */
        this.transform.position = StartPosition;
        this.rigidBody.velocity = Vector2.zero; //Frenamos la velosidad del Player.
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
        Debug.DrawRay(this.transform.position, Vector2.down * distanciaDelSuelo, Color.blue);  
        
        /*
         * Setiamos la animacion segun lo que indique el metodo que creamos.
         */
        animator.SetBool(STATE_ON_THE_GROUND, IsTouchinTheGround());
    }

    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Jump(); 
            }
            if (rigidBody.velocity.x < runningSpeed)
            {
                rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y); //Seteo de la velocidad para que aumente hasat la velocidad runigSpeed.
            }
        }
        if (GameManager.sharedInstance.currentGameState == GameState.menu)
        {
           // rigidBody.velocity = new Vector2(0, 0);
           rigidBody.Sleep();
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
            return true; 
        }
        else
        {
             return false;
        }
    }

    public void Die()
    {
        this.animator.SetBool(STATE_ALIVE, false); // cambia el estado bool de isAlive a false
        GameManager.sharedInstance.GameOver(); // le indica al GameManager que tiene que entrar en estado GameOver
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y); // Desaselera lateralmente (en el eje de las x)
        if (jumpDie == 0) // Verifica si ya habia dado el salto
        {
            jumpDie = 1;
            rigidBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse); // Realiz el salto.
        }
    }
}