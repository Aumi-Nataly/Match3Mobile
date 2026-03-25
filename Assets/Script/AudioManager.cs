using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip removeSoundClip;

    private AudioSource audioRemoveSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        audioRemoveSource = gameObject.AddComponent<AudioSource>();
        audioRemoveSource.clip = removeSoundClip;
        audioRemoveSource.volume = 0.8f;
    }

    public void PlayRemoveSound()
    {
        if (audioRemoveSource != null && removeSoundClip != null)
        {
            audioRemoveSource.PlayOneShot(removeSoundClip);
        }
    }
}
