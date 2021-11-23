using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEngine;
#endif


[RequireComponent(typeof(Game_UI_Handler))]
public class Game_Manager : MonoBehaviour {

    public static Game_Manager Instance  { get; private set; }
    public Game_UI_Handler _gameUIHandler { get; private set; }

    [Header( "Refrences" )]
    [SerializeField] GameObject _titleScreen;
    [SerializeField] GameObject _settingsScreen;
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] GameObject _gameWonScreen;


    [Header( "Knife Spawning" )]
    [SerializeField] Vector2 _knifeSpawnPosition;
    [SerializeField] GameObject _knifePrefab;

    [HideInInspector]
    public int _knifeCount;
    [HideInInspector]
    public int _gameLevel = 1;
    [HideInInspector]
    public bool _isGameOver;
    [HideInInspector]
    public bool _inGame;
    private bool _isMuted;
    private AudioSource _gameAudio;

    private void Awake() {
        
        // ★彡[ Declaring this object instance ]彡★
        Instance = this;
        // ★彡[ Getting Game UI Handler script component ]彡★
        _gameUIHandler = GetComponent<Game_UI_Handler> ();
        // ★彡[ Getting Audio Source component ]彡★
        _gameAudio = GetComponent<AudioSource> ();
    }

    private void Start() {
        
        // ★彡[ Getting random number of kinves count when the game starts ]彡★
        // _knifeCount = UnityEngine.Random.Range( 5, 10 );
    }

    public void OnSuccessfulKnifeHit() {

        // ★彡[ Checking if knife count is not zero yet to spawn more knives ]彡★
        if ( _knifeCount > 0 ) {

            SpawnKnife();
        }
        else {

            // ★彡[ Other wise displaying game over Screen and making all the movement false ]彡★
            GameWon();
            // _gameLevel++;
            // LevelSystem();
        }
    }

    private void SpawnKnife() {

        // ★彡[ Decreasing the number of knives ]彡★
        _knifeCount--;
        // ★彡[ Instantiating knife prefab at the knife spawn position ]彡★
        Instantiate( _knifePrefab, _knifeSpawnPosition, quaternion.identity );
    }


    public void RestartGame() {

        // ★彡[ Reloading the scene if restart button is clicked ]彡★
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    public void ExitTheApplication() {

        // ★彡[ Making the game exit on pressing exit button ( checking if it is being played in unity engine or as an app ) ]彡★
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }

    public void VolumeMute() {

        if ( !_isMuted ) {

            _gameAudio.Stop();
            _isMuted = true;
        } else {

            _gameAudio.Play();
            _isMuted = false;
        }
    }

    public void StartGame( int knifeCount ) {

        _isGameOver = false;
        _inGame = true;
        _knifeCount = knifeCount;

        _titleScreen.SetActive( false );
        // ★彡[ Spawning knives when games starts ]彡★
        SpawnKnife();
        // ★彡[ setting initial knife count to random knife count that we got in the start function ]彡★
        _gameUIHandler.SetInitialKnifeCount( _knifeCount );

        // ★彡[ checking if the settings screen is active in the heirarchy to disable it inorder to let user play the game ]彡★
        if( _settingsScreen.activeInHierarchy == true )
            _settingsScreen.SetActive( false );
    }

    public void SettingsMenu() {

        // ★彡[ setting title menu to false and displaying settings menu when settings button is clicked ]彡★
        if ( !_inGame )
        _titleScreen.SetActive( false );
        _settingsScreen.SetActive( true );
    }

    public void BackButton() {

        // ★彡[ deactivating settings button when back button is pressed ]彡★
        _settingsScreen.SetActive( false );

        // ★彡[ checking if not in game to enable the title screen back ]彡★
        if ( !_inGame )
            _titleScreen.SetActive( true );
    }

    public void GameOver() {

        // ★彡[ setting the game over bool to true and displaying game over screen ]彡★
        _isGameOver = true;
        _gameOverScreen.SetActive( true );

    }

    public void GameWon() {

        // ★彡[ setting the game over bool to true and displaying game Won screen ]彡★
        _isGameOver = true;
        _gameWonScreen.SetActive( true );

    }

    public void LevelSystem() {

        if( _gameLevel == 2 ) {

            StartGame( 8 );
        }
        if( _gameLevel == 3 ) {

            StartGame( 10 );
        }
        if( _gameLevel == 4 ) {

            StartGame( 12 );
        }
        if( _gameLevel == 5 ) {

            StartGame( 15 );
        }
    }
}
