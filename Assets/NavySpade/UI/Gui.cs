using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Tutorial;
using pj94.Code.Tutor;
using TMPro;
using UnityEngine;


public class Gui : MonoBehaviour{
    public static Gui Instance;

    [Header("Components")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TMP_Text _levelCounter;
    [SerializeField]
    private TutorialShopController toturShopController;

    [field: SerializeField]
    public TutorHandAnimation TutorHand{ get; set; }

    [field: SerializeField]
    public GameObject ButtonRestart{ get; set; }

    private void Awake(){
                   // TutorialShopController.Instance = toturShopController;

        ButtonRestart.SetActive(false);
        Instance = this;
        TutorHand.Hide();
    }

    private void OnDestroy(){
        Instance = null;
    }


    #region getters

    public Canvas Canvas => canvas;
    public string LevelCounter{
        set{
            _levelCounter.gameObject.SetActive(true);
            _levelCounter.text = value;
            if (LevelManager.LevelIndex == 10 && !TutorialShopController.Instance.TutorDone){
                TutrHand1.Instance.Show();
            } 
        }
    }

    #endregion
}