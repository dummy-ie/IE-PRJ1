using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    [SerializeField] private bool _scrollLeft;

    [SerializeField] private bool _followPlayer;

    private float _singleTextureWidth;
    private GameObject _player;

    void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        _singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Scroll()
    {
        float delta = _moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta, 0f, 0f);
    }

    void FollowPlayer()
    {
        transform.position = new Vector3(_player.transform.position.x * _moveSpeed / 10, 0f, 0f);
    }

    void CheckReset()
    {
        if ((Mathf.Abs(transform.position.x) - _singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        SetupTexture();
        if (_scrollLeft)
            _moveSpeed = -_moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null && _followPlayer)
        {
            FollowPlayer();
        }
        else
            Scroll();
        CheckReset();
    }
}
