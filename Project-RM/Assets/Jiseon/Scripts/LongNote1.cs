using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote1 : MonoBehaviour
{
    float newScale = 1.0f;
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        newScale += Time.deltaTime;
        rectTransform.localScale = new Vector3(newScale, 1.0f, 1.0f);//This works

        rectTransform.localScale.Set(newScale, 1.0f, 1.0f); //This DOES not. Despicable bug

        if (Input.GetKeyDown(KeyCode.A))//LeftMouseButton
        {
            newScale += Time.deltaTime;
            rectTransform.localScale = new Vector3(newScale, 1.0f, 1.0f);//This works

            rectTransform.localScale.Set(newScale, 1.0f, 1.0f); //This DOES not. Despicable bug
        }
        else if (Input.GetMouseButtonDown(1))//RightMouseButton
        {
            DecreaseObjectSize(5f, new Vector3(0f, 0f, 1f));
        }

        // Resize(2f, Vector3.right);
    }
    public void IncreaseObjectSize(float amount, Vector3 direction)
    {
        transform.position += direction * amount / 2; // Move the object in the direction of scaling, so that the corner on ther side stays in place
        transform.localScale += direction * amount; // Scale object in the specified direction
    }
    public void DecreaseObjectSize(float amount, Vector3 direction)
    {
        transform.position -= direction * amount / 2;
        transform.localScale -= direction * amount;
    }

    public void Resize(float amount, Vector3 direction)
    {
        transform.position += direction * amount / 2; // Move the object in the direction of scaling, so that the corner on ther side stays in place
        transform.localScale += direction * amount; // Scale object in the specified direction
    }
}
