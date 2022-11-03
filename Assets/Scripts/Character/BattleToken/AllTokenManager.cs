using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Function:
///     1. Load all tokens from json and save as scriptable object as asset
///     2. Provide an All token effect review dictionary
/// 
/// </summary>
public class AllTokenManager
{
    private Dictionary<int, Token> allTokens;

    private Dictionary<int, Token> allActiveTokens;

    private Dictionary<int, Token> allSupportTokens;
    private Dictionary<int, Token> allSpecialTokens;

    AllTokenManager(string address)
    {
        Debug.Log(address); // json file address

        // initial 4 different storage place
        allTokens = new Dictionary<int, Token>();   
        allActiveTokens = new Dictionary<int,Token>();
        allSupportTokens = new Dictionary<int, Token>();
        allSpecialTokens = new Dictionary<int, Token>();
    }

    AllTokenManager(int order)
    {
        // loop and find all SO tokens from the asset database
    }


    public void ReadJsonNOutputSO()
    {
        // read json file



        // convert to so via Token template



        // overwrite on previous SOs
    }


    public Dictionary<int, string> GetTokenDict()
    {

        return new Dictionary<int, string>();
    }

    public string GetDescription(Token token)
    {
        // Get the description area from the token instance
        return "";
    }

}
