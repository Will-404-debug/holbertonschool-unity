using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialLinks : MonoBehaviour
{
    public void OpenEmail()
    {
        Application.OpenURL("mailto:contact5.0@komowl.company");
    }

    public void OpenLinkedIn()
    {
        Application.OpenURL("https://www.linkedin.com/in/wgd-world");
    }

    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/kom_owl");
    }

    public void OpenGithub()
    {
        Application.OpenURL("https://github.com/Will-404-debug");
    }

    public void OpenWebsite()
    {
        Application.OpenURL("https://www.komowl.company/");
    }
}
