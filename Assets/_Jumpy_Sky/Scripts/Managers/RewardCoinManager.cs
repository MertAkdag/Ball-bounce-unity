using CBGames;
using System.Collections;
using UnityEngine;

public class RewardCoinManager : MonoBehaviour
{

    /// <summary>
    /// Reward an amount of coins for Total Coins with given delay time
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="delay"></param>
    public void RewardTotalCoins(int amount, float delay)
    {
        StartCoroutine(CRRewardingTotalCoins(amount, delay));
    }

    /// <summary>
    /// Reward an amount of coins for Collected Coins with given delay time
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="delay"></param>
    public void RewardCollectedCoins(int amount, float delay)
    {
        StartCoroutine(CRRewardingCollectedCoins(amount, delay));
    }


    /// <summary>
    /// Remove all collected coins.
    /// </summary>
    /// <param name="delay"></param>
    public void RemoveCollectedCoins(float delay)
    {
        StartCoroutine(CRRemoveCollectedCoins(delay));
    }


    /// <summary>
    /// Remove an amount of total coins.
    /// </summary>
    /// <param name="delay"></param>
    public void RemoveTotalCoins(float delay, int amount)
    {
        StartCoroutine(CRRemovingTotalCoins(amount, delay));
    }


    IEnumerator CRRewardingTotalCoins(int amount, float delay)
    {
        ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.rewarded);
        yield return new WaitForSeconds(delay);
        //Reward coins
        float t = 0;
        float runTime = 0.5f;
        int startTotalCoins = ServicesManager.Instance.CoinManager.TotalCoins;
        int endTotalCoins = startTotalCoins + amount;
        while (t < runTime)
        {
            t += Time.deltaTime;
            //float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
            //int newTotalCoins = (int)Mathf.Lerp(startTotalCoins, endTotalCoins, factor);
            
            ServicesManager.Instance.CoinManager.SetTotalCoins(endTotalCoins);
            yield return null;
        }
    }


    IEnumerator CRRemovingTotalCoins(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);

        //Remove coins
        float t = 0;
        float runTime = 0.5f;
        int startTotalCoins = ServicesManager.Instance.CoinManager.TotalCoins;
        int endTotalCoins = startTotalCoins - amount;
        while (t < runTime)
        {
            
            ServicesManager.Instance.CoinManager.SetTotalCoins(startTotalCoins - amount);
            yield return null;
        }
    }

    IEnumerator CRRewardingCollectedCoins(int amount, float delay)
    {
        ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.rewarded);
        yield return new WaitForSeconds(delay);
        //Reward coins
        float t = 0;
        float runTime = 0.5f;
        int startCollectedCoins = ServicesManager.Instance.CoinManager.CollectedCoins;
        int endCollectedCoins = startCollectedCoins + amount;
        Debug.Log("end collected eşit " + endCollectedCoins);
        while (t < runTime)
        {
            t += Time.deltaTime;
            //float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
            //int newCollectedCoins = (int)Mathf.Lerp(startCollectedCoins, endCollectedCoins, factor);
            ServicesManager.Instance.CoinManager.SetCollectedCoins(endCollectedCoins);
            Debug.Log("bura çalıştı ab");
            yield return null;
        }
    }


    private IEnumerator CRRemoveCollectedCoins(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Remove coins
        float t = 0;
        float runTime = 0.5f;
        int startCollectedCoins = ServicesManager.Instance.CoinManager.CollectedCoins;
        while (t < runTime)
        {
            t += Time.deltaTime;
            float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
            int newCollectedCoins = (int)Mathf.Lerp(startCollectedCoins, 0, factor);
            ServicesManager.Instance.CoinManager.SetCollectedCoins(newCollectedCoins);
            yield return null;
        }
    }
}
//t += Time.deltaTime;
//float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
//int newTotalCoins = (int)Mathf.Lerp(startTotalCoins, endTotalCoins, factor);

//ServicesManager.Instance.CoinManager.SetTotalCoins(newTotalCoins);
//yield return null;