using UnityEngine;
using UnityEngine.SceneManagement;

public class General
{
    public void MScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void MMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void CMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
}
