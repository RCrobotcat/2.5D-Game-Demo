using System.Collections.Generic;
using UnityEngine;
using TMPro; // P�id�me TextMeshPro namespace
using Spine;
using Spine.Unity;

namespace ApocalypseHeroesSpine2DCharactersDEMO
{
public class ObjectSwitcherDEMO : MonoBehaviour
{
    public List<GameObject> demoObjects; // seznam objekt?ve sc�n?
    public TMP_Dropdown demoCharacterDropdown; // roletkov?menu pro v�b�r postavy
    public TMP_Dropdown demoAnimationDropdown; // roletkov?menu pro v�b�r animace
    public TMP_Dropdown demoSkinDropdown; // roletkov?menu pro v�b�r skinu

    private SkeletonAnimation currentSkeletonAnimation;
    private string currentAnimationName;

    void Start()
    {
        // na za��tku nastav�me v�echny objekty jako neaktivn?
        foreach (GameObject obj in demoObjects)
        {
            obj.SetActive(false);
        }

        // napln�me roletkov?menu pro v�b�r postavy
        demoCharacterDropdown.ClearOptions();
        List<string> characterNames = new List<string>();
        foreach (GameObject obj in demoObjects)
        {
            characterNames.Add(obj.name);
        }
        demoCharacterDropdown.AddOptions(characterNames);
        demoCharacterDropdown.interactable = characterNames.Count > 1; // nastav�me roletkov?menu jako interaktivn?pouze pokud je v�ce ne?jedna mo�nost v�b�ru

        // nastav�me akci, kter?se provede p�i v�b�ru postavy
        demoCharacterDropdown.onValueChanged.AddListener(delegate {
            SwitchCharacter(demoCharacterDropdown.value);
        });

        // nastav�me akci, kter?se provede p�i v�b�ru animace
        demoAnimationDropdown.onValueChanged.AddListener(delegate {
            PlayAnimation(demoAnimationDropdown.options[demoAnimationDropdown.value].text);
        });

        // nastav�me akci, kter?se provede p�i v�b�ru skinu
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
        // p�ep�n�n?postav pomoc?�ipek nahoru a dolu
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

        // p�ep�n�n?animac?pomoc?�ipek nahoru a dolu s dr�en�m Alt
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

        // p�ep�n�n?skin?pomoc?�ipek nahoru a dolu s dr�en�m Ctrl
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
        // deaktivujeme aktu�ln?objekt
        if (currentSkeletonAnimation != null)
        {
            currentSkeletonAnimation.gameObject.SetActive(false);
        }

        // aktivujeme nov?objekt
        GameObject newObject = demoObjects[index];
        newObject.SetActive(true);
        currentSkeletonAnimation = newObject.GetComponent<SkeletonAnimation>();

        // napln�me roletkov?menu pro v�b�r animace
        demoAnimationDropdown.ClearOptions();
        List<string> animationNames = new List<string>();
        foreach (var animation in currentSkeletonAnimation.Skeleton.Data.Animations)
        {
            animationNames.Add(animation.Name);
        }
        demoAnimationDropdown.AddOptions(animationNames);
        demoAnimationDropdown.interactable = animationNames.Count > 1; // nastav�me roletkov?menu jako interaktivn?pouze pokud je v�ce ne?jedna mo�nost v�b�ru

        // napln�me roletkov?menu pro v�b�r skinu
        demoSkinDropdown.ClearOptions();
        List<string> skinNames = new List<string>();
        foreach (var skin in currentSkeletonAnimation.Skeleton.Data.Skins)
        {
            skinNames.Add(skin.Name);
        }
        if (skinNames.Count == 0) // pokud neexistuj?��dn?skiny, p�id�me mo�nost "default"
        {
            skinNames.Add("default");
        }
        else if (skinNames.Count > 1 && skinNames.Contains("default")) // pokud existuj?dal�� skiny, odstran�me mo�nost "default"
        {
            skinNames.Remove("default");
        }
        demoSkinDropdown.AddOptions(skinNames);
        demoSkinDropdown.interactable = skinNames.Count > 1; // nastav�me roletkov?menu jako interaktivn?pouze pokud je v�ce ne?jedna mo�nost v�b�ru

        // pokud je dostupn?stejn?animace jako u p�edchoz?postavy, spust�me ji
        int animationIndex = animationNames.IndexOf(currentAnimationName);
        if (animationIndex >= 0)
        {
            demoAnimationDropdown.value = animationIndex;
            PlayAnimation(currentAnimationName);
        }
        // jinak spust�me prvn?animaci
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

            // po zm�n?skinu znovu spust�me aktu�ln?vybranou animaci
            PlayAnimation(currentAnimationName);
        }
    }
}
}