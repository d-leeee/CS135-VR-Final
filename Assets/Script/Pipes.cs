
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pipes : MonoBehaviour
{
    public static float speed = 5f;
    public static float baseSpeed = 5f;
    public GameObject despawn;
    public InputActionReference buttonPressA;
    public float minHeight = -10f;
    public float maxHeight = 0f;
    public float randomHeight;
    public CapsuleCollider bottomPipe;
    public CapsuleCollider topPipe;
    public CharacterController player;
    public GameObject pipe1;
    public GameObject pipe2;

    public AudioSource scoreScource;
    public AudioClip scoreClip;
    private bool scorePlayedThisPass = false;

    public AudioSource deathSource;
    public AudioClip deathClip;
    private bool deathSoundPlayed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset score flag when pipe is back at starting position
        if (this.transform.position.x > 0) // Log the score
        {
            scorePlayedThisPass = false;
        }

        Debug.Log($"Update called on {gameObject.name}");
        
        //score
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (this.transform.position.x < -1.5f) {
            // Player has passed the box collider
            ScoreOutput.score++; // Increment score
            Debug.Log("Score: " + ScoreOutput.score);

            int speedMultiplier = ScoreOutput.score / 20; 

            speed = baseSpeed + (speedMultiplier * 1.5f);

            randomHeight = Random.Range(minHeight, maxHeight);


            float overshoot = -1.5f - this.transform.position.x;
            float resetX = 15f - overshoot; 
            transform.position = new Vector3(resetX, randomHeight, transform.position.z);

            // randomHeight = Random.Range(minH // Log the scoreeight, maxHeight);
            // transform.position = new Vector3(15, randomHeight, transform.position.z);
        
            // Play score sound once per pipe pass
            if (!scorePlayedThisPass && scoreScource != null && scoreClip != null)
            {
                scoreScource.PlayOneShot(scoreClip);
                scorePlayedThisPass = true;
            }
        }

        // Check collision with bottom pipe
        if (player != null && bottomPipe != null)
        {
            if (CheckCapsuleCollision(bottomPipe))
            {
                Debug.Log("Player hit bottom pipe! Game Over!");
                if (!deathSoundPlayed && deathSource != null && deathClip != null)
                {
                    deathSource.PlayOneShot(deathClip);
                    deathSoundPlayed = true;
                }
                Time.timeScale = 0f; 
            }
        }

        // Check collision with top pipe
        if (player != null && topPipe != null)
        {
            if (CheckCapsuleCollision(topPipe))
            {
                Debug.Log("Player hit top pipe! Game Over!");
                if (!deathSoundPlayed && deathSource != null && deathClip != null)
                {
                    deathSource.PlayOneShot(deathClip);
                    deathSoundPlayed = true;
                }
                Time.timeScale = 0f; 
            }
        }
    }

    // Helper method to check collision between player and a capsule collider
    bool CheckCapsuleCollision(CapsuleCollider capsule)
    {
        if (capsule == null)
        {
            Debug.LogWarning("Capsule collider is null!");
            return false;
        }

        Vector3 point1 = capsule.bounds.center + Vector3.up * capsule.bounds.extents.y;
        Vector3 point2 = capsule.bounds.center - Vector3.up * capsule.bounds.extents.y;
        
        Collider[] hits = Physics.OverlapCapsule(point1, point2, capsule.radius, -1, QueryTriggerInteraction.Collide);
        Debug.Log($"Checking {capsule.gameObject.name}: Found {hits.Length} colliders");
        
        foreach (Collider hit in hits)
        {
            Debug.Log($"  Collider: {hit.gameObject.name}, Has CharacterController: {hit.GetComponent<CharacterController>() != null}");
            
            // Check if this collider belongs to the player object
            if (hit.gameObject == player.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void OnEnable()
    {
        if (buttonPressA != null && buttonPressA.action != null)
        {
            buttonPressA.action.performed += OnButtonPressA;
        }
    }
    void OnDisable()
    {
        if (buttonPressA != null && buttonPressA.action != null)
        {
            buttonPressA.action.performed -= OnButtonPressA;
        }
    }

    private void OnButtonPressA(InputAction.CallbackContext callback)
    {
        // Fully restart the game by reloading the scene
        Time.timeScale = 1f; // Resume time first
        ScoreOutput.score = 0; // Reset score
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}