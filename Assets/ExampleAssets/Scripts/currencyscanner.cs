using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class currencyscanner : MonoBehaviour
{
    ARTrackedImageManager m_TrackedImageManager;
    // Scriptable objects to hold all country currency data
    public currency_scriptable[] AllCountryCurrency;

    private void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Called when tracked images are added/updated/removed
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateInfo(trackedImage);
        }
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {
        // Get the TextMeshProUGUI component to display currency information
        TextMeshProUGUI CurrencyInfoText = trackedImage.transform.GetChild(0).GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        // Check if the tracked image is still being tracked
        if (trackedImage.trackingState != TrackingState.None)
        {
            string CurrencyName = trackedImage.referenceImage.name;
            string CurrencyCountry = "";
            string CurrencyType = "";
            bool isCurrencyDetected = false;

            // Loop through all available currencies to find a match
            foreach (var currentCountryCurrency in AllCountryCurrency)
            {
                foreach (var currencyNote in currentCountryCurrency.currencyData)
                {
                    if (currencyNote.m_CurrencyValue == CurrencyName)
                    {
                        // Update currency details
                        CurrencyCountry = currencyNote.m_Binding.m_CurrencyName;
                        CurrencyType = currencyNote.m_Binding.m_CurrencyType;
                        isCurrencyDetected = true;
                        break;
                    }
                }

                if (isCurrencyDetected)
                    break; // Exit loop if currency is detected
            }

            // If currency is detected, update the text component with formatted currency info
            if (isCurrencyDetected)
            {
                string formattedCurrencyInfo = FormatCurrencyString(CurrencyName, CurrencyType, CurrencyCountry);
                CurrencyInfoText.text = formattedCurrencyInfo;
            }
            else
            {
                // If no match is found, show a default message
                CurrencyInfoText.text = "Currency not recognized";
            }
        }
        else
        {
            // If the image is no longer tracked, clear the text
            CurrencyInfoText.text = "";
        }
    }

    // Helper method to format the currency information string
    string FormatCurrencyString(string CurrencyName, string CurrencyType, string CurrencyCountry)
    {
        // Format the currency name by removing specific prefixes
        CurrencyName = CurrencyName.StartsWith("I") ? CurrencyName.Remove(0, 4) : CurrencyName.Remove(0, 3);
        return string.Format("{0} {1} ({2})", CurrencyCountry, CurrencyName, CurrencyType);
    }
}
