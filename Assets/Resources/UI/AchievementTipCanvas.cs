using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AchievementTipCanvas : Singleton<AchievementTipCanvas>
{
    public GameObject unlockTip;
    public Image icon;
    public Image ac_name;

    public List<Sprite> icons;
    public List<Sprite> ac_names;

    public void ShowMe(Achievement_SO achievement_SO)
    {
        this.gameObject.SetActive(true); //���ܿ�ʼЭ��
        StartCoroutine(PopThePanel(achievement_SO));
    }

    public IEnumerator PopThePanel(Achievement_SO achievement_SO)
    {
        Vector3 lastpos = unlockTip.transform.position;

        //�ɼ�
        unlockTip.transform.parent.gameObject.SetActive(true);
        unlockTip.SetActive(true);

        //unlockTip.GetComponentInChildren<Text>().text = "�����ɾ�:" + achievement_SO.acName.ToString();
        icon.sprite = icons[achievement_SO.acIndex];
        ac_name.sprite = ac_names[achievement_SO.acIndex];
        icon.SetNativeSize();
        ac_name.SetNativeSize();

        float percent = 0f;
        float amount = 365f;
        while (percent < 1)
        {
            percent += Time.deltaTime / 1f;
            unlockTip.transform.position += Vector3.down * amount * Time.deltaTime / 1f;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / 1f;
            AchievementTipCanvas.Instance.transform.position += Vector3.up * amount * Time.deltaTime / 1f;
            yield return null;
        }

        //���ɼ�
        unlockTip.SetActive(false);
        unlockTip.transform.parent.gameObject.SetActive(false);

        unlockTip.transform.position = lastpos;
        Debug.Log("���PopThePanel");
    }

    //test
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        Debug.Log("GetKeyDown(KeyCode.Tab)");
    //        GameManager_global.GetInstance().gameData_SO.ach_4_Firstcake.UpdateMe(1);
    //    }
    //}
}
