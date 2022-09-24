using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Singleton battle controller, handler turn and characters on field
/// </summary>
public class BattleController : MonoBehaviour
{
    private static BattleController _instance;
    public static BattleController Instance { get { return _instance; } }


    public GameObject BattleCharacterModel;
    [SerializeField] private LayerMask layermask;

    // test field
    public BattlePathfinder pathfinder;

    //pathfinding/range display support
    public float moveSpeed;
    private List<BattleTile> path = new List<BattleTile>();
    private List<BattleTile> inRangeTiles = new List<BattleTile>();

    private bool isMoving= false;
    private BattlePhrase state;
    // Battle Controll related
    public enum BattlePhrase
    {
        Idle, // Prepare the battleGround and other inital stats
        WaitMove, // player move target 
        WaitToken, // player token target
        WaitAI, // wait ai action

        WaitAction, // player animation 
        WaitResult
    }

    private bool isTest=true;
    private BattleCharacter focusedCharacter;
    private List<BattleCharacter> CharacterList =new List<BattleCharacter>();

    //Token 
    private List<Token> tokenQueue;
    private int tokenTargetDirection = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        pathfinder = new BattlePathfinder(); //注册寻路
        tokenQueue = new List<Token>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle click Event
        PhraseStateMachine();

        FieldDisplay();
        if (Input.GetKeyDown(KeyCode.K)) // test 
        {
            if(CharacterList.Count == 0)
            {
                // add 2 player and 2 AI 
                
            }


            BattleStart();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CheckResult();
        }



