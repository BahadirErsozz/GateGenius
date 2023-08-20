using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;





public class TabletManager : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] string[] Inputs;
    [SerializeField] string[] Outputs;
    [SerializeField] float stepCooldown;
    [SerializeField] Transform inHolder;
    [SerializeField] Transform outHolder;




    private int stepNum;
    private bool inStep;
    private bool inValuation;
    private float nextStep;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        run();


        if (Input.GetKey(KeyCode.Alpha5)){
            runValuation();
        }
    }


    public void runValuation()
    {
        inValuation = true;
        stepNum = 0;
        inStep = false;
    }

    void run()
    {
        if (inValuation)
        {
            if(Time.time < nextStep)
            {
                Debug.Log("waiting");
                return;
            }


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
        GameObject.Find("GameManager").GetComponent<GameManager>().TriggerWonStage(id);
    }

    void stepSuccess()
    {
        Debug.Log("Step Succed");
    }

    void failure()
    {
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
}
