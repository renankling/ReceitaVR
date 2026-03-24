using UnityEngine;

public class PainelInicial : MonoBehaviour
{
    public GameObject painel;

    void Start()
    {
        painel.SetActive(true); 
    }

    public void FecharPainel()
    {
        painel.SetActive(false);
    }
}