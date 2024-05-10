using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image UIJoystick;
    [SerializeField] private Image UIStick;
    private Vector2 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        UIJoystick = GetComponent<Image>();
        UIStick = transform.GetChild(0).GetComponent<Image>();
    }

    // Called when a pointer is pressed on the joystick
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    // Called when a pointer is lifted off the joystick
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector2.zero;
        UIStick.rectTransform.anchoredPosition = Vector2.zero;
    }

    // Called when a pointer is dragged across the joystick
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;

        // Converts the screen position of the joystick to a local position within the joystick rectangle
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UIJoystick.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / UIJoystick.rectTransform.sizeDelta.x);
            pos.y = (pos.y / UIJoystick.rectTransform.sizeDelta.x);

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Sets the position of the joystick handle within the joystick background
            UIStick.rectTransform.anchoredPosition = new Vector2(inputVector.x * (UIJoystick.rectTransform.sizeDelta.x / 2), inputVector.y * (UIJoystick.rectTransform.sizeDelta.y / 2));
        }
    }

    // Returns the horizontal value of the input vector
    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    // Returns the vertical value of the input vector
    public float Vertical()
    {
        if (inputVector.y != 0)
            return inputVector.y;
        else
            return Input.GetAxis("Vertical");
    }
}