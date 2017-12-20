using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A singleton like class that handles instantiation of different interfaces that needs additional components (such as UI elements).
/// HOW TO USE: 
/// 1. Create an empty GameObject and attach this script as a component and check the isMaster bool to true.
/// 2. Add interfaces prefabs to the interfaces array in the inspector.
/// 3. Add AR Objects prefabs to the arObjectPrefabs array in the inspector.
/// 
/// NOTES!: In Unity Script Execution Order set InterfaceFactory to above default time. Also make sure there is only one InterfaceFactory in every scene.
/// </summary>
public class InterfaceFactory : MonoBehaviour
{
    public bool isMaster = false;

    private static InterfaceFactory instance;

    public GameObject[] interfaces;
    public GameObject[] arObjectPrefabs;

    [SerializeField] private Text debugConsole; /// Old, used before to print debug messages on android

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

        print("WARNING! InterfaceFactory has not been setup in scene. Will return null.");
        return null;
    }

    /// <summary>
    /// Creates a AnswerBehaviour interface and return the reference.
    /// </summary>
    /// <param name="parent">The to be parent of the Behaviour.</param>
    /// <param name="answerType">The AnswerBehaviour type.</param>
    /// <returns>The Behaviour reference.</returns>
    public AnswerBehaviour GetAnswerInterface(Transform parent, AnswerBehaviour.AnswerType answerType)
    {
        AnswerBehaviour ab = null;
        foreach (GameObject go in this.interfaces)
        {
            ab = go.GetComponent<AnswerBehaviour>();
            if (ab != null && ab.answerType == answerType)
                return Instantiate(go, parent).GetComponent<AnswerBehaviour>();
        }
        return null;
    }

    /// <summary>
    /// Creates a QuestionBehaviour interface and return the reference.
    /// </summary>
    /// <param name="parent">The to be parent of the Behaviour.</param>
    /// <param name="questionType">The QuestionBehaviour type.</param>
    /// <returns>The Behaviour reference.</returns>
    public QuestionBehaviour GetQuestionInterface(Transform parent, QuestionBehaviour.QuestionType questionType)
    {
        QuestionBehaviour qb = null;
        foreach (GameObject go in this.interfaces)
        {
            qb = go.GetComponent<QuestionBehaviour>();
            if (qb != null)
            {
                if (qb.questionType == questionType)
                    return Instantiate(go, parent).GetComponent<QuestionBehaviour>();
            }
        }
        return null;
    }

    /// <summary>
    /// Creates an ARObject of given type.
    /// </summary>
    /// <param name="aRObjaectType">The ARObjectType of the ArObject to create.</param>
    /// <param name="parent">The parent the created ARObject should have.</param>
    public void SpawnARObject(ARObjectType aRObjaectType, Transform parent)
    {
        this.SpawnARObject(aRObjaectType, parent, Vector3.zero);
    }

    /// <summary>
    /// Creates an ARObject of given type.
    /// </summary>
    /// <param name="aRObjaectType">The ARObjectType of the ArObject to create.</param>
    /// <param name="parent">The parent the created ARObject should have.</param>
    /// <param name="position">The position of the ARObject.</param>
    public void SpawnARObject(ARObjectType aRObjaectType, Transform parent, Vector3 position)
    {
        this.SpawnARObject(aRObjaectType, parent, position, Quaternion.identity);
    }

    /// <summary>
    /// Creates an ARObject of given type.
    /// </summary>
    /// <param name="aRObjaectType">The ARObjectType of the ArObject to create.</param>
    /// <param name="parent">The parent the created ARObject should have.</param>
    /// <param name="position">The position of the ARObject.</param>
    /// <param name="rotation">The rotation of the ARObject.</param>
    public void SpawnARObject(ARObjectType aRObjaectType, Transform parent, Vector3 position, Quaternion rotation)
    {
        foreach (GameObject g in this.arObjectPrefabs)
        {
            ArObjectType arObj = g.GetComponent<ArObjectType>();
            if (arObj != null)
            {
                if (arObj.objType == aRObjaectType)
                {
                    GameObject go = Instantiate(g, parent);
                    go.transform.position = position;
                    go.transform.rotation = rotation;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Old, used to print messages on android before. Dont use, make something else.
    /// </summary>
    /// <param name="text">The test to print.</param>
    public void DebugLog(string text)
    {
        if (this.debugConsole != null)
            this.debugConsole.text += System.Environment.NewLine + text;
    }
}
