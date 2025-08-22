[System.Serializable]
public struct TimeEffectProperty<T>
{
    public T presetValue;
    public T instanceValue;
    public bool isOverridden;

    public TimeEffectProperty(T value)
    {
        presetValue = value;
        instanceValue = value;
        isOverridden = false;
    }

    public void SetPresetValue(T value)
    {
        presetValue = value;

        ////For non overridden values apply preset
        //if (!isOverridden)
        //{
        //    SetInstanceValue(value);
        //}
    }

    public void SetInstanceValue(T value)
    {
        instanceValue = value;
    }

    public void SetOverrideBool(bool value)
    {
        isOverridden = value;
    }
}