namespace AlphaPlus.Auth.Models;

public enum SCResult : int
{
    NONE = 0,
    //PROVIDE_SC_FIRST_TIME,
    PROVIDED_SC,
    PROVIDED_SC_SUCCESS,
    FORCED_CHANGE_SC_WITHOUT_VALIDATION,
    FORCED_CHANGE_SC
}