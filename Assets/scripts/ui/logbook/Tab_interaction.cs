using NUnit.Framework;
using UnityEngine;

public class Tab_interaction : MonoBehaviour
{

    public void Activate()
    {

        Debug.Log("setting as last sibling");
        this.transform.SetAsLastSibling();
    }

}
