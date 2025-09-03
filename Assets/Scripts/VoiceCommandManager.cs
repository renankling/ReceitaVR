using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class VoiceCommandManager : MonoBehaviour
{
    private KeywordRecognizer reconhecedor;
    private Dictionary<string, System.Action> comandos = new Dictionary<string, System.Action>();

    [Header("Objetos de UI")]
    public GameObject painelReceita;

    [Header("Animator da Geladeira")]
    public Animator animatorGeladeira;
    public Animator animatorArmario;

    [Header("Câmeras")]
    public Camera cameraPrincipal;
    public Camera cameraGeladeira;
    public Camera cameraArmario;

    [Header("Receita Manager")]
    public ReceitaManager ReceitaManager;

    void Start()
    {
        Debug.Log("VoiceCommandManager: Start chamado.");

        if (painelReceita == null)
        {
            Debug.LogError("VoiceCommandManager: painelReceita não está atribuído no Inspector!");
            return;
        }

        if (animatorGeladeira == null)
        {
            Debug.LogError("VoiceCommandManager: Animator da geladeira não atribuído!");
            return;
        }

        // --- Ações do painel ---
        System.Action acaoAbrirPainel = () => {
            painelReceita.SetActive(true);
            Debug.Log("Comando de abertura reconhecido. Painel ativado.");
        };

        System.Action acaoFecharPainel = () => {
            painelReceita.SetActive(false);
            Debug.Log("Comando de fechamento reconhecido. Painel desativado.");
        };

        // --- Ações da geladeira ---
        System.Action acaoAbrirGeladeira = () =>
        {
            animatorGeladeira.SetTrigger("AbrirGeladeira");
            Debug.Log("Animação de abrir geladeira disparada.");

            if (cameraPrincipal != null && cameraGeladeira != null)
            {
                cameraPrincipal.enabled = false;
                cameraGeladeira.enabled = true;
                Debug.Log("Câmera da geladeira ativada.");
            }
        };

        System.Action acaoFecharGeladeira = () =>
        {
            animatorGeladeira.SetTrigger("FecharGeladeira");
            Debug.Log("Animação de fechar geladeira disparada.");

            if (cameraPrincipal != null && cameraGeladeira != null)
            {
                cameraGeladeira.enabled = false;
                cameraPrincipal.enabled = true;
                Debug.Log("Câmera principal reativada.");
            }
        };

        System.Action acaoAbrirArmario = () =>
        {
            animatorArmario.SetTrigger("AbrirArmario");
            Debug.Log("Animação de abrir armario disparada.");
            if (cameraPrincipal != null && cameraArmario != null)
            {
                cameraArmario.enabled = true;
                cameraPrincipal.enabled = false;
                Debug.Log("Câmera principal desativada.");
            }
        };

        System.Action acaoFecharArmario = () =>
        {

            animatorArmario.SetTrigger("FecharArmario");
            Debug.Log("Animação de fechar armario disparada.");
            if (cameraPrincipal != null && cameraArmario != null)
            {
                cameraArmario.enabled = false;
                cameraPrincipal.enabled = true;
                Debug.Log("Câmera principal ativada.");
            }
        };

        // --- Comandos para abrir painel ---
        comandos.Add("abrir receita", acaoAbrirPainel);
        comandos.Add("abrir a receita", acaoAbrirPainel);
        comandos.Add("abra receita", acaoAbrirPainel);
        comandos.Add("abra a receita", acaoAbrirPainel);
        comandos.Add("abre receita", acaoAbrirPainel);
        comandos.Add("abre a receita", acaoAbrirPainel);
        comandos.Add("receita abre", acaoAbrirPainel);
        comandos.Add("receita abrir", acaoAbrirPainel);
        comandos.Add("receita abra", acaoAbrirPainel);

        // --- Comandos para fechar painel ---
        comandos.Add("fechar receita", acaoFecharPainel);
        comandos.Add("fechar a receita", acaoFecharPainel);
        comandos.Add("fecha a receita", acaoFecharPainel);
        comandos.Add("fecha receita", acaoFecharPainel);
        comandos.Add("feche receita", acaoFecharPainel);
        comandos.Add("feche a receita", acaoFecharPainel);
        comandos.Add("receita fechar", acaoFecharPainel);
        comandos.Add("receita fecha", acaoFecharPainel);

        // --- Comandos para abrir geladeira ---
        comandos.Add("abrir geladeira", acaoAbrirGeladeira);
        comandos.Add("abrir a geladeira", acaoAbrirGeladeira);
        comandos.Add("abra geladeira", acaoAbrirGeladeira);
        comandos.Add("abra a geladeira", acaoAbrirGeladeira);
        comandos.Add("abre geladeira", acaoAbrirGeladeira);
        comandos.Add("abre a geladeira", acaoAbrirGeladeira);
        comandos.Add("geladeira abre", acaoAbrirGeladeira);
        comandos.Add("geladeira abra", acaoAbrirGeladeira);
        comandos.Add("geladeira abrir", acaoAbrirGeladeira);

        // --- Comandos para fechar geladeira ---
        comandos.Add("fechar geladeira", acaoFecharGeladeira);
        comandos.Add("fechar a geladeira", acaoFecharGeladeira);
        comandos.Add("feche a geladeira", acaoFecharGeladeira);
        comandos.Add("feche geladeira", acaoFecharGeladeira);
        comandos.Add("fecha a geladeira", acaoFecharGeladeira);
        comandos.Add("fecha geladeira", acaoFecharGeladeira);
        comandos.Add("geladeira fecha", acaoFecharGeladeira);
        comandos.Add("geladeira fechar", acaoFecharGeladeira);

        //--- Comandos para abrir armario
        comandos.Add("abrir o armário", acaoAbrirArmario);
        comandos.Add("abrir o armario", acaoAbrirArmario);
        comandos.Add("abrir armário", acaoAbrirArmario);
        comandos.Add("abrir armario", acaoAbrirArmario);
        comandos.Add("abre o armário", acaoAbrirArmario);
        comandos.Add("abre armário", acaoAbrirArmario);
        comandos.Add("abra o armário", acaoAbrirArmario);
        comandos.Add("abra armário", acaoAbrirArmario);
        comandos.Add("armário abre", acaoAbrirArmario);
        comandos.Add("armário abra", acaoAbrirArmario);
        comandos.Add("armário abrir", acaoAbrirArmario);


        //--- Comandos para fechar o armario
        comandos.Add("fechar o armário", acaoFecharArmario);
        comandos.Add("fechar armário", acaoFecharArmario);
        comandos.Add("feche o armário", acaoFecharArmario);
        comandos.Add("feche armário", acaoFecharArmario);
        comandos.Add("fecha o armário", acaoFecharArmario);
        comandos.Add("fecha armário", acaoFecharArmario);
        comandos.Add("armário fechar", acaoFecharArmario);
        comandos.Add("armário fecha", acaoFecharArmario);


        //--- Comandos de itens
        comandos.Add("pegar ovo", () => ReceitaManager.JogadorPegou("ovo"));
        comandos.Add("pegar leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pegar chocolate", () => ReceitaManager.JogadorPegou("chocolate"));
        comandos.Add("pegar frutas", () => ReceitaManager.JogadorPegou("frutas"));
        comandos.Add("pegar baunilha", () => ReceitaManager.JogadorPegou("baunilha"));
        comandos.Add("pegar cenoura", () => ReceitaManager.JogadorPegou("cenoura"));

        // Comando de finalizar
        comandos.Add("finalizar receita", () => ReceitaManager.FinalizarReceita());



        // --- Inicialização do KeywordRecognizer ---
        string[] keywords = comandos.Keys.ToArray();
        if (keywords.Length == 0)
        {
            Debug.LogError("VoiceCommandManager: Nenhum comando de palavra-chave adicionado.");
            return;
        }

        Debug.Log("VoiceCommandManager: Tentando inicializar KeywordRecognizer com " + keywords.Length + " comandos.");
        foreach (string keyword in keywords)
        {
           // Debug.Log("Comando registrado: " + keyword);
        }

        reconhecedor = new KeywordRecognizer(keywords);
        reconhecedor.OnPhraseRecognized += OnReconhecerComando;
        reconhecedor.Start();
        Debug.Log("VoiceCommandManager: KeywordRecognizer iniciado.");
    }

    void OnDestroy()
    {
        if (reconhecedor != null)
        {
            reconhecedor.Stop();
            reconhecedor.Dispose();
            reconhecedor = null;
            Debug.Log("VoiceCommandManager: KeywordRecognizer parado e liberado.");
        }
    }

    private void OnReconhecerComando(PhraseRecognizedEventArgs args)
    {
        string recognizedText = args.text.ToLower();
        Debug.Log("Você disse: '" + recognizedText + "' (Confiança: " + args.confidence + ")");

        if (comandos.ContainsKey(recognizedText))
        {
            comandos[recognizedText].Invoke();
        }
        else
        {
            Debug.LogWarning("Comando reconhecido, mas não mapeado: '" + recognizedText + "'");
        }
    }
}
