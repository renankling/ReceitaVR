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

    [Header("C�meras")]
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
            Debug.LogError("VoiceCommandManager: painelReceita n�o est� atribu�do no Inspector!");
            return;
        }

        if (animatorGeladeira == null)
        {
            Debug.LogError("VoiceCommandManager: Animator da geladeira n�o atribu�do!");
            return;
        }

        // --- A��es do painel ---
        System.Action acaoAbrirPainel = () => {
            painelReceita.SetActive(true);
            Debug.Log("Comando de abertura reconhecido. Painel ativado.");
        };

        System.Action acaoFecharPainel = () => {
            painelReceita.SetActive(false);
            Debug.Log("Comando de fechamento reconhecido. Painel desativado.");
        };

        // --- A��es da geladeira ---
        System.Action acaoAbrirGeladeira = () =>
        {
            animatorGeladeira.SetTrigger("AbrirGeladeira");
            Debug.Log("Anima��o de abrir geladeira disparada.");

            if (cameraPrincipal != null && cameraGeladeira != null)
            {
                cameraPrincipal.enabled = false;
                cameraGeladeira.enabled = true;
                Debug.Log("C�mera da geladeira ativada.");
            }
        };

        System.Action acaoFecharGeladeira = () =>
        {
            animatorGeladeira.SetTrigger("FecharGeladeira");
            Debug.Log("Anima��o de fechar geladeira disparada.");

            if (cameraPrincipal != null && cameraGeladeira != null)
            {
                cameraGeladeira.enabled = false;
                cameraPrincipal.enabled = true;
                Debug.Log("C�mera principal reativada.");
            }
        };

        System.Action acaoAbrirArmario = () =>
        {
            animatorArmario.SetTrigger("AbrirArmario");
            Debug.Log("Anima��o de abrir armario disparada.");
            if (cameraPrincipal != null && cameraArmario != null)
            {
                cameraArmario.enabled = true;
                cameraPrincipal.enabled = false;
                Debug.Log("C�mera principal desativada.");
            }
        };

        System.Action acaoFecharArmario = () =>
        {

            animatorArmario.SetTrigger("FecharArmario");
            Debug.Log("Anima��o de fechar armario disparada.");
            if (cameraPrincipal != null && cameraArmario != null)
            {
                cameraArmario.enabled = false;
                cameraPrincipal.enabled = true;
                Debug.Log("C�mera principal ativada.");
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
        comandos.Add("abrir o arm�rio", acaoAbrirArmario);
        comandos.Add("abrir o armario", acaoAbrirArmario);
        comandos.Add("abrir arm�rio", acaoAbrirArmario);
        comandos.Add("abrir armario", acaoAbrirArmario);
        comandos.Add("abre o arm�rio", acaoAbrirArmario);
        comandos.Add("abre arm�rio", acaoAbrirArmario);
        comandos.Add("abra o arm�rio", acaoAbrirArmario);
        comandos.Add("abra arm�rio", acaoAbrirArmario);
        comandos.Add("arm�rio abre", acaoAbrirArmario);
        comandos.Add("arm�rio abra", acaoAbrirArmario);
        comandos.Add("arm�rio abrir", acaoAbrirArmario);


        //--- Comandos para fechar o armario
        comandos.Add("fechar o arm�rio", acaoFecharArmario);
        comandos.Add("fechar arm�rio", acaoFecharArmario);
        comandos.Add("feche o arm�rio", acaoFecharArmario);
        comandos.Add("feche arm�rio", acaoFecharArmario);
        comandos.Add("fecha o arm�rio", acaoFecharArmario);
        comandos.Add("fecha arm�rio", acaoFecharArmario);
        comandos.Add("arm�rio fechar", acaoFecharArmario);
        comandos.Add("arm�rio fecha", acaoFecharArmario);


        //--- Comandos de itens
        comandos.Add("pegar ovo", () => ReceitaManager.JogadorPegou("ovo"));
        comandos.Add("pegar leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pegar chocolate", () => ReceitaManager.JogadorPegou("chocolate"));
        comandos.Add("pegar frutas", () => ReceitaManager.JogadorPegou("frutas"));
        comandos.Add("pegar baunilha", () => ReceitaManager.JogadorPegou("baunilha"));
        comandos.Add("pegar cenoura", () => ReceitaManager.JogadorPegou("cenoura"));

        // Comando de finalizar
        comandos.Add("finalizar receita", () => ReceitaManager.FinalizarReceita());



        // --- Inicializa��o do KeywordRecognizer ---
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
        Debug.Log("Voc� disse: '" + recognizedText + "' (Confian�a: " + args.confidence + ")");

        if (comandos.ContainsKey(recognizedText))
        {
            comandos[recognizedText].Invoke();
        }
        else
        {
            Debug.LogWarning("Comando reconhecido, mas n�o mapeado: '" + recognizedText + "'");
        }
    }
}
