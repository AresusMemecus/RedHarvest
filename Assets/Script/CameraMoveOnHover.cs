using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoveOnHover : MonoBehaviour
{
    public Camera camera; // Ссылка на камеру
    public Collider collider; // Коллайдер, в пределах которого должна оставаться камера
    public float moveSpeed = 1f; // Скорость движения камеры
    public float followStrength = 0.1f; // Степень отклонения камеры в сторону курсора
    public float deadZoneSize = 0.2f; // Размер мёртвой зоны (например, 0.2 означает 20% от экрана)

    private Vector3 originalPosition; // Исходная позиция камеры
    private Vector2 screenCenter; // Центр экрана

    void Start()
    {
        originalPosition = camera.transform.position;
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2); // Центр экрана
    }

    void Update()
    {
        if (Mouse.current == null || camera == null || collider == null) return;

        // Получаем позицию мыши на экране
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        
        // Вычисляем размеры мёртвой зоны в пикселях
        float deadZoneWidth = Screen.width * deadZoneSize;
        float deadZoneHeight = Screen.height * deadZoneSize;

        // Проверяем, находится ли курсор в пределах мёртвой зоны
        if (Mathf.Abs(mouseScreenPos.x - screenCenter.x) > deadZoneWidth || Mathf.Abs(mouseScreenPos.y - screenCenter.y) > deadZoneHeight)
        {
            // Преобразуем позицию курсора в мировые координаты
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, camera.nearClipPlane));

            // Получаем направление от текущего положения камеры к позиции курсора
            Vector3 direction = (mouseWorldPos - camera.transform.position).normalized;

            // Перемещаем камеру в сторону курсора, но ограничиваем движение в пределах коллайдера
            Vector3 targetPosition = camera.transform.position + direction * followStrength;

            // Ограничиваем движение камеры в пределах коллайдера
            targetPosition = GetClampedPosition(targetPosition);

            // Плавно двигаем камеру к новой позиции
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Функция для ограничения положения камеры в пределах коллайдера
    private Vector3 GetClampedPosition(Vector3 targetPosition)
    {
        // Получаем границы коллайдера
        Bounds bounds = collider.bounds;

        // Ограничиваем положение камеры в пределах этих границ
        float clampedX = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
        float clampedY = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);
        float clampedZ = Mathf.Clamp(targetPosition.z, bounds.min.z, bounds.max.z);

        return new Vector3(clampedX, clampedY, clampedZ);
    }
}
