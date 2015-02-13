using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FacebookManager : MonoBehaviour {

	protected string lastResponse = "";
	protected Texture2D lastResponseTexture;
	
	public string FeedToId = "",
	FeedLink = "",
	FeedLinkName = "",
	FeedLinkCaption = "",
	FeedLinkDescription = "",
	FeedPicture = "",
	FeedMediaSource = "",
	FeedActionName = "",
	FeedActionLink = "",
	FeedReference = "";
	public bool IncludeFeedProperties = false;
	private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();


	public void PostStatus()
	{
		CallFBInit();
	}

	private void CallFBInit()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}

	private void OnInitComplete()
	{
		if (!FB.IsLoggedIn){
			CallFBLogin();
		}
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Time.timeScale = (isGameShown) ? 1 : 0;
	}

	private void CallFBLogin()
	{
		FB.Login("public_profile,email,user_friends", LoginCallback);
	}

	void LoginCallback(FBResult result)
	{
		if (result.Error != null)
			lastResponse = "Error Response:\n" + result.Error;
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
		}
		else
		{
			lastResponse = "Login was successful!";
			CallFBFeed ();
		}
	}
		
	private void CallFBFeed()
	{
		Dictionary<string, string[]> feedProperties = null;
		if (IncludeFeedProperties)
		{
			feedProperties = FeedProperties;
		}
		FB.Feed(
			toId: FeedToId,
			link: FeedLink,
			linkName: FeedLinkName,
			linkCaption: FeedLinkCaption,
			linkDescription: FeedLinkDescription,
			picture: FeedPicture,
			mediaSource: FeedMediaSource,
			actionName: FeedActionName,
			actionLink: FeedActionLink,
			reference: FeedReference,
			properties: feedProperties,
			callback: Callback
			);
	}

	protected void Callback(FBResult result)
	{
		lastResponseTexture = null;
		// Some platforms return the empty string instead of null.
		if (!String.IsNullOrEmpty (result.Error))
		{
			lastResponse = "Error Response:\n" + result.Error;
		}
		else if (!String.IsNullOrEmpty (result.Text))
		{
			lastResponse = "Success Response:\n" + result.Text;
		}
		else if (result.Texture != null)
		{
			lastResponseTexture = result.Texture;
			lastResponse = "Success Response: texture\n";
		}
		else
		{
			lastResponse = "Empty Response\n";
		}
	}
}
