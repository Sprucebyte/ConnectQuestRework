using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject mapContainer;
    [SerializeField] GameObject levelContainer;
    [SerializeField] GameObject characterContainer;
    [SerializeField] GameObject boardContainer;




    enum State
    {
        Map,
        Fight,
        Dialouge,
        Paused,
        Waiting,
        Cutscene,
    }

    [SerializeField] State state = State.Map;


    private void Start()
    {
        
    }

    private void Update()
    {
        switch (state)
        {
            case State.Map:
                mapContainer.SetActive(true);
                levelContainer.SetActive(false);
                boardContainer.SetActive(false);
            break;

            case State.Fight:
                mapContainer.SetActive(false);
                levelContainer.SetActive(true);
                boardContainer.SetActive(true);    
            break;

            case State.Dialouge:

            break;

            case State.Paused:

            break;
        }

    }



    /*
    List<Team> teams = new List<Team>();
    Team currentTeam;
    int currentTeamValue; 


    private void Start()
    {
        currentTeamValue = 0;
        currentTeam = teams[currentTeamValue];
    }
    private void Update()
    {

    }

    public void NextTeam()
    {
        if (currentTeamValue < teams.Count) {
            currentTeamValue++;
        }
        else currentTeamValue = 0;
        currentTeam = teams[currentTeamValue];
    }

    public void NextCharacter()
    {
        currentTeam.NextCharacter();
    }
    */

}
