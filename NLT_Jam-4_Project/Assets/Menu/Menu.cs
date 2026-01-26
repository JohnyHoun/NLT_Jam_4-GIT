using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public enum MenuAction
    {
        PlayGame,
        ExitGame,
        ShowCredits
    }

    public MenuAction action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (action)
            {
                case MenuAction.PlayGame:
                    SceneManager.LoadScene("Base_Scene"); // Troque pelo nome da sua cena
                    break;

                case MenuAction.ExitGame:
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    break;

                case MenuAction.ShowCredits:
                    FindObjectOfType<CreditsUI>().ShowCredits();
                    break;
            }
        }
    }
}
