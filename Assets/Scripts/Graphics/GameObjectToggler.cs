using UnityEngine;

public class GameObjectToggler : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;

    public bool Toggle()
    {
        return OnToggle();
    }

    protected virtual bool OnToggle()
    {
        bool state = !_targetObject.activeSelf;
        _targetObject.SetActive(state);
        return state;
    }
}