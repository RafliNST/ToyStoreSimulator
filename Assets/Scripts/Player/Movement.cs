using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        float speed;
        [SerializeField]
        AnimationCurve movementCurve;

        bool isMoving, isCoroutineRunning = false;
        float curveSpeed;
        float movingTime = 0f;
        Coroutine moveCoroutine;

        private void Start()
        {
            GameInput.Instance.onPlayerMove.performed += (InputAction.CallbackContext ctx) =>
            {
                isMoving = true;
                Debug.Log("IsMoving: " + isMoving);
                if (!isCoroutineRunning)
                {
                    isCoroutineRunning = true;
                    moveCoroutine = StartCoroutine(PerformMoving());
                }
            };

            GameInput.Instance.onPlayerMove.canceled += (InputAction.CallbackContext ctx) =>
            {
                movingTime = 0f;
                isMoving = false;
                Debug.Log("IsMoving: " + isMoving);
                StopCoroutine("PerformMoving");
                isCoroutineRunning = false;
            };
        }

        private void Update()
        {

        }

        private void OnDisable()
        {
            GameInput.Instance.onPlayerMove.performed -= (InputAction.CallbackContext ctx) =>
            {
                isMoving = true;
                Debug.Log("IsMoving: " + isMoving);
                if (!isCoroutineRunning)
                {
                    isCoroutineRunning = true;
                    StartCoroutine(PerformMoving());
                }
            };

            GameInput.Instance.onPlayerMove.canceled -= (InputAction.CallbackContext ctx) =>
            {
                isMoving = false;
                Debug.Log("IsMoving: " + isMoving);
                StopCoroutine("PerformMoving");
                isCoroutineRunning = false;
            };
        }

        private void WhileMoving(InputAction.CallbackContext ctx)
        {
            StartCoroutine(PerformMoving());
        }

        System.Collections.IEnumerator PerformMoving()
        {
            while (isMoving)
            {
                movingTime += Time.deltaTime;
                curveSpeed = Mathf.Clamp01(movingTime);
                Debug.Log($"Moving Time: {movingTime} - Curve Speed : {curveSpeed}");

                transform.position += GameInput.Instance.Move() * movementCurve.Evaluate(curveSpeed) * speed * Time.deltaTime;
                yield return null;
            }

        }
    }
}
