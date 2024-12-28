using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    LineRenderer lineRenderer; // Inspector'da görünmesi için SerializeField ekledik.
    private GameController gameController;

    public void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void Launch()
    {
        transform.DOLocalMoveY(500, 1.5f, false);
    }

    public void MoveUp(int distance)
    {
        transform.DOLocalMoveY(transform.position.y + 184 + distance, 0.25f, false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CollisionDetector>() != null)
        {
            // When it collides with a ball, it attaches the ball to the rotative object:
            RotationObject rotationObject = FindObjectOfType<RotationObject>();
            Vector3 position = collision.transform.position;

            DOTween.Kill(transform); // Ends the current DOTween animation

            // Generates a line between the ball and the center
            transform.parent = rotationObject.transform;

            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.widthMultiplier = 3;
                lineRenderer.positionCount = 2;

                // Set the gradient color
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] {
                        new GradientColorKey(ParseHexColor("#9089E3"), 0f), // Start at 0% with #9089E3
                        new GradientColorKey(Color.white, 0.7f) // At 70%, use white
                    },
                    new GradientAlphaKey[] {
                        new GradientAlphaKey(1f, 0f), // Full opacity at start
                        new GradientAlphaKey(1f, 0.7f) // Full opacity at 70%
                    }
                );
                lineRenderer.colorGradient = gradient;
            }

            lineRenderer.SetPosition(0, rotationObject.transform.position);
            lineRenderer.SetPosition(1, transform.position);

            // Add score when ball successfully attaches
            ScoreManager.Instance.AddScore();
        }
        else if (collision.GetComponent<Ball>())
        {
            gameController.GameOver();
        }
    }

    void Update()
    {
        // Changes the final position of the line every frame
        if (lineRenderer != null && gameController.lose != true)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private Color ParseHexColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        else
        {
            Debug.LogError($"Invalid color code: {hex}");
            return Color.white; // Default color if parsing fails
        }
    }
}
