﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ranking : GenericScreen 
{
	public GameObject rankingCard;
	public Text cardName, cardXP, cardPlace;

	public void Start() 
	{
		AlertsAPI.instance.Init();
		backScene = "Home";

		if (sceneSchema == null)
			sceneSchema = "user-ranking";

		RequestRanking();
	}

	private void RequestRanking()
	{
		WWW rankingRequest;

		if (sceneSchema.Equals("user-ranking"))
			rankingRequest = RankingAPI.RequestUserRanking();
		else
			rankingRequest = RankingAPI.RequestGroupRanking();

		string Response = rankingRequest.text,
		Error = rankingRequest.error;

		if (rankingRequest.responseHeaders["STATUS"] == "HTTP/1.1 200 OK")
		{
			Debug.Log("Response:" + Response);

			if (sceneSchema.Equals("user-ranking"))
			{
				RankingAPI.UpdateUserRanking(Response);
				CreateUserCards();
			}
			else
			{
				RankingAPI.UpdateGroupRanking(Response);
				CreateGroupCards();
			}
		}
		else 
		{
			AlertsAPI.instance.makeAlert("Ops!\nHouve um problema ao receber o ranking. Tente novamente em alguns instantes.", "Tudo bem");
			LoadBackScene();
		}
	}

	private void CreateUserCards ()
     {
     	int place = 1;
     	Vector3 Position = rankingCard.transform.position;

     	foreach (User user in RankingAPI.userRanking)
        {
        	if (place == 6)
        		break;

        	cardName.text = user.name;
        	cardXP.text = user.xp + " TintaLuz";
        	cardPlace.text = place.ToString();

            Position = new Vector3(Position.x, Position.y, Position.z);

            GameObject Card = (GameObject) Instantiate(rankingCard, Position, Quaternion.identity);
            Card.transform.SetParent(GameObject.Find("Area").transform, false);

            place++;
        }

        Destroy(rankingCard);
    }

    private void CreateGroupCards ()
    {
     	int place = 1;
     	Vector3 Position = rankingCard.transform.position;

     	foreach (Group group in RankingAPI.groupRanking)
        {
        	if (place == 6)
        		break;
        	
        	cardName.text = group.name;
        	cardXP.text = group.points + " TintaLuz";
        	cardPlace.text = place.ToString();

            Position = new Vector3(Position.x, Position.y, Position.z);

            GameObject Card = (GameObject) Instantiate(rankingCard, Position, Quaternion.identity);
            Card.transform.SetParent(GameObject.Find("Area").transform, false);

            place++;
        }

        Destroy(rankingCard);
    }

    public void ShowRanking(string type)
    {
    		sceneSchema = type;
    		ReloadScene();
    }
}
