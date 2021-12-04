using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;

public class Knife_Behaviour_Handler : MonoBehaviour {

    [SerializeField] Vector2 _shootForce;

    private bool _canAccessKnife = true;
    private Rigidbody2D _knifeRigidbody;
    private AudioSource _audio;
    public AudioClip _hitAudio;
    public AudioClip _missAudio;
    private int _hasCollidedwithKnives;
    private int _initialKnifeCount;

    private void Awake() {
        
        // ★彡[ Getting refrence to the Rigidbody 2d component ]彡★
        _knifeRigidbody = GetComponent<Rigidbody2D> ();
        _audio = GetComponent<AudioSource> ();
    }

    private void Start() {
        
        _initialKnifeCount = Game_Manager.Instance._knifeCount;
    }

    private void Update() {
        
        KnifeMovement();
    }

    private void KnifeMovement() {

        // ★彡[ Condition to check and get Input method and if knife is still accessible ]彡★
        if ( Input.GetMouseButtonDown(0) && _canAccessKnife && !Game_Manager.Instance._isGameOver ) {
            
            // ★彡[ Adding force to knife rigidbody in order to make it move ]彡★
            _knifeRigidbody.AddForce( _shootForce, ForceMode2D.Impulse );
            // ★彡[ Setting the gravity scale back to normal to let the knife get affected by gravity ]彡★
            _knifeRigidbody.gravityScale = 1;

            if( Game_Manager.Instance._knifeCount > 0 ) {

                // ★彡[ Decrementing the knife count ]彡★
                Game_Manager.Instance._gameUIHandler.DecreamentKnifeCount();
            }
        }
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        
        // ★彡[ Condition to check we can still access the knife ]彡★
        if ( !_canAccessKnife ) {

            // ★彡[ Returning null value if we can't access the knife ]彡★
            return;
        }

        // ★彡[ Making the can access knife to false to not to apply any kind of changes to that particular knife anymore ]彡★
        _canAccessKnife = false;

        // ★彡[ Checking if the collided game object has the specific tag to execute below code ]彡★
        if ( other.collider.tag == "Log" ) {

            StopMovement();
            // ★彡[ Setting other collided gameObject as parent of knife in order to make knife rotate along with its parent ]彡★
            this.transform.SetParent( other.collider.transform );
            // ★彡[ Spawning new knife after each knife collides with the specified collision gameObject ]彡★
            Game_Manager.Instance.OnSuccessfulKnifeHit();

            // ★彡[ Checking if all knives have been hit succesfully to destroy the collided game object ]彡★
            if ( _hasCollidedwithKnives == _initialKnifeCount + 1 ) {

                Destroy( other.gameObject );
            }
        }

        // ★彡[ Checking if the collided game object has the specific tag to execute below code ]彡★
        else if ( other.collider.tag == "Knife" ) {

            FlowAway();
            Game_Manager.Instance.GameOver();
        }
    }

    private void StopMovement() {

        _hasCollidedwithKnives++;
        // ★彡[ Playing sound effects on object hit ]彡★
        _audio.PlayOneShot( _hitAudio );
        // ★彡[ Playing knife hit particle effect ]彡★
        GetComponent<ParticleSystem> ().Play();
        // ★彡[ Making the rigidbody velocity zero in order to stop the movement after it collided with specific gameObject ]彡★
        _knifeRigidbody.velocity = Vector2.zero;
        // ★彡[ Making the rigidbody body type kinematic to not affect any movement after it is collided (specially gravity) ]彡★
        _knifeRigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FlowAway() {

        // ★彡[ If the knife collides with other knife we are changing its velocity to minus Vector3.one which is equal to "new Vector2(-1,-1)" in order to make knife flow away ]彡★
        _audio.PlayOneShot( _missAudio );
        _knifeRigidbody.velocity = new Vector2( -3, -3 );
    }
}
