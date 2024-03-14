using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float _moveSpeedX;
    [SerializeField] private float _moveSpeedY = 1;
    [SerializeField] private bool _scrollLeft;
    [SerializeField] private bool _followPlayer;

    private float _singleTextureWidth;
    private GameObject _player;
    private ICinemachineCamera _virtualCamera;

    void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        _singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Scroll()
    {
        float delta = _moveSpeedX * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        transform.position += new Vector3(delta, 0f, 0f);
    }

    void FollowPlayer()
    {
        transform.position = new Vector3(Camera.main.transform.position.x + _player.transform.position.x * _moveSpeedX / 10, Camera.main.transform.position.y, 0f);
    }

    void CheckReset()
    {
        if ((Mathf.Abs(transform.position.x) - _singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupTexture();
        if (_scrollLeft)
            _moveSpeedX = -_moveSpeedX;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        if (_player != null && _followPlayer)
        {
            FollowPlayer();
        }
        else
            Scroll();
        CheckReset();*/
        if (!_followPlayer)
            Scroll();
        //CheckRest
    }

    public void MoveX(float delta)
    {
        if (_followPlayer)
        {
            Debug.Log("moving X");
            Vector3 newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            newPos.x -= delta * _moveSpeedX / 10;

            transform.localPosition = newPos;
        }
    }

    public void MoveY(float delta)
    {
        if (_followPlayer)
        {
            Debug.Log("moving Y");
            Vector3 newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            newPos.y -= delta * _moveSpeedY / 10;

            transform.localPosition = newPos;
        }
    }
}
