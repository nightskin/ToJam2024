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
    [SerializeField] RectTransform crossHair;
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
        Steer();
        Move();

        if(actions.Shoot.IsPressed())
        {
            shootTimer -= Time.deltaTime;
            if(shootTimer < 0)
            {
                Shoot();
            }
        }
        if(GetComponent<HealthScript>().IsDead())
        {
            Cursor.lockState = CursorLockMode.None;
            mesh.gameObject.SetActive(false);
            controller.enabled = false;
            hud.SetActive(false);
            gameOverMenu.SetActive(true);
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
        if (camera.transform.GetComponent<PlayerCamera>().camDistance > 0)
        {
            Ray ray = camera.ScreenPointToRay(crossHair.position);
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().owner = this.gameObject;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                bullet.GetComponent<BulletScript>().direction = (hit.point - bulletSpawn.position).normalized;
            }
            else
            {
                bullet.GetComponent<BulletScript>().direction = ray.direction;
            }
            shootTimer = fireRate;
        }
        else
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().owner = this.gameObject;
            bullet.GetComponent<BulletScript>().direction = bulletSpawn.forward;
            shootTimer = fireRate;
        }

    }

    void Steer()
    {
        if (!GetComponent<HealthScript>().IsDead())
        {
            float x = actions.Look.ReadValue<Vector2>().x;
            float y = actions.Look.ReadValue<Vector2>().y;
            //MouseLook
            xRot += y * lookSpeed * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, -90, 90);
            yRot += x * lookSpeed * Time.deltaTime;
            zRot = Mathf.Lerp(zRot, -x * 45, 10 * Time.deltaTime);
            camera.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
            transform.rotation = camera.transform.localRotation;
        }

    }

    void Move()
    {
        if(!GetComponent<HealthScript>().IsDead())
        {
            controller.Move(camera.transform.forward * speed * Time.deltaTime);
        }
    }

}
