using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using System.Linq;
using CBGames;

public class LeaderboardManager : MonoBehaviour
{

	[Header("Config")]
	[Header("Maximum User When Getting User Data. Set -1 To Disable And Get All User Data")]
	[SerializeField] private int maxUser = 20;
	[Header("Private And Public Code")]
	[SerializeField] private string leaderboardPrivateCode = "b48eP8dPn0yaau4sy6JmdgQJgYqH_3M0yOORpBzA00Iw";
	[SerializeField] private string leaderboardPublicCode = "5e690595fe232612b89ef73f";


	public int MaxUser { get { return maxUser; } }


	private string SetLeaderboardDataUrl { get { return "http://dreamlo.com/lb/" + leaderboardPrivateCode; } }
	private string GetLeaderboardDataUrl { get { return "http://dreamlo.com/lb/" + leaderboardPublicCode; } }


	private void Awake()
	{
		//UnityWebRequest www = new UnityWebRequest(SetLeaderboardDataUrl + "/clear");
		//www.SendWebRequest();
		//GetPlayerLeaderboardData();

		//StartCoroutine(SetTest("Stick", 10));
		//StartCoroutine(SetTest("EnelK", 5));
		//StartCoroutine(SetTest("zuri", 8));
		//StartCoroutine(SetTest("Dean", 12));
		//StartCoroutine(SetTest("Zach", 7));
		//StartCoroutine(SetTest("Sammy", 11));
		//StartCoroutine(SetTest("Joe", 16));
		//StartCoroutine(SetTest("kurt", 3));
		//StartCoroutine(SetTest("mixi", 2));
		//StartCoroutine(SetTest("long", 19));
		//StartCoroutine(SetTest("siren", 18));
		//StartCoroutine(SetTest("bool", 15));
		//StartCoroutine(SetTest("pool", 14));
		//StartCoroutine(SetTest("Waken", 12));
		//StartCoroutine(SetTest("lord", 6));
	}


	/// <summary>
	/// Checking internent connection.
	/// </summary>
	/// <param name="callback"></param>
	public void CheckConnectedInternet(Action<bool> callback)
    {
		StartCoroutine(CRCheckingConnectedInternet(callback));
    }
    private IEnumerator CRCheckingConnectedInternet(Action<bool> callback)
    {
		UnityWebRequest www = new UnityWebRequest("http://google.com");
		yield return www;
		if (www.error != null)
			callback?.Invoke(false);
		else
		{
			callback?.Invoke(true);
		}
	}


	/// <summary>
	/// Is the user set username or not. 
	/// </summary>
	/// <returns></returns>
	public bool IsSetUsername()
	{
		return !string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerPrefsKey.SAVED_USER_NAME_PPK, string.Empty));
	}



	/// <summary>
	/// Checking whether the given username already exists.
	/// </summary>
	/// <param name="username"></param>
	/// <param name="callback"></param>
	public void CheckUsernameExists(string username, Action<bool> callback)
	{
		GetPlayerLeaderboardData((data) =>
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (data[i].Name.Equals(username))
				{
					callback(true);
					return;
				}
			}

			callback(false);
			return;
		});
	}



	/// <summary>
	/// Set player leaderboard data.
	/// </summary>
	/// <param name="data"></param>
	public void SetPlayerLeaderboardData()
	{
		StartCoroutine(CRUpdatePlayerLeaderboardData());
	}
	private IEnumerator CRUpdatePlayerLeaderboardData()
	{
		string username = PlayerPrefs.GetString(PlayerPrefsKey.SAVED_USER_NAME_PPK);
		int level = PlayerPrefs.GetInt(PlayerPrefsKey.SAVED_LEVEL_PPK);
		string requestUrl = SetLeaderboardDataUrl + "/add-pipe/" + UnityWebRequest.EscapeURL(username) + "/" + level.ToString();
		UnityWebRequest www = new UnityWebRequest(requestUrl);
		yield return www.SendWebRequest();
	}

	private IEnumerator SetTest(string name, int level)
	{
		string requestUrl = SetLeaderboardDataUrl + "/add-pipe/" + UnityWebRequest.EscapeURL(name) + "/" + level.ToString();
		UnityWebRequest unityWebRequest = new UnityWebRequest(requestUrl);
		DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
		unityWebRequest.downloadHandler = dH;
		yield return unityWebRequest.SendWebRequest();
	}



	public void GetPlayerLeaderboardData(Action<List<PlayerLeaderboardData>> callback)
	{
		StartCoroutine(CRGetPlayerLeaderboardData(callback));
	}
	private IEnumerator CRGetPlayerLeaderboardData(Action<List<PlayerLeaderboardData>> callback)
	{
		string requestUrl = GetLeaderboardDataUrl + "/pipe" ;
		UnityWebRequest unityWebRequest = new UnityWebRequest(requestUrl);
		DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
		unityWebRequest.downloadHandler = dH;
		yield return unityWebRequest.SendWebRequest();

		if (!unityWebRequest.isHttpError && !unityWebRequest.isNetworkError)
		{
			callback?.Invoke(ConvertData(unityWebRequest.downloadHandler.text));
		}
		else
		{
			Debug.Log("Error downloading: " + unityWebRequest.error);
		}
	}





	private List<PlayerLeaderboardData> ConvertData(string data)
	{
		List<PlayerLeaderboardData> listResult = new List<PlayerLeaderboardData>();
		string[] rows = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		for(int i = 0; i < rows.Length; i++)
		{
			string[] chars = rows[i].Split(new char[] { '|' }, StringSplitOptions.None);
			PlayerLeaderboardData leaderboardData = new PlayerLeaderboardData();
			leaderboardData.SetName(chars[0]);
			leaderboardData.SetLevel(int.Parse(chars[1]));
			listResult.Add(leaderboardData);
		}

		IComparer<PlayerLeaderboardData> comparer = new PlayerLeaderboardDataComparer();
		PlayerLeaderboardData[] datas = listResult.ToArray();
		Array.Sort(datas, comparer);
		return datas.ToList();
	}
}
