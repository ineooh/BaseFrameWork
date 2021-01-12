using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public delegate void Callback();
public delegate void CallbackBooleanParam(bool result);

//namespace UTILS
//{
    public enum TIME_FORMAT
    {
        FULL,//00:00:00
        FULL_WITH_STRING,//00h:00m:00
        SHORT,//00:00
        SHORT_WITH_STRING,//00m:00s
        SHORT_WITH_FULL_STRING,//00 minute : 00 second
        DAY,
        IF_SHORT_THEN_MORE_DETAILS,// 1 day, 0 day -> 23h
        DAY_HOUR_THEN_MORE_DETAILS,// 1 day 23h -> 23:59:59
        NONE
    };
    public enum TIMER_TYPE
    {
        COUNT_DOWN,
        ELAPSE,
        NONE
    };



public class MyUtils : SingletonMono<MyUtils>
{
    public const int PERCENT_BASE_NUM = 10000;
    public const int SECONDS_DAY = 86400;
#if !BUILD_LIVE
    public static bool isDebugBuild = false;
#else
    public static bool isDebugBuild = false;
#endif

    static public float normalAngle(float getAngle)
    {
        float tmpA = getAngle % 360;
        if (tmpA < 0)
        {
            tmpA += 360;
        }
        return tmpA;
    }

	static public float normalAngle2(float getAngle)
	{
		float tmpA = getAngle % 360;
		if (getAngle > 180)
		{
			tmpA = getAngle - 360;
		}
		return tmpA;
	}
	static public Vector3 normalEuler(Vector3 getAngle)
	{
		getAngle.x = MyUtils.normalAngle2(getAngle.x);
		getAngle.y = MyUtils.normalAngle2(getAngle.y);
		getAngle.z = MyUtils.normalAngle2(getAngle.z);
		return getAngle;
	}

	static public void debug(string message, bool force = false)
    {
        if (force || isDebugBuild)
        {
            Debug.Log(message);
        }
    }

    static public void debugWarning(string message, bool force = false)
    {
        if (force || isDebugBuild)
        {
            Debug.LogWarning(message);
        }
    }

