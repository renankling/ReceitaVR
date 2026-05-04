using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class VoiceMenuManager : MonoBehaviour
{
    private KeywordRecognizer reconhecedor;
    private Dictionary<string, System.Action> comandos = new Dictionary<string, System.Action>();


    void Start()
    {
        
        comandos.Add("jogar", () => SceneManager.LoadScene(1));
        comandos.Add("iniciar", () => SceneManager.LoadScene(1));
        comandos.Add("começar", () => SceneManager.LoadScene(1));

        comandos.Add("creditos", () => SceneManager.LoadScene("Creditos"));
        comandos.Add("créditos", () => SceneManager.LoadScene("Creditos"));

        comandos.Add("sair", SairJogo);
        comandos.Add("fechar jogo", SairJogo);

        reconhecedor = new KeywordRecognizer(comandos.Keys.ToArray());
        reconhecedor.OnPhraseRecognized += OnReconhecer;
        reconhecedor.Start();

        Debug.Log("VoiceMenuManager iniciado.");
    }

    void OnReconhecer(PhraseRecognizedEventArgs args)
    {
        string comando = args.text.ToLower();
        Debug.Log("Você disse: " + comando);

        if (comandos.ContainsKey(comando))
        {
            comandos[comando].Invoke();
        }
    }


    void SairJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    void OnDestroy()
    {
        if (reconhecedor != null)
        {
            reconhecedor.Stop();
            reconhecedor.Dispose();
        }
    }
}