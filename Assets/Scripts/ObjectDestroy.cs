using UnityEngine;
using UnityEngine.SceneManagement;

public class PreventObjectFromChangingScene : MonoBehaviour
{
    // Tag objektu, který nechceme, aby pøežil zmìnu scény
    public string tagToExcludeFromScene = "ExcludeFromScene";

    // Tato metoda znièí objekt pøed zmìnou scény
    public void DestroyObjectBeforeSceneChange()
    {
        // Najde objekt s požadovaným tagem
        GameObject objectToDestroy = GameObject.FindGameObjectWithTag(tagToExcludeFromScene);

        // Pokud objekt existuje, znièí ho
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

    // Tato metoda zmìní scénu po znièení objektu
    public void ChangeScene(string sceneName)
    {
        // Nejprve zniète objekt, který nechcete, aby pøežil zmìnu scény
        DestroyObjectBeforeSceneChange();

        // Poté zmìòte scénu
        SceneManager.LoadScene(sceneName);
    }
}
