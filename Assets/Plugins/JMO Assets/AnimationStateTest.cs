using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public class AnimationStateTest : MonoBehaviour
{
    public enum AnimeStateTest
    {
        [Description("��ֹ")]
        Idle,
        [Description("�ƶ�")]
        Move,
        [Description("����������")]
        BaseBowAttack,
        [Description("����")]
        Injured,
        [Description("����")]
        Die
    }

    public AnimeStateTest AnimeState = AnimeStateTest.Idle;
}
