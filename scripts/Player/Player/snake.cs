using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class snake : MonoBehaviour
{   public PlayerInputControl inputControl;
    public Vector2 facedir;
    public GameObject pepper;
    private int len=2;//蛇的长度 
    private float step;//步长
    public GameObject tail;
    public Sprite[] tailSkins;//存储尾巴贴图
    public SpriteRenderer tailrender;//尾巴贴图
    public Vector2 direction = new Vector2(0,0); // 蛇的移动方向  
      
    public Transform[] snakeParts ; // 蛇的身体部分列表  
    public Sprite[] snakeSkins;
    public SpriteRenderer[] snakeSkinRenderers; // 存储每个身体部分的贴图
    public Transform headpos;
    public SpriteRenderer head;
    public Sprite[] headSkin;
    private bool shouldMove = false;
    private bool change=true;
    //飞行
    private float flyvelocity=10000;
    // Start is called before the first frame update  
    private void Awake()
    {
        inputControl=new PlayerInputControl();
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }

    void Start()  
    {  
        facedir=new Vector2(1,0);
         step=1f;
    }  
  
    // Update is called once per frame  
    private void Update(){
         direction = inputControl.Gameplay.Move.ReadValue<Vector2>();

         if ((direction.x!=0||direction.y!=0)&&change){
            shouldMove =true;
            change=false; 
         }
         if(direction.x==0&&direction.y==0){
            shouldMove =false;
            change=true;
         }
    }
    void FixedUpdate()  
    {     // 移动蛇的头部     
            Move();
    }  
    void tailchange(){
        if(snakeSkinRenderers[len-1].sprite==snakeSkins[0]){
            if(tailrender.sprite==tailSkins[0]){
                tailrender.sprite=tailSkins[3];
            }else{
                tailrender.sprite=tailSkins[2];
            }
        }
        if(snakeSkinRenderers[len-1].sprite==snakeSkins[1]){
            if(tailrender.sprite==tailSkins[1]){
                tailrender.sprite=tailSkins[0];
            }else{
                tailrender.sprite=tailSkins[3];
            }
        }
        if(snakeSkinRenderers[len-1].sprite==snakeSkins[2]){
            if(tailrender.sprite==tailSkins[0]){
                tailrender.sprite=tailSkins[1];
            }else{
                tailrender.sprite=tailSkins[2];
            }
        }if(snakeSkinRenderers[len-1].sprite==snakeSkins[3]){
            if(tailrender.sprite==tailSkins[2]){
                tailrender.sprite=tailSkins[1];
            }else{
                tailrender.sprite=tailSkins[0];
            }
        }
    }
    void bodychange(){
         Vector2 temp=direction+2*facedir;
         if(temp==new Vector2(2,1)||temp==new Vector2(-1,-2)){
        snakeSkinRenderers[0].sprite=snakeSkins[0];
         }else if(temp==new Vector2(-2,1)||temp==new Vector2(1,-2)){
            snakeSkinRenderers[0].sprite=snakeSkins[1];
         }else if(temp==new Vector2(2,-1)||temp==new Vector2(-1,2)){
            snakeSkinRenderers[0].sprite=snakeSkins[2];
         }else if(temp==new Vector2(1,2)||temp==new Vector2(-2,-1)){
            snakeSkinRenderers[0].sprite=snakeSkins[3];
         }
    }
    void bodyset(){
        
        if (direction==new Vector2(1,0)||direction==new Vector2(-1,0)){
            snakeSkinRenderers[0].sprite=snakeSkins[4];
        }else if (direction==new Vector2(0,1)||direction==new Vector2(0,-1)){
            snakeSkinRenderers[0].sprite=snakeSkins[5];
        }
    }
    void headchange(){
        if(direction==new Vector2(1,0)){
            head.sprite=headSkin[0];
        }else  if(direction==new Vector2(0,-1)){
            head.sprite=headSkin[1];
        }else  if(direction==new Vector2(-1,0)){
            head.sprite=headSkin[2];
        }else  if(direction==new Vector2(0,1)){
            head.sprite=headSkin[3];
        }
    }
    private void Move()
    {   if (shouldMove&&direction!=-facedir){
        Vector3 newPosition = (Vector3)headpos.position + (Vector3)direction * step ;
         //更新蛇的贴图
        headchange();
        tailchange();  
        for (int i = len - 1; i > 0; i--) 
        {
        snakeSkinRenderers[i].sprite=snakeSkinRenderers[i-1].sprite;
        }
        bodyset();
        if(direction!=facedir&&facedir!=new Vector2(0,0))
        {
        bodychange();
        }
        facedir=direction;
        // 移动蛇  
        tail.transform.position=snakeParts[len-1].position;
         for (int i = len - 1; i > 0; i--)  
        {  
            snakeParts[i].position = snakeParts[i - 1].position;  
        }  
        snakeParts[0].position=headpos.position;
        headpos.position = newPosition;  
        shouldMove=false;
    }
    }  
   
    void OnCollisionEnter(ControllerColliderHit hit){
         if (hit.rigidbody==pepper.GetComponent<Rigidbody>()){
            Destroy(pepper);
         }
         fly();
    }
    void fly(){
        for(int i=0;i<10;i++){
        headpos.position*=direction*flyvelocity;
        }

    }
}
