using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class OptionFiller : MonoBehaviour
{
    public Button[] text_options;

    public List<Button> GetButtons()
    {
        return text_options.ToList();
    }
}
