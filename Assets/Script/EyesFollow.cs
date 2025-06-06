using UnityEngine;
using UnityEngine.InputSystem;

public class EyeInsideContainer : MonoBehaviour
{
    public RectTransform container;  // Контейнер, в пределах которого глаз должен двигаться
    public RectTransform eye;        // Глаз, который двигается
    public float speed = 10f;        // Скорость движения глаза

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue(); // Позиция мыши в экране (пикселях)

        // Получаем экранные координаты контейнера
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);
        // corners[0] - нижний левый угол, corners[2] - верхний правый

        Vector2 min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        // Клэмпим позицию мыши внутри контейнера
        float clampedX = Mathf.Clamp(mousePos.x, min.x, max.x);
        float clampedY = Mathf.Clamp(mousePos.y, min.y, max.y);
        Vector2 clampedPos = new Vector2(clampedX, clampedY);

        // Плавно двигаем глаз к позиции мыши, но ограниченной контейнером
        Vector2 eyePos = Vector2.Lerp(eye.position, clampedPos, speed * Time.deltaTime);

        eye.position = eyePos;
    }
}
