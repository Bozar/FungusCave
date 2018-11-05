using UnityEngine;

public class ObjectMetaInfo : MonoBehaviour
{
    public MainObjectTag MainTag { get; private set; }
    public SubObjectTag SubTag { get; private set; }

    public void SetMainTag(MainObjectTag tag)
    {
        MainTag = tag;
    }

    public void SetSubTag(SubObjectTag tag)
    {
        SubTag = tag;
    }
}
