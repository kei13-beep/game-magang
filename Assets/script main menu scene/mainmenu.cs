using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;           // Singleton instance

    public Button startButton;                 // Reference to the Start button
    public Button quitButton;                  // Reference to the Quit button
    public AudioClip clickSound;               // Assign the click sound in the Inspector
    public AudioSource backgroundMusic;        // Reference to the background music AudioSource
    public Slider musicVolumeSlider;           // Reference to the volume slider for background music
    private AudioSource clickAudioSource;      // AudioSource for the click sound

    private void Awake()
    {
        // Singleton pattern to make sure only one instance of MainMenu exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy any duplicate instance
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);  // Persist this GameObject across scenes
    }

    private void Start()
    {
        // Initialize background music
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.volume = musicVolumeSlider != null ? musicVolumeSlider.value : 0.5f;
            backgroundMusic.Play();
        }

        // Set up click sound AudioSource
        clickAudioSource = gameObject.AddComponent<AudioSource>();
        clickAudioSource.playOnAwake = false;
        clickAudioSource.clip = clickSound;
        clickAudioSource.volume = 0.2f; // Set initial volume for click sound

        // Add listeners to buttons in the main menu
        if (startButton != null)
        {
            startButton.onClick.AddListener(() => StartCoroutine(OnStartGameButtonClicked()));
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() => StartCoroutine(OnQuitGameButtonClicked()));
        }

        // Add listener for the music volume slider
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    private IEnumerator OnStartGameButtonClicked()
    {
        PlayClickSound();
        if (startButton != null) startButton.interactable = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SampleScene");  // Load target scene
        if (startButton != null) startButton.interactable = true;
    }

    private IEnumerator OnQuitGameButtonClicked()
    {
        PlayClickSound();
        if (quitButton != null) quitButton.interactable = false;
        yield return new WaitForSeconds(1f);
        Debug.Log("Exiting Game...");
        Application.Quit();
        if (quitButton != null) quitButton.interactable = true;
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            clickAudioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Click sound not assigned!");
        }
    }

    private void SetMusicVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume;
        }
    }
}
