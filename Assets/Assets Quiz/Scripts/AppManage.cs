using UnityEngine;
using UnityEngine.UI;

public class AppManage : MonoBehaviour
{
    General general;

    private void Awake()
    {
        general = new General();
    }

    #region NAVIGATION MANAGE

    public void MScene(string scene)
    {
        general.MScene(scene);
    }

    public void MMenu(GameObject menu)
    {
        general.MMenu(menu);
    }

    public void CMenu(GameObject menu)
    {
        general.CMenu(menu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion NAVIGATION MANAGE
}
