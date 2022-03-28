using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    Vector3 playerPos;
    Vector3 targetPos;

    #region RED_CAR_VARIABLES && Person_Variables
    private Vector4 Red_WaterCarShader;
    private Vector4 Red_DirtyCarShader;
    private Vector4 Red_BubbleCarShader;
    private Material Red_WaterCarMat;
    private Material Red_BubbleCarMat;
    private Material Red_DirtyCarMat;
    private float Red_minShader;
    private float Red_maxShader;
    public float Red_DissolveSpeed;
    private Vector4 PersonShader;
    private Material PersonMat;
    #endregion

    #region PINK_CAR_VARIABLES
    private Vector4 Pink_WaterCarShader;
    private Vector4 Pink_DirtyCarShader;
    private Vector4 Pink_BubbleCarShader;
    private Material Pink_WaterCarMat;
    private Material Pink_BubbleCarMat;
    private Material Pink_DirtyCarMat;
    private float Pink_minShader;
    private float Pink_maxShader;
    public float Pink_DissolveSpeed;
    #endregion

    #region YELLOW_CAR_VARIABLES
    private Vector4 Yellow_WaterCarShader;
    private Vector4 Yellow_DirtyCarShader;
    private Vector4 Yellow_BubbleCarShader;
    private Material Yellow_WaterCarMat;
    private Material Yellow_BubbleCarMat;
    private Material Yellow_DirtyCarMat;
    private float Yellow_minShader;
    private float Yellow_maxShader;
    public float Yellow_DissolveSpeed;
    #endregion

    #region ORANGE_CAR_VARIABLES
    private Vector4 Orange_WaterCarShader;
    private Vector4 Orange_DirtyCarShader;
    private Vector4 Orange_BubbleCarShader;
    private Material Orange_WaterCarMat;
    private Material Orange_BubbleCarMat;
    private Material Orange_DirtyCarMat;
    private float Orange_minShader;
    private float Orange_maxShader;
    public float Orange_DissolveSpeed;
    #endregion

    #region GREEN_CAR_VARIABLES
    private Vector4 Green_WaterCarShader;
    private Vector4 Green_DirtyCarShader;
    private Vector4 Green_BubbleCarShader;
    private Material Green_WaterCarMat;
    private Material Green_BubbleCarMat;
    private Material Green_DirtyCarMat;
    private float Green_minShader;
    private float Green_maxShader;
    public float Green_DissolveSpeed;
    #endregion

    public Slider slider; // Player Follow SliderBar
    public GameObject FinishLine; // Finishline
    public TextMeshProUGUI MoneyText; // Money Text
    public GameObject PerfectSign;
    public GameObject BadSign;
    public GameObject GoodSign;
    public Transform Spawner; // Keeps produced signs
    public GameObject MoneyTarget; 
    public SlowMotion SlowMotion; // SlowMotion Script
    public ParticleSystem Confetti; //Confetti Particle
    Animation GreatAnim;
    public GameObject greatImage;


    public MoneyCollect moneyCollect; //Money Anim Script
    private float Maxdistance;
    public float smooth = 1;
    public float Speed = 2; //CurrrentSpeed
    public float maxSpeed = 3; // maximum Speed
    public float pSpeed = 0.25f; //perfectSpeed
    public float gSpeed = 0.1f; //badSpeed
    public float bSpeed = 0.2f; //goodSpeed
    
    [SerializeField]
    public int Money;
    private int AiScore;
    private float signHigh;

    public bool isMoving = false;
    bool once = false; //needed to find finishline obj.
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        //random bool // bool trueorfalse = (Random.value > 0.5f);


        #region Car Start Variables
        ///<Summary>
        /// cars shader and dissolve speed. hold to limits maxshader & minshader
        /// </Summary>>
        //RedCar
        Red_BubbleCarShader.z = 1.26f;
        Red_DirtyCarShader.z = 1.26f;
        Red_WaterCarShader.z = 1.26f;
        Red_maxShader = -0.82f;
        Red_minShader = 0.56f;
        Red_DissolveSpeed = 4f; //Dengeli//

        //PinkCar
        Pink_BubbleCarShader.z = 4;
        Pink_DirtyCarShader.z = 4;
        Pink_WaterCarShader.z = 4;
        Pink_maxShader = -2.5f;
        Pink_minShader = 2;
        Pink_DissolveSpeed = 7.5f; //Dengeli//

        //YellowCar
        Yellow_BubbleCarShader.z = 2.25f;
        Yellow_DirtyCarShader.z = 2.25f;
        Yellow_WaterCarShader.z = 2.25f;
        Yellow_maxShader = -1.75f;
        Yellow_minShader = 1.3f;
        Yellow_DissolveSpeed = 5f; //Dengeli //

        //OrangeCar
        Orange_BubbleCarShader.z = 1.75f;
        Orange_DirtyCarShader.z = 1.75f;
        Orange_WaterCarShader.z = 1.75f;
        Orange_maxShader = 0.03f;
        Orange_minShader = 1.25f;
        Orange_DissolveSpeed = 3f; // Dengeli // 

        //GreenCar
        Green_BubbleCarShader.z = 1.4f;
        Green_DirtyCarShader.z = 1.4f;
        Green_WaterCarShader.z = 1.4f;
        Green_maxShader = -0.9f;
        Green_minShader = 0.8f;
        Green_DissolveSpeed = 3f; // Dengeli//
        #endregion
        
        playerPos = transform.position;
        signHigh = 2.5f; // Player Sign High
        GreatAnim = greatImage.gameObject.GetComponent<Animation>();

        if (PlayerPrefs.HasKey("Money"))
        {
           Money = PlayerPrefs.GetInt("Money");
        }
        Debug.Log(Money);

        
    }

    float getDistance() // Player - Finish Distance
    {
        return Vector2.Distance(this.transform.position,FinishLine.transform.position);
    }

    void SetProgress(float sliderDis) // Sliderbar set value 
    {
        slider.value = sliderDis;
    }

    private void FixedUpdate()
    {
        if (Money < 0)
        {
            Money = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (FinishLine && !once)
        {
            Maxdistance = getDistance();
            slider.maxValue = Maxdistance;
            once = true;
        }
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
        {
            return;
        }
        
        if (GameManager.instance.levelCount > 9)
        {
            return;
        }

        if (isMoving && !GameManager.isGameEnded)
        {
            PlayerMovement();
            MoneyText.text = Money.ToString();
            if (transform.position.x < Maxdistance && transform.position.x <= FinishLine.transform.position.x)
            {
                float distance =  Maxdistance - getDistance();
                SetProgress(distance);
            }
        }


    }

    void PlayerMovement() // Player Movement
    {
        if (isMoving)
        {
            targetPos = FinishLine.transform.position;
            playerPos = transform.position;
            //transform.position = Vector3.MoveTowards(playerPos, Vector3.Lerp(playerPos, targetPos, smooth), Speed * Time.deltaTime);
            transform.position += Vector3.right * Speed * Time.deltaTime;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerFinish" && !GameManager.isGameEnded)
        {
            Confetti.Play();
            AiScore = AiController.instance.AiScore;
            Debug.Log(AiScore + " && " + Money);
            Debug.Log("AISpeed: " + AiController.instance.Speed);
            Debug.Log("PlayerSpeed: " + Speed);
            GameManager.instance.OnLevelEnded();
            GreatAnim.Play();
        }

        #region Slow Motion Time
        if (other.gameObject.tag == "SlowMotionForWatery")
        {
            SlowMotion.SlowMotionEffect();
        }

        if (other.gameObject.tag == "SlowMotionForDirty")
        {
            SlowMotion.SlowMotionEffect();
        }

        if (other.gameObject.tag == "SlowMotionForFoam")
        {
            SlowMotion.SlowMotionEffect();
        }

        if (other.gameObject.tag == "SlowMotionForHuman")
        {
            SlowMotion.SlowMotionEffect();
        }
        #endregion
    }
    private void OnTriggerStay(Collider other)
    {
        #region Red_Car Properties
        ///<Summary>
        ///eger BubbleCar ise  && isClickedWashButton true ise 
        ///</Summary>>
        if (other.gameObject.tag == "Red_BubbleCar" && GameManager.instance.isClickedWashButton)
        {
            Red_BubbleCarMat = other.GetComponent<Renderer>().material;
            Red_BubbleCarShader.z -= Time.deltaTime * Red_DissolveSpeed;
            Red_BubbleCarMat.SetVector("_DissolveOffest", Red_BubbleCarShader);
        }
        ///<Summary>
        ///eger DirtyCar ise  && isClickedBubbleButton true ise 
        ///</Summary>>
        if (other.gameObject.tag == "Red_DirtyCar" && GameManager.instance.isClickedBubbleButton)
        {
            Red_DirtyCarMat = other.GetComponent<Renderer>().material;
            Red_DirtyCarShader.z -= Time.deltaTime * Red_DissolveSpeed;
            Red_DirtyCarMat.SetVector("_DissolveOffest", Red_DirtyCarShader);
        }
        ///<Summary>
        ///eger WaterCar ise  && isClickedAirButton true ise 
        ///</Summary>>
        if (other.gameObject.tag == "Red_WaterCar" && GameManager.instance.isClickedAirButton)
        {
            Red_WaterCarMat = other.GetComponent<Renderer>().material;
            Red_WaterCarShader.z -= Time.deltaTime * Red_DissolveSpeed;
            Red_WaterCarMat.SetVector("_DissolveOffest", Red_WaterCarShader);
        }
        #endregion

        #region Pink_Car Properties

        if (other.gameObject.tag == "Pink_BubbleCar" && GameManager.instance.isClickedWashButton)
        {
            Pink_BubbleCarMat = other.GetComponent<Renderer>().material;
            Pink_BubbleCarShader.z -= Time.deltaTime * Pink_DissolveSpeed;
            Pink_BubbleCarMat.SetVector("_DissolveOffest", Pink_BubbleCarShader);
        }

        if (other.gameObject.tag == "Pink_DirtyCar" && GameManager.instance.isClickedBubbleButton)
        {
            Pink_DirtyCarMat = other.GetComponent<Renderer>().material;
            Pink_DirtyCarShader.z -= Time.deltaTime * Pink_DissolveSpeed;
            Pink_DirtyCarMat.SetVector("_DissolveOffest", Pink_DirtyCarShader);
        }

        if (other.gameObject.tag == "Pink_WaterCar" && GameManager.instance.isClickedAirButton)
        {
            Pink_WaterCarMat = other.GetComponent<Renderer>().material;
            Pink_WaterCarShader.z -= Time.deltaTime * Pink_DissolveSpeed;
            Pink_WaterCarMat.SetVector("_DissolveOffest", Pink_WaterCarShader);
        }
        #endregion

        #region Yellow_Car Properties

        if (other.gameObject.tag == "Yellow_BubbleCar" && GameManager.instance.isClickedWashButton)
        {
            Yellow_BubbleCarMat = other.GetComponent<Renderer>().material;
            Yellow_BubbleCarShader.z -= Time.deltaTime * Yellow_DissolveSpeed;
            Yellow_BubbleCarMat.SetVector("_DissolveOffest", Yellow_BubbleCarShader);
        }

        if (other.gameObject.tag == "Yellow_DirtyCar" && GameManager.instance.isClickedBubbleButton)
        {
            Yellow_DirtyCarMat = other.GetComponent<Renderer>().material;
            Yellow_DirtyCarShader.z -= Time.deltaTime * Pink_DissolveSpeed;
            Yellow_DirtyCarMat.SetVector("_DissolveOffest", Yellow_DirtyCarShader);
        }

        if (other.gameObject.tag == "Yellow_WaterCar" && GameManager.instance.isClickedAirButton)
        {
            Yellow_WaterCarMat = other.GetComponent<Renderer>().material;
            Yellow_WaterCarShader.z -= Time.deltaTime * Yellow_DissolveSpeed;
            Yellow_WaterCarMat.SetVector("_DissolveOffest", Yellow_WaterCarShader);
        }
        #endregion

        #region Orange_Car Properties

        if (other.gameObject.tag == "Orange_BubbleCar" && GameManager.instance.isClickedWashButton)
        {
            Orange_BubbleCarMat = other.GetComponent<Renderer>().material;
            Orange_BubbleCarShader.z -= Time.deltaTime * Orange_DissolveSpeed;
            Orange_BubbleCarMat.SetVector("_DissolveOffest", Orange_BubbleCarShader);
        }

        if (other.gameObject.tag == "Orange_DirtyCar" && GameManager.instance.isClickedBubbleButton)
        {
            Orange_DirtyCarMat = other.GetComponent<Renderer>().material;
            Orange_DirtyCarShader.z -= Time.deltaTime * Orange_DissolveSpeed;
            Orange_DirtyCarMat.SetVector("_DissolveOffest", Orange_DirtyCarShader);
        }

        if (other.gameObject.tag == "Orange_WaterCar" && GameManager.instance.isClickedAirButton)
        {
            Orange_WaterCarMat = other.GetComponent<Renderer>().material;
            Orange_WaterCarShader.z -= Time.deltaTime * Orange_DissolveSpeed;
            Orange_WaterCarMat.SetVector("_DissolveOffest", Orange_WaterCarShader);
        }
        #endregion

        #region Green_Car Properties

        if (other.gameObject.tag == "Green_BubbleCar" && GameManager.instance.isClickedWashButton)
        {
            Green_BubbleCarMat = other.GetComponent<Renderer>().material;
            Green_BubbleCarShader.z -= Time.deltaTime * Green_DissolveSpeed;
            Green_BubbleCarMat.SetVector("_DissolveOffest", Green_BubbleCarShader);
        }

        if (other.gameObject.tag == "Green_DirtyCar" && GameManager.instance.isClickedBubbleButton)
        {
            Green_DirtyCarMat = other.GetComponent<Renderer>().material;
            Green_DirtyCarShader.z -= Time.deltaTime * Green_DissolveSpeed;
            Green_DirtyCarMat.SetVector("_DissolveOffest", Green_DirtyCarShader);
        }

        if (other.gameObject.tag == "Green_WaterCar" && GameManager.instance.isClickedAirButton)
        {
            Green_WaterCarMat = other.GetComponent<Renderer>().material;
            Green_WaterCarShader.z -= Time.deltaTime * Green_DissolveSpeed;
            Green_WaterCarMat.SetVector("_DissolveOffest", Green_WaterCarShader);
        }
        #endregion

        if (other.gameObject.tag == "Person" && GameManager.instance.isClickedButton)
        {
            PersonMat = other.GetComponent<Renderer>().material;
            PersonShader.x -= Time.deltaTime * Red_DissolveSpeed;
            PersonMat.SetVector("_DissolveOffest", PersonShader);
        }

        #region Slow Motion Time
        if (other.gameObject.tag == "SlowMotionForWatery")
        {
            //active Watery Tutorial Panel
            GameManager.instance.WateryPanel.SetActive(true);
        }

        if (other.gameObject.tag == "SlowMotionForDirty")
        {
            // active Dirty Tutorial Panel
            GameManager.instance.DirtyPanel.SetActive(true);
        }

        if (other.gameObject.tag == "SlowMotionForFoam")
        {
            //Active Foam Tutorial Panel
            GameManager.instance.FoamPanel.SetActive(true);
        }

        if (other.gameObject.tag == "SlowMotionForHuman")
        {
            //Active Human Tutorial Panel
            GameManager.instance.HumanPanel.SetActive(true);
        }
        #endregion

    }

    private void OnTriggerExit(Collider other)
    {
        #region Red_Car Properties
        if (other.gameObject.CompareTag("Red_WaterCar"))
        {
            RedCheckCleanWater();
        }

        if (other.gameObject.tag == "Red_DirtyCar")
        {
            RedCheckCleanDirty();
        }

        if (other.gameObject.tag == "Red_BubbleCar")
        {
            RedCheckCleanBubble();
        }
        #endregion

        #region Pink_Car Properties
        if (other.gameObject.tag == "Pink_WaterCar")
        {
            PinkCheckCleanWater();
        }

        if (other.gameObject.tag == "Pink_DirtyCar")
        {
            PinkCheckCleanDirty();
        }

        if (other.gameObject.tag == "Pink_BubbleCar")
        {
            PinkCheckCleanBubble();
        }
        #endregion

        #region Yellow_Car Properties
        if (other.gameObject.tag == "Yellow_WaterCar")
        {
            YellowCheckCleanWater();
        }

        if (other.gameObject.tag == "Yellow_DirtyCar")
        {
            YellowCheckCleanDirty();
        }

        if (other.gameObject.tag == "Yellow_BubbleCar")
        {
            YellowCheckCleanBubble();
        }
        #endregion

        #region Orange_Car Properties
        if (other.gameObject.tag == "Orange_WaterCar")
        {
            OrangeCheckCleanWater();
        }

        if (other.gameObject.tag == "Orange_DirtyCar")
        {
            OrangeCheckCleanDirty();
        }

        if (other.gameObject.tag == "Orange_BubbleCar")
        {
            OrangeCheckCleanBubble();
        }
        #endregion

        #region Green_Car Properties
        if (other.gameObject.tag == "Green_WaterCar")
        {
            GreenCheckCleanWater();
        }

        if (other.gameObject.tag == "Green_DirtyCar")
        {
            GreenCheckCleanDirty();
        }

        if (other.gameObject.tag == "Green_BubbleCar")
        {
            GreenCheckCleanBubble();
        }
        #endregion

        #region Stop Slow Motion Time
        if (other.gameObject.tag == "SlowMotionForWatery")
        {
            //deactive Watery Tutorial Panel
            GameManager.instance.WateryPanel.SetActive(false);
        }

        if (other.gameObject.tag == "SlowMotionForDirty")
        {
            // deactive Dirty Tutorial Panel
            GameManager.instance.DirtyPanel.SetActive(false);
        }

        if (other.gameObject.tag == "SlowMotionForFoam")
        {
            //deActive Foam Tutorial Panel
            GameManager.instance.FoamPanel.SetActive(false);
        }

        if (other.gameObject.tag == "SlowMotionForHuman")
        {
            //deActive Human Tutorial Panel
            GameManager.instance.HumanPanel.SetActive(false);
        }
        #endregion

        if (other.gameObject.tag == "Person")
        {
            CheckCleanPerson();
        }
        
        // PlayerPrefs.SetInt("Money", Money);
    }

    /// <summary>
    /// Checks the cleanliness of each car.
    /// </summary>
    #region RED_CAR_CLEANING
    void RedCheckCleanDirty()
    {
        ///<Smummary>
        ///DirtyCar Check
        ///</Smummary>>
        if (Red_DirtyCarShader.z < Red_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x,transform.position.y + signHigh, transform.position.z),PerfectSign.transform.rotation,Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Red_DirtyCarShader.z > Red_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else if (Mathf.RoundToInt(Red_DirtyCarShader.z) == 0)
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
       
    }
    void RedCheckCleanWater()
    {
        ///<Smummary>
        ///WaterCar Check
        ///</Smummary>>
        if (Red_WaterCarShader.z < Red_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Red_WaterCarShader.z > Red_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else if (Mathf.RoundToInt(Red_WaterCarShader.z) == 0)
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
        
    }

    void RedCheckCleanBubble()
    {
        ///<Smummary>
        ///BubbleCar Check
        ///</Smummary>>
        if (Red_BubbleCarShader.z < Red_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Red_BubbleCarShader.z > Red_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else if (Mathf.RoundToInt(Red_BubbleCarShader.z) == 0)
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
    }
    #endregion

    #region PINK_CAR_CHECKING

    void PinkCheckCleanWater()
    {
        if (Pink_WaterCarShader.z < Pink_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Pink_WaterCarShader.z > Pink_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

    }
    void PinkCheckCleanDirty ()
    {
        if (Pink_DirtyCarShader.z < Pink_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Pink_DirtyCarShader.z > Pink_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);

        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
        
    }

    void PinkCheckCleanBubble()
    {
        if (Pink_BubbleCarShader.z < Pink_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Pink_BubbleCarShader.z >  Pink_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
    }
    #endregion

    #region YELLOW_CAR_CHECKING

    void YellowCheckCleanWater()
    {
        if (Yellow_WaterCarShader.z < Yellow_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Yellow_WaterCarShader.z > Yellow_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

    }
    void YellowCheckCleanDirty()
    {
        if (Yellow_DirtyCarShader.z < Yellow_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            Speed += pSpeed;
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Yellow_DirtyCarShader.z > Yellow_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed < 3)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

    }

    void YellowCheckCleanBubble()
    {
        if (Yellow_BubbleCarShader.z < Yellow_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Yellow_BubbleCarShader.z > Yellow_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
    }
    #endregion

    #region ORANGE_CAR_CHECKING

    void OrangeCheckCleanWater()
    {
        if (Orange_WaterCarShader.z < Orange_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Orange_WaterCarShader.z > Orange_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

    }
    void OrangeCheckCleanDirty()
    {
        if (Orange_DirtyCarShader.z < Orange_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Orange_DirtyCarShader.z > Orange_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

    }

    void OrangeCheckCleanBubble()
    {
        if (Orange_BubbleCarShader.z < Orange_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Orange_BubbleCarShader.z > Orange_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
    }
    #endregion

    #region GREEN_CAR_CHECKING

    void GreenCheckCleanWater()
    {
        if (Green_WaterCarShader.z < Green_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Green_WaterCarShader.z > Green_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

    }
    void GreenCheckCleanDirty()
    {
        if (Green_DirtyCarShader.z < Green_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

        else if (Green_DirtyCarShader.z > Green_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }

    }

    void GreenCheckCleanBubble()
    {
        if (Green_BubbleCarShader.z < Green_maxShader)
        {
            //Perfect
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540,960,0));
        }

        else if (Green_BubbleCarShader.z > Green_minShader)
        {
            //Bad
            if (Money > 0)
            {
                Money -= 2;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
        }

        else
        {
            //Good
            Money += 3;
            MoneyText.text = Money.ToString();
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
        }
    }
    #endregion

    /// <summary>
    /// Check the cleanliness of each person
    /// </summary>
    void CheckCleanPerson ()
    {
        if (PersonShader.x < -0.3f)
        {
            if (Money > 0)
            {
                Money -= 5;
            }
            MoneyText.text = Money.ToString();
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            moneyCollect.FailMoneyMove(MoneyTarget.transform.position);
            Speed -= 0.1f;
        }

        else
        {
            Money += 5;
            MoneyText.text = Money.ToString();
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            moneyCollect.StartMoneyMove(new Vector3(540, 960, 0));
            if (Speed < maxSpeed)
            {
                Speed += 0.1f;
            }
        }
    }

}
