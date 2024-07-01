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
}
