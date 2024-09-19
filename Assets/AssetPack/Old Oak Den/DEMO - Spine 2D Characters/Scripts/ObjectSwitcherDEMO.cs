using System.Collections.Generic;
using UnityEngine;
using TMPro; // Pøidáme TextMeshPro namespace
using Spine;
using Spine.Unity;

namespace ApocalypseHeroesSpine2DCharactersDEMO
{
public class ObjectSwitcherDEMO : MonoBehaviour
{
    public List<GameObject> demoObjects; // seznam objekt?ve scén?
    public TMP_Dropdown demoCharacterDropdown; // roletkov?menu pro výbìr postavy
    public TMP_Dropdown demoAnimationDropdown; // roletkov?menu pro výbìr animace
    public TMP_Dropdown demoSkinDropdown; // roletkov?menu pro výbìr skinu

    private SkeletonAnimation currentSkeletonAnimation;
    private string currentAnimationName;

    void Start()
    {
        // na zaèátku nastavíme všechny objekty jako neaktivn?
        foreach (GameObject obj in demoObjects)
        {
            obj.SetActive(false);
        }

        // naplníme roletkov?menu pro výbìr postavy
        demoCharacterDropdown.ClearOptions();
        List<string> characterNames = new List<string>();
        foreach (GameObject obj in demoObjects)
        {
            characterNames.Add(obj.name);
        }
        demoCharacterDropdown.AddOptions(characterNames);
        demoCharacterDropdown.interactable = characterNames.Count > 1; // nastavíme roletkov?menu jako interaktivn?pouze pokud je více ne?jedna možnost výbìru

        // nastavíme akci, kter?se provede pøi výbìru postavy
        demoCharacterDropdown.onValueChanged.AddListener(delegate {
            SwitchCharacter(demoCharacterDropdown.value);
        });

        // nastavíme akci, kter?se provede pøi výbìru animace
        demoAnimationDropdown.onValueChanged.AddListener(delegate {
            PlayAnimation(demoAnimationDropdown.options[demoAnimationDropdown.value].text);
        });

        // nastavíme akci, kter?se provede pøi výbìru skinu
        demoSkinDropdown.onValueChanged.AddListener(delegate {
            SwitchSkin(demoSkinDropdown.options[demoSkinDropdown.value].text);
        });

        // aktivujeme prvn?objekt
        if (demoObjects.Count > 0)
        {
            SwitchCharacter(0);
        }
    }

    void Update()
    {
        // pøepínán?postav pomoc?šipek nahoru a dolu
        if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                demoCharacterDropdown.value = (demoCharacterDropdown.value + 1) % demoCharacterDropdown.options.Count;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                demoCharacterDropdown.value = (demoCharacterDropdown.value - 1 + demoCharacterDropdown.options.Count) % demoCharacterDropdown.options.Count;
            }
        }

        // pøepínán?animac?pomoc?šipek nahoru a dolu s drženým Alt
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                demoAnimationDropdown.value = (demoAnimationDropdown.value + 1) % demoAnimationDropdown.options.Count;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                demoAnimationDropdown.value = (demoAnimationDropdown.value - 1 + demoAnimationDropdown.options.Count) % demoAnimationDropdown.options.Count;
            }
        }

        // pøepínán?skin?pomoc?šipek nahoru a dolu s drženým Ctrl
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                demoSkinDropdown.value = (demoSkinDropdown.value + 1) % demoSkinDropdown.options.Count;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                demoSkinDropdown.value = (demoSkinDropdown.value - 1 + demoSkinDropdown.options.Count) % demoSkinDropdown.options.Count;
            }
        }
    }

    private void SwitchCharacter(int index)
    {
        // deaktivujeme aktuáln?objekt
        if (currentSkeletonAnimation != null)
        {
            currentSkeletonAnimation.gameObject.SetActive(false);
        }

        // aktivujeme nov?objekt
        GameObject newObject = demoObjects[index];
        newObject.SetActive(true);
        currentSkeletonAnimation = newObject.GetComponent<SkeletonAnimation>();

        // naplníme roletkov?menu pro výbìr animace
        demoAnimationDropdown.ClearOptions();
        List<string> animationNames = new List<string>();
        foreach (var animation in currentSkeletonAnimation.Skeleton.Data.Animations)
        {
            animationNames.Add(animation.Name);
        }
        demoAnimationDropdown.AddOptions(animationNames);
        demoAnimationDropdown.interactable = animationNames.Count > 1; // nastavíme roletkov?menu jako interaktivn?pouze pokud je více ne?jedna možnost výbìru

        // naplníme roletkov?menu pro výbìr skinu
        demoSkinDropdown.ClearOptions();
        List<string> skinNames = new List<string>();
        foreach (var skin in currentSkeletonAnimation.Skeleton.Data.Skins)
        {
            skinNames.Add(skin.Name);
        }
        if (skinNames.Count == 0) // pokud neexistuj?žádn?skiny, pøidáme možnost "default"
        {
            skinNames.Add("default");
        }
        else if (skinNames.Count > 1 && skinNames.Contains("default")) // pokud existuj?další skiny, odstraníme možnost "default"
        {
            skinNames.Remove("default");
        }
        demoSkinDropdown.AddOptions(skinNames);
        demoSkinDropdown.interactable = skinNames.Count > 1; // nastavíme roletkov?menu jako interaktivn?pouze pokud je více ne?jedna možnost výbìru

        // pokud je dostupn?stejn?animace jako u pøedchoz?postavy, spustíme ji
        int animationIndex = animationNames.IndexOf(currentAnimationName);
        if (animationIndex >= 0)
        {
            demoAnimationDropdown.value = animationIndex;
            PlayAnimation(currentAnimationName);
        }
        // jinak spustíme prvn?animaci
        else if (animationNames.Count > 0)
        {
            PlayAnimation(animationNames[0]);
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (currentSkeletonAnimation != null)
        {
            currentSkeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
            currentAnimationName = animationName;
        }
    }

    private void SwitchSkin(string skinName)
    {
        if (currentSkeletonAnimation != null)
        {
            currentSkeletonAnimation.initialSkinName = skinName;
            currentSkeletonAnimation.Initialize(true);

            // po zmìn?skinu znovu spustíme aktuáln?vybranou animaci
            PlayAnimation(currentAnimationName);
        }
    }
}
}