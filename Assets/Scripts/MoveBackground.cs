using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField, Header("�w�i�摜�̎Q��")]
    private GameObject[] bgObject;

    [SerializeField, Header("�J�����̎Q��")]
    private Camera mainCamera;

    private float bgSkySpeed = 23f;          // �w�i�̋�̃X�s�[�h
    private float bgCloudSpeed = 25f;        // �w�i�̉_�̃X�s�[�h
    private float bgBackMountain = 30f;     // �w�i�̗��̎R�̃X�s�[�h
    private float bgFrontMountain = 35f;    // �w�i�̕\�̎R�̃X�s�[�h

    private float loopWidth = 34f;          // ���[�v�����镝

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

            for (int i = 0; i < bgObject.Length; i++)
            {
                float speed = GetSpeed(i);
                bgObject[i].transform.position = CalcPosition(moveDirection, bgObject[i], speed);
            }
        }
    }

    private float GetSpeed(int index)
    {
        return index switch
        {
            0 => bgSkySpeed,
            1 => bgCloudSpeed,
            2 => bgBackMountain,
            3 => bgFrontMountain,
            _ => 0f,
        };
    }

    private Vector3 CalcPosition(Vector3 moveDirection, GameObject gameObject, float speed)
    {
        Vector3 newPosition = gameObject.transform.position + moveDirection * speed * Time.deltaTime;

        // �J������X���W����ɃI�t�Z�b�g���v�Z
        float cameraX = mainCamera.transform.position.x;
        float objectX = newPosition.x;

        if (objectX < cameraX - loopWidth / 2)
        {
            newPosition.x += loopWidth;
        }
        else if (objectX > cameraX + loopWidth / 2)
        {
            newPosition.x -= loopWidth;
        }

        return newPosition;
    }
}
