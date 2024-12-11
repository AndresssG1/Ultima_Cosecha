using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuGameOver;
    public GameObject menuFinal;
    public GameObject flecha;
    public List<GameObject> flechas;

    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoGranos;

    public bool start = false; 
    public bool gameOver = false; 
    public bool final = false;
    public int vidas = 10; 
    public int granos = 0;

    public Renderer fondo;
    public float velocidad;

    void Start()
    {
        start = false;
        menuPrincipal.SetActive(true); 
        menuGameOver.SetActive(false);
        menuFinal.SetActive(false);

        GenerarFlechas();

        ActualizarUI();
    }

    void Update()
    {
        if (!start) 
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                start = true;
                vidas = 10;
                granos = 0;
                ActualizarUI();
            }
        }

        if (start && gameOver) 
        {
            menuGameOver.SetActive(true);
            if (Input.GetKeyDown(KeyCode.X))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }

        if (start && !gameOver && vidas > 0) 
        {
            menuPrincipal.SetActive(false);
            fondo.material.mainTextureOffset += new Vector2(0.002f, 0) * Time.deltaTime;
        }

        if (vidas <= 0)
        {
            gameOver = true;
        }

        if (final)
        {
            menuFinal.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                menuFinal.SetActive(false);
                vidas = 10;
                granos = 0;
                final = false;
            }
        }  

        MoverFlechas();
    }

    public void ActualizarUI()
    {
        textoVidas.text = "Vidas: " + vidas; 
        textoGranos.text = "Granos: " + granos; 
    }

    private void GenerarFlechas()
    {
        for (int i = 0; i < 4; i++)
        {
            float randomX = Random.Range(55f, 75f);
            float randomY = (randomX < 20) ? 0.5f : 1f;
            GameObject nuevaFlecha = Instantiate(flecha, new Vector3(randomX, randomY, 0), Quaternion.identity);
            flechas.Add(nuevaFlecha); 
        }
    }

    private void MoverFlechas()
    {
        foreach (GameObject flecha in flechas)
        {
            flecha.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * velocidad;
        }
    }
}
