using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class CPRgameMode : MonoBehaviour
{
    public static CPRgameMode instance = null;
    void Awake()
    {
        instance = this;
    }

    //UI
    [SerializeField]
    public GameObject StartUI;

    public bool isPerfect = true;


    public GameObject ChestPanel;
    public GameObject DetailPlaying;
    public GameObject breathDetail;
    public GameObject BreathPanel;



    public GameObject cpr;
    private CPRPlayerAnimation pa;

    public GameObject Patient;
    private CPRKeyEvent ke;
    public PlayerHP_Bar HP;


    public GameObject ending;
    public Text endingscore;
    public Text endingresult;



    // AED panel
    public GameObject AEDdetail_1;
    public GameObject AEDdetail_2;
    public GameObject AEDQuiz;
    public GameObject AEDdetail_4;
    public GameObject AEDdetail_5;
    public GameObject AEDdetail_6;

    public void AED1to2()
    {
        AEDdetail_1.SetActive(false);
        AEDdetail_2.SetActive(true);
    }

    public void AED2to3()
    {
        AEDdetail_2.SetActive(false);
        AEDQuiz.SetActive(true);
    }

    private int AEDQuiz_n = 0;


    public void AEDQuiztrue()
    {
        AEDQuiz_n += 1;
    }
    public void AEDQuizfalse()
    {
        AEDQuiz_n += 1;
        isPerfect = false ;
    }
    public void AEDQuizto4()
    {
        AEDQuiz.SetActive(false);
        AEDdetail_4.SetActive(true);
    }
    public void AED4to5()
    {
        AEDdetail_4.SetActive(false);
        AEDdetail_5.SetActive(true);
    }
    public void AED5to6()
    {
        AEDdetail_5.SetActive(false);
        AEDdetail_6.SetActive(true);
    }





    public Text uncorrectTXT;




    void Start()
    {
        StartUI.SetActive(false);
        ChestPanel.SetActive(false);
        DetailPlaying.SetActive(false);
        breathDetail.SetActive(false);
        BreathPanel.SetActive(false);
        ending.SetActive(false);


        uncorrectTXT.text = "";

        pa = cpr.GetComponentInChildren<CPRPlayerAnimation>();
        ke = Patient.GetComponentInChildren<CPRKeyEvent>();

        HP = Patient.GetComponentInChildren<PlayerHP_Bar>();
        // 3초 후에 MyFunction 함수 실행
        Invoke("start_setting", 3f);

    }

    public void start_setting()
    {
        Camera.main.GetComponent<CPRCameraMovement>().enabled = false;
        StartUI.SetActive(true);
    }

    public void StartBtn()
    {

        Camera.main.GetComponent<CPRCameraMovement>().enabled = true;
        StartUI.SetActive(false);

    }

    public void chestTruenext()
    {
        isPerfect = true;
        ChestPanel.SetActive(false);

        DetailPlaying.SetActive(true) ;
    }
    public void chestFalsenext()
    {
        ChestPanel.SetActive(false);
        isPerfect = false;
        DetailPlaying.SetActive(true);
    }

    public void chestDetail()
    {
        DetailPlaying.SetActive(false);
        BreathPanel.SetActive(true);
    }

    void Update()
    {
        if(AEDQuiz_n == 2)
        {
            AEDQuiz_n = 0; // 초기화
            AEDQuizto4();
        }
    }

    public void BreathTruenext()
    {
        BreathPanel.SetActive(false);

        breathDetail.SetActive(true);
    }
    public void BreathFalsenext()
    {
        BreathPanel.SetActive(false);
        isPerfect = false;
        breathDetail.SetActive(true);
    }

    public void breathDetailnext() // cpr 시나리오 끝
    {
        breathDetail.SetActive(false);
        cprAEDending();
        isPerfect = true;
    }
    public void AED6tonext() // aed 시나리오 끝
    {
        AEDdetail_6.SetActive(false);
        cprAEDending();

        isPerfect = true;
    }

    public void cprAEDending()
    {
        pa._isCPR = false;


        ke.is_statQ = false;
        pa._isCPR = true;

        if (!isPerfect)
        {
            uncorrectTXT.text = "정확하지 않은 응급처치";
            Invoke("RMtxt", 3f);
        }
        Invoke("CPR_anime", 3f);
        if (isPerfect)
        {
            uncorrectTXT.text = "정확한 응급처치";
            Invoke("RMtxt", 3f);
            Invoke("Ending", 3f);
        }
    }

    public void RMtxt()
    {
        uncorrectTXT.text = "";
    }

    public void Ending()
    {
        ending.SetActive(true);
        int score = (int)HP.currenthp;

        string resultText ="";
        string scoreText = "";

        if (HP.currenthp < 50)
        {
            resultText = "최적의 골든 타임은 아니지만 그래도 알맞은 응급처치는 하였습니다\n 한 사람의 생명을 구했습니다!";
            scoreText = $"결과 : 5 분 이상 소요\nscore:{score}";
        }
        else if (HP.currenthp > 50 && HP.currenthp <= 100)
        {
            resultText = "축하합니다!\n정확하고 신속한 응급처치 덕에 \n 한 사람의 생명을 구했습니다!!";
            scoreText = $"결과 : 5 분 이내 소요\nscore:{score}";
        }
        else if (HP.currenthp == 0)
        {
            resultText = "정확하지 못한 응급처치 때문에 한 사람의 목숨을 구하지 못했습니다.\n 오른쪽 CPR 가이드를 참고하여 주세요!";
            scoreText = $"결과 : 10 분 초과\nscore:{score}";
        }

        endingscore.text = scoreText;
        endingresult.text = resultText;

        LocalPlayerManager.instance.Score += score;
    }
    public void CPR_anime()
    {
        pa._isCPR = false;
    }
    public void ToLobbyBtn()
    {
        SceneManager.LoadScene("Lobby");
    }
}