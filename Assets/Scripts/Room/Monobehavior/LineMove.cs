using UnityEngine;

public class LineMove : MonoBehaviour
{
  public LineRenderer lineRenderer;
  public float speed = 0.2f;
void Update()
  {
    var offset=lineRenderer.material.mainTextureOffset;
    offset.x+=speed*Time.deltaTime;
    lineRenderer.material.mainTextureOffset=offset;
  }
}
