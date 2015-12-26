/*
 * 
 * Author      : Bharat Soni
 * Description : This class will Manage Alphabet Images that will be displayed on grid
 * 
 * 
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlphabetManager : MonoBehaviour {

	public Image alphabetPrefab;
	public Texture2D charaterStringTexture;
	public Texture2D StringTexture;
	Image[] alphabetArray=new Image[26];
	Sprite[] alphabet=new Sprite[26];

	// this will load the alphabet
	public void LoadMyAlphabetArray(float  width , float height)
	{
		LoadAlphabetSprite ();
		for(int i=0;i< alphabetArray.Length ;i++)
		{
			Image tempImg=Instantiate(alphabetPrefab) as Image;
			tempImg.sprite= alphabet[i];
			tempImg.rectTransform.localScale=new Vector3(1,1,1);
			alphabetArray[i]=tempImg;
		}
	}
    
	// this method is to get the individual alphabet 
	public Sprite GetAlphabateImg(int index)
	{
		if (index < alphabet.Length) 
		{
			return alphabet[index];
		}
		return null;
	}


	// this method will crop each alphbet from the sprite and will create the separate sprite 
	public void LoadAlphabetSprite()
	{
		float charWidth = StringTexture.width / 4;
		float charHeight = StringTexture.height/7;
		int counter = 0;
		for (int i=0; i<7; i++) 
		{
			for (int j=0; j<4; j++) 
			{
				if(counter < 26)
				{
					Rect rect = new Rect((j* charWidth),(StringTexture.height-charHeight) -(i * charHeight),charWidth ,charHeight);
					alphabet[counter]=Sprite.Create(StringTexture,rect,new Vector2(0.5f,0.5f) );
				}
				counter++;
			}
		}
	}
}
