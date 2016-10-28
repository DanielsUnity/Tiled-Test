using UnityEngine;
using System.Collections;

public class SpawnPointManager : MonoBehaviour {



    public void ManageSpawnPoint(ExitPointID lastExitPointID)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {

            if (lastExitPointID == ExitPointID.NotSpecified)
            {
                GameObject defaultSpawnPoint = GameObject.FindGameObjectWithTag("Default Spawn Point");
                if (defaultSpawnPoint != null)
                {
                    player.transform.position = defaultSpawnPoint.transform.position;
                    return;
                }
                else
                {
                    Debug.LogWarning("Default spawn point missing in current scene!");
                }
                return;
            }

            ExitPoint[] exitPoints = FindObjectsOfType<ExitPoint>();

            foreach (ExitPoint exitPoint in exitPoints)
            {
                if (lastExitPointID >= ExitPointID.SpecialEnterNorth1)
                {
                    if (exitPoint.exitPointID == lastExitPointID)
                    {
                        SpawnInPlace(player, exitPoint);
                        return;
                    }
                }
                else if (lastExitPointID <= ExitPointID.EnterEast4)
                {
                    if (exitPoint.exitPointID == lastExitPointID + 8)
                    {
                        SpawnInPlace(player, exitPoint);
                        return;
                    }
                }
                else
                {
                    if (exitPoint.exitPointID == lastExitPointID - 8)
                    {
                        SpawnInPlace(player, exitPoint);
                        return;
                    }
                }
            }

            if (exitPoints.Length > 1)
            {
                SpawnInPlace(player, exitPoints[0]);
                Debug.LogWarning("Exit and entrance didn't match, entered by a random spawn point");
            }
            return;
        }
        else
        {
            Debug.LogWarning("Player not found", this);
            return;
        }
    }

    private void SpawnInPlace(GameObject player, ExitPoint exitPoint)
    {
        player.transform.position = exitPoint.transform.FindChild("Spawn Point").position;

        ExitPointID id = exitPoint.exitPointID;
        if (id >= ExitPointID.SpecialEnterEast1 && id <= ExitPointID.SpecialEnterEast4)
        {
            player.GetComponent<CharacterBehaviorModel>().SetFacingDirection(Vector3.left);
        }
        if (id >= ExitPointID.SpecialEnterWest1 && id <= ExitPointID.SpecialEnterWest4)
        {
            player.GetComponent<CharacterBehaviorModel>().SetFacingDirection(Vector3.right);
        }
        if (id >= ExitPointID.SpecialEnterNorth1 && id <= ExitPointID.SpecialEnterNorth4)
        {
            player.GetComponent<CharacterBehaviorModel>().SetFacingDirection(Vector3.down);
        }
        if (id >= ExitPointID.SpecialEnterSouth1 && id <= ExitPointID.SpecialEnterSouth4)
        {
            player.GetComponent<CharacterBehaviorModel>().SetFacingDirection(Vector3.up);
        }
    }
}
