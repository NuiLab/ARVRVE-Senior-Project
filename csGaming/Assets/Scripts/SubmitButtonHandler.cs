﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubmitButtonHandler : MonoBehaviour {
	public Transform solutionPanel;
	public GameObject player;
	Vector3 originalPos;
	Quaternion originalRot;

	public Button submit;
	public Button clear;

	public Text attempts; 
	int attempt; 

	GameObject[] editBtn;

	UIPlayCommand commandExecution;
	UIButtonClick commandProcessor;

	// Use this for initialization
	void Start () {
		//Mairim
		Button submitBtn = submit.GetComponent<Button>();
		submitBtn.onClick.AddListener (AddCommandsToCommandList);

		//Record original position of the player 
		originalPos = player.transform.position;
		originalRot = player.transform.rotation;

		//Create instance of the two player handler classes
		commandProcessor = GetComponent<UIButtonClick>();
		commandExecution = GetComponent<UIPlayCommand> (); 

        attempt = 0; 


	}

	void AddCommandsToCommandList()
	{
		/* Return player to original position */
		player.transform.position = originalPos;
		player.transform.rotation = originalRot;
        commandExecution.rotation = 0f;
        /*If user had previously clicked the play Button, return player to original position and 
		 * clear commandList so that when user clicks it again, commands are not duplicated
		 */
        if (commandProcessor.commandList.Count == 0) {  //Solution Panel (commandList) is empty, add commands to the list
			foreach (Transform child in solutionPanel) {
				if (child.name != "Command") { //Original prefab gets ignored
					commandProcessor.commandList.Add (child.name);
				}
				sendCommandsToCharacter ();
			}
		} else { //Solution panel has commands, return to original position, and retry 
			commandProcessor.commandList.Clear();
			//Move player to its original position
			AddCommandsToCommandList();
		}

        /* Disable all controls while character is moving */
        print("Should be interactable: " + submit.interactable);
        submit.interactable = false;
		clear.interactable = false; 
		editBtn = GameObject.FindGameObjectsWithTag ("Edits");

		foreach (GameObject btn in editBtn) {
			btn.GetComponent<Button>().interactable = false;
		}
        print("Should not be interactable: " + submit.interactable);
		/* Increase no of attempts */
		attempt++;

		attempts.text = attempt.ToString (); 

	}

	void sendCommandsToCharacter()
	{
		//UIPlayCommand.Execute executes the command list
		commandExecution.TaskOnClick();


	}

	// Update is called once per frame
	void Update () {
		//print (solutionPanel.transform.childCount);
		if (solutionPanel.transform.childCount == 1) {
			submit.interactable = false;
			clear.interactable = false; 
		} else {
			submit.interactable = true;
			clear.interactable = true; 
		}
	}
}
