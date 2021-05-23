using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadNextLevelDelay = 1;
    [SerializeField] float reloadLevelDelay = 1; 
     
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isTransitioning = false;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();   
    }

    private void OnCollisionEnter(Collision other)
    {
        int activeLevel = 0;

        if (isTransitioning) { return ;}

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish": // hit the landing platform
                activeLevel ++;
                StartSuccessSequence();
                break;
            default: // hit the ground or enemy object
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();                      
        audioSource.PlayOneShot(success);             
        successParticles.Play();       
        GetComponent<Movement>().enabled = false;
        Invoke ("LoadNextLevel", loadNextLevelDelay);
    }    

    void StartCrashSequence()
    {
        isTransitioning = true; 

        audioSource.Stop();
        audioSource.PlayOneShot(crash);       
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke ("ReloadLevel", reloadLevelDelay);        
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);  
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentSceneIndex);    
    }  
      
}

