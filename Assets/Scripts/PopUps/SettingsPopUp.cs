using UnityEngine;
using KendaGame.Helpers;
using TMPro;
using I2.Loc;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsPopUp : MonoBehaviour {
    [SerializeField]
    private UIButtonAnimator musicButton; // Button to control music state (on/off).

    [SerializeField]
    private Image musicMute; // Icon to indicate whether music is muted or not.

    [SerializeField]
    private UIButtonAnimator sfxButton; // Button to control sound effects state (on/off).

    [SerializeField]
    private Image sfxMute; // Icon to indicate whether sound effects are muted or not.

    [SerializeField]
    private UIButtonAnimator exitButton; // Button to close the settings popup.

    //[SerializeField]
    //TMP_Dropdown languageDropDown; // Dropdown UI element for language selection.

    private bool isMusicMuted; // Indicates whether music is currently muted.
    private bool isSfxMuted; // Indicates whether sound effects are currently muted.

    private void Start() {
        // Initialize the music and sound effects mute states based on AudioController.
        isMusicMuted = !AudioController.Instance.GetMusicState();
        isSfxMuted = !AudioController.Instance.GetSfxState();

        InitializeButtons(); // Set up click listeners for buttons and dropdown.
        UpdateButtonIcon(); // Update button icons (mute icons) based on the current states.
    }

    private void InitializeButtons() {
      //  languageDropDown.onValueChanged.AddListener(OnLanguageDropdown); // Listen for dropdown value changes.
        musicButton.AddClickListener(OnMusicButtonPressed); // Listen for music button click.
        sfxButton.AddClickListener(OnSfxButtonPressed); // Listen for sound effects button click.
        exitButton.AddClickListener(OnExitButtonPressed); // Listen for exit button click.
    }

    private void OnMusicButtonPressed() {
        // Toggle the music state, update the 'isMusicMuted' variable, and update the button icon.
        AudioController.Instance.SetMusicState(isMusicMuted);
        isMusicMuted = !isMusicMuted;
        UpdateButtonIcon();
    }

    private void OnSfxButtonPressed() {
        // Toggle the sound effects state, update the 'isSfxMuted' variable, and update the button icon.
        AudioController.Instance.SetSfxState(isSfxMuted);
        isSfxMuted = !isSfxMuted;
        UpdateButtonIcon();
    }

    public void UpdateButtonIcon() {
        // Update button icons (mute icons) based on the current 'isSfxMuted' and 'isMusicMuted' states.
        sfxMute.enabled = isSfxMuted;
        musicMute.enabled = isMusicMuted;
    }
    /*
    private void OnLanguageDropdown(int langIndex) {
        // Update the game's language based on the selected dropdown value.
        // The language code 'ar' represents Arabic, and 'en' represents English.
        string languageCode = (langIndex == 0 ? "ar" : "en");
        if (LocalizationManager.CurrentLanguageCode != languageCode) {
            LocalizationManager.CurrentLanguageCode = languageCode;
            LocalizationManager.UpdateSources();
        }
    }
    */
    private void OnLogoutButtonPressed() {
        // This function will be implemented after setting up the database and authentication.
    }

    private void OnExitButtonPressed() {
        // Hide the settings popup by deactivating the game object.
        gameObject.SetActive(false);
    }

    public void ShowPopUp() {
        // Show the settings popup by activating the game object.
        gameObject.SetActive(true);
    }
}
