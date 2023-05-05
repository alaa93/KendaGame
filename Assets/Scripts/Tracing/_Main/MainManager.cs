using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameObject coloringMenu, paintingMenu;
    public RectTransform coloringMenuButton, paintingMenuButton;

    void Start()
    {
        OnMenuButtonClicked(PlayerPrefs.GetInt("isPainting", 0) == 1);
    }

    public void OnMenuButtonClicked(bool isPainting)
    {
        PlayerPrefs.SetInt("isPainting", isPainting ? 1 : 0);
        PlayerPrefs.Save();

        paintingMenu.SetActive(isPainting);
        coloringMenu.SetActive(!isPainting);

        paintingMenuButton.transform.localScale = isPainting ? Vector3.one : new Vector3(0.7f, 0.7f, 0);
        coloringMenuButton.transform.localScale = !isPainting ? Vector3.one : new Vector3(0.7f, 0.7f, 0);
    }

    public void PlaySoundClick()
    {
        MusicController.USE.PlaySound(MusicController.USE.clickSound);
    }
}