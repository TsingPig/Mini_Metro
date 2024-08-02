using UnityEngine;

namespace MVPFrameWork
{
    public static class TransformExtends
    {
        public static T Find<T>(this Transform trans, string path) where T : Component
        {
            T val = null;
            Transform transform = trans?.Find(path);
            return ((object)transform != null) ? transform.GetComponent<T>() : null;
        }

        public static bool Contains(this RectTransform container, RectTransform trans)
        {
            if(trans == null)
            {
                return false;
            }

            Vector3[] array = new Vector3[4];
            container.GetWorldCorners(array);
            float width = Mathf.Abs(array[2].x - array[0].x);
            float height = Mathf.Abs(array[2].y - array[0].y);
            Rect rect = new Rect(array[0].x, array[0].y, width, height);
            Vector3[] array2 = new Vector3[4];
            trans.GetWorldCorners(array2);
            bool result = false;
            foreach(Vector3 point in array2)
            {
                if(rect.Contains(point))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool TotalContains(this RectTransform container, RectTransform trans)
        {
            if(trans == null)
            {
                return false;
            }

            Vector3[] array = new Vector3[4];
            container.GetWorldCorners(array);
            float width = Mathf.Abs(array[2].x - array[0].x);
            float height = Mathf.Abs(array[2].y - array[0].y);
            Rect rect = new Rect(array[0].x, array[0].y, width, height);
            Vector3[] array2 = new Vector3[4];
            trans.GetWorldCorners(array2);
            bool result = true;
            foreach(Vector3 point in array2)
            {
                if(!rect.Contains(point))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}