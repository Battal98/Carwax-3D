using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiController : MonoBehaviour
{
    public static AiController instance;
    #region Variables
    Vector3 playerPos;
    Vector3 targetPos;

    #region RED_CAR_VARIABLES
    private Vector4 Red_WaterCarShader;
    private Vector4 Red_DirtyCarShader;
    private Vector4 Red_BubbleCarShader;
    private Material Red_WaterCarMat;
    private Material Red_BubbleCarMat;
    private Material Red_DirtyCarMat;
    private float Red_minShader;
    private float Red_maxShader;
    private float Red_randomfloat;
    private Material PersonMat;
    private Vector4 PersonShader;
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
    private float Pink_randomfloat;
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
    private float Yellow_randomfloat;
    #endregion

    #region OORANGE_CAR_VARIABLES
    private Vector4 Orange_WaterCarShader;
    private Vector4 Orange_DirtyCarShader;
    private Vector4 Orange_BubbleCarShader;
    private Material Orange_WaterCarMat;
    private Material Orange_BubbleCarMat;
    private Material Orange_DirtyCarMat;
    private float Orange_minShader;
    private float Orange_maxShader;
    private float Orange_randomfloat;
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
    private float Green_randomfloat;
    #endregion
    bool once = false;
    public GameObject FinishLine;

    public List<ParticleSystem> WaterParticles;
    public ParticleSystem BubbleParticle;
    public ParticleSystem BubbleParticle2;
    public ParticleSystem AirParticle;
    public ParticleSystem AirParticle2;

    public bool isClickedWashButton;
    public bool isClickedAirButton;
    public bool isClickedBubbleButton;
    public bool isClickedButton;

    public Transform target;
    public GameObject PerfectSign;
    public GameObject BadSign;
    public GameObject GoodSign;
    public Transform Spawner;
    public Slider slider;
    public Image FailImage;
    Animation FailAnim;

    public float smooth = 1;
    public float Speed = 2; //CurrrentSpeed
    public float maxSpeed = 3; // maximum Speed
    public float pSpeed = 0.25f; //perfectSpeed
    public float gSpeed = 0.1f; //badSpeed
    public float bSpeed = 0.2f; //goodSpeed

    private float signHigh;
    private float Maxdistance;
    public int AiScore;
    public int clickRandomButton;

    public bool isMoving = false;
    #endregion
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        #region A.I._Car_variables
        //RedCar
        Red_BubbleCarShader.z = 1.26f;
        Red_DirtyCarShader.z = 1.26f;
        Red_WaterCarShader.z = 1.26f;
        Red_maxShader = -0.82f;
        Red_minShader = 0.56f;

        //PinkCar
        Pink_BubbleCarShader.z = 3.85f;
        Pink_DirtyCarShader.z = 3.85f;
        Pink_WaterCarShader.z = 3.85f;
        Pink_maxShader = -2.5f;
        Pink_minShader = 2;

        //YellowCar
        Yellow_BubbleCarShader.z = 2.25f;
        Yellow_DirtyCarShader.z = 2.25f;
        Yellow_WaterCarShader.z = 2.25f;
        Yellow_maxShader = -1.75f;
        Yellow_minShader = 1.3f;

        //OrangeCar
        Orange_BubbleCarShader.z = 1.75f;
        Orange_DirtyCarShader.z = 1.75f;
        Orange_WaterCarShader.z = 1.75f;
        Orange_maxShader = 0.03f;
        Orange_minShader = 1.25f;

        //GreenCar
        Green_BubbleCarShader.z = 1.4f;
        Green_DirtyCarShader.z = 1.4f;
        Green_WaterCarShader.z = 1.4f;
        Green_maxShader = -0.9f;
        Green_minShader = 0.8f;
        #endregion
        //targetPos = FinishLine.transform.position;
        playerPos = transform.position;
        FailAnim = FailImage.gameObject.GetComponent<Animation>();
        signHigh = 2.5f;
    }

    float getDistance()
    {
        return Mathf.Abs( Vector2.Distance(this.transform.position, FinishLine.transform.position));
    }

    void SetProgress(float sliderDis)
    {
        slider.value = sliderDis;
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
            ParticleSystem();

            if (transform.position.x < Maxdistance && transform.position.x <= FinishLine.transform.position.x)
            {
                float distance = Maxdistance - getDistance();
                SetProgress(distance);
            }
        }
    }

    void PlayerMovement()
    {
        if (isMoving)
        {
            targetPos = FinishLine.transform.position;
            playerPos = transform.position;
            //transform.position = Vector3.MoveTowards(playerPos, Vector3.Lerp(playerPos, targetPos, smooth), Speed * Time.deltaTime);
            transform.position += Vector3.right * Speed * Time.deltaTime;
        }

    }

    void ParticleSystem()
    {
        if (isClickedWashButton)
        {
            WaterParticles[0].Play();
            WaterParticles[1].Play();
            WaterParticles[2].Play();
            WaterParticles[3].Play();
            WaterParticles[4].Play();
            WaterParticles[5].Play();
        }

        if (isClickedBubbleButton)
        {
            BubbleParticle.Play();
            BubbleParticle2.Play();
        }

        if (isClickedAirButton)
        {
            AirParticle.Play();
            AirParticle2.Play();
        }

        if (isClickedButton && !isClickedAirButton && !isClickedBubbleButton && !isClickedWashButton)
        {
            clickRandomButton = Random.Range(1,3);
            if (clickRandomButton == 1)
            {
                AirParticle.Play();
                AirParticle2.Play();
            }
            if (clickRandomButton == 2 )
            {
                BubbleParticle.Play();
                BubbleParticle2.Play();
            }
            if (clickRandomButton == 3)
            {
                WaterParticles[0].Play();
                WaterParticles[1].Play();
                WaterParticles[2].Play();
                WaterParticles[3].Play();
                WaterParticles[4].Play();
                WaterParticles[5].Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Random float value
        #region RED_CAR_AI_PROPERTIES
        if (other.gameObject.tag == "Red_BubbleCar")
        {
            Red_randomfloat = Random.Range(1f, 3f);
        }

        if (other.gameObject.tag == "Red_DirtyCar")
        {
            Red_randomfloat = Random.Range(1f, 3f);
        }

        if (other.gameObject.tag == "Red_WaterCar")
        {
            Red_randomfloat = Random.Range(1f, 3f);
        }
        #endregion

        #region PINK_CAR_AI_PROPERTIES
        if (other.gameObject.tag == "Pink_BubbleCar")
        {
            Pink_randomfloat = Random.Range(2f, 6.6f);
        }

        if (other.gameObject.tag == "Pink_DirtyCar")
        {
            Pink_randomfloat = Random.Range(5f, 6.6f);
        }

        if (other.gameObject.tag == "Pink_WaterCar")
        {
            Pink_randomfloat = Random.Range(5f, 6.6f);
        }
        #endregion

        #region YELLOW_CAR_AI_PROPERTIES
        if (other.gameObject.tag == "Yellow_BubbleCar")
        {
            Yellow_randomfloat = Random.Range(2f, 5f);
        }

        if (other.gameObject.tag == "Yellow_DirtyCar")
        {
            Yellow_randomfloat = Random.Range(2f, 5f);
        }

        if (other.gameObject.tag == "Yellow_WaterCar")
        {
            Yellow_randomfloat = Random.Range(2f, 5f);
        }
        #endregion

        #region ORANGE_CAR_AI_PROPERTIES
        if (other.gameObject.tag == "Orange_BubbleCar")
        {
            Orange_randomfloat = Random.Range(0.5f, 3f);
        }

        if (other.gameObject.tag == "Orange_DirtyCar")
        {
            Orange_randomfloat = Random.Range(0.5f, 3f);
        }

        if (other.gameObject.tag == "Orange_WaterCar")
        {
            Orange_randomfloat = Random.Range(0.5f, 3f);
        }
        #endregion

        #region GREEN_CAR_AI_PROPERTIES
        if (other.gameObject.tag == "Green_BubbleCar")
        {
            Green_randomfloat = Random.Range(0.5f, 2.5f);
        }

        if (other.gameObject.tag == "Green_DirtyCar")
        {
            Green_randomfloat = Random.Range(0.5f, 2.5f);
        }

        if (other.gameObject.tag == "Green_WaterCar")
        {
            Green_randomfloat = Random.Range(0.5f, 2.5f);
        }
        #endregion


        if (other.gameObject.tag == "Person")
        {
            Yellow_randomfloat = Random.Range(0f,0.3f);
        }

        if (other.gameObject.tag == "EnemyFinish" && !GameManager.isGameEnded)
        {
            FailAnim.Play();
            Debug.Log("AI: " + (AiScore) +  "Player: " + PlayerController.instance.Money);
            Debug.Log("AISpeed: " + Speed);
            Debug.Log("PlayerSpeed: " + PlayerController.instance.Speed);
            GameManager.instance.OnLevelFailed(); 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // using random float value
        #region RED_CAR_AI_PROPERTIES
        ///<Summary>
        ///eger BubbleCar ise 
        ///</Summary>>
        if (other.gameObject.tag == "Red_BubbleCar")
        {
            Red_BubbleCarMat = other.GetComponent<Renderer>().material;
            Red_BubbleCarShader.z -= Red_randomfloat * Time.deltaTime;
            isClickedWashButton = true;
            Red_BubbleCarMat.SetVector("_DissolveOffest", Red_BubbleCarShader);

        }
        ///<Summary>
        ///eger DirtyCar ise   
        ///</Summary>>
        if (other.gameObject.tag == "Red_DirtyCar" )
        {
            Debug.Log("active");
            Red_DirtyCarMat = other.GetComponent<Renderer>().material;
            isClickedBubbleButton = true;
            Red_DirtyCarShader.z -= Red_randomfloat * Time.deltaTime;
            Red_DirtyCarMat.SetVector("_DissolveOffest", Red_DirtyCarShader);
        }
        ///<Summary>
        ///eger WaterCar ise   
        ///</Summary>>
        if (other.gameObject.tag == "Red_WaterCar")
        {
            Red_WaterCarMat = other.GetComponent<Renderer>().material;
            Red_WaterCarShader.z -= Red_randomfloat * Time.deltaTime;
            isClickedAirButton = true;
            Red_WaterCarMat.SetVector("_DissolveOffest", Red_WaterCarShader);
        }
        #endregion

        #region PINK_CAR_AI_PROPERTIES

        if (other.gameObject.tag == "Pink_BubbleCar")
        {
            Pink_BubbleCarMat = other.GetComponent<Renderer>().material;
            Pink_BubbleCarShader.z -= Pink_randomfloat * Time.deltaTime;
            isClickedWashButton = true;
            Pink_BubbleCarMat.SetVector("_DissolveOffest", Pink_BubbleCarShader);

        }
        if (other.gameObject.tag == "Pink_DirtyCar")
        {
            Pink_DirtyCarMat = other.GetComponent<Renderer>().material;
            isClickedBubbleButton = true;
            Pink_DirtyCarShader.z -= Pink_randomfloat * Time.deltaTime;
            Pink_DirtyCarMat.SetVector("_DissolveOffest", Pink_DirtyCarShader);
        }
        if (other.gameObject.tag == "Pink_WaterCar")
        {   
            Pink_WaterCarMat = other.GetComponent<Renderer>().material;
            Pink_WaterCarShader.z -= Red_randomfloat * Time.deltaTime;
            isClickedAirButton = true;
            Pink_WaterCarMat.SetVector("_DissolveOffest", Pink_WaterCarShader);
        }
        #endregion

        #region YELLOW_CAR_AI_PROPERTIES

        if (other.gameObject.tag == "Yellow_BubbleCar")
        {
            Yellow_BubbleCarMat = other.GetComponent<Renderer>().material;
            Yellow_BubbleCarShader.z -= Time.deltaTime * Yellow_randomfloat;
            isClickedWashButton = true;
            Yellow_BubbleCarMat.SetVector("_DissolveOffest", Yellow_BubbleCarShader);
        }

        if (other.gameObject.tag == "Yellow_DirtyCar")
        {
            Yellow_DirtyCarMat = other.GetComponent<Renderer>().material;
            Yellow_DirtyCarShader.z -= Time.deltaTime * Yellow_randomfloat;
            isClickedBubbleButton = true;
            Yellow_DirtyCarMat.SetVector("_DissolveOffest", Yellow_DirtyCarShader);
        }

        if (other.gameObject.tag == "Yellow_WaterCar")
        {
            Yellow_WaterCarMat = other.GetComponent<Renderer>().material;
            Yellow_WaterCarShader.z -= Time.deltaTime * Yellow_randomfloat;
            isClickedAirButton = true;
            Yellow_WaterCarMat.SetVector("_DissolveOffest", Yellow_WaterCarShader);
        }
        #endregion

        #region ORANGE_CAR_AI_PROPERTIES

        if (other.gameObject.tag == "Orange_BubbleCar")
        {
            Orange_BubbleCarMat = other.GetComponent<Renderer>().material;
            Orange_BubbleCarShader.z -= Time.deltaTime * Orange_randomfloat;
            isClickedWashButton = true;
            Orange_BubbleCarMat.SetVector("_DissolveOffest", Orange_BubbleCarShader);
        }

        if (other.gameObject.tag == "Orange_DirtyCar")
        {
            Orange_DirtyCarMat = other.GetComponent<Renderer>().material;
            Orange_DirtyCarShader.z -= Time.deltaTime * Orange_randomfloat;
            isClickedBubbleButton = true;
            Orange_DirtyCarMat.SetVector("_DissolveOffest", Orange_DirtyCarShader);
        }

        if (other.gameObject.tag == "Orange_WaterCar")
        {
            Orange_WaterCarMat = other.GetComponent<Renderer>().material;
            Orange_WaterCarShader.z -= Time.deltaTime * Orange_randomfloat;
            isClickedAirButton = true;
            Orange_WaterCarMat.SetVector("_DissolveOffest", Orange_WaterCarShader);
        }
        #endregion

        #region GREEN_CAR_AI_PROPERTIES

        if (other.gameObject.tag == "Green_BubbleCar")
        {
            Green_BubbleCarMat = other.GetComponent<Renderer>().material;
            Green_BubbleCarShader.z -= Time.deltaTime * Green_randomfloat;
            isClickedWashButton = true;
            Green_BubbleCarMat.SetVector("_DissolveOffest", Green_BubbleCarShader);
        }

        if (other.gameObject.tag == "Green_DirtyCar")
        {
            Green_DirtyCarMat = other.GetComponent<Renderer>().material;
            Green_DirtyCarShader.z -= Time.deltaTime * Green_randomfloat;
            isClickedBubbleButton = true;
            Green_DirtyCarMat.SetVector("_DissolveOffest", Green_DirtyCarShader);
        }

        if (other.gameObject.tag == "Green_WaterCar")
        {
            Green_WaterCarMat = other.GetComponent<Renderer>().material;
            Green_WaterCarShader.z -= Time.deltaTime * Green_randomfloat;
            isClickedAirButton = true;
            Green_WaterCarMat.SetVector("_DissolveOffest", Green_WaterCarShader);
        }
        #endregion

        if (other.gameObject.tag == "Person")
        {
            PersonMat = other.GetComponent<Renderer>().material;
            PersonShader.z -= Red_randomfloat * Time.deltaTime;
            isClickedButton = true;
            PersonMat.SetVector("_DissolveOffest", PersonShader);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        #region Red_Car Properties
        if (other.gameObject.tag == "Red_WaterCar")
        {
            isClickedAirButton = false;
            RedCheckCleanWater();
        }

        if (other.gameObject.tag == "Red_DirtyCar")
        {
            isClickedBubbleButton = false;
            RedCheckCleanDirty();
        }

        if (other.gameObject.tag == "Red_BubbleCar")
        {
            isClickedWashButton = false;
            RedCheckCleanBubble();
            
        }
        #endregion

        #region Pink_Car Properties
        if (other.gameObject.tag == "Pink_WaterCar")
        {
            isClickedAirButton = false;
            PinkCheckCleanWater();
            
        }

        if (other.gameObject.tag == "Pink_DirtyCar")
        {
            isClickedBubbleButton = false;
            PinkCheckCleanDirty();
            
        }

        if (other.gameObject.tag == "Pink_BubbleCar")
        {
            isClickedWashButton = false;
            PinkCheckCleanBubble();
            
        }
        #endregion

        #region Yellow_Car Properties
        if (other.gameObject.tag == "Yellow_WaterCar")
        {
            isClickedAirButton = false;
            YellowCheckCleanWater();

        }

        if (other.gameObject.tag == "Yellow_DirtyCar")
        {
            isClickedBubbleButton = false;
            YellowCheckCleanDirty();

        }

        if (other.gameObject.tag == "Yellow_BubbleCar")
        {
            isClickedWashButton = false;
            YellowCheckCleanBubble();

        }
        #endregion

        #region Orange_Car Properties
        if (other.gameObject.tag == "Orange_WaterCar")
        {
            isClickedAirButton = false;
            OrangeCheckCleanWater();

        }

        if (other.gameObject.tag == "Orange_DirtyCar")
        {
            isClickedBubbleButton = false;
            OrangeCheckCleanDirty();

        }

        if (other.gameObject.tag == "Orange_BubbleCar")
        {
            isClickedWashButton = false;
            OrangeCheckCleanBubble();

        }
        #endregion

        #region Green_Car Properties
        if (other.gameObject.tag == "Green_WaterCar")
        {
            isClickedAirButton = false;
            GreenCheckCleanWater();

        }

        if (other.gameObject.tag == "Green_DirtyCar")
        {
            isClickedBubbleButton = false;
            GreenCheckCleanDirty();

        }

        if (other.gameObject.tag == "Green_BubbleCar")
        {
            isClickedWashButton = false;
            GreenCheckCleanBubble();

        }
        #endregion

        if (other.gameObject.tag == "Person")
        {
            isClickedButton = false;
            CheckCleanPerson();

        }
    }

    #region RED_CAR_AI_CHECKCLEANING
    void RedCheckCleanDirty()
    {
        ///<Smummary>
        ///DirtyCar Check
        ///</Smummary>>
        if (Red_DirtyCarShader.z < Red_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            AiScore += 5;
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
        }

        else if (Red_DirtyCarShader.z > Red_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            AiScore -= 2;
            Speed -= bSpeed;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            AiScore += 3;
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
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
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            AiScore += 5;
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
        }

        else if (Red_WaterCarShader.z > Red_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            AiScore -= 2;
            Speed -= bSpeed;

        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            AiScore += 3;
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
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
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            AiScore += 5;
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
        }

        else if (Red_BubbleCarShader.z > Red_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            AiScore -= 2;
            Speed -= bSpeed;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            AiScore += 3;
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
        }
    }
    #endregion

    #region PINK_CAR_A.I._CHECKING
    void PinkCheckCleanWater()
    {
        if (Pink_WaterCarShader.z < Pink_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            AiScore += 5;
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
        }

        else if (Pink_WaterCarShader.z > Pink_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            AiScore -= 2;
            Speed -= bSpeed;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            AiScore += 3;
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
        }

    }
    void PinkCheckCleanDirty()
    {
        if (Pink_DirtyCarShader.z < Pink_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            AiScore += 5;
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
        }

        else if (Pink_DirtyCarShader.z > Pink_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            AiScore -= 2;
            Speed -= bSpeed;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            AiScore += 3;
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
        }

    }

    void PinkCheckCleanBubble()
    {
        if (Pink_BubbleCarShader.z < Pink_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            AiScore += 5;
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
        }

        else if (Pink_BubbleCarShader.z > Pink_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            AiScore -= 2;
            Speed -= bSpeed;
        }

        else 
        {
            //Godd
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            AiScore += 3;
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
        }
    }
    #endregion

    #region YELLOW_CAR_CHECKING

    void YellowCheckCleanWater()
    {
        if (Yellow_WaterCarShader.z < Yellow_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Yellow_WaterCarShader.z > Yellow_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }

    }
    void YellowCheckCleanDirty()
    {
        if (Yellow_DirtyCarShader.z < Yellow_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Yellow_DirtyCarShader.z > Yellow_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }

    }

    void YellowCheckCleanBubble()
    {
        if (Yellow_BubbleCarShader.z < Yellow_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Yellow_BubbleCarShader.z > Yellow_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }
    }
    #endregion

    #region ORANGE_CAR_CHECKING

    void OrangeCheckCleanWater()
    {
        if (Orange_WaterCarShader.z < Orange_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Orange_WaterCarShader.z > Orange_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }

    }
    void OrangeCheckCleanDirty()
    {
        if (Orange_DirtyCarShader.z < Orange_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Orange_DirtyCarShader.z > Orange_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;

        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }

    }

    void OrangeCheckCleanBubble()
    {
        if (Orange_BubbleCarShader.z < Orange_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Orange_BubbleCarShader.z > Orange_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }
    }
    #endregion

    #region GREEN_CAR_CHECKING

    void GreenCheckCleanWater()
    {
        if (Green_WaterCarShader.z < Green_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Green_WaterCarShader.z > Green_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }

    }
    void GreenCheckCleanDirty()
    {
        if (Green_DirtyCarShader.z < Green_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Green_DirtyCarShader.z > Green_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;


        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }

    }

    void GreenCheckCleanBubble()
    {
        if (Green_BubbleCarShader.z < Green_maxShader)
        {
            //Perfect
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += pSpeed;
            }
            AiScore += 5;
        }

        else if (Green_BubbleCarShader.z > Green_minShader)
        {
            //Bad
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
            Speed -= bSpeed;
            AiScore -= 2;
        }

        else
        {
            //Good
            Instantiate(GoodSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), GoodSign.transform.rotation, Spawner);
            if (Speed <= maxSpeed)
            {
                Speed += gSpeed;
            }
            AiScore += 3;
        }
    }
    #endregion
    void CheckCleanPerson()
    {
        if (PersonShader.x < -0.3f)
        {
            AiScore -= 5;
            Instantiate(BadSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), BadSign.transform.rotation, Spawner);
        }

        else
        {
            AiScore += 5;
            Instantiate(PerfectSign, new Vector3(this.transform.position.x, transform.position.y + signHigh, transform.position.z), PerfectSign.transform.rotation, Spawner);
        }
    }
}
