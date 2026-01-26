using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos_2 : MonoBehaviour
{
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private bool exit = false;
    [SerializeField] private bool loadGame;
    [SerializeField] private bool loadMenu;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        if(objectToShow != null)
            objectToShow.SetActive(true);

        if(exit)
            Application.Quit();

        if (loadGame)
            SceneManager.LoadScene("Base_Scene");

        if (loadMenu)
            SceneManager.LoadScene("Menu");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        objectToShow.SetActive(false);
    }
}