        if (isTest) // character generator 
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var hit = GetFocusedOnTile();
                if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    BattleTile battleTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                    var testc = Instantiate(BattleCharacterModel, GameObject.Find("PlayerTeam").transform).GetComponent<BattleCharacter>();
                    testc.characterData.CharacterName = "player" + UnityEngine.Random.Range(0,10);
                    testc.activeTile = battleTile;
                    PositionCharacterOnTile(testc, battleTile);
                    CharacterList.Add(testc);
                    
                }
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                var hit = GetFocusedOnTile();
                if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    BattleTile battleTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                    var testc = Instantiate(BattleCharacterModel, GameObject.Find("AITeam").transform).GetComponent<BattleCharacter>();
                    testc.Team = 0;
                    testc.characterData.CharacterName = "AI" + UnityEngine.Random.Range(0, 10);
                    testc.activeTile = battleTile;
                    PositionCharacterOnTile(testc, battleTile);
                    CharacterList.Add(testc);
                }
            }
        } else
        {
            LoadBattleData();
        }



    }


    private void LoadBattleData()
    {
        //Update battlefield


        //Update battleCharacter




    }


    /// <summary>
    /// 显示代币组目标内容
    /// </summary>
    private void FieldDisplay()
    {
        if(state == BattlePhrase.WaitToken)
        {
            // depends on current token set, display target option
        }
    }

    /// <summary>
    /// 显示战斗动作文字
    /// </summary>
    /// <param name="iptext"></param>
    /// <returns></returns>
    IEnumerator DisplayAttackText(string iptext)
    {
        GameObject.Find("AttackTextUI").GetComponent<Text>().text = iptext;

        yield return new WaitForSeconds(2);
        GameObject.Find("AttackTextUI").GetComponent<Text>().text = "";
    }

    

    /// <summary>
    /// 状态机
    /// </summary>
    private void PhraseStateMachine()
    {
        if (state == BattlePhrase.Idle) // cant do anything -- default state
        {
            Debug.Log("Idle state");
        } else if (state == BattlePhrase.WaitMove) // wait player select move target position
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = GetFocusedOnTile();
                if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    BattleTile targetTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                    Debug.Log($"玩家点击了{targetTile.gridPos}");

                    if (focusedCharacter != null && targetTile != null)

                    {
                        if (focusedCharacter.steps > 0)
                        {
                            targetTile.GetComponent<BattleTile>().ShowTile();
                            //Move test
                            path = pathfinder.FindPath(focusedCharacter.activeTile, targetTile);
                            isMoving = true;
                            StartCoroutine(MoveByPath(focusedCharacter, path));
                            state = BattlePhrase.WaitAction;
                        }
                        else
                        {
                            Debug.Log("Out of steps");
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                var hit = GetFocusedOnTile();
                if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    BattleTile battleTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                    Debug.Log($"玩家点击了{battleTile.gridPos}");
                }
            }



        } else if (state == BattlePhrase.WaitToken)
        {
            if(tokenQueue.Count == 0)
            {
                state = BattlePhrase.Idle;
                return;
            }


            //display targettable area by mousepos
            var hit = GetFocusedOnTile();
            if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
            {
                BattleTile battleTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                var mousePos = battleTile.gridPos;
                var charPos = focusedCharacter.activeTile.gridPos;
                var norm = (mousePos - charPos);
                var newTokenTargetDirection = -1;
                if (norm.x > 0)
                {
                    newTokenTargetDirection = 0;
                }
                else
                {
                    newTokenTargetDirection = 1;
                }

                if (tokenTargetDirection != newTokenTargetDirection)
                {
                    tokenTargetDirection = newTokenTargetDirection;
                    List<Vector3Int> tiles = new List<Vector3Int>();
                    if (tokenTargetDirection == 0)
                    {
                        tiles.Add(focusedCharacter.activeTile.gridPos + new Vector3Int(1, 0, 0));
                    }
                    else
                    {
                        tiles.Add(focusedCharacter.activeTile.gridPos + new Vector3Int(-1, 0, 0));
                    }
                    BattleManager.Instance.ClearAllTileHighlighter();
                    foreach (var tile in tiles)
                    {
                        BattleManager.Instance.GetTileByPos(tile).ShowTile();

                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    BattleTile targetTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                    Debug.Log($"玩家点击了{targetTile.gridPos}");

                    if (focusedCharacter != null && targetTile != null)
                    {
                        if (targetTile.standon != null)
                            StartCoroutine(DisplayAttackText($"{focusedCharacter.characterData.CharacterName}攻击了{targetTile.standon.characterData.CharacterName}"));

                        //Update token highlighter

                        //record target pos

                        // depends on the the token pool's status, wait numbers of targeting select


                    }
                } else
                {
                    // clear token target list

                    // backward 1 step

                    // target list is empty, back to normal state & unlock pool
                }

            }

        } else if (state == BattlePhrase.WaitAction)
        {
            Debug.Log("character is doing something");
        } else if (state == BattlePhrase.WaitAI)
        {
            // wait for specific button push to skip the ai perform page
            Debug.Log($"AI:{focusedCharacter} is performing");
        } else if (state == BattlePhrase.WaitResult)
        {
            // wait for specific button push to skip the resulting page
            Debug.Log("The system is resulting");
        }
    }


    

    /// <summary>
    /// 后续战斗场景开始时导入Player和NPC用
    /// </summary>
    /// <param name="battleChars"></param>
    public void InitialBattleCharacter(Dictionary<BattleCharacter, Vector2> battleChars)
    {
        foreach (BattleCharacter bc in battleChars.Keys)
        {
            if (bc.Team == 1)
            {
                var tmp = Instantiate(BattleCharacterModel, GameObject.Find("PlayerTeam").transform);
                tmp.transform.position = battleChars[bc];
            }
            else
            {
                var tmp = Instantiate(BattleCharacterModel, GameObject.Find("AITeam").transform);
                tmp.transform.position = battleChars[bc];
            }
        }
    }

    /// <summary>
    /// 鼠标点击tile辅助
    /// </summary>
    /// <returns></returns>
    public RaycastHit? GetFocusedOnTile()
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mousePos, out hit, 200, layermask))
        {
            return hit;
        }



        return null;
    }

    
    /// <summary>
    /// 移动、放置人物辅助
    /// </summary>
    /// <param name="bc"></param>
    /// <param name="tile"></param>
    private void PositionCharacterOnTile(BattleCharacter bc, BattleTile tile)
    {
        //clear history position 
        bc.activeTile.standon = null;
        bc.activeTile.isBlocked = false;

        bc.transform.position = tile.transform.position;
        bc.activeTile = tile;
        tile.standon = bc;
        tile.isBlocked = true;
    }

    /// <summary>
    /// 可行动范围显示辅助
    /// </summary>
    /// <param name="rangeNum"></param>
    private void GetInRangeTiles(int rangeNum)
    {
        foreach(var ele in inRangeTiles)
        {
            ele.HideTile();
        }

        inRangeTiles = pathfinder.GetTilesInRange(focusedCharacter.activeTile, rangeNum);

        foreach ( var ele in inRangeTiles)
        {
            ele.ShowTile();
        }
    }






    /// <summary>
    /// 战斗流程
    /// </summary>
    void BattleStart()
    {
        Debug.Log("战斗开始");
        // load characters

        SortCharacterListBySpd(true);
        focusedCharacter = CharacterList[0];

        // check character isAI
        if(focusedCharacter.Team == 1)
        {
            PlayerTurn();
        } else
        {
            StartCoroutine(AITurn());
        }

    }

    /// <summary>
    /// 队伍排列辅助
    /// </summary>
    /// <param name="initial"></param>
    private void SortCharacterListBySpd(bool initial = false)
    {
        if (initial)
        {
            CharacterList.OrderBy(x => x.GetSpeed());
        } else
        {
            CharacterList.OrderBy(x => x.GetSpeed());

            // find previous
            int tempidx = CharacterList.IndexOf(focusedCharacter);
            for (int i = 0; i < tempidx; i++)
            {
                CharacterList.Add(CharacterList[0]);
                CharacterList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// 关注下一个角色
    /// </summary>
    /// <returns></returns>
    private IEnumerator FocusNextCharacter()
    {
        // 排序
        CharacterList.Add(CharacterList[0]);
        CharacterList.RemoveAt(0);
        focusedCharacter = CharacterList[0];

        // Character refresh token & turn start action
        yield return new WaitForSeconds(1);
        if( focusedCharacter.Team == 1) // player
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(AITurn());
        }
    }

    /// <summary>
    /// 战斗结果确认辅助
    /// </summary>
    private void CheckResult()
    {
        // if one team all dead or mc died
        bool oneteamDied = false;
        if(oneteamDied)
        {
            Debug.Log("开始总结");
        } else
        {
            StartCoroutine( FocusNextCharacter());
        }

    }

    /// <summary>
    /// 当前人物UI更新器
    /// </summary>
    private void UpdateUI()
    {
        // load current character's data into UI

    }


    /// <summary>
    /// 玩家回合逻辑
    /// </summary>
    private void PlayerTurn()
    {
        UpdateUI(); // update ui and active base Action
        UpdateTokenPool(); // 
        focusedCharacter.restoreSteps();
        Debug.Log($"player:{focusedCharacter.characterData.CharacterName}的回合");
        state = BattlePhrase.WaitMove; // wait action select
    }

    /// <summary>
    /// 根据focuscharacter来更新对应的token池子
    /// </summary>
    void UpdateTokenPool()
    {
        //check focus character
        if (focusedCharacter == null) Debug.LogError("找不到目标角色");

        GameObject.Find("TokenPoolUI").GetComponent<TokenPoolUI>().LoadData(focusedCharacter);
        // update UI

    }





    #region move
    public void StartMove()
    {
        Debug.Log("Wait move target");
        state = BattlePhrase.WaitMove;
    }

    /// <summary>
    /// 移动控制器
    /// </summary>
    /// <param name="currentchar"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator MoveByPath(BattleCharacter currentchar, List<BattleTile> path)
    {
        if (path.Count == 0)
        {
            state = BattlePhrase.Idle;
            yield break;
        }
        Debug.Log(path.Count);
        var step = moveSpeed * Time.deltaTime;
        float yIndex = path[0].transform.position.y;

        if (path.Count > 0) state = BattlePhrase.WaitAction;

        while (path.Count > 0)
        {
            yield return new WaitForEndOfFrame();
            currentchar.transform.position = Vector3.MoveTowards(currentchar.transform.position, path[0].transform.position, step);
            currentchar.transform.position = new Vector3(currentchar.transform.position.x, yIndex, currentchar.transform.position.z);
            if (Vector3.Distance(currentchar.transform.position, path[0].transform.position) < 0.001f)
            {
                currentchar.steps--;

                PositionCharacterOnTile(currentchar, path[0]);
                path.RemoveAt(0);
                GetInRangeTiles(currentchar.steps);
                if (currentchar.steps == 0)
                {
                    path.Clear();
                }
            }

        }

        if (currentchar.steps != 0)
        {
            state = BattlePhrase.WaitMove;
        }
        else
        {
            state = BattlePhrase.Idle;
        }
    }

    /// <summary>
    /// 坐标到坐标移动辅助
    /// </summary>
    /// <param name="currentchar"></param>
    /// <param name="targetTile"></param>
    /// <returns></returns>
    public IEnumerator MoveToTile(BattleCharacter currentchar, BattleTile targetTile)
    {
        //Move test
        path = pathfinder.FindPath(focusedCharacter.activeTile, targetTile);
        isMoving = true;
        // 被阻碍
        if (path.Count == 0)
        {
            state = BattlePhrase.Idle;
            yield break;
        }
        var step = moveSpeed * Time.deltaTime;
        float yIndex = path[0].transform.position.y;

        if (path.Count > 0) state = BattlePhrase.WaitAction;

        while (path.Count > 0)
        {
            yield return new WaitForEndOfFrame();
            currentchar.transform.position = Vector3.MoveTowards(currentchar.transform.position, path[0].transform.position, step);
            currentchar.transform.position = new Vector3(currentchar.transform.position.x, yIndex, currentchar.transform.position.z);
            if (Vector3.Distance(currentchar.transform.position, path[0].transform.position) < 0.001f)
            {

                PositionCharacterOnTile(currentchar, path[0]);
                path.RemoveAt(0);
                GetInRangeTiles(currentchar.steps);
            }

        }

        if (currentchar.steps != 0)
        {
            state = BattlePhrase.WaitMove;
        }
        else
        {
            state = BattlePhrase.Idle;
        }

    }
    #endregion



    #region token
    /// <summary>
    /// 读取token按钮辅助
    /// </summary>
    public void StartTokenProcess()
    {
        Debug.Log("Wait token target");

        //check how many target need to choose
        var tempqueue = GameObject.Find("TokenQueueUI").GetComponent<TokenQueueUI>().GetTokenQueue();
        foreach (var a in tempqueue)
        {
            Debug.Log(a.tokenName);
        }
        tokenQueue = tempqueue;

        state = BattlePhrase.WaitToken;
    }

    

    #endregion


    #region AI
    /// <summary>
    /// AI 回合逻辑
    /// </summary>
    /// <returns></returns>
    private IEnumerator AITurn()
    {
        Debug.Log($"AI:{focusedCharacter.characterData.CharacterName}回合");
        //check sight


        // move 
        focusedCharacter.restoreSteps();
        path = pathfinder.FindPath(focusedCharacter.activeTile, GameObject.Find("PlayerTeam").transform.GetChild(0).GetComponent<BattleCharacter>().activeTile);
        Debug.Log(focusedCharacter.activeTile.gridPos);
        Debug.Log(GameObject.Find("PlayerTeam").transform.GetChild(0).GetComponent<BattleCharacter>().activeTile.gridPos);
        isMoving = true;
        yield return StartCoroutine(MoveByPath(focusedCharacter, path));

        // item & attack\
        StartCoroutine(DisplayAttackText($"{focusedCharacter.characterData.CharacterName}攻击了{GameObject.Find("PlayerTeam").transform.GetChild(0).GetComponent<BattleCharacter>().activeTile.standon.characterData.CharacterName}"));


        // finish action 
        CheckResult();
    }

    #endregion



}
