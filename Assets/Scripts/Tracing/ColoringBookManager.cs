using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using KendaGame.Navigation;
using KendaGame.Helpers;

public class ColoringBookManager : MonoBehaviour {
  #region variables

  public static ColoringBookManager _instance;
  public static ColoringBookManager Instance {
    get {
      if (_instance == null) {
        _instance = FindObjectOfType<ColoringBookManager>();
      }
      return _instance;
    }
  }
  public Material maskTexMaterial;
  private Texture2D maskTex;
  public List<Sprite> maskTexList;
  public int maskTexIndex = -1;
  public static string ID = "0";

  // list of drawmodes
  public enum DrawMode {
    Pencil,
    Marker,
    Spray,
    Sticker,
    Pattern,
    Magic
  }

  //	*** Default settings ***
  public List<Color32> paintColors;
  private Color32 paintColor = new Color32(255, 0, 0, 255);
  private int brushSize = 0; // default brush size
  public List<int> brushesSize;
  private DrawMode drawMode = DrawMode.Pencil;
  private bool useLockArea = true;
  private byte[] lockMaskPixels; // locking mask pixels
  private int selectedColorNumber = 0;
  private bool paintBucketMode = false;
  //	*** private variables ***
  private byte[] pixels; // byte array for texture painting, this is the image that we paint into.
  private byte[] maskPixels; // byte array for mask texture

  private Texture2D tex; // texture that we paint into (it gets updated from pixels[] array when painted)

  private int texWidth = 1024;
  private int texHeight = 576;
  private RaycastHit hit;
  private bool wentOutside = false;

  private Vector2 pixelUV; // with mouse
  private Vector2 pixelUVOld; // with mouse

  private bool textureNeedsUpdate = false; // if we have modified texture

  ////////////////////////////////////////////////////
  public Camera mainCamera;
  GraphicRaycaster m_Raycaster;
  EventSystem m_EventSystem;
  bool clickOnUI = false;

  #endregion


  #region Init And Control Functions

  private void Awake() {

    if (SceneManager.Instance.GetActiveScene() == SceneManager.Scenes.ColoringEngine) {
      maskTexIndex = LettersHelper.GetActiveLetterIndex();
    }

    m_Raycaster = FindObjectOfType<GraphicRaycaster>();
    m_EventSystem = FindObjectOfType<EventSystem>();
    if (SceneManager.Instance && Camera.main) {
      mainCamera = Camera.main;
    }

    GetComponent<Renderer>().sortingOrder = -99;
    if (maskTexIndex < 0) {
      maskTex = null;
    } else {
      maskTex = DuplicateTexture(maskTexList[maskTexIndex].texture);
    }

    InitializeEverything();
  }

