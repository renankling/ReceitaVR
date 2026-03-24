using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CreditosManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += VoltarMenu;
    }

    void VoltarMenu(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu");
    }
}