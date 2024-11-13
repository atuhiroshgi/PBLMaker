using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("Player")]
    private Player player;
    [SerializeField, Header("�v���C���[�̈ʒu")]
    private Transform playerTransform;
    [SerializeField, Header("�J�������ړ����n�߂�x���͈̔�")]
    private float xOffsetThreshold = 2f;
    [SerializeField, Header("�J�������ړ����n�߂�y���͈̔�")]
    private float yOffsetThreshold = 1.5f;
    [SerializeField, Header("�J�����̈ړ����x")]
    private float cameraSpeed = 20f;
    [SerializeField, Header("�J������x���̍ŏ��l")]
    private float minX = 0f;
    [SerializeField, Header("�J������x���̍ő�l")]
    private float maxX = 120f;
    [SerializeField, Header("�J������y���̍ŏ��l")]
    private float minY = 0f;
    [SerializeField, Header("�J������y���̍ő�l")]
    private float maxY = 35f;

    private Vector3 initialCameraPosition;       // �J�����̏����ʒu
    private Vector3 startPosition;                  // ���s�J�n���̃J�����̈ʒu
    private float cameraFollowSpeed;                // �J�������v���C���[�𒍎�����Ƃ��̒ǔ����x

    private void Awake()
    {
        // �J�����̏����ʒu���擾
        initialCameraPosition = transform.position;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            FollowPlayer();
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ���͂�����΃J�������ړ�
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // �J�����̐V�����ʒu���v�Z
            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
            Vector3 newPosition = this.transform.position + moveDirection * cameraSpeed * Time.deltaTime;

            // X����Y���̈ړ��͈͂𐧌�
            if (newPosition.x < minX) newPosition.x = minX;
            if (newPosition.x > maxX) newPosition.x = maxX;
            if (newPosition.y < minY) newPosition.y = minY;
            if (newPosition.y > maxY) newPosition.y = maxY;

            // �J�������ړ�������
            this.transform.position = newPosition;
        }
    }

    /// <summary>
    /// �v���C���[��Ǐ]����
    /// </summary>
    /// <returns></returns>
    private void FollowPlayer()
    {
        if (!ExecuteManager.Instance.GetIsExecute()) return;

        // �v���C���[�ƃJ�����̑��Έʒu
        Vector3 playerOffsetFromCamera = playerTransform.position - transform.position;

        // �J�������ړ����J�n���邩�̔���
        float newCameraPosX = transform.position.x;
        float newCameraPosY = transform.position.y;

        cameraFollowSpeed = player.GetPlayerSpeed();

        // X�������̏���
        if (Mathf.Abs(playerOffsetFromCamera.x) > xOffsetThreshold)
        {
            newCameraPosX = playerTransform.position.x - (Mathf.Sign(playerOffsetFromCamera.x) * xOffsetThreshold);
        }

        // Y�������̏���
        if (Mathf.Abs(playerOffsetFromCamera.y) > yOffsetThreshold)
        {
            newCameraPosY = playerTransform.position.y - (Mathf.Sign(playerOffsetFromCamera.y) * yOffsetThreshold);
        }

        // X����Y���̈ړ��͈͂𐧌�
        newCameraPosX = Mathf.Clamp(newCameraPosX, minX, maxX);
        newCameraPosY = Mathf.Clamp(newCameraPosY, minY, maxY);

        // �J�����̐V�����ʒu�ɕ�Ԃ��Ĉړ�
        Vector3 newCameraPosition = new Vector3(newCameraPosX, newCameraPosY, initialCameraPosition.z);
        transform.position = Vector3.Lerp(transform.position, newCameraPosition, cameraFollowSpeed * Time.deltaTime);
    }

    /// <summary>
    /// ���s���J�n���ꂽ�Ƃ��ɃJ�����̈ʒu���L��
    /// </summary>
    public void RememberStartPosition()
    {
        startPosition = transform.position;
    }

    /// <summary>
    /// ���s�������_�ł̃J�����̈ʒu�ɖ߂�
    /// </summary>
    public void MoveToStartPosition()
    {
        this.transform.position = startPosition;
    }
}
