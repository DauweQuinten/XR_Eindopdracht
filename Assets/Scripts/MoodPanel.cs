using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodPanel : MonoBehaviour
{
    [SerializeField] Transform xrRig;
    [SerializeField] SpriteRenderer iconPlaceholder;

    [SerializeField] Sprite HungryIcon;
    [SerializeField] Sprite PetMeIcon;
    [SerializeField] Sprite boredIcon;
    Dictionary<Mood, Sprite> SpriteMap = new Dictionary<Mood, Sprite>();

    private Mood currentMood = Mood.neutral;

    
    public enum Mood
    {
        Hungry,
        Sad,
        bored,
        neutral
    }


    private void Awake()
    {
        SpriteMap.Add(Mood.Hungry, HungryIcon);
        SpriteMap.Add(Mood.Sad, PetMeIcon);
        SpriteMap.Add(Mood.bored, boredIcon);
        SpriteMap.Add(Mood.neutral, null);
    }

    // Update is called once per frame
    void Update()
    {
        LookAtOrigin();
        iconPlaceholder.sprite = SpriteMap[currentMood];
    }

    
    public void SetMood(Mood mood)
    {
        currentMood = mood;
    }

    public Mood GetMood()
    {
        return currentMood;
    }

    void LookAtOrigin()
    {
        Vector3 forwardVector = (xrRig.position - transform.position).normalized;
        Quaternion lookatRotation = Quaternion.LookRotation(forwardVector, Vector3.up);
        lookatRotation.z = 0f;
        lookatRotation.x = 0f;
        transform.rotation = lookatRotation;
    }
}
