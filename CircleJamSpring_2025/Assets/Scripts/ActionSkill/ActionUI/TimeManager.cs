using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageRotator : MonoBehaviour
{
    [SerializeField] private Image targetImage; // 回転させるImage
    [SerializeField] private float fromAngle = 130f;  // 開始角度
    [SerializeField] private float toAngle = 20f; // 終了角度
    [SerializeField] private float duration = 60f;  // 回転にかける時間（秒）

    private void Start()
    {
        // コルーチンを開始して回転させる
        StartCoroutine(RotateImage());
    }

    private IEnumerator RotateImage()
    {
        float elapsedTime = 0f; // 経過時間をカウント

        while (elapsedTime < duration)
        {
            // 0.0 ~ 1.0 の値を求める
            float t = elapsedTime / duration;

            // 角度をLerpで補間
            float angle = Mathf.Lerp(fromAngle, toAngle, t);

            // 回転を適用
            targetImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);

            Debug.Log($"経過時間: {elapsedTime:F2}秒 / {duration}秒, 現在の角度: {angle:F1}°");

            elapsedTime += Time.deltaTime; // 経過時間を更新
            yield return null; // 次のフレームまで待機
        }

        // 最終角度を正しく設定
        targetImage.rectTransform.rotation = Quaternion.Euler(0, 0, toAngle);
        Debug.Log("回転完了！");
    }
}

