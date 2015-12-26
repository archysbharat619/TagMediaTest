/*
 * 
 * Author      : Bharat Soni
 * Description : This class will manage the question grid and the play as well 
 * 
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridManager : MonoBehaviour 
{

	// Use this for initialization
	// prefab  to create the visible unit of teh game 
	public Image blankImgPrefab;
	public Image AnswerPrefab;
	public Button donePrefab;
	public Button bprefab;
	public Image wrongPrefab;
	public Image correctPrefab;

	// this is to show the wrong and correct msgs 
	Image wrong,right;
// this is to hold the alphabet manager's reference 
	AlphabetManager alphabetManager;
// this is to hold the grid 
	public GameObject  container;
	float startX, startY;
	float tileWidth ,tileHeight;
	int noOfRow =10; // max row num allowed 
	int noOfCol =10; // max col allowed
	float GridWidth, GridHeight;
	float padding = 1;
	float ScrenPercentage =100;

	int currentNoOfRow=3;  // game play grid row num
	int currentNoOfCol=3;  // game play grid col num
	Vector3[] answerGridArry = new Vector3[20];
	int currentAnsweGridNo=0;
	Button[,] myQuestionArry;
	int noOfLetterClicked=0;
	char[] alphabateArry=new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R',
	                                 'S','T','U','V','W','X','Y','Z'};
	// this is to assign index to each letter 
	List<char> alphabateList=new List<char>(){'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R',
		'S','T','U','V','W','X','Y','Z'};

	int[] answerArray = new int[20];
    
	// this is to load specific question grid for eg of taking 3 x3 grid
	string questionString="AIEHTTMCR";
	char[] tempQuestionArray;
	List<Button> destructionArry;

	List<char> remaingLetter;
	void Start () 
	{
		tempQuestionArray = questionString.ToCharArray ();
		alphabetManager = GetComponent<AlphabetManager> ();
		alphabetManager.LoadAlphabetSprite ();
		WordBankManager.GetInstance ().LoadDictionaryFromTextFile ();
		calculateGridDimensionsAndCreateEMptyGrid (currentNoOfRow,currentNoOfCol);
		destructionArry = new List<Button> ();
		remaingLetter = new List<char> ();
		remaingLetter.AddRange (tempQuestionArray);

	}
// this will get the index of char which will create its id
	public int GetWordIndex(char val)
	{
		return alphabateList.IndexOf (val);
	}
	#region UIPart creation  *********************************
//************************************************************************************************************
// this is rough UI part not paid much attention as it was not the part of problem defination 
	// it is made to visualize the game 
	// this method is to create the grid
	void calculateGridDimensionsAndCreateEMptyGrid(int row , int col)
	{
		int minRowVal;
		int minColVal;

		if (row <= noOfRow && col <= noOfCol) {
			minRowVal = Mathf.FloorToInt ((noOfRow - row) / 2);
			minColVal = Mathf.FloorToInt ((noOfCol - col) / 2);
			myQuestionArry=new Button[row,col];
		} else 
		{
			return ;
		}
		GridWidth = Screen.width * (ScrenPercentage/100);
		GridHeight = GridWidth;  // as i want square alphabates
		int index = 0;
		tileWidth = Screen.width / noOfCol;
		tileHeight = tileWidth; // to keep It Square
		startX = Screen.width / 2 - ((tileWidth * noOfCol)/2 - tileWidth /2);
		startY = Screen.height  - (tileHeight * noOfRow)+ tileHeight/3;
		int counter = 0;
		int wordIndex=0;
		for (int i=0; i< noOfRow; i++) 
		{
			for(int j=0; j< noOfCol; j++)
			{
				Image img = Instantiate(blankImgPrefab) as Image;
				img.rectTransform.SetParent(this.transform);
				img.rectTransform.sizeDelta=(new Vector2(tileWidth,tileHeight));
				img.rectTransform.localScale=new Vector3(1,1,1);
				img.rectTransform.position= new Vector3(startX +(j * tileWidth),startY + (i * tileHeight),1);
				// this is to create answer grid
				if((i>=minRowVal && i<(minRowVal+row))&& (j>=minColVal && j<(col+minColVal)))
				{
					Button alphabetTab = Instantiate(bprefab) as Button;
					RectTransform rt= alphabetTab.GetComponent<RectTransform>();
					rt.SetParent(container.transform);
					rt.sizeDelta=(new Vector2(tileWidth,tileHeight));
					rt.localScale=new Vector3(1,1,1);
					rt.position= new Vector3(startX +(j * tileWidth),startY + (i * tileHeight),0);
					//int id=Random.Range(0,26); this was for random generation 
					int id= GetWordIndex(tempQuestionArray[wordIndex]);
					alphabetTab.image.sprite=alphabetManager.GetAlphabateImg(id);
					int r = i-minRowVal;
					int c = j-minColVal;
					AlphaBetInfo info=alphabetTab.gameObject.AddComponent<AlphaBetInfo>();
					info.setInfo(id,rt.position,alphabateArry[id]);
					alphabetTab.onClick.AddListener(delegate { OnClick(r,c); });
					myQuestionArry[r,c]=alphabetTab;
					wordIndex++;
				}
				if(counter < 2)
				{
					Image tempImg = Instantiate(AnswerPrefab) as Image;
					tempImg.rectTransform.SetParent(this.transform);
					tempImg.rectTransform.sizeDelta=(new Vector2(tileWidth,tileHeight));
					tempImg.rectTransform.localScale=new Vector3(1,1,1);
					answerGridArry[index]=new Vector3(startX +(j * tileWidth),(startY-(tileHeight * 1.1f)) - (counter * tileHeight),1);
					tempImg.rectTransform.position =answerGridArry[index] ;
					index++;
				}
			}
			counter++;
		}
		Button done = Instantiate(donePrefab) as Button;
		RectTransform donert= done.GetComponent<RectTransform>();
		donert.SetParent(container.transform);
		donert.sizeDelta=(new Vector2(tileWidth * 2,tileHeight));
		donert.localScale=new Vector3(1,1,1);
		donert.position= new Vector3(Screen.width/2,(startY-(tileHeight * 3.2f)),0);
		done.onClick.AddListener(delegate { OnDOneClick(); });

		wrong = Instantiate(wrongPrefab) as Image;
		wrong.rectTransform.SetParent(container.transform);
		wrong.rectTransform.sizeDelta=(new Vector2(tileWidth * 2,tileHeight));
		wrong.rectTransform.localScale=new Vector3(0,0,0);
		wrong.rectTransform.position= new Vector3(Screen.width/4,(startY-(tileHeight * 3.2f)),0);

		right = Instantiate(correctPrefab) as Image;
		right.rectTransform.SetParent(container.transform);
		right.rectTransform.sizeDelta=(new Vector2(tileWidth * 2,tileHeight));
		right.rectTransform.localScale=new Vector3(0,0,0);
		right.rectTransform.position= new Vector3(Screen.width/1.2f ,(startY-(tileHeight * 3.2f)),0);
	}
	#endregion ***********************
//*********************************************************************************************************************
//*********************************************************************************************************************

	#region GamePlayMethods ************************************
	//this will execute the click method if any alphabet  is selected from the grid to create the answer string
	public void OnClick(int row ,int col)
	{
		Button temp = myQuestionArry [row, col]; // get button of corresponding row and col 
		AlphaBetInfo info = temp.gameObject.GetComponent<AlphaBetInfo> (); // get its info from AlphabetInfo class
		if (info.IsAnswerChar ()) 
		{
			// if it is already a part of answer then user must have clicked this to remove from answer array
			// so remove it and put it back to its original position
			temp.GetComponent<RectTransform> ().position = info.GetOriginalPosition ();
			currentAnsweGridNo=info.GetAnswerIndex();
			info.SetAnswerChar(false);
			noOfLetterClicked--;
			destructionArry.Remove(temp);
		} 
		else 
		{
			// if its a fresh selection then mark it as a part of answer string and put it in current answer array
			info.SetAnswerChar(true);
			temp.GetComponent<RectTransform> ().position = answerGridArry[currentAnsweGridNo];
			info.SetAnswerIndex(currentAnsweGridNo);
			answerArray[currentAnsweGridNo]=info.GetButtonId();
			noOfLetterClicked++;
			currentAnsweGridNo = noOfLetterClicked;
			destructionArry.Add(temp);
		}
	}

	// this will create the string from the list of char 
	string CreateString(List<char> clist)
	{
		string str="";
		for (int i=0; i < clist.Count; i++) 
		{
			str=str+clist[i];
		}
		return str;
	}

	// this will create the string from the selected alphabet so we can check in the word bankto
	// validate it 
	string CreateAnswerString()
	{
		string str="";
		if (currentAnsweGridNo == 0) 
		{
			return null;
		}
		for (int i=0; i < currentAnsweGridNo; i++) 
		{
			str=str+alphabateArry[answerArray[i]];
		}
		return str;
	}
// this is a co routine to show the correct or wrong answer massage for a perticular time 
	float timer=0;
	IEnumerator showMsg(int i)
	{
		while (timer < 2) 
		{
			timer+= Time.deltaTime;
			yield return null;
		}
		switch(i)
		{
		case 1:
			right.rectTransform.localScale=new Vector3(0,0,0);
			break;
		case 2:
			wrong.rectTransform.localScale=new Vector3(0,0,0);
			break;
		}
		timer = 0;
	}

// once the user is done with creating the string , it has to press the done button so we can process 
// its answer  this method will process the naswer string and also will validate the 
// grid if it contains atleast one valid word or not 

	public void OnDOneClick()
	{
		string answer = CreateAnswerString ();
		if (WordBankManager.GetInstance ().CheckAnswerString(answer.ToLower())) //chek the validity of word
		{ 
			Debug.Log (" This is the valid string ");
			right.rectTransform.localScale=new Vector3(1,1,1);
			StartCoroutine( showMsg(1));
			char[] removeletter =answer.ToCharArray();
			for(int i=0;i <destructionArry.Count;i++ )
			{
				remaingLetter.Remove(removeletter[i]);
				Destroy(destructionArry[i].gameObject);
			}
			for (int i=0; i < currentAnsweGridNo; i++) 
			{
				answerArray[i]=-1;
			}
			noOfLetterClicked=0;
			currentAnsweGridNo=0;
			destructionArry.Clear();
			if(WordBankManager.GetInstance ().validateGrid(CreateString(remaingLetter)))// check the validity of grid
			{
				Debug.Log("Grid is still valid for further play ");
			}
			else
			{
				Debug.Log("No valid word in the grid !!!!!!! ");
			}
		} 
		else 
		{
			Debug.Log (" in valid word or mising from bank");
			wrong.rectTransform.localScale=new Vector3(1,1,1);
			StartCoroutine( showMsg(2));
			char[] removeletter =answer.ToCharArray();
			for(int i=0;i <destructionArry.Count;i++ )
			{
				remaingLetter.Remove(removeletter[i]);
				Destroy(destructionArry[i].gameObject);
			}
			for (int i=0; i < currentAnsweGridNo; i++) 
			{
				answerArray[i]=-1;
			}
			if(WordBankManager.GetInstance ().validateGrid(CreateString(remaingLetter)))// check the validity of grid
			{
				Debug.Log("Grid is still valid for further play ");
			}
			else
			{
				Debug.Log("No valid word in the grid !!!!!!! ");
			}
			noOfLetterClicked=0;
			currentAnsweGridNo=0;
			destructionArry.Clear();
		}
	}

	#endregion
}
