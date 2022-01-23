using UnityEngine;
using UnityEngine.UI;

public class QuizManage : MonoBehaviour
{
    General general;

    public InputField inputFieldName;
    public Text warningTextInputForName;
    public GameObject QuizContainer;
    public GameObject panelInputName;
    public GameObject panelMain;
    public GameObject panelResultAnswer;
    public GameObject iconCheck, iconX;
    public Text txtResult, txtScore;
    public GameObject panelEndQuiz;
    public GameObject iconNewScore;
    public Text txtNameStudent;
    public Text txtScoreStudent;
    public Text txtHighScoreStudent;
    
    public AudioSource audioSourceFinishSad, audioSourceFinishFun;
    public AudioSource audioSourceCorrect, audioSourceIncorrect;


    public Text scoreInViewText;
    public Text timeForAnswerText;
    public Text numOfQuestionText;

    private int answered = 0;
    private int sumOfQuestion;
    private static float timeForAnswer = 15f;
    private static float timeForPopUp = 1.5f;

    private string p_studentName = "";
    private float p_timeForAnswer = timeForAnswer;
    private float p_timeForPopUp = timeForPopUp;
    private float p_score = 0;
    private float p_highScore;
    private int p_numOfQuestion = 0;

    private bool p_quizStart = false;
    private bool p_popUp = false;


    private void Awake()
    {
        general = new General();

        sumOfQuestion = QuizContainer.transform.childCount;

        p_highScore = PlayerPrefs.GetFloat("highscore");

        audioSourceFinishSad = audioSourceFinishSad.GetComponent<AudioSource>();
        audioSourceFinishFun = audioSourceFinishFun.GetComponent<AudioSource>();
        audioSourceIncorrect.GetComponent<AudioSource>();
        audioSourceCorrect.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (p_quizStart)
        {
            timeForAnswerText.text = "Time: " +((int)p_timeForAnswer).ToString();
            p_timeForAnswer -= Time.deltaTime;
            if(p_timeForAnswer <= 0f)
            {
                PopUpResultAnswer("late");
                p_popUp = true;
                p_quizStart = false;
                p_timeForAnswer = timeForAnswer;
            }
        }

        if(p_popUp)
        {
            p_timeForPopUp -= Time.deltaTime;
            if(p_timeForPopUp <= 0)
            {
                ClosePopUpResultAnswer();
                p_popUp = false;
                p_timeForPopUp = timeForPopUp;
            }
        }
    }

    private void ShowQuiz()
    {
        p_quizStart = true;
        p_numOfQuestion += 1;
        scoreInViewText.text = "Score : " +((p_score/sumOfQuestion)*100f).ToString();
        numOfQuestionText.text = p_numOfQuestion.ToString();
        RandomQuiz(QuizContainer.transform);
    }

    private void RandomQuiz(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        int q = Random.Range(0, transform.childCount);

        if(transform.GetChild(q).name != "Answered")
        {
            answered += 1;
            transform.GetChild(q).gameObject.SetActive(true);
            transform.GetChild(q).name = "Answered";
            RandomAnswer(transform.GetChild(q));
        }
        else
        {
            if (answered < transform.childCount)
            {
                RandomQuiz(transform);
            }
            else
            {
                p_quizStart = false;
                PopUpEndQuiz();
            }
        }

    }

    private void RandomAnswer(Transform transform)
    {
        Vector3[] pos_button = new Vector3[transform.childCount];
        bool[] terisi = new bool[transform.childCount];

        for (int i = 0; i < pos_button.Length; i++)
        {
            pos_button[i] = transform.GetChild(i).position;
            terisi[i] = false;
        }

        for (int i = 0; i < pos_button.Length; i++)
        {
            bool loop = true;
            while (loop)
            {
                int pos_random = Random.Range(0, transform.childCount);
                if (!terisi[pos_random])
                {
                    transform.GetChild(i).transform.position = pos_button[pos_random];
                    terisi[pos_random] = true;
                    loop = false;
                }
                else
                {
                    loop = true;
                }
            }
        }
    }

    public void CorrectAnswer()
    {
        PopUpResultAnswer("correct");
        p_popUp = true;
        p_quizStart = false;
        p_timeForAnswer = timeForAnswer;
    }

    public void IncorrectAnswer()
    {
        PopUpResultAnswer("incorrect");
        p_popUp = true;
        p_quizStart = false;
        p_timeForAnswer = timeForAnswer;
    }


    private void PopUpResultAnswer(string result)
    {
        panelMain.SetActive(false);

        switch(result)
        {
            case "late" :
                audioSourceIncorrect.PlayOneShot(audioSourceIncorrect.clip);
                iconX.SetActive(true);
                txtResult.text = "WAKTU HABIS!"; break;
            case "incorrect":
                audioSourceIncorrect.PlayOneShot(audioSourceIncorrect.clip);
                iconX.SetActive(true);
                txtResult.text = "JAWABAN SALAH"; break;
            case "correct":
                audioSourceCorrect.PlayOneShot(audioSourceCorrect.clip);
                p_score += 1f;
                iconCheck.SetActive(true);
                txtResult.text = "JAWABAN BENAR"; break;
            default :
                iconCheck.SetActive(false);
                iconX.SetActive(false);
                txtResult.text = ""; break;
        }
            
        txtScore.text = "Score : " + ((p_score/sumOfQuestion)*100f).ToString();
        panelResultAnswer.SetActive(true);
    }

    private void ClosePopUpResultAnswer()
    {
        iconCheck.SetActive(false);
        iconX.SetActive(false);
        txtResult.text = "";
        txtScore.text = "";
        panelResultAnswer.SetActive(false);
        panelMain.SetActive(true);
        ShowQuiz();
    }

    private void PopUpEndQuiz()
    {
        if((p_score / sumOfQuestion) * 100f > p_highScore)
        {
            PlayerPrefs.SetFloat("highscore", (p_score / sumOfQuestion) * 100f);
            audioSourceFinishFun.PlayOneShot(audioSourceFinishFun.clip);
            iconNewScore.SetActive(true);
        }
        else
        {
            audioSourceFinishSad.PlayOneShot(audioSourceFinishSad.clip);
            iconNewScore.SetActive(false);
        }

        panelMain.SetActive(false);
        txtNameStudent.text = p_studentName;
        txtScoreStudent.text = "Score : " + ((p_score / sumOfQuestion) * 100f).ToString();
        txtHighScoreStudent.text = "Highscore : " +p_highScore.ToString();
        panelEndQuiz.SetActive(true);
    }


    #region NAVIGATION MANAGE

    public void MScene(string scene)
    {
        general.MScene(scene);
    }

    public void MMenu(GameObject menu)
    {
        if(menu.name == "PanelMain")
        {
            if (string.IsNullOrEmpty(inputFieldName.text))
            {
                warningTextInputForName.gameObject.SetActive(true);
                inputFieldName.placeholder.gameObject.SetActive(false);
                return;
            }
            else
            {
                CMenu(panelInputName);
                p_studentName = inputFieldName.text;
                ShowQuiz();
            }
        }

        general.MMenu(menu);
    }

    public void CMenu(GameObject menu)
    {
        general.CMenu(menu);
    }

    #endregion NAVIGATION MANAGE

}
