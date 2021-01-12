/// Credit Tomasz Schelenz 
/// Sourced from - https://bitbucket.org/ddreaper/unity-ui-extensions/issues/81/infinite-scrollrect
/// Demo - https://www.youtube.com/watch?v=uVTV7Udx78k  - configures automatically.  - works in both vertical and horizontal (but not both at the same time)  - drag and drop  - can be initialized by code (in case you populate your scrollview content from code)
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.UI.Extensions
{
    /// <summary>
    /// Infinite scroll view with automatic configuration 
    /// 
    /// Fields
    /// - InitByUSer - in case your scrollrect is populated from code, you can explicitly Initialize the infinite scroll after your scroll is ready
    /// by callin Init() method
    /// 
    /// Notes
    /// - doesn't work in both vertical and horizontal orientation at the same time.
    /// - in order to work it disables layout components and size fitter if present(automatically)
    /// 
    /// </summary>
    [AddComponentMenu("UI/Extensions/UI Infinite Scroll")]
    public class UI_InfiniteScroll : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
    {
        public Button[] buttonList;
        public bool isAutoAlignFollowPoint = false;
        public RectTransform alignPoint;

        public bool isChooseCenter = false;
        public RectTransform centerPoint;
        public RectTransform SelectImage;

        //if true user will need to call Init() method manually (in case the contend of the scrollview is generated from code or requires special initialization)
        [Tooltip("If false, will Init automatically, otherwise you need to call Init() method")]
        public bool InitByUser = false;

        private ScrollRect _scrollRect;
        private ContentSizeFitter _contentSizeFitter;
        private VerticalLayoutGroup _verticalLayoutGroup;
        private HorizontalLayoutGroup _horizontalLayoutGroup;
        private GridLayoutGroup _gridLayoutGroup;
        private bool _isVertical = false;
        private bool _isHorizontal = false;
        private float _disableMarginX = 0;
        private float _disableMarginY = 0;
        private bool _hasDisabledGridComponents = false;
        private List<RectTransform> items = new List<RectTransform>();
        private Vector2 _newAnchoredPosition = Vector2.zero;
        //TO DISABLE FLICKERING OBJECT WHEN SCROLL VIEW IS IDLE IN BETWEEN OBJECTS
        private float _treshold = 100f;
        private int _itemCount = 0;
        private float _recordOffsetX = 0;
        private float _recordOffsetY = 0;

        void Awake()
        {
            if (!InitByUser)
                Init();
            if( isChooseCenter )
                SelectIndex(curIndex);
            if (isAutoAlignFollowPoint)
            {
                if (SelectImage != null)
                {
                    SelectImage.gameObject.SetActive(true);
                    SelectImage.position = items[0].position;
                    SelectImage.transform.parent = _scrollRect.content.transform;
                }
            }
        }

        public void Init()
        {
            if (GetComponent<ScrollRect>() != null)
            {
                for (int i = 0; i < buttonList.Length; i++)
                {
                    if (buttonList[i] != null)
                    {
                        string tmpButtonName = buttonList[i].name;
                        buttonList[i].onClick.AddListener(() => OnClickButton(tmpButtonName));
                    }
                }
                _scrollRect = GetComponent<ScrollRect>();
                _scrollRect.onValueChanged.AddListener(OnScroll);
                _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
                //if (isChooseCenter)
                {
                    for (int i = 0; i < _scrollRect.content.childCount; i++)
                    {
                        RectTransform r = _scrollRect.content.GetChild(i).GetComponent<RectTransform>();
                        items.Add(r);
                        r.GetComponent<Button>().onClick.AddListener(() => OnClickItem(r));
                    }
                }
                if (_scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
                {
                    _verticalLayoutGroup = _scrollRect.content.GetComponent<VerticalLayoutGroup>();
                }
                if (_scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
                {
                    _horizontalLayoutGroup = _scrollRect.content.GetComponent<HorizontalLayoutGroup>();
                }
                if (_scrollRect.content.GetComponent<GridLayoutGroup>() != null)
                {
                    _gridLayoutGroup = _scrollRect.content.GetComponent<GridLayoutGroup>();
                }
                if (_scrollRect.content.GetComponent<ContentSizeFitter>() != null)
                {
                    _contentSizeFitter = _scrollRect.content.GetComponent<ContentSizeFitter>();
                }

                _isHorizontal = _scrollRect.horizontal;
                _isVertical = _scrollRect.vertical;

                if (_isHorizontal && _isVertical)
                {
                    Debug.LogError("UI_InfiniteScroll doesn't support scrolling in both directions, plase choose one direction (horizontal or vertical)");
                }

                _itemCount = _scrollRect.content.childCount;
            }
            else
            {
                Debug.LogError("UI_InfiniteScroll => No ScrollRect component found");
            }
        }

        void DisableGridComponents()
        {
            if (_isVertical)
            {
                _recordOffsetY = items[0].GetComponent<RectTransform>().anchoredPosition.y - items[1].GetComponent<RectTransform>().anchoredPosition.y;
                _disableMarginY = _recordOffsetY * _itemCount / 2;// _scrollRect.GetComponent<RectTransform>().rect.height/2 + items[0].sizeDelta.y;
            }
            if (_isHorizontal)
            {
                _recordOffsetX = items[1].GetComponent<RectTransform>().anchoredPosition.x - items[0].GetComponent<RectTransform>().anchoredPosition.x;
                _disableMarginX = _recordOffsetX * _itemCount / 2;//_scrollRect.GetComponent<RectTransform>().rect.width/2 + items[0].sizeDelta.x;
            }

            if (_verticalLayoutGroup)
            {
                _verticalLayoutGroup.enabled = false;
            }
            if (_horizontalLayoutGroup)
            {
                _horizontalLayoutGroup.enabled = false;
            }
            if (_contentSizeFitter)
            {
                _contentSizeFitter.enabled = false;
            }
            if (_gridLayoutGroup)
            {
                _gridLayoutGroup.enabled = false;
            }
            _hasDisabledGridComponents = true;
        }

        public void OnScroll(Vector2 pos)
        {
            if (!_hasDisabledGridComponents)
                DisableGridComponents();
            for (int i = 0; i < items.Count; i++)
            {
                if (_isHorizontal)
                {
                    if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).x > _disableMarginX + _treshold)
                    {
                        _newAnchoredPosition = items[i].anchoredPosition;
                        _newAnchoredPosition.x -= _itemCount * _recordOffsetX;
                        items[i].anchoredPosition = _newAnchoredPosition;
                        _scrollRect.content.GetChild(_itemCount - 1).transform.SetAsFirstSibling();
                    }
                    else if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).x < -_disableMarginX)
                    {
                        _newAnchoredPosition = items[i].anchoredPosition;
                        _newAnchoredPosition.x += _itemCount * _recordOffsetX;
                        items[i].anchoredPosition = _newAnchoredPosition;
                        _scrollRect.content.GetChild(0).transform.SetAsLastSibling();
                    }
                }

                if (_isVertical)
                {
                    if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).y > _disableMarginY + _treshold)
                    {
                        _newAnchoredPosition = items[i].anchoredPosition;
                        _newAnchoredPosition.y -= _itemCount * _recordOffsetY;
                        items[i].anchoredPosition = _newAnchoredPosition;
                        _scrollRect.content.GetChild(_itemCount - 1).transform.SetAsFirstSibling();
                    }
                    else if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).y < -_disableMarginY)
                    {
                        _newAnchoredPosition = items[i].anchoredPosition;
                        _newAnchoredPosition.y += _itemCount * _recordOffsetY;
                        items[i].anchoredPosition = _newAnchoredPosition;
                        _scrollRect.content.GetChild(0).transform.SetAsLastSibling();
                    }
                }
            }
            if (isChooseCenter)
            {
                float minDistance = 99f;
                int index = -1;
                for (int i = 0; i < items.Count; i++)
                {
                    if (minDistance > Vector3.Distance(items[i].position, centerPoint.position))
                    {
                        index = i;
                        minDistance = Vector3.Distance(items[i].position, centerPoint.position);
                    }
                }
                if (curIndex != index)
                {
                    StartCoroutine(doSelect(items[index]));
                    StartCoroutine(doDeselect(items[curIndex]));
                    curIndex = index;
                }
                if (_scrollRect.velocity.magnitude < 100 && !isMouseDown)
                {
                    _scrollRect.velocity = Vector3.zero;
                    Vector2 wPosOfItem = items[curIndex].transform.position;
                    float deltaX = centerPoint.transform.position.x - wPosOfItem.x;
                    Vector3 wPosOfContainer = _scrollRect.content.transform.position;
                    wPosOfContainer.x += deltaX;
                    _scrollRect.content.transform.position = Vector3.Lerp(_scrollRect.content.transform.position, wPosOfContainer, 0.1f);
                }
            }
            else if( isAutoAlignFollowPoint)
            {
                float minDistance = 9999f;
                int index = -1;
                for (int i = 0; i < items.Count; i++)
                {
                    if (minDistance > Vector3.Distance(items[i].position, centerPoint.position))
                    {
                        index = i;
                        minDistance = Vector3.Distance(items[i].position, centerPoint.position);
                    }
                }
                curIndex = index;
                if (_scrollRect.velocity.magnitude < 100 && !isMouseDown)
                {
                    _scrollRect.velocity = Vector3.zero;
                    Vector2 wPosOfItem = items[index].transform.position;
                    float deltaX = centerPoint.transform.position.x - wPosOfItem.x;
                    Vector3 wPosOfContainer = _scrollRect.content.transform.position;
                    wPosOfContainer.x += deltaX;
                    _scrollRect.content.transform.position = Vector3.Lerp(_scrollRect.content.transform.position, wPosOfContainer, 0.1f);
                }
            }
        }

        float duration_select_animation = 0.2f;
        float scale_select_animation = 1.4f;
        IEnumerator doSelect(RectTransform item)
        {
            float time = Time.realtimeSinceStartup;
            Vector3 scale = Vector3.one* scale_select_animation;
            while (Time.realtimeSinceStartup - time < duration_select_animation)
            {
                item.localScale = Vector3.Lerp(item.localScale, scale, (Time.realtimeSinceStartup - time) / duration_select_animation);
                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator doDeselect(RectTransform item )
        {
            float time = Time.realtimeSinceStartup;
            Vector3 scale = Vector3.one;
            while (Time.realtimeSinceStartup - time < 0.2f)
            {
                item.localScale = Vector3.Lerp(item.localScale, scale, (Time.realtimeSinceStartup - time) / duration_select_animation);
                yield return new WaitForEndOfFrame();
            }
        }

        int curIndex = 0;
        private void Update()
        {
            //if (isChooseCenter)
            //{
            //    if (Input.GetKeyDown(KeyCode.LeftArrow))
            //    {
            //        curIndex--;
            //        if (curIndex < 0)
            //            curIndex = items.Count - 1;
            //        SelectIndex(curIndex, true);
            //    }
            //    if (Input.GetKeyDown(KeyCode.RightArrow))
            //    {
            //        curIndex++;
            //        if (curIndex > items.Count - 1)
            //            curIndex = 0;
            //        SelectIndex(curIndex, true);
            //    }
            //}
        }
        Vector3 despos;
        public void SelectIndex(int index , bool smooth = false)
        {
            if (index >= 0 && index < items.Count)
            {
                Vector2 wPosOfItem = items[index].transform.position;
                float deltaX = centerPoint.transform.position.x - wPosOfItem.x;
                Vector3 wPosOfContainer = _scrollRect.content.transform.position;
                wPosOfContainer.x += deltaX;
                if (!smooth)
                {
                    _scrollRect.content.transform.position = wPosOfContainer;
                    OnScroll(Vector2.zero);
                }
                else
                {
                    despos = wPosOfContainer;
                    StopCoroutine("doStartSelect");
                    StartCoroutine("doStartSelect");
                }
            }
        }
        
        IEnumerator doStartSelect()
        {
            float time = Time.realtimeSinceStartup;
            while ( Time.realtimeSinceStartup - time< 0.3f)
            {
                _scrollRect.content.transform.position = Vector3.Lerp(_scrollRect.content.transform.position, despos, (Time.realtimeSinceStartup - time) / 0.3f);
                yield return new WaitForEndOfFrame();
            }
        }

        void OnClickButton( string name)
        {
            switch(name)
            {
                case "BtnLeft":
                        curIndex--;
                        if (curIndex < 0)
                            curIndex = items.Count - 1;
                        SelectIndex(curIndex, true);
                    break;

                case "BtnRight":
                        curIndex++;
                        if (curIndex > items.Count - 1)
                            curIndex = 0;
                        SelectIndex(curIndex, true);
                    break;
                   
            }

        }
        void OnClickItem( RectTransform item )
        {
            if (isChooseCenter)
            {
                _scrollRect.velocity = Vector3.zero;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == item)
                    {
                        curIndex = i;
                        SelectIndex(curIndex, true);
                        return;
                    }
                }
            }
            else if( isAutoAlignFollowPoint)
            {
                if( SelectImage !=null)
                    SelectImage.position = item.position;
            }
        }
        bool isMouseDown = false;
        public void OnPointerDown(PointerEventData data)
        {
            isMouseDown = true;
            _scrollRect.velocity = Vector3.zero;

        }
        public void OnPointerUp(PointerEventData data)
        {
            isMouseDown = false;
        }
    }
}