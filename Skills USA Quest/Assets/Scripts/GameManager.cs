using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MatchSettings matchSettings;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("There is more than one GameManager in the scene!");
        }

        else
        {
            instance = this;
        }
    }

    #region Player Tracking
    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();

    public static void RegisterPlayer(string netID, PlayerManager playerManager)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, playerManager);
        playerManager.transform.name = playerID;
    }

    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static PlayerManager GetPlayer(string playerID)
    {
        if (!players.ContainsKey(playerID))
        {
            Debug.Log("The key " + playerID + " does not exist!");
            return null;
        }
        return players[playerID];
    }

    /*
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,200, 200,500));
        GUILayout.BeginVertical();

        foreach(string player in players.Keys)
        {
            GUILayout.Label(player + " " + players[player].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    */
    #endregion
}
