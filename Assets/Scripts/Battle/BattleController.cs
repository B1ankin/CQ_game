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
    private bool battleStartSign;

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
    private BattleEffects beffects;
    private EntryTable etable;

    // UI support 
    public GameObject battleWaitQueuelele;


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
        beffects = new BattleEffects();
        etable = new EntryTable();
        battleStartSign = false;


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

        if ( Input.GetKeyUp(KeyCode.Space))
        {
            if (!battleStartSign)
            {
                if(CharacterList.Count == 0)
                {
                    StartCoroutine(DisplayAttackText("没添加人物"));
                    return;
                }
                BattleStart();

            } else
            {
                CheckResult(); // 手动确认下一个
            }
        }
        
        else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            CheckResult();
        }
        
        



        if (isTest && !battleStartSign) // character generator 
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var hit = GetFocusedOnTile();
                if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    BattleTile battleTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                    if (battleTile.standon == null)
                    {
                        var testc = Instantiate(BattleCharacterModel, GameObject.Find("PlayerTeam").transform).GetComponent<BattleCharacter>();
                        testc.characterData.CharacterName = "player" + UnityEngine.Random.Range(0, 10);
                        testc.activeTile = battleTile;
                        PositionCharacterOnTile(testc, battleTile);
                        CharacterList.Add(testc);
                    }
                    
                    
                }
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                var hit = GetFocusedOnTile();
                if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                {
                    BattleTile battleTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                    if (battleTile.standon == null) // 确认没占位
                    {
                        var testc = Instantiate(BattleCharacterModel, GameObject.Find("AITeam").transform).GetComponent<BattleCharacter>();
                        testc.Team = 0;
                        testc.characterData.CharacterName = "AI" + UnityEngine.Random.Range(0, 10);
                        testc.activeTile = battleTile;
                        PositionCharacterOnTile(testc, battleTile);
                        CharacterList.Add(testc);
                    }
                    
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
            if (focusedCharacter.steps <= 0)
            {
                if( focusedCharacter.tokenSlots <= 0)
                {
                    state = BattlePhrase.Idle;
                    CheckResult(); // 移动自动确认下一个
                    return;
                }
                state = BattlePhrase.WaitToken;
                return;
            }
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
            if(focusedCharacter.tokenSlots <= 0)
            {
                if (focusedCharacter.steps > 0)
                {
                    state = BattlePhrase.WaitMove;
                    return;
                }


                state = BattlePhrase.Idle;
                CheckResult(); // token 自动确认一下一个
                return;
            }


            // 显示目标范围 -- shape
            var hit = GetFocusedOnTile();
            if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
            {
                BattleTile battleTile = hit.Value.collider.gameObject.GetComponent<BattleTile>();
                var mousePos = battleTile.gridPos;
                var charPos = focusedCharacter.activeTile.gridPos;
                var norm = (mousePos - charPos);
                var newTokenTargetDirection = -1;

                // 判断鼠标朝向
                if (norm.x > 0)
                {
                    newTokenTargetDirection = 4;
                }
                else
                {
                    newTokenTargetDirection = 2;
                }
                if (norm.z > Mathf.Abs(norm.x))
                {
                    newTokenTargetDirection = 3;
                }
                else if (norm.z * -1 > Mathf.Abs(norm.x))
                {
                    newTokenTargetDirection = 1;
                }


                if (true)//tokenTargetDirection != newTokenTargetDirection)
                {
                    focusedCharacter.SetDirection(newTokenTargetDirection);

                    tokenTargetDirection = newTokenTargetDirection;
                    List<Vector3Int> tiles = new List<Vector3Int>();
                    /*if (tokenTargetDirection == 4)
                    {
                        tiles.Add(focusedCharacter.activeTile.gridPos + new Vector3Int(1, 0, 0));
                        tiles.Add(focusedCharacter.activeTile.gridPos + new Vector3Int(1, 0, 0));
                    }
                    else if(tokenTargetDirection == 2)
                    {
                        tiles.Add(focusedCharacter.activeTile.gridPos + new Vector3Int(-1, 0, 0));
                    } else if (tokenTargetDirection == 3)
                    {
                        tiles.Add(focusedCharacter.activeTile.gridPos + new Vector3Int(0,0,1));
                    } else if (tokenTargetDirection == 1)
                    {
                        tiles.Add(focusedCharacter.activeTile.gridPos + new Vector3Int(0, 0, -1));
                    } else
                    {
                        Debug.Log("光标相对方向判断错误");
                    }*/

                    tiles.Add(battleTile.gridPos);


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
                        {


                            // 攻击指示
                            Token t_action = null ;
                            List<Token> t_support_list  = new List<Token>();
                            foreach (  var a in tokenQueue)
                            {
                                if (a.GetType() == typeof(ActionToken)) t_action = a;
                                else if (a.GetType() == typeof(SupportToken))  t_support_list.Add(a);
                                else if (a.GetType() == typeof(SpecialToken)) t_action = a;
                            }
                            
                            // 执行token内容 
                            if( t_action == null)
                            {
                                StartCoroutine(DisplayAttackText("Token池子缺少Action 或 Special Token"));
                            } else
                            {
                                StartCoroutine(DisplayAttackText($"{focusedCharacter.characterData.CharacterName}攻击了{targetTile.standon.characterData.CharacterName}，造成{focusedCharacter.GetDamage()}点伤害"));


                                beffects.tokenProcess(t_action, t_support_list, focusedCharacter, targetTile);

                                // update UI

                                focusedCharacter.tokenSlots -= 1 + t_support_list.Count;
                                UpdateUI();

                                // reset state
                                if (focusedCharacter.steps > 0)
                                {
                                    state = BattlePhrase.WaitMove;
                                }
                                else if (focusedCharacter.tokenSlots <= 0)
                                {
                                    CheckResult();
                                }
                            }



                        }



                        //Update token highlighter

                        //record target pos


                        // depends on the the token pool's status, wait numbers of targeting select

                    }
                } else if ( Input.GetMouseButtonDown(1))
                {

                    CheckResult();
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


    public List<Vector3Int> AddShapeMatrix(Vector3Int refVector, int shapeIndex)
    {
        var ret = new List<Vector3Int>();
        var temp = etable.GetShapeMatrix(shapeIndex);



        return ret;
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




    void BattleReset()
    {
        for(int i = 0; i <  GameObject.Find("PlayerTeam").transform.childCount; i++)
        {
            Destroy(GameObject.Find("PlayerTeam").transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameObject.Find("AITeam").transform.childCount; i++)
        {
            Destroy(GameObject.Find("AITeam").transform.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < GameObject.Find("BattleWaitQueue").transform.childCount; i++)
        {
            Destroy(GameObject.Find("BattleWaitQueue").transform.GetChild(i).gameObject);
        }
        focusedCharacter = null;
        CharacterList = new List<BattleCharacter>();
        state = BattlePhrase.Idle;
        BattleManager.Instance.ClearAllTileStandOn();
        BattleManager.Instance.ClearAllTileHighlighter();
        battleStartSign = false;
    }




    /// <summary>
    /// 战斗流程
    /// </summary>
    void BattleStart()
    {
        Debug.Log("战斗开始");
        // load characters
        battleStartSign = true;
        SortCharacterListBySpd(true);

        // 生成wait queue ui内容
        Transform waitQueuet = GameObject.Find("BattleWaitQueue").transform;
        foreach (var character1 in CharacterList)
        {
            var a = Instantiate(battleWaitQueuelele);
            a.name = "WaitEle " + character1.characterData.CharacterName;
            a.transform.SetParent(waitQueuet);
            a.transform.localScale = Vector3.one;
            //a.transform.Find("waitQueueEleName").GetComponent<Text>().text = character1.characterData.CharacterName;


            if (character1.Team == 1)
            {
                a.GetComponent<Image>().color = new Color(0, 0, 1, .4f);
            }
            else
            {
                a.GetComponent<Image>().color = new Color(1, 0, 0, .4f);
            }


        }



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
        Transform waitQueuet = GameObject.Find("BattleWaitQueue").transform;


        CharacterList.Add(CharacterList[0]);
        CharacterList.RemoveAt(0);
        waitQueuet.GetChild(0).SetAsLastSibling();
        if (CharacterList[0].IsDead())
        {
            CharacterList.RemoveAt(0);
            Destroy(waitQueuet.GetChild(0).gameObject);
        }
        focusedCharacter = CharacterList[0];


        // Character refresh token & turn start action
        yield return new WaitForSeconds(1);
        if( focusedCharacter.Team == 1) // player
        {
            PlayerTurn();
        }
        else
        {
            ResetUI(); // 清除TOken池子剩余内容
            StartCoroutine(AITurn());
        }
    }

    /// <summary>
    /// 战斗结果确认辅助
    /// </summary>
    private void CheckResult()
    {
        bool aiDead = true;
        bool playerDead = true;

        Transform playerTeamT = GameObject.Find("PlayerTeam").transform;
        Transform AITeamT = GameObject.Find("AITeam").transform;

        // if one team all dead or mc died
        for (int i = 0; i < playerTeamT.childCount; i++)
        {
            if (!playerTeamT.GetChild(i).GetComponent<BattleCharacter>().IsDead())
            {
                playerDead = false;
                break;
            }
        }

        for (int i = 0; i < AITeamT.childCount; i++)
        {
            if (!AITeamT.GetChild(i).GetComponent<BattleCharacter>().IsDead())
            {
                aiDead = false;
                break;
            }
        }

        if (aiDead)
        {
            Debug.Log("玩家获胜");
        } else if (playerDead) {
            Debug.Log("敌方获胜");
        }
        else
        {
            StartCoroutine(FocusNextCharacter());
        }

    }

    /// <summary>
    /// 当前人物UI更新器
    /// </summary>
    private void UpdateUI()
    {
        //reset pool based on current character
        UpdateTokenPool();


        // reset queue();
        var tokenQueueUI = GameObject.Find("TokenQueueUI").GetComponent<TokenQueueUI>();
        tokenQueueUI.ResetTokenQueue();
        tokenQueueUI.UpdateTokenQueue(focusedCharacter.tokenSlots);

        // character panel
        // 现在用一个头像
        GameObject.Find("CharacterImage").GetComponent<Image>().sprite = focusedCharacter.characterData.characterImage;
        GameObject.Find("CharacterNameUI").GetComponent<Text>().text = focusedCharacter.characterData.CharacterName;
        GameObject.Find("HPBarUI").transform.localScale = new Vector3(focusedCharacter.GetHealthPercent(), 1,1);
        GameObject.Find("SPBarUI").transform.localScale = new Vector3(focusedCharacter.GetSanityPercent(), 1, 1);


    }

    private void ResetUI()
    {
        var tokenQueueUI = GameObject.Find("TokenQueueUI").GetComponent<TokenQueueUI>();
        tokenQueueUI.ResetTokenQueue();
    }


    /// <summary>
    /// 玩家回合逻辑
    /// </summary>
    private void PlayerTurn()
    {
        UpdateUI(); // update ui and active base Action


        // reset dynamic stats
        focusedCharacter.restoreSlots();
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
    /// 移动控制器 called by move and move token, TODO move TOken different animation
    /// </summary>
    /// <param name="currentchar"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator MoveByPath(BattleCharacter currentchar, List<BattleTile> path, bool updateDir = true)
    {
        if (path.Count == 0)
        {
            state = BattlePhrase.Idle;
            yield break;
        }
        Debug.Log(path.Count);
        var step = moveSpeed * Time.deltaTime;
        float yIndex = path[0].transform.position.y;

        if (path.Count > 0)
        {
            state = BattlePhrase.WaitAction;
            if (updateDir)
            {
                currentchar.SetAnimation(1);
            }
        }
        

        while (path.Count > 0)
        {
            yield return new WaitForEndOfFrame();

            if (updateDir)
            {
                var t_vec = currentchar.transform.position - path[0].transform.position;
                Debug.Log(t_vec.ToString());
                if (t_vec.x > 0) //left
                {
                    currentchar.SetDirection(2);
                }
                else if (t_vec.x < 0)
                {
                    currentchar.SetDirection(4);
                }
                else if (t_vec.z > 0)
                {
                    currentchar.SetDirection(1);
                }
                else if (t_vec.z < 0)
                {
                    currentchar.SetDirection(3);
                }
            }

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

        // finish moving action
        currentchar.SetAnimation(0);



        if (currentchar.steps != 0)
        {
            state = BattlePhrase.WaitMove;
        }
        else
        {
            state = BattlePhrase.Idle;
        }
    }


    public void MoveToTile1(BattleCharacter currentchar, BattleTile targetTile)
    {
        StartCoroutine(MoveToTile(currentchar, targetTile));
    }
    /// <summary>
    /// 坐标到坐标移动辅助
    /// </summary>
    /// <param name="currentchar"></param>
    /// <param name="targetTile"></param>
    /// <returns></returns>
    private IEnumerator MoveToTile(BattleCharacter currentchar, BattleTile targetTile)
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
        BattleCharacter target = null;
        int currentMax = 999;
        for (int targetIndex = 0; targetIndex < GameObject.Find("PlayerTeam").transform.childCount; targetIndex ++) 
        {
            var tempCharacter = GameObject.Find("PlayerTeam").transform.GetChild(targetIndex).GetComponent<BattleCharacter>();
            if (tempCharacter.IsDead()) continue; // 忽略尸体
            int tempDis = tempCharacter.GetDistance(focusedCharacter);
            Debug.Log($"{tempCharacter.characterData.CharacterName} 距离 {tempDis} ");
            if (tempDis < currentMax)
            {
                currentMax = tempDis;
                target = tempCharacter;
            }
        }
        if (target == null) {
            Debug.Log("找不到敌方单位");
        }
        // move 
        focusedCharacter.restoreSteps();


        path = pathfinder.FindPath(focusedCharacter.activeTile, target.activeTile);
        Debug.Log(focusedCharacter.activeTile.gridPos);
        Debug.Log(GameObject.Find("PlayerTeam").transform.GetChild(0).GetComponent<BattleCharacter>().activeTile.gridPos);
        isMoving = true;
        yield return StartCoroutine(MoveByPath(focusedCharacter, path));
        // face targetCharacter
        var targetPos = target.activeTile.gridPos;
        var charPos = focusedCharacter.activeTile.gridPos;
        var norm = (targetPos - charPos);
        var newTokenTargetDirection = -1;

        // 判断鼠标朝向
        if (norm.x > 0)
        {
            newTokenTargetDirection = 4;
        }
        else
        {
            newTokenTargetDirection = 2;
        }
        if (norm.z > Mathf.Abs(norm.x))
        {
            newTokenTargetDirection = 3;
        }
        else if (norm.z * -1 > Mathf.Abs(norm.x))
        {
            newTokenTargetDirection = 1;
        }
        focusedCharacter.SetDirection(newTokenTargetDirection);


        // item & attack\
        if (target.GetDistance(focusedCharacter) < 2) // 横竖范围2确认
        {
            // 攻击计算和执行
            var alive = focusedCharacter.TestDamage(target); // 默认ai的技能
            
            // 攻击效果
            StartCoroutine(DisplayAttackText($"{focusedCharacter.characterData.CharacterName}攻击了{GameObject.Find("PlayerTeam").transform.GetChild(0).GetComponent<BattleCharacter>().activeTile.standon.characterData.CharacterName}"));

        
        } else
        {
            Debug.Log("目标在范围外");
        }


        // finish action 
        CheckResult();
    }

    #endregion



}
