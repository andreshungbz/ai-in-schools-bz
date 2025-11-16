using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueLine : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text dialogueText;   // The text on the prefab
    public AudioSource audioSource; // The audio source on the prefab

    [Header("Settings")]
    public float defaultDisplayTime = 2f;  // Duration if a line has no audio

    [Header("Dialogue Data")]
    public Transform dialogueLinesParent;  // Parent containing 01-line, 02-line, etc.

    [Header("Scene Settings")]
    public string sceneToLoad = "Menu";  // Scene name to load after dialogue

    // Called by Visual Script via MonoBehaviour Invoke
    public void PlaySequenceFromChildren()
    {
        StartCoroutine(PlayLinesCoroutine());
    }

    private IEnumerator PlayLinesCoroutine()
    {
        foreach (Transform line in dialogueLinesParent)
        {
            // Get TMP_Text and AudioSource from this child
            TMP_Text lineText = line.GetComponentInChildren<TMP_Text>();
            AudioSource lineAudio = line.GetComponentInChildren<AudioSource>();

            // Update dialogue text
            if (lineText != null)
                dialogueText.text = lineText.text;

            // Play audio if it exists, otherwise wait default time
            if (lineAudio != null && lineAudio.clip != null)
            {
                audioSource.clip = lineAudio.clip;
                audioSource.Play();
                yield return new WaitForSeconds(lineAudio.clip.length);
            }
            else
            {
                yield return new WaitForSeconds(defaultDisplayTime);
            }
        }

        // Dialogue finished → load the Menu scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
