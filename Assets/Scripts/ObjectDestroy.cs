using UnityEngine;
using UnityEngine.SceneManagement;

public class PreventObjectFromChangingScene : MonoBehaviour
{
    // Tag objektu, kter� nechceme, aby p�e�il zm�nu sc�ny
    public string tagToExcludeFromScene = "ExcludeFromScene";

    // Tato metoda zni�� objekt p�ed zm�nou sc�ny
    public void DestroyObjectBeforeSceneChange()
    {
        // Najde objekt s po�adovan�m tagem
        GameObject objectToDestroy = GameObject.FindGameObjectWithTag(tagToExcludeFromScene);

        // Pokud objekt existuje, zni�� ho
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Debug.Log($"Object with tag {tagToExcludeFromScene} has been destroyed before scene change.");
        }
        else
        {
            Debug.Log($"No object found with tag {tagToExcludeFromScene}.");
        }
    }

    // Tato metoda zm�n� sc�nu po zni�en� objektu
    public void ChangeScene(string sceneName)
    {
        // Nejprve zni�te objekt, kter� nechcete, aby p�e�il zm�nu sc�ny
        DestroyObjectBeforeSceneChange();

        // Pot� zm��te sc�nu
        SceneManager.LoadScene(sceneName);
    }
}
