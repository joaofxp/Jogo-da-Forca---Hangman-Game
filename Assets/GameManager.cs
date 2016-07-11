using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //Texto na tela
    public Text gameText;
    public Text logText;
    public RawImage blinkImage;
    public Text finalText;
    public Text tentativasText;
    public GameObject retryButton;
    public GameObject tecladoJogo;
    public int tentativas;
    //Lista de palavras
    public string[] palavras;
    public List<Image> stickImages;
    string palavraAtual;
    char[] letrasPalavraAtual;
    
    void Awake()
    {
        //Limpa o texto da tela
        gameText.text = "";
        //Numero de Tentativas;
        //Pega uma palavra aleatoria da lista de palavras
        palavraAtual = palavras[Random.Range(0, palavras.Length)];
        tentativas = 5;
        tentativasText.text = "Tentativas: " + tentativas.ToString();
        letrasPalavraAtual = palavraAtual.ToCharArray();

        //Converter a lista de letras em hifens
        for (int i = 0; i < letrasPalavraAtual.Length; i++)
        {
            letrasPalavraAtual[i] = '-';
        }

        //Colocar a lista de letras na tela
        gameText.text = new string(letrasPalavraAtual);

        Debug.Log("A palavra atual é: " + palavraAtual); //Palavra Atual
        Debug.Log(letrasPalavraAtual.Length); //Tamanho da lista de letras
    }

    //Checar se o char existe na palavra atual, fazer as alterações necessárias e atualizar o texto
    public void CheckLetter(string Letter)
    {
        //Converter para char o string
        char CharLetter = Letter.ToCharArray()[0];
        //Se o char existir
        if (palavraAtual.Contains(Letter))
        {
            //Contagem
            int count = 0;
            //Verifica quantas vezes o char aparece na palavraAtual, para evitar que só altere uma vez, caso a palavra possua RR por exemplo
            foreach (char Char in palavraAtual)
            {
                //Adiciona o char na lista de letras passando a contagem como índice.
                if (Char == CharLetter)
                {
                    letrasPalavraAtual[count] = CharLetter;
                }
                count++;
            }
            //Atualizacao do texto na tela
            gameText.text = new string(letrasPalavraAtual);
            if (gameText.text == palavraAtual)
            {
                //finalText.gameObject.SetActive(true);
                finalText.text = "Você Ganhou!";
                retryButton.SetActive(true);
                tecladoJogo.SetActive(false);
            }
        } else
        {
            StopAllCoroutines();
            StartCoroutine(CharLetterWrong());
        }
        logText.text = logText.text + CharLetter + " ";
    }

    IEnumerator CharLetterWrong()
    {
        do
        {
            Color color = blinkImage.color;
            color.a += 0.1f;
            blinkImage.color = color;
            yield return new WaitForSeconds(0.02f);
        } while (blinkImage.color.a < 0.5f);
        do
        {
            Color color = blinkImage.color;
            color.a -= 0.1f;
            blinkImage.color = color;
            yield return new WaitForSeconds(0.02f);
        } while (blinkImage.color.a > 0);

        if (tentativas > 0)
        {
            tentativas--;
            stickImages[0].enabled = false;
            stickImages.RemoveAt(0);
            tentativasText.text = "Tentativas: "+ tentativas.ToString();
            if (tentativas == 0)
            {
                finalText.text = "Você Perdeu!";
                retryButton.SetActive(true);
                tecladoJogo.SetActive(false);
                gameText.text = palavraAtual;
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
