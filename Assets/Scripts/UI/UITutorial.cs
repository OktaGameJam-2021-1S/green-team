using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    
    [SerializeField] private List<Sprite> _tutorialSprites = default;
    private Image _tutorialImage;

    private int _index;

    private void Awake()
    {
        _tutorialImage = GetComponent<Image>();
        _index = -1;
        NextImage();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            NextImage();
        }
    }

    private void NextImage()
    {
        _index++;
        if (_tutorialSprites.Count <= _index)
        {
            SceneManager.LoadScene("Main");
            return;
        }

        _tutorialImage.sprite = _tutorialSprites[_index];
    }

}
