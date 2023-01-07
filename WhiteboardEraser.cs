using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteboardEraser : MonoBehaviour
{
    //The transform component of the eraser's bottom
    [SerializeField] private Transform _eraserBottom;
    //The size of the eraser, in pixels
    [SerializeField] private int _eraserSize = 50;

    //The renderer component of the eraser's bottom
    private Renderer _renderer;
    //The height of the eraser's bottom
    private float _eraserHeight;
    //The result of the raycast from the eraser's bottom
    private RaycastHit _touch;
    //The Whiteboard component of the object that the eraser's bottom is touching
    private Whiteboard _whiteboard;
    //The original colors of the whiteboard's texture
    private Color[] _originalColors;
    //The texture coordinates of the point where the eraser's bottom was touching the whiteboard on the previous frame
    private Vector2 _touchPos, _lastTouchPos;
    //The rotation of the eraser on the previous frame when the eraser's bottom was touching the whiteboard
    private Quaternion _lastTouchRot;

    //Start is called before the first frame update
    void Start()
    {
        //Get the renderer component of the eraser's bottom
        _renderer = _eraserBottom.GetComponent<Renderer>();
        //Set _eraserHeight to the height of the eraser's bottom
        _eraserHeight = _eraserBottom.localScale.y;
    }

    //Update is called once per frame
    void Update()
    {
        //Execute the Draw method
        Draw();
    }

    private void Draw()
    {
        //Cast a ray from the eraser's bottom in the direction the eraser is facing
        if (Physics.Raycast(_eraserBottom.position, transform.up, out _touch, _eraserHeight))
        {
            //If the ray hits an object with the "Whiteboard" tag
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                //Get the Whiteboard component of the object that the eraser's bottom is touching
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                //Store the original colors of the whiteboard's texture in the _originalColors array
                _originalColors = _whiteboard.texture.GetPixels();

                //Get the texture coordinates of the point where the ray hit the whiteboard
                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                //Set _lastTouchPos to _touchPos only if the eraser is touching the whiteboard
                if (_touch.transform.CompareTag("Whiteboard"))
                {
                    _lastTouchPos = _touchPos;
                }

                //Convert the texture coordinates to pixel coordinates on the whiteboard's texture
                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_eraserSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_eraserSize / 2));

                //Checks if the x and y pixel coordinates are within the bounds of the whiteboard's texture. If either x or y is less than 0 or greater than the width or height of the whiteboard's texture, exits method
                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                //Set a square of pixels on the whiteboard's texture to the original colors of the whiteboard
                _whiteboard.texture.SetPixels(x, y, _eraserSize, _eraserSize, _originalColors);

                //Set _originalColors to the original colors of the whiteboard's texture
                _originalColors = _whiteboard.texture.GetPixels(x, y, _eraserSize, _eraserSize);

                //Apply the changes to the whiteboard's texture
                _whiteboard.texture.Apply();
            }
            else
            {
                //Set _whiteboard to null
                _whiteboard = null;
            }
        }
        else
        {
            //Set _whiteboard to null
            _whiteboard = null;
        }
    }
}
