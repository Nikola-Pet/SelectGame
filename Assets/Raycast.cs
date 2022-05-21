using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    public Button btnRed;
    public Button btnGreen;
    public Button btnBlue;

    public GameObject kocka;
    public GameObject lopta;
    public GameObject cilindar;



    RaycastHit hitInfo;

    private Color oldColor;

    private Renderer renderer;

  

    public List<Tuple<string, Vector3>> savedPositions = new List<Tuple<string, Vector3>>();

    Vector3 lastPosition;


    bool hit;
    Vector3 worldMousePosition;



    private void Start()
    {
        SavePositions(kocka.name, kocka.transform.position);
        SavePositions(lopta.name, lopta.transform.position);
        SavePositions(cilindar.name, cilindar.transform.position);
    }


    private void Update()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)); // pozicija misa 

        if (Input.GetKeyDown(KeyCode.Mouse0) && hit) //ako hita u vec hitan objecakt 
        {
            renderer.material.color = oldColor; // vraca na staro
            hit = !hit;
        }


        if (Input.GetKeyDown(KeyCode.Mouse0) && !hit) // hitanje u ne uhitan objekat 
        {
            
            hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo); // info iz uhitanog obj ide u hitInfo
            if (hit)
            {
                if (hitInfo.collider.tag == "Player")
                {
                    renderer = hitInfo.transform.gameObject.GetComponent<Renderer>();

                    oldColor = renderer.material.color;                 //izmena parametra
                    renderer.material.color = Color.cyan;
                }
                else
                {
                    hit = !hit;
                }

            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0)&&hit)
        {


            SavePositions(hitInfo.transform.name, hitInfo.transform.position); // cuva lokaciju kad otpustimo taster
        }
        if (Input.GetKey(KeyCode.Mouse0) && hit)
        {

            hitInfo.transform.position = worldMousePosition; // obj prati kursor
        }

        if (Input.GetKeyDown(KeyCode.T) && hit)
        {
            var sav = savedPositions.FindAll(x => x.Item1 == hitInfo.transform.name);
            int count = sav.Count;

            if (count > 1)
            {
                var lastPosition = sav[count - 2];
                hitInfo.transform.position = lastPosition.Item2;
                savedPositions.Remove(sav[count - 1]);
            }
            else
            {

                hitInfo.transform.position = sav[0].Item2;
            }

        }

        //-------------------------------------------

        if (hit)
        {
            
            btnRed.onClick.AddListener(() => ChangeColor(btnRed.gameObject.GetComponent<Button>().colors.normalColor));
            btnBlue.onClick.AddListener(() => ChangeColor(btnBlue.gameObject.GetComponent<Button>().colors.normalColor));
            btnGreen.onClick.AddListener(() => ChangeColor(btnGreen.gameObject.GetComponent<Button>().colors.normalColor));

        }

    }

    public void ChangeColor(Color color)
    {
        renderer.material.color = color;
    }

   
    void SavePositions(string objName, Vector3 vector3)
    {
        savedPositions.Add(new Tuple<string, Vector3>(objName, vector3));

    }

}
