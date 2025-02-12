using UnityEditor;

public static class CmsMenuItems
{
    [MenuItem("CMS/Reload")]
    public static void CmsReload()
    {
        Cms.Unload();
        Cms.Init();
    }
}