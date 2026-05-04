using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class VoiceCommandManager : MonoBehaviour
{
    private KeywordRecognizer reconhecedor;
    private Dictionary<string, System.Action> comandos = new Dictionary<string, System.Action>();

    [Header("Objetos de UI")]
    public GameObject painelReceita;

    [Header("Animator da Geladeira")]
    public Animator animatorGeladeira;
    public Animator animatorArmario;
    public GameObject canvasPontuacao;

    [Header("C?meras")]
    public Camera cameraPrincipal;
    public Camera cameraGeladeira;
    public Camera cameraArmario;

    [Header("Receita Manager")]
    public ReceitaManager ReceitaManager;

    [Header("Painel de Informacoes")]
    public GameObject painelInformacoes;

    void Start()
    {
        Debug.Log("VoiceCommandManager: Start chamado.");

        if (painelReceita == null)
        {
            Debug.LogError("VoiceCommandManager: painelReceita nao esta atribuido no Inspector!");
            return;
        }

        if (animatorGeladeira == null)
        {
            Debug.LogError("VoiceCommandManager: Animator da geladeira nao atribuido!");
            return;
        }

        // --- Acoees do painel ---
        System.Action acaoAbrirPainel = () => {
            painelReceita.SetActive(true);

            ReceitaManager.RegistrarAberturaReceita();

            Debug.Log("Comando de abertura reconhecido. Painel ativado.");
        };

        System.Action acaoFecharPainel = () => {
            painelReceita.SetActive(false);
            Debug.Log("Comando de fechamento reconhecido. Painel desativado.");
        };

        // --- Acoes do painel de informa??es ---
        System.Action acaoFecharInformacoes = () => {
            if (painelInformacoes != null)
            {
                painelInformacoes.SetActive(false);
                Debug.Log("Painel de informacoes fechado.");
            }
        };

        System.Action acaoAbrirInformacoes = () => {
            if (painelInformacoes != null)
            {
                painelInformacoes.SetActive(true);
                Debug.Log("Painel de informacoes aberto.");
            }
        };

        // --- A??es da geladeira ---
        System.Action acaoAbrirGeladeira = () =>
        {
            animatorGeladeira.SetTrigger("AbrirGeladeira");
            Debug.Log("Animacao de abrir geladeira disparada.");

            if (cameraPrincipal != null && cameraGeladeira != null)
            {
                cameraPrincipal.enabled = false;
                cameraGeladeira.enabled = true;
                Debug.Log("Camera da geladeira ativada.");
            }
        };

        System.Action acaoFecharGeladeira = () =>
        {
            animatorGeladeira.SetTrigger("FecharGeladeira");
            Debug.Log("Animacao de fechar geladeira disparada.");

            if (cameraPrincipal != null && cameraGeladeira != null)
            {
                cameraGeladeira.enabled = false;
                cameraPrincipal.enabled = true;
                Debug.Log("Camera principal reativada.");
            }
        };

        System.Action acaoAbrirArmario = () =>
        {
            animatorArmario.SetTrigger("AbrirArmario");
            Debug.Log("Animacao de abrir armario disparada.");
            if (cameraPrincipal != null && cameraArmario != null)
            {
                cameraArmario.enabled = true;
                cameraPrincipal.enabled = false;
                Debug.Log("Camera principal desativada.");
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
                Debug.Log("Camera principal ativada.");
            }
        };

        comandos.Add("próximo nível", ProximoNivel);
        comandos.Add("proximo nivel", ProximoNivel);

        // --- Comandos do telefone ---
        comandos.Add("atender telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("atender o telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("atenda o telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("atenda telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("parar telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("parar o telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("pare telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("pare o telefone", () => ReceitaManager.PararTelefone());
        comandos.Add("atender celular", () => ReceitaManager.PararTelefone());
        comandos.Add("atender o celular", () => ReceitaManager.PararTelefone());
        comandos.Add("atenda o celular", () => ReceitaManager.PararTelefone());
        comandos.Add("atenda celular", () => ReceitaManager.PararTelefone());
        comandos.Add("parar celular", () => ReceitaManager.PararTelefone());
        comandos.Add("parar o celular", () => ReceitaManager.PararTelefone());
        comandos.Add("pare celular", () => ReceitaManager.PararTelefone());
        comandos.Add("pare o celular", () => ReceitaManager.PararTelefone());

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
        //ovo
        comandos.Add("pegar ovo", () => TentarPegar("ovo"));
        comandos.Add("pegar o ovo", () => TentarPegar("ovo"));
        comandos.Add("pegue ovo", () => TentarPegar("ovo"));
        comandos.Add("pegue o ovo", () => TentarPegar("ovo"));
        comandos.Add("pega ovo", () => TentarPegar("ovo"));
        comandos.Add("pega o ovo", () => TentarPegar("ovo"));

        //leite
        comandos.Add("pegar leite", () => TentarPegar("leite"));
        comandos.Add("pegar o leite", () => TentarPegar("leite"));
        comandos.Add("pegue leite", () => TentarPegar("leite"));
        comandos.Add("pegue o leite", () => TentarPegar("leite"));
        comandos.Add("pega leite", () => TentarPegar("leite"));
        comandos.Add("pega o leite", () => TentarPegar("leite"));
        comandos.Add("pegar a caixa de leite", () => TentarPegar("leite"));
        comandos.Add("pegue a caixa de leite", () => TentarPegar("leite"));
        comandos.Add("pega a caixa de leite", () => TentarPegar("leite"));


        //chocolate
        comandos.Add("pegar chocolate", () => TentarPegar("chocolate"));
        comandos.Add("pegar o chocolate", () => TentarPegar("chocolate"));
        comandos.Add("pegue chocolate", () => TentarPegar("chocolate"));
        comandos.Add("pegue o chocolate", () => TentarPegar("chocolate"));
        comandos.Add("pega chocolate", () => TentarPegar("chocolate"));
        comandos.Add("pega o chocolate", () => TentarPegar("chocolate"));

        //cenoura
        comandos.Add("pegar cenoura", () => TentarPegar("cenoura"));
        comandos.Add("pegar a cenoura", () => TentarPegar("cenoura"));
        comandos.Add("pegue cenoura", () => TentarPegar("cenoura"));
        comandos.Add("pegue a cenoura", () => TentarPegar("cenoura"));
        comandos.Add("pega cenoura", () => TentarPegar("cenoura"));
        comandos.Add("pega a cenoura", () => TentarPegar("cenoura"));


        //água de coco 
        comandos.Add("pegar água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pegar a água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pegue água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pegue a água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pega água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pega a água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pegar a caixa de água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pegue a caixa de água de coco", () => TentarPegar("água de coco"));
        comandos.Add("pega a caixa de água de coco", () => TentarPegar("água de coco"));

        //refrigerante de uva
        comandos.Add("pegar refrigerante de uva", () => TentarPegar("refrigerante de uva"));
        comandos.Add("pegar o refrigerante de uva", () => TentarPegar("refrigerante de uva"));
        comandos.Add("pegue refrigerante de uva", () => TentarPegar("refrigerante de uva"));
        comandos.Add("pegue o refrigerante de uva", () => TentarPegar("refrigerante de uva"));
        comandos.Add("pega refrigerante de uva", () => TentarPegar("refrigerante de uva"));
        comandos.Add("pega o refrigerante de uva", () => TentarPegar("refrigerante de uva"));

        //banana
        comandos.Add("pegar banana", () => TentarPegar("banana"));
        comandos.Add("pegar a banana", () => TentarPegar("banana"));
        comandos.Add("pegue banana", () => TentarPegar("banana"));
        comandos.Add("pegue a banana", () => TentarPegar("banana"));
        comandos.Add("pega banana", () => TentarPegar("banana"));
        comandos.Add("pega a banana", () => TentarPegar("banana"));


        //ma??
        comandos.Add("pegar maçã", () => TentarPegar("maçã"));
        comandos.Add("pegar a maçã", () => TentarPegar("maçã"));
        comandos.Add("pegue maçã", () => TentarPegar("maçã"));
        comandos.Add("pegue a maçã", () => TentarPegar("maçã"));
        comandos.Add("pega maçã", () => TentarPegar("maçã"));
        comandos.Add("pega a maçã", () => TentarPegar("maçã"));

        //manteiga
        comandos.Add("pegar manteiga", () => TentarPegar("manteiga"));
        comandos.Add("pegar a manteiga", () => TentarPegar("manteiga"));
        comandos.Add("pegue manteiga", () => TentarPegar("manteiga"));
        comandos.Add("pegue a manteiga", () => TentarPegar("manteiga"));
        comandos.Add("pega manteiga", () => TentarPegar("manteiga"));
        comandos.Add("pega a manteiga", () => TentarPegar("manteiga"));

        //morango
        comandos.Add("pegar morango", () => TentarPegar("morango"));
        comandos.Add("pegar o morango", () => TentarPegar("morango"));
        comandos.Add("pegue morango", () => TentarPegar("morango"));
        comandos.Add("pegue o morango", () => TentarPegar("morango"));
        comandos.Add("pega morango", () => TentarPegar("morango"));
        comandos.Add("pega o morango", () => TentarPegar("morango"));

        //abacaxi
        comandos.Add("pegar abacaxi", () => TentarPegar("abacaxi"));
        comandos.Add("pegar o abacaxi", () => TentarPegar("abacaxi"));
        comandos.Add("pegue abacaxi", () => TentarPegar("abacaxi"));
        comandos.Add("pegue o abacaxi", () => TentarPegar("abacaxi"));
        comandos.Add("pega abacaxi", () => TentarPegar("abacaxi"));
        comandos.Add("pega o abacaxi", () => TentarPegar("abacaxi"));

        //cereal
        comandos.Add("pegar cereal", () => TentarPegar("cereal"));
        comandos.Add("pegar o cereal", () => TentarPegar("cereal"));
        comandos.Add("pegue cereal", () => TentarPegar("cereal"));
        comandos.Add("pegue o cereal", () => TentarPegar("cereal"));
        comandos.Add("pega cereal", () => TentarPegar("cereal"));
        comandos.Add("pega o cereal", () => TentarPegar("cereal"));
        comandos.Add("pega a caixa de cereal", () => TentarPegar("cereal"));
        comandos.Add("pegue a caixa de cereal", () => TentarPegar("cereal"));
        comandos.Add("pegar a caixa de cereal", () => TentarPegar("cereal"));

        //arroz
        comandos.Add("pegar arroz", () => TentarPegar("arroz"));
        comandos.Add("pegar o arroz", () => TentarPegar("arroz"));
        comandos.Add("pegue arroz", () => TentarPegar("arroz"));
        comandos.Add("pegue o arroz", () => TentarPegar("arroz"));
        comandos.Add("pega arroz", () => TentarPegar("arroz"));
        comandos.Add("pega o arroz", () => TentarPegar("arroz"));
        comandos.Add("pega o pacote de arroz", () => TentarPegar("arroz"));
        comandos.Add("pegue o pacote a de arroz", () => TentarPegar("arroz"));
        comandos.Add("pegar o pacote de arroz", () => TentarPegar("arroz"));
        comandos.Add("pega o saco de arroz", () => TentarPegar("arroz"));
        comandos.Add("pegar o saco de arroz", () => TentarPegar("arroz"));
        comandos.Add("pegue o saco de arroz", () => TentarPegar("arroz"));

        //?gua
        comandos.Add("pegar água", () => TentarPegar("água"));
        comandos.Add("pegar a água", () => TentarPegar("água"));
        comandos.Add("pegue água", () => TentarPegar("água"));
        comandos.Add("pegue a água", () => TentarPegar("água"));
        comandos.Add("pega água", () => TentarPegar("água"));
        comandos.Add("pega a água", () => TentarPegar("água"));
        comandos.Add("pegar a garrafa de água", () => TentarPegar("água"));
        comandos.Add("pegue a garrafa de água", () => TentarPegar("água"));
        comandos.Add("pega a garrafa de água", () => TentarPegar("água"));

        //batata 
        comandos.Add("pegar batata", () => TentarPegar("batata"));
        comandos.Add("pegar a batata", () => TentarPegar("batata"));
        comandos.Add("pegue batata", () => TentarPegar("batata"));
        comandos.Add("pegue a batata", () => TentarPegar("batata"));
        comandos.Add("pega batata", () => TentarPegar("batata"));
        comandos.Add("pega a batata", () => TentarPegar("batata"));


        //cebola
        comandos.Add("pegar cebola", () => TentarPegar("cebola"));
        comandos.Add("pegar a cebola", () => TentarPegar("cebola"));
        comandos.Add("pegue cebola", () => TentarPegar("cebola"));
        comandos.Add("pegue a cebola", () => TentarPegar("cebola"));
        comandos.Add("pega cebola", () => TentarPegar("cebola"));
        comandos.Add("pega a cebola", () => TentarPegar("cebola"));

        //p?o
        comandos.Add("pegar pão", () => TentarPegar("pão"));
        comandos.Add("pegar o pão", () => TentarPegar("pão"));
        comandos.Add("pegue pão", () => TentarPegar("pão"));
        comandos.Add("pegue o pão", () => TentarPegar("pão"));
        comandos.Add("pega pão", () => TentarPegar("pão"));
        comandos.Add("pega o pão", () => TentarPegar("pão"));

        //alho
        comandos.Add("pegar alho", () => TentarPegar("alho"));
        comandos.Add("pegar o alho", () => TentarPegar("alho"));
        comandos.Add("pegue alho", () => TentarPegar("alho"));
        comandos.Add("pegue o alho", () => TentarPegar("alho"));
        comandos.Add("pega alho", () => TentarPegar("alho"));
        comandos.Add("pega o alho", () => TentarPegar("alho"));

        //limao
        comandos.Add("pegar limão", () => TentarPegar("limão"));
        comandos.Add("pegar o limão", () => TentarPegar("limão"));
        comandos.Add("pegue limão", () => TentarPegar("limão"));
        comandos.Add("pegue o limão", () => TentarPegar("limão"));
        comandos.Add("pega limão", () => TentarPegar("limão"));
        comandos.Add("pega o limão", () => TentarPegar("limão"));

        //peixe
        comandos.Add("pegar peixe", () => TentarPegar("peixe"));
        comandos.Add("pegar o peixe", () => TentarPegar("peixe"));
        comandos.Add("pegue peixe", () => TentarPegar("peixe"));
        comandos.Add("pegue o peixe", () => TentarPegar("peixe"));
        comandos.Add("pega peixe", () => TentarPegar("peixe"));
        comandos.Add("pega o peixe", () => TentarPegar("peixe"));

        //tomate
        comandos.Add("pegar tomate", () => TentarPegar("tomate"));
        comandos.Add("pegar o tomate", () => TentarPegar("tomate"));
        comandos.Add("pegue tomate", () => TentarPegar("tomate"));
        comandos.Add("pegue o tomate", () => TentarPegar("tomate"));
        comandos.Add("pega tomate", () => TentarPegar("tomate"));
        comandos.Add("pega o tomate", () => TentarPegar("tomate"));

        //milho
        comandos.Add("pegar milho", () => TentarPegar("milho"));
        comandos.Add("pegar o milho", () => TentarPegar("milho"));
        comandos.Add("pegue milho", () => TentarPegar("milho"));
        comandos.Add("pegue o milho", () => TentarPegar("milho"));
        comandos.Add("pega milho", () => TentarPegar("milho"));
        comandos.Add("pega o milho", () => TentarPegar("milho"));
        comandos.Add("pega a espiga de milho", () => TentarPegar("milho"));
        comandos.Add("pegar a espiga de milho", () => TentarPegar("milho"));
        comandos.Add("pegue a espiga de milho", () => TentarPegar("milho"));



        //óleo
        comandos.Add("pegar óleo", () => TentarPegar("óleo"));
        comandos.Add("pegar o óleo", () => TentarPegar("óleo"));
        comandos.Add("pegue óleo", () => TentarPegar("óleo"));
        comandos.Add("pegue o óleo", () => TentarPegar("óleo"));
        comandos.Add("pega óleo", () => TentarPegar("óleo"));
        comandos.Add("pega o óleo", () => TentarPegar("óleo"));
        comandos.Add("pegar a garrafa de óleo", () => TentarPegar("óleo"));
        comandos.Add("pegue a garrafa de óleo", () => TentarPegar("óleo"));
        comandos.Add("pega a garrafa de óleo", () => TentarPegar("óleo"));


        //farinha de trigo
        comandos.Add("pegar farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pegar a farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pegue farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pegue a farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pega farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pega a farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pega o saco de farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pegar o saco de farinha de trigo", () => TentarPegar("farinha de trigo"));
        comandos.Add("pegue o saco de farinha de trigo", () => TentarPegar("farinha de trigo"));


        //azeite
        comandos.Add("pegar azeite", () => TentarPegar("azeite"));
        comandos.Add("pegar o azeite", () => TentarPegar("azeite"));
        comandos.Add("pegue azeite", () => TentarPegar("azeite"));
        comandos.Add("pegue o azeite", () => TentarPegar("azeite"));
        comandos.Add("pega azeite", () => TentarPegar("azeite"));
        comandos.Add("pega o azeite", () => TentarPegar("azeite"));
        comandos.Add("pegar a garrafa de azeite", () => TentarPegar("azeite"));
        comandos.Add("pegue a garrafa de azeite", () => TentarPegar("azeite"));
        comandos.Add("pega a garrafa de azeite", () => TentarPegar("azeite"));


        // Comando de finalizar
        comandos.Add("finalizar receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("finalizar a receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("finaliza receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("finaliza a receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("terminar receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("terminar a receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("termina a receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("termina receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("encerrar receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("encerrar a receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("encerra receita", () => ReceitaManager.FinalizarReceita());
        comandos.Add("encerra a receita", () => ReceitaManager.FinalizarReceita());

        // --- Comandos para abrir informacoes ---
        comandos.Add("abrir informacoes", acaoAbrirInformacoes);
        comandos.Add("abrir informações", acaoAbrirInformacoes);
        comandos.Add("abrir as informacoes", acaoAbrirInformacoes);
        comandos.Add("abrir as informações", acaoAbrirInformacoes);
        comandos.Add("abra informacoes", acaoAbrirInformacoes);
        comandos.Add("abra informações", acaoAbrirInformacoes);
        comandos.Add("abre informacoes", acaoAbrirInformacoes);
        comandos.Add("abre informações", acaoAbrirInformacoes);

        // --- Comandos para fechar informacoes ---
        comandos.Add("fechar informacoes", acaoFecharInformacoes);
        comandos.Add("fechar informações", acaoFecharInformacoes);
        comandos.Add("fechar as informacoes", acaoFecharInformacoes);
        comandos.Add("fechar as informações", acaoFecharInformacoes);
        comandos.Add("feche informacoes", acaoFecharInformacoes);
        comandos.Add("feche informações", acaoFecharInformacoes);
        comandos.Add("fecha informacoes", acaoFecharInformacoes);
        comandos.Add("fecha informações", acaoFecharInformacoes);
        comandos.Add("informacoes fechar", acaoFecharInformacoes);
        comandos.Add("informações fechar", acaoFecharInformacoes);

      


        // --- Inicializacao do KeywordRecognizer ---
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

    bool EstaNaTelaFinal()
    {
        return canvasPontuacao != null && canvasPontuacao.activeSelf;
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
        Debug.Log("Você disse: '" + recognizedText + "'");

        if ((recognizedText == "próximo nível" || recognizedText == "proximo nivel") && !EstaNaTelaFinal())
        {
            Debug.Log("Comando ignorado: próximo nível só funciona na tela final.");
            return;
        }


        if (!PodeExecutarComandos())
        {
            
            if (recognizedText.Contains("fechar") || recognizedText.Contains("feche") || recognizedText.Contains("fecha"))
            {
                if (comandos.ContainsKey(recognizedText))
                    comandos[recognizedText].Invoke();
            }
            else
            {
                Debug.Log("Comandos bloqueados: painel aberto.");
            }

            return;
        }

        if (comandos.ContainsKey(recognizedText))
        {
            comandos[recognizedText].Invoke();
        }
    }
    bool PodePegarIngrediente(string nomeIngrediente)
    {
        Ingrediente[] todos = FindObjectsOfType<Ingrediente>();

        foreach (Ingrediente ing in todos)
        {
            if (ing.nome.ToLower() == nomeIngrediente.ToLower())
            {
                // Verifica câmera ativa
                if (ing.local == LocalIngrediente.Bancada && cameraPrincipal.enabled)
                    return true;

                if (ing.local == LocalIngrediente.Geladeira && cameraGeladeira.enabled)
                    return true;

                if (ing.local == LocalIngrediente.Armario && cameraArmario.enabled)
                    return true;

                Debug.Log($"Você não está na câmera correta para pegar {nomeIngrediente}");
                return false;
            }
        }

        Debug.LogWarning($"Ingrediente {nomeIngrediente} não encontrado!");
        return false;
    }
    void TentarPegar(string nome)
    {
        if (PodePegarIngrediente(nome))
        {
            ReceitaManager.JogadorPegou(nome);
        }
    }
    bool PodeExecutarComandos()
    {
        if (painelReceita != null && painelReceita.activeSelf)
            return false;

        if (painelInformacoes != null && painelInformacoes.activeSelf)
            return false;

        return true;
    }
    void ProximoNivel()
    {
        Debug.Log("Indo para o próximo nível...");

        if (ReceitaManager != null)
        {
            ReceitaManager.ProximoNivel();
        }
        else
        {
            
            int cenaAtual = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(cenaAtual + 1);
        }
    }
}