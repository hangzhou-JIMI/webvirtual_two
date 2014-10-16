using UnityEngine;
using System.Collections;


// 	Author: Lu Zexi
//	2014-10-16



/// <summary>
/// Camera coc touch.
/// </summary>
public class CamerCOCTouch : MonoBehaviour
{
	private bool moveEnable = true;		//enable move camera
	private bool scaleEnable = true;	//enable scale camera

	private float scale_min_y = 10;		//scale min y
	private float scale_max_y = 100;	//scale max y

	private float move_min_x = -100;	//move min x
	private float move_max_x = 100;		//move max x
	private float move_min_y = -100;	//move min y
	private float move_max_y = 100;		//move max y

	private float move_rate_min = 0.01f;	//move rate x
	private float move_rate_max = 0.2f;	//move rate x
	private float scale_rate = 0.05f;	//scale rate

	//temp value
	private Vector2 mousePos;
	private bool mouseStart = false;
	private Vector2 mousePos2;
	private bool mouseStart2 = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(this.moveEnable)
		{
			if (Input.touchCount > 0)
			{
				int fingerCount = 0;
				Touch curTouch = new Touch();
				foreach (Touch touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Moved)
					{
						fingerCount++;
						curTouch = touch;
					}
				}
				if (fingerCount == 1 && Input.touchCount == 1)
				{
					Vector2 delta = curTouch.deltaPosition;
					float move_rate = this.move_rate_min + this.move_rate_max*(this.transform.position.y - this.scale_min_y)/(this.scale_max_y - this.scale_min_y);
					Vector3 transPos = this.transform.position + Vector3.right*delta.x*-move_rate + Vector3.forward*delta.y*-move_rate;
					if(transPos.x >= this.move_min_x && transPos.x <= this.move_max_x
					   && transPos.z >= this.move_min_y && transPos.z <= this.move_max_y)
					{
						Debug.Log( "move vect : " + (Vector3.right*delta.x*-move_rate + Vector3.forward*delta.y*-move_rate) );
						this.transform.position = transPos;
					}
				}
			}
#if UNITY_EDITOR
			if( Input.GetMouseButton(0) )
			{
				if(!this.mouseStart)
				{
					this.mouseStart = true;
					this.mousePos = new Vector2(Input.mousePosition.x , Input.mousePosition.y);
				}
				Vector2 delta = new Vector2( Input.mousePosition.x , Input.mousePosition.y ) - this.mousePos;
				this.mousePos = Input.mousePosition;
				float move_rate = this.move_rate_min + this.move_rate_max*(this.transform.position.y - this.scale_min_y)/(this.scale_max_y - this.scale_min_y);
				Vector3 transPos = this.transform.position + Vector3.right*delta.x*-move_rate + Vector3.forward*delta.y*-move_rate;
				if(transPos.x >= this.move_min_x && transPos.x <= this.move_max_x
				   && transPos.z >= this.move_min_y && transPos.z <= this.move_max_y)
				{
					Debug.Log("move rate :" + move_rate);
					this.transform.position = transPos;
				}
			}
			else
			{
				this.mouseStart = false;
			}
#endif
		}

		if(this.scaleEnable)
		{
			if (Input.touchCount == 2)
			{
				int fingerCount = 0;
				Touch curTouch = Input.touches[0];
				Touch curTouch2 = Input.touches[1];
				foreach (Touch touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Moved)
					{
						fingerCount++;
					}
				}
				if (fingerCount >= 1)
				{
					float mg = (curTouch.deltaPosition - curTouch2.deltaPosition).magnitude;
					float mg_rate = 1;
					float dis = (curTouch.position - curTouch2.position).magnitude;
					float last_dis = ( (curTouch.position - curTouch.deltaPosition) - (curTouch2.position - curTouch2.deltaPosition) ).magnitude;
					if( last_dis > dis )
						mg_rate = -1;

					Vector2 delta = curTouch.deltaPosition;
					Vector3 transPos = this.transform.position + (this.transform.forward*mg*mg_rate*-this.scale_rate);
					if(transPos.y >= this.scale_min_y && transPos.y <= this.scale_max_y)
					{
						Debug.Log( "scale vect : " + (this.transform.forward*mg*mg_rate*this.scale_rate) );
						this.transform.position = transPos;
					}
				}
			}
#if UNITY_EDITOR
			if( Input.GetMouseButton(1) )
			{
				if(!this.mouseStart2)
				{
					this.mouseStart2 = true;
					this.mousePos2 = new Vector2(Input.mousePosition.x , Input.mousePosition.y);
				}
				Vector2 delta = new Vector2( Input.mousePosition.x , Input.mousePosition.y ) - this.mousePos2;
				this.mousePos2 = Input.mousePosition;
				Vector3 transPos = this.transform.position + (this.transform.forward*delta.x*-this.scale_rate);
				if(transPos.y >= this.scale_min_y && transPos.y <= this.scale_max_y)
				{
					this.transform.position = transPos;
				}
			}
			else
			{
				this.mouseStart2 = false;
			}
#endif
		}
	}
}
