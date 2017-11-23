using UnityEngine;

/// <summary>
/// A singleton like class that handles instantiation of different interfaces that needs additional components (such as UI elements).
/// Create a GameObject or Prefab that holds all interface prefab references and activate the master bool to ensure it is the only one used.
/// To add a new interface, add a private serialized gameobject field and make a public function for getting the interface object.
/// NOTE: In Unity Script Execution Order set InterfaceFactory to above default time.
/// </summary>
public class InterfaceFactory : MonoBehaviour
{
    public bool isMaster = false;

    private static InterfaceFactory instance;

    [SerializeField] private GameObject choiceAnswerInterface;
    [SerializeField] private GameObject textBoxQuestionInterface;

    /// <summary>
    /// Ensure simple singleton like behaviour with thread lock.
    /// </summary>
    void Start ()
    {
        lock (this)
        {
            if (this.isMaster)
            {
                if (instance == null)
                    instance = this;
                else if (instance != this)
                    Destroy(this);
            }
        }
	}
	
    /// <summary>
    /// Gets the instance of the InterfaceFactory.
    /// </summary>
    /// <returns>The instance.</returns>
    public static InterfaceFactory GetInstance()
    {
        if (instance != null)
            return instance;

        print("InterfaceFactory has not been setup in scene. Will return null.");
        return null;
    }

    /// <summary>
    /// Creates a AnswerBehaviour interface and return the reference.
    /// </summary>
    /// <param name="parent">The to be parent of the Behaviour.</param>
    /// <param name="answerType">The AnswerBehaviour type.</param>
    /// <returns>The Behaviour reference.</returns>
    public AnswerBehaviour GetAnswerInterface(Transform parent, Story.AnswerType answerType)
    {
        if (answerType == Story.AnswerType.Choices)
            return Instantiate(choiceAnswerInterface, parent).GetComponent<AnswerBehaviour>();
        else
            return null;
    }

    /// <summary>
    /// Creates a QuestionBehaviour interface and return the reference.
    /// </summary>
    /// <param name="parent">The to be parent of the Behaviour.</param>
    /// <param name="questionType">The QuestionBehaviour type.</param>
    /// <returns>The Behaviour reference.</returns>
    public QuestionBehaviour GetQuestionInterface(Transform parent, Story.QuestionType questionType)
    {
        if (questionType == Story.QuestionType.TextBox)
            return Instantiate(this.textBoxQuestionInterface, parent).GetComponent<QuestionBehaviour>();
        else
            return null;
    }
}
