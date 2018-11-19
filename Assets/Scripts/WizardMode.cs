using UnityEngine;

public class WizardMode : MonoBehaviour
{
    private SchedulingSystem schedule;

    public bool IsWizardMode { get; private set; }
    public bool RenderAll { get; private set; }

    public void AddEnergy()
    {
        schedule.CurrentActor.GetComponent<Energy>().RestoreEnergy(2000, false);
    }

    public void DrinkPotion()
    {
        if (schedule.CurrentActor.GetComponent<Potion>().HasEnoughPotion(1))
        {
            schedule.CurrentActor.GetComponent<Potion>().DrinkPotion();
        }
    }

    public void GainHP()
    {
        schedule.CurrentActor.GetComponent<Potion>().GainPotion(90);
        //schedule.CurrentActor.GetComponent<Stress>().GainStress(1);
        //schedule.CurrentActor.GetComponent<HP>().GainHP(2);
    }

    public void Initialize()
    {
        FindObjects.GameLogic.GetComponent<Initialize>().InitializeGame();
    }

    public void LoseHP()
    {
        //schedule.CurrentActor.GetComponent<Potion>().LosePotion(5);
        //schedule.CurrentActor.GetComponent<Stress>().ClearStress();
        schedule.CurrentActor.GetComponent<HP>().LoseHP(2);
    }

    public void PrintEnergy()
    {
        schedule.CurrentActor.GetComponent<Energy>().PrintEnergy();
    }

    public void PrintSchedule()
    {
        schedule.PrintSchedule();
    }

    public void SwitchRenderAll()
    {
        RenderAll = !RenderAll;
    }

    private void Awake()
    {
        IsWizardMode = true;
        RenderAll = false;
    }

    private void Start()
    {
        schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
    }
}
