using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour
{
    public TMP_InputField nameInput;
    [SerializeField]GameObject scorecard;
    public GameObject endMenu;

    public  TMP_Text ScoreText;

    public void Update(){
        ScoreText.text=ScoreManager.highScore.ToString();
    }

    public void SaveName()
    {
        int pscore=ScoreManager.highScore;
        string playerName = nameInput.text;
        string pname=playerName;
        HighscoreTable.AddHighscoreEntry(pscore,pname);
    }

    public void Replay()
    {
        Time.timeScale=1f;
        endMenu.SetActive(false);
        Transition Trann = FindObjectOfType<Transition>();
        Trann.LoadLevel3();
    }
}
