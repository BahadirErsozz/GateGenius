using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;





public class TabletManager : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] string[] Inputs;
    [SerializeField] string[] Outputs;
    [SerializeField] float stepCooldown;
    [SerializeField] Transform inHolder;
    [SerializeField] Transform outHolder;
    [SerializeField] Transform wireHolder;
    [SerializeField] Transform chipHolder;
    [SerializeField] TMP_Text evaulationText;
    [SerializeField] float textWaitDuration = 2;




    private int stepNum;
    private bool inStep;
    private bool inValuation;
    private float nextStep;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        evaulationText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        run();
    }


    public void runValuation()
    {
        inValuation = true;
        stepNum = 0;
        inStep = false;
        evaulationText.gameObject.SetActive(true);
    }
    public void resetTable()
    {
        foreach(Transform c in chipHolder)
        {
            Destroy(c.gameObject);
        }
        foreach (Transform c in wireHolder)
        {
            Destroy(c.gameObject);
        }
    }

    void run()
    {
        if (inValuation)
        {
            
            if (Time.time < nextStep)
            {
                Debug.Log("waiting");
                return;
            }
            evaulationText.text = "Evaulating(" + (stepNum + 1) + "/" + (Outputs.Length) + ")";

            //valuation
            if (inStep)
            {
                bool result = valuateOutputs(Outputs[stepNum]);

                if (!result)
                {
                    inValuation = false;
                    inStep = false;
                    failure();
                }

                inStep = false;
                stepNum++;
                stepSuccess();

            }
            //set inputs
            else
            {
                if (stepNum >= Inputs.Length)
                {
                    inValuation = false;
                    inStep = false;
                    puzzleWon();
                    return;
                }
                assingInputs(Inputs[stepNum]);              
                inStep = true;
            }
            nextStep = Time.time + stepCooldown;
        }
    }


    void puzzleWon()
    {
        Debug.Log("Puzzle Won");
        StartCoroutine(textFeedback("Congratulations!"));
        GameObject.Find("GameManager").GetComponent<GameManager>().TriggerWonStage(id);
    }

    void stepSuccess()
    {
        Debug.Log("Step Succed");
    }

    void failure()
    {
        player.TakeDamage(20);
        StartCoroutine(textFeedback("Failed!"));
        Debug.Log("Puzzle Failed");
    }

    void assingInputs(string inputs)
    {
        List<IOPin> ins = GetChildNodes(inHolder);

        for (int i = 0; i < ins.Count; i++)
        {
            Pin.HighlightState state = (inputs[i] == '1' ? Pin.HighlightState.Highlighted : Pin.HighlightState.None);
            ins[i].SetHighlightState(state);
            Debug.Log("Name: " + ins[i].name +" step: " + stepNum + "input: " + state + " out:" + "real:" + ins[i].state);
        }
    }

    bool valuateOutputs(string expecteds)
    {
        List<IOPin> outs = GetChildNodes(outHolder);

        for(int i = 0; i < outs.Count; i++)
        {
            Pin.HighlightState expected = (expecteds[i] == '1'? Pin.HighlightState.Highlighted : Pin.HighlightState.None);
            Debug.Log("step: " + stepNum+"expected: " + expected +" out:" + "out:" + outs[i].state);
            if (outs[i].state != expected)
            {
                return false;
            }
        }
        return true;
    }



    List<IOPin> GetChildNodes(Transform parent)
    {
        List<IOPin> children = new List<IOPin>();

        foreach( Transform child in parent)
        {
            children.Add(child.GetComponent<IOPin>());
        }
        return children;
    }


    private IEnumerator textFeedback(string text)
    {
        evaulationText.text = text;
        yield return new WaitForSeconds(textWaitDuration);
        evaulationText.gameObject.SetActive(false);

    }
}
