using UnityEngine;

public class WizardMode : MonoBehaviour
{
    private SchedulingSystem schedule;

    public bool IsWizardMode { get; private set; }

    public void AddEnergy()
    {
        schedule.CurrentActor.GetComponent<Energy>().RestoreEnergy(2000, false);
    }

    public void Initialize()
    {
        FindObjects.GameLogic.GetComponent<Initialize>().InitializeGame();
    }

    public void PrintEnergy()
    {
        schedule.CurrentActor.GetComponent<Energy>().PrintEnergy();
    }

    public void RenderAll()
    {
        FindObjects.GameLogic.GetComponent<Test>().RenderAll
            = !FindObjects.GameLogic.GetComponent<Test>().RenderAll;
    }

    private void Awake()
    {
        IsWizardMode = true;
    }

    private void Start()
    {
        schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
    }
}
