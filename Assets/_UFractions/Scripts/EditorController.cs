using UnityEngine;
using TMPro;

/// <summary>
/// Thrown together editor with no real functionality except saving the hardcoded debug story and display local story folder path.
/// </summary>
public class EditorController : MonoBehaviour
{

    public Story story;
    public TextMeshProUGUI nameText;
    public TMP_InputField pathText;

	/// <summary>
    /// Get the local story path on start.
    /// </summary>
	void Start ()
    {
        pathText.text = StoryManager.Instance.GetLocalStoriesPath();
    }

    /// <summary>
    /// Saves the debug story of storymanager.
    /// </summary>
    public void SaveStory()
    {
        StoryManager.Instance.CreateDebugStory(this.nameText.text);
    }

    /// <summary>
    /// Set the name to use when saving the story.
    /// </summary>
    /// <param name="name"></param>
    public void SetStoryName(string name)
    {
        story.SetName(name);
    }
}
