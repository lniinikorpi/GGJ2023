using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretButton : MonoBehaviour
{
    private TurretData m_turret;
    public TurretData Turret {
        get => m_turret;
        set {
            m_turret = value;
            if(value.icon != null) {
                iconImage.sprite = value.icon;
                iconImage.SetNativeSize();
            }
        }
    }
    [SerializeField]
    private Image iconImage;
    private void OnCurrencyChanged(EventArgs args) {
        Toggle toggle = GetComponent<Toggle>();
        toggle.interactable = args.intValue >= Turret.price;
        if (toggle.isOn) { 
            toggle.isOn = args.intValue >= Turret.price;
        }
    }
    private void OnEnable() {
        GlobalEventSender.CurrencyChangedEvent += OnCurrencyChanged;
    }

    private void OnDisable() {
        GlobalEventSender.CurrencyChangedEvent -= OnCurrencyChanged;
    }
}
