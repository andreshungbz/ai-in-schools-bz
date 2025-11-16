using UnityEngine;
using Unity.VisualScripting;
using TMPro;

[UnitTitle("Play Dialogue Line")]
[UnitCategory("Dialogue")]
public class PlayDialogueLineUnit : Unit
{
    [DoNotSerialize] public ControlInput inputTrigger;
    [DoNotSerialize] public ControlOutput outputTrigger;

    [DoNotSerialize] public ValueInput audioSource;
    [DoNotSerialize] public ValueInput panelTextInput;
    [DoNotSerialize] public ValueInput textInput;
    [DoNotSerialize] public ValueInput fallbackDuration;

    [DoNotSerialize] public ValueOutput durationOut;
    [DoNotSerialize] public ValueOutput panelTextOut;

    protected override void Definition()
    {
        // Flow
        inputTrigger = ControlInput("In", Play);
        outputTrigger = ControlOutput("Out");
        Succession(inputTrigger, outputTrigger);

        // Inputs
        audioSource = ValueInput<AudioSource>("AudioSource");
        panelTextInput = ValueInput<TMP_Text>("PanelText");
        textInput = ValueInput<string>("Text", "");
        fallbackDuration = ValueInput<float>("FallbackSeconds", 2f);

        // Outputs
        durationOut = ValueOutput<float>("Duration");
        panelTextOut = ValueOutput<TMP_Text>("PanelTextOut");

        // Requirements
        Requirement(audioSource, inputTrigger);
        Requirement(panelTextInput, inputTrigger);
        Requirement(textInput, inputTrigger);
    }

    private ControlOutput Play(Flow flow)
    {
        var src = flow.GetValue<AudioSource>(audioSource);
        var panelText = flow.GetValue<TMP_Text>(panelTextInput);
        var lineText = flow.GetValue<string>(textInput);
        float fallback = flow.GetValue<float>(fallbackDuration);

        float duration = fallback;

        // Update text
        if (panelText != null)
            panelText.text = lineText;

        // Play audio & get duration
        if (src != null)
        {
            src.enabled = true;
            src.gameObject.SetActive(true);

            if (src.clip != null)
            {
                src.Stop();
                src.Play();
                duration = src.clip.length;
            }
        }

        // Output both values
        flow.SetValue(durationOut, duration);
        flow.SetValue(panelTextOut, panelText); // persistent TMP_Text

        return outputTrigger;
    }
}
