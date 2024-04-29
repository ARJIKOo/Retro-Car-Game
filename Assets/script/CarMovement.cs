using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;


public class CarMovement : MonoBehaviour
{
    
    [Header("SPEED")]
    [SerializeField] private float speed;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float increasSpeed;
    [SerializeField] private float DefaultSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private bool isBoost = false;
    
    [Header("CAR MOVEMENT")]
    [SerializeField] private float carRptate;
    public bool moving = false;
    
    [Header("Animation")]
    [SerializeField] private GameObject fireGameObject;
    [SerializeField] private GameObject nitroFire1;
    [SerializeField] private GameObject nitroFire2;
    
    [Header("UI")]
    [Header("UI/speedBOOST")]
    [SerializeField] private Image SpeedBoostImage;
    [SerializeField] private float imageFill = 1;
    [SerializeField] private TextMeshProUGUI FoodCount;
    [SerializeField] private int FoodCoin;

    [Header("UI/SPIDOMETR")] 
    [SerializeField] private Image spidometrImage;
    [SerializeField] private float spidometrRotation;
    [SerializeField] private float spidometrRotationMaxNum;
    [SerializeField] private float spidometrRotationdef;
    [SerializeField] private TextMeshProUGUI spidometrNum;
    
    

    [Header("Timer")]
    [SerializeField] public float TimerCount;
    [SerializeField] public float defaultTimerCount;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        FoodCoinLoad();
        
    }

    // Update is called once per frame
    void Update()
    {
      CarRotate();
      CarMove();
      Timer();
        
    }

    public void CarMove()
    {
        if (speed >= MaxSpeed && isBoost ==false)
        {
            speed = MaxSpeed;
            nitroFire1.SetActive(false);
            nitroFire2.SetActive(false);
            

        }else if (speed >= MaxSpeed && isBoost == true)
        {
            speed = boostSpeed;
            nitroFire1.SetActive(true);
            nitroFire2.SetActive(true);
        }
        
  
        
        
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            speed += increasSpeed * Time.deltaTime;
            
            transform.Translate(0,y: -speed * Time.deltaTime,0);
            
            moving = true;

            if (speed <= MaxSpeed )
            {
                spidometrRotation += speed /spidometrRotation  ;
                spidometrRotationdef = (int)spidometrRotation * 2;
                spidometrImage.transform.rotation=Quaternion.Euler(0,0,-spidometrRotation*3);
                spidometrNum.text = spidometrRotationdef.ToString();
            }else if (isBoost == true && speed <= boostSpeed+1)
            {
                spidometrRotation += speed /spidometrRotation  ;
                spidometrRotationdef = (int)spidometrRotation * 2;
                if (spidometrRotation <= spidometrRotationMaxNum)
                {
                    spidometrImage.transform.rotation=Quaternion.Euler(0,0,-spidometrRotation*3);
                    spidometrNum.text = spidometrRotationdef.ToString();
                }
                
            }
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            speed += increasSpeed * Time.deltaTime;
            transform.Translate(0,speed * Time.deltaTime,0);
            moving = true;
            if (speed <= MaxSpeed && isBoost==false )
            {
                spidometrRotation += speed /spidometrRotation  ;
                spidometrRotationdef = (int)spidometrRotation * 2;
                spidometrImage.transform.rotation=Quaternion.Euler(0,0,-spidometrRotation*3);
                spidometrNum.text = spidometrRotationdef.ToString();
            }else if (isBoost == true && speed <= 17)
            {
                spidometrRotation += speed /spidometrRotation  ;
                spidometrRotationdef = (int)spidometrRotation * 2;
                spidometrImage.transform.rotation=Quaternion.Euler(0,0,-spidometrRotation*3);
                spidometrNum.text = spidometrRotationdef.ToString();
            }
        }
        else
        {
            moving = false;
            speed = 0;
            spidometrRotation = 1f;
            spidometrRotationdef = 1f;
            spidometrImage.transform.rotation=Quaternion.Euler(0,0,0);
            spidometrNum.text = 0.ToString();
        }
        
       
        
        
        
    }

    public void CarRotate()
    {
        if (Input.GetKey(KeyCode.RightArrow) && moving==true)
        {
            transform.Rotate(0,0,-carRptate );
        }else if (Input.GetKey(KeyCode.LeftArrow) && moving==true)
        {
            transform.Rotate(0,0,carRptate );
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            fireGameObject.SetActive(true);
            speed = 0f;
            increasSpeed = 0f;
            carRptate = 0f;
            FoodCoinSave();
            Invoke("gameRestart",3);
            nitroFire1.SetActive(false);
            nitroFire2.SetActive(false);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            isBoost = true;
            speed = boostSpeed;
            Invoke("DefaultSpeeds",defaultTimerCount);
            
        }

        if (other.gameObject.CompareTag("Food"))
        {
            FoodCoin ++;
        
            FoodCount.text = FoodCoin.ToString();
        }
               
    }

    void DefaultSpeeds()
    {
        speed = DefaultSpeed;
        isBoost = false;
    }

    void gameRestart()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
     }

    void FoodCoinSave()
    {
        PlayerPrefs.SetInt("FoodCoinKay",FoodCoin);
        PlayerPrefs.Save();
    }

    void FoodCoinLoad()
    {
        if (PlayerPrefs.HasKey("FoodCoinKay"))
        {
            FoodCoin = PlayerPrefs.GetInt("FoodCoinKay");
            FoodCount.text = FoodCoin.ToString();
        }
    }

    void Timer()
    {
        if (isBoost == true && TimerCount >= 0)
        {
            TimerCount -= Time.deltaTime;
            SpeedBoostImage.fillAmount = TimerCount/defaultTimerCount;
        }
        else
        {
            TimerCount = defaultTimerCount;
            isBoost = false;
            SpeedBoostImage.fillAmount = 1;

        }
        
    }
}
