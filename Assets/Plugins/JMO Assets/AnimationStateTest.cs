using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public class AnimationStateTest : MonoBehaviour
{
    public enum AnimeStateTest
    {
        [Description("¾²Ö¹")]
        Idle,
        [Description("ÒÆ¶¯")]
        Move,
        [Description("»ù´¡¹­¹¥»÷")]
        BaseBowAttack,
        [Description("ÊÜÉË")]
        Injured,
        [Description("ËÀÍö")]
        Die
    }

    public AnimeStateTest AnimeState = AnimeStateTest.Idle;
}
