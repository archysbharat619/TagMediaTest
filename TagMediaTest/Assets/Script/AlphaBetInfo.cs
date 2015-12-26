/*
 * 
 * Author      : Bharat Soni
 * Description : This class is to carry all the information of the alphabet object 
 * visible on the grid 
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public class AlphaBetInfo : MonoBehaviour {

	int buttonId=-1; //this is the id to uniquely identify the button representing its attached alphabet  

	bool IsSelectedAsAnswer=false; // this is to show whether the alphabet is selected for answer string 
	Vector3 position;              // this will hold the psition of alphabet on the grid 
	int answerIndex=-1;            // this will hold the index of alphabet in answer string so we can easily revert it back
	char letter;                   // this will hold the alphabet which this object represent 

	// this is getter methd for id
	public int GetButtonId()
	{
		return buttonId;
	}
// this setter for button id 
	public void SetButtonId(int id)
	{
		 buttonId = id;
	}
	// this will tell if the char is part of answerstring or not
	public bool IsAnswerChar()
	{
		return IsSelectedAsAnswer;
	}
	// once the word is selected we will make it a part of answer
	public void SetAnswerChar( bool flag)
	{
		 IsSelectedAsAnswer =flag;
	}
	// this will return the original position of the alphabet in the grid 
	public Vector3 GetOriginalPosition()
	{
		return position;
	}
	// this is to fill the info all together 
	public void setInfo(int id, Vector3 pos , char val)
	{
		position = pos;
		buttonId = id;
		letter = val;
	}
	// this will set the answer index
	public void SetAnswerIndex(int index)
	{
		 answerIndex = index;
	}
	// this will get the index of alphabet in the answer array 
	public int GetAnswerIndex()
	{
		return answerIndex;
	}
	// this will return the actual alphabet value 
	public char GetLetter()
	{
		return letter;
	}
}
