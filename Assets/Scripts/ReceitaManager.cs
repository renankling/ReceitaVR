using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReceitaManager : MonoBehaviour
{
    private List<string> receitaCorreta;     
    private List<string> ingredientesPegos; 
    private int pontos;

    private List<string> possiveisExtras = new List<string> { "chocolate", "banana", "maçã", "morango", "cenoura" };

    public TextMeshProUGUI receitaText;

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

        
        List<string> baseIngredientes = new List<string> { "ovo", "leite" };

        
        List<string> escolhidos = new List<string>();
        while (escolhidos.Count < 2)
        {
            string ing = possiveisExtras[Random.Range(0, possiveisExtras.Count)];
            if (!escolhidos.Contains(ing))
            {
                escolhidos.Add(ing);
            }
        }

        
        receitaCorreta.AddRange(baseIngredientes);
        receitaCorreta.AddRange(escolhidos);

        
        for (int i = 0; i < receitaCorreta.Count; i++)
        {
            string temp = receitaCorreta[i];
            int randomIndex = Random.Range(i, receitaCorreta.Count);
            receitaCorreta[i] = receitaCorreta[randomIndex];
            receitaCorreta[randomIndex] = temp;
        }

        Debug.Log("Receita gerada: " + string.Join(", ", receitaCorreta));
    }

    private void AtualizarUI()
    {
        receitaText.text = "Receita:\n";
        for (int i = 0; i < receitaCorreta.Count; i++)
        {
            receitaText.text += (i + 1) + ". " + receitaCorreta[i] + "\n";
        }
    }
    public void JogadorPegou(string ingrediente)
    {
        ingredientesPegos.Add(ingrediente);
        AvaliarIngrediente(ingrediente, ingredientesPegos.Count - 1);
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
        Debug.Log($" Receita finalizada! Pontuação: {pontos}");
    }
    
    private void DesativarIngrediente(string nomeIngrediente)
    {
        Ingrediente[] todos = FindObjectsOfType<Ingrediente>();
        foreach (Ingrediente ing in todos)
        {
            if (ing.nome.ToLower() == nomeIngrediente.ToLower())
            {
                ing.gameObject.SetActive(false); // desativa o objeto
                Debug.Log($"Ingrediente {nomeIngrediente} desativado na cena.");
                return;
            }
        }
        Debug.LogWarning($"Ingrediente {nomeIngrediente} não encontrado na cena.");
    }
    
}
