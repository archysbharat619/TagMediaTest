/*
 * 
 * Author      : Bharat Soni
 * Description : This class will Manage the dictionary of word and will provide helper methods related 
 * to word bank for game play 
 * 
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WordBankManager : MonoBehaviour {

	// to hold the data of dictionary 
	private List<string> WordBank;             // this is to hold the original word bank 
	private List<string> WordBankOfSortedWors; // this is to hold wordbank of alphabetically sorted words
	string m_fileName="wordBank";              // this is the text file name for the 10000 common english word


//*********************************************************************************************
	// this method will return true if the word is in word bank otherwise return false
	public bool CheckAnswerString(string anserString)
	{
		if ( WordBank.Contains(anserString.ToLower ())) 
		{
			return true; //if word is valid and member of word bank
		}
		return false; // if it is not present in dictionary
	}
//**********************************************************************************************	
// this will check the alphabetically sorted word in sorted word bank 
	public bool CheckAnswerStringInSortedBank(string anserString)
	{
		if (WordBankOfSortedWors.Contains (anserString.ToLower ())) 
		{
			return true; //if word is valid and member of word bank
		}
		return false; // if it is not present in dictionary
	}

//********************************************************************************************************
	// this method is load file from local location  it contains 10000 common english word
	// we have sorted the each word of word bank alphabetically so the order of word will be regardless 
	// And we can avoid the heavy permutation calculations , this sorted bank is stored in WordBankOfSortedWors list 
	public void LoadDictionaryFromTextFile()
	{
		// to hold the dictionary 
		WordBank = new List<string> ();
		WordBankOfSortedWors = new List<string> ();
		StreamReader streamReader = new StreamReader ( "Assets/Resources/"+m_fileName+".txt");
		string line;
		while ((line = streamReader.ReadLine()) != null)
		{
			WordBank.Add(line);
			WordBankOfSortedWors.Add(GetSortedString(line));
		}
	}
//*************************************************************************************************************	
	// this is to load the dictionary from resource folder 
	void LoadFileFromResource()
	{
		WordBank = new List<string> ();
		TextAsset textasset = Resources.Load (m_fileName)as TextAsset;
		WordBank.AddRange(textasset.text.Split("\n"[0]));
	}


	// this method will sort all the  letter  of string in alphabetical order
	public string GetSortedString(string str)
	{
		System.Char[] chars = str.ToCharArray();
		List<char> t = new List<char> ();
		t.AddRange (chars);
		t.Sort ();
		string result="";
		for (int i=0; i< t.Count; i++) 
		{
			result=result+""+t[i];
		}
		return result;
	}
	
// this method will find all possible combination of given word or string 
	public string[] Combination(string str)
	{
		if (string.IsNullOrEmpty (str))
			return null;
		
		if (str.Length == 1)
		return new string[] { str };
		// read the last character
		char c = str[str.Length - 1];
		// apart from the last character send remaining string for further processing
		string[] returnArray = Combination(str.Substring(0, str.Length - 1));
		// List to keep final string combinations
		List<string> finalArray = new List<string>();
		// add whatever is coming from the previous routine
		for (int i=0; i< returnArray.Length; i++) 
		{
			if(!finalArray.Contains(returnArray[i]))
			{
			   finalArray.Add(returnArray[i]);
			}
		}	
		// take the last character
		  finalArray.Add(c.ToString());
		// take the combination between the last char and the returning strings from the previous routine
		for (int j=0; j< returnArray.Length; j++) {
			if(!finalArray.Contains(returnArray[j]+c))
			finalArray.Add (returnArray[j] + c);
		}
		return finalArray.ToArray();
	}

//***************************************************************************************************
// this method will validate the grid if there is a valid word possible or not 
// this method will find  all the possible combinations of word remain in the grid and then 
// will check the sorted array so no need to calculate permutation of each combination 
// and as soon as it finds a valid word it will return true 

	public bool validateGrid( string str)
	{
		List<string> result = new List<string> ();
		result.AddRange(Combination (str.ToLower()));
		for (int i=0; i<result.Count; i++) 
		{
			if(result[i].Length > 1)
			{
				string temp=GetSortedString(result[i]);
				int index = WordBankOfSortedWors.IndexOf(temp);
			  if (index >=0) 
			 {
					Debug.Log(WordBank[index]); // this will print which valid word is possible just for check 
					return true;
			 }
			}
		}
		return false;
	}
	
//************************************************************************************8
	// singletone Implementation 
	static private WordBankManager m_instance;
	static public WordBankManager GetInstance()
	{
		
		if (m_instance == null)
		{
			m_instance = Object.FindObjectOfType(typeof(WordBankManager)) as WordBankManager;
			
			if (m_instance == null)
			{
				GameObject temp = new GameObject("WordBankManager");
				DontDestroyOnLoad(temp);
				m_instance = temp.AddComponent<WordBankManager>();
			}
			return m_instance;
		}
		return m_instance;
	}
}
