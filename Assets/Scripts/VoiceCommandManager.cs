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
        comandos.Add("pegar ovo", () => ReceitaManager.JogadorPegou("ovo"));
        comandos.Add("pegar o ovo", () => ReceitaManager.JogadorPegou("ovo"));
        comandos.Add("pegue ovo", () => ReceitaManager.JogadorPegou("ovo"));
        comandos.Add("pegue o ovo", () => ReceitaManager.JogadorPegou("ovo"));
        comandos.Add("pega ovo", () => ReceitaManager.JogadorPegou("ovo"));
        comandos.Add("pega o ovo", () => ReceitaManager.JogadorPegou("ovo"));

        //leite
        comandos.Add("pegar leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pegar o leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pegue leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pegue o leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pega leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pega o leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pegar a caixa de leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pegue a caixa de leite", () => ReceitaManager.JogadorPegou("leite"));
        comandos.Add("pega a caixa de leite", () => ReceitaManager.JogadorPegou("leite"));


        //chocolate
        comandos.Add("pegar chocolate", () => ReceitaManager.JogadorPegou("chocolate"));
        comandos.Add("pegar o chocolate", () => ReceitaManager.JogadorPegou("chocolate"));
        comandos.Add("pegue chocolate", () => ReceitaManager.JogadorPegou("chocolate"));
        comandos.Add("pegue o chocolate", () => ReceitaManager.JogadorPegou("chocolate"));
        comandos.Add("pega chocolate", () => ReceitaManager.JogadorPegou("chocolate"));
        comandos.Add("pega o chocolate", () => ReceitaManager.JogadorPegou("chocolate"));

        //cenoura
        comandos.Add("pegar cenoura", () => ReceitaManager.JogadorPegou("cenoura"));
        comandos.Add("pegar a cenoura", () => ReceitaManager.JogadorPegou("cenoura"));
        comandos.Add("pegue cenoura", () => ReceitaManager.JogadorPegou("cenoura"));
        comandos.Add("pegue a cenoura", () => ReceitaManager.JogadorPegou("cenoura"));
        comandos.Add("pega cenoura", () => ReceitaManager.JogadorPegou("cenoura"));
        comandos.Add("pega a cenoura", () => ReceitaManager.JogadorPegou("cenoura"));


        //água de coco 
        comandos.Add("pegar água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pegar a água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pegue água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pegue a água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pega água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pega a água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pegar a caixa de água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pegue a caixa de água de coco", () => ReceitaManager.JogadorPegou("água de coco"));
        comandos.Add("pega a caixa de água de coco", () => ReceitaManager.JogadorPegou("água de coco"));

        //refrigerante de uva
        comandos.Add("pegar refrigerante de uva", () => ReceitaManager.JogadorPegou("refrigerante de uva"));
        comandos.Add("pegar o refrigerante de uva", () => ReceitaManager.JogadorPegou("refrigerante de uva"));
        comandos.Add("pegue refrigerante de uva", () => ReceitaManager.JogadorPegou("refrigerante de uva"));
        comandos.Add("pegue o refrigerante de uva", () => ReceitaManager.JogadorPegou("refrigerante de uva"));
        comandos.Add("pega refrigerante de uva", () => ReceitaManager.JogadorPegou("refrigerante de uva"));
        comandos.Add("pega o refrigerante de uva", () => ReceitaManager.JogadorPegou("refrigerante de uva"));

        //banana
        comandos.Add("pegar banana", () => ReceitaManager.JogadorPegou("banana"));
        comandos.Add("pegar a banana", () => ReceitaManager.JogadorPegou("banana"));
        comandos.Add("pegue banana", () => ReceitaManager.JogadorPegou("banana"));
        comandos.Add("pegue a banana", () => ReceitaManager.JogadorPegou("banana"));
        comandos.Add("pega banana", () => ReceitaManager.JogadorPegou("banana"));
        comandos.Add("pega a banana", () => ReceitaManager.JogadorPegou("banana"));


        //ma??
        comandos.Add("pegar maçã", () => ReceitaManager.JogadorPegou("maçã"));
        comandos.Add("pegar a maçã", () => ReceitaManager.JogadorPegou("maçã"));
        comandos.Add("pegue maçã", () => ReceitaManager.JogadorPegou("maçã"));
        comandos.Add("pegue a maçã", () => ReceitaManager.JogadorPegou("maçã"));
        comandos.Add("pega maçã", () => ReceitaManager.JogadorPegou("maçã"));
        comandos.Add("pega a maçã", () => ReceitaManager.JogadorPegou("maçã"));

        //manteiga
        comandos.Add("pegar manteiga", () => ReceitaManager.JogadorPegou("manteiga"));
        comandos.Add("pegar a manteiga", () => ReceitaManager.JogadorPegou("manteiga"));
        comandos.Add("pegue manteiga", () => ReceitaManager.JogadorPegou("manteiga"));
        comandos.Add("pegue a manteiga", () => ReceitaManager.JogadorPegou("manteiga"));
        comandos.Add("pega manteiga", () => ReceitaManager.JogadorPegou("manteiga"));
        comandos.Add("pega a manteiga", () => ReceitaManager.JogadorPegou("manteiga"));

        //morango
        comandos.Add("pegar morango", () => ReceitaManager.JogadorPegou("morango"));
        comandos.Add("pegar o morango", () => ReceitaManager.JogadorPegou("morango"));
        comandos.Add("pegue morango", () => ReceitaManager.JogadorPegou("morango"));
        comandos.Add("pegue o morango", () => ReceitaManager.JogadorPegou("morango"));
        comandos.Add("pega morango", () => ReceitaManager.JogadorPegou("morango"));
        comandos.Add("pega o morango", () => ReceitaManager.JogadorPegou("morango"));

        //abacaxi
        comandos.Add("pegar abacaxi", () => ReceitaManager.JogadorPegou("abacaxi"));
        comandos.Add("pegar o abacaxi", () => ReceitaManager.JogadorPegou("abacaxi"));
        comandos.Add("pegue abacaxi", () => ReceitaManager.JogadorPegou("abacaxi"));
        comandos.Add("pegue o abacaxi", () => ReceitaManager.JogadorPegou("abacaxi"));
        comandos.Add("pega abacaxi", () => ReceitaManager.JogadorPegou("abacaxi"));
        comandos.Add("pega o abacaxi", () => ReceitaManager.JogadorPegou("abacaxi"));

        //cereal
        comandos.Add("pegar cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pegar o cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pegue cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pegue o cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pega cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pega o cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pega a caixa de cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pegue a caixa de cereal", () => ReceitaManager.JogadorPegou("cereal"));
        comandos.Add("pegar a caixa de cereal", () => ReceitaManager.JogadorPegou("cereal"));

        //arroz
        comandos.Add("pegar arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pegar o arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pegue arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pegue o arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pega arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pega o arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pega o pacote de arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pegue o pacote a de arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pegar o pacote de arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pega o saco de arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pegar o saco de arroz", () => ReceitaManager.JogadorPegou("arroz"));
        comandos.Add("pegue o saco de arroz", () => ReceitaManager.JogadorPegou("arroz"));

        //?gua
        comandos.Add("pegar água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pegar a água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pegue água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pegue a água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pega água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pega a água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pegar a garrafa de água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pegue a garrafa de água", () => ReceitaManager.JogadorPegou("água"));
        comandos.Add("pega a garrafa de água", () => ReceitaManager.JogadorPegou("água"));

        //batata 
        comandos.Add("pegar batata", () => ReceitaManager.JogadorPegou("batata"));
        comandos.Add("pegar a batata", () => ReceitaManager.JogadorPegou("batata"));
        comandos.Add("pegue batata", () => ReceitaManager.JogadorPegou("batata"));
        comandos.Add("pegue a batata", () => ReceitaManager.JogadorPegou("batata"));
        comandos.Add("pega batata", () => ReceitaManager.JogadorPegou("batata"));
        comandos.Add("pega a batata", () => ReceitaManager.JogadorPegou("batata"));


        //cebola
        comandos.Add("pegar cebola", () => ReceitaManager.JogadorPegou("cebola"));
        comandos.Add("pegar a cebola", () => ReceitaManager.JogadorPegou("cebola"));
        comandos.Add("pegue cebola", () => ReceitaManager.JogadorPegou("cebola"));
        comandos.Add("pegue a cebola", () => ReceitaManager.JogadorPegou("cebola"));
        comandos.Add("pega cebola", () => ReceitaManager.JogadorPegou("cebola"));
        comandos.Add("pega a cebola", () => ReceitaManager.JogadorPegou("cebola"));

        //p?o
        comandos.Add("pegar pão", () => ReceitaManager.JogadorPegou("pão"));
        comandos.Add("pegar o pão", () => ReceitaManager.JogadorPegou("pão"));
        comandos.Add("pegue pão", () => ReceitaManager.JogadorPegou("pão"));
        comandos.Add("pegue o pão", () => ReceitaManager.JogadorPegou("pão"));
        comandos.Add("pega pão", () => ReceitaManager.JogadorPegou("pão"));
        comandos.Add("pega o pão", () => ReceitaManager.JogadorPegou("pão"));

        //alho
        comandos.Add("pegar alho", () => ReceitaManager.JogadorPegou("alho"));
        comandos.Add("pegar o alho", () => ReceitaManager.JogadorPegou("alho"));
        comandos.Add("pegue alho", () => ReceitaManager.JogadorPegou("alho"));
        comandos.Add("pegue o alho", () => ReceitaManager.JogadorPegou("alho"));
        comandos.Add("pega alho", () => ReceitaManager.JogadorPegou("alho"));
        comandos.Add("pega o alho", () => ReceitaManager.JogadorPegou("alho"));

        //limao
        comandos.Add("pegar limão", () => ReceitaManager.JogadorPegou("limão"));
        comandos.Add("pegar o limão", () => ReceitaManager.JogadorPegou("limão"));
        comandos.Add("pegue limão", () => ReceitaManager.JogadorPegou("limão"));
        comandos.Add("pegue o limão", () => ReceitaManager.JogadorPegou("limão"));
        comandos.Add("pega limão", () => ReceitaManager.JogadorPegou("limão"));
        comandos.Add("pega o limão", () => ReceitaManager.JogadorPegou("limão"));

        //peixe
        comandos.Add("pegar peixe", () => ReceitaManager.JogadorPegou("peixe"));
        comandos.Add("pegar o peixe", () => ReceitaManager.JogadorPegou("peixe"));
        comandos.Add("pegue peixe", () => ReceitaManager.JogadorPegou("peixe"));
        comandos.Add("pegue o peixe", () => ReceitaManager.JogadorPegou("peixe"));
        comandos.Add("pega peixe", () => ReceitaManager.JogadorPegou("peixe"));
        comandos.Add("pega o peixe", () => ReceitaManager.JogadorPegou("peixe"));

        //tomate
        comandos.Add("pegar tomate", () => ReceitaManager.JogadorPegou("tomate"));
        comandos.Add("pegar o tomate", () => ReceitaManager.JogadorPegou("tomate"));
        comandos.Add("pegue tomate", () => ReceitaManager.JogadorPegou("tomate"));
        comandos.Add("pegue o tomate", () => ReceitaManager.JogadorPegou("tomate"));
        comandos.Add("pega tomate", () => ReceitaManager.JogadorPegou("tomate"));
        comandos.Add("pega o tomate", () => ReceitaManager.JogadorPegou("tomate"));

        //milho
        comandos.Add("pegar milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pegar o milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pegue milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pegue o milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pega milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pega o milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pega a espiga de milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pegar a espiga de milho", () => ReceitaManager.JogadorPegou("milho"));
        comandos.Add("pegue a espiga de milho", () => ReceitaManager.JogadorPegou("milho"));



        //óleo
        comandos.Add("pegar óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pegar o óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pegue óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pegue o óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pega óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pega o óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pegar a garrafa de óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pegue a garrafa de óleo", () => ReceitaManager.JogadorPegou("óleo"));
        comandos.Add("pega a garrafa de óleo", () => ReceitaManager.JogadorPegou("óleo"));


        //farinha de trigo
        comandos.Add("pegar farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pegar a farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pegue farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pegue a farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pega farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pega a farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pega o saco de farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pegar o saco de farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));
        comandos.Add("pegue o saco de farinha de trigo", () => ReceitaManager.JogadorPegou("farinha de trigo"));


        //azeite
        comandos.Add("pegar azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pegar o azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pegue azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pegue o azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pega azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pega o azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pegar a garrafa de azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pegue a garrafa de azeite", () => ReceitaManager.JogadorPegou("azeite"));
        comandos.Add("pega a garrafa de azeite", () => ReceitaManager.JogadorPegou("azeite"));


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
    bool PodeExecutarComandos()
    {
        if (painelReceita != null && painelReceita.activeSelf)
            return false;

        if (painelInformacoes != null && painelInformacoes.activeSelf)
            return false;

        return true;
    }
}