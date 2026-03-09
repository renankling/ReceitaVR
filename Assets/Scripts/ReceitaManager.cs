using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReceitaManager : MonoBehaviour
{
    private List<string> receitaCorreta;     
    private List<string> ingredientesPegos; 
    private int pontos;

    private List<string> possiveisExtras = new List<string> { "banana", "maçã", "morango" };

    public TextMeshProUGUI receitaText;
    private List<string> linhasUI = new List<string>();
    public GameObject canvasPontuacao;
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoBotaoProximoNivel;
    public AudioSource telefoneAudio;
    public AudioClip telefoneClip;

    void Start()
    {
        GerarReceita();
        AtualizarUI();

    }

    void GerarReceita()
    {
        receitaCorreta = new List<string>();
        ingredientesPegos = new List<string>();
        pontos = 0;

        
        List<string> todosIngredientes = new List<string> {
        "ovo","leite","chocolate","cenoura","água de coco","refrigerante de uva","banana",
        "maçã","manteiga","morango","abacaxi","cereal","arroz","batata","cebola",
        "pão","alho","limão","peixe","tomate","milho","óleo","farinha de trigo","azeite"
    };

        List<string> fixos = new List<string>();
        int totalIngredientes = 0;

        
        string nomeCena = SceneManager.GetActiveScene().name;

      
        if (nomeCena == "Cena2") 
        {
            fixos = new List<string> { "pão", "manteiga" };
            possiveisExtras = new List<string> { "leite", "ovo", "banana", "morango", "cereal", "maçã", "água de coco", "limão" };
            totalIngredientes = 6;
        }
        else if (nomeCena == "Cena3") 
        {
            fixos = new List<string> { "arroz", "batata" };
            possiveisExtras = new List<string> { "cebola", "tomate", "alho", "azeite", "peixe", "limão" };
            totalIngredientes = 8;
        }
        else if (nomeCena == "Cena4")
        {
            fixos = new List<string> { "peixe", "arroz" };
            possiveisExtras = new List<string> { "batata", "cebola", "alho", "azeite", "tomate", "milho", "limão", "farinha de trigo", "manteiga", "óleo" };
            totalIngredientes = 10;
        }
        else if (nomeCena == "Cena5") 
        {
            fixos = new List<string>();
            possiveisExtras = new List<string> { "leite", "banana", "morango", "maçã", "abacaxi", "cereal", "água de coco", "pão", "manteiga", "limão", "refrigerante de uva", "ovo" };
            totalIngredientes = 10;
        }
        else 
        {
            fixos = new List<string> { "ovo", "leite" };
            possiveisExtras = new List<string> { "limão", "abacaxi", "banana", "maçã", "morango" };
            totalIngredientes = 4;
        }

    
        int quantidadeRandom = Mathf.Max(0, totalIngredientes - fixos.Count);

       
        quantidadeRandom = Mathf.Min(quantidadeRandom, possiveisExtras.Count);

     
        List<string> escolhidos = new List<string>();
        while (escolhidos.Count < quantidadeRandom)
        {
            string ing = possiveisExtras[Random.Range(0, possiveisExtras.Count)];
            if (!escolhidos.Contains(ing))
            {
                escolhidos.Add(ing);
            }
        }

    
        receitaCorreta.AddRange(fixos);
        receitaCorreta.AddRange(escolhidos);

        for (int i = 0; i < receitaCorreta.Count; i++)
        {
            string temp = receitaCorreta[i];
            int randomIndex = Random.Range(i, receitaCorreta.Count);
            receitaCorreta[i] = receitaCorreta[randomIndex];
            receitaCorreta[randomIndex] = temp;
        }

        AtualizarUI();
        Debug.Log($"Receita gerada ({nomeCena}): {string.Join(", ", receitaCorreta)}");
    }



    private void AtualizarUI()
    {
        linhasUI.Clear();

        for (int i = 0; i < receitaCorreta.Count; i++)
        {
            linhasUI.Add($"{i + 1}. {receitaCorreta[i]}");
        }

        RedesenharUI();
    }

    private void RedesenharUI()
    {
        receitaText.text = "";
        foreach (string linha in linhasUI)
        {
            receitaText.text += linha + "\n";
        }
    }

    private void RiscarIngrediente(string ingrediente)
    {
        ingrediente = ingrediente.ToLower();

        for (int i = 0; i < linhasUI.Count; i++)
        {

            if (linhasUI[i].ToLower().Contains(ingrediente) && !linhasUI[i].Contains("<s>"))
            {
                string original = linhasUI[i];
                string riscado = original.Replace(ingrediente, $"<s>{ingrediente}</s>");
                linhasUI[i] = riscado;
                break;
            }
        }

        RedesenharUI();
    }


    public void JogadorPegou(string ingrediente)
    {
        ingredientesPegos.Add(ingrediente);
        AvaliarIngrediente(ingrediente, ingredientesPegos.Count - 1);
        RiscarIngrediente(ingrediente);

        if (SceneManager.GetActiveScene().name == "Cena5" && ingredientesPegos.Count == 5)
        {
            TocarTelefone();
        }

        if (ingredientesPegos.Count >= receitaCorreta.Count)
        {
            FinalizarReceita();
        }
    }

    private void AvaliarIngrediente(string ingrediente, int posicao)
    {
        if (posicao < receitaCorreta.Count)
        {
            if (receitaCorreta[posicao] == ingrediente)
            {
                pontos += 3;
                Debug.Log($" Pegou {ingrediente} na ordem certa (+3 pontos)");
                DesativarIngrediente(ingrediente);
            }
            else if (receitaCorreta.Contains(ingrediente))
            {
                pontos += 1;
                DesativarIngrediente(ingrediente);
                Debug.Log($" Pegou {ingrediente}, mas fora de ordem (+1 ponto)");

            }
            else
            {
                DesativarIngrediente(ingrediente);
                Debug.Log($" {ingrediente} não faz parte da receita (0 pontos)");
            }
        }
        else
        {
            if (receitaCorreta.Contains(ingrediente))
            {
                pontos += 1;
                Debug.Log($" Pegou {ingrediente}, mas fora de ordem (+1 ponto)");
                DesativarIngrediente(ingrediente);
            }
            else
            {
                Debug.Log($" {ingrediente} extra além da receita (0 pontos)");
                DesativarIngrediente(ingrediente);
            }
            
        }
    }



    public void FinalizarReceita()
    {
        Debug.Log($"Receita finalizada! Pontuação: {pontos}");

        canvasPontuacao.SetActive(true);
        textoPontuacao.text = "Pontuação Final\n" + pontos + " pontos";

        int cenaAtual = SceneManager.GetActiveScene().buildIndex;

        if (cenaAtual == 4)
        {
            textoBotaoProximoNivel.text = "Jogar Novamente";
        }
        else
        {
            textoBotaoProximoNivel.text = "Próximo Nível";
        }
    }

    private void DesativarIngrediente(string nomeIngrediente)
    {
        Ingrediente[] todos = FindObjectsOfType<Ingrediente>();
        foreach (Ingrediente ing in todos)
        {
            if (ing.nome.ToLower() == nomeIngrediente.ToLower())
            {
                ing.gameObject.SetActive(false); 
                Debug.Log($"Ingrediente {nomeIngrediente} desativado na cena.");
                return;
            }
        }
        Debug.LogWarning($"Ingrediente {nomeIngrediente} não encontrado na cena.");
    }

    public void ProximoNivel()
    {
        int cenaAtual = SceneManager.GetActiveScene().buildIndex;

        if (cenaAtual == 4)
        {

            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(cenaAtual + 1);
        }
    }

    void TocarTelefone()
    {
        telefoneAudio.PlayOneShot(telefoneClip);
    }

}