    static public void debugError(string message, bool force = false)
    {
        if (force || isDebugBuild)
        {
            Debug.LogError(message);
        }
    }
    public static bool IsPointInPolygon(Vector3[] polygon, Vector3 testPoint)
    {
        bool result = false;
        int j = polygon.Length - 1;
        for (int i = 0; i < polygon.Length; i++)
        {
            if (polygon[i].y < testPoint.y && polygon[j].y >= testPoint.y || polygon[j].y < testPoint.y && polygon[i].y >= testPoint.y)
            {
                if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < testPoint.x)
                {
                    result = !result;
                }
            }
            j = i;
        }
        return result;
    }

    public static bool IsInPolygon(Vector3[] poly, Vector3 p)
    {
        Vector3 p1, p2;


        bool inside = false;


        if (poly.Length < 3)
        {
            return inside;
        }


        var oldVector3 = new Vector3(
            poly[poly.Length - 1].x, poly[poly.Length - 1].z);


        for (int i = 0; i < poly.Length; i++)
        {
            var newVector3 = new Vector3(poly[i].x, poly[i].z);


            if (newVector3.x > oldVector3.x)
            {
                p1 = oldVector3;

                p2 = newVector3;
            }

            else
            {
                p1 = newVector3;

                p2 = oldVector3;
            }


            if ((newVector3.x < p.x) == (p.x <= oldVector3.x)
                && (p.z - (long)p1.z) * (p2.x - p1.x)
                < (p2.z - (long)p1.z) * (p.x - p1.x))
            {
                inside = !inside;
            }


            oldVector3 = newVector3;
        }


        return inside;
    }


    public static void ResizeSpriteToScreen(GameObject theSprite, Camera theCamera, int fitToScreenWidth, int fitToScreenHeight)
    {
        SpriteRenderer sr = theSprite.GetComponent<SpriteRenderer>();

        theSprite.transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = (float)(theCamera.orthographicSize * 2.0);
        float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);

        if (fitToScreenWidth != 0)
        {
            Vector2 sizeX = new Vector2(worldScreenWidth / width / fitToScreenWidth, theSprite.transform.localScale.y);
            theSprite.transform.localScale = sizeX;
        }

        if (fitToScreenHeight != 0)
        {
            Vector2 sizeY = new Vector2(theSprite.transform.localScale.x, worldScreenHeight / height / fitToScreenHeight);
            theSprite.transform.localScale = sizeY;
        }
    }

    static public string getTwoDigits(int value)
    {
        if (value == 0)
        {
            return "00";
        }
        else if (value < 10)
        {
            return "0" + value;
        }
        else
        {
            return value + "";
        }
    }
    static public string getTwoDigits(long value)
    {
        if (value == 0)
        {
            return "00";
        }
        else if (value < 10)
        {
            return "0" + value;
        }
        else
        {
            return value + "";
        }
    }

    static public string ConvertSecondToStringFull(int time)
    {
        int second = time % 60;
        int minute = ((time % 3600) / 60);
        int hour = time / 3600;

        string timeFull = "";

        //if (Languages.s_currentLang == Languages.Lang.EN)
        if (true)
        {
            if (hour > 0)
                timeFull += hour.ToString() + (hour > 1 ? " hours " : " hour ");
            if (minute > 0)
                timeFull += minute.ToString() + (minute > 1 ? " mins " : " min ");
            if (second > 0)
                timeFull += second.ToString() + (second > 1 ? " secs " : " sec");
        }
        else
        {
            if (hour > 0)
                timeFull += hour.ToString() + " giờ ";
            if (minute > 0)
                timeFull += minute.ToString() + " phút ";
            if (second > 0)
                timeFull += second.ToString() + " giây ";
        }
        return timeFull;
        //}
    }

    static public string ConvertSecondToStringFull(long time)
    {
        long second = time % 60;
        long minute = ((time % 3600) / 60);
        long hour = time / 3600;

        string timeFull = "";

        //if (Languages.s_currentLang == Languages.Lang.EN)
        if (true)
        {
            if (hour > 0)
                timeFull += hour.ToString() + (hour > 1 ? " hours " : " hour ");
            if (minute > 0)
                timeFull += minute.ToString() + (minute > 1 ? " mins " : " min ");
            if (second > 0)
                timeFull += second.ToString() + (second > 1 ? " secs " : " sec");
        }
        else
        {
            if (hour > 0)
                timeFull += hour.ToString() + " giờ ";
            if (minute > 0)
                timeFull += minute.ToString() + " phút ";
            if (second > 0)
                timeFull += second.ToString() + " giây ";
        }
        return timeFull;
        //}
    }

    static public string ConvertSecondToStringShort(int time)
    {
        int second = time % 60;
        int minute = ((time % 3600) / 60);
        int hour = time / 3600;

        string timeFull = "";

        int count = 0;
        //if (Languages.s_currentLang == Languages.Lang.EN)
        {
            if (hour > 0)
            {
                timeFull += hour.ToString() + "H ";
                count++;
            }
            if (minute > 0)
            {
                timeFull += minute.ToString() + "M ";
                count++;
            }
            if (second > 0 && count <= 1)
            {
                timeFull += second.ToString() + "S";
            }
        }
        //else
        //{
        //    if (hour > 0)
        //    {
        //        timeFull += hour.ToString() + "G ";
        //        //count++;
        //    }
        //    if (minute > 0)
        //    {
        //        timeFull += minute.ToString() + "P ";
        //        //count++;
        //    }
        //    if (second > 0 && count <= 1)
        //    {
        //        timeFull += second.ToString() + " ";
        //        count++;
        //    }
        //}
        return timeFull;
        //}
    }

    public static string ConvertSecondToString(int time, TIME_FORMAT type = TIME_FORMAT.FULL)
    {
        long second = (long)(time % 60);
        long minute = (long)((time % 3600) / 60);
        long hour = (long)time / 3600;
        if (type == TIME_FORMAT.DAY)
            return Mathf.CeilToInt((float)hour / 24f).ToString();
        else if (type == TIME_FORMAT.IF_SHORT_THEN_MORE_DETAILS)
        {
            if (hour < 24)
                return ConvertSecondToString(time, TIME_FORMAT.SHORT_WITH_FULL_STRING);
            else
            {
                return Mathf.CeilToInt((float)hour / 24f).ToString() + " " + "day";
            }
        }
        else if (type == TIME_FORMAT.DAY_HOUR_THEN_MORE_DETAILS)
        {
            if (hour < 24)
                return ConvertSecondToString(time);
            else
            {
                int day = Mathf.FloorToInt((float)hour / 24f);
                hour = hour % 24;

                return day + " " + (day > 1 ? "days" : "day") + " " + hour + " " + (hour > 1 ? "hours" : "hour");
            }
        }
        else if (type == TIME_FORMAT.SHORT)
            return (getTwoDigits(minute + hour * 60) + ":" + getTwoDigits(second));
        else if (type == TIME_FORMAT.SHORT_WITH_STRING)
            return (getTwoDigits(minute + hour * 60) + " " + "min_short" + " " + getTwoDigits(second) + " " + "sec_short");
        else if (type == TIME_FORMAT.SHORT_WITH_FULL_STRING)
            return (getTwoDigits(minute + hour * 60) + " " + "min" + " " + getTwoDigits(second) + " " + "sec");
        else
            return (getTwoDigits(hour) + ":" + getTwoDigits(minute) + ":" + getTwoDigits(second));
    }
    static public string ConvertTimeToStringDay(int time)
    {
        // 7 ngay 00:00:00
        int second = time % 60;
        int minute = ((time % 3600) / 60);
        int hour = time / 3600;
        int day = hour / 24;
        hour = hour % 24;
        if (day > 0)
            return day + " days" + " " + (getTwoDigits(hour) + ":" + getTwoDigits(minute) + ":" + getTwoDigits(second));
        else
            return (getTwoDigits(hour) + ":" + getTwoDigits(minute) + ":" + getTwoDigits(second));
    }

    static public Vector3 getAnchor(Sprite sprite)
    {
        if (sprite == null)
            return Vector3.zero;
        Bounds bounds = sprite.bounds;
        float pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
        float pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;
        float pixelsToUnitsX = sprite.textureRect.width * (0.5f - pivotX);
        float pixelsToUnitsY = sprite.textureRect.height * (0.5f - pivotY);
        return new Vector3(pixelsToUnitsX, pixelsToUnitsY, 1f);
    }

    static public Vector3 WorldToUI(float x, float y, float z)
    {
        return new Vector3(x * 100f, y * 100f, z * 100f);
    }
    static public Vector3 UIToWorld(float x, float y, float z)
    {
        return new Vector3(x / 100f, y / 100f, z / 100f);
    }

    static Color colorDark;
    static public void MakeImageColorDark(Image image)
    {
        if (colorDark == null)
        {
            colorDark = new Color(.6f, .6f, .6f, 1f);
        }
        image.CrossFadeColor(new Color(.6f, .6f, .6f, 1f), .7f, false, false);
        Image[] childs = image.transform.GetComponentsInChildren<Image>();
        foreach (Image img in childs)
        {
            img.CrossFadeColor(new Color(.6f, .6f, .6f, 1f), .7f, false, false);
        }
    }

    static public void MakeImageColorNormal(Image image)
    {
        image.CrossFadeColor(new Color(1, 1, 1, 1), .7f, false, false);
        Image[] childs = image.transform.GetComponentsInChildren<Image>();
        foreach (Image img in childs)
        {
            img.CrossFadeColor(new Color(1, 1, 1, 1), .7f, false, false);
        }
    }



    public static string convertNumberToStringWithCommas(long value)
    {
        if (value < 1000) return value.ToString();
        string str = "";
        //var sofuck:Int = 0 ;
        while (true)
        {
            long sofuck = (value % 1000);
            //str =  + str;
            value = (long)(value / 1000);

            if (value > 0)
            {
                if (sofuck == 0)
                    str = "000" + str;
                else if (sofuck < 10)
                    str = "00" + sofuck + str;
                else if (sofuck < 100)
                    str = "0" + sofuck + str;
                else
                    str = sofuck + str;

            }
            else
            {
                str = sofuck + str;
            }

            if (value > 0)
                str = "," + str;
            else
                break;
        }
        return str;
    }

    public static string convertNumberToStringWithCommas(uint value)
    {
        if (value < 1000) return value.ToString();
        string str = "";
        //var sofuck:Int = 0 ;
        while (true)
        {
            long sofuck = (value % 1000);
            //str =  + str;
            value = (uint)(value / 1000);

            if (value > 0)
            {
                if (sofuck == 0)
                    str = "000" + str;
                else if (sofuck < 10)
                    str = "00" + sofuck + str;
                else if (sofuck < 100)
                    str = "0" + sofuck + str;
                else
                    str = sofuck + str;

            }
            else
            {
                str = sofuck + str;
            }

            if (value > 0)
                str = "," + str;
            else
                break;
        }
        return str;
    }

    public static float levelGold(int gold)
    {

        float number = 1.0f;

        while (gold > 0)
        {
            gold /= 10;
            if (gold > 0) number += 0.4f;
        }


        return number;

    }

    public static void cleanChild(Transform transform)
    {
        Transform[] childs = transform.GetComponentsInChildren<Transform>();
        foreach (Transform trans in childs)
        {
            if (trans != transform)
                Destroy(trans.gameObject);
        }
    }


    public static bool LineIntersectsRect(Vector2 p1, Vector2 p2, Rect r)
    {
        return LineIntersectsLine(p1, p2, new Vector2(r.xMin, r.yMin), new Vector2(r.xMax, r.yMin)) ||
               LineIntersectsLine(p1, p2, new Vector2(r.xMax, r.yMin), new Vector2(r.xMax, r.yMax)) ||
               LineIntersectsLine(p1, p2, new Vector2(r.xMax, r.yMax), new Vector2(r.xMin, r.yMax)) ||
               LineIntersectsLine(p1, p2, new Vector2(r.xMin, r.yMax), new Vector2(r.xMin, r.yMin)) ||
               (r.Contains(p1) && r.Contains(p2));
    }

    private static bool LineIntersectsLine(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
    {
        float q = (l1p1.y - l2p1.y) * (l2p2.x - l2p1.x) - (l1p1.x - l2p1.x) * (l2p2.y - l2p1.y);
        float d = (l1p2.x - l1p1.x) * (l2p2.y - l2p1.y) - (l1p2.y - l1p1.y) * (l2p2.x - l2p1.x);

        if (d == 0)
        {
            return false;
        }

        float r = q / d;

        q = (l1p1.y - l2p1.y) * (l1p2.x - l1p1.x) - (l1p1.x - l2p1.x) * (l1p2.y - l1p1.y);
        float s = q / d;

        if (r < 0 || r > 1 || s < 0 || s > 1)
        {
            return false;
        }

        return true;
    }

    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        MyUtils.debug("origin " + origin.ToString());
        MyUtils.debug("date.ToUniversalTime() " + date.ToUniversalTime().ToString());
        TimeSpan diff = date.ToUniversalTime() - origin;
        MyUtils.debug("diff " + Math.Floor(diff.TotalSeconds));
        return Math.Floor(diff.TotalSeconds);
    }

    static public long GetHarvestGainGold(int UserLevel, int m_secGrowTotal)
    {
        double G;
        double T;
        double P1;
        double P2;
        double V;
        G = m_secGrowTotal / 60;
        T = 60 / G;
        P1 = (T - 1);
        P2 = (double)UserLevel / 100 + 1.2d;
        return (long)Math.Floor(((G * P2 + P1 * 15) * 0.35d) + 0.5f);

    }

    static public long GetHarvestGainExp(int UserLevel, int m_secGrowTotal)
    {
        double G;
        double T;
        double P1;
        double P2;
        double V;
        G = m_secGrowTotal / 60;
        T = 60 / G;
        P1 = (T - 1);
        P2 = (double)UserLevel / 100 + 1.2d;
        return (long)Math.Floor(((G * P2 + (T - 1) * 15) * 0.6d) + 0.5f);


    }

    static public String GetNameStringEng(int month)
    {
        string name = string.Empty;
        switch (month)
        {
            case 1:
                name = "January"; break;
            case 2:
                name = "February"; break;
            case 3:
                name = "March"; break;
            case 4:
                name = "April"; break;
            case 5:
                name = "May"; break;
            case 6:
                name = "June"; break;
            case 7:
                name = "July"; break;
            case 8:
                name = "August"; break;
            case 9:
                name = "September"; break;
            case 10:
                name = "October"; break;
            case 11:
                name = "November"; break;
            case 12:
                name = "December"; break;
        }
        return name;
    }

    public static string convertNumberToStringWithCommas(int value)
    {
        if (value < 1000) return value.ToString();
        string str = "";
        string decimalSplit = ".";
        //if (Languages.s_currentLang == Languages.Lang.EN)
        //{
        //    decimalSplit = ".";
        //}
        //var sofuck:Int = 0 ;
        while (true)
        {
            int sofuck = (value % 1000);
            //str =  + str;
            value = (int)(value / 1000);

            if (value > 0)
            {
                if (sofuck == 0)
                    str = "000" + str;
                else if (sofuck < 10)
                    str = "00" + sofuck + str;
                else if (sofuck < 100)
                    str = "0" + sofuck + str;
                else
                    str = sofuck + str;

            }
            else
            {
                str = sofuck + str;
            }

            if (value > 0)
                str = decimalSplit + str;
            else
                break;
        }
        return str;
    }

    public static int RandomIndexWithArrayRate(int[] arr)
    {
        int idx = 0;
        int rateTotal = 0;
        foreach (int i in arr)
        {
            rateTotal += i;
        }
        int random = UnityEngine.Random.Range(1, rateTotal);
        rateTotal = 0;
        int count = 0;
        foreach (int i in arr)
        {
            rateTotal += i;
            if (random <= rateTotal)
            {
                idx = count;
                return idx;
            }

            count++;
        }

        return idx;
    }
	

	public static RaycastHit2D GetRayCastHit2D()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        return hit;
    }
	public static RaycastHit GetRayCastHit()
	{

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999f))
		{
			//Debug.Log(hit.point);
		}

		return hit;
	}
	public static bool CheckInScreen(Vector3 wpos, Vector3 Mainposition , float extraZone = 0 , bool is3D = true)
    {
		//return false;
		if (is3D)
		{
			Vector3 posMain = Mainposition;
			float distance = Vector3.Distance(wpos, posMain);
			if (distance < 40 + extraZone * 40)
				return true;
			else
				return false;
		}
		else
		{
			Vector3 screenPoint = Camera.main.WorldToViewportPoint(wpos);

			if (screenPoint.x > 0 - extraZone && screenPoint.x < 1 + extraZone && screenPoint.y > 0 - extraZone && screenPoint.y < 1 + extraZone)
				return true;
			else
				return false;
		}
    }
	private void OnDrawGizmos()
	{
		//Vector3 posMain = MapControl.Instance.mainChar.transform.position;

		//Vector3 screenPoint = Camera.main.WorldToViewportPoint(posMain);

		//DebugGUI.Log(screenPoint.ToString());
		//Vector3 posScreen1 = new Vector3(screenPoint.x -2 , screenPoint.y -2 , screenPoint.z);
		//Vector3 posScreen2 = new Vector3(screenPoint.x +2 , screenPoint.y -2 , screenPoint.z);
		//Vector3 posScreen3 = new Vector3(screenPoint.x -2 , screenPoint.y +2 , screenPoint.z);
		//Vector3 posScreen4 = new Vector3(screenPoint.x +2 , screenPoint.y +2 , screenPoint.z);

		//Vector3 posWorld1 = Camera.main.ViewportToWorldPoint(posScreen1);
		//Vector3 posWorld2 = Camera.main.ViewportToWorldPoint(posScreen2);
		//Vector3 posWorld3 = Camera.main.ViewportToWorldPoint(posScreen3);
		//Vector3 posWorld4 = Camera.main.ViewportToWorldPoint(posScreen4);
		////posWorld1.y = 0.5f;
		////posWorld2.y = 0.5f;
		////posWorld3.y = 0.5f;
		////posWorld4.y = 0.5f;
		//Gizmos.color = Color.red;
		//Gizmos.DrawLine(posWorld1, posWorld3);
		//Gizmos.DrawLine(posWorld2, posWorld4);
		//Gizmos.DrawLine(posWorld3, posWorld4);
		//Gizmos.DrawLine(posWorld2, posWorld1);

	}

	public static int[,] RotateMatrixCounterClockwise(int[,] oldMatrix)
    {
        int[,] newMatrix = new int[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;

        }
        return newMatrix;
    }
    public static Quaternion GetRotationByDir(Vector3 dir)
    {
        Vector3 targetAngle = new Vector3(0, 0, -Mathf.Atan2(dir.x, dir.y) * 180 / Mathf.PI);
        Quaternion r = Quaternion.Euler(targetAngle);
        return r;
    }
    public static Quaternion GetRotationByDir3D(Vector3 dir)
    {
        Vector3 targetAngle = new Vector3(0, -Mathf.Atan2(dir.x, dir.z) * 180 / Mathf.PI, 0);
        Quaternion r = Quaternion.Euler(targetAngle);
        return r;
    }
    public static float AngleBetween(Vector3 vector1, Vector3 vector2)
    {
        float sin = vector1.x * vector2.y - vector2.x * vector1.y;
        float cos = vector1.x * vector2.x + vector1.y * vector2.y;
        return Mathf.Abs(Mathf.Atan2(sin, cos) * (180f / (float)Math.PI));
    }
    public IEnumerator TweenValueForTextUI( Text txt, int valueFrom, int valueTo , float duration )
    {
        float time = Time.realtimeSinceStartup;
        int value = valueFrom;
        while (value != valueTo)
        {
            value = (int)Mathf.Lerp(value , valueTo , (Time.realtimeSinceStartup - time)/ duration);
            txt.text = MyUtils.convertNumberToStringWithCommas(value);
            yield return new WaitForEndOfFrame();

        }
    }
    public static void TweenScale(Transform target, float duration , float from , float to , bool pingpong = false)
    {
        Instance.StartCoroutine(Instance.DoTweenScale(target, duration, from, to, pingpong));
    }
    public IEnumerator DoTweenScale(Transform target, float duration, float from, float to, bool pingpong = false)
    {
        Vector3 scaleFrom = Vector3.one*from;
        Vector3 scaleTo = Vector3.one*to;
        target.localScale = scaleFrom;

        float time = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup - time<=duration)
        {
            target.localScale = Vector3.Lerp(scaleFrom, scaleTo, (Time.realtimeSinceStartup - time) / duration);
            yield return new WaitForEndOfFrame();
        }
        target.localScale = scaleTo;
        if ( pingpong)
        {
            time = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - time <= duration)
            {
                target.localScale = Vector3.Lerp(scaleTo , scaleFrom, (Time.realtimeSinceStartup - time) / duration);
                yield return new WaitForEndOfFrame();
            }
        }
    }
	public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
	}
	public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		Vector3 rhs = point - lineStart;
		Vector3 vector2 = lineEnd - lineStart;
		float magnitude = vector2.magnitude;
		Vector3 lhs = vector2;
		if (magnitude > 1E-06f)
		{
			lhs = (Vector3)(lhs / magnitude);
		}
		float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
		return (lineStart + ((Vector3)(lhs * num2)));
	}

	public static void SlowMotion( float timescale , float duration )
	{
		Instance.StartCoroutine ( Instance.doSlowMotion ( timescale , duration ) );
	}

	public IEnumerator doSlowMotion( float timescale , float duration)
	{
//		float tmp = Time.timeScale;
		Time.timeScale = timescale;
		yield return new WaitForSeconds (duration * timescale);
		Time.timeScale = 1;
	}

	public static Color SetAlphaThisColor (Color color , float Alpha)
	{
		color.a = Alpha;
		return color;
	}

	public static void SmoothSlowMotion(float d1, float ts1 , float d2 , float d3 )
	{
		Instance.StartCoroutine(Instance.doSmoothSlowMotion(d1,  ts1,  d2,  d3));
	}
	public IEnumerator doSmoothSlowMotion(float d1, float ts, float d2, float d3)
	{
		
		Time.timeScale = ts;
		yield return new WaitForSeconds(d2 * ts);
		float time = Time.realtimeSinceStartup;
		while ( true)
		{
			if (Time.timeScale == 1)
				yield break;
			Time.timeScale = Mathf.Lerp (ts  , 1, (Time.realtimeSinceStartup - time)/d3) ;
			yield return new WaitForEndOfFrame();
		}
	}
	public static Color ColorSetAlpha ( Color c , float a)
	{
		c.a = a;
		return c;
	}

	public static Vector2 WorldToScreen( Vector3 worldPos , RectTransform canvas )
	{
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
		screenPoint = screenPoint - (canvas.sizeDelta / 2f)* canvas.localScale.x;
		return screenPoint;
	}

	public static bool CheckInternet()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
			return true;
		else
			return false;
	}


	public static string GenerateRandomPLayerName()
	{
		TimeSpan now = new TimeSpan( DateTime.Now.Ticks);
		string nameGen = "Player" +  now.Hours+ now.Minutes + now.Seconds + now.Milliseconds ;
		return nameGen;
	}

    public static T ClassCopy<T>(T other)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
        }
    }

    public static List<int> RandomIndexArray(int length)
	{
        List<int> arr = new List<int>();
		for (int i = 0; i < length; i++)
		{
            arr.Add(i);
        }

        List<int> arrRandom = new List<int>();
		while (arr.Count > 0)
		{
            int index = UnityEngine.Random.Range(0, arr.Count);
            arrRandom.Add(arr[ index]);
            arr.RemoveAt(index);

        }
        return arrRandom;
    }

    public static bool Random50_50()
	{
        return UnityEngine.Random.Range(0, 2) == 0;
	}
}