  private Texture2D DuplicateTexture(Texture2D originalTexture) {
    RenderTexture renderTex = RenderTexture.GetTemporary(
                originalTexture.width,
                originalTexture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

    Graphics.Blit(originalTexture, renderTex);
    RenderTexture previous = RenderTexture.active;
    RenderTexture.active = renderTex;
    Texture2D readableText = new Texture2D(originalTexture.width, originalTexture.height);
    readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
    readableText.Apply();
    RenderTexture.active = previous;
    RenderTexture.ReleaseTemporary(renderTex);
    return readableText;
  }

  private void InitializeEverything() {
    CreateFullScreenQuad();

    // create texture
    if (maskTex) {
      GetComponent<Renderer>().material = maskTexMaterial;

      texWidth = maskTex.width;
      texHeight = maskTex.height;
      GetComponent<Renderer>().material.SetTexture("_MaskTex", maskTex);

      useLockArea = true;
    } else {
      texWidth = 1024;
      texHeight = 576;

      useLockArea = false;
    }

    if (!GetComponent<Renderer>().material.HasProperty("_MainTex")) Debug.LogError("Fatal error: Current shader doesn't have a property: '_MainTex'");

    // create new texture
    tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
    GetComponent<Renderer>().material.SetTexture("_MainTex", tex);

    // init pixels array
    pixels = new byte[texWidth * texHeight * 4];

    OnClearButtonClicked();

    // set texture modes
    tex.filterMode = FilterMode.Point;
    tex.wrapMode = TextureWrapMode.Clamp;
    //tex.wrapMode = TextureWrapMode.Repeat;

    if (maskTex) {
      ReadMaskImage();
    }

    // locking mask enabled
    if (useLockArea) {
      lockMaskPixels = new byte[texWidth * texHeight * 4];
    }
  }

  private void CreateFullScreenQuad() {
    Camera cam = Camera.main;
    // create mesh plane, fits in camera view (with screensize adjust taken into consideration)
    Mesh go_Mesh = GetComponent<MeshFilter>().mesh;
    go_Mesh.Clear();
    go_Mesh.vertices = new[] {
                cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane + 0.1f)), // bottom left
				cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane + 0.1f)), // top left
				cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane + 0.1f)), // top right
				cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane + 0.1f)) // bottom right
			};
    go_Mesh.uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    go_Mesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };

    go_Mesh.RecalculateNormals();

    go_Mesh.tangents = new[] { new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f) };

    // mesh collider
    gameObject.GetComponent<MeshCollider>().sharedMesh = go_Mesh;
  }

  private void ReadMaskImage() {
    maskPixels = new byte[texWidth * texHeight * 4];

    int pixel = 0;
    for (int y = 0; y < texHeight; y++) {
      for (int x = 0; x < texWidth; x++) {
        Color c = maskTex.GetPixel(x, y);
        maskPixels[pixel] = (byte)(c.r * 255);
        maskPixels[pixel + 1] = (byte)(c.g * 255);
        maskPixels[pixel + 2] = (byte)(c.b * 255);
        maskPixels[pixel + 3] = (byte)(c.a * 255);
        pixel += 4;
      }
    }
  }

  private void Start() {
    paintColor = paintColors[selectedColorNumber];
    OnChangeColorButtonClicked(selectedColorNumber);
    OnChangeBrushSizeButtonClicked(1);
  }

  private void LateUpdate() {
    MousePaint();
    UpdateTexture();
  }

  private void MousePaint() {
    if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
      PointerEventData m_PointerEventData = new PointerEventData(m_EventSystem);
      m_PointerEventData.position = Input.mousePosition;
      List<RaycastResult> results = new List<RaycastResult>();
      m_Raycaster.Raycast(m_PointerEventData, results);
      clickOnUI = results.Count > 0;

      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit)) {
        if (hit.collider == null || !hit.collider.gameObject.name.Contains("PaintingBoard")) {
          return;
        }
      } else {
        RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit2.collider == null || !hit2.collider.gameObject.name.Contains("PaintingBoard")) {
          return;
        }
      }
    }

    if (!clickOnUI) {
      if (Input.GetMouseButtonDown(0)) {
        if (useLockArea) {
          if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1)) return;
          CreateAreaLockMask((int)(hit.textureCoord.x * texWidth), (int)(hit.textureCoord.y * texHeight));
        }

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1)) { wentOutside = true; return; }

        pixelUVOld = pixelUV; // take previous value, so can compare them
        pixelUV = hit.textureCoord;
        pixelUV.x *= texWidth;
        pixelUV.y *= texHeight;

        if (wentOutside) { pixelUVOld = pixelUV; wentOutside = false; }

        textureNeedsUpdate = true;
      }

      if (Input.GetMouseButtonUp(0)) {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1)) { wentOutside = true; return; }
      }

      if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
        // Only if we hit something, then we continue
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1)) { wentOutside = true; return; }

        pixelUVOld = pixelUV; // take previous value, so can compare them
        pixelUV = hit.textureCoord;
        pixelUV.x *= texWidth;
        pixelUV.y *= texHeight;

        if (wentOutside) { pixelUVOld = pixelUV; wentOutside = false; }

        // lets paint where we hit
        switch (drawMode) {
          case DrawMode.Pencil: // drawing
              {
              if (!paintBucketMode) {
                DrawCircle((int)pixelUV.x, (int)pixelUV.y);
              } else {
                if (maskTex) {
                  FloodFillMaskOnlyWithThreshold((int)pixelUV.x, (int)pixelUV.y);
                } else {
                  FloodFillWithTreshold((int)pixelUV.x, (int)pixelUV.y);
                }
              }
            }
            break;

          default: // unknown mode
            break;
        }

        textureNeedsUpdate = true;
      }

      if (Input.GetMouseButtonDown(0)) {
        // take this position as start position
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1)) return;

        pixelUVOld = pixelUV;
      }

      // check distance from previous drawing point and connect them with DrawLine
      if (Vector2.Distance(pixelUV, pixelUVOld) > brushSize) {
        switch (drawMode) {
          case DrawMode.Pencil: // drawing
              {
              DrawLine(pixelUVOld, pixelUV);
            }
            break;

          default: // other modes
            break;
        }
        pixelUVOld = pixelUV;
        textureNeedsUpdate = true;
      }
    }
  }

  private void CreateAreaLockMask(int x, int y) {
    if (maskTex) {
      LockAreaFillWithThresholdMaskOnly(x, y);
    } else {
      LockMaskFillWithThreshold(x, y);
    }
  }

  private void LockAreaFillWithThresholdMaskOnly(int x, int y) {
    // create locking mask floodfill, using threshold, checking pixels from mask only

    // get canvas color from this point
    byte hitColorR = maskPixels[((texWidth * (y) + x) * 4) + 0];
    byte hitColorG = maskPixels[((texWidth * (y) + x) * 4) + 1];
    byte hitColorB = maskPixels[((texWidth * (y) + x) * 4) + 2];
    byte hitColorA = maskPixels[((texWidth * (y) + x) * 4) + 3];

    Queue<int> fillPointX = new Queue<int>();
    Queue<int> fillPointY = new Queue<int>();
    fillPointX.Enqueue(x);
    fillPointY.Enqueue(y);

    int ptsx, ptsy;
    int pixel = 0;

    lockMaskPixels = new byte[texWidth * texHeight * 4];

    while (fillPointX.Count > 0) {

      ptsx = fillPointX.Dequeue();
      ptsy = fillPointY.Dequeue();

      if (ptsy - 1 > -1) {
        pixel = (texWidth * (ptsy - 1) + ptsx) * 4; // down

        if (lockMaskPixels[pixel] == 0 // this pixel is not used yet
            && (CompareThreshold(maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(maskPixels[pixel + 1], hitColorG))
            && (CompareThreshold(maskPixels[pixel + 2], hitColorB))
            && (CompareThreshold(maskPixels[pixel + 3], hitColorA))) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy - 1);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx + 1 < texWidth) {
        pixel = (texWidth * ptsy + ptsx + 1) * 4; // right
        if (lockMaskPixels[pixel] == 0
            && (CompareThreshold(maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(maskPixels[pixel + 1], hitColorG))
            && (CompareThreshold(maskPixels[pixel + 2], hitColorB))
            && (CompareThreshold(maskPixels[pixel + 3], hitColorA))) {
          fillPointX.Enqueue(ptsx + 1);
          fillPointY.Enqueue(ptsy);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx - 1 > -1) {
        pixel = (texWidth * ptsy + ptsx - 1) * 4; // left
        if (lockMaskPixels[pixel] == 0
            && (CompareThreshold(maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(maskPixels[pixel + 1], hitColorG))
            && (CompareThreshold(maskPixels[pixel + 2], hitColorB))
            && (CompareThreshold(maskPixels[pixel + 3], hitColorA))) {
          fillPointX.Enqueue(ptsx - 1);
          fillPointY.Enqueue(ptsy);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsy + 1 < texHeight) {
        pixel = (texWidth * (ptsy + 1) + ptsx) * 4; // up
        if (lockMaskPixels[pixel] == 0
            && (CompareThreshold(maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(maskPixels[pixel + 1], hitColorG))
            && (CompareThreshold(maskPixels[pixel + 2], hitColorB))
            && (CompareThreshold(maskPixels[pixel + 3], hitColorA))) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy + 1);
          lockMaskPixels[pixel] = 1;
        }
      }
    }
  }

  private void LockMaskFillWithThreshold(int x, int y) {
    // create locking mask floodfill, using threshold

    // get canvas color from this point
    byte hitColorR = pixels[((texWidth * (y) + x) * 4) + 0];
    byte hitColorG = pixels[((texWidth * (y) + x) * 4) + 1];
    byte hitColorB = pixels[((texWidth * (y) + x) * 4) + 2];
    byte hitColorA = pixels[((texWidth * (y) + x) * 4) + 3];

    Queue<int> fillPointX = new Queue<int>();
    Queue<int> fillPointY = new Queue<int>();
    fillPointX.Enqueue(x);
    fillPointY.Enqueue(y);

    int ptsx, ptsy;
    int pixel = 0;

    lockMaskPixels = new byte[texWidth * texHeight * 4];

    while (fillPointX.Count > 0) {

      ptsx = fillPointX.Dequeue();
      ptsy = fillPointY.Dequeue();

      if (ptsy - 1 > -1) {
        pixel = (texWidth * (ptsy - 1) + ptsx) * 4; // down

        if (lockMaskPixels[pixel] == 0 // this pixel is not used yet
            && (CompareThreshold(pixels[pixel + 0], hitColorR) || CompareThreshold(pixels[pixel + 0], paintColor.r)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(pixels[pixel + 1], hitColorG) || CompareThreshold(pixels[pixel + 1], paintColor.g))
            && (CompareThreshold(pixels[pixel + 2], hitColorB) || CompareThreshold(pixels[pixel + 2], paintColor.b))
            && (CompareThreshold(pixels[pixel + 3], hitColorA) || CompareThreshold(pixels[pixel + 3], paintColor.a))) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy - 1);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx + 1 < texWidth) {
        pixel = (texWidth * ptsy + ptsx + 1) * 4; // right
        if (lockMaskPixels[pixel] == 0
            && (CompareThreshold(pixels[pixel + 0], hitColorR) || CompareThreshold(pixels[pixel + 0], paintColor.r)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(pixels[pixel + 1], hitColorG) || CompareThreshold(pixels[pixel + 1], paintColor.g))
            && (CompareThreshold(pixels[pixel + 2], hitColorB) || CompareThreshold(pixels[pixel + 2], paintColor.b))
            && (CompareThreshold(pixels[pixel + 3], hitColorA) || CompareThreshold(pixels[pixel + 3], paintColor.a))) {
          fillPointX.Enqueue(ptsx + 1);
          fillPointY.Enqueue(ptsy);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx - 1 > -1) {
        pixel = (texWidth * ptsy + ptsx - 1) * 4; // left
        if (lockMaskPixels[pixel] == 0
            && (CompareThreshold(pixels[pixel + 0], hitColorR) || CompareThreshold(pixels[pixel + 0], paintColor.r)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(pixels[pixel + 1], hitColorG) || CompareThreshold(pixels[pixel + 1], paintColor.g))
            && (CompareThreshold(pixels[pixel + 2], hitColorB) || CompareThreshold(pixels[pixel + 2], paintColor.b))
            && (CompareThreshold(pixels[pixel + 3], hitColorA) || CompareThreshold(pixels[pixel + 3], paintColor.a))) {
          fillPointX.Enqueue(ptsx - 1);
          fillPointY.Enqueue(ptsy);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsy + 1 < texHeight) {
        pixel = (texWidth * (ptsy + 1) + ptsx) * 4; // up
        if (lockMaskPixels[pixel] == 0
            && (CompareThreshold(pixels[pixel + 0], hitColorR) || CompareThreshold(pixels[pixel + 0], paintColor.r)) // if pixel is same as hit color OR same as paint color
            && (CompareThreshold(pixels[pixel + 1], hitColorG) || CompareThreshold(pixels[pixel + 1], paintColor.g))
            && (CompareThreshold(pixels[pixel + 2], hitColorB) || CompareThreshold(pixels[pixel + 2], paintColor.b))
            && (CompareThreshold(pixels[pixel + 3], hitColorA) || CompareThreshold(pixels[pixel + 3], paintColor.a))) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy + 1);
          lockMaskPixels[pixel] = 1;
        }
      }
    }
  }

  private void UpdateTexture() {
    if (textureNeedsUpdate) {
      textureNeedsUpdate = false;
      tex.LoadRawTextureData(pixels);
      tex.Apply(false);
    }
  }

  #endregion
  #region OnButtonsClicked

  public void PlaySoundButtonClicked() {
    MusicController.USE.PlaySound(MusicController.USE.clickSound);
  }
  public void OnChangeColorButtonClicked(int num) {
    selectedColorNumber = num;
    paintColor = paintColors[selectedColorNumber];
  }

  public void OnChangeBrushSizeButtonClicked(int num) {
    brushSize = brushesSize[num];
  }

  public void OnClearButtonClicked() {
    int pixel = 0;
    for (int y = 0; y < texHeight; y++) {
      for (int x = 0; x < texWidth; x++) {
        pixels[pixel] = 255;
        pixels[pixel + 1] = 255;
        pixels[pixel + 2] = 255;
        pixels[pixel + 3] = 255;
        pixel += 4;
      }
    }
    tex.LoadRawTextureData(pixels);
    tex.Apply(false);
  }

  public Rect GUIRectWithObject(GameObject go) {
    Vector3 cen = go.GetComponent<Renderer>().bounds.center;
    Vector3 ext = go.GetComponent<Renderer>().bounds.extents;
    Vector2[] extentPoints = new Vector2[8]
     {
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
     };
    Vector2 min = extentPoints[0];
    Vector2 max = extentPoints[0];
    foreach (Vector2 v in extentPoints) {
      min = Vector2.Min(min, v);
      max = Vector2.Max(max, v);
    }
    return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
  }

  private Vector2 WorldToGUIPoint(Vector3 world) {
    Vector2 screenPoint = Camera.main.WorldToScreenPoint(world);
    return screenPoint;
  }
  #endregion
  #region Painting Functions

  private void DrawCircle(int x, int y) {
    int pixel = 0;

    // draw fast circle: 
    int r2 = brushSize * brushSize;
    int area = r2 << 2;
    int rr = brushSize << 1;
    for (int i = 0; i < area; i++) {
      int tx = (i % rr) - brushSize;
      int ty = (i / rr) - brushSize;

      if (tx * tx + ty * ty < r2) {
        if (x + tx < 0 || y + ty < 0 || x + tx >= texWidth || y + ty >= texHeight) continue;

        pixel = (texWidth * (y + ty) + x + tx) * 4;

        if (!useLockArea || (useLockArea && lockMaskPixels[pixel] == 1)) {
          pixels[pixel] = paintColor.r;
          pixels[pixel + 1] = paintColor.g;
          pixels[pixel + 2] = paintColor.b;
          pixels[pixel + 3] = paintColor.a;
        }
      }
    }
  }

  private void FloodFillMaskOnlyWithThreshold(int x, int y) {
    // get canvas hit color
    byte hitColorR = maskPixels[((texWidth * (y) + x) * 4) + 0];
    byte hitColorG = maskPixels[((texWidth * (y) + x) * 4) + 1];
    byte hitColorB = maskPixels[((texWidth * (y) + x) * 4) + 2];
    byte hitColorA = maskPixels[((texWidth * (y) + x) * 4) + 3];

    if (paintColor.r == hitColorR && paintColor.g == hitColorG && paintColor.b == hitColorB && paintColor.a == hitColorA) return;

    Queue<int> fillPointX = new Queue<int>();
    Queue<int> fillPointY = new Queue<int>();
    fillPointX.Enqueue(x);
    fillPointY.Enqueue(y);

    int ptsx, ptsy;
    int pixel = 0;

    lockMaskPixels = new byte[texWidth * texHeight * 4];

    while (fillPointX.Count > 0) {
      ptsx = fillPointX.Dequeue();
      ptsy = fillPointY.Dequeue();

      if (ptsy - 1 > -1) {
        pixel = (texWidth * (ptsy - 1) + ptsx) * 4; // down
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(maskPixels[pixel + 0], hitColorR)
            && CompareThreshold(maskPixels[pixel + 1], hitColorG)
            && CompareThreshold(maskPixels[pixel + 2], hitColorB)
            && CompareThreshold(maskPixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy - 1);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx + 1 < texWidth) {
        pixel = (texWidth * ptsy + ptsx + 1) * 4; // right
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(maskPixels[pixel + 0], hitColorR)
            && CompareThreshold(maskPixels[pixel + 1], hitColorG)
            && CompareThreshold(maskPixels[pixel + 2], hitColorB)
            && CompareThreshold(maskPixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx + 1);
          fillPointY.Enqueue(ptsy);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx - 1 > -1) {
        pixel = (texWidth * ptsy + ptsx - 1) * 4; // left
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(maskPixels[pixel + 0], hitColorR)
            && CompareThreshold(maskPixels[pixel + 1], hitColorG)
            && CompareThreshold(maskPixels[pixel + 2], hitColorB)
            && CompareThreshold(maskPixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx - 1);
          fillPointY.Enqueue(ptsy);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsy + 1 < texHeight) {
        pixel = (texWidth * (ptsy + 1) + ptsx) * 4; // up
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(maskPixels[pixel + 0], hitColorR)
            && CompareThreshold(maskPixels[pixel + 1], hitColorG)
            && CompareThreshold(maskPixels[pixel + 2], hitColorB)
            && CompareThreshold(maskPixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy + 1);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }
    }
  }

  private void FloodFillWithTreshold(int x, int y) {
    // get canvas hit color
    byte hitColorR = pixels[((texWidth * (y) + x) * 4) + 0];
    byte hitColorG = pixels[((texWidth * (y) + x) * 4) + 1];
    byte hitColorB = pixels[((texWidth * (y) + x) * 4) + 2];
    byte hitColorA = pixels[((texWidth * (y) + x) * 4) + 3];

    if (paintColor.r == hitColorR && paintColor.g == hitColorG && paintColor.b == hitColorB && paintColor.a == hitColorA) return;

    Queue<int> fillPointX = new Queue<int>();
    Queue<int> fillPointY = new Queue<int>();
    fillPointX.Enqueue(x);
    fillPointY.Enqueue(y);

    int ptsx, ptsy;
    int pixel = 0;

    lockMaskPixels = new byte[texWidth * texHeight * 4];

    while (fillPointX.Count > 0) {
      ptsx = fillPointX.Dequeue();
      ptsy = fillPointY.Dequeue();

      if (ptsy - 1 > -1) {
        pixel = (texWidth * (ptsy - 1) + ptsx) * 4; // down
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(pixels[pixel + 0], hitColorR)
            && CompareThreshold(pixels[pixel + 1], hitColorG)
            && CompareThreshold(pixels[pixel + 2], hitColorB)
            && CompareThreshold(pixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy - 1);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx + 1 < texWidth) {
        pixel = (texWidth * ptsy + ptsx + 1) * 4; // right
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(pixels[pixel + 0], hitColorR)
            && CompareThreshold(pixels[pixel + 1], hitColorG)
            && CompareThreshold(pixels[pixel + 2], hitColorB)
            && CompareThreshold(pixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx + 1);
          fillPointY.Enqueue(ptsy);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsx - 1 > -1) {
        pixel = (texWidth * ptsy + ptsx - 1) * 4; // left
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(pixels[pixel + 0], hitColorR)
            && CompareThreshold(pixels[pixel + 1], hitColorG)
            && CompareThreshold(pixels[pixel + 2], hitColorB)
            && CompareThreshold(pixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx - 1);
          fillPointY.Enqueue(ptsy);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }

      if (ptsy + 1 < texHeight) {
        pixel = (texWidth * (ptsy + 1) + ptsx) * 4; // up
        if (lockMaskPixels[pixel] == 0
            && CompareThreshold(pixels[pixel + 0], hitColorR)
            && CompareThreshold(pixels[pixel + 1], hitColorG)
            && CompareThreshold(pixels[pixel + 2], hitColorB)
            && CompareThreshold(pixels[pixel + 3], hitColorA)) {
          fillPointX.Enqueue(ptsx);
          fillPointY.Enqueue(ptsy + 1);
          DrawPoint(pixel);
          lockMaskPixels[pixel] = 1;
        }
      }
    }
  }

  private bool CompareThreshold(byte a, byte b) {
    if (a < b) {
      a ^= b; b ^= a; a ^= b;
    }

    return (a - b) <= 128;
  }

  private void DrawPoint(int pixel) {
    pixels[pixel] = paintColor.r;
    pixels[pixel + 1] = paintColor.g;
    pixels[pixel + 2] = paintColor.b;
    pixels[pixel + 3] = paintColor.a;
  }

  private void DrawLine(Vector2 start, Vector2 end) {
    int x0 = (int)start.x;
    int y0 = (int)start.y;
    int x1 = (int)end.x;
    int y1 = (int)end.y;
    int dx = Mathf.Abs(x1 - x0);
    int dy = Mathf.Abs(y1 - y0);
    int sx, sy;
    if (x0 < x1) { sx = 1; } else { sx = -1; }
    if (y0 < y1) { sy = 1; } else { sy = -1; }
    int err = dx - dy;
    bool loop = true;
    int minDistance = (int)(brushSize >> 1);
    int pixelCount = 0;
    int e2;
    while (loop) {
      pixelCount++;
      if (pixelCount > minDistance) {
        pixelCount = 0;
        DrawCircle(x0, y0);
      }
      if ((x0 == x1) && (y0 == y1)) loop = false;
      e2 = 2 * err;
      if (e2 > -dy) {
        err = err - dy;
        x0 = x0 + sx;
      }
      if (e2 < dx) {
        err = err + dx;
        y0 = y0 + sy;
      }
    }
  }

  #endregion
}