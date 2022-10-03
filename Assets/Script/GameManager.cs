using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Creamos un ENUMERADOR
 * Los enumerados se utilizan para poder almacenar el
 * estado del juego.
 *
 * 1. Estar dentro del menu.
 * 2. Estar dentro de la partida
 * 3. Estar muerto
 *
 * Este se coloca por fuera de la clase, para poder
 * acceder a el desde otros scripts sin problemas.
 */
public enum GameState
{
    menu,
    inGame,
    gameOver
}


public class GameManager : MonoBehaviour
{
    /*
     * Creamos una variable publica del tipo GameState,
     * para definir en que estado se comenzara el juego.
     */
    public GameState currentGameState = GameState.menu;

    /*
     * Creamso un singleton de instancia compartida (sharedInstance) 
     */
    public static GameManager sharedInstance;

    private void Awake()
    {
        /*
         * Consultamso si la variable sharedInstan ya fue asignada por otro script,
         * en caso de que no haya sido asiganda a otro script, lo que hacemos es asignarlo
         * a este (this) script. De esta manera evitamos posibles  errores.
         */
        if(sharedInstance == null)
        {
            /*
             * Inicializamos nuestro singleton.
             */
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pausa") && currentGameState == GameState.inGame)
        {
            BackToMenu();
        }
        else if (Input.GetButtonDown("Pausa") && currentGameState == GameState.menu)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }
    
    /*
     * Creamos un metodo privado, para que solamente el
     * Script GameManager se el que lo pueda modificar.
     * Este metodo lo vamos a usar para cambiar el
     * estado del juego.
     */

    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            //TODO: Abrir el menu.
        }
        else if (newGameState == GameState.inGame)
        {
            //TODO: Iniciar el juego.
        }
        else if (newGameState == GameState.gameOver)
        {
            //TODO: GameOver.
        }

        /*
         * Enfatisamos en que currentGameState es una variable
         * del propio GameManager y que esta resibe del parametro
         * newGameState que preparamos.
         */
        this.currentGameState = newGameState;
    }
}
