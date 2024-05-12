using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Controls controls;
    Controls.PlayerActions actions;
    [SerializeField] GameObject hud;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;

    [SerializeField] Transform mesh;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Camera camera;
    [SerializeField] Image crossHair;
    [SerializeField] CharacterController controller;
    [SerializeField] ParticleSystem speedLines;

    [SerializeField] float lookSpeed = 100;
    float xRot = 0;
    float yRot = 0;
    float zRot = 0;

    float speed;
    [SerializeField] float walkSpeed = 100;
    [SerializeField] float runSpeed = 200;

    [SerializeField] float fireRate = 0.2f;
    float shootTimer = 0;
    RaycastHit lockOn;
    [SerializeField] LayerMask lockOnLayer;

    void Awake()
    {
        controls = new Controls();
        actions = controls.Player;
        actions.Enable();
        actions.Shoot.performed += Shoot_performed;
        actions.Run.performed += Run_performed;
        actions.Run.canceled += Run_canceled;
        actions.Zoom.performed += Zoom_performed;
        actions.Pause.performed += Pause_performed;
        
        speed = walkSpeed;
        if(!controller) controller = GetComponent<CharacterController>();
        
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        actions.Shoot.performed -= Shoot_performed;
        actions.Run.performed -= Run_performed;
        actions.Run.canceled -= Run_canceled;
        actions.Zoom.performed -= Zoom_performed;
        actions.Pause.performed -= Pause_performed;
    }

    void Update()
    {
        if(GetComponent<HealthScript>().IsDead())
        {
            Cursor.lockState = CursorLockMode.None;
            mesh.gameObject.SetActive(false);
            controller.enabled = false;
            hud.SetActive(false);
            gameOverMenu.SetActive(true);
        }
        else
        {
            Steer();
            Move();
            if (actions.Shoot.IsPressed())
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer < 0)
                {
                    Shoot();
                }
            }
        }
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!GetComponent<HealthScript>().IsDead())
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                hud.SetActive(true);
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                hud.SetActive(false);
                pauseMenu.SetActive(true);
            }
        }
    }

    private void Zoom_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(camera.transform.GetComponent<PlayerCamera>().camDistance > 0)
        {
            mesh.GetComponent<MeshRenderer>().enabled = false;
            camera.transform.GetComponent<PlayerCamera>().camDistance = 0;
            camera.transform.GetComponent<PlayerCamera>().offset = new Vector3(0, 0.5f, 0);
        }
        else if(camera.transform.GetComponent<PlayerCamera>().camDistance == 0)
        {
            mesh.GetComponent<MeshRenderer>().enabled = true;
            camera.transform.GetComponent<PlayerCamera>().camDistance = 10;
            camera.transform.GetComponent<PlayerCamera>().offset = new Vector3(0, 4, 0);
        }

    }
    
    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Shoot();
    }

    private void Run_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        speedLines.Play();
        speed = runSpeed;
    }

    private void Run_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        speedLines.Stop();
        speed = walkSpeed;
    }

    void Shoot()
    {
        if (crossHair.color == Color.red)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().owner = this.gameObject;
            bullet.GetComponent<BulletScript>().homingTarget = lockOn.transform;
            shootTimer = fireRate;
        }
        else
        {
            Ray ray = camera.ScreenPointToRay(crossHair.rectTransform.position);
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().owner = this.gameObject;
            bullet.GetComponent<BulletScript>().direction = ray.direction;
            shootTimer = fireRate;
        }

    }

    void Steer()
    {
        float x = actions.Look.ReadValue<Vector2>().x;
        float y = actions.Look.ReadValue<Vector2>().y;
        //MouseLook
        xRot += y * lookSpeed * Time.deltaTime;
        xRot = Mathf.Clamp(xRot, -90, 90);
        yRot += x * lookSpeed * Time.deltaTime;
        zRot = Mathf.Lerp(zRot, -x * 45, 10 * Time.deltaTime);
        camera.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        transform.rotation = Quaternion.Euler(xRot, yRot, zRot);

        Ray ray = camera.ScreenPointToRay(crossHair.rectTransform.position);
        if (Physics.SphereCast(ray, 4, out lockOn, camera.farClipPlane ,lockOnLayer))
        {
            crossHair.color = Color.red;
        }
        else
        {
            crossHair.color = Color.white;
        }

    }

    void Move()
    {
        controller.Move(camera.transform.forward * speed * Time.deltaTime);

    }

}
